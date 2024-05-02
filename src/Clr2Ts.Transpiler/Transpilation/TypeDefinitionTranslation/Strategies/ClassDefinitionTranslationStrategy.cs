﻿using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Extensions;
using Clr2Ts.Transpiler.Logging;
using Clr2Ts.Transpiler.Transpilation.Configuration;
using Clr2Ts.Transpiler.Transpilation.DefaultValues;
using Clr2Ts.Transpiler.Transpilation.Templating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Clr2Ts.Transpiler.Transpilation.TypeDefinitionTranslation.Strategies
{
    /// <summary>
    /// Strategy for translating definitions of class types.
    /// </summary>
    public sealed class ClassDefinitionTranslationStrategy: TranslationStrategyBase
    {
        private readonly DefaultValueProvider _defaultValueProvider;
        private readonly TranspilationConfiguration _configuration;

        /// <summary>
        /// Creates a <see cref="ClassDefinitionTranslationStrategy"/>.
        /// </summary>
        /// <param name="configurationSource">Source for the configuration that should be used.</param>
        /// <param name="templatingEngine">Engine to use for loading templates.</param>
        /// <param name="documentationSource">Source for looking up documentation comments for members.</param>
        /// <param name="logger">Logger to use for writing log messages.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="templatingEngine"/> or <paramref name="documentationSource"/> or <paramref name="logger"/> is null.</exception>
        public ClassDefinitionTranslationStrategy(
            IConfigurationSource configurationSource,
            ITemplatingEngine templatingEngine,
            IDocumentationSource documentationSource,
            ILogger logger)
            : base(
                configurationSource,
                templatingEngine,
                documentationSource,
                logger
            )
        {
            _defaultValueProvider = new DefaultValueProvider(configurationSource, logger, TypeReferenceTranslator);
            _configuration = configurationSource.GetSection<TranspilationConfiguration>();
        }

        /// <summary>
        /// Is overridden to define which type definitions can be translated by this strategy.
        /// </summary>
        /// <param name="type">Type definition that should be translated.</param>
        /// <returns>True, if this strategy can be used to translate the specified type definition; otherwise false.</returns>
        protected override bool CanTranslate(Type type)
        {
            return type.IsClass;
        }

        /// <summary>
        /// Is overridden to define how the type definition is translated by this strategy.
        /// </summary>
        /// <param name="type">Type definition that should be tranlated.</param>
        /// <param name="templatingEngine">Templating engine that can be used to fill predefined code snippets.</param>
        /// <returns>Result of the translation.</returns>
        protected override CodeFragment Translate(Type type, ITemplatingEngine templatingEngine)
        {
            var decorators = DecoratorTranslator.GenerateDecorators(type);
            var properties = GeneratePropertyDefinitions(type, templatingEngine);
            var declaration = GenerateClassDeclaration(type);
            var deps = declaration.dependencies.Merge(properties.dependencies).Merge(decorators.Dependencies);

            var code = templatingEngine.UseTemplate(
                "ClassDefinition",
                new Dictionary<string, string> {
                    { "Decorators", decorators.DecoratorCode },
                    { "ClassName", type.GetNameWithGenericTypeParameters() },
                    { "ClassDeclaration", declaration.code },
                    { "Documentation", GenerateDocumentationComment(type) },
                    { "Properties", properties.code.AddIndentation() },
                    { "Dependencies", GenerateDependencyInitialization(deps, templatingEngine) }, {
                        "ConstructorCode", declaration.isDerived
                                               ? $"{Environment.NewLine}super();".AddIndentation(2)
                                               : string.Empty
                    }
                }
            );

            return new CodeFragment(
                CodeFragmentId.ForClrType(type),
                code,
                deps
            );
        }

        private (string code, bool isDerived, CodeDependencies dependencies) GenerateClassDeclaration(Type type)
        {
            var isDerived = false;
            var deps = CodeDependencies.Empty;
            var declarationParts = new List<string>();

            // Type name with generic type parameters (including constraints).
            declarationParts.Add(
                type.GetNameWithGenericTypeParameters(
                    t => {
                        var constraintTranslations = t.GetGenericParameterConstraints()
                                                      .Except(
                                                          new[] {
                                                              typeof(ValueType)
                                                          }
                                                      )
                                                      .Select(TypeReferenceTranslator.Translate);

                        deps = constraintTranslations.Select(x => x.Dependencies).Aggregate(deps, (d, next) => d.Merge(next));
                        return !constraintTranslations.Any()
                                   ? string.Empty
                                   : $" extends {string.Join(" & ", constraintTranslations.Select(x => x.ReferencedTypeName))}";
                    }
                )
            );

            // Extending the base class.
            if (HasBaseType(type)) {
                var baseTypeTranslation = TypeReferenceTranslator.Translate(type.BaseType);
                declarationParts.Add($"extends {baseTypeTranslation.ReferencedTypeName}");
                deps = deps.Merge(baseTypeTranslation.Dependencies);
                isDerived = true;
            }

            // Implementing interfaces.
            var interfaces = type.GetSelfImplementedInterfaces();
            if (interfaces.Any()) {
                var interfaceTranslations = interfaces.Select(TypeReferenceTranslator.Translate);
                declarationParts.Add($"implements {string.Join(", ", interfaceTranslations.Select(t => t.ReferencedTypeName))}");
                deps = interfaceTranslations.Select(t => t.Dependencies).Aggregate(deps, (d, next) => d.Merge(next));
            }

            return (string.Join(" ", declarationParts), isDerived, deps);
        }

        private (string code, CodeDependencies dependencies) GeneratePropertyDefinitions(Type type, ITemplatingEngine templatingEngine)
        {
            var propertyCodeSnippets = new List<string>();
            var deps = CodeDependencies.Empty;

            if (_configuration.InjectTypeHintCondition?.IsMatch(type) ?? false) {
                propertyCodeSnippets.Add(GenerateTypeProperty(type, templatingEngine));
            }

            foreach (var property in GetPropertiesForTranspilation(type)) {
                Logger.WriteInformation($"Translating property {property.Name} on type {type}.");

                var typeReferenceTranslation = TypeReferenceTranslator.Translate(property.PropertyType);
                var decorators = DecoratorTranslator.GenerateDecorators(property);
                deps = deps
                       .Merge(typeReferenceTranslation.Dependencies)
                       .Merge(decorators.Dependencies);

                propertyCodeSnippets.Add(
                    templatingEngine.UseTemplate(
                        "ClassPropertyDefinition",
                        new Dictionary<string, string> {
                            { "Documentation", GenerateDocumentationComment(property) },
                            { "Decorators", decorators.DecoratorCode },
                            { "PropertyModifiers", $"{(IsPropertyOverride(type, property) ? "override " : "")}" },
                            { "PropertyName", GetTypeScriptPropertyName(property) },
                            { "PropertyType", typeReferenceTranslation.ReferencedTypeName },
                            { "Assignment", _defaultValueProvider.Assignment(property) }
                        }
                    )
                );
            }

            var properties = string.Join(Environment.NewLine, propertyCodeSnippets);
            return (properties, deps);
        }

        private bool IsPropertyOverride(Type type, PropertyInfo property)
        {
            return HasBaseType(type)
                && type.BaseType is { IsClass: true }
                && (property == null || property.DeclaringType != type);
        }

        private bool HasBaseType(Type type)
        {
            return !Configuration.FlattenBaseTypes && type.BaseType != typeof(object);
        }

        private string GenerateTypeProperty(Type type, ITemplatingEngine templatingEngine)
        {
            var value = $"{type}, {type.Assembly.GetName().Name}";
            return templatingEngine.UseTemplate(
                "ClassPropertyDefinition",
                new Dictionary<string, string> {
                    { "Documentation", GenerateDocumentationComment("The injected $type reference for JSON-Deserialization") },
                    { "Decorators", "" },
                    { "PropertyModifiers", $"{(IsPropertyOverride(type, null) ? "override " : "")}readonly " },
                    { "PropertyName", "$type" },
                    { "PropertyType", TypeReferenceTranslator.Translate(value.GetType()).ReferencedTypeName },
                    { "Assignment", $@" = ""{value}""" }
                }
            );
        }

        private string GenerateDependencyInitialization(CodeDependencies dependencies, ITemplatingEngine templatingEngine)
        {
            if (!Configuration.RuntimeDependencyLoading)
                return string.Empty;

            var loadableDependencies = dependencies.CodeFragments
                                                   .Where(x => x.TryRecreateClrType(out var type) && !type.IsInterface)
                                                   .Select(x => x.ExportedName)
                                                   .Concat(dependencies.Imports.Select(x => x.Name));

            return templatingEngine.UseTemplate(
                "DependencyInitialization",
                new Dictionary<string, string> { { "Dependencies", string.Join(", ", loadableDependencies) } }
            );
        }

        private IEnumerable<PropertyInfo> GetPropertiesForTranspilation(Type type)
        {
            return Configuration.FlattenBaseTypes
                       ? type.GetProperties()
                       : type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        private string GetTypeScriptPropertyName(PropertyInfo property)
        {
            return Configuration.CamelCase
                       ? property.Name.ToCamelCase()
                       : property.Name;
        }
    }
}
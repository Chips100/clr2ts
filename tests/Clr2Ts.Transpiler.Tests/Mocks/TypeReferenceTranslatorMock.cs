using Clr2Ts.Transpiler.Transpilation;
using Clr2Ts.Transpiler.Transpilation.TypeReferenceTranslation;
using System;
using System.Collections.Generic;

namespace Clr2Ts.Transpiler.Tests.Mocks
{
    /// <summary>
    /// Mock implementation of <see cref="ITypeReferenceTranslator"/> that allows
    /// specifying fixed values for translations of types.
    /// </summary>
    public sealed class TypeReferenceTranslatorMock : ITypeReferenceTranslator
    {
        private readonly IDictionary<Type, TypeReferenceTranslationResult> _translations
            = new Dictionary<Type, TypeReferenceTranslationResult>
            {
                { typeof(bool), new TypeReferenceTranslationResult("bool", CodeDependencies.Empty) },
                { typeof(int), new TypeReferenceTranslationResult("number", CodeDependencies.Empty) },
            };

        /// <summary>
        /// Translates the specified type reference.
        /// </summary>
        /// <param name="referencedType">Type reference that should be tranlated.</param>
        /// <returns>Result of the translation.</returns>
        public TypeReferenceTranslationResult Translate(Type referencedType)
        {
            if (_translations.TryGetValue(referencedType, out var translation)) return translation;

            throw new InvalidOperationException($"Unexpected translation of reference to type {referencedType}.");
        }

        /// <summary>
        /// Configures this mock to translate references to the specified type with a fixed translation.
        /// </summary>
        /// <typeparam name="T">Type for which references will be translated.</typeparam>
        /// <param name="translation">Translation that will be used for references to the specified type.</param>
        /// <returns>The current mock itself (fluent interface).</returns>
        public TypeReferenceTranslatorMock UseTranslation<T>(TypeReferenceTranslationResult translation)
        {
            _translations[typeof(T)] = translation ?? throw new ArgumentNullException(nameof(translation));
            return this;
        }
    }
}
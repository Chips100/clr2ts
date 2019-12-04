using System;

namespace Clr2Ts.Transpiler.Transpilation.Decorators
{
    /// <summary>
    /// Represents a piece of TypeScript code that contains decorators.
    /// </summary>
    public sealed class DecoratorTranslationResult
    {
        /// <summary>
        /// Creates a DecoratorTranslationResult.
        /// </summary>
        /// <param name="decoratorCode">TypeScript code that contains the decorators.</param>
        /// <param name="dependencies">Dependencies that are required for the decorators.</param>
        public DecoratorTranslationResult(string decoratorCode, CodeDependencies dependencies)
        {
            DecoratorCode = decoratorCode;
            Dependencies = dependencies ?? throw new ArgumentNullException(nameof(dependencies));
        }

        /// <summary>
        /// Gets the TypeScript code that contains the decorators.
        /// </summary>
        public string DecoratorCode { get; }

        /// <summary>
        /// Gets the dependencies that are required for the decorators.
        /// </summary>
        public CodeDependencies Dependencies { get; }

        /// <summary>
        /// Gets an empty result with no decorators that can be used as a seed when aggregating multiple decorators.
        /// </summary>
        public static DecoratorTranslationResult Empty { get; }
            = new DecoratorTranslationResult(string.Empty, CodeDependencies.Empty);

        /// <summary>
        /// Merges another set of decorators into the current set of decorators.
        /// </summary>
        /// <param name="other">Other decorators that should be merged into the current set.</param>
        /// <returns>A set of decorators from both sets.</returns>
        public DecoratorTranslationResult Merge(DecoratorTranslationResult other)
            => new DecoratorTranslationResult(
                DecoratorCode + other.DecoratorCode,
                Dependencies.Merge(other.Dependencies));
    }
}

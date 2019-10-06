using System;

namespace Clr2Ts.Transpiler.Transpilation
{
    /// <summary>
    /// Represents an import of a TypeScript artifact that is defined
    /// outside of the transpiled codebase.
    /// </summary>
    public sealed class Import
    {
        /// <summary>
        /// Creates an import referencing a TypeScript artifact
        /// defined outside of the transpiled codebase.
        /// </summary>
        /// <param name="name">Name of the TypeScript artifact that is imported.</param>
        /// <param name="importSource">Source from which the TypeScript artifact is being imported (e.g. "@scope/lib" for library code).</param>
        public Import(string name, string importSource)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));
            if (string.IsNullOrWhiteSpace(importSource)) throw new ArgumentException("ImportSource cannot be empty.", nameof(importSource));

            Name = name;
            ImportSource = importSource;
        }

        /// <summary>
        /// Gets the name of the TypeScript artifact that is imported.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the source from which the TypeScript artifact is being imported (e.g. "@scope/lib" for library code).
        /// </summary>
        public string ImportSource { get; }


        public override string ToString() => $"{ Name } from { ImportSource }";
        public override bool Equals(object obj) => ToString().Equals((obj as Import)?.ToString());
        public override int GetHashCode() => ToString().GetHashCode();

        public static bool operator ==(Import a, Import b)
            => a?.Equals(b) ?? b == null;

        public static bool operator !=(Import a, Import b)
            => !(a == b);
    }
}
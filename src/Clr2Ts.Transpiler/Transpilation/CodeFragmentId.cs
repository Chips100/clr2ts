using System;

namespace Clr2Ts.Transpiler.Transpilation
{
    /// <summary>
    /// Represents an ID that is used to reference code fragments.
    /// </summary>
    public sealed class CodeFragmentId
    {
        /// <summary>
        /// Creates a <see cref="CodeFragmentId"/>.
        /// </summary>
        /// <param name="name">Name that is used for identifying the code fragment.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or empty.</exception>
        public CodeFragmentId(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));

            Name = name;
        }

        /// <summary>
        /// Gets the name that is used for identifying the code fragment.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Derives the ID for the code fragment that defines the specified type.
        /// </summary>
        /// <param name="type">Type that is defined in the referenced code fragment.</param>
        /// <returns>The ID for the code fragment that defines the specified type.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="type"/> is null.</exception>
        public static CodeFragmentId ForClrType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            // Use the full name (namespace + type name) of the type for referencing the code fragment.
            return new CodeFragmentId(type.FullName);
        }

        public override bool Equals(object obj) => Name.Equals((obj as CodeFragmentId)?.Name);
        public override int GetHashCode() => Name.GetHashCode();
    }
}
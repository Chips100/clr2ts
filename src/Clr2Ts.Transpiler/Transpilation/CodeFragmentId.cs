using System;

namespace Clr2Ts.Transpiler.Transpilation
{
    /// <summary>
    /// Represents an ID that is used to reference code fragments.
    /// </summary>
    public sealed class CodeFragmentId
    {
        // To retreive the type information later, keep a reference.
        // The public signature explicitly hints that this is not the case;
        // the reference might by eliminated later (i.e. for serialization purposes).
        private readonly Type _clrType;

        /// <summary>
        /// Creates a <see cref="CodeFragmentId"/>.
        /// </summary>
        /// <param name="name">Name that is used for identifying the code fragment.</param>
        /// <param name="clrType">Type information, if this identifier represents a type; otherwise false.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or empty.</exception>
        private CodeFragmentId(string name, Type clrType)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));

            Name = name;
            _clrType = clrType;
        }

        /// <summary>
        /// Gets the name that is used for identifying the code fragment.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Tries to recreate the type information that is represented by this identifier.
        /// </summary>
        /// <param name="type">Type information of the type that is represented by this identifier; or null if it does not represent a type.</param>
        /// <returns>True, if the original type information could be recreated; otherwise false.</returns>
        public bool TryRecreateClrType(out Type type)
        {
            type = _clrType;
            return type != null;
        }

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
            return new CodeFragmentId(type.FullName, type);
        }

        public override string ToString() => Name;
        public override bool Equals(object obj) => Name.Equals((obj as CodeFragmentId)?.Name);
        public override int GetHashCode() => Name.GetHashCode();
    }
}
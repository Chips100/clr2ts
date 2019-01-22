using System;

namespace Clr2Ts.Transpiler.Transpilation
{
    public class CodeFragmentId
    {
        public CodeFragmentId(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty.", nameof(name));

            Name = name;
        }

        public string Name { get; }

        public static CodeFragmentId ForClrType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return new CodeFragmentId(type.FullName);
        }
    }
}
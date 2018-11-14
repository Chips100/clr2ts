using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clr2Ts.Transpiler.TypeSystemModel
{
    public sealed class ClrTypeInfo
    {
        public ClrTypeInfo(ClrTypeIdentifier identifier, string name, string @namespace, string summary)
        {
            Identifier = identifier;
            Name = name;
            Namespace = @namespace;
            Summary = summary;
        }

        public ClrTypeIdentifier Identifier { get; }

        public string Name { get; }

        public string Namespace { get; }

        public string Summary { get; }

        public override string ToString() => $"{Name} ({Namespace})";

        public static ClrTypeInfo FromType(Type type, string summary) =>
            new ClrTypeInfo(ClrTypeIdentifier.CreateNew(), type.Name, type.Namespace, summary);
    }
}
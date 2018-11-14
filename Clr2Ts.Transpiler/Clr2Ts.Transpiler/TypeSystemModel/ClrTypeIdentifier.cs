using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clr2Ts.Transpiler.TypeSystemModel
{
    public class ClrTypeIdentifier
    {
        public ClrTypeIdentifier(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            Id = id;
        }

        public Guid Id { get; }

        public static ClrTypeIdentifier CreateNew() => new ClrTypeIdentifier(Guid.NewGuid());

        public override bool Equals(object obj)
        {
            if (obj is ClrTypeIdentifier identifier) return identifier.Id == Id;

            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
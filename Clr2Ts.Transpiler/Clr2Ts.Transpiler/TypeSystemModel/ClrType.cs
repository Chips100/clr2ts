using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clr2Ts.Transpiler.TypeSystemModel
{
    public abstract class ClrType
    {
        protected ClrType(ClrTypeInfo typeInfo)
        {
            TypeInfo = typeInfo ?? throw new ArgumentNullException(nameof(typeInfo));
        }

        public ClrTypeInfo TypeInfo { get; }

        public override string ToString() => TypeInfo.ToString();

        public static ClrType FromType(Type type)
        {
            var typeInfo = ClrTypeInfo.FromType(type, "");
            
            // Work in Progress.
            if (type.IsClass)
            {
                return new ClrClass(typeInfo);
            }

            if (type.IsEnum)
            {
                return new ClrEnum(typeInfo);
            }

            // Exception?
            return null;
        }
    }
}
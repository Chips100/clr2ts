using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clr2Ts.Transpiler.TypeSystemModel
{
    public sealed class ClrEnum : ClrType
    {
        public ClrEnum(ClrTypeInfo typeInfo) : base(typeInfo)
        { }
    }
}
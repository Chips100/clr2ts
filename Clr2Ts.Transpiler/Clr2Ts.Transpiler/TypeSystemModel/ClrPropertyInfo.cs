using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Clr2Ts.Transpiler.TypeSystemModel
{
    public sealed class ClrPropertyInfo
    {
        public ClrPropertyInfo(string summary)
        {
            Summary = summary;
        }
        

        public string Summary { get; }
    }
}
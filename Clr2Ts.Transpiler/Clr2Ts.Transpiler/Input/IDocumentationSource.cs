using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Clr2Ts.Transpiler.Input
{
    public interface IDocumentationSource
    {
        string GetDocumentationText(MemberInfo member);
    }
}
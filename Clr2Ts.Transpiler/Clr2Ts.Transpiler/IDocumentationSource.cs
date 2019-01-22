using System.Reflection;

namespace Clr2Ts.Transpiler
{
    public interface IDocumentationSource
    {
        string GetDocumentationText(MemberInfo member);
    }
}
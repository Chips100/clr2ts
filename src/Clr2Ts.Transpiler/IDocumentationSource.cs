using System.Reflection;

namespace Clr2Ts.Transpiler
{
    /// <summary>
    /// Allows lookup of documentation comments for members defined in an assembly.
    /// </summary>
    public interface IDocumentationSource
    {
        /// <summary>
        /// Gets the documentation text for the specified member.
        /// </summary>
        /// <param name="member">Member for which the documentation text should be looked up.</param>
        /// <returns>A string with the documentation text; or null if it could not be found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="member"/> is null.</exception>
        string GetDocumentationText(MemberInfo member);
    }
}
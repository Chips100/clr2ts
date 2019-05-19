using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Clr2Ts.Transpiler.Input
{
    /// <summary>
    /// Allows lookup of documentation comments of an assembly from an XML source.
    /// </summary>
    /// <remarks>
    /// The XML file is expected to be located in the same directory 
    /// as the assembly file with the same name, but an .xml file extension.
    /// </remarks>
    public sealed class AssemblyXmlDocumentationSource: IDocumentationSource
    {
        /// <summary>
        /// Gets the documentation text for the specified member.
        /// </summary>
        /// <param name="member">Member for which the documentation text should be looked up.</param>
        /// <returns>A string with the documentation text; or null if it could not be found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="member"/> is null.</exception>
        public string GetDocumentationText(MemberInfo member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));
            
            // Look for the XML documentation file.
            var xmlFile = Path.ChangeExtension(member.Module.Assembly.Location, "xml");
            if (!File.Exists(xmlFile)) return null;

            // Search for the specified member in the XML file.
            // TODO: Keep loaded XML files in memory.
            return XDocument.Load(xmlFile).Descendants("member")
                .FirstOrDefault(d => d.Attribute("name").Value == GetXmlName(member))
                ?.Descendants("summary").FirstOrDefault()?.Value.ToString().Trim();
        }

        private string GetXmlName(MemberInfo member)
        {
            if (member is Type type)
            {
                return $"T:{type.FullName}";
            }
            else if (member is PropertyInfo property)
            {
                return $"P:{property.DeclaringType.FullName}.{property.Name}";
            }
            else if (member is FieldInfo field)
            {
                return $"F:{field.DeclaringType.FullName}.{field.Name}";
            }

            throw new ArgumentOutOfRangeException($"Unsupported MemberInfo for documentation: {member.GetType()}");
        }
    }
}
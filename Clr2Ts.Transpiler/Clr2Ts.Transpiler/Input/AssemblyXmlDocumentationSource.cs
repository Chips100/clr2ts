using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Clr2Ts.Transpiler.Input
{
    public class AssemblyXmlDocumentationSource: IDocumentationSource
    {
        public string GetDocumentationText(MemberInfo member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            var assembly = member.Module.Assembly;
            var xmlFile = Path.ChangeExtension(assembly.Location, "xml");
            if (!File.Exists(xmlFile))
            {
                return null;
            }

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

            throw new ArgumentOutOfRangeException($"Unsupported MemberInfo for documentation: {member.GetType()}");
        }
    }
}

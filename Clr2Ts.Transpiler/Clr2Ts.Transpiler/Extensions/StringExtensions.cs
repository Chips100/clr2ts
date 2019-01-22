using System.IO;
using System.Linq;
using System.Text;

namespace Clr2Ts.Transpiler.Extensions
{
    public static class StringExtensions
    {
        public static string AddIndentation(this string input, int level = 1)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(input)))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                var indentation = new string(Enumerable.Repeat('\t', level).ToArray());
                var builder = new StringBuilder();
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    builder.AppendLine($"{indentation}{line}");
                }

                return builder.ToString();
            }
        }
    }
}
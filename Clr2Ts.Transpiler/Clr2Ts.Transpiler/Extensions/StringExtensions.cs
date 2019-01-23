using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Clr2Ts.Transpiler.Extensions
{
    /// <summary>
    /// Defines extension methods for instances of <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Indents all lines of the specified string by the specified number of tabs.
        /// </summary>
        /// <param name="input">String with the lines that should be indented.</param>
        /// <param name="level">Number of tabs by which the lines should be indented (defaults to 1).</param>
        /// <returns>The indented string.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="level"/> is negative.</exception>
        public static string AddIndentation(this string input, int level = 1)
        {
            if (level < 0) throw new ArgumentOutOfRangeException(nameof(level), "level for indentation cannot be negative.");

            // Quick exit for trivial cases.
            if (string.IsNullOrWhiteSpace(input) || level == 0) return input;

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
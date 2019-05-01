using System;
using System.Linq;

namespace Clr2Ts.Transpiler.Extensions
{
    /// <summary>
    /// Defines extension methods for instances of <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a name from PascalCase to camelCase.
        /// </summary>
        /// <param name="input">Input in PascalCase.</param>
        /// <returns>The name in camelCase.</returns>
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // Just convert the first character to lower case.
            return input.Substring(0, 1).ToLower() + input.Substring(1);
        }
        
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

            // Early exit for trivial cases (and normalization to empty string).
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            if (level == 0) return input;

            // Split by NewLine and put the indented lines back together.
            var indentation = new string(Enumerable.Repeat('\t', level).ToArray());
            return string.Join(Environment.NewLine, input
                .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                .Select(line => $"{indentation}{line}".TrimEnd()));
        }
    }
}
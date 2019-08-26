using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// Uses the current string as a template with placeholders to
        /// be filled with values from the specified context.
        /// </summary>
        /// <param name="template">String that is used as the template.</param>
        /// <param name="context">Context that holds the values to be inserted into the template.</param>
        /// <returns>The template string filled with values from the context.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is null.</exception>
        public static string FormatWith(this string template, IDictionary<string, object> context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            // Early exit for empty string.
            if (string.IsNullOrWhiteSpace(template)) return string.Empty;

            // Regex that finds placeholders enclosed by curly braces,
            // e.g. "Hello, { ContextValue.Name }!". Respects escaping of opening curly braces.
            return new Regex(@"(?<!\\){[^}]*}")
                // Replace placeholders in template string
                // by inserting values from the context.
                .Replace(template, match => EvaluateTemplateContext(match.Value, context))

                // Unescape characters that have been escaped
                // to not be matched by the templating process.
                .Replace(@"\{", "{");
        }

        private static string EvaluateTemplateContext(string path, IDictionary<string, object> context)
        {
            var cleanPath = path.Trim('{', '}').Replace(" ", string.Empty);
            var pathElements = new Queue<string>(cleanPath.Split('.'));

            // Follow the path in the object's properties.
            context.TryGetValue(pathElements.Dequeue(), out var obj);
            while (obj != null && pathElements.Any())
            {
                obj = obj.GetType().GetProperty(pathElements.Dequeue()).GetValue(obj);
            }

            return obj?.ToString();
        }
    }
}
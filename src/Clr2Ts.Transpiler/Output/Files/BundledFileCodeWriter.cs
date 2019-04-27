using Clr2Ts.Transpiler.Transpilation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Clr2Ts.Transpiler.Output.Files
{
    /// <summary>
    /// CodeWriter that bundles and writes all code fragments into a single file.
    /// </summary>
    public sealed class BundledFileCodeWriter: ICodeWriter
    {
        private readonly string _file;

        /// <summary>
        /// Creates a <see cref="BundledFileCodeWriter"/>.
        /// </summary>
        /// <param name="file">Name of the file to which the code fragments should be written.</param>
        public BundledFileCodeWriter(string file)
        {
            if (string.IsNullOrWhiteSpace(file)) throw new ArgumentNullException("file cannot be empty.");

            _file = file;
        }

        /// <summary>
        /// Writes the specified set of code fragments to the output.
        /// </summary>
        /// <param name="codeFragments">Code fragments that should be written.</param>
        public void Write(IEnumerable<CodeFragment> codeFragments)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_file));

            using (var writer = new StreamWriter(_file, false, Encoding.UTF8))
            {
                foreach (var fragment in codeFragments)
                {
                    writer.Write(fragment.Code);
                    writer.WriteLine();
                    writer.WriteLine();
                }
            }
        }
    }
}
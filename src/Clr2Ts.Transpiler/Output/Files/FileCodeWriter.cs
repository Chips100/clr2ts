using Clr2Ts.Transpiler.Transpilation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Clr2Ts.Transpiler.Output.Files
{
    /// <summary>
    /// CodeWriter that writes the code fragments into individual files.
    /// </summary>
    public sealed class FileCodeWriter : ICodeWriter
    {
        private const char CodeFragmentIdSeparator = '.';
        private readonly string _directory;
        private readonly bool _mimicNamespacesWithSubdirectories;

        /// <summary>
        /// Creates a <see cref="FileCodeWriter"/>.
        /// </summary>
        /// <param name="directory">Directory, in which the files should be created.</param>
        /// <param name="mimicNamespacesWithSubdirectories">If set to true, files will be placed into subdirectories that correspond to the namespaces.</param>
        public FileCodeWriter(string directory, bool mimicNamespacesWithSubdirectories)
        {
            _directory = directory;
            _mimicNamespacesWithSubdirectories = mimicNamespacesWithSubdirectories;
        }

        /// <summary>
        /// Writes the specified set of code fragments to the output.
        /// </summary>
        /// <param name="codeFragments">Code fragments that should be written.</param>
        public void Write(IEnumerable<CodeFragment> codeFragments)
        {
            var writtenFiles = new HashSet<string>();
            foreach (var fragment in codeFragments)
            {
                writtenFiles.Add(WriteCodeFragment(fragment).FullName);
            }

            // Clean the target directory from trailing files.
            var existingFiles = Directory.EnumerateFiles(_directory, "*.ts", SearchOption.AllDirectories);
            foreach (var file in existingFiles.Where(f => !writtenFiles.Contains(new FileInfo(f).FullName)))
            {
                File.Delete(file);
            }
        }

        /// <summary>
        /// Writes a single code fragment to the corresponding file.
        /// </summary>
        /// <param name="codeFragment">The code fragment that should be written.</param>
        private FileInfo WriteCodeFragment(CodeFragment codeFragment)
        {
            var file = GetFileNameFor(codeFragment.Id);
            Directory.CreateDirectory(Path.GetDirectoryName(file));

            using (var writer = new StreamWriter(file, false, Encoding.UTF8))
            {
                // Import artifacts defined outside of the transpiled codebase.
                foreach (var import in codeFragment.Dependencies.Imports)
                {
                    writer.WriteLine(GetImportFor(import));
                }

                // Import other transpiled code fragments that this fragment depends on.
                foreach (var dependency in codeFragment.Dependencies.CodeFragments)
                {
                    writer.WriteLine(GetImportFor(codeFragment.Id, dependency));
                }

                writer.WriteLine();
                writer.Write(codeFragment.Code);
            }

            return new FileInfo(file);
        }

        /// <summary>
        /// Gets the name of the file that the code fragment should be written to.
        /// </summary>
        /// <param name="codeFragmentId">ID of the code fragment that should be written.</param>
        /// <returns>The name of the file that the code fragment should be written to.</returns>
        private string GetFileNameFor(CodeFragmentId codeFragmentId)
        {
            var parts = codeFragmentId.Name.Split(CodeFragmentIdSeparator);
            var directories = new List<string> { _directory };
            var fileName = $"{parts.Last()}.ts";

            if (_mimicNamespacesWithSubdirectories)
            {
                directories.AddRange(parts.Take(parts.Length - 1));
            }

            return Path.Combine(directories.Concat(new[] { fileName }).ToArray());
        }

        /// <summary>
        /// Gets an import statement that adds a reference to another code fragment.
        /// </summary>
        /// <param name="self">The current code fragment that should reference another.</param>
        /// <param name="codeFragmentId">The other code fragment that should be imported.</param>
        /// <returns>The import statement that adds a reference to the other code fragment.</returns>
        private string GetImportFor(CodeFragmentId self, CodeFragmentId codeFragmentId)
        {
            var parts = codeFragmentId.Name.Split(CodeFragmentIdSeparator);
            var subdirectory = string.Empty;

            if (_mimicNamespacesWithSubdirectories)
            {
                // Take own nesting into account; walking up those directories first.
                var selfDepth = self.Name.Split(CodeFragmentIdSeparator).Length - 1;
                var up = string.Join(string.Empty, Enumerable.Repeat("../", selfDepth));

                subdirectory = up + string.Join(string.Empty,
                    parts.Take(parts.Length - 1).Select(p => $"{p}/"));
            }

            return $"import {{ {codeFragmentId.ExportedName} }} from './{subdirectory}{ parts.Last() }';";
        }

        private string GetImportFor(Import import)
            => $"import {{ {import.Name} }} from '{import.ImportSource}';";
    }
}
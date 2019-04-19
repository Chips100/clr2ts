using Clr2Ts.Transpiler.Configuration;
using Clr2Ts.Transpiler.Input;
using Clr2Ts.Transpiler.Transpilation.Templating;
using Clr2Ts.Transpiler.Transpilation.TypeScript;
using System;
using System.IO;
using System.Linq;

namespace Clr2Ts.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configFile = args[0];
            var configuration = ConfigurationRoot.FromJsonFile(configFile);

            var assemblyFiles = configuration.Input.AssemblyFiles
                .Select(f => new FileInfo(f).FullName);

            // Process each assembly in seperate AppDomain? AppDomainContext had to be dropped for .NET Standard 2.0
            var assemblyScanner = new AssemblyScanner();
            var types = assemblyFiles.SelectMany(af => assemblyScanner.GetTypesForTranspilation(af));

            var transpiler = new TypeScriptTranspiler(
                EmbeddedResourceTemplatingEngine.ForTypeScript(), 
                new AssemblyXmlDocumentationSource());

            var result = transpiler.Transpile(types);

            foreach (var fragment in result.CodeFragments)
            {
                Console.WriteLine(fragment.Id.Name);
                foreach (var dependency in fragment.Dependencies) Console.WriteLine(dependency.Name);
                Console.WriteLine(fragment.Code);

                File.WriteAllText(@"C:\temp\" + fragment.Id.Name + ".ts", fragment.Code);
            }

            Console.ReadLine();
        }
    }
}
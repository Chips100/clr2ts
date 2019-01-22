using Clr2Ts.Transpiler.Input;
using Clr2Ts.Transpiler.Transpilation.TypeScript;
using System;
using System.IO;

namespace Clr2Ts.Transpiler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var assemblyScanner = new AssemblyScanner();
            var sampleAssemblyName = "Clr2Ts.Transpiler.Tests.SampleAssembly";
            var file = new FileInfo(Path.Combine("..", "..", "..", sampleAssemblyName, "bin", sampleAssemblyName + ".dll"));
            var types = assemblyScanner.GetTypesForTranspilation(file.FullName);

            var transpiler = new TypeScriptTranspiler(new EmbeddedResourceTemplatingEngine(), new AssemblyXmlDocumentationSource());
            var result = transpiler.Transpile(types);

            foreach(var fragment in result.CodeFragments)
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
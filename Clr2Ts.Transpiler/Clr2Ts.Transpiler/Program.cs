using Clr2Ts.Transpiler.Transpilation.TypeScript;
using System;

namespace Clr2Ts.Transpiler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var transpiler = new TypeScriptTranspiler();
            var fragment = transpiler.Transpile(typeof(SomeModel));

            Console.WriteLine(fragment.Id.Name);
            foreach (var dependency in fragment.Dependencies) Console.WriteLine(dependency.Name);
            Console.WriteLine(fragment.Code);
            Console.ReadLine();
        }

    }

    public class SomeModel
    {
        public string SomeString { get; set; }

        public int SomeInt { get; set; }
    }
}
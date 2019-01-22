using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace Clr2Ts.Transpiler.Configuration
{
    public class ConfigurationRoot
    {
        public ConfigurationRoot(InputConfiguration input)
        {
            Input = input;
        }

        public InputConfiguration Input { get; }

        public static ConfigurationRoot ReadFromJsonFile(string file)
        {
            return JsonConvert.DeserializeObject<ConfigurationRoot>(
                File.ReadAllText(file, Encoding.UTF8));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTPad.Model.Intellisence
{
    public static class Snipet
    {
        public static readonly string SNIPET_DIR = "snipets";

        public static Dictionary<string, string> SnipetDictionary { get; } = new Dictionary<string, string>();

        static Snipet()
        {
            foreach (var file in Directory.GetFiles(SNIPET_DIR))
            {
                string key = Path.GetFileNameWithoutExtension(file);
                string value = File.ReadAllText(file);

                SnipetDictionary.Add(key, value);
            }
        }
    }
}

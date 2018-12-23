using CsTxt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSTPad.Model
{
    public static class Compiler
    {
        public static CSharpText CSharpTextCompiler { get; } = new CSharpText(string.Empty);

        public static async Task<(string Result, string CSharp)> Compile(string text)
        {
            CSharpTextCompiler.Content = text;

            string csharp = string.Empty;
            try
            {
                csharp = await CSharpTextCompiler.CompileToCSharpAsync();
                text = await CSharpTextCompiler.RunAsync();
            }
            catch (Exception ex)
            {
                csharp = ex.Message + "\r\n" + ex.StackTrace;
                text = string.Empty;
            }

            return (text, csharp);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsTxt.Block
{
    public static class Extension
    {
        public static string ToCSharp(this IEnumerable<IBlock> blocks)
        {
            var usings = string.Join("\r\n", blocks.Where(x => x is UsingBlock).Select(x => x.ToCSharpCode()).ToArray());
            var scripts = string.Join("\r\n", blocks.Where(x => !(x is UsingBlock)).Select(x => x.ToCSharpCode()).ToArray());

            return string.Join("\r\n", new string[] { usings, scripts, });
        }

        public static async Task<string> ToCSharpAsync(this IEnumerable<IBlock> blocks)
        {

            string result = await Task.Run<string>(() => blocks.ToCSharp());

            return result;
        }
    }
}

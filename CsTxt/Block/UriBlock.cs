using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CsTxt.Block
{
    public class UriBlock : BlockBase, IBlock
    {
        private static readonly string REGEX = @"@\([^\)]*\)";

        public override BlockType Type => BlockType.Uri;

        public override string Content { get; protected set; }
        
        public static IBlock Parse(string text)
        {
            if (Regex.IsMatch(text, REGEX))
            {
                var result = Regex.Match(text, REGEX).Value;

                return new UriBlock
                {
                    Content = result,
                };
            }

            return Null;
        }

        public override string ToCSharpCode()
        {
            string path = Content.TrimStart('@').TrimStart('(').TrimEnd(')').Replace("\\", "\\\\").Replace("\"", "\\\"");

            if (File.Exists(path))
            {
                return $"Out.Append(System.IO.File.ReadAllText(\"{path}\"));\r\n";
            }
            else
            {
                return $"Out.Append(\"<CSTextError: [{path}]は存在しません>\");\r\n";
            }
        }
    }
}

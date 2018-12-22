using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CsTxt.Block
{
    public class ScriptBlock : BlockBase, IBlock
    {
        private static readonly string REGEX_INNER_TEXT = "( .*)#(.*)$";
        private static readonly string START_WITH_TOKEN = "@{";
        private static readonly string INVALID_NEAR_TOKEN = "@{=";

        private ScriptBlock() { }

        public override BlockType Type { get; } = BlockType.Script;
        
        public override string Content { get; protected set; }
        
        public static IBlock Parse(string text)
        {
            int index = 0;
            StringBuilder sb = new StringBuilder();

            if (text.Length < START_WITH_TOKEN.Length)
            {
                return Null;
            }
            else if (text.StartsWith(START_WITH_TOKEN) && !text.StartsWith(INVALID_NEAR_TOKEN))
            {
                sb.Append(START_WITH_TOKEN);

                index += START_WITH_TOKEN.Length;
            }
            else
            {
                return Null;
            }

            int nest = 0;
            for (bool isFindExitBlock = false; !isFindExitBlock; index++)
            {
                if ('{' == text[index])
                {
                    sb.Append(text[index]);
                    nest++;
                }
                else if ('}' == text[index])
                {
                    if (0 < nest)
                    {
                        sb.Append(text[index]);
                        nest--;
                    }
                    else
                    {
                        sb.Append(text[index]);

                        isFindExitBlock = true;
                    }
                }
                else
                {
                    sb.Append(text[index]);
                }
            }
            
            string result = sb.ToString();

            return new ScriptBlock
            {
                Content = result,
            };
        }

        public override string ToCSharpCode()
        {
            var content = Content.TrimStart('@').TrimStart('{').TrimEnd('}');
            StringBuilder sb = new StringBuilder(content);

            foreach (Match m in Regex.Matches(content, REGEX_INNER_TEXT, RegexOptions.Multiline))
            {
                string key = m.Groups[0].Value;
                string indent = m.Groups[1].Value;
                string csharp =
                    indent + BlockFactory.Parse(m.Groups[2].Value).ToCSharp().Replace("\r\n", $"\r\n{indent}");

                sb.Replace(key, csharp);
            }

            return sb.ToString();
        }
    }
}

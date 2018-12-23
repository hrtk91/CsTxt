using System;
using System.Collections.Generic;
using System.Text;

namespace CsTxt.Block
{
    public class CalcBlock : BlockBase, IBlock
    {
        private static readonly string START_WITH_TOKEN = "@{=";

        private CalcBlock() { }

        public override BlockType Type { get; } = BlockType.Calc;

        public override string Content { get; protected set; }

        public static IBlock Parse(string text)
        {
            int index = 0;
            StringBuilder sb = new StringBuilder();

            if (text.Length < START_WITH_TOKEN.Length)
            {
                return Null;
            }
            else if (text.StartsWith(START_WITH_TOKEN))
            {
                sb.Append(text.Substring(index, START_WITH_TOKEN.Length));
                index += START_WITH_TOKEN.Length;
            }
            else
            {
                return Null;
            }

            int nest = 0;
            for (bool isFindExitBlock = false;index < text.Length && !isFindExitBlock; index++)
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

            return new CalcBlock
            {
                Content = result,
            };
        }

        public override string ToCSharpCode()
        {
            string calc = Content.TrimStart('@').TrimStart('{').TrimStart('=').TrimEnd('}');

            return $"Out.Append({calc});";
        }
    }
}

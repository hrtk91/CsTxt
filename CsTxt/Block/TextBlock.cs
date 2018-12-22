using System;
using System.Collections.Generic;
using System.Text;

namespace CsTxt.Block
{
    public class TextBlock : BlockBase, IBlock
    {
        private TextBlock() { }

        public override BlockType Type { get; } = BlockType.Text;

        public override string Content { get; protected set; }

        public static IBlock Parse(string text)
        {
            int index = 0;
            StringBuilder sb = new StringBuilder();

            for (bool isExitBlock = false;!isExitBlock && index < text.Length;index++)
            {
                if ('@' == text[index])
                {
                    if ('@' == text[index + 1])
                    {
                        // 2連続@ならエスケープとして解釈
                        sb.Append('@');

                        index++;
                    }
                    else
                    {
                        // 1個の@ならスクリプトブロックとして解釈
                        isExitBlock = true;
                    }
                }
                else
                {
                    sb.Append(text[index]);
                }
            }

            string result = sb.ToString();

            return new TextBlock
            {
                Content = result,
            };
        }

        public override string ToCSharpCode()
        {
            string escape = Content.Replace("\r", "\\r").Replace("\n", "\\n");

            return $"Out.Append(\"{escape}\");";
        }
    }
}

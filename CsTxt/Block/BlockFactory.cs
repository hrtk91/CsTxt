using System;
using System.Collections.Generic;
using System.Text;

namespace CsTxt.Block
{
    public static class BlockFactory
    {
        private static ICollection<Func<string, IBlock>> Parsers { get; } = new List<Func<string, IBlock>>
        {
            UsingBlock.Parse,
            ScriptBlock.Parse,
            CalcBlock.Parse,
            TextBlock.Parse,
        };

        public static bool TryParse(string text, Func<string, IBlock> func, out IBlock block)
        {
            block = func.Invoke(text);

            return BlockBase.Null != block;
        }

        public static IList<IBlock> Parse(string text)
        {
            IList<IBlock> blocks = new List<IBlock>();

            for (int index = 0;index < text.Length;)
            {
                string subText = text.Substring(index);

                using (ReadOnlyValue<int> currentIndex = ReadOnly<int>.Register(index))
                {
                    foreach (var parser in Parsers)
                    {
                        if (TryParse(text.Substring(index), parser, out IBlock block))
                        {
                            index += block.Content.Length;

                            if (0 < block.Content.Length)
                            {
                                blocks.Add(block);
                            }

                            block = BlockBase.Null;
                        }
                    }

                    if (currentIndex.Value == index)
                    {
                        break;
                    }
                }
            }

            return blocks;
        }
    }
}

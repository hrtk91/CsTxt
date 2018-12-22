using System;
using System.Collections.Generic;
using System.Text;

namespace CsTxt.Block
{
    internal class NullBlock : IBlock
    {
        public BlockType Type { get; }

        public string Content { get; } = string.Empty;

        private static Lazy<IBlock> LazyInstance { get; } = new Lazy<IBlock>(() => new NullBlock());

        public static IBlock Value => LazyInstance.Value;

        public string ToCSharpCode()
        {
            return string.Empty;
        }
    }
}

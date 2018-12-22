using System;
using System.Collections.Generic;
using System.Text;

namespace CsTxt.Block
{
    public abstract class BlockBase : IBlock
    {
        public static IBlock Null { get; } = NullBlock.Value;

        public abstract BlockType Type { get; }

        public abstract string Content { get; protected set; }

        public abstract string ToCSharpCode();
    }
}

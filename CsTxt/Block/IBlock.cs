using System;
using System.Collections.Generic;
using System.Text;

namespace CsTxt.Block
{
    public interface IBlock
    {
        BlockType Type { get; }

        string Content { get; }

        string ToCSharpCode();
    }
}

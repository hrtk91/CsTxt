using CsTxt.Block;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CsTxt
{
    public class CSharpText
    {
        public string Content { get; set; } = string.Empty;
        public IList<string> References { get; } = new List<string> { "System.Core", "System.Net.Http", };

        public CSharpText(string text)
        {
            Content = text;

        }

        public async Task<string> CompileToCSharpAsync()
        {
            var blocks = BlockFactory.Parse(Content);
            blocks.Insert(0, UsingBlock.Parse("@using System.Text;"));
            blocks.Insert(1, ScriptBlock.Parse("@{ var Out = new StringBuilder(); }"));
            blocks.Add(ScriptBlock.Parse("@{ string result = Out.ToString(); }"));

            return await blocks.ToCSharpAsync();
        }

        public async Task<string> RunAsync(string text)
        {
            Content = text;

            return await RunAsync();
        }

        public async Task<string> RunAsync()
        {
            var blocks = BlockFactory.Parse(Content);
            blocks.Insert(0, UsingBlock.Parse("@using System.Text;"));
            blocks.Insert(1, ScriptBlock.Parse("@{ var Out = new StringBuilder(); }"));
            blocks.Add(ScriptBlock.Parse("@{ string result = Out.ToString(); }"));

            var csharp = blocks.ToCSharp();

            var options = ScriptOptions.Default;

            foreach (var refelence in References)
            {
                options = options.AddReferences(refelence);
            }

            var script = CSharpScript.Create(csharp, options);
            var state = await script.RunAsync();

            foreach (var variable in state.Variables)
            {
                if (variable.Name == "result")
                {
                    return variable.Value.ToString();
                }
            }

            return string.Empty;
        }
    }
}

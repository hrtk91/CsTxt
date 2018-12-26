using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CSTPad.Model.Text
{
    public abstract class TextBoxProcessorBase : IDisposable
    {
        protected static readonly string INDENT = "    ";

        protected TextBox AssociatedObject { get; set; }

        public int Caret { get => AssociatedObject.CaretIndex; set => AssociatedObject.CaretIndex = value; }
        
        public void Registor(TextBox target)
        {
            AssociatedObject = target;
            AssociatedObject.PreviewKeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            char key = KeyConverter.GetCharFromKey(e.Key);

            OnKeyDown(AssociatedObject.Text, key, AssociatedObject.CaretIndex, e);
        }

        protected abstract void OnKeyDown(string text, char key, int caret, System.Windows.Input.KeyEventArgs e);

        protected static (int indent, bool isScriptBlock) ScriptIndentAnalyze(string text, int position)
        {
            int indent = 0;
            bool isScriptBlock = false;

            for (int i = 0; i < text.Length && i < position; i++)
            {
                if (text.Substring(i).StartsWith("@{"))
                {
                    i += 1;
                    indent += 1;
                    isScriptBlock = true;

                    continue;
                }

                if (text.Substring(i).StartsWith("{"))
                {
                    if (isScriptBlock)
                    {
                        indent++;
                    }
                }

                if (text.Substring(i).StartsWith("}"))
                {
                    if (isScriptBlock)
                    {
                        if (1 < indent)
                        {
                            indent--;
                        }
                        else
                        {
                            indent--;
                            isScriptBlock = false;
                        }
                    }
                }
            }

            return (indent, isScriptBlock);
        }
        
        protected static string GetIndent(int indent)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var i in Enumerable.Range(0, indent))
            {
                sb.Append(INDENT);
            }

            return sb.ToString();
        }

        protected static (int Start, int End, string Line) GetCaretLineInfo(string text, int caret)
        {
            int start = 0;
            int end = 0;

            for (start = caret;1 < start && '\n' != text[start - 1];start--)
            {

            }

            for (end = caret;end < text.Length - 1 && '\n' != text[end+1];end++)
            {

            }

            int length = end - start;
            string line = text.Substring(start, length);

            return (start, end, line);
        }

        public void Dispose()
        {
            AssociatedObject.PreviewKeyDown -= OnKeyDown;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace CSTPad.Model
{
    public class CsTextBehavior : Behavior<TextBox>
    {
        private static string INDENT { get; } = "    ";
        private static string NEW_LINE_SRING { get; } = "\r\n";

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.KeyUp += OnKeyUp;
            AssociatedObject.PreviewKeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    // Shift + Tab
                    int caret = AssociatedObject.CaretIndex;
                    StringBuilder sb = new StringBuilder();
                    for (int i = caret - 1;0 < i;i--)
                    {
                        if ('\n' == AssociatedObject.Text[i])
                        {
                            break;
                        }

                        sb.Insert(0, AssociatedObject.Text[i]);
                    }

                    string line = sb.ToString();

                    if (line.All(x => ' ' == x))
                    {
                        if (4 <= line.Length)
                        {
                            AssociatedObject.Text = AssociatedObject.Text.Remove(caret - 4, 4);
                            AssociatedObject.CaretIndex = caret - 4;
                        }
                        else
                        {
                            AssociatedObject.Text = AssociatedObject.Text.Remove(caret - line.Length, line.Length);
                            AssociatedObject.CaretIndex = caret - line.Length;
                        }
                    }
                }
                else
                {
                    // Tab
                    int caret = AssociatedObject.CaretIndex;
                    AssociatedObject.Text =
                        AssociatedObject.Text.Insert(caret, "    ");
                    AssociatedObject.CaretIndex = caret + 4;
                }

                e.Handled = true;
            }

            if (e.Key == Key.Enter)
            {
                // TODO: 前行のインデントを保持
            }
        }

        private (int indent, bool isScriptBlock) Analyze(string text, int position)
        {
            int indent = 0;
            bool isScriptBlock = false;

            for (int i = 0;i < text.Length && i < position;i++)
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

        private (int startIndex, int endIndex, string text) GetLine(string text, int index)
        {
            int startIndex = index;
            for (int i = index;0 < i && !NEW_LINE_SRING.Contains(text[i]);i--)
            {
                startIndex = i;
            }

            int endIndex = index;
            for (int i = startIndex;i < text.Length && !NEW_LINE_SRING.Contains(text[i]);i++)
            {
                endIndex = i + 1;
            }

            int length = endIndex - startIndex;

            return (startIndex, endIndex, text.Substring(startIndex, length));
        }

        private string GetIndent(int indent)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var i in Enumerable.Range(0, indent))
            {
                sb.Append(INDENT);
            }

            return sb.ToString();
        }

        private void OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (0 == AssociatedObject.Text.Length)
            {
                return;
            }

            if (e.Key == System.Windows.Input.Key.Return)
            {
                string text = AssociatedObject.Text;
                int index = AssociatedObject.SelectionStart;
                int indent = Analyze(AssociatedObject.Text, index).indent;

                StringBuilder sb = new StringBuilder(text);
                for (int i = 0; i < indent; i++)
                {
                    sb.Insert(index, INDENT);
                }

                AssociatedObject.Text = sb.ToString();
                AssociatedObject.SelectionStart = index + INDENT.Length * indent;
            }
            
            if (0 < AssociatedObject.SelectionStart && '}' == AssociatedObject.Text[AssociatedObject.SelectionStart - 1])
            {
                (int indentCount, bool isScriptBlock) = Analyze(AssociatedObject.Text, AssociatedObject.SelectionStart - 1);
                (int startIndex, int endIndex, string line) = GetLine(AssociatedObject.Text, AssociatedObject.SelectionStart - 1);

                if (isScriptBlock && Regex.IsMatch(line, "^ +} *$"))
                {
                    string text = AssociatedObject.Text;
                    string indent = GetIndent(indentCount - 1);
                    string newText = 
                        text.Remove(startIndex, endIndex - startIndex)
                            .Insert(startIndex, indent)
                            .Insert(startIndex + indent.Length, "}");
                    int caret = startIndex + INDENT.Length * (indentCount - 1) + 1;

                    AssociatedObject.Text = newText;
                    AssociatedObject.SelectionStart = caret;
                }
            }
        }
    }
}

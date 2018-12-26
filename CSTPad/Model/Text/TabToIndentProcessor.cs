using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CSTPad.Model.Text
{
    /// <summary>Tabキー入力管理</summary>
    public class TabToIndentProcessor : TextBoxProcessorBase
    {
        protected override void OnKeyDown(string text, char key, int caret, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                {
                    if (0 == AssociatedObject.SelectionLength)
                    {
                        // 単行インデント削除
                        RemoveOneLineTab(text, caret);
                    }
                    else
                    {
                        // 複数行インデント削除
                        RemoveMultiLineTab(text, caret);
                    }
                }
                else
                {
                    if (0 == AssociatedObject.SelectionLength)
                    {
                        // 単行インデント追加
                        InsertOneLineTab(text, caret);
                    }
                    else
                    {
                        // 複数行インデント追加
                        InsertMultiLineTab(text, caret);
                    }
                }

                e.Handled = true;
            }
        }

        private void InsertOneLineTab(string text, int caret)
        {
            // Tab: 1段階インデント追加
            AssociatedObject.Text = text.Insert(caret, "    ");

            AssociatedObject.CaretIndex = caret + INDENT.Length;
        }

        private void RemoveOneLineTab(string text, int caret)
        {
            // Shift + Tab: 1段階インデント解除
            StringBuilder sb = new StringBuilder();

            for (int i = caret - 1; 0 < i; i--)
            {
                if ('\n' == text[i])
                {
                    break;
                }

                sb.Insert(0, text[i]);
            }

            string line = sb.ToString();

            if (line.All(x => ' ' == x))
            {
                if (INDENT.Length <= line.Length)
                {
                    AssociatedObject.Text = text.Remove(caret - INDENT.Length, INDENT.Length);
                    AssociatedObject.CaretIndex = caret - INDENT.Length;
                }
                else
                {
                    AssociatedObject.Text = text.Remove(caret - line.Length, line.Length);
                    AssociatedObject.CaretIndex = caret - line.Length;
                }
            }
        }

        private void InsertMultiLineTab(string text, int caret)
        {
            int selectionStart = AssociatedObject.SelectionStart;
            int selectionEnd = selectionStart + AssociatedObject.SelectionLength;
            string selectionText = AssociatedObject.Text.Substring(selectionStart, AssociatedObject.SelectionLength);
            string newText = Regex.Replace(selectionText, "^", INDENT, RegexOptions.Multiline);

            AssociatedObject.Text =
                text.Substring(0, selectionStart) +
                newText +
                text.Substring(selectionEnd);

            AssociatedObject.SelectionStart = selectionStart;
            AssociatedObject.SelectionLength = newText.Length;
        }

        private void RemoveMultiLineTab(string text, int caret)
        {
            int selectionStart = AssociatedObject.SelectionStart;
            int selectionEnd = selectionStart + AssociatedObject.SelectionLength;
            string selectionText = AssociatedObject.Text.Substring(selectionStart, AssociatedObject.SelectionLength);
            string newText = Regex.Replace(selectionText, "^ {0,4}", string.Empty, RegexOptions.Multiline);

            AssociatedObject.Text =
                text.Substring(0, selectionStart) +
                newText +
                text.Substring(selectionEnd);

            AssociatedObject.SelectionStart = selectionStart;
            AssociatedObject.SelectionLength = newText.Length;
        }
    }
}

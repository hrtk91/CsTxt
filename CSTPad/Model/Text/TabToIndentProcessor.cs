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
            int start = 0;
            int end = 0;

            if (caret == 0)
            {
                return;
            }

            for (start = caret; 0 < start && text[start - 1] != '\n'; start--) ;
            for (end = caret; end < text.Length - 1 && text[end + 1] != '\n'; end++) ;

            string line = text.Substring(start, end - start);
            string newText = string.Empty;
            if (text[caret - 1] == ' ')
            {
                var linecaret = caret - start;
                var spaceCount = 0;
                for (spaceCount = 0; spaceCount < INDENT.Length; spaceCount++)
                {
                    var index = linecaret - spaceCount - 1;
                    if (index >= 0 && line[index] == ' ')
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                newText = line.Remove(linecaret - spaceCount, spaceCount);
            }
            else
            {
                newText = Regex.Replace(line, "^ {0,4}", string.Empty, RegexOptions.None);
            }

            AssociatedObject.Text = text.Substring(0, start) + newText + text.Substring(end);
            AssociatedObject.CaretIndex = caret - (line.Length - newText.Length);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                        // 単行インデント追加
                        InsertOneLineTab(caret);
                    }
                    else
                    {
                        // TODO: 複数行インデント追加
                    }
                }
                else
                {
                    if (0 == AssociatedObject.SelectionLength)
                    {
                        // 単行インデント削除
                        RemoveOneLineTab(caret);
                    }
                    else
                    {
                        // TODO: 複数行インデント削除
                    }
                }

                e.Handled = true;
            }
        }

        private void InsertOneLineTab(int caret)
        {
            // Shift + Tab: 1段階インデント解除
            StringBuilder sb = new StringBuilder();

            for (int i = caret - 1; 0 < i; i--)
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
                if (INDENT.Length <= line.Length)
                {
                    AssociatedObject.Text = AssociatedObject.Text.Remove(caret - INDENT.Length, INDENT.Length);
                    AssociatedObject.CaretIndex = caret - 4;
                }
                else
                {
                    AssociatedObject.Text = AssociatedObject.Text.Remove(caret - line.Length, line.Length);
                    AssociatedObject.CaretIndex = caret - line.Length;
                }
            }
        }

        private void RemoveOneLineTab(int caret)
        {
            // Tab: 1段階インデント追加
            AssociatedObject.Text =
                AssociatedObject.Text.Insert(caret, "    ");

            AssociatedObject.CaretIndex = caret + INDENT.Length;
        }
    }
}

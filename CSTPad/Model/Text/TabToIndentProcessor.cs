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
                    // Tab: 1段階インデント追加
                    AssociatedObject.Text =
                        AssociatedObject.Text.Insert(caret, "    ");

                    AssociatedObject.CaretIndex = caret + 4;
                }

                e.Handled = true;
            }
        }
    }
}

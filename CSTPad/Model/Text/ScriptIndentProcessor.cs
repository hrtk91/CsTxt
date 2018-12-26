using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSTPad.Model.Text
{
    /// <summary>スクリプト内でのインデント管理</summary>
    public class ScriptIndentProcessor : TextBoxProcessorBase
    {
        protected override void OnKeyDown(string text, char key, int caret, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                (int indent, bool isBlock) = ScriptIndentAnalyze(text, caret);

                if (isBlock)
                {
                    // スクリプト内ではブラケット数に合わせてオートインデント
                    string space = GetIndent(indent);
                    AssociatedObject.Text =
                        text.Substring(0, caret) + "\r\n" +
                        space +
                        text.Substring(caret);

                    AssociatedObject.CaretIndex = caret + "\r\n".Length + space.Length;
                }
                else
                {
                    // テキスト内では前の行のスペース数に合わせてオートインデント
                    var lineInfo = GetCaretLineInfo(text, caret);
                    var space = new string(lineInfo.Line.TakeWhile(x => ' ' == x).ToArray());

                    AssociatedObject.Text =
                        text.Substring(0, lineInfo.End) + "\r\n" + space + text.Substring(lineInfo.End);
                    AssociatedObject.CaretIndex = lineInfo.End + "\r\n".Length + space.Length;
                }

                e.Handled = true;
            }

            if ('}' == key)
            {
                (int indent, bool isBlock) = ScriptIndentAnalyze(text, caret);

                var lineInfo = GetCaretLineInfo(text, caret);
                
                if (string.IsNullOrWhiteSpace(lineInfo.Line))
                {
                    string space = GetIndent(indent - 1);

                    AssociatedObject.Text =
                        text.Substring(0, lineInfo.Start) + space + "}" +
                        text.Substring(lineInfo.End, text.Length - lineInfo.End);

                    if (0 < indent)
                    {
                        // スペース4つ以上(現インデント幅 - 1の位置にキャレットを設定)
                        // 現在位置 - スペース幅 + "}".Length
                        AssociatedObject.CaretIndex = caret - INDENT.Length + 1;
                    }
                    else
                    {
                        // スペース3つ以内(全スペース削除後の位置にキャレットを設定)
                        AssociatedObject.CaretIndex = lineInfo.Start;
                    }

                    e.Handled = true;
                }
            }
        }
    }
}

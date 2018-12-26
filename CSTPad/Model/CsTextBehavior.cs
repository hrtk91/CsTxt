using CSTPad.Model.Text;
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
        private TextBoxProcessorBase TabToIndent { get; } = new TabToIndentProcessor();
        private TextBoxProcessorBase ScriptIndent { get; } = new ScriptIndentProcessor();

        protected override void OnAttached()
        {
            base.OnAttached();

            TabToIndent.Registor(AssociatedObject);
            ScriptIndent.Registor(AssociatedObject);
        }
    }
}

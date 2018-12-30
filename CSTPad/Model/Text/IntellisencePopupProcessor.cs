using CSTPad.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSTPad.Model.Text
{
    public class IntellisencePopupProcessor : TextBoxProcessorBase
    {
        public ICommand FocusCommand { get; set; }

        public ObservableCollection<string> Items { get; set; }

        protected override void OnKeyDown(string text, char key, int caret, KeyEventArgs e)
        {
            Items?.Clear();

            if (e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Left || e.Key == Key.Right)
            {
                if (0 < (Items?.Count ?? 0))
                {
                    FocusCommand?.Execute(null);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfKit.ViewModelKit;

namespace CSTPad.ViewModel
{
    public class DebugEditorViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual string CsText { get; set; } = string.Empty;

        public virtual string CSharpText { get; set; } = string.Empty;

        public virtual string ResultText { get; set; } = string.Empty;

        public ICommand InitializeCSharpView = new ActionCommand(context =>
        {

        });
    }
}

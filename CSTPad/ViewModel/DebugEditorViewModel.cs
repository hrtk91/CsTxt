using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

        public virtual DateTime NowTime { get; set; } = DateTime.Now;

        public ICommand Initialize => new ActionCommand(context =>
        {
            Timer timer = new Timer(1000);
            timer.Elapsed += (sender, e) => NowTime = DateTime.Now;
            timer.Start();
        });
    }
}

using CSTPad.Model;
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
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool IsInitializing { get; set; } = true;

        public ICommand Initialize => new ActionCommand(async _ =>
        {
            await Compiler.Compile("@using System;");

            IsInitializing = false;
        });
    }
}

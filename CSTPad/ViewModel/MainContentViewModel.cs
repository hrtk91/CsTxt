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
    public class MainContentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool IsToolBarExpand { get; set; } = false;

        public ICommand Initialize => new ActionCommand(_ =>
        {

        });
    }
}

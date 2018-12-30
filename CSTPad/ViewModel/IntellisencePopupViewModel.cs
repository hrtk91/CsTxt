using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfKit.ViewModelKit;

namespace CSTPad.ViewModel
{
    public class IntellisencePopupViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual bool IsFocus { get; set; } = false;

        public virtual bool IsVisible { get; set; } = false;
        
        public ObservableCollection<string> Items { get; } = new ObservableCollection<string>();

        public ICommand Initialize => new ActionCommand(_ =>
        {
            Items.CollectionChanged += (sender, e) =>
            {
                if (0 < Items.Count)
                {
                    IsVisible = true;
                }
                else
                {
                    IsVisible = false;
                }
            };
        });
    }
}

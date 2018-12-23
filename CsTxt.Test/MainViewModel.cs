using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using WpfKit.ViewModelKit;

namespace CsTxt.Test
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private CSharpText CSharpTextCompiler { get; } = new CSharpText(string.Empty);

        public virtual string FileName { get; set; }

        public virtual string CSharpText { get; set; }

        public virtual string CSharp { get; set; }

        public virtual string Text { get; set; }

        public virtual string Now { get; set; }

        public ICommand Initialize => new ActionCommand(_ =>
        {

            PropertyChanged += async (sender, e) =>
            {
                if (e.PropertyName == nameof(CSharpText))
                {
                    CSharpTextCompiler.Content = CSharpText;
                    try
                    {
                        CSharp = await CSharpTextCompiler.CompileToCSharpAsync();
                        Text = await CSharpTextCompiler.RunAsync();
                    }
                    catch (Exception ex)
                    {
                        CSharp = ex.Message + "\r\n" + ex.StackTrace;
                        Text = string.Empty;
                    }
                }
            };

            Timer timer = new Timer(1000);
            timer.Elapsed += (sender, e) => Now = DateTime.Now.ToString();
            timer.Start();
        });
    }
}

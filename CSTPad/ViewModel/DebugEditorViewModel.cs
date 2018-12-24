using CsTxt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Timers;
using System.Windows.Input;
using WpfKit.ViewModelKit;

namespace CSTPad.ViewModel
{
    public class DebugEditorViewModel : INotifyPropertyChanged
    {
        private static CSharpText CSharpTextInstance { get; } = new CSharpText(string.Empty);

        public event PropertyChangedEventHandler PropertyChanged;

        private ActionBlock<string> CSharpConvertionBlock { get; set; }

        public virtual string CsText { get; set; } = string.Empty;

        public virtual string CSharp { get; set; } = string.Empty;

        public virtual string ResultText { get; set; } = string.Empty;

        public virtual DateTime NowTime { get; set; } = DateTime.Now;

        public ICommand Initialize => new ActionCommand(context =>
        {
            CSharpConvertionBlock = new ActionBlock<string>(text =>
            {
                CSharp = ResultText = "<処理中...>";

                try
                {
                    var result = CSharpTextInstance.RunAsync(text).Result;

                    CSharp = CSharpTextInstance.CompileToCSharpAsync().Result;
                    ResultText = result;
                }
                catch (Exception e)
                {
                    ResultText = e.InnerException.Message + "\r\n" + e.InnerException.StackTrace;
                }
            });

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "NowTime")
                {
                    return;
                }

                if (nameof(CsText) == e.PropertyName)
                {
                    CSharpConvertionBlock.Post(CsText);
                }
            };

            Timer timer = new Timer(1000);
            timer.Elapsed += (sender, e) => NowTime = DateTime.Now;
            timer.Start();

            CSharpConvertionBlock.Post(string.Empty);
        });
    }
}

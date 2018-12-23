using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CSTPad.Model
{
    public class CSharpResultConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string csharp && !string.IsNullOrWhiteSpace(csharp))
            {
                try
                {
                    Compiler.CSharpTextCompiler.Content = csharp;

                    return Compiler.CSharpTextCompiler.RunAsync().Result;
                }
                catch (Exception e)
                {
                    return e.InnerException.Message + "\r\n" + e.InnerException.StackTrace;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

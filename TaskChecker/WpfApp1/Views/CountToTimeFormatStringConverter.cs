using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApp1.Views
{
    class CountToTimeFormatStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int seconds = (int)value;
            var span = new TimeSpan(0, 0, seconds);

            // フォーマットする
            var hhmmss = span.ToString(@"hh\:mm\:ss");
            return hhmmss;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

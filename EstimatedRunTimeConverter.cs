using System;
using System.Globalization;
using System.Windows.Data;

namespace PocketTDPControl
{

    public class EstimatedRunTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            int.TryParse(value as string, out var runtime);

            // 71582788 = charging
            return runtime == 71582788? "charging" : runtime.ToString();

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

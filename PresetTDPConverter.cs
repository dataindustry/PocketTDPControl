using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace PocketTDPControl
{
    public class PresetTDPConverter : IMultiValueConverter
    {
        private ObservableCollection<int> presetTDP;
        private int presetTDPIndex;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            this.presetTDP = values[0] as ObservableCollection<int>;
            this.presetTDPIndex = (int)values[1];
            return (double)presetTDP[presetTDPIndex];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var presetTDPValue = (double)value;
            this.presetTDP[this.presetTDPIndex] = (int)Math.Round(presetTDPValue, 0);
            return new object[] {this.presetTDP, this.presetTDPIndex};
        }
    }
}

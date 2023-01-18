using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace PocketTDPControl
{
    public class PresetTDPConverter : IMultiValueConverter
    {
        private ObservableCollection<int> presetTDP;
        private int selectedPresetTDPIndex;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            this.presetTDP = values[0] as ObservableCollection<int>;

            if (values[1] != null)
                this.selectedPresetTDPIndex = (int)values[1];
            else
                {
                this.selectedPresetTDPIndex = 0;
            }

            return (double)presetTDP[selectedPresetTDPIndex];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var presetTDPValue = (double)value;
            this.presetTDP[this.selectedPresetTDPIndex] = (int)Math.Round(presetTDPValue, 0);
            return new object[] {this.presetTDP, this.selectedPresetTDPIndex};
        }
    }

}

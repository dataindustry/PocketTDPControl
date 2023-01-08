using System;
using System.Windows;

namespace PocketTDPControl
{
    /// <summary>
    /// TDPSliderWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TDPSliderWindow : Window
    {
        public TDPSliderWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

    }
}

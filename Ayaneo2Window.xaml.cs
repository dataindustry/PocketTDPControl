using System.Windows;

namespace PocketTDPControl
{
    /// <summary>
    /// Ayaneo2Window.xaml 的交互逻辑
    /// </summary>
    public partial class Ayaneo2Window : Window
    {
        public Ayaneo2Window()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

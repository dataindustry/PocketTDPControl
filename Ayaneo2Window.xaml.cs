using PocketTDPControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Windows.UI.Xaml.Controls;
using WindowsInput.Native;
using CheckBox = System.Windows.Controls.CheckBox;

namespace PocketTDPControl
{
    /// <summary>
    /// Ayaneo2Window.xaml 的交互逻辑
    /// </summary>
    public partial class AYANEO2Window : Window
    {
        public AYANEO2Window()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void Ayaneo2LOGOCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Operation.CustomKeyMapping(
                new[] { Keys.LWin, Keys.RControlKey, Keys.F17 },
                new[] { VirtualKeyCode.LWIN, VirtualKeyCode.VK_G });
        }

        private void Ayaneo2LOGOCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Operation.CancelCustomKeyMapping();
        }
    }
}

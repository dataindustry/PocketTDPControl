using Microsoft.Web.WebView2.Core;
using PocketTDPControl;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Windows.UI.Xaml.Controls;
using WindowsInput.Native;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = e.Source as CheckBox;

            webView.CoreWebView2.PostWebMessageAsString(checkbox.Name);

            if(checkbox.Name == "LOGO")
                Operation.CustomKeyMapping(
                    new[] { Keys.LWin, Keys.RControlKey, Keys.F17 },
                    new[] { VirtualKeyCode.LWIN, VirtualKeyCode.VK_G });
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkbox = e.Source as CheckBox;

            webView.CoreWebView2.PostWebMessageAsString("reset");

            if (checkbox.Name == "LOGO")
                Operation.CancelCustomKeyMapping();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var environment = await CoreWebView2Environment.CreateAsync(userDataFolder: Environment.CurrentDirectory);
            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompletedAsync;
            await webView.EnsureCoreWebView2Async(environment);
        }

        private void WebView_CoreWebView2InitializationCompletedAsync(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {

            webView.CoreWebView2.SetVirtualHostNameToFolderMapping("local", Environment.CurrentDirectory,
                CoreWebView2HostResourceAccessKind.Allow);
            webView.Source = new Uri(Environment.CurrentDirectory + @"/AYANEO2Window.html");

        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }
    }   
}

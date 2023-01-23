using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PocketTDPControl
{
    /// <summary>
    /// CurveWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CurveWindow : Window
    {
        public CurveWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            webView.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompletedAsync;
            webView.EnsureCoreWebView2Async();

        }

        private async void WebView_CoreWebView2InitializationCompletedAsync(object sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {

            string text = System.IO.File.ReadAllText(@"./AYANEO2Window.html");
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(text);

        }
    }
}

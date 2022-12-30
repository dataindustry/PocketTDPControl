using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace PocketTDPControl
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

        }

        public bool Adjust(string type, int tdp) {

            Process p = new Process();

            p.StartInfo.FileName = "ryzenadj.exe";
            p.StartInfo.Arguments = $"-{type} {tdp}";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();

            var successResult = p.StandardOutput.ReadToEnd();

            p.Close();
            p.Dispose();

            if (successResult.StartsWith("Sucessfully set"))
            {
                return true;
            }
            else {
                return false;
            }
        }

        public bool LoopbackExempt() 
        {
            RegistryKey rkConfig = Registry.CurrentUser.OpenSubKey("Software\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\CurrentVersion\\AppContainer\\Mappings\\");

            var keys = rkConfig.GetSubKeyNames();

            foreach (string key in keys)
            {

                if (rkConfig.OpenSubKey(key).GetValue("DisplayName").ToString() == "PocketTDPControlWidget")
                {
                    Console.WriteLine(key);

                    Process p = new Process();

                    p.StartInfo.FileName = "CheckNetIsolation";
                    p.StartInfo.Arguments = $"LoopbackExempt -a -p={key}";
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;

                    p.Start();

                    string error = p.StandardError.ReadToEnd();

                    p.Close();

                    if (error.Trim() == "")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Adjust("a", 22000);
            // Adjust("b", 22000);
            // Adjust("c", 22000);

            
        }

    }
}

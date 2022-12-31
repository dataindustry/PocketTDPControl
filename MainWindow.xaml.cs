using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace PocketTDPControl
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private TDPViewModel ViewModel;

        private string FilePath = "tdp.json";

        ConcurrentQueue<int> TDPQueue = new ConcurrentQueue<int>();

        private NotifyIcon TrayIcon = null;

        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists(this.FilePath))
            {
                this.ViewModel = JsonConvert.DeserializeObject<TDPViewModel>(File.ReadAllText("tdp.json"));
            }
            else
            {
                this.ViewModel = new TDPViewModel();
                File.WriteAllText(this.FilePath, JsonConvert.SerializeObject(this.ViewModel));
            }

            this.DataContext = this.ViewModel;
            this.ViewModel.PropertyChanged += OnPropertyChanged;

            Task t = new Task(() =>
            {
                while (true)
                {

                    if (TDPQueue.IsEmpty)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    TDPQueue.TryDequeue(out var tdp);

                    Operation.Adjust("a", this.ViewModel.CurrentTDP);
                    Operation.Adjust("b", this.ViewModel.CurrentTDP);
                    Operation.Adjust("c", this.ViewModel.CurrentTDP);

                }
            });
            t.Start();

            ServiceHost host = new ServiceHost(typeof(MainService));
            host.Open();

            InitialTray();
            this.TrayIcon.Visible = false;

        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TDPQueue.Enqueue(this.ViewModel.CurrentTDP);
        }

        private void AdjustButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button clickedButton = sender as System.Windows.Controls.Button;
            int tdp = Convert.ToInt32(clickedButton.Content.ToString());
            this.ViewModel.CurrentTDP = tdp;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.PresetTDP = this.ViewModel.CurrentTDP;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.TrayIcon.Visible = false;
            this.TrayIcon.Dispose();
            File.WriteAllText(this.FilePath, JsonConvert.SerializeObject(this.ViewModel));
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {

            this.Visibility = Visibility.Hidden;
            this.TrayIcon.Visible = true;

        }

        private void InitialTray()
        {
            TrayIcon = new NotifyIcon();
            TrayIcon.Text = this.Title;
            TrayIcon.Visible = true;
            TrayIcon.Icon = new Icon(@"TrayIcon.ico");
            TrayIcon.Click += TrayIcon_Click;
        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            this.Visibility = Visibility.Visible;
            this.TrayIcon.Visible = false;
            this.Activate();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow setting = new SettingWindow();
            double[] d = new double[2];
            d[0] = this.Top + this.Height / 2;
            d[1] = this.Left + this.Width / 2;
            setting.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;
            setting.Top = d[0] - setting.Height / 2;
            setting.Left = d[1] - setting.Width / 2;
            setting.ShowDialog();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

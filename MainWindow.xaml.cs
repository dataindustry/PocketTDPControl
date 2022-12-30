using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TDPQueue.Enqueue(this.ViewModel.CurrentTDP);
        }

        private void AdjustButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
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
            File.WriteAllText(this.FilePath, JsonConvert.SerializeObject(this.ViewModel));
        }

        private void LoopbackExemptButton_Click(object sender, RoutedEventArgs e)
        {
            Task t = new Task(() =>
            {
                Operation.LoopbackExempt();
            });
            t.Start();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }
    }
}

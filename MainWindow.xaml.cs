using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Native;
using static System.Management.ManagementObjectCollection;

namespace PocketTDPControl
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private TDPViewModel ViewModel;

        private readonly string FilePath = "tdp1.json";

        private ConcurrentQueue<int> TDPQueue;

        private NotifyIcon TrayIcon;

        private KeyboardHook CustomizeKeyboardHook;

        private HardwareMonitor HWM;

        public MainWindow()
        {
            InitializeComponent();

            InitialViewModel();

            InitialTray();

            InitialWCFServer();

            InitialTDPAdjustor();

            RemapAyaneo2IconKeyToGameBarKey();

            CheckBattery();

            InitialSensorReading();

        }

        private void InitialSensorReading()
        {
            HWM = new HardwareMonitor(this.ViewModel);

            Task t = new Task(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);

                    HWM.Update();

                }
            });
            t.Start();
        }

        private void CheckBattery()
        {

            ManagementObjectEnumerator mom = new ManagementClass("Win32_Battery").GetInstances().GetEnumerator();
            if (mom.MoveNext())
            {
                Console.WriteLine(mom.Current.Properties["EstimatedChargeRemaining"].Value);
                Console.WriteLine(mom.Current.Properties["EstimatedRunTime"].Value);
            }
        }

        private void RemapAyaneo2IconKeyToGameBarKey()
        {
            CustomizeKeyboardHook = new KeyboardHook();
            CustomizeKeyboardHook.InstallHook(this.KeyboardHookKeyPress);

            if(this.ViewModel.IsAyaneo2LogoRemapEnabled)
                Operation.RemapComboKey(
                    new[] { Keys.LWin, Keys.RControlKey, Keys.F17 },
                    new[] { VirtualKeyCode.LWIN, VirtualKeyCode.VK_G });
        
        }
        private void InitialViewModel()
        {
            if (File.Exists(this.FilePath))
            {
                this.ViewModel = JsonConvert.DeserializeObject<TDPViewModel>(File.ReadAllText(FilePath));
            }
            else
            {
                this.ViewModel = new TDPViewModel();
                File.WriteAllText(this.FilePath, JsonConvert.SerializeObject(this.ViewModel));
            }

            this.ViewModel.PresetTDP = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            this.DataContext = this.ViewModel;
            this.ViewModel.PropertyChanged += OnPropertyChanged;

        }
        private void InitialTDPAdjustor()
        {
            this.TDPQueue = new ConcurrentQueue<int>();

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

                    Operation.Adjust("a", this.ViewModel.ApplyTDP);
                    Operation.Adjust("b", this.ViewModel.ApplyTDP);
                    Operation.Adjust("c", this.ViewModel.ApplyTDP);

                }
            });
            t.Start();
        }
        private void InitialWCFServer()
        {
            ServiceHost host = new ServiceHost(typeof(MainService));
            host.Open();
        }
        private void InitialTray()
        {
            TrayIcon = new NotifyIcon();
            TrayIcon.Text = this.Title;
            TrayIcon.Visible = true;
            TrayIcon.Icon = new Icon(@"TrayIcon.ico");
            TrayIcon.Click += TrayIcon_Click;
        }

        private void KeyboardHookKeyPress(KeyboardHook.HookStruct hookStruct, out bool handle)
        {
            handle = false;

            foreach (var pair in Operation.FromComboKey.ToArray())
            {
                if (hookStruct.vkCode == pair.Key)
                {
                    Operation.FromComboKey[pair.Key] = true;
                }
            }

            var checklist = Operation.FromComboKey.Values.Distinct();

            if (checklist.Count() == 1 && checklist.First() == true)
            {

                foreach (var pair in Operation.FromComboKey.ToArray())
                {
                    Operation.FromComboKey[pair.Key] = false;
                }
                new InputSimulator().Keyboard.ModifiedKeyStroke(Operation.ToModifierKey, Operation.ToKey);
            }

            handle = true;

        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TDPQueue.Enqueue(this.ViewModel.ApplyTDP);

            if(e.PropertyName == nameof(this.ViewModel.IsAyaneo2LogoRemapEnabled)) {
                if (this.ViewModel.IsAyaneo2LogoRemapEnabled)
                {
                    Operation.RemapComboKey(
                    new[] { Keys.LWin, Keys.RControlKey, Keys.F17 },
                    new[] { VirtualKeyCode.LWIN, VirtualKeyCode.VK_G });
                }
                else {
                    Operation.FromComboKey.Clear();
                    Operation.ToModifierKey = 0;
                    Operation.ToKey = 0;
                }
            }
        }
        private void AdjustButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button clickedButton = sender as System.Windows.Controls.Button;
            int tdp = Convert.ToInt32(clickedButton.Content.ToString());
            this.ViewModel.ApplyTDP = tdp;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            this.TrayIcon.Visible = false;
            this.TrayIcon.Dispose();

            CustomizeKeyboardHook?.UninstallHook();

            File.WriteAllText(this.FilePath, JsonConvert.SerializeObject(this.ViewModel));
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {

            this.Visibility = Visibility.Hidden;
            this.TrayIcon.Visible = true;

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
            setting.DataContext = this.ViewModel;
            setting.ShowDialog();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

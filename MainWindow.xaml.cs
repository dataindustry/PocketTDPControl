using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
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

        private ViewModel ViewModel;

        private readonly string FilePath = "tdp.json";

        private ConcurrentQueue<int> TDPQueue;

        private ConcurrentQueue<int> FanSpeedPrecentageQueue;

        private NotifyIcon TrayIcon;

        private HardwareMonitor HWM;

        private TDPWindow TDPWindowDialog;

        private SettingWindow SettingWindowDialog;
        private Window MachineWindow;

        private MethodInfo GetFanSpeedPrecentage;
        private MethodInfo SetFanSpeedPrecentage;
        private MethodInfo SetFanSpeedToAutoControl;
        private MethodInfo SetFanSpeedToManualControl;


        public MainWindow()
        {
            InitializeComponent();

            InitialViewModel();

            InitialSensorReading();

            InitialTray();

            InitialWCFServer();

            InitialFanController();

            InitialTDPAdjustor();

            InitialBatteryStatusReading();

            InitialDialog();

        }

        private void InitialDialog()
        {
            this.TDPWindowDialog = new TDPWindow();
            this.TDPWindowDialog.DataContext = this.ViewModel;

            this.SettingWindowDialog = new SettingWindow();
            this.SettingWindowDialog.DataContext = this.ViewModel;

        }
        private void InitialSensorReading()
        {
            HWM = new HardwareMonitor(this.ViewModel);

            Task t = new Task(() =>
            {
                while (true)
                {

                    HWM.Update();

                    this.ViewModel.Machine = Operation.DetermineMachineType(this.ViewModel.MachineName);

                    if (this.ViewModel.Machine != MachineType.None) {

                        this.ViewModel.IsSupportedMachine= true;

                        if(this.GetFanSpeedPrecentage == null) 
                            this.GetFanSpeedPrecentage = typeof(Operation).GetMethod($"Get{this.ViewModel.Machine}FanSpeedPrecentage");

                        this.ViewModel.FanSpeedPrecentage = (int)(Convert.ToDouble(this.GetFanSpeedPrecentage.Invoke(null, null)) * 100 / (double)byte.MaxValue);
                        this.ViewModel.FanSpeed = this.ViewModel.FanSpeedPrecentage * Convert.ToInt32(this.ViewModel.Machine) / 100;

                        if (!this.ViewModel.IsFanSpeedManualControlEnabled) this.ViewModel.ApplyFanSpeedPrecentage = this.ViewModel.FanSpeedPrecentage;
                    }
                    else
                    {
                        this.ViewModel.IsSupportedMachine = false;
                    }

                    Thread.Sleep(1000);
                }
            });
            t.Start();
        }
        private void InitialBatteryStatusReading()
        {
            
            Task t = new Task(() =>
            {
                while (true)
                {
              
                    ManagementObjectEnumerator mom = new ManagementClass("Win32_Battery").GetInstances().GetEnumerator();
                    if (mom.MoveNext())
                    {
                        this.ViewModel.EstimatedChargeRemaining = int.Parse(mom.Current.Properties["EstimatedChargeRemaining"].Value.ToString());
                        this.ViewModel.EstimatedRunTime = int.Parse(mom.Current.Properties["EstimatedRunTime"].Value.ToString());
                    }

                    Thread.Sleep(10000);

                }
            });
            t.Start();
            
        }
        private void InitialFanController() {

            this.FanSpeedPrecentageQueue = new ConcurrentQueue<int>();

            if (this.ViewModel.Machine != MachineType.None)
            {

                Task t = new Task(() =>
                {
                    while (true)
                    {

                        if (FanSpeedPrecentageQueue.IsEmpty)
                        {
                            Thread.Sleep(500);
                            continue;
                        }

                        if (FanSpeedPrecentageQueue.TryDequeue(out var fanSpeedPrecentage) && this.ViewModel.IsFanSpeedManualControlEnabled)
                        {
                            if(this.SetFanSpeedPrecentage == null) 
                                this.SetFanSpeedPrecentage = typeof(Operation).GetMethod($"Set{this.ViewModel.Machine}FanSpeedPrecentage");

                            this.SetFanSpeedPrecentage.Invoke(null, new object[] { (byte)fanSpeedPrecentage });
                        }
                    }
                });
                t.Start();
            }

        }
        private void InitialViewModel()
        {
            if (File.Exists(this.FilePath))
            {
                this.ViewModel = JsonConvert.DeserializeObject<ViewModel>(File.ReadAllText(FilePath));
            }
            else
            {
                this.ViewModel = new ViewModel();
                File.WriteAllText(this.FilePath, JsonConvert.SerializeObject(this.ViewModel));
            }

            this.ViewModel.IsEditModeEnabled = false;

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
                        Thread.Sleep(500);
                        continue;
                    }

                    if(TDPQueue.TryDequeue(out var tdp)) {
                        Operation.Adjust(
                            new string[]{ "a", "b", "c"}, 
                            new int[] { this.ViewModel.ApplyTDP, this.ViewModel.ApplyTDP, this.ViewModel.ApplyTDP });
                    }

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
            TrayIcon.Icon = new Icon(@"./TrayIcon.ico");
            TrayIcon.Click += TrayIcon_Click;
        }

        private void ChangeSudokuButtonStyle(System.Windows.Controls.Button button)
        {

            var grid = button.Parent as System.Windows.Controls.Grid;
            foreach (var child in grid.Children)
            {
                var sudokuButton = child as System.Windows.Controls.Button;
                sudokuButton.Background = System.Windows.Media.Brushes.Transparent;
            }

            button.Background = System.Windows.Media.Brushes.Aqua;
        }
        private void CenterizeWindowRelativeToMainWindow(Window window)
        {

            double[] d = new double[2];
            d[0] = this.Top + this.Height / 2;
            d[1] = this.Left + this.Width / 2;
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Top = d[0] - window.Height / 2;
            window.Left = d[1] - window.Width / 2;

        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(this.ViewModel.ApplyTDP))
                    TDPQueue.Enqueue(this.ViewModel.ApplyTDP);

            if(e.PropertyName == nameof(this.ViewModel.ApplyFanSpeedPrecentage) && this.ViewModel.IsFanSpeedManualControlEnabled)
                FanSpeedPrecentageQueue.Enqueue(this.ViewModel.ApplyFanSpeedPrecentage);
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

            Operation.DisposeKeyboardHook();

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
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(this.FilePath, JsonConvert.SerializeObject(this.ViewModel));

            this.TDPWindowDialog?.Close();
            this.MachineWindow?.Close();
            this.SettingWindowDialog?.Close();
            this.Close();
        }
        private void SudokuButton_Click(object sender, RoutedEventArgs e)
        {
            var button = e.OriginalSource as System.Windows.Controls.Button;
            ChangeSudokuButtonStyle(button);
            this.ViewModel.SelectedPresetTDPIndex = int.Parse(button.Name.Split('_')[1]);

            if (this.ViewModel.IsEditModeEnabled)
            {
                this.TDPWindowDialog.WindowStartupLocation = WindowStartupLocation.Manual;
                this.TDPWindowDialog.Top = this.Top;
                this.TDPWindowDialog.Left = this.Left + this.Width + 5;
            }
            else {
                this.ViewModel.ApplyTDP = this.ViewModel.PresetTDP[this.ViewModel.SelectedPresetTDPIndex];
            }

        }
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            var newPresetTDP = this.ViewModel.PresetTDP.ToArray<int>();
            Array.Sort(newPresetTDP);
            this.ViewModel.PresetTDP = new ObservableCollection<int>(newPresetTDP);
        }
        private void EditModeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.TDPWindowDialog.Top = this.Top;
            this.TDPWindowDialog.Left = this.Left + this.Width + 5;
        }
        private void GlobalSettingButton_Click(object sender, RoutedEventArgs e)
        {
            CenterizeWindowRelativeToMainWindow(SettingWindowDialog);
            this.SettingWindowDialog.Visibility = Visibility.Visible;
            this.SettingWindowDialog.Show();
            this.SettingWindowDialog.Activate();
        }
        private void MachineSettingButton_Click(object sender, RoutedEventArgs e)
        {

            if (this.ViewModel.Machine != MachineType.None)
            {

                if (this.MachineWindow == null)
                {
                    Type machineWindowType =
                        Type.GetType($"{MethodBase.GetCurrentMethod().DeclaringType.Namespace}.{this.ViewModel.Machine}Window");
                    this.MachineWindow = (Window)Activator.CreateInstance(machineWindowType);
                    this.MachineWindow.DataContext = this.ViewModel;
                }

                CenterizeWindowRelativeToMainWindow(this.MachineWindow);
                this.MachineWindow.Visibility = Visibility.Visible;
                this.MachineWindow.Show();
                this.MachineWindow.Activate();

            }
        }

        private void FanSpeedControlCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.SetFanSpeedToManualControl == null)
                this.SetFanSpeedToManualControl = typeof(Operation).GetMethod($"Set{this.ViewModel.Machine}FanSpeedToManualControl");
            this.SetFanSpeedToManualControl.Invoke(null, null);
        }

        private void FanSpeedControlCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.SetFanSpeedToAutoControl == null)
                this.SetFanSpeedToAutoControl = typeof(Operation).GetMethod($"Set{this.ViewModel.Machine}FanSpeedToAutoControl");
            this.SetFanSpeedToAutoControl.Invoke(null, null);
        }
    }
}

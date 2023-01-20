using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

namespace PocketTDPControl
{
    public class ViewModel : ViewModelBase
    {

        private string machineName;
        private MachineType machine;
        private bool isSupportedMachine;

        private string cpuName;
        private int cpuTemperture;
        private int cpuClock;

        private string gpuName;
        private int gpuTemperture;
        private int gpuClock;

        private int memoryAvailable;
        private int memoryUsed;

        private int estimatedChargeRemaining;
        private int estimatedRunTime;

        private int readingCpuTDP;
        private int readingGpuTDP;
        private int readingFPS;

        private int minTDP;
        private int maxTDP;

        private int fanSpeed;
        private int fanSpeedPrecentage;
        private int applyFanSpeedPrecentage;
        private bool isFanSpeedManualControlEnabled;

        private int applyTDP;

        private ObservableCollection<int> presetTDP;
        private int selectedPresetTDPIndex;

        private bool isEditModeEnabled;

        public string MachineName { get => machineName; set { machineName = value; RaisePropertyChanged(); } }
        public MachineType Machine { get => machine; set { machine = value; RaisePropertyChanged(); } }
        public bool IsSupportedMachine { get => isSupportedMachine; set { isSupportedMachine = value; RaisePropertyChanged(); } }
        public string CpuName { get => cpuName; set { cpuName = value; RaisePropertyChanged(); } }
        public string GpuName { get => gpuName; set { gpuName = value; RaisePropertyChanged(); } }
        public int CpuTemperture { get => cpuTemperture; set { cpuTemperture = value; RaisePropertyChanged(); } }
        public int GpuTemperture { get => gpuTemperture; set { gpuTemperture = value; RaisePropertyChanged(); } }
        public int CpuClock { get => cpuClock; set { cpuClock = value; RaisePropertyChanged(); } }
        public int GpuClock { get => gpuClock; set { gpuClock = value; RaisePropertyChanged(); } }
        public int MemoryAvailable { get => memoryAvailable; set { memoryAvailable = value; RaisePropertyChanged(); } }
        public int MemoryUsed { get => memoryUsed; set { memoryUsed = value; RaisePropertyChanged(); } }
        public int EstimatedChargeRemaining { get => estimatedChargeRemaining; set { estimatedChargeRemaining = value; RaisePropertyChanged(); } }
        public int EstimatedRunTime { get => estimatedRunTime; set { estimatedRunTime = value; RaisePropertyChanged(); } }
        public bool IsEditModeEnabled
        {
            get => isEditModeEnabled; set
            {
                isEditModeEnabled = value; RaisePropertyChanged();
            }
        }
        public int ReadingCpuTDP
        {
            get => readingCpuTDP; set
            {
                readingCpuTDP = value; RaisePropertyChanged();
            }
        }
        public int ReadingGpuTDP
        {
            get => readingGpuTDP; set
            {
                readingGpuTDP = value; RaisePropertyChanged();
            }
        }
        public int ReadingFPS
        {
            get => readingFPS; set
            {
                readingFPS = value; RaisePropertyChanged();
            }
        }
        public int MinTDP
        {
            get => minTDP; set
            {
                minTDP = value; RaisePropertyChanged();
            }
        }
        public int MaxTDP
        {
            get => maxTDP; set
            {
                maxTDP = value; RaisePropertyChanged();
            }
        }
        public int ApplyTDP
        {
            get => applyTDP; set
            {
                applyTDP = value; RaisePropertyChanged();
            }
        }
        public int FanSpeed
        {
            get => fanSpeed; set
            {
                fanSpeed = value; RaisePropertyChanged();
            }
        }
        public int FanSpeedPrecentage
        {
            get => fanSpeedPrecentage; set
            {
                fanSpeedPrecentage = value; RaisePropertyChanged();
            }
        }
        public int ApplyFanSpeedPrecentage
        {
            get => applyFanSpeedPrecentage; set
            {
                applyFanSpeedPrecentage = value; RaisePropertyChanged();
            }
        }
        public bool IsFanSpeedManualControlEnabled
        {
            get => isFanSpeedManualControlEnabled; set
            {
                isFanSpeedManualControlEnabled = value; RaisePropertyChanged();
            }
        }
        public ObservableCollection<int> PresetTDP
        {
            get { return presetTDP; }
            set { presetTDP = value; RaisePropertyChanged(); }
        }
        public int SelectedPresetTDPIndex
        {
            get => selectedPresetTDPIndex; set
            {
                selectedPresetTDPIndex = value; RaisePropertyChanged();
            }
        }

    }

}

using GalaSoft.MvvmLight;

namespace PocketTDPControl
{
    public class TDPViewModel : ViewModelBase
    {
        private string cpuName;
        private string gpuName;
        private string machineName;
        private int cpuTemperture;
        private int gpuTemperture;
        private bool isGpuAti;
        private int readingTDP;
        private int applyTDP;
        private int[] presetTDP;
        private bool isEditModeEnabled;
        private bool isAyaneo2LogoRemapEnabled;

        public string CpuName { get => cpuName; set { cpuName = value; RaisePropertyChanged(); } }
        public string GpuName { get => gpuName; set { gpuName = value; RaisePropertyChanged(); } }
        public string MachineName { get => machineName; set { machineName = value; RaisePropertyChanged(); } }
        public int CpuTemperture { get => cpuTemperture; set { cpuTemperture = value; RaisePropertyChanged(); } }
        public int GpuTemperture { get => gpuTemperture; set { gpuTemperture = value; RaisePropertyChanged(); } }

        public bool IsGpuAti { get => isGpuAti; set { isGpuAti = value; RaisePropertyChanged(); } }

        public bool IsEditModeEnabled
        {
            get => isEditModeEnabled; set
            {
                isEditModeEnabled = value; RaisePropertyChanged();
            }
        }

        public bool IsAyaneo2LogoRemapEnabled
        {
            get => isAyaneo2LogoRemapEnabled; set
            {
                isAyaneo2LogoRemapEnabled = value; RaisePropertyChanged();
            }
        }

        public int ReadingTDP
        {
            get => readingTDP; set
            {
                readingTDP = value; RaisePropertyChanged();
            }
        }

        public int ApplyTDP
        {
            get => applyTDP; set
            {
                applyTDP = value; RaisePropertyChanged();
            }
        }

        public int[] PresetTDP
        {
            get => presetTDP; set
            {
                presetTDP = value; RaisePropertyChanged();
            }
        }

    }

}

using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

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
        private int readingFPS;
        private int minTDP;
        private int maxTDP;
        private int applyTDP;
        private ObservableCollection<int> presetTDP;
        private int presetTDPIndex;
        private bool isEditModeEnabled;
        private bool isAyaneo2LogoRemapEnabled;
        private int mainWindowTop;
        private int mainWindowLeft;

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

        public ObservableCollection<int> PresetTDP
        {
            get { return presetTDP; }
            set { presetTDP = value; RaisePropertyChanged(); }
        }

        public int PresetTDPIndex
        {
            get => presetTDPIndex; set
            {
                presetTDPIndex = value; RaisePropertyChanged();
            }
        }
        public int MainWindowTop
        {
            get => mainWindowTop; set
            {
                mainWindowTop = value; RaisePropertyChanged();
            }
        }

        public int MainWindowLeft
        {
            get => mainWindowLeft; set
            {
                mainWindowLeft = value; RaisePropertyChanged();
            }
        }

    }

}

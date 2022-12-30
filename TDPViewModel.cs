using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PocketTDPControl
{
    public class TDPViewModel:ViewModelBase
    {
        private int currentTDP;
        private int presetTDP;
        private int readingTDP;

        public int CurrentTDP { get => currentTDP; set
            {
                currentTDP = value; RaisePropertyChanged();
            }
        }
        public int PresetTDP
        {
            get => presetTDP; set
            {
                presetTDP = value; RaisePropertyChanged();
            }
        }

        public int ReadingTDP
        {
            get => readingTDP; set
            {
                readingTDP = value; RaisePropertyChanged();
            }
        }

    }

}

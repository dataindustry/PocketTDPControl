using OpenHardwareMonitor.Hardware;
using System;

namespace PocketTDPControl
{
    internal class HardwareMonitor
    {
        private TDPViewModel viewModel;

        /**
         * Init OpenHardwareMonitor.dll Computer Object
         **/

        public Computer computer = new Computer()
        {
            GPUEnabled = true,
            CPUEnabled = true,
            // RAMEnabled = true,
            MainboardEnabled = true,
            // FanControllerEnabled = true,
            // HDDEnabled = true
        };

        public HardwareMonitor(TDPViewModel viewModel)
        {
            this.viewModel = viewModel;
            computer.Open();
        }

        /**
         * Pulls data from OHM
         **/

        public void Update()
        {
            foreach (var hardware in computer.Hardware)
            {

                if(hardware.HardwareType== HardwareType.Mainboard) {

                    hardware.Update();
                    this.viewModel.MachineName = hardware.Name;
                }

                if (hardware.HardwareType == HardwareType.CPU)
                {
                    hardware.Update();
                    this.viewModel.CpuName= hardware.Name;

                    foreach (var sensor in hardware.Sensors)

                        if (sensor.SensorType == SensorType.Power && sensor.Name.Contains("CPU Package"))
                        {
                            this.viewModel.ReadingTDP = (int)sensor.Value;
                        }

                }

                if (hardware.HardwareType == HardwareType.GpuAti)
                {
                    hardware.Update();
                    this.viewModel.IsGpuAti = true;
                }

            }
        }

    }
}

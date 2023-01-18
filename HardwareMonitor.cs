using LibreHardwareMonitor.Hardware;
using LibreHardwareMonitor.Hardware.CPU;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Documents;

namespace PocketTDPControl
{
    internal class HardwareMonitor
    {
        private ViewModel viewModel;

        /**
         * Init OpenHardwareMonitor.dll Computer Object
         **/

        public Computer computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsMotherboardEnabled = true,
            IsControllerEnabled = true,
            IsNetworkEnabled = false,
            IsStorageEnabled = true
        };

        public HardwareMonitor(ViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.computer.Open();
        }

        /**
         * Pulls data from OHM
         **/

        public void Update()
        {

            var cpuClock = new List<int>();

            foreach (var hardware in computer.Hardware)
            {
                try
                {
                    hardware.Update();
                }
                catch (Exception)
                {
                    continue;
                }

                if(hardware.HardwareType== HardwareType.Motherboard) {

                    this.viewModel.MachineName = hardware.Name;
                }

                if (hardware.HardwareType == HardwareType.Memory)
                {
                    foreach (var sensor in hardware.Sensors) {

                        if (sensor.SensorType == SensorType.Data && sensor.Name.Contains("Memory Available"))
                        {
                            this.viewModel.MemoryAvailable = sensor.Value != null ? 0 : (int)sensor.Value;
                        }

                        if (sensor.SensorType == SensorType.Data && sensor.Name.Contains("Memory Used"))
                        {
                            this.viewModel.MemoryUsed = sensor.Value != null ? 0 : (int)sensor.Value;
                        }
                    }

                }

                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    this.viewModel.CpuName = hardware.Name;

                    foreach (var sensor in hardware.Sensors)
                    {

                        if (sensor.SensorType == SensorType.Power && sensor.Name.Contains("Package"))
                        {
                            this.viewModel.ReadingCpuTDP = sensor.Value == null? 0 : (int)sensor.Value;
                        }

                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("Core"))
                        {
                            this.viewModel.CpuTemperture = (int)sensor.Value;
                        }

                        if (sensor.SensorType == SensorType.Clock && sensor.Name.Contains("Core"))
                        {
                            cpuClock.Add((int)sensor.Value);
                        }

                        // sum power pre core as cpu only power
                    }

                }

                if (hardware.HardwareType == HardwareType.GpuAmd)
                {
                    this.viewModel.GpuName = hardware.Name;

                    foreach (var sensor in hardware.Sensors) {

                        if (sensor.SensorType == SensorType.Power && sensor.Name.Contains("GPU Package"))
                        {
                            this.viewModel.ReadingGpuTDP = (int)sensor.Value;
                        }

                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("GPU VR SoC"))
                        {
                            this.viewModel.GpuTemperture = (int)sensor.Value;
                        }

                        if (sensor.SensorType == SensorType.Clock && sensor.Name.Contains("GPU Core"))
                        {
                            this.viewModel.GpuClock = (int)sensor.Value;
                        }

                        if (sensor.SensorType == SensorType.Factor && sensor.Name.Contains("Fullscreen FPS"))
                        {
                            this.viewModel.ReadingFPS = (int)sensor.Value;
                        }
                    }

                }

                if(cpuClock.Count> 0)
                    this.viewModel.CpuClock = cpuClock.Max();

            }
        }

    }
}

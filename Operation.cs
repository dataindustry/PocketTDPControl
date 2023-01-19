using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace PocketTDPControl
{
    internal class Operation
    {

        public static bool Adjust(string type, int tdp)
        {

            Process p = new Process();

            p.StartInfo.FileName = "./ryzenadj/ryzenadj.exe";
            p.StartInfo.Arguments = $"-{type} {tdp * 1000}";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;

            p.Start();

            var successResult = p.StandardOutput.ReadToEnd();

            p.Close();
            p.Dispose();

            if (successResult.StartsWith("Sucessfully set"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool LoopbackExempt()
        {
            RegistryKey rkConfig = Registry.CurrentUser.OpenSubKey("Software\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\CurrentVersion\\AppContainer\\Mappings\\");

            var keys = rkConfig.GetSubKeyNames();

            foreach (string key in keys)
            {

                if (rkConfig.OpenSubKey(key).GetValue("DisplayName").ToString() == "PocketTDPControlWidget")
                {
                    Process p = new Process();

                    p.StartInfo.FileName = "CheckNetIsolation";
                    p.StartInfo.Arguments = $"LoopbackExempt -a -p={key}";
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardInput = true;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.StartInfo.RedirectStandardError = true;
                    p.StartInfo.CreateNoWindow = true;

                    p.Start();

                    string error = p.StandardError.ReadToEnd();

                    p.Close();

                    if (error.Trim() == "")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            return false;
        }

        /// <summary>
        /// START keyboard mapping
        /// </summary>
        private static Dictionary<int, bool> FromComboKey = new Dictionary<int, bool>();
        private static VirtualKeyCode ToModifierKey, ToKey = 0;
        private static KeyboardHook KBH = new KeyboardHook();
        private static void KeyboardHookKeyPress(KeyboardHook.HookStruct hookStruct, out bool handle)
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
        public static void InitialKeyboardHook() {
            KBH.InstallHook(KeyboardHookKeyPress);
        }
        public static void CustomKeyMapping(Keys[] fromComboKey, VirtualKeyCode[] toComboKey)
        {
            KBH.InstallHook(KeyboardHookKeyPress);

            foreach (var key in fromComboKey)
            {

                var keyCode = (int)key;

                if (Operation.FromComboKey.ContainsKey(keyCode))
                {
                    Operation.FromComboKey[keyCode] = false;
                }
                else { Operation.FromComboKey.Add(keyCode, false); }

            }

            Operation.ToModifierKey = toComboKey[0];
            Operation.ToKey = toComboKey[1];

        }
        public static void CancelCustomKeyMapping() {

            FromComboKey.Clear();
            ToModifierKey = 0;
            ToKey = 0;

        }
        public static void DisposeKeyboardHook() {

            KBH?.UninstallHook();

        }
        // END keyboard mapping

        /// <summary>
        /// START Ayaneo 2 fan control via ec direct r/w
        /// </summary>
        public static Ols ols = null;
        public static ushort reg_addr = 78;
        public static ushort reg_data = 79;
        public static event EventHandler OlsInitFailedEvent;
        public static float GetAyaneo2FanSpeedPrecentage() => (float)ECRamDirectRead((ushort)6153);
        public static void SetAyaneo2FanSpeedPrecentage(byte fanSpeedPrecentage) => ECRamDirectWrite((ushort)1099, fanSpeedPrecentage);
        public static void SetAyaneo2FanSpeedToAutoControl() => ECRamDirectWrite((ushort)1098, (byte)0);
        public static void SetAyaneo2FanSpeedToManualControl() => ECRamDirectWrite((ushort)1098, (byte)1);
        public static byte ECRamDirectRead(ushort address)
        {
            if (ols == null)
                OlsInit();
            if (ols == null)
                return 0;
            byte num1 = (byte)((int)address >> 8 & (int)byte.MaxValue);
            byte num2 = (byte)((uint)address & (uint)byte.MaxValue);
            try
            {
                reg_addr = (ushort)78;
                reg_data = (ushort)79;
                ols.WriteIoPortByte(reg_addr, (byte)46);
                ols.WriteIoPortByte(reg_data, (byte)17);
                ols.WriteIoPortByte(reg_addr, (byte)47);
                ols.WriteIoPortByte(reg_data, num1);
                ols.WriteIoPortByte(reg_addr, (byte)46);
                ols.WriteIoPortByte(reg_data, (byte)16);
                ols.WriteIoPortByte(reg_addr, (byte)47);
                ols.WriteIoPortByte(reg_data, num2);
                ols.WriteIoPortByte(reg_addr, (byte)46);
                ols.WriteIoPortByte(reg_data, (byte)18);
                ols.WriteIoPortByte(reg_addr, (byte)47);
                return ols.ReadIoPortByte(reg_data);
            }
            catch
            {
                OlsFree();
                return 0;
            }
        }
        public static void ECRamDirectWrite(ushort address, byte data)
        {
            if (ols == null)
                OlsInit();
            if (ols == null)
                return;
            byte num1 = (byte)((int)address >> 8 & (int)byte.MaxValue);
            byte num2 = (byte)((uint)address & (uint)byte.MaxValue);
            try
            {
                reg_addr = (ushort)78;
                reg_data = (ushort)79;
                ols.WriteIoPortByte(reg_addr, (byte)46);
                ols.WriteIoPortByte(reg_data, (byte)17);
                ols.WriteIoPortByte(reg_addr, (byte)47);
                ols.WriteIoPortByte(reg_data, num1);
                ols.WriteIoPortByte(reg_addr, (byte)46);
                ols.WriteIoPortByte(reg_data, (byte)16);
                ols.WriteIoPortByte(reg_addr, (byte)47);
                ols.WriteIoPortByte(reg_data, num2);
                ols.WriteIoPortByte(reg_addr, (byte)46);
                ols.WriteIoPortByte(reg_data, (byte)18);
                ols.WriteIoPortByte(reg_addr, (byte)47);
                ols.WriteIoPortByte(reg_data, data);
            }
            catch
            {
                OlsFree();
            }
        }
        public static bool OlsInit()
        {
            bool flag = false;
            ols = new Ols();
            switch (ols.GetStatus())
            {
                case 0:
                    flag = true;
                    break;
            }
            switch (ols.GetDllStatus())
            {
                case 0:
                    flag = true;
                    break;
                case 1:
                    int num1 = (int)MessageBox.Show("WingRing0 DLL Status Error!! OLS_UNSUPPORTED_PLATFORM");
                    OlsFree();
                    break;
                case 2:
                    int num2 = (int)MessageBox.Show("WingRing0 DLL Status Error!! OLS_DRIVER_NOT_LOADED");
                    OlsFree();
                    break;
                case 3:
                    int num3 = (int)MessageBox.Show("WingRing0 DLL Status Error!! OLS_DLL_DRIVER_NOT_FOUND");
                    OlsFree();
                    break;
                case 4:
                    int num4 = (int)MessageBox.Show("WingRing0 DLL Status Error!! OLS_DLL_DRIVER_UNLOADED");
                    OlsFree();
                    break;
                case 5:
                    int num5 = (int)MessageBox.Show("WingRing0 DLL Status Error!! DRIVER_NOT_LOADED_ON_NETWORK");
                    OlsFree();
                    break;
                case 9:
                    int num6 = (int)MessageBox.Show("WingRing0 DLL Status Error!! OLS_DLL_UNKNOWN_ERROR");
                    OlsFree();
                    break;
            }
            if (ols != null)
                return flag;
            RaiseOlsInitFailedEvent();
            return false;
        }
        public static void OlsFree()
        {
            if (ols != null)
                ols.Dispose();
            ols = (Ols)null;
        }
        public static void RaiseOlsInitFailedEvent()
        {
            if (OlsInitFailedEvent == null)
                return;
            OlsInitFailedEvent((object)null, (EventArgs)new OlsInitFailedEventArgs());
        }
        // END Ayaneo 2 fan control via ec direct r/w
    }
}

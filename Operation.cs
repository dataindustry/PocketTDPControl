using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace PocketTDPControl
{
    internal class Operation
    {

        public static Dictionary<int, bool> FromComboKey = new Dictionary<int, bool>();

        public static VirtualKeyCode ToModifierKey, ToKey;
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
                    Console.WriteLine(key);

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

        public static void RemapComboKey(Keys[] fromComboKey, VirtualKeyCode[] toComboKey)
        {

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

        public static void KeyboardHookKeyPress(KeyboardHook.HookStruct hookStruct, out bool handle)
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
    }
}

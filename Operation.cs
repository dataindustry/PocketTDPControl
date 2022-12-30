using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketTDPControl
{
    internal class Operation
    {
        public static bool Adjust(string type, int tdp)
        {

            Process p = new Process();

            p.StartInfo.FileName = "ryzenadj.exe";
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
    }
}

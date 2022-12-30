using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PocketTDPControl
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    public class MainService : IMainService
    {
        public void Adjust(string target, int tdp)
        {
            Console.Out.WriteLine(target+ " " + tdp);
            Operation.Adjust(target, tdp);
        }
    }
}

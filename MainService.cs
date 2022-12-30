using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PocketTDPControl
{
    public class MainService : IMainService
    {
        public void Adjust(string target, int tdp)
        {
            Operation.Adjust(target, tdp);
        }
    }
}

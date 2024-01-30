using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class FIS
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public string Process { get; set; }
        public string Station { get; set; }
        public bool pass { get; set; }
    }
}

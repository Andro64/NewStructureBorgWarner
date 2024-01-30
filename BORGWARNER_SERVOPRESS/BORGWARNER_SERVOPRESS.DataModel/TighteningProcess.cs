using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class TighteningProcess
    {
        public string id { get; set; }
        public bool result { get; set; }
        public string resultResponse { get; set; }
        public bool status { get; set; }
        public string Angle { get; set; }
        public string Torque { get; set; }
    }
}

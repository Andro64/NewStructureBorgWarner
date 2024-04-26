using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class RunHistory
    {
        public int id { get; set; }
        public string partnumber { get; set; }
        public string serial { get; set; }
        public string serial2 { get; set; }
        public DateTime date { get; set; }
        public string result { get; set; }
        public string screw1Torque { get; set; }
        public string screw1Angle { get; set; }
        public string screw2Torque { get; set; }
        public string screw2Angle { get; set; }
        public string screw3Torque { get; set; }
        public string screw3Angle { get; set; }
        public string screw4Torque { get; set; }
        public string screw4Angle { get; set; }
        public string MyProperty { get; set; }
        public string screw5Torque { get; set; }
        public string screw5Angle { get; set; }
        public string ultraboardpadinspection { get; set; }
        public string ultraboardcapinspection { get; set; }
        public string insulatorinspection { get; set; }       
    }
}

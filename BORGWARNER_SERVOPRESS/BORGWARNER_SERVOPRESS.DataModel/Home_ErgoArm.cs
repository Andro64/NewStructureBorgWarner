using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class Home_ErgoArm
    {
        public double Encoder1 { get; set; }
        public double Encoder2 { get; set; }
        public double Tolerance { get; set; }
        public double Encoder1_Max { get; set; }
        public double Encoder1_Min { get; set; }
        public double Encoder2_Max { get; set; }
        public double Encoder2_Min { get; set; }

    }
}

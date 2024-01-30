using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class PositionErgoArm
    {
        public double encoder1 { get; set; }
        public double encoder2 { get; set; }
        public bool InHome { get; set; }
        public bool InPositionReadyToProcess { get; set; }
    }
}

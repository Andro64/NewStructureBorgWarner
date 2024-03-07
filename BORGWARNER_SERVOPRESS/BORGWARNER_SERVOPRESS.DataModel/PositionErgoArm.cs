using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class PositionErgoArm
    {
        public double encoder1 { get; set; } = 0;
        public double encoder2 { get; set; } = 0;
        public bool InHome { get; set; }
        public bool InPositionReadyToProcess { get; set; }
    }
}

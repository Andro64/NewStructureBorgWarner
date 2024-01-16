using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class Devices
    {        
        public List<Camara> lstCamaras { get; set; }
        public List<EpsonRobot> lstRobots { get; set; }
        public List<Screwdriver> lstScrewdrivers { get; set; }
        public List<ErgoArm> lstErgoArms { get; set; }
        public List<FIS> lstFISs { get; set; }


    }
}

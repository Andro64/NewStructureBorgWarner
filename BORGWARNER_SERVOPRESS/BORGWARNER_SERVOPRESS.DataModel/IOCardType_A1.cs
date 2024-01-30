using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class IOCardType_A1
    {

        #region Input                      
        public bool Pressure_Sensor { get; set; }
        public bool Prestoper_Pallet_Present { get; set; }
        public bool Stopper_Pallet_Present { get; set; }
        public bool PA3 { get; set; }
        public bool ToolActive { get; set; }
        public bool PB1 { get; set; }
        public bool PB2 { get; set; }
        public bool E_Stop_Active { get; set; }
        #endregion

        #region OutPuts
        public bool Lamp_Grn { get; set; }
        public bool Lamp_Wht { get; set; }
        public bool Lamp_Red { get; set; }
        public bool K3_1 { get; set; }
        public bool Cyl_Stopper_Act { get; set; }
        public bool Cyl_Prestopper_Act { get; set; }
        public bool K6_1 { get; set; }
        public bool Reset_Safety { get; set; }      
        #endregion
    }
}

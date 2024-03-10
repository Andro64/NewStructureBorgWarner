using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class IOCardType_M1
    {
        #region InPuts     
        public bool Main_Pressure { get; set; }
        public bool OptoBtn { get; set; }
        public bool Pallet_Pre_Stopper { get; set; }
        public bool Pallet_Stopper { get; set; }
        public bool Screw_Present_Oth { get; set; }
        public bool Screw_Level_Oth { get; set; }
        public bool MaskInHolder { get; set; }
        public bool SecurityOK { get; set; }
        #endregion

        #region OutPuts
        public bool Opto_Grn { get; set; }
        public bool Opto_Yllw { get; set; }
        public bool Opto_Red { get; set; }
        public bool Reset_Signal { get; set; }
        public bool K4 { get; set; }
        public bool ScrewDispenser { get; set; }
        public bool Vacuum { get; set; }
        public bool K7 { get; set; }             
        #endregion
    }
}

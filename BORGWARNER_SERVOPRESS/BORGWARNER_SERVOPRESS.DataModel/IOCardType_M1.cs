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
        public bool PA0 { get; set; }
        public bool OptoBtn { get; set; }
        public bool PalletatPreStation { get; set; }
        public bool PalletatStation { get; set; }
        public bool PB0 { get; set; }
        public bool PB1 { get; set; }
        public bool MaskatHolder { get; set; }
        public bool SecurityOK { get; set; }
        #endregion

        #region OutPuts
        public bool Opto_Grn { get; set; }
        public bool Opto_Yllw { get; set; }
        public bool Opto_Red { get; set; }
        public bool Reset_Signal { get; set; }
        public bool K4 { get; set; }
        public bool K5 { get; set; }
        public bool ScrewDispenser { get; set; }
        public bool K7 { get; set; }             
        #endregion
    }
}

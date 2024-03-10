using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class IOCardType_M2
    {
        #region InPuts     
        public bool Trigger_Scanner { get; set; }
        public bool MaskatHousing { get; set; }
        public bool PA2 { get; set; }
        public bool PA3 { get; set; }
        public bool Cyl_Fixing_Pall_Ext { get; set; }
        public bool Cyl_Fixing_Pall_Ret { get; set; }
        public bool PB2 { get; set; }
        public bool PB3 { get; set; }
        #endregion

        #region OutPuts
        public bool PalletFixingExt { get; set; }
        public bool PalletFixingRet { get; set; }
        public bool Cyl_Stopper { get; set; }
        public bool Cyl_Pres_Stopper { get; set; }
        public bool K4 { get; set; }
        public bool K5 { get; set; }
        public bool K6 { get; set; }
        public bool K7 { get; set; }
        #endregion
    }
}

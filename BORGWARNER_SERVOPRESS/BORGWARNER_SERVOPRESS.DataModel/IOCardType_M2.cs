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
        public bool HousingatPallet { get; set; }
        public bool MaskatHousing { get; set; }
        public bool UltraCapBoardReadytoScan { get; set; }
        public bool PA3 { get; set; }
        public bool CylFixingExtd { get; set; }
        public bool CylFixingRetd { get; set; }
        public bool PB2 { get; set; }
        public bool PB3 { get; set; }
        #endregion

        #region OutPuts
        public bool CylFixingExt { get; set; }
        public bool CylFixingRet { get; set; }
        public bool PalletStopperRet { get; set; }
        public bool Prestopper { get; set; }
        public bool K4 { get; set; }
        public bool K5 { get; set; }
        public bool K6 { get; set; }
        public bool K7 { get; set; }
        #endregion
    }
}

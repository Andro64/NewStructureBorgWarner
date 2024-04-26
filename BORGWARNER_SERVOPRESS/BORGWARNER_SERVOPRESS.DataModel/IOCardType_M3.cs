using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class IOCardType_M3
    {
        #region InPuts     
        public bool PA0 { get; set; }
        public bool PA1 { get; set; }
        public bool PA2 { get; set; }
        public bool ST13Available { get; set; }
        public bool PB0 { get; set; }
        public bool PB1 { get; set; }
        public bool Scrap_presence { get; set; }
        public bool PB3 { get; set; }
        #endregion

        #region OutPuts
        public bool ST12Available { get; set; }
        public bool LampRCam { get; set; }
        public bool LampLCam { get; set; }
        public bool K3 { get; set; }
        public bool K4 { get; set; }
        public bool K5 { get; set; }
        public bool K6 { get; set; }
        public bool ReleScrap { get; set; }
        #endregion
    }
}

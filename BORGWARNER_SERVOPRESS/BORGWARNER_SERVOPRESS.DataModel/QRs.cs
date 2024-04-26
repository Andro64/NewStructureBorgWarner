using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class QRs
    {
        public string scan1 { get; set; } = string.Empty;
        public string scan2 { get; set; } = string.Empty;
        public string HOUSING { get; set; }
        public string HVDC_BUSBAR { get; set; }
        public string HARNESS { get; set; }
        public string TOP_COVER { get; set; }
        public string From_FIS { get; set; }
        public string To_FIS { get; set; }
    }
}

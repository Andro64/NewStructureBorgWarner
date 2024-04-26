using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class Programs_ScrewDriver
    {
        public int id { get; set; }
        public int id_model_screw { get; set; }
        public string screwing { get; set; }
        public string rescrewing { get; set; }
        public string unscrewing { get; set; }
        public string simulated { get; set; }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class Screw
    {
        public int id { get; set; }
        public string Tornillo { get; set; }
        public string Encoder1 { get; set; }
        public string Encoder2 { get; set; }
        public string Tolerancia { get; set; }
        public string id_Name { get; set; }        
    }
}

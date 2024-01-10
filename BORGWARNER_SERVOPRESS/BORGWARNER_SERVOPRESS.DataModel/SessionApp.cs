using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class SessionApp
    {        
        public string connStr { get; set; }
        public int statusCiclo { get; set; }
        public Settings settings { get; set; }
    }
}

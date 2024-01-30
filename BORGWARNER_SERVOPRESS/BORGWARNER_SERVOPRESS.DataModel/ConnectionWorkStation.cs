using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class ConnectionWorkStation
    {
        public int id { get; set; }
        public int idTypeDevice { get; set; }
        public string TypeDevice { get; set; }
        public string IP { get; set; }             
        public int Port { get; set; }
        public int idTypeConnection { get; set; }
        public string TypeConnection { get; set; }

    }
}


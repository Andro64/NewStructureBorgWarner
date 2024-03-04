using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class CommandCamara
    {
        public int id { get; set; }
        public int id_type_connection { get; set; }
        public string des_type_connection { get; set; }
        public int id_type_camara { get; set; }
        public string des_type_camara { get; set; }
        public int id_connections_by_workstation { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public string command_user { get; set; }
        public string command_setstring { get; set; }
        public string command_setevent { get; set; }
        public string command_getvalue_test { get; set; }
        public string command_getvalue_real { get; set; }
        public string command_getjob { get; set; }
        public string command_setjob { get; set; }
        public string command_getvalue_test_1 { get; set; }
        public string command_getvalue_real_1 { get; set; }
        public string command_getvalue_test_2 { get; set; }
        public string command_getvalue_real_2 { get; set; }
    }
}

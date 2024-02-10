using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public enum eTypeConnection
    {
        Main = 1,
        Auxiliary = 2,
        InputDevices = 3,
        Emergency = 4,
        Scan_1 = 5,
        Scan_2 = 6,
        CognexD900 = 7,
        CognexD2800 = 8

    }
    public enum eTypeDevices
    {
        ErgoArm = 1,
        Screw = 2,
        RobotEpson = 3,
        Camara = 4,
        Scanner = 5,
        FIS = 6
    }
    public class Enums
    {
        public eTypeConnection e_TypeConnection { get; set; }
        public eTypeDevices e_TypeDevices { get; set; }        
    }

    public enum eMessageBoxResult
    {
        OK,
        Cancel
    }

    public enum eMessageBoxIcon
    {
        None,
        Information,
        Question,
        Warning,
        Error
    }
}

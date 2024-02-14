using BORGWARNER_SERVOPRESS.DataModel.Views;
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
        public List<Settings> settings { get; set; }
        public User user { get; set; }
        public List<ConnectionWorkStation> connectionsWorkStation { get; set; }
        public IOCardType_M1 Sensors_M1 { get; set; } = new IOCardType_M1();
        public IOCardType_M2 Sensors_M2 { get; set; } = new IOCardType_M2();
        public IOCardType_M3 Sensors_M3 { get; set; } = new IOCardType_M3();
        public QRs QR { get; set; }
        public string PathImageResultFromCamera { get; set; }
        public int ModelScrewSelected { get; set; }
        public PositionErgoArm positionErgoArm { get; set; }        
        public string MessageOfProcess { get; set; }
        public string ImageOfProcess { get; set; }
        public string PathDirectoryResourcesOfThisProyect { get; set; }
        public string PathOperationalImages { get; set; }
        public List<TotalRegistersByTables> lstTotalRegistersByTables { get; set; }
        public bool TaksRunExecuting { get; set; }        
        public ModelViewTypeWorkstation typeWorkstation { get; set; }
}
}

using BORGWARNER_SERVOPRESS.DataModel.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class SessionApp
    {
        public string connStr { get; set; }
        public int statusCiclo { get; set; }
        public List<Settings> settings { get; set; }
        public List<CommandCamara> commandCamaras { get; set; }
        public User user { get; set; }
        public List<ConnectionWorkStation> connectionsWorkStation { get; set; }
        public IOCardType_M1 Sensors_M1 { get; set; } = new IOCardType_M1();
        public IOCardType_M2 Sensors_M2 { get; set; } = new IOCardType_M2();
        public IOCardType_M3 Sensors_M3 { get; set; } = new IOCardType_M3();
        public QRs QR { get; set; } = new QRs();
        public string PathImageResultFromCamera { get; set; }
        public int ModelScrewSelected { get; set; }
        public PositionErgoArm positionErgoArm { get; set; } = new PositionErgoArm();
        public string MessageOfProcess { get; set; }
        public bool OnlyMessageOfProcess { get; set; }
        public string ImageOfProcess { get; set; }
        public BitmapImage BitmapImageOfProcess { get; set; }
        public string PathDirectoryResourcesOfThisProyect { get; set; }
        public string PathOperationalImages { get; set; }
        public List<TotalRegistersByTables> lstTotalRegistersByTables { get; set; }
        public bool TaksRunExecuting { get; set; }
        public ModelViewTypeWorkstation typeWorkstation { get; set; }
        public bool sensorToCheck {get;set;}
        public TypeExecutionScrew typeExecutionScrew { get; set; } = new TypeExecutionScrew();
        public Programs_ScrewDriver programs_ScrewDriver { get; set; }
        public string messageTorque { get; set; }
        public bool isScrewingFinished { get; set; }
        public bool areImagePASSProcessFinished { get; set; }
        public List<string> images { get; set; } = new List<string>();
    }
}

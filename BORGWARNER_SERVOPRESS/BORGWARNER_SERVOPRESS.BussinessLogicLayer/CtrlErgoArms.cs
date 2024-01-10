using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class CtrlErgoArms
    {
        SessionApp sessionApp;
        public CtrlErgoArms(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }

        public void screwingCycle()
        {
          
        }
        public void Ejecutatorque()
        {
            ErgoArm ergoArm = new ErgoArm(sessionApp);            

            ergoArm.connectingRobot();
            if(ergoArm.isRobotConnected())
            {
                if(ergoArm.controllerConnectionInitation() == "0002")
                {
                    ergoArm.ScrewingProgram_by_Model("modelo1", true, false);
                    if(ergoArm.enableScrewdriver()== "0005")
                    {
                        if(ergoArm.screwingSubscription() =="0005")
                        {
                            Debug.WriteLine("Se ejecutado atornillador");
                        }
                    }
                }
            }
            ergoArm.disconnectingRobot();
        }
    }
}

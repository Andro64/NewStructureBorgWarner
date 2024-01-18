﻿using BORGWARNER_SERVOPRESS.DataModel;
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
        Views.ViewMain viewMain;
        public CtrlErgoArms(SessionApp _sessionApp, Views.ViewMain _viewMain)
        {
            sessionApp = _sessionApp;
            viewMain = _viewMain;
        }
        

        public async Task Ejecutatorque(IProgress<string> progress)
        {
            ErgoArm ergoArm = new ErgoArm(sessionApp, viewMain);
            try
            {
                Task.Run(async () =>
                {
                    await ergoArm.startReadSensors(progress);
                }).Wait();

                ergoArm.connectingRobot();
                if (ergoArm.isRobotConnected())
                {
                    if (ergoArm.controllerConnectionInitation() == "0002")
                    {
                        ergoArm.ScrewingProgram_by_Model("modelo1", true, false);
                        if (ergoArm.enableScrewdriver() == "0005")
                        {
                            if (ergoArm.screwingSubscription() == "0005")
                            {
                                Task.Run(async () =>
                                {
                                   await viewMain.getStatusScrew("Se ejecutado atornillador");
                                }).Wait();

                                Debug.WriteLine("Se ejecutado atornillador");
                            }
                        }
                    }
                }
                //ergoArm.disconnectingRobot();
            }
            catch (Exception ex)
            {
                viewMain.getStatusScrew("Error: " + ex.Message);
                Debug.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                ergoArm.endReadSensors();
                ergoArm.disconnectingRobot();
            }
        }
    }
}

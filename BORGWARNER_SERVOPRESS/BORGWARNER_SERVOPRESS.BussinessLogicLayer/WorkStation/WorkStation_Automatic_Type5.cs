using BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation
{
    public class WorkStation_Automatic_Type5 : Workstation
    {
        public override string Type => @"
                                        ╭∩╮( •̀_•́ )╭∩╮ \n 
                                       WS Automatica Tipo 5 \n
  (\    |@@|
(__/\__ \--/ __
   \___|----|  |   __
       \ }{ /\ )_ / _\
       /\__/\ \__O (__
      (--/\--)    \__/
      _)(  )(_
     `---''---`
                                       ";

        public override event EventHandler<TextBoxInfoEventArgs> CreateTextBoxRequested;
        public override event EventHandler RemoveTextBoxRequested;
        SensorsIOGeneric sensorsIO;
        SessionApp sessionApp;

        public WorkStation_Automatic_Type5(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            sensorsIO = new SensorsIOGeneric(sessionApp);
        }
        public override void CancelProcess()
        {
            throw new NotImplementedException();
        }


        public override void RequestCreateTextBox(string msg, int PositionX, int PositionY, bool HasError = false)
        {
            throw new NotImplementedException();
        }

        public override void RequestRemoveTextBox()
        {
            throw new NotImplementedException();
        }

        public override Task StartProcess()
        {
            throw new NotImplementedException();
        }
    }
}

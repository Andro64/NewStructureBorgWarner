using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation
{
    public class WorkStation_Automatic_Type3 : Workstation
    {
        public override string Type => "╭∩╮( •̀_•́ )╭∩╮ \n WS Automatica Tipo 3";

        public override event EventHandler<TextBoxInfoEventArgs> CreateTextBoxRequested;
        public override event EventHandler RemoveTextBoxRequested;

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

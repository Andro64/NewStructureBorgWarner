using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation
{
    public class WorkStation_Automatic_Type4 : Workstation
    {
        public override string Type => "╭∩╮( •̀_•́ )╭∩╮ \n WS Automatica Tipo 4";

        public override event EventHandler<TextBoxInfoEventArgs> CreateTextBoxRequested;
        public override event EventHandler RemoveTextBoxRequested;

        public override void CancelProcess()
        {
            throw new NotImplementedException();
        }

        public override void RequestCreateTextBox(string msg, int PositionX, int PositionY)
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

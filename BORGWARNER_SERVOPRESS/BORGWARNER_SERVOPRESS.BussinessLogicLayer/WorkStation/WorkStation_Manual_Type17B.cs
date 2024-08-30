using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation
{
    public class WorkStation_Manual_Type17B : Workstation
    {
        public override string Type => "(◕‿◕) \n Manual Tipo 17B";

        public override event EventHandler<TextBoxInfoEventArgs> CreateTextBoxRequested;
        public override event EventHandler RemoveTextBoxRequested;

        public override void CancelProcess()
        {
            throw new NotImplementedException();
        }

        public override void RequestCreateTextBox(string msg, int PositionX, int PositionY, int Width, int Height, bool HasError = false, eStyleText eStyleText = eStyleText.None)
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

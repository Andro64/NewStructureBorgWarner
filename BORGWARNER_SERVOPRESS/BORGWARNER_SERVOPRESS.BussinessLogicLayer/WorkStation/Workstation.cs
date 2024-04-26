using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation
{
    public abstract class Workstation
    {
        public abstract string Type { get; }

        public override string ToString()
        {
            return $"Workstation type: {Type}";
        }

        public abstract void CancelProcess();        
        public abstract Task StartProcess();
        public abstract void RequestCreateTextBox(string msg, int PositionX, int PositionY,bool HasError = false);        
        public abstract event EventHandler<TextBoxInfoEventArgs> CreateTextBoxRequested;
        public abstract void RequestRemoveTextBox();
        public abstract event EventHandler RemoveTextBoxRequested;


    }
}

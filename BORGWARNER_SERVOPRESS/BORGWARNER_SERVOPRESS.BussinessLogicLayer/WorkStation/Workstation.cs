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

        public void CancelProcess()
        {

        }
        public async Task StartProcess()
        {
        }
    }
}

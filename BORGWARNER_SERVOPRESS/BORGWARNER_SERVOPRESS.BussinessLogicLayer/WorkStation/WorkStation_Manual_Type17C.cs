using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation
{
    public class WorkStation_Manual_Type17C : Workstation
    {
        public override string Type => "(◕‿◕) \n Manual Tipo 17C"; 
        public override void CancelProcess()
        {
            throw new NotImplementedException();
        }
        public override Task StartProcess()
        {
            throw new NotImplementedException();
        }
     }
}

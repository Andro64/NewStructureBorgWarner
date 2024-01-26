using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class ErgoArm
    {
        SessionApp sessionApp;
        public ErgoArm(SessionApp _sessionApp, Views.ViewMain _viewMain)
        {
            sessionApp = _sessionApp;           
        }

    }
}

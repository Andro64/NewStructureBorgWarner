using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public interface ISensorObserver
    {
        void UpdateStatus(string sensorName, string newStatus);
    }
}

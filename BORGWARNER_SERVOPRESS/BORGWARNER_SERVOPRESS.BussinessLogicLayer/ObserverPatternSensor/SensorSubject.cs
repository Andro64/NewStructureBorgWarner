using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class SensorSubject
    {
        public event EventHandler<Tuple<string, string>> StatusChanged;

        public void NotifyStatusChange(string sensorName, string newStatus)
        {
            StatusChanged?.Invoke(this, Tuple.Create(sensorName, newStatus));
        }
    }
}

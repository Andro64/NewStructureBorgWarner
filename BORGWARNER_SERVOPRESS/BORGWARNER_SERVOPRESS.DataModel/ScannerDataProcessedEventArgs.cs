using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class ScannerDataProcessedEventArgs : EventArgs
    {
        public string Result { get; }

        public ScannerDataProcessedEventArgs(string result)
        {
            Result = result;
        }
    }
}

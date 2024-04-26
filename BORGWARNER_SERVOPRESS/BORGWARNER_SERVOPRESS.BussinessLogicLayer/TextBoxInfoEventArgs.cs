using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class TextBoxInfoEventArgs : EventArgs
    {
        public string Text { get; set; }
        public System.Windows.Point Position { get; set; }
        public bool HasError { get; set; }
    }
}

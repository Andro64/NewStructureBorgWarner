using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class ModelScrew
    {
        public int id { get; set; }
        public string partNumber { get; set; }
        public string serial { get; set; }
        public string name_model { get; set; }
        public string description { get; set; }
        public int quantity_screws { get; set; }

    }
}

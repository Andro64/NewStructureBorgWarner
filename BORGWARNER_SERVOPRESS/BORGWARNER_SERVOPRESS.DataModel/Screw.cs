using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class Screw
    {
        public int id { get; set; }
        public int id_screw { get; set; }
        public double encoder1 { get; set; }
        public double encoder2 { get; set; }
        public double tolerance { get; set; }
        public int id_model_screw { get; set; }
        public string desc_model { get; set; }
        public int text_position_X { get; set; }
        public int text_position_Y { get; set; }
        public TighteningProcess tighteningprocess { get; set; }

    }
}

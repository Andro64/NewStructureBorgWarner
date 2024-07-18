using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
	public class ADUPort
	{
		public int id { get; set; }
		public string IOCard { get; set; }
		public string keySensor { get; set; }		
		public string label { get; set; }
		public bool Value { get; set; }
		public int id_ADU { get; set; }
		public int id_index { get; set; }
		public string id_routine { get; set; }
	}
}

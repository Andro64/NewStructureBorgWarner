﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class TotalRegistersByTables
    {
        public string  NameTable  { get; set; }
        public int TotalRegisters { get; set; }
        public int NumPages { get; set; }
        public int[] Pages { get; set; }
    }
}

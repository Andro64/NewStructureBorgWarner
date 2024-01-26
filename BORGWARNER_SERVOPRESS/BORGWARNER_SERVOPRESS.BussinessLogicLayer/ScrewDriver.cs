using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using MySql.Data.MySqlClient;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class ScrewDriver
    {
        SessionApp sessionApp;        
        public ScrewDriver(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            
        }
        
       
    }
}

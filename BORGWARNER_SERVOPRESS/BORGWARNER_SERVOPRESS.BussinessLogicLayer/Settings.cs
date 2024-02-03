using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class Settings
    {
        SessionApp sessionApp;
        CommunicationGeneral communication;
        public Settings(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            communication = new CommunicationGeneral(sessionApp);
        }
        public List<DataModel.Settings> getSettings()
        {            
            return communication.getSettings();
        }
        public List<ConnectionWorkStation> getConnections()
        {         
            return communication.getConnectionsDevices();
        }
        public List<TotalRegistersByTables> getTotalRegByTables()
        {         
            return communication.getTotalRegByTables();
        }
    }
}

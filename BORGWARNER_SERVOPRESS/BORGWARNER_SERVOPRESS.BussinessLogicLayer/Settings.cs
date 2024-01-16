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
        public Settings(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }
        public List<DataModel.Settings> getSettings()
        {
            CommunicationGeneral communication = new CommunicationGeneral(sessionApp);
            return communication.getSettings();
        }
    }
}

using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using System.Text.RegularExpressions;

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
        public List<DataModel.Settings> getSettings(int TypeWorkstation)
        {
            return communication.getSettings(TypeWorkstation);
        }
        public List<ConnectionWorkStation> getConnections(int TypeWorkstation)
        {         
            return communication.getConnectionsDevices(TypeWorkstation);
        }
        public List<TotalRegistersByTables> getTotalRegByTables()
        {         
            return communication.getTotalRegByTables();
        }
        public ModelViewTypeWorkstation getTypeWorksatiton()
        {
            //List<DataModel.Settings> settings = communication.getSettings();            
            //int idWS = int.Parse(settings.FirstOrDefault(x=>x.setting.Equals("TYPE_WORKSTATION")).valueSetting);
            //return communication.getModelViewTypeWorkstation_By_Id(idWS).FirstOrDefault();
            return communication.getWorkstation();
        }
        public string GetImageFolderName(string nameWorkstation)
        {
            return string.Concat(GetUpperAndDigits(nameWorkstation),@"\");
        }

        public string GetUpperAndDigits(string input)
        {
            // Expresión regular para buscar mayúsculas y dígitos
            Regex regex = new Regex("[A-Z0-9]");

            // Obtener los caracteres que coinciden con la expresión regular
            MatchCollection matches = regex.Matches(input);

            // Construir una nueva cadena con los caracteres encontrados
            string result = "";
            foreach (Match match in matches)
            {
                result += match.Value;
            }

            return result;
        }
    }
}

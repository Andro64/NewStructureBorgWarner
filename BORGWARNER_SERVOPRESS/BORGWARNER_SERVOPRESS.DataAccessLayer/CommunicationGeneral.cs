using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    public class CommunicationGeneral
    {
        SessionApp sessionApp;
        public CommunicationGeneral(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }

        public List<Settings> getSettings()
        {
            List<Settings> lstSettings = new List<Settings>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_SETTINGS");
                lstSettings = resultData.AsEnumerable().Select(row =>
                new Settings
                {
                    id = row.Field<int>("id"),
                    setting = row.Field<string>("setting"),
                    valueSetting = row.Field<string>("value_setting")                    
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return lstSettings;

        }
    }
}

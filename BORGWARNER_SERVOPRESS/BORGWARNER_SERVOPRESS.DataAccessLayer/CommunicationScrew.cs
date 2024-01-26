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
    class CommunicationScrew
    {
        SessionApp sessionApp;
        public CommunicationScrew(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }
        public List<Screw> getScrews()
        {
            List<Screw> lstScrews = new List<Screw>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_SCREWS");
                lstScrews = resultData.AsEnumerable().Select(row =>
                new Screw
                {
                    id = row.Field<int>("id"),
                    Tornillo = row.Field<string>("Tornillo"),
                    Encoder1 = row.Field<string>("Encoder1"),
                    Encoder2 = row.Field<string>("Encoder2"),
                    Tolerancia = row.Field<string>("Tolerancia"),
                    id_Name = row.Field<string>("id_Name")
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return lstScrews;

        }
    }
}

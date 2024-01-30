using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    public class CommunicationScrew
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
                    id_screw = row.Field<int>("id_screw"),
                    encoder1 = row.Field<double>("encoder1"),
                    encoder2 = row.Field<double>("encoder2"),
                    tolerance = row.Field<double>("tolerance"),
                    id_model_screw = row.Field<int>("id_model_screw"),
                    desc_model = row.Field<string>("name_model")
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return lstScrews;
        }
        public List<ModelScrew> getModelsScrew()
        {
            List<ModelScrew> lstScrews = new List<ModelScrew>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_MODELS_SCREWS");
                lstScrews = resultData.AsEnumerable().Select(row =>
                new ModelScrew
                {
                    id = row.Field<int>("id"),
                    partNumber = row.Field<string>("partNumber"),
                    serial = row.Field<string>("serial"),
                    name_model = row.Field<string>("name_model"),
                    description = row.Field<string>("description"),
                    quantity_screws = row.Field<int>("quantity_screws")
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

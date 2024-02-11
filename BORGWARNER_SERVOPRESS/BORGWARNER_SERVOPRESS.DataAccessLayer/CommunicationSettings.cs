using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;


namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    public class CommunicationSettings
    {
        private SessionApp sessionApp;
        private int numRegbyPages;
        public CommunicationSettings(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            numRegbyPages = int.Parse(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("GRID_Number_Reg_by_Page")).valueSetting);
        }

        public List<ModelViewSettings> getModelViewSettings(int indexPage)
        {
            List<ModelViewSettings> lstScrews = new List<ModelViewSettings>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_SETTINGS_PAG", new MySqlParameter[] {
                    new MySqlParameter("page_p", MySqlDbType.Int32) { Value = indexPage },
                    new MySqlParameter("size_p", MySqlDbType.Int32) { Value = numRegbyPages } });
                lstScrews = resultData.AsEnumerable().Select(row =>
                new ModelViewSettings
                {
                    id = row.Field<int>("id"),
                    setting = row.Field<string>("setting"),
                    valueSetting = row.Field<string>("value_setting")                    
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstScrews;
        }

        public void Ins_Upd_ModelViewSettings(ModelViewSettings ModelViewSettings)
        {
            try
            {
                MYSQL_DB mySQL = new MYSQL_DB(sessionApp.connStr);

                mySQL.ExecuteNonQuerySP("SP_INS_UPD_SETTINGS", new MySqlParameter[] {
                    new MySqlParameter("p_id", MySqlDbType.Int32) { Value = ModelViewSettings.id },
                    new MySqlParameter("p_setting", MySqlDbType.VarChar) { Value = ModelViewSettings.setting },
                    new MySqlParameter("p_value_setting", MySqlDbType.VarChar) { Value = ModelViewSettings.valueSetting }                    
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
        }
        public void Del_ModelViewSettings(ModelViewSettings ModelViewSettings)
        {
            try
            {
                MYSQL_DB mySQL = new MYSQL_DB(sessionApp.connStr);

                mySQL.ExecuteNonQuerySP("SP_DEL_SETTINGS", new MySqlParameter[] {
                    new MySqlParameter("p_id", MySqlDbType.Int32) { Value = ModelViewSettings.id }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
        }
        public List<ModelViewTypeWorkstation> getModelViewTypeWorkstation()
        {
            List<ModelViewTypeWorkstation> lstScrews = new List<ModelViewTypeWorkstation>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_TYPE_WORKSTATION");
                lstScrews = resultData.AsEnumerable().Select(row =>
                new ModelViewTypeWorkstation
                {
                    id = row.Field<int>("id"),
                    description = row.Field<string>("description")
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
                throw;
            }
            return lstScrews;
        }
        
    }
}

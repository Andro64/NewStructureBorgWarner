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
    public class CommunicationRunHistory
    {
        private SessionApp sessionApp;
        private int numRegbyPages;
        public CommunicationRunHistory(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            numRegbyPages = int.Parse(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("GRID_Number_Reg_by_Page")).valueSetting);
        }
        
        public List<ModelViewRunHistory> getModelViewRunHistory(int indexPage)
        {
            List<ModelViewRunHistory> lstScrews = new List<ModelViewRunHistory>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_RUNS", new MySqlParameter[] {
                    new MySqlParameter("page_p", MySqlDbType.Int32) { Value = indexPage },
                    new MySqlParameter("size_p", MySqlDbType.Int32) { Value = numRegbyPages } });
                lstScrews = resultData.AsEnumerable().Select(row =>
                new ModelViewRunHistory
                {
                    id = row.Field<int>("id"),
                    partNumber = row.Field<string>("partNumber"),
                    serial = row.Field<string>("serial"),
                    serial2 = row.Field<string>("serial2"),
                    date = row.Field<DateTime>("date"),
                    result = row.Field<string>("result"),
                    screw1Torque = row.Field<string>("screw1Torque"),
                    screw1Angle = row.Field<string>("screw1Angle"),
                    screw2Torque = row.Field<string>("screw2Torque"),
                    screw2Angle = row.Field<string>("screw2Angle"),
                    screw3Torque = row.Field<string>("screw3Torque"),
                    screw3Angle = row.Field<string>("screw3Angle"),
                    screw4Torque = row.Field<string>("screw4Torque"),
                    screw4Angle = row.Field<string>("screw4Angle"),
                    screw5Torque = row.Field<string>("screw5Torque"),
                    screw5Angle = row.Field<string>("screw5Angle"),
                    ultracappadinspect = row.Field<string>("ultracappadinspect"),
                    ultracapboardinspect = row.Field<string>("ultracapboardinspect"),
                    insulatorinspect = row.Field<string>("insulatorinspect")
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstScrews;
        }

        public void Ins_Upd_ModelViewRunHistory(ModelViewRunHistory ModelViewRunHistory)
        {
            try
            {
                MYSQL_DB mySQL = new MYSQL_DB(sessionApp.connStr);

                mySQL.ExecuteNonQuerySP("SP_INS_UPD_RUNS", new MySqlParameter[] {
                    new MySqlParameter("p_id", MySqlDbType.Int32) { Value = ModelViewRunHistory.id },
                    new MySqlParameter("p_partNumber", MySqlDbType.VarChar) { Value = ModelViewRunHistory.partNumber },
                    new MySqlParameter("p_serial", MySqlDbType.VarChar) { Value = ModelViewRunHistory.serial },
                    new MySqlParameter("p_serial2", MySqlDbType.VarChar) { Value = ModelViewRunHistory.serial2 },
                    new MySqlParameter("p_date", MySqlDbType.DateTime) { Value = ModelViewRunHistory.date },
                    new MySqlParameter("p_result", MySqlDbType.VarChar) { Value = ModelViewRunHistory.result },
                    new MySqlParameter("p_Screw1Torque", MySqlDbType.VarChar) { Value = ModelViewRunHistory.screw1Torque },
                    new MySqlParameter("p_Screw1Angle", MySqlDbType.VarChar) { Value = ModelViewRunHistory.screw1Angle },
                    new MySqlParameter("p_Screw2Torque", MySqlDbType.VarChar) { Value = ModelViewRunHistory.screw2Torque },
                    new MySqlParameter("p_Screw2Angle", MySqlDbType.VarChar) { Value = ModelViewRunHistory.screw2Angle },
                    new MySqlParameter("p_Screw3Torque", MySqlDbType.VarChar) { Value = ModelViewRunHistory.screw3Torque },
                    new MySqlParameter("p_Screw3Angle", MySqlDbType.VarChar) { Value = ModelViewRunHistory.screw3Angle },
                    new MySqlParameter("p_Screw4Torque", MySqlDbType.VarChar) { Value = ModelViewRunHistory.screw4Torque },
                    new MySqlParameter("p_Screw4Angle", MySqlDbType.VarChar) { Value = ModelViewRunHistory.screw4Angle },
                    new MySqlParameter("p_Screw5Torque", MySqlDbType.VarChar) { Value = ModelViewRunHistory.screw5Torque },
                    new MySqlParameter("p_Screw5Angle", MySqlDbType.VarChar) { Value = ModelViewRunHistory.screw5Angle },
                    new MySqlParameter("p_ultracappadinspect", MySqlDbType.VarChar) { Value = ModelViewRunHistory.ultracappadinspect },
                    new MySqlParameter("p_ultracapboardinspect", MySqlDbType.VarChar) { Value = ModelViewRunHistory.ultracapboardinspect },
                    new MySqlParameter("p_insulatorinspect", MySqlDbType.VarChar) { Value = ModelViewRunHistory.insulatorinspect }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
        }
        public void Del_ModelViewRunHistory(ModelViewRunHistory ModelViewRunHistory)
        {
            try
            {
                MYSQL_DB mySQL = new MYSQL_DB(sessionApp.connStr);

                mySQL.ExecuteNonQuerySP("SP_DEL_RUNS", new MySqlParameter[] {
                    new MySqlParameter("p_id", MySqlDbType.Int32) { Value = ModelViewRunHistory.id }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
        }
    }
}

using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using MySql.Data.MySqlClient;
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
        private SessionApp sessionApp;
        private int numRegbyPages;
        public CommunicationScrew(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            numRegbyPages = int.Parse(sessionApp.settings.FirstOrDefault(x=>x.setting.Equals("GRID_Number_Reg_by_Page")).valueSetting);
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
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
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
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstScrews;
        }
        public List<ModelViewModelsScrew> getModelViewModelsScrew(int indexPage)
        {
            List<ModelViewModelsScrew> lstScrews = new List<ModelViewModelsScrew>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_MODELS_SCREWS_PAG", new MySqlParameter[] {
                    new MySqlParameter("page_p", MySqlDbType.Int32) { Value = indexPage },
                    new MySqlParameter("size_p", MySqlDbType.Int32) { Value = numRegbyPages } });
                lstScrews = resultData.AsEnumerable().Select(row =>
                new ModelViewModelsScrew
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
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstScrews;
        }
        public List<ModelViewModelsScrew> getModelViewModelsScrew()
        {
            List<ModelViewModelsScrew> lstScrews = new List<ModelViewModelsScrew>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_MODELS_SCREWS");
                lstScrews = resultData.AsEnumerable().Select(row =>
                new ModelViewModelsScrew
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
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstScrews;
        }

        public void Ins_Upd_ModelViewModelsScrew(ModelViewModelsScrew modelViewModelsScrew)
        {            
            try
            {
                MYSQL_DB mySQL = new MYSQL_DB(sessionApp.connStr);               

                 mySQL.ExecuteNonQuerySP("SP_INS_UPD_MODELS_SCREWS", new MySqlParameter[] {
                    new MySqlParameter("p_id", MySqlDbType.Int32) { Value = modelViewModelsScrew.id },
                    new MySqlParameter("p_partNumber", MySqlDbType.VarChar) { Value = modelViewModelsScrew.partNumber },
                    new MySqlParameter("p_serial", MySqlDbType.VarChar) { Value = modelViewModelsScrew.serial },
                    new MySqlParameter("p_name_model", MySqlDbType.VarChar) { Value = modelViewModelsScrew.name_model },
                    new MySqlParameter("p_description", MySqlDbType.VarChar) { Value = modelViewModelsScrew.description },
                    new MySqlParameter("p_quantity_screws", MySqlDbType.Int32) { Value = modelViewModelsScrew.quantity_screws }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }            
        }
        public void Del_ModelViewModelsScrew(ModelViewModelsScrew modelViewModelsScrew)
        {
            try
            {
                MYSQL_DB mySQL = new MYSQL_DB(sessionApp.connStr);

                mySQL.ExecuteNonQuerySP("SP_DEL_MODELS_SCREWS", new MySqlParameter[] {
                    new MySqlParameter("p_id", MySqlDbType.Int32) { Value = modelViewModelsScrew.id }                    
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
        }

        public List<ModelViewPositionScrew> getModelViewPositionScrew(int indexPage)
        {
            List<ModelViewPositionScrew> lstScrews = new List<ModelViewPositionScrew>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_SCREWS_PAG", new MySqlParameter[] {
                    new MySqlParameter("page_p", MySqlDbType.Int32) { Value = indexPage },
                    new MySqlParameter("size_p", MySqlDbType.Int32) { Value = numRegbyPages } });
                lstScrews = resultData.AsEnumerable().Select(row =>
                new ModelViewPositionScrew
                {
                    id = row.Field<int>("id"),
                    id_screw = row.Field<int>("id_screw"),
                    encoder1 = row.Field<double>("encoder1"),
                    encoder2 = row.Field<double>("encoder2"),
                    tolerance = row.Field<double>("tolerance"),
                    id_model_screw = row.Field<int>("id_model_screw")
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstScrews;
        }

        public void Ins_Upd_ModelViewPositionScrew(ModelViewPositionScrew ModelViewPositionScrew)
        {
            try
            {
                MYSQL_DB mySQL = new MYSQL_DB(sessionApp.connStr);

                mySQL.ExecuteNonQuerySP("SP_INS_UPD_SCREWS", new MySqlParameter[] {
                    new MySqlParameter("p_id", MySqlDbType.Int32) { Value = ModelViewPositionScrew.id },
                    new MySqlParameter("p_id_screw", MySqlDbType.Int32) { Value = ModelViewPositionScrew.id_screw },
                    new MySqlParameter("p_encoder1", MySqlDbType.Double) { Value = ModelViewPositionScrew.encoder1 },
                    new MySqlParameter("p_encoder2", MySqlDbType.Double) { Value = ModelViewPositionScrew.encoder2 },
                    new MySqlParameter("p_tolerance", MySqlDbType.Double) { Value = ModelViewPositionScrew.tolerance },
                    new MySqlParameter("p_id_model_screw", MySqlDbType.Int32) { Value = ModelViewPositionScrew.id_model_screw }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
        }
        public void Del_ModelViewPositionScrew(ModelViewPositionScrew ModelViewPositionScrew)
        {
            try
            {
                MYSQL_DB mySQL = new MYSQL_DB(sessionApp.connStr);

                mySQL.ExecuteNonQuerySP("SP_DEL_SCREWS", new MySqlParameter[] {
                    new MySqlParameter("p_id", MySqlDbType.Int32) { Value = ModelViewPositionScrew.id }
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

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
    public class CommunicationUsers
    {
        private SessionApp sessionApp;
        private int numRegbyPages;
        public CommunicationUsers(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            numRegbyPages = int.Parse(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("GRID_Number_Reg_by_Page")).valueSetting);
        }
        public List<ModelViewUsers> getModelViewUsers(int indexPage)
        {
            List<ModelViewUsers> lstScrews = new List<ModelViewUsers>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_USERS_PAG", new MySqlParameter[] {
                    new MySqlParameter("page_p", MySqlDbType.Int32) { Value = indexPage },
                    new MySqlParameter("size_p", MySqlDbType.Int32) { Value = numRegbyPages } });
                lstScrews = resultData.AsEnumerable().Select(row =>
                new ModelViewUsers
                {
                    id = row.Field<int>("id_users"),
                    name = row.Field<string>("name"),
                    lastName = row.Field<string>("lastName"),
                    username = row.Field<string>("username"),
                    password = row.Field<string>("password"),
                    id_profile = row.Field<int>("id_profile"),
                    profile_description = row.Field<string>("profile_description")
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstScrews;
        }

        public void Ins_Upd_ModelViewUsers(ModelViewUsers ModelViewUsers)
        {
            try
            {
                MYSQL_DB mySQL = new MYSQL_DB(sessionApp.connStr);

                mySQL.ExecuteNonQuerySP("SP_INS_UPD_USERS", new MySqlParameter[] {
                    new MySqlParameter("p_id", MySqlDbType.Int32) { Value = ModelViewUsers.id },
                    new MySqlParameter("p_name", MySqlDbType.VarChar) { Value = ModelViewUsers.name },
                    new MySqlParameter("p_lastName", MySqlDbType.VarChar) { Value = ModelViewUsers.lastName },
                    new MySqlParameter("p_username", MySqlDbType.VarChar) { Value = ModelViewUsers.username },
                    new MySqlParameter("p_password", MySqlDbType.VarChar) { Value = ModelViewUsers.password },
                    new MySqlParameter("p_id_profile", MySqlDbType.VarChar) { Value = ModelViewUsers.id_profile }                    
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
        }
        public void Del_ModelViewUsers(ModelViewUsers ModelViewUsers)
        {
            try
            {
                MYSQL_DB mySQL = new MYSQL_DB(sessionApp.connStr);

                mySQL.ExecuteNonQuerySP("SP_DEL_USERS", new MySqlParameter[] {
                    new MySqlParameter("p_id", MySqlDbType.Int32) { Value = ModelViewUsers.id }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
        }

        public List<ModelViewProfile> getModelViewProfile()
        {
            List<ModelViewProfile> lstScrews = new List<ModelViewProfile>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_PROFILES");
                lstScrews = resultData.AsEnumerable().Select(row =>
                new ModelViewProfile
                {
                    id = row.Field<int>("id"),                    
                    description = row.Field<string>("description")                    
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstScrews;
        }

        public bool authenticationSP(string user, string password)
        {
            bool aut = false;
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                MySqlParameter mySqlParameter = new MySqlParameter();



                int result = mYSQL.ExecuteIntScalarSP("CheckPassword",
                                                                    new MySqlParameter("username_p", user),
                                                                    new MySqlParameter("password_p", password));

                aut = result == 1 ? true : false;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return aut;
        }
        public List<ModelViewUsers> ValidateUsers(string user, string password)
        {
            List<ModelViewUsers> lstScrews = new List<ModelViewUsers>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_VALIDATE_USER", new MySqlParameter[] {
                    new MySqlParameter("p_username", MySqlDbType.String) { Value = user },
                    new MySqlParameter("p_password", MySqlDbType.String) { Value = password } });
                lstScrews = resultData.AsEnumerable().Select(row =>
                new ModelViewUsers
                {
                    id = row.Field<int>("id_users"),
                    name = row.Field<string>("name"),
                    lastName = row.Field<string>("lastName"),
                    username = row.Field<string>("username"),
                    password = row.Field<string>("password"),
                    id_profile = row.Field<int>("id_profile"),
                    profile_description = row.Field<string>("profile_description")
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstScrews;
        }
    }
}

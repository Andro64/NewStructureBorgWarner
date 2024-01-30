using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class UsersAdmin
    {
        SessionApp sessionApp;
        public UsersAdmin(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }
        public bool authentication(string user, string password)
        {
            bool aut = false;
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable dtUsers = mYSQL.GetDataTable("users", "name,lastName,username,password,profile", $"username = '{user}'");
                if(dtUsers.Rows.Count > 0)
                {
                    aut = dtUsers.Rows[0]["password"].ToString() == password ? true : false;
                    if (aut)
                    {
                        User _user = new User{
                            name = dtUsers.Rows[0]["name"].ToString(),
                            lastName = dtUsers.Rows[0]["lastname"].ToString(),
                            userName = dtUsers.Rows[0]["username"].ToString(),
                            password = dtUsers.Rows[0]["password"].ToString(),
                            profile = dtUsers.Rows[0]["profile"].ToString()
                        };
                        sessionApp.user = _user;
                    } 
                }                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;                
            }
            return aut;
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
                Debug.WriteLine(ex.Message);
                throw;
            }
            return aut;
        }
    }
}
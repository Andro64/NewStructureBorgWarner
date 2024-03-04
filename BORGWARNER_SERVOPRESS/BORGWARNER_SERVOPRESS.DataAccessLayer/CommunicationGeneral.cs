using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using MySql.Data.MySqlClient;
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


        public ModelViewTypeWorkstation getWorkstation()
        {
            List<ModelViewTypeWorkstation> WorkStations = new List<ModelViewTypeWorkstation>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_WORKSTATION");
                WorkStations = resultData.AsEnumerable().Select(row =>
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
            return WorkStations.FirstOrDefault();

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
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstSettings;

        }

        public List<Settings> getSettings(int TypeWorkstation)
        {
            List<Settings> lstSettings = new List<Settings>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_SETTINGS_BY_WS", new MySqlParameter[] {
                    new MySqlParameter("p_id_TypeWorkstation", MySqlDbType.Int32) { Value = TypeWorkstation } });
                lstSettings = resultData.AsEnumerable().Select(row =>
                new Settings
                {
                    id_TypeWorkstation = row.Field<int>("id_TypeWorkstation"),
                    id = row.Field<int>("id"),
                    setting = row.Field<string>("setting"),
                    valueSetting = row.Field<string>("value_setting")
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
                throw;
            }
            return lstSettings;

        }

        public List<ConnectionWorkStation> getConnectionsDevices()
        {
            List<ConnectionWorkStation> lstconnectionsRobots = new List<ConnectionWorkStation>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_CONNECTIONS");
                lstconnectionsRobots = resultData.AsEnumerable().Select(row =>
                new ConnectionWorkStation
                {
                    id = row.Field<int>("id"),
                    idTypeDevice = row.Field<int>("id_type_device"),
                    TypeDevice = row.Field<string>("des_type_device"),
                    IP = row.Field<string>("ip"),
                    Port = row.Field<int>("port_robot"),
                    idTypeConnection = row.Field<int>("id_type_connection"),
                    TypeConnection = row.Field<string>("des_type_connection")
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstconnectionsRobots;
        }

        public List<ConnectionWorkStation> getConnectionsDevices(int TypeWorkstation)
        {
            List<ConnectionWorkStation> lstconnectionsRobots = new List<ConnectionWorkStation>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_CONNECTIONS_BY_WS", new MySqlParameter[] {
                    new MySqlParameter("p_id_TypeWorkstation", MySqlDbType.Int32) { Value = TypeWorkstation } });
                lstconnectionsRobots = resultData.AsEnumerable().Select(row =>
                new ConnectionWorkStation
                { 
                    id_TypeWorkstation = row.Field<int>("id_TypeWorkstation"),
                    id = row.Field<int>("id"),
                    idTypeDevice = row.Field<int>("id_type_device"),
                    TypeDevice = row.Field<string>("des_type_device"),
                    IP = row.Field<string>("ip"),
                    Port = row.Field<int>("port_robot"),
                    idTypeConnection = row.Field<int>("id_type_connection"),
                    TypeConnection = row.Field<string>("des_type_connection")
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
                throw;
            }
            return lstconnectionsRobots;
        }

        public int[] getNumsPages(double top)
        {
            int[] arrayIntegers = new int[(int)top];
            for (int i = 1; i <= top; i++)
            {
                arrayIntegers[i - 1] = i;
            }
            return arrayIntegers;
        }
        public List<TotalRegistersByTables> getTotalRegByTables()
        {            
            List<TotalRegistersByTables> lstSettings = new List<TotalRegistersByTables>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_TOTALREG_BY_TABLES");
                lstSettings = resultData.AsEnumerable().Select(row =>
                new TotalRegistersByTables
                {
                    NameTable = row.Field<string>("name_table"),
                    //TotalRegisters = row.Field<int>("total_reg"),
                    //NumPages = row.Field<int>("pages"),
                    //Pages = getNumsPages(row.Field<int>("pages"))
                    TotalRegisters = Convert.IsDBNull(row["total_reg"]) ? 0 : row.Field<int>("total_reg"),
                    NumPages = Convert.IsDBNull(row["pages"]) ? 0 : row.Field<int>("pages"),
                    Pages = Convert.IsDBNull(row["pages"]) ? new int[0] : getNumsPages(row.Field<int>("pages"))
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                throw;
            }
            return lstSettings;
        }

        public List<ModelViewTypeWorkstation> getModelViewTypeWorkstation_By_Id(int idTypeWork)
        {
            List<ModelViewTypeWorkstation> lstTypeWorkstation = new List<ModelViewTypeWorkstation>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_TYPE_WORKSTATION_BY_ID", new MySqlParameter[] {
                    new MySqlParameter("p_id", MySqlDbType.Int32) { Value = idTypeWork } });
                lstTypeWorkstation = resultData.AsEnumerable().Select(row =>
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
            return lstTypeWorkstation;
        }

        public List<CommandCamara> getCommandCamera()
        {
            List<CommandCamara> lstCamera = new List<CommandCamara>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_COMMAND_CAMERAS");
                lstCamera = resultData.AsEnumerable().Select(row =>
                new CommandCamara
                {
                    id = row.Field<int>("id"),
                    id_type_connection = row.Field<int>("id_type_connection"),
                    des_type_connection = row.Field<string>("des_type_connection"),
                    id_type_camara = row.Field<int>("id_type_camara"),
                    des_type_camara = row.Field<string>("des_type_camara"),
                    id_connections_by_workstation = row.Field<int>("id_connections_by_workstation"),
                    ip = row.Field<string>("ip"),
                    port = row.Field<int>("port"),
                    command_user = row.Field<string>("command_user"),
                    command_setstring = row.Field<string>("command_setstring"),
                    command_setevent = row.Field<string>("command_setevent"),
                    command_getvalue_test = row.Field<string>("command_setevent"),
                    command_getvalue_real = row.Field<string>("command_getvalue_real"),
                    command_getjob = row.Field<string>("command_getjob"),
                    command_setjob = row.Field<string>("command_setjob"),
                    command_getvalue_test_1 = row.Field<string>("command_getvalue_test_1"),
                    command_getvalue_real_1 = row.Field<string>("command_getvalue_real_1"),
                    command_getvalue_test_2 = row.Field<string>("command_getvalue_test_2"),
                    command_getvalue_real_2 = row.Field<string>("command_getvalue_real_2")
                }).ToList();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
                throw;
            }
            return lstCamera;

        }

    }
}

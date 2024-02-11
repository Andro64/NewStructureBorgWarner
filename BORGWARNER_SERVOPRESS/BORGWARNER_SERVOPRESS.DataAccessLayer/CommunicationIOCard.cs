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
    public class CommunicationIOCard
    {
        SessionApp sessionApp;
        List<ConnectionWorkStation> connections;
        
        public CommunicationIOCard(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            connections = getConnectionsRobot();
        }
        public List<ConnectionWorkStation> getConnectionsRobot()
        {
            List<ConnectionWorkStation> lstconnectionsRobots = new List<ConnectionWorkStation>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_ROBOT_CONNECTIONS");
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
    }
}

using BORGWARNER_SERVOPRESS.DataModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    public class CommunicationFIS
    {
        SessionApp sessionApp;
        FIS fis;
        public CommunicationFIS(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            fis = new FIS();
            initialize();
        }
        public void initialize()
        {
            
            fis.IP = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals((int)eTypeDevices.FIS) && x.idTypeConnection.Equals((int)eTypeConnection.Main)).IP;
            fis.Port = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals((int)eTypeDevices.FIS) && x.idTypeConnection.Equals((int)eTypeConnection.Main)).Port;            
            fis.Process = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("fis_process")).valueSetting;
            fis.Station = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("fis_station")).valueSetting;            
        }
        public DataFIS SendBREQToFIS(string serial)
        {
            MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
            DataFIS dataFIS = new DataFIS();
            string response;
            //string msg = "BREQ|id=" + serial.Substring(0, serial.Length - 1) + "|process=" + fis.Process + "|station=" + fis.Station;            
            string msg = "BREQ|id=" + serial + "|process=" + fis.Process + "|station=" + fis.Station;

            dataFIS.to_fis = msg;
            response = Sockets.Client(fis.IP, Convert.ToInt32(fis.Port), msg);
            dataFIS.from_fis = response;
            Object[] values = { serial, msg, response };
            using (MySqlConnection conn = new MySqlConnection(sessionApp.connStr))
            {
                conn.Open();
                mYSQL.Insert(conn, "fis_history", "model,to_fis,from_fis", values);
                conn.Close();
            }
            return dataFIS;
        }
        public DataFIS BREQ(string tofis, bool onlymsg)
        {
            DataFIS dataFIS = new DataFIS();
            string response;
            string msg = tofis;
            dataFIS.to_fis = msg;
            response = Sockets.Client(fis.IP, Convert.ToInt32(fis.Port), msg);
            dataFIS.from_fis = response;
            return dataFIS;
        }
        public DataFIS BCMP(string serial, bool pass)
        {
            MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
            DataFIS dataFIS = new DataFIS();
            string response = "";
            string msg = "BCMP|id=" + serial.Substring(0, serial.Length - 1) + "|process=" + fis.Process + "|station=" + fis.Station + "|status=" + (pass ? "PASS" : "FAIL");
            dataFIS.to_fis = msg;
            response = Sockets.Client(fis.IP, Convert.ToInt32(fis.Port), msg);
            dataFIS.from_fis = response;
            Object[] values = { serial, msg, response };
            using (MySqlConnection conn = new MySqlConnection(sessionApp.connStr))
            {
                conn.Open();
                mYSQL.Insert(conn, "fis_history", "model,to_fis,from_fis", values);
                conn.Close();
            }
            return dataFIS;
        }
    }
}

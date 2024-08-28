using BORGWARNER_SERVOPRESS.DataModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            try
            {               
                string response;
                //string msg = "BREQ|id=" + serial.Substring(0, serial.Length - 1) + "|process=" + fis.Process + "|station=" + fis.Station;            
                string msg = "BREQ|id=" + serial.Substring(0, serial.Length - 1) + "|process=" + fis.Process + "|station=" + fis.Station;

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
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error" + ex.Message);
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
        public DataFIS BCMP(string serialParent, string serialChild, bool isTighteningOK, List<Screw> lstScrewsToProcess,int MaxNumberAttempts)
        {
            MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
            DataFIS dataFIS = new DataFIS();
            try
            {
                string response = "";
                string msg = "";
                int indexReg = 1;
                
                string modelo = "42054204"; // quitar
                msg = "BCMP|process=" + fis.Process + "_01" + "|station=" + fis.Station + "|status=" + (isTighteningOK ? "PASS" : "FAIL");
                foreach (var screw in lstScrewsToProcess)
                {

                    screw.screw_identifier = "42036404A"; // quitar
                    if (screw.tighteningprocess.status)
                    {
                        msg = msg + "|testres=" + screw.screw_identifier + "_1_" + indexReg + ", " + screw.tighteningprocess.Torque.Trim() + ",," + screw.tighteningprocess.Angle.Trim() + "," + screw.tighteningprocess.Attempt;
                    }
                    else
                    {
                        if (screw.tighteningprocess.Torque != null)
                        {
                            msg = msg + "|ftestres=" + screw.screw_identifier + "_1_" + indexReg + ", " + screw.tighteningprocess.Torque.Trim() + ",," + screw.tighteningprocess.Angle.Trim() + "," + MaxNumberAttempts;
                        }
                    }
                    indexReg++;
                }

                msg = msg + "|msg=" + (isTighteningOK ? "Pass " : "Fail ") + "at installing the 250KW Current Sense Harness & HVDC Cover Subassembly";

                msg = msg + "|pid=" + serialParent.Substring(0, serialParent.Length - 1) + "|id=" + serialChild.Substring(0, serialChild.Length - 1) + "|model=" + modelo;


                dataFIS.to_fis = msg;
                response = Sockets.Client(fis.IP, Convert.ToInt32(fis.Port), msg);
                dataFIS.from_fis = response;
                Object[] values = { serialParent, msg, response };
                using (MySqlConnection conn = new MySqlConnection(sessionApp.connStr))
                {
                    conn.Open();
                    mYSQL.Insert(conn, "fis_history", "model,to_fis,from_fis", values);
                    conn.Close();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - BCMP Error: " + ex.Message);
            }
            return dataFIS;
        }
        public DataFIS BCMP(string serialParent, string serialChild, bool isTighteningOK)
        {
            MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
            DataFIS dataFIS = new DataFIS();
            try
            {
                string response = "";
                string msg = "";
                int indexReg = 1;

                string modelo = "42054204"; // quitar

                msg = "BCMP|process=" + fis.Process + "_02" + "|station=" + fis.Station + "|status=" + (isTighteningOK ? "PASS" : "FAIL") + "|msg=" + (isTighteningOK ? "Pass " : "Fail ") + "at installing Top Cover"
               + "|pid=" + serialParent.Substring(0, serialParent.Length - 1) + "|id=" + serialChild.Substring(0, serialChild.Length - 1) + "|model=" + modelo;

                dataFIS.to_fis = msg;
                response = Sockets.Client(fis.IP, Convert.ToInt32(fis.Port), msg);
                dataFIS.from_fis = response;
                Object[] values = { serialParent, msg, response };
                using (MySqlConnection conn = new MySqlConnection(sessionApp.connStr))
                {
                    conn.Open();
                    mYSQL.Insert(conn, "fis_history", "model,to_fis,from_fis", values);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - BCMP Error: " + ex.Message);
            }
            return dataFIS;
        }
    }
}

using BORGWARNER_SERVOPRESS.DataModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    public class CommunicationRobot
    {
        SessionApp sessionApp;
        List<ConnectionRobot> connections;
        public enum eTypeConnection
        {
            Main = 1,
            Auxiliary = 2,
            InputDevices = 3,
            Emergency = 4
        }
        public enum eTypeRobot
        {
            ErgoArm = 1,
            Screw = 2,
            RobotEpson = 3
        }
        public CommunicationRobot(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            connections = getConnectionsRobot();            
        }
        public List<ConnectionRobot> getConnectionsRobot()
        {
            List<ConnectionRobot> lstconnectionsRobots = new List<ConnectionRobot>();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_ROBOT_CONNECTIONS");
                lstconnectionsRobots = resultData.AsEnumerable().Select(row =>
                new ConnectionRobot
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
                Debug.WriteLine(ex.Message);
                throw;
            }
            return lstconnectionsRobots;

        }

        public Dictionary<eTypeRobot,string> becomeTypoRobotToEnum(List<ConnectionRobot> lstTypeRobotFromBD)
        {
            Dictionary<eTypeRobot, string> dctTypeRobot = new Dictionary<eTypeRobot, string>();

            foreach (var typeRobotFromBD in lstTypeRobotFromBD)
            {
                if (Enum.TryParse<eTypeRobot>(typeRobotFromBD.TypeDevice, out eTypeRobot typeEnum))
                {
                    dctTypeRobot.Add(typeEnum, typeRobotFromBD.TypeDevice);
                }
                else
                {
                    // Manejar el caso en el que el nombre del tipo Robot no coincide con el enum
                    Debug.WriteLine($"No se puede convertir el robot: {typeRobotFromBD.TypeDevice}");
                }
            }

            return dctTypeRobot;
        }
        public Socket connectRobot(eTypeRobot robotSelected, eTypeConnection connectionSelected)
        {
            try
            {
                ConnectionRobot connectionRobot = connnectionSelected(robotSelected, connectionSelected);
                Socket socket = new Socket(IPAddress.Parse(connectionRobot.IP).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(connectionRobot.IP), connectionRobot.Port);
                socket.Connect(iPEndPoint);
                return socket;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
        public ConnectionRobot connnectionSelected(eTypeRobot robotSelected, eTypeConnection connectionSelected)
        {
            ConnectionRobot connectionRobot;
            try
            {
                connectionRobot = connections.
                FirstOrDefault(x => x.idTypeConnection.Equals((int)connectionSelected) && x.idTypeDevice.Equals((int)robotSelected));
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                throw;
            }
            return connectionRobot;
        }
        public void disconnectRobot(Socket socket)
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
        public string responseRobot(Socket socket)
        {

            if (socket.Connected)
            {
                byte[] responseFromRobot = new byte[1025];
                socket.Receive(responseFromRobot);
                return Encoding.ASCII.GetString(responseFromRobot);
            }
            return string.Empty;
        }
        public string responseRobot(Socket socket, int positionStart, int length)
        {
            if (socket.Connected)
            {
                byte[] responseFromRobot = new byte[1025];
                socket.Receive(responseFromRobot);
                string response = Encoding.ASCII.GetString(responseFromRobot).Substring(positionStart, length).ToString();
                Console.WriteLine(response);
                return response;
            }
            return string.Empty;
        }
        public int sendCodesRobot(Socket socket, string code)
        {
            int result = 0;
            byte[] encodeCode;
            try
            {
                encodeCode = Encoding.Default.GetBytes(code);
                result = socket.Send(encodeCode, SocketFlags.None);
                //regreso
                byte[] responseFromRobot = new byte[1024];
                //int byteRec = socket.Receive(responseFromRobot);
                //string strretorno = Encoding.ASCII.GetString(responseFromRobot,0, byteRec);//.Substring(4, 4).ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                throw;
            }

            return result;
        }
    }
}

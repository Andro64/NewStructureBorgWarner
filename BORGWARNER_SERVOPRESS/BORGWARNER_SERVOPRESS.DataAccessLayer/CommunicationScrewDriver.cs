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
    public class CommunicationScrewDriver
    {
        SessionApp sessionApp;
        List<ConnectionWorkStation> connections;
        public CommunicationScrewDriver(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;            
        }

        public Socket connectScrewDriver(eTypeDevices ScrewDriverSelected, eTypeConnection connectionSelected)
        {
            try
            {
                ConnectionWorkStation connectionScrewDriver = connnectionSelected(ScrewDriverSelected, connectionSelected);
                Socket socket = new Socket(IPAddress.Parse(connectionScrewDriver.IP).AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(connectionScrewDriver.IP), connectionScrewDriver.Port);
                socket.Connect(iPEndPoint);
                return socket;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + "Error: " + ex.Message);
                throw;
            }
        }
        public ConnectionWorkStation connnectionSelected(eTypeDevices ScrewDriverSelected, eTypeConnection connectionSelected)
        {
            ConnectionWorkStation connectionScrewDriver;
            try
            {
                connectionScrewDriver = connections.
                FirstOrDefault(x => x.idTypeConnection.Equals((int)connectionSelected) && x.idTypeDevice.Equals((int)ScrewDriverSelected));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + "Error: " + ex.Message);
                throw;
            }
            return connectionScrewDriver;
        }
        public void disconnectScrewDriver(Socket socket)
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
        public string responseScrewDriver(Socket socket)
        {
            if (socket.Connected)
            {
                byte[] responseFromScrewDriver = new byte[1025];
                socket.Receive(responseFromScrewDriver);
                return Encoding.ASCII.GetString(responseFromScrewDriver);
            }
            return string.Empty;
        }
        public string responseScrewDriver(Socket socket, int positionStart, int length)
        {
            if (socket.Connected)
            {
                byte[] responseFromScrewDriver = new byte[1025];
                socket.Receive(responseFromScrewDriver);
                string response = Encoding.ASCII.GetString(responseFromScrewDriver).Substring(positionStart, length).ToString();
                Console.WriteLine($"{DateTime.Now} - "  + response);
                return response;
            }
            return string.Empty;
        }
        public int sendCodesScrewDriver(Socket socket, string code)
        {
            int result = 0;
            byte[] encodeCode;
            try
            {
                encodeCode = Encoding.Default.GetBytes(code);
                result = socket.Send(encodeCode, SocketFlags.None);
                //regreso
                byte[] responseFromScrewDriver = new byte[1024];
                //int byteRec = socket.Receive(responseFromScrewDriver);
                //string strretorno = Encoding.ASCII.GetString(responseFromScrewDriver,0, byteRec);//.Substring(4, 4).ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + "Error: " + ex.Message);
                throw;
            }

            return result;
        }
    }
}

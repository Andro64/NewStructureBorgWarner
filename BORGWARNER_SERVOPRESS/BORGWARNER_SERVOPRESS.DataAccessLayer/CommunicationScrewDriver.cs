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
                connectionScrewDriver = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeConnection.Equals((int)connectionSelected) && x.idTypeDevice.Equals((int)ScrewDriverSelected));
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
            try
            {
                if (socket.Connected)
                {
                    Debug.WriteLine("Estoy conectado responseScrewDriver");
                    byte[] responseFromScrewDriver = new byte[1025];
                    socket.Receive(responseFromScrewDriver);
                    Debug.WriteLine("Aqui recibi del socket");
                    string result =Encoding.ASCII.GetString(responseFromScrewDriver);
                    Debug.WriteLine("Esta es la respuesta" + result);
                    return result;
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error:" + ex.Message);
            }
            return string.Empty;
        }
        public async Task<string> ResponseScrewDriverAsync(Socket socket)
        {
            //Debug.WriteLine($"Entre: ResponseScrewDriverAsync ");
            if (socket.Connected)
            {
                try
                {
                    byte[] responseFromScrewDriver = new byte[1025];
                    int bytesRead = await ReceiveAsync(socket, responseFromScrewDriver);
                    return Encoding.ASCII.GetString(responseFromScrewDriver, 0, bytesRead);
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Error(ResponseScrewDriverAsync):" + ex.Message);
                }

            }
            return string.Empty;
        }
        private Task<int> ReceiveAsync(Socket socket, byte[] buffer)
        {
            //Debug.WriteLine($"Entre: ReceiveAsync ");

            try
            {
                return Task<int>.Factory.FromAsync(
                    (callback, state) => socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, callback, state),
                    socket.EndReceive,
                    null);
            }
            catch (Exception ex)
            {
                // Maneja la excepción aquí
                Debug.WriteLine($"Se produjo una excepción en ReceiveAsync: {ex.Message}");
                throw; // Puedes relanzar la excepción o manejarla de otra manera según tus necesidades
            }
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
        public string getProgramScrewDriver(TypeExecutionScrew typeExecutionScrew)
        {
            string numProgram = string.Empty;

            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_SCREW_PROGRAM", new MySqlParameter[] {
                    new MySqlParameter("p_rework", MySqlDbType.Bit) { Value = typeExecutionScrew.Rework },
                    new MySqlParameter("p_debug", MySqlDbType.Bit) { Value = typeExecutionScrew.Debug },
                    new MySqlParameter("p_removeScrew", MySqlDbType.Bit) { Value = typeExecutionScrew.RemoveScrew } });
                numProgram = resultData.AsEnumerable().Select(x => x.Field<string>("ProgramaAtornillador")).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
                throw;
            }
            return numProgram;
        }
        public Programs_ScrewDriver getPrograms_ScrewDriver()
        {
            Programs_ScrewDriver programs = new Programs_ScrewDriver();
            try
            {
                MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
                DataTable resultData = mYSQL.ExecuteSP("SP_GET_SCREW_PROGRAMS");
                programs = resultData.AsEnumerable().Select(row =>
                new Programs_ScrewDriver
                {
                    id = row.Field<int>("id"),
                    id_model_screw = row.Field<int>("id_model_screw"),
                    screwing = row.Field<string>("screwing"),
                    rescrewing = row.Field<string>("rescrewing"),
                    unscrewing = row.Field<string>("unscrewing"),
                    simulated = row.Field<string>("simulated")
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
                throw;
            }
            return programs;
        }
    }
}

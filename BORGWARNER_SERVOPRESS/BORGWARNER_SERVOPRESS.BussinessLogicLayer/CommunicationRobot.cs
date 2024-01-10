using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class CommunicationRobot
    {       
        public Socket connectRobot(string ip, int port)
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry("localhost");
                IPAddress iPAddress = host.AddressList[0];

                Socket socket = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                //IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, port);
                socket.Connect(iPEndPoint);
                return socket;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
        public void disconnectRobot(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
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
                result = socket.Send(encodeCode,SocketFlags.None);
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

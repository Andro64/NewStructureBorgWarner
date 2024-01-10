using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class Robot
    {
        SessionApp sessionApp;
        Devices devices;
        public Robot(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }
        public bool InitializeRobot()
        {
            bool blInizialze = false;
            try
            {
                DevicesBL devicesBL = new DevicesBL(sessionApp);
                devices = devicesBL.getDevices();
                //ADU iocard1 = devicesBL.initializeCard(devices.ioCard.NumSerial);
                blInizialze = true;
            }
            catch 
            {
                throw;
            }
            return blInizialze;
        }
        public bool startConnectionRobot()
        {
            bool blConnection = false;
            if (InitializeRobot())
            {
                Socket EpsonRobot_RunModeStart_Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //IPEndPoint EpsonRobot_RunModeStart_IPEndPoint = new IPEndPoint(IPAddress.Parse(devices.epsonRobot.IP), devices.epsonRobot.Port);
                if (!EpsonRobot_RunModeStart_Server.Connected)
                {
                    //EpsonRobot_RunModeStart_Server.Connect(EpsonRobot_RunModeStart_IPEndPoint);
                }
                if (EpsonRobot_RunModeStart_Server.Connected)
                {
                    SendCodesRobot(EpsonRobot_RunModeStart_Server, "$Login" + (char)13 + (char)10);
                    Thread.Sleep(50);
                    string responseLogin = responseRobot(EpsonRobot_RunModeStart_Server);

                    SendCodesRobot(EpsonRobot_RunModeStart_Server, "$Reset" + (char)13 + (char)10);
                    Thread.Sleep(50);
                    string responseReset = responseRobot(EpsonRobot_RunModeStart_Server);

                    Thread.Sleep(500);

                    if (responseLogin.Contains("Login"))
                    {
                        SendCodesRobot(EpsonRobot_RunModeStart_Server, "$Start,0" + (char)13 + (char)10);
                        Thread.Sleep(50);
                        string responseStart = responseRobot(EpsonRobot_RunModeStart_Server);

                        if (responseLogin.Contains("Start"))
                        {
                            //Socket EpsonRobot_RunMode_Server = connectAdditonsPortsRobot(devices.epsonRobot.IP,4000);
                            //Socket EpsonRobot2_RunMode_Server = connectAdditonsPortsRobot(devices.epsonRobot.IP, 4001);
                            //Socket EpsonRobot4_RunMode_Server = connectAdditonsPortsRobot(devices.epsonRobot.IP, 4003);
                            blConnection = true;
                        }                        
                    }
                }                                
            }
            return blConnection;
        }
        
        public Socket connectAdditonsPortsRobot(string ip, int port)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                socket.Connect(iPEndPoint);
                return socket;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
        public string responseRobot(Socket socket)
        {
            byte[] responseFromRobot = new byte[1025];
            socket.Receive(responseFromRobot);
            return Encoding.ASCII.GetString(responseFromRobot);
        }
        public string responseRobot(Socket socket,int positionStart,int length)
        {
            byte[] responseFromRobot = new byte[1025];
            socket.Receive(responseFromRobot);
            return Encoding.ASCII.GetString(responseFromRobot).Substring(positionStart, length).ToString();
        }
        public int SendCodesRobot(Socket socket, string code)
        {
            int result = 0;
            byte[] encodeCode;
            try
            {
                encodeCode = Encoding.Default.GetBytes(code);
                result = socket.Send(encodeCode);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                throw;
            }

            return result;
        }
    }
}

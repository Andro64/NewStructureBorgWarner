using BORGWARNER_SERVOPRESS.DataModel;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    public class CommunicationErgoArm
    {
        SessionApp sessionApp;
        TcpClient tcpClient;
        ModbusIpMaster modbusIPMaster;
        public CommunicationErgoArm(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            tcpClient = new TcpClient();
        }

        public void Connect()
        {
            try
            {
                string ip = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals((int)eTypeDevices.ErgoArm) && x.idTypeConnection.Equals((int)eTypeConnection.Main)).IP;
                int port = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals((int)eTypeDevices.ErgoArm) && x.idTypeConnection.Equals((int)eTypeConnection.Main)).Port;
                tcpClient.Connect(IPAddress.Parse(ip), port);
                modbusIPMaster = ModbusIpMaster.CreateIp(tcpClient);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + "Error: " + ex.Message);
            }
        }
        public void Disconnect()
        {                      
            tcpClient.Close();
        }
        public async Task getDataPositionScrew(CancellationToken cancellationToken, Screw screw)
        {            
            const ushort startingAddressOfInputRegisters = 0;
            const ushort numberRegisterToRead = 2;
            sessionApp.positionErgoArm = new PositionErgoArm();
            while (!sessionApp.positionErgoArm.InPositionReadyToProcess && !cancellationToken.IsCancellationRequested)
            {
                ushort[] dataErgoArm = modbusIPMaster.ReadInputRegisters(startingAddressOfInputRegisters, numberRegisterToRead);
                sessionApp.positionErgoArm.encoder1 = -(Convert.ToDouble(dataErgoArm[0]) * 0.0036) + 110.94;
                sessionApp.positionErgoArm.encoder2 = (Convert.ToDouble(dataErgoArm[1]) * 0.0132) - 106.57;
                Debug.WriteLine($"{DateTime.Now} - encoder1: " + sessionApp.positionErgoArm.encoder1 + " encoder2: " + sessionApp.positionErgoArm.encoder2 + " Leyendo la posicion del ErgoArm");
                validatePosition(screw);
                Debug.WriteLine($"{DateTime.Now} - encoder1: " + sessionApp.positionErgoArm.encoder1 + " encoder2: " + sessionApp.positionErgoArm.encoder2 + " Validando la posicion del ErgoArm");
                //Debug.WriteLine($"{DateTime.Now} - "  + "Validando la posicion del ErgoArm");
                Thread.Sleep(50);
            }
            
        }
        public async Task getDataPosition(CancellationToken cancellationToken)
        {
            const ushort startingAddressOfInputRegisters = 0;
            const ushort numberRegisterToRead = 2;
            sessionApp.positionErgoArm = new PositionErgoArm();
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    ushort[] dataErgoArm = modbusIPMaster.ReadInputRegisters(startingAddressOfInputRegisters, numberRegisterToRead);
                    sessionApp.positionErgoArm.encoder1 = -(Convert.ToDouble(dataErgoArm[0]) * 0.0036) + 110.94;
                    sessionApp.positionErgoArm.encoder2 = (Convert.ToDouble(dataErgoArm[1]) * 0.0132) - 106.57;
                    //Debug.WriteLine($"{DateTime.Now} - " + "Leyendo la posicion del ErgoArm");
                    //Debug.WriteLine($"{DateTime.Now} - " + "Validando la posicion del ErgoArm");
                    Thread.Sleep(50);                    
                }

            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
        }
        public void validatePosition(Screw screw)
        {
            double MinPositionEncoder1 = screw.encoder1 - screw.tolerance;
            double MaxPositionEncoder1 = screw.encoder1 + screw.tolerance;
            double MinPositionEncoder2 = screw.encoder2 - screw.tolerance;
            double MaxPositionEncoder2 = screw.encoder2 + screw.tolerance;

            if ((sessionApp.positionErgoArm.encoder1 > MinPositionEncoder1) && (sessionApp.positionErgoArm.encoder1 < MaxPositionEncoder1) && (sessionApp.positionErgoArm.encoder2 > MinPositionEncoder2) && (sessionApp.positionErgoArm.encoder2 < MaxPositionEncoder2))
            {
                sessionApp.positionErgoArm.InPositionReadyToProcess = true;
            }
            else
            {
                sessionApp.positionErgoArm.InPositionReadyToProcess = false;
            }
        }

        public bool isConnect()
        {
            return tcpClient.Connected;
        }
    }
}

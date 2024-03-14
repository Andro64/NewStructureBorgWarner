using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class ScrewDriver
    {
        SessionApp sessionApp;
        Socket connection;
        CommunicationScrewDriver communicationScrewDriver;
        private bool connectedScrewDriver;
        public ScrewDriver(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            communicationScrewDriver = new CommunicationScrewDriver(sessionApp);
        }
        private bool ScrewingCompleted(Screw screw)
        {
            while (!screw.tighteningprocess.result)
            {
                string response = communicationScrewDriver.responseScrewDriver(connection);
                screw.tighteningprocess.resultResponse = response.Substring(4, 4).ToString();
                screw.tighteningprocess.result = screw.tighteningprocess.resultResponse == "0061";
                screw.tighteningprocess.id = response.Substring(221, 10).ToString();

                Debug.WriteLine($"{DateTime.Now} - "  + response);
                if (screw.tighteningprocess.result)
                {
                    screw.tighteningprocess.Torque = response.Substring(142, 4).ToString();
                    screw.tighteningprocess.Angle = response.Substring(170, 4).ToString();
                    screw.tighteningprocess.status = response.Substring(107, 1).ToString().Equals("2");
                    return true;
                }
                //Thread.Sleep(5);
            }
            return false;
        }

        public bool FirstTighteningAttempt(Screw screw, TypeExecutionScrew typeExecutionScrew)
        {
            connect();
            if (isScrewDriverConnected())
            {
                if (InRange()) 
                {
                    enableScrewdriver();
                    if (ScrewingProgram_by_Model(typeExecutionScrew) == "0005")
                    {
                        if (screwingSubscription() == "0005")
                        {
                            if (ScrewingCompleted(screw))
                            {
                                disconnect();
                                return screw.tighteningprocess.status;
                            }
                        }
                    }
                }
            }
            disconnect();
            return false;            
        }

        public bool SecondTighteningAttempt(Screw screw, TypeExecutionScrew typeExecutionScrew)
        {            
            return FirstTighteningAttempt(screw, typeExecutionScrew);
        }
        public bool ThirdTighteningAttempt(Screw screw, TypeExecutionScrew typeExecutionScrew)
        {            
            return FirstTighteningAttempt(screw, typeExecutionScrew);
        }
        public void disconnect()
        {
            connection.Shutdown(SocketShutdown.Both);
            connection.Close();
        }
        public bool InRange()
        {
            return CleansFiledsTorqueAndAngle() == "0002";
        }
        public void connect()
        {
            connection = communicationScrewDriver.connectScrewDriver(eTypeDevices.Screw, eTypeConnection.Main);
            connectedScrewDriver = connection.Connected;
        }
        public bool isScrewDriverConnected()
        {
            return connectedScrewDriver;
        }
        public string CleansFiledsTorqueAndAngle()
        {
            if (connection.Connected)
            {
                communicationScrewDriver.sendCodesScrewDriver(connection, "00200001001000000000\0");
                string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
                Debug.WriteLine($"{DateTime.Now} - "  + response);
                return response;
            }
            return string.Empty;
        }
        public string ScrewingProgram_by_Model(TypeExecutionScrew typeExecutionScrew)
        {
            string ScrewingProgram = communicationScrewDriver.getProgramScrewDriver(typeExecutionScrew);
            communicationScrewDriver.sendCodesScrewDriver(connection, "002300180010000000000" + ScrewingProgram + "\0");
            string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
            return response;
        }
        public string enableScrewdriver()
        {
            communicationScrewDriver.sendCodesScrewDriver(connection, "00200043000000000000\0");
            string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
            Debug.WriteLine($"{DateTime.Now} - "  + response);
            return response;
        }
        public string screwingSubscription()
        {
            //communicationScrewDriver.sendCodesScrewDriver(connection, @"00200043000000000000\0");
            communicationScrewDriver.sendCodesScrewDriver(connection, "00200060000000000000\0");
            string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
            Debug.WriteLine($"{DateTime.Now} - "  + response);
            return response;
        }




    }
}

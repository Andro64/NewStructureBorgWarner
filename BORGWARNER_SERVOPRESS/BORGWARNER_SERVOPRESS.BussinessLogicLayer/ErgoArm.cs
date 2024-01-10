using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class ErgoArm
    {
        SessionApp sessionApp;
        CommunicationRobot communicationRobot;
        Socket connection;
        
        private bool connectedRobot;
        public ErgoArm(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            communicationRobot = new CommunicationRobot();            
        }        
        public void initializeModels()
        {
         
        }
        public void runRobot()
        {
            //Conectamos el robot
        }
        public void connectingRobot()
        {           
            connection = communicationRobot.connectRobot("::1", 5000);
            connectedRobot= connection.Connected;
        }
        public bool isRobotConnected()
        {
            return connectedRobot;
        }

        public void disconnectingRobot()
        {
            communicationRobot.disconnectRobot(connection);
        }
        public string controllerConnectionInitation()
        {
            if (connection.Connected)
            {
                communicationRobot.sendCodesRobot(connection, @"00200001001000000000\0");
                string response = communicationRobot.responseRobot(connection, 4, 4);
                Debug.WriteLine(response);    
                return response;
            }
            return string.Empty;
        }
        public string enableScrewdriver()
        {
            communicationRobot.sendCodesRobot(connection, @"00200043000000000000\0");
            string response = communicationRobot.responseRobot(connection, 4, 4);
            Debug.WriteLine(response);
            return response;
        }
        public string screwingSubscription()
        {
            communicationRobot.sendCodesRobot(connection, @"00200060000000000000\0");
            string response = communicationRobot.responseRobot(connection, 4, 4);
            Debug.WriteLine(response);
            return response;
        }
        public void ScrewingProgram_by_Model(string model,bool rework, bool debug)
        {
            string ScrewingProgram = string.Empty;
            if (model == "modelo1")
            {
                if (!rework && !debug)
                {
                    ScrewingProgram = "01";
                }
                else if (rework)
                {
                    ScrewingProgram = "05";
                }
                else if (debug)
                {
                    ScrewingProgram = "25";
                }
            }
            else if (model == "modelo2")
            {
                if (!rework && !debug)
                {
                    ScrewingProgram = "11";
                }
                else if (rework)
                {
                    ScrewingProgram = "15";
                }
                else if (debug)
                {
                    ScrewingProgram = "25";
                }
            }
            communicationRobot.sendCodesRobot(connection, @"002300180010000000000" + ScrewingProgram + @"\0");
        }
    }
}

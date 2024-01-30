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
    public class ErgoArmOLD
    {
        SessionApp sessionApp;
        Views.ViewMain viewMain;

        CommunicationRobot communicationRobot;
        Socket connection;
        IOCards ioCard_Type1;
        IOCards ioCard_Type2;
        IOCards ioCard_Type3;
        CancellationTokenSource cancellationToken_ioCard1;
        CancellationTokenSource cancellationToken_ioCard2;
        CancellationTokenSource cancellationToken_ioCard3;

        private bool connectedRobot;
        public ErgoArmOLD(SessionApp _sessionApp, Views.ViewMain _viewMain)
        {
            sessionApp = _sessionApp;
            viewMain = _viewMain;
            communicationRobot = new CommunicationRobot(sessionApp);            
        }
        public ErgoArmOLD(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;            
            communicationRobot = new CommunicationRobot(sessionApp);
        }
        public void startReadSensors(IProgress<string> progressScrew)
        {
            Debug.WriteLine("Inicia lectura de los sensores");
            progressScrew.Report("Inicia lectura de los sensores");



            ioCard_Type1 = new IOCards(sessionApp, new IOCardType_M1());//IOCard_Type1(sessionApp, viewMain);
            cancellationToken_ioCard1 = new CancellationTokenSource();
            Task.Run(async () =>
            {
                ioCard_Type1.getDataInput(cancellationToken_ioCard1.Token, progressScrew);
            }).Wait();

            ioCard_Type2 = new IOCards(sessionApp, new IOCardType_M2());//IOCard_Type1(sessionApp, viewMain);
            cancellationToken_ioCard2 = new CancellationTokenSource();
            Task.Run(async () =>
            {
                 ioCard_Type2.getDataInput(cancellationToken_ioCard2.Token, progressScrew);
            }).Wait();

            ioCard_Type3 = new IOCards(sessionApp, new IOCardType_M3());//IOCard_Type1(sessionApp, viewMain);
            cancellationToken_ioCard3 = new CancellationTokenSource();
            Task.Run(async () =>
            {

                 ioCard_Type3.getDataInput(cancellationToken_ioCard3.Token, progressScrew);
            }).Wait();
        }
        public void endReadSensors()
        {
            cancellationToken_ioCard1.Cancel();
            cancellationToken_ioCard2.Cancel();
            cancellationToken_ioCard3.Cancel();
            //viewMain.getStatusScrew("Termine de leer los sensores");
            Debug.WriteLine("Termine de leer los sensores");
        }
        
        public void connectingRobot()
        {            
            connection = communicationRobot.connectRobot(CommunicationRobot.eTypeRobot.RobotEpson, CommunicationRobot.eTypeConnection.Main);
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

using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class VisionSystem
    {
        SessionApp sessionApp;
        Camara camara;
        TCP_IP TCPcamara;

        public VisionSystem(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            Initialize();
        }
        public void Initialize()
        {
            TCPcamara = new TCP_IP(camara.IP,camara.Port);
            camara = new Camara()
            {
                IP = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals(eTypeDevices.Scanner) && x.idTypeConnection.Equals(eTypeConnection.CognexD900)).IP,
                Port = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals(eTypeDevices.Scanner) && x.idTypeConnection.Equals(eTypeConnection.CognexD900)).Port
            };
            TCPcamara.Conectar();
        }
        public void getNameImageResultFromCamera()
        {
            Thread.Sleep(300);
            string file = GetLatestFile(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("Cognex_Camara_Path")).valueSetting);
            Thread.Sleep(300);
            string nameFile = file.Remove(file.Length - 3) + "jpg";
            Thread.Sleep(500);
            sessionApp.PathImageResultFromCamera = nameFile;
        } 
        public void Disconnect()
        {
            TCPcamara.Desconectar();
        }
               
        public bool FirstInspectionAttempt()
        {
            string readingReuslt;
            readingReuslt = tryCommunicationCamera("SFEXP 5.00" + (char)13 + (char)10);
            Thread.Sleep(100);

            if (ValidateResponse(readingReuslt)) 
            { 
                Thread.Sleep(10); 
            }

            return validateInspectionResult("GVOutput" + (char)13 + (char)10);                  
        }
        
        public bool SecondInspectionAttempt()
        {
            string readingReuslt;           

            readingReuslt = tryCommunicationCamera("SFEXP 20.00" + (char)13 + (char)10);
            Thread.Sleep(100);

            if (ValidateResponse(readingReuslt)) 
            { 
                Thread.Sleep(10); 
            }

            if(validateConnectorCable() && validateRountingCable())
            {
                return true;
            }
            return false;
        }

       
        public bool ThirdInspectionAttempt()
        {
            string readingReuslt;
            readingReuslt = tryCommunicationCamera("SFEXP 10.00" + (char)13 + (char)10);
            Thread.Sleep(100);

            if (ValidateResponse(readingReuslt)) 
            { 
                Thread.Sleep(10); 
            }
            return validateInspectionResult("GVResultado" + (char)13 + (char)10);            
        }

        private bool validateConnectorCable()
        {
            string readingReuslt;
            readingReuslt = getResultCamera("GVOutput3" + (char)13 + (char)10);
            Thread.Sleep(100);
            return ValidateResponse(readingReuslt);
        }
        private bool validateRountingCable()
        {
            string readingReuslt;
            readingReuslt = getResultCamera("GVOutput2" + (char)13 + (char)10);
            Thread.Sleep(100);
            return ValidateResponse(readingReuslt);
        }

        private bool validateInspectionResult(string command)
        {
            string readingReuslt;
            readingReuslt = getResultCamera(command);
            Thread.Sleep(100);
            return ValidateResponse(readingReuslt);
        }
        private bool ValidateResponse(string readingReuslt)
        {
            if (readingReuslt.Contains("1\r\n") || readingReuslt.Contains("1.000\r\n"))
            {
                return true;
            }
            return false;
        }
        private string tryCommunicationCamera(string commandAttempt)
        {
            TCPcamara.EnviarComando(commandAttempt);
            Thread.Sleep(100);
            TCPcamara.EnviarComando("SE8" + (char)13 + (char)10);
            TCPcamara.EnviarComando("GVOutput1" + (char)13 + (char)10);
            return TCPcamara.Leer();
        }
        private string getResultCamera(string command)
        {
            TCPcamara.EnviarComando(command);
            return TCPcamara.Leer();
        }

        private static string GetLatestFile(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            string file = dir.GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .First().ToString();

            return $"{path}\\{file}";
        }        
    }
}

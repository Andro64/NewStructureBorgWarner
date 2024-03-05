using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        eTypeConnection typeCamera;
        CommandCamara commands;

        public VisionSystem(SessionApp _sessionApp, eTypeConnection _typeCamera)
        {
            sessionApp = _sessionApp;
            typeCamera = _typeCamera;
            commands = new CommandCamara();
            Initialize();
        }
        public void Initialize()
        {
            try
            {
                commands = sessionApp.commandCamaras.FirstOrDefault(x => x.id_type_connection.Equals((int)typeCamera));
                camara = new Camara()
                {
                    IP = commands.ip, //sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals((int)eTypeDevices.Camara) && x.idTypeConnection.Equals((int)typeCamera)).IP,
                    Port = commands.port//sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals((int)eTypeDevices.Camara) && x.idTypeConnection.Equals((int)typeCamera)).Port
                };

                TCPcamara = new TCP_IP(camara.IP, camara.Port);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
            }
        }
        private void Connect()
        {
            try
            {
                TCPcamara.Conectar();
                if (commands.command_user != "")
                {
                    TCPcamara.EnviarComando(commands.command_user + (char)13 + (char)10);
                    TCPcamara.EnviarComando("" + (char)13 + (char)10);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
            }
        }
        public bool isConnect()
        {
            return TCPcamara.conectado;
        }
        private bool ReadingBait()
        {
            try
            {
                string FileName = string.Empty;
                string result = string.Empty;
                if (commands.command_setstring != "")
                {
                    FileName = sessionApp.QR.scan1.Substring(0, (sessionApp.QR.scan1.Length - 1));
                    TCPcamara.EnviarComando(commands.command_setstring + FileName + (char)13 + (char)10);
                    Thread.Sleep(150);
                }

                TCPcamara.EnviarComando(commands.command_setevent + (char)13 + (char)10);
                TCPcamara.EnviarComando(commands.command_getvalue_test + (char)13 + (char)10);
                Thread.Sleep(500);
                result = TCPcamara.Leer();
                return ValidateResponse(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
            }
            return false;
        }

        private bool ReadingReal()
        {
            try
            {
                string result = string.Empty;
                TCPcamara.EnviarComando(commands.command_getvalue_real + (char)13 + (char)10);
                Thread.Sleep(500);
                result = TCPcamara.Leer();
                return ValidateResponse(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
            }
            return false;
        }


        public void getNameImageResultFromCamera()
        {
            Thread.Sleep(300);
            string file = GetLatestFile(commands.path_image);
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
            Connect();
            if (!isConnect())
            {
                return false;
            }
            if (!ReadingBait())
            {
                return false;
            }
            return ReadingReal();


            /*
            string readingReuslt;
            readingReuslt = tryCommunicationCamera("SFEXP 5.00" + (char)13 + (char)10);
            Thread.Sleep(100);

            if (ValidateResponse(readingReuslt)) 
            { 
                Thread.Sleep(10); 
            }

            return validateInspectionResult("GVOutput" + (char)13 + (char)10);       */

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

            if (validateConnectorCable() && validateRountingCable())
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
                Thread.Sleep(500);
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

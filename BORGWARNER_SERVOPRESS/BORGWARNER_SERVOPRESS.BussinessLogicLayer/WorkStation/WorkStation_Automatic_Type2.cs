using BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;


namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation
{
    public class WorkStation_Automatic_Type2 : Workstation
    {
        public override string Type => "╭∩╮( •̀_•́ )╭∩╮ \n WS Automatica Tipo 2";
       
        SessionApp sessionApp;
        private CancellationTokenSource _cancellationTokenSource;
        private bool isCancellationRequested = false;

        public override event EventHandler<TextBoxInfoEventArgs> CreateTextBoxRequested;
        public override event EventHandler RemoveTextBoxRequested;

        public WorkStation_Automatic_Type2(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }

        public override void CancelProcess()
        {
            throw new NotImplementedException();
        }

        public void showMessageAndImage(string message, string nameimage = "", bool isImageInDiferentPath = false)
        {
            sessionApp.MessageOfProcess = message;
            sessionApp.ImageOfProcess = isImageInDiferentPath ? nameimage : sessionApp.PathOperationalImages + nameimage;
            Debug.WriteLine($"{DateTime.Now} - " + "Msg:" + message + " -  Image show:" + nameimage);
        }

        public override async Task StartProcess()
        {
            Debug.WriteLine($"{DateTime.Now} - " + "Path de Imagenes:" + sessionApp.PathOperationalImages);

            await Task.Run(() =>
            {
                showMessageAndImage("Inicia Proceso de atornillado", "GNC_HousingWithScanner.png");
                Thread.Sleep(3000);
                showMessageAndImage("Esperamos pallet en Pre-Stopper", "GNC_Mask.png");
                Thread.Sleep(3000);
                showMessageAndImage("Esperamos CLAMP DE PALLET EXTENDIDO", "GNC_Opto.jpeg");
                Thread.Sleep(3000);
                showMessageAndImage("Esperamos que el OPERADOR COLOCAQUE EL HOUSING", "GNC_Padlock.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("SCANNER 1 LEE CODIGO SERIAL: ", "GNC_PalletInHousing.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("PIDE A OPERADOR COLOCAR ULTRA CAP BOARD PAD Y ACTIVAR OPTO", "GNC_PalletInStation.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("ESPERA ACTIVACION DE OPTO ", "GNC_ScrewdriverInHome.png");
                Thread.Sleep(3000);
                showMessageAndImage("Fallo primer intento ESPERA ACTIVACION DE OPTO ", "GNC_SlidePalletOutOfStation.png");
                Thread.Sleep(3000);
                showMessageAndImage("ESPERA ULTRA CAP BOARD SE COLOQUE EN NIDO ", "GNC_ValidatePalletEnteringStation.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("PIDE A OPERADOR TOMAR ULTRA CAP BOARD Y COLOCAR EN NIDO ", "GNC_WaitPallet.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("SCANNER 1 LEE CODIGO SERIAL: ", "KYC_Scanner.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("Los 3 intentos han fallado. ");
                Thread.Sleep(3000);  ///Falta poner que hace en este caso

                /*    sessionApp.MessageOfProcess = "PIDE A OPERADOR TOMAR ULTRA CAP BOARD Y COLOCAR EN NIDO";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "pressure.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "ESPERA ULTRA CAP BOARD SE COLOQUE EN NIDO ";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "pressure.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = $"SCANNER 1 LEE CODIGO SERIAL: ";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "pressure.jpg";
                   Thread.Sleep(5);

                   sessionApp.MessageOfProcess = "PIDE A OPERADOR TOMAR HARNESS, CONECTAR A ULTRA CAP BOARD, COLOCAR EN HOUSING, REALIZAR RUTEO DE HARNESS SOBRE HOUSING Y PRESIONAR OPTO";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "ESPERA ACTIVACION DE OPTO ";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "button.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "Fallo primer intento ESPERA ACTIVACION DE OPTO ";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "button.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "Fallo segundo intento ESPERA ACTIVACION DE OPTO ";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "button.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "Los 3 intentos han fallado. ";
                   Thread.Sleep(3000);  ///Falta poner que hace en este caso
                   sessionApp.MessageOfProcess = "2 PIDE A OPERADOR TOMAR TOMAR MASCARA Y COLOCAR SOBRE HOUSING";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "pressure.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "Esperamos que el OPERADOR COLOCAQUE MASCARA SOBRE HOUSING";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "pallet.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "PIDE A OPERADOR TOMAR ATORNILLADOR Y REALIZAR ATORNILLADO CORRESPONDIENTE";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "pressure.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "BRAZO ERGONOMICO EN POSICION";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "pallet.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "Fallo primer intento de atornillado  ESPERA ACTIVACION DE OPTO ";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "button.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "Fallo segundo intento de atornillado ESPERA ACTIVACION DE OPTO ";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "button.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "Los 3 intentos han fallado. de atornillado";
                   Thread.Sleep(3000);  ///Falta poner que hace en este caso
                   sessionApp.MessageOfProcess = "PIDE A OPERADOR COLOCAR BRAZO EN HOME Y RETIRAR MASCARA";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "pressure.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "PIDE A OPERADOR TOMAR INSULADOR, COLOCAR SOBRE ULTRA CAP BOARD Y ACTIVAR OPTO";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "button.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "OPTO ACTIVADO";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "Fallo primer intento ESPERA ACTIVACION DE OPTO ";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "pallet.jpg";
                   Thread.Sleep(3000);

                   sessionApp.MessageOfProcess = "Fallo segundo intento ESPERA ACTIVACION DE OPTO ";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "button.jpg";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "Los 3 intentos han fallado. ";
                   Thread.Sleep(3000);  ///Falta poner que hace en este caso
                   sessionApp.MessageOfProcess = "INSPECCION OK ENVIA BCMP A FIS";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "DETECTA CLAMP DE PALLET RETRAIDO";
                   Thread.Sleep(3000);
                   sessionApp.MessageOfProcess = "LIBERA PALLET";
                   sessionApp.ImageOfProcess = sessionApp.PathOperationalImages + "pallet.jpg";
                   Thread.Sleep(3000);
                   */
                showMessageAndImage("La informacion correspondiente a los tornillos esta incompleta");
                Thread.Sleep(3000);
                showMessageAndImage("Finaliza Proceso de atornillado", @"C:\Users\bas1s\OneDrive\Imágenes\Trabajo\CONINTEC\Success.gif", true);
                sessionApp.TaksRunExecuting = false;
            });
        }

        public override void RequestCreateTextBox(string msg, int PositionX, int PositionY)
        {
            throw new NotImplementedException();
        }

        public override void RequestRemoveTextBox()
        {
            throw new NotImplementedException();
        }
    }
}

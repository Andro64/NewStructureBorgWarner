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
    class WorkStation_Automatic_Type1 : Workstation
    {
        public override string Type => " ╭∩╮( •̀_•́ )╭∩╮ \n WS Automatica Tipo 1";

        SensorsIO sensorsIO;
        SessionApp sessionApp;
        private CancellationTokenSource _cancellationTokenSource;
        private bool isCancellationRequested = false;

        public override event EventHandler<TextBoxInfoEventArgs> CreateTextBoxRequested;
        public override event EventHandler RemoveTextBoxRequested;

        public WorkStation_Automatic_Type1(SessionApp _sessionApp)
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
            Scanner scanner;
            CommunicationFIS fIS;
            VisionSystem visionSystem;
            Screws screws;
            ErgoArm ergoArm;
            ScrewDriver screwdriver;
            DataFIS dataFIS;

            string serial;
            string resultFIS;
            int quantityScrews;

            

            showMessageAndImage("Esperamos PALLET EN STOPPER", "GNC_PalletInHousing.jpg");
            await CheckSensorAndWait(() => sensorsIO.PalletInStopper(), "Esperamos pallet en Pre-Stopper");
            if (isCancellationRequested) { return; };

            showMessageAndImage("Esperamos PALLET EN STOPPER", "GNC_PalletInHousing.jpg");
            await CheckSensorAndWait(() => sensorsIO.ExtendedPalletClamp(), "Esperamos CLAMP DE PALLET EXTENDIDO");
            if (isCancellationRequested) { return; };

            showMessageAndImage("SCANNER 1 LEE CODIGO SERIAL", "KYC_Scanner.jpg");
            scanner = new Scanner(sessionApp, eTypeConnection.Scan_1);
            serial = scanner.ScanQR("LON");
            if (isCancellationRequested) { return; };

            showMessageAndImage("SE ENVIA BREQ DE HOUSING A FIS", "GNC_HousingWithScanner.jpg");
            fIS = new CommunicationFIS(sessionApp);
            dataFIS = fIS.SendBREQToFIS(serial);
            showMessageAndImage("SE RECIBE BCNF DE HOUSING POR PARTE DE FIS", "GNC_HousingWithScanner.jpg");
            if (isCancellationRequested) { return; };           
            if (dataFIS.from_fis.Contains("PASS"))
            {
                showMessageAndImage("PIDE A OPERADOR COLOCAR HVDC BUSBAR COVER DEBAJO DEL SCANNER 2", "GNC_HousingWithScanner.jpg");
                showMessageAndImage("SENSOR DE TRIGGER DE SCANNER 2 DETECTA", "KYC_Scanner.jpg");
                scanner = new Scanner(sessionApp, eTypeConnection.Scan_2);
                serial = scanner.ScanQR("LON");
                if (isCancellationRequested) { return; };

                showMessageAndImage("SE ENVIA BREQ DE HVDC BUSBAR COVER A FIS", "GNC_HousingWithScanner.jpg");
                fIS = new CommunicationFIS(sessionApp);
                dataFIS = fIS.SendBREQToFIS(serial);
                showMessageAndImage("SE RECIBE BCNF DE HVDC BUSBAR COVER POR PARTE DE FIS", "GNC_HousingWithScanner.jpg");
                if (isCancellationRequested) { return; };
                if (dataFIS.from_fis.Contains("PASS"))
                { 
                
                }

                }

            }
        public async Task CheckSensorAndWait(Func<bool> sensorCheck, string debugMessage)
        {
            if (!sensorCheck())
            {
                Debug.WriteLine($"{DateTime.Now} - {debugMessage}");
                _cancellationTokenSource = new CancellationTokenSource();
                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorCheck());
            }
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

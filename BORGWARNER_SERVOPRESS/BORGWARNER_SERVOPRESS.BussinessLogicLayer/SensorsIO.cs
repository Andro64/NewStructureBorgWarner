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
    public class SensorsIO
    {
        SessionApp sessionApp;
        
        IOCards ioCard_Type_M1;
        IOCards ioCard_Type_M2;
        IOCards ioCard_Type_M3;
        CancellationTokenSource cancellationToken_ioCard1;
        CancellationTokenSource cancellationToken_ioCard2;
        CancellationTokenSource cancellationToken_ioCard3;
        public SensorsIO(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;                    
        }        
        public void startRead()
        {   
            ioCard_Type_M1 = new IOCards(sessionApp, new IOCardType_M1());
            cancellationToken_ioCard1 = new CancellationTokenSource();
            Task.Run(async () =>
            {
                ioCard_Type_M1.getDataInput(cancellationToken_ioCard1.Token);
            }).Wait();
            Debug.WriteLine("Inicia lectura de los sensores ioCard1");

            ioCard_Type_M2 = new IOCards(sessionApp, new IOCardType_M2());
            cancellationToken_ioCard2 = new CancellationTokenSource();
            Task.Run(async () =>
            {
                ioCard_Type_M2.getDataInput(cancellationToken_ioCard2.Token);
            }).Wait();
            Debug.WriteLine("Inicia lectura de los sensores ioCard2");

            ioCard_Type_M3 = new IOCards(sessionApp, new IOCardType_M3());
            cancellationToken_ioCard3 = new CancellationTokenSource();
            Task.Run(async () =>
            {

                ioCard_Type_M3.getDataInput(cancellationToken_ioCard3.Token);
            }).Wait();
            Debug.WriteLine("Inicia lectura de los sensores ioCard3");
        }
        public void endRead()
        {
            cancellationToken_ioCard1.Cancel();            
            Debug.WriteLine("Termine de leer los sensores ioCard1");
            cancellationToken_ioCard2.Cancel();
            Debug.WriteLine("Termine de leer los sensores ioCard2");
            cancellationToken_ioCard3.Cancel();
            Debug.WriteLine("Termine de leer los sensores ioCard3");
        }

       public bool PalletInStopper()
        {
            return sessionApp.Sensors_M1.PalletatPreStation;
        }
        public bool ExtendedPalletClamp()
        {
            return sessionApp.Sensors_M1.PalletatPreStation;
        }
        public bool PlacedHousing()
        {
            return sessionApp.Sensors_M1.PalletatPreStation;
        }
        public bool UltraCapBoardPadinPlace()
        {
            return sessionApp.Sensors_M1.OptoBtn;
        }
        public bool UCBdConnected_RoutingHarness_PlaceInHousing()
        {
            return sessionApp.Sensors_M1.OptoBtn;
        }
        public bool WasPressedOpto()
        {
            return sessionApp.Sensors_M1.OptoBtn;
        }
        public bool UltraCapBoardReadyToScan()
        {
            return sessionApp.Sensors_M2.UltraCapBoardReadytoScan;
        }
        public bool MaskOnHousing()
        {
            return sessionApp.Sensors_M2.MaskatHousing;
        }
        public bool ScrewInScrap()
        {
            return sessionApp.Sensors_M1.MaskatHolder;
        }
        public bool MaskInHolder()
        {
            return sessionApp.Sensors_M1.MaskatHolder;
        }
        public bool DetectsRetractedPalletClamp()
        {
            return sessionApp.Sensors_M1.MaskatHolder;
        }
        public void ActivateSignalToScrewDispenser()
        {
            sessionApp.Sensors_M1.ScrewDispenser = true;
            ioCard_Type_M1.sendDataOutput();
        }        
        public void WaitingResponse(bool sensorToCheck)
        {
            while(!sensorToCheck)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(5);
                }).Wait();
            }
        }

    }
}

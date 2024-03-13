using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

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
            initialize();
        }
        public void initialize()
        {
            ioCard_Type_M1 = new IOCards(sessionApp, new IOCardType_M1());
            cancellationToken_ioCard1 = new CancellationTokenSource();

            ioCard_Type_M2 = new IOCards(sessionApp, new IOCardType_M2());
            cancellationToken_ioCard2 = new CancellationTokenSource();

            ioCard_Type_M3 = new IOCards(sessionApp, new IOCardType_M3());
            cancellationToken_ioCard3 = new CancellationTokenSource();


        }
        public void startRead()
        {

            Task.Run(async () =>
            {
                ioCard_Type_M1.getDataInput(cancellationToken_ioCard1.Token);
            }).Wait();
            Debug.WriteLine($"{DateTime.Now} - " + "Inicia lectura de los sensores ioCard1");


            Task.Run(async () =>
            {
                ioCard_Type_M2.getDataInput(cancellationToken_ioCard2.Token);
            }).Wait();
            Debug.WriteLine($"{DateTime.Now} - " + "Inicia lectura de los sensores ioCard2");

            Task.Run(async () =>
            {

                ioCard_Type_M3.getDataInput(cancellationToken_ioCard3.Token);
            }).Wait();
            Debug.WriteLine($"{DateTime.Now} - " + "Inicia lectura de los sensores ioCard3");
        }
        public void endRead()
        {
            cancellationToken_ioCard1.Cancel();
            Debug.WriteLine($"{DateTime.Now} - " + "Termine de leer los sensores ioCard1");
            cancellationToken_ioCard2.Cancel();
            Debug.WriteLine($"{DateTime.Now} - " + "Termine de leer los sensores ioCard2");
            cancellationToken_ioCard3.Cancel();
            Debug.WriteLine($"{DateTime.Now} - " + "Termine de leer los sensores ioCard3");
        }
        public void SendDataOutpusM1()
        {
            ioCard_Type_M1.sendDataOutput();
        }
        public void SendDataOutpusM2()
        {
            ioCard_Type_M2.sendDataOutput();
        }
        public void SendDataOutpusM3()
        {
            ioCard_Type_M3.sendDataOutput();
        }
        public bool PalletInStopper()
        {
            isWaiting = true;
            return (sessionApp.Sensors_M1.Pallet_Stopper) && sessionApp.Sensors_M1.SecurityOK && sessionApp.Sensors_M1.Main_Pressure;
        }
        public bool PalletOutStopper()
        {
            isWaiting = true;
            return (!sessionApp.Sensors_M1.Pallet_Stopper) && sessionApp.Sensors_M1.SecurityOK && sessionApp.Sensors_M1.Main_Pressure;
        }
        public async Task SecurePallet(CancellationTokenSource cancellationTokenSource)
        {
            await Task.Run(async () =>
            {
                while (sessionApp.Sensors_M1.Pallet_Pre_Stopper)
                {
                    sessionApp.Sensors_M2.Cyl_Pres_Stopper = true;
                    SendDataOutpusM2();
                }
                while (!sessionApp.Sensors_M1.Pallet_Stopper && sessionApp.Sensors_M1.Main_Pressure)
                {
                    Thread.Sleep(50);
                }
            }, cancellationTokenSource.Token);
        }
        public void ExtendedPalletClamp()
        {
            sessionApp.Sensors_M2.PalletFixingRet = false;
            SendDataOutpusM2();
            Thread.Sleep(10);
            sessionApp.Sensors_M2.PalletFixingExt = true;
            SendDataOutpusM2();
        }
        public void RetractPalletClamp()
        {
            sessionApp.Sensors_M2.PalletFixingExt = false;
            SendDataOutpusM2();
            Thread.Sleep(10);
            sessionApp.Sensors_M2.PalletFixingRet = true;
            SendDataOutpusM2();
        }
        public bool isExtendedClamp()
        {
            return sessionApp.Sensors_M2.Cyl_Fixing_Pall_Ext;
        }
        public bool isRetractedClamp()
        {
            return sessionApp.Sensors_M2.Cyl_Fixing_Pall_Ret;
        }
        public bool PlacedHousing()
        {
            isWaiting = true;
            return sessionApp.Sensors_M1.Pallet_Pre_Stopper;            
        }
        public bool isTriggerScanner()
        {
            isWaiting = true;
            sessionApp.sensorToCheck = sessionApp.Sensors_M2.Trigger_Scanner;
            return sessionApp.Sensors_M2.Trigger_Scanner;
        }
        public bool ST13Available()
        {
            isWaiting = true;
            return sessionApp.Sensors_M3.ST13Available;
        }

        public bool UltraCapBoardPadinPlace()
        {
            isWaiting = true;
            return sessionApp.Sensors_M1.OptoBtn;
        }
        public bool UCBdConnected_RoutingHarness_PlaceInHousing()
        {
            return sessionApp.Sensors_M1.OptoBtn;
        }
        public bool WasPressedOpto()
        {
            isWaiting = true;            
            return sessionApp.Sensors_M1.OptoBtn;
        }
        public bool UltraCapBoardReadyToScan()
        {
            return sessionApp.Sensors_M2.K4;//buscar  el sensor correcto
        }
        public bool MaskOnHousing()
        {
            return sessionApp.Sensors_M2.MaskatHousing;
        }
        public bool ScrewInScrap()
        {
            return sessionApp.Sensors_M3.Scrap_presence;
        }
        public bool MaskInHolder()
        {
            return sessionApp.Sensors_M1.MaskInHolder;
        }
        public bool DetectsRetractedPalletClamp()
        {
            return sessionApp.Sensors_M1.K4;//buscar  el sensor correcto
        }
        public void ActivateSignalToScrewDispenser()
        {
            sessionApp.Sensors_M1.ScrewDispenser = true;
            ioCard_Type_M1.sendDataOutput();
        }
        public void Turn_ON_Vacuumm()
        {
            sessionApp.Sensors_M1.Vacuum = true;
            SendDataOutpusM1();
        }
        public void Turn_OFF_Vacuumm()
        {
            sessionApp.Sensors_M1.Vacuum = false;
            SendDataOutpusM1();
        }
        public void StopCylinder()
        {
            sessionApp.Sensors_M2.Cyl_Stopper = true;
            SendDataOutpusM2();
        }
        private bool isWaiting = true;
        public async Task WaitingResponse(CancellationTokenSource cancellationTokenSource, Func<bool> sensorToCheck)
        {
            //while(!sensorToCheck)
            //{
            //    Task.Run(async () =>
            //    {
            //        await Task.Delay(5);
            //    }).Wait();
            //}

            await Task.Run(async () =>
            {
                while (!sensorToCheck() && isWaiting)
                {
                   
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    Debug.WriteLine("Esperando......");
                    await Task.Delay(5);
                }
            }, cancellationTokenSource.Token);
            if (!sensorToCheck())
            {
                Debug.WriteLine("Ya no estoy esperando.");
                //isWaiting = true;
            }
        }
        public async Task WaitingResponseByTime(CancellationTokenSource cancellationTokenSource, bool sensorToCheck, int time)
        {           
            await Task.Run(async () =>
            {
                var stopwatch = Stopwatch.StartNew();

                while (!sessionApp.sensorToCheck && isWaiting)//while (!sensorToCheck && isWaiting)
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    Debug.WriteLine("Esperando......");
                    if (stopwatch.ElapsedMilliseconds >= time) // 1000 milisegundos = 1 segundos
                    {
                        cancellationTokenSource.Cancel(); // Cancela la tarea
                        sessionApp.TaksRunExecuting = false;
                    }
                    await Task.Delay(5);
                }
            }, cancellationTokenSource.Token);
            if (!sensorToCheck)
            {
                Debug.WriteLine("Ya no estoy esperando.");                
            }
        }
        public void StopWaiting()
        {
            isWaiting = false;
        }

    }
}

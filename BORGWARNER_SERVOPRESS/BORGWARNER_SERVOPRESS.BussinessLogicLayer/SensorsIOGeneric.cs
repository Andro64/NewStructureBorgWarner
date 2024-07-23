using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class SensorsIOGeneric
    {
        SessionApp sessionApp;

        IOCardsGeneric ioCardsGeneric;
        CancellationTokenSource cancellationToken_ioCardsGeneric;
        
        public ManualResetEvent continueScrewFunction = new ManualResetEvent(true);
        CancellationTokenSource cancellationToken_ScrewFunction;
        public SensorsIOGeneric(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            initialize();
        }
        public void initialize()
        {
            ioCardsGeneric = new IOCardsGeneric(sessionApp);
            cancellationToken_ioCardsGeneric = new CancellationTokenSource();
            cancellationToken_ScrewFunction = new CancellationTokenSource();
        }
        public void startRead()
        {
            
            IOCardsGeneric io = new IOCardsGeneric(sessionApp);
            Task.Run(async () =>
            {
                io.GetDataInput(cancellationToken_ioCardsGeneric.Token);
            }).Wait();
            Debug.WriteLine($"{DateTime.Now} - " + "Inicia lectura de los sensores.");           
        }
        public void endRead()
        {
            cancellationToken_ioCardsGeneric.Cancel();
            Debug.WriteLine($"{DateTime.Now} - " + "Termine de leer los sensores");
            cancellationToken_ScrewFunction.Cancel();
        }
        
        public void SendDataOutpusGeneric(string keySensor)
        {
            ioCardsGeneric.sendDataOutput(keySensor);
        }
        public void SendDataOutpusGenericSameADU(List<keySensorValue> keySensorvalue)
        {
            ioCardsGeneric.sendDataOutputSameADU(keySensorvalue);
        }
        public bool PalletInPreStopper()
        {
            return sessionApp.IOSensorsGenerics.Pallet_Pre_Stopper;
            //return sessionApp.Sensors_M1.Pallet_Pre_Stopper;
        }
        public bool PalletInStopper()
        {
            return sessionApp.IOSensorsGenerics.Pallet_Stopper && sessionApp.IOSensorsGenerics.SecurityOK && sessionApp.IOSensorsGenerics.Main_Pressure;
            //await Sequence_Stoper_PrestoperAsync(cancellationTokenSource);
            //return sessionApp.Sensors_M1.Pallet_Stopper && sessionApp.Sensors_M1.SecurityOK && sessionApp.Sensors_M1.Main_Pressure;            
        }
        public bool PalletOutStopper()
        {
            isWaiting = true;
            return (!sessionApp.IOSensorsGenerics.Pallet_Stopper) && sessionApp.IOSensorsGenerics.SecurityOK && sessionApp.IOSensorsGenerics.Main_Pressure;
            //return (!sessionApp.Sensors_M1.Pallet_Stopper) && sessionApp.Sensors_M1.SecurityOK && sessionApp.Sensors_M1.Main_Pressure;
        }
        public bool WaitingForProduct(CancellationTokenSource cancellationTokenSource, bool isProcessFinished)
        {
            Sequence_Stoper_PrestoperAsync(cancellationTokenSource, isProcessFinished);
            return sessionApp.IOSensorsGenerics.Pallet_Stopper && sessionApp.IOSensorsGenerics.SecurityOK && sessionApp.IOSensorsGenerics.Main_Pressure;
            //return sessionApp.Sensors_M1.Pallet_Stopper && sessionApp.Sensors_M1.SecurityOK && sessionApp.Sensors_M1.Main_Pressure;
        }
        public async Task Sequence_Stoper_PrestoperAsync(CancellationTokenSource cancellationTokenSource, bool isProcessFinished)
        {
            #region Escenario 1
            //if (!sessionApp.Sensors_M1.Pallet_Pre_Stopper && !sessionApp.Sensors_M1.Pallet_Stopper)
            if (!sessionApp.IOSensorsGenerics.Pallet_Pre_Stopper && !sessionApp.IOSensorsGenerics.Pallet_Stopper)
            {
                await SecurePallet_PreStopper_By_Time(cancellationTokenSource, 500);
                await SecurePallet_Stopper_By_Time(cancellationTokenSource, 500);
            }
            //if (sessionApp.Sensors_M1.Pallet_Pre_Stopper && !sessionApp.Sensors_M1.Pallet_Stopper)
            if (sessionApp.IOSensorsGenerics.Pallet_Pre_Stopper && !sessionApp.IOSensorsGenerics.Pallet_Stopper)
            {
                await UnSecurePallet_PreStopper_By_Time(cancellationTokenSource, 500);
            }
            //if (!sessionApp.Sensors_M1.Pallet_Pre_Stopper && sessionApp.Sensors_M1.Pallet_Stopper)
            if (!sessionApp.IOSensorsGenerics.Pallet_Pre_Stopper && sessionApp.IOSensorsGenerics.Pallet_Stopper)
            {
                await SecurePallet_PreStopper_By_Time(cancellationTokenSource, 500);
            }
            //if (sessionApp.Sensors_M1.Pallet_Pre_Stopper && sessionApp.Sensors_M1.Pallet_Stopper)
            if (sessionApp.IOSensorsGenerics.Pallet_Pre_Stopper && sessionApp.IOSensorsGenerics.Pallet_Stopper)
            {
                await SecurePallet_PreStopper_By_Time(cancellationTokenSource, 500);
                await SecurePallet_Stopper_By_Time(cancellationTokenSource, 500);
            }
            if (isProcessFinished)
            {
                await SecurePallet_PreStopper_By_Time(cancellationTokenSource, 500);
                await UnSecurePallet_Stopper_By_Time(cancellationTokenSource, 500);
                Thread.Sleep(5000);
                await SecurePallet_Stopper_By_Time(cancellationTokenSource, 500);
            }
            #endregion
        }
        public async Task SecurePallet_PreStopper_By_Time(CancellationTokenSource cancellationTokenSource, int milliseconds)
        {
            //sessionApp.Sensors_M2.Cyl_Pres_Stopper = false;            
            //SendDataOutpusM2();
            SendValueByKeySensor("Cyl_Pres_Stopper", false);

            var timer = new System.Timers.Timer(milliseconds);
            timer.Elapsed += (sender, e) =>
            {
                timer.Stop();
            };
            timer.AutoReset = false;
            timer.Start();
        }
        public async Task SecurePallet_Stopper_By_Time(CancellationTokenSource cancellationTokenSource, int milliseconds)
        {
            //sessionApp.Sensors_M2.Cyl_Stopper = false;
            //SendDataOutpusM2();
            SendValueByKeySensor("Cyl_Stopper", false);

            var timer = new System.Timers.Timer(milliseconds);
            timer.Elapsed += (sender, e) =>
            {
                timer.Stop();
            };
            timer.AutoReset = false;
            timer.Start();
        }
        public async Task UnSecurePallet_PreStopper_By_Time(CancellationTokenSource cancellationTokenSource, int milliseconds)
        {
            //sessionApp.Sensors_M2.Cyl_Pres_Stopper = true;
            //SendDataOutpusM2();
            SendValueByKeySensor("Cyl_Pres_Stopper", true);

            var timer = new System.Timers.Timer(milliseconds);
            timer.Elapsed += (sender, e) =>
            {
                timer.Stop();
            };
            timer.AutoReset = false;
            timer.Start();
        }
        public async Task UnSecurePallet_Stopper_By_Time(CancellationTokenSource cancellationTokenSource, int milliseconds)
        {
            //sessionApp.Sensors_M2.Cyl_Stopper = true;
            //SendDataOutpusM2();
            SendValueByKeySensor("Cyl_Stopper", true);

            var timer = new System.Timers.Timer(milliseconds);
            timer.Elapsed += (sender, e) =>
            {
                timer.Stop();
            };
            timer.AutoReset = false;
            timer.Start();
        }
        public async Task SecurePallet(CancellationTokenSource cancellationTokenSource)
        {            
            if (cancellationTokenSource != null)
            {
                await Task.Run(async () =>
                {
                    if (sessionApp.IOSensorsGenerics.Pallet_Stopper)
                    {
                        //sessionApp.Sensors_M2.Cyl_Pres_Stopper = false;
                        //sessionApp.Sensors_M2.Cyl_Stopper = false;
                        //SendDataOutpusM2();
                        SendDataOutpusGenericSameADU(new List<keySensorValue>()
                        { 
                            new keySensorValue { keySensor = "Cyl_Pres_Stopper", value = false },
                            new keySensorValue { keySensor = "Cyl_Stopper", value = false }
                        });
                    }
                    while (!sessionApp.IOSensorsGenerics.Pallet_Stopper && sessionApp.IOSensorsGenerics.Main_Pressure)
                    {
                        Thread.Sleep(50);
                    }
                }, cancellationTokenSource.Token);
            }
        }
        public async Task UnsecurePallet(CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                await Task.Run(async () =>
                {
                    if (!sessionApp.IOSensorsGenerics.Pallet_Stopper)
                    {
                    //sessionApp.Sensors_M2.Cyl_Pres_Stopper = true;
                    //sessionApp.Sensors_M2.Cyl_Stopper = true;
                    //SendDataOutpusM2();
                    
                    SendDataOutpusGenericSameADU(new List<keySensorValue>()
                                {
                                new keySensorValue() { keySensor = "Cyl_Pres_Stopper", value = true },
                                new keySensorValue() { keySensor = "Cyl_Stopper", value = true }                                
                                });
                }
                    while (sessionApp.IOSensorsGenerics.Pallet_Stopper && sessionApp.IOSensorsGenerics.Main_Pressure)
                    {
                        Thread.Sleep(50);
                    }
                }, cancellationTokenSource.Token);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void ExtendedPalletClamp()
        {
            //sessionApp.Sensors_M2.PalletFixingRet = false;
            //SendDataOutpusM2();
            SendValueByKeySensor("PalletFixingRet", false);

            Thread.Sleep(10);
            //sessionApp.Sensors_M2.PalletFixingExt = true;
            //SendDataOutpusM2();
            SendValueByKeySensor("PalletFixingExt", true);
        }
        public void RetractPalletClamp()
        {
            //sessionApp.Sensors_M2.PalletFixingExt = false;
            //SendDataOutpusM2();
            SendValueByKeySensor("PalletFixingExt", false);
            Thread.Sleep(10);
            //sessionApp.Sensors_M2.PalletFixingRet = true;
            //SendDataOutpusM2();
            SendValueByKeySensor("PalletFixingRet", true);
        }
        public bool isExtendedClamp()
        {
            return sessionApp.IOSensorsGenerics.Cyl_Fixing_Pall_Ext;
        }
        public bool isRetractedClamp()
        {
            return sessionApp.IOSensorsGenerics.Cyl_Fixing_Pall_Ret;
        }
        public bool PlacedHousing()
        {
            isWaiting = true;
            return sessionApp.IOSensorsGenerics.Pallet_Pre_Stopper;
        }
        public bool isTriggerScanner()
        {
            isWaiting = true;
            return sessionApp.IOSensorsGenerics.Trigger_Scanner;
        }
        public bool isOutPieceHDVC()
        {
            isWaiting = true;
            return !sessionApp.IOSensorsGenerics.Trigger_Scanner;
        }
        public bool ST13Available()
        {
            isWaiting = true;
            return sessionApp.IOSensorsGenerics.ST13Available;
        }
        public bool UltraCapBoardPadinPlace()
        {
            isWaiting = true;
            return sessionApp.IOSensorsGenerics.OptoBtn;
        }
        public bool UCBdConnected_RoutingHarness_PlaceInHousing()
        {
            return sessionApp.IOSensorsGenerics.OptoBtn;
        }
        public bool WasPressedOpto()
        {
            isWaiting = true;
            return sessionApp.IOSensorsGenerics.OptoBtn;
        }
        //public bool UltraCapBoardReadyToScan()
        //{
        //    return sessionApp.IOSensorsGenerics.K4;//buscar  el sensor correcto
        //}
        public bool MaskOnHousing()
        {
            bool value = false;
            value = sessionApp.IOSensorsGenerics.MaskatHousing;
            return value;

        }
        public async Task MonitorMaskOnHousing()
        {
            while (!cancellationToken_ScrewFunction.IsCancellationRequested)
            {
                if (sessionApp.IOSensorsGenerics.MaskatHousing)
                {
                    sessionApp.MessageOfProcess = $"Por favor, realice el atornillado ";
                    continueScrewFunction.Set();
                    Debug.WriteLine("La función principal se ha reanudado por que tiene la mascara");
                }
                else
                {
                    sessionApp.MessageOfProcess = "Por favor, vuelva a colocar la máscara sobre el housing.";

                    await Task.Run(() =>
                    {
                        // Simula trabajo realizando una pausa
                        Thread.Sleep(100);
                    });
                    continueScrewFunction.Reset();
                    Debug.WriteLine("La función principal se ha detenido por que no tiene la mascara");
                }

            }
        }

        public bool ScrewInScrap()
        {
            isWaiting = true;
            return sessionApp.IOSensorsGenerics.Scrap_presence;
        }
        public bool MaskInHolder()
        {
            isWaiting = true;
            return sessionApp.IOSensorsGenerics.MaskInHolder;
        }
        //public bool DetectsRetractedPalletClamp()
        //{
        //    return sessionApp.IOSensorsGenerics.K4;//buscar  el sensor correcto
        //}
        public void ActivateSignalToScrewDispenser()
        {
            //sessionApp.IOSensorsGenerics.Pallet_Pre_Stopper.ToString();
            //sessionApp.Sensors_M1.ScrewDispenser = true;
            //ioCard_Type_M1.sendDataOutput();
            SendValueByKeySensor("ScrewDispenser", true);
        }
        public void Turn_ON_Vacuumm()
        {            
            //sessionApp.Sensors_M1.Vacuum = true;
            //SendDataOutpusM1();
            SendValueByKeySensor("Vacuum", true);
        }
        public void Turn_OFF_Vacuumm()
        {
            //sessionApp.Sensors_M1.Vacuum = false;
            //SendDataOutpusM1();
            SendValueByKeySensor("Vacuum", false);
        }
        public void ActivateVacumm_by_time(int miliseconds)
        {
            Turn_ON_Vacuumm();
            Thread.Sleep(miliseconds);
            Turn_OFF_Vacuumm();

        }
        public void StopCylinder()
        {
            //sessionApp.Sensors_M2.Cyl_Stopper = true;
            //SendDataOutpusM2();
            SendValueByKeySensor("Cyl_Stopper", true);
        }
        public void DispenseAScrew()
        {
            //sessionApp.Sensors_M1.ScrewDispenser = true;
            //SendDataOutpusM1();
            SendValueByKeySensor("ScrewDispenser", true);
            Thread.Sleep(1000);
            //sessionApp.Sensors_M1.ScrewDispenser = false;
            //SendDataOutpusM1();
            SendValueByKeySensor("ScrewDispenser", false);
        }
        public void ResetScrap()
        {
            //sessionApp.Sensors_M3.ReleScrap = true;
            //SendDataOutpusM3();
            SendValueByKeySensor("ReleScrap", true);
            Thread.Sleep(1000);
            //sessionApp.Sensors_M3.ReleScrap = false;
            //SendDataOutpusM3();
            SendValueByKeySensor("ReleScrap", false);
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
                    //Debug.WriteLine("Esperando......");
                    await Task.Delay(5);
                }
            }, cancellationTokenSource.Token);
            if (!sensorToCheck())
            {
                Debug.WriteLine("Ya no estoy esperando.");
                //isWaiting = true;
            }
        }
        public async Task WaitingResponseByTime(CancellationTokenSource cancellationTokenSource, Func<bool> sensorToCheck, int time)
        {
            await Task.Run(async () =>
            {
                var stopwatch = Stopwatch.StartNew();

                while (!sensorToCheck() && isWaiting)
                {
                    cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    //Debug.WriteLine("Esperando......");
                    if (stopwatch.ElapsedMilliseconds >= time) // 1000 milisegundos = 1 segundos
                    {
                        cancellationTokenSource.Cancel(); // Cancela la tarea
                        sessionApp.TaksRunExecuting = false;
                    }
                    await Task.Delay(5);
                }
            }, cancellationTokenSource.Token);
            if (!sensorToCheck())
            {
                Debug.WriteLine("Ya no estoy esperando.");
            }
        }
        public void StopWaiting()
        {
            isWaiting = false;
        }

        private void SendValueByKeySensor(string keySensor, bool value)
        {
            sessionApp.ADUPorts.Where(x => x.keySensor.Equals(keySensor)).First().Value = value;
            SendDataOutpusGeneric(keySensor);
        }
      
    }
}

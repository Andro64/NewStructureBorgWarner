﻿using BORGWARNER_SERVOPRESS.DataModel;
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
        public  ManualResetEvent continueScrewFunction = new ManualResetEvent(true);
        CancellationTokenSource cancellationToken_ScrewFunction;
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

            cancellationToken_ScrewFunction = new CancellationTokenSource();
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

            //Thread monitorThread_Mask = new Thread(() => WaitingMonitorMaskOnHousing());
            //monitorThread_Mask.Start();
           //Thread monitorThread_MaskInHousing = new Thread(() => MonitorMaskOnHousing());
           //monitorThread_MaskInHousing.Start();


            //Thread monitorThread_Position = new Thread(() => MonitorPosition());
            //monitorThread_Position.Start();

            //_ = MonitorMaskOnHousing();
            //_ = MonitorPosition();

        }
        public void endRead()
        {
            cancellationToken_ioCard1.Cancel();
            Debug.WriteLine($"{DateTime.Now} - " + "Termine de leer los sensores ioCard1");
            cancellationToken_ioCard2.Cancel();
            Debug.WriteLine($"{DateTime.Now} - " + "Termine de leer los sensores ioCard2");
            cancellationToken_ioCard3.Cancel();
            Debug.WriteLine($"{DateTime.Now} - " + "Termine de leer los sensores ioCard3");

            cancellationToken_ScrewFunction.Cancel();
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
        public bool PalletInPreStopper()
        {
            return sessionApp.Sensors_M1.Pallet_Pre_Stopper;
        }
        public bool PalletInStopper()
        {
            //await Sequence_Stoper_PrestoperAsync(cancellationTokenSource);
            return sessionApp.Sensors_M1.Pallet_Stopper && sessionApp.Sensors_M1.SecurityOK && sessionApp.Sensors_M1.Main_Pressure;
        }
        public bool PalletOutStopper()
        {
            isWaiting = true;
            return (!sessionApp.Sensors_M1.Pallet_Stopper) && sessionApp.Sensors_M1.SecurityOK && sessionApp.Sensors_M1.Main_Pressure;
        }
        public bool WaitingForProduct(CancellationTokenSource cancellationTokenSource, bool isProcessFinished)
        {
            Sequence_Stoper_PrestoperAsync(cancellationTokenSource, isProcessFinished);
            return sessionApp.Sensors_M1.Pallet_Stopper && sessionApp.Sensors_M1.SecurityOK && sessionApp.Sensors_M1.Main_Pressure;
        }
        public async Task Sequence_Stoper_PrestoperAsync(CancellationTokenSource cancellationTokenSource, bool isProcessFinished)
        {
            #region Escenario 1
            if (!sessionApp.Sensors_M1.Pallet_Pre_Stopper && !sessionApp.Sensors_M1.Pallet_Stopper)
            {
                
                await SecurePallet_PreStopper_By_Time(cancellationTokenSource, 500);                
                await SecurePallet_Stopper_By_Time(cancellationTokenSource, 500);
            }
            if (sessionApp.Sensors_M1.Pallet_Pre_Stopper && !sessionApp.Sensors_M1.Pallet_Stopper)
            {                
                await UnSecurePallet_PreStopper_By_Time(cancellationTokenSource, 500);
            }
            if (!sessionApp.Sensors_M1.Pallet_Pre_Stopper && sessionApp.Sensors_M1.Pallet_Stopper)
            {                
                await SecurePallet_PreStopper_By_Time(cancellationTokenSource, 500);
            }
            if (sessionApp.Sensors_M1.Pallet_Pre_Stopper && sessionApp.Sensors_M1.Pallet_Stopper)
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
            sessionApp.Sensors_M2.Cyl_Pres_Stopper = false;
            SendDataOutpusM2();

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
            sessionApp.Sensors_M2.Cyl_Stopper = false;
            SendDataOutpusM2();

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
            sessionApp.Sensors_M2.Cyl_Pres_Stopper = true;
            SendDataOutpusM2();

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
            sessionApp.Sensors_M2.Cyl_Stopper = true;
            SendDataOutpusM2();

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
                    if (sessionApp.Sensors_M1.Pallet_Stopper)
                    {
                        sessionApp.Sensors_M2.Cyl_Pres_Stopper = false;
                        sessionApp.Sensors_M2.Cyl_Stopper = false;
                        SendDataOutpusM2();
                    }
                    while (!sessionApp.Sensors_M1.Pallet_Stopper && sessionApp.Sensors_M1.Main_Pressure)
                    {
                        Thread.Sleep(50);
                    }
                }, cancellationTokenSource.Token);
            }
        }
        public async Task UnsecurePallet(CancellationTokenSource cancellationTokenSource)
        {
            await Task.Run(async () =>
            {
                //while (!sessionApp.Sensors_M1.Pallet_Stopper)
                //{
                //    sessionApp.Sensors_M2.Cyl_Pres_Stopper = true;
                //    sessionApp.Sensors_M2.Cyl_Stopper = true;
                //    SendDataOutpusM2();
                //    if(sessionApp.Sensors_M1.Pallet_Pre_Stopper)
                //    {
                //        break;
                //    }
                //}
                if (!sessionApp.Sensors_M1.Pallet_Stopper)
                {
                    sessionApp.Sensors_M2.Cyl_Pres_Stopper = true;
                    sessionApp.Sensors_M2.Cyl_Stopper = true;
                    SendDataOutpusM2();                  
                }
                while (sessionApp.Sensors_M1.Pallet_Stopper && sessionApp.Sensors_M1.Main_Pressure)
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
            //isWaiting = true;            
            return sessionApp.Sensors_M2.Trigger_Scanner;
        }
        public bool isOutPieceHDVC()
        {
            isWaiting = true;            
            return !sessionApp.Sensors_M2.Trigger_Scanner;
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
           bool value = false;           
            value = sessionApp.Sensors_M2.MaskatHousing;            
            return value;

        }
        public async Task  MonitorMaskOnHousing()
        {
            while(!cancellationToken_ScrewFunction.IsCancellationRequested)
            {
                if(sessionApp.Sensors_M2.MaskatHousing)
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

        public async Task WaitingMonitorMaskOnHousing()
        {
            while (!cancellationToken_ScrewFunction.IsCancellationRequested)
            {
                if (sessionApp.Sensors_M2.MaskatHousing)
                {                    
                    sessionApp.MessageOfProcess = $"Por favor, realice el atornillado ";                    
                    Debug.WriteLine("La función principal se ha reanudado por que tiene la mascara");
                }
                else
                {
                    sessionApp.MessageOfProcess = "Por favor, vuelva a colocar la máscara sobre el housing.";
                    await Task.Delay(500);                    
                    Debug.WriteLine("La función principal se ha detenido por que no tiene la mascara");
                    while (!sessionApp.Sensors_M2.MaskatHousing)
                    {
                        Task.Run(async () =>
                        {
                            await Task.Delay(500);
                        }).Wait();
                    }
                }

            }
        }

        //public async Task MonitorPosition()
        //{
        //    while (!cancellationToken_ScrewFunction.IsCancellationRequested)
        //    {
        //        if (sessionApp.positionErgoArm.InPositionReadyToProcess)
        //        {
        //            sessionApp.MessageOfProcess = "Por favor, continue con el proceso.";
        //            continueScrewFunction.Set();
        //            Debug.WriteLine("La función principal se ha reanudado por que esta en posicion");
        //        }
        //        else
        //        {
        //            sessionApp.MessageOfProcess = $"Por favor, posiciones el brazo ergonomico del tornillo.";

        //            await Task.Run(() =>
        //            {
        //                // Simula trabajo realizando una pausa
        //                Thread.Sleep(100);
        //            });
        //            continueScrewFunction.Reset();
        //            Debug.WriteLine("La función principal se ha detenido por no estar en posicion");
        //        }

        //    }
        //}

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
        public void ActivateVacumm_by_time(int miliseconds)
        {
            Turn_ON_Vacuumm();
            Thread.Sleep(miliseconds);
            Turn_OFF_Vacuumm();

        }
        public void StopCylinder()
        {
            sessionApp.Sensors_M2.Cyl_Stopper = true;
            SendDataOutpusM2();
        }
        public void DispenseAScrew()
        {
            sessionApp.Sensors_M1.ScrewDispenser = true;
            SendDataOutpusM1();
            Thread.Sleep(1000);
            sessionApp.Sensors_M1.ScrewDispenser = false;
            SendDataOutpusM1();
        }
        public void ResetScrap()
        {
            sessionApp.Sensors_M3.K6 = true;
            SendDataOutpusM3();
            Thread.Sleep(1000);
            sessionApp.Sensors_M3.K6 = false;
            SendDataOutpusM3();
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

    }
}

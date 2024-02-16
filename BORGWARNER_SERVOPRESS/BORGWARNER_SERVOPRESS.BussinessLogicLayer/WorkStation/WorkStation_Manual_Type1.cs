using BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class WorkStation_Manual_Type1 : Workstation
    {
        public override string Type => "(◕‿◕) \n Manual Tipo 1";

        SensorsIO sensorsIO;
        SessionApp sessionApp;
        private CancellationTokenSource _cancellationTokenSource;
        private bool isCancellationRequested = false;

        public WorkStation_Manual_Type1(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            sensorsIO = new SensorsIO(sessionApp);
        }
        public void showMessageAndImage(string message, string nameimage = "", bool isImageInDiferentPath = false)
        {
            sessionApp.MessageOfProcess = message;
            sessionApp.ImageOfProcess = isImageInDiferentPath ? nameimage : sessionApp.PathOperationalImages + nameimage;
            Debug.WriteLine($"{DateTime.Now} - "  + "Msg:" +  message + " -  Image show:" + nameimage);
        }

        public async Task MensajesPantalla()
        {

            Debug.WriteLine($"{DateTime.Now} - "  + "Path de Imagenes:" + sessionApp.PathOperationalImages);

            await Task.Run(() =>
            {
                showMessageAndImage("Inicia Proceso de atornillado", "pallet.jpg");                
                Thread.Sleep(3000);
                showMessageAndImage("Esperamos pallet en Pre-Stopper", "master.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("Esperamos CLAMP DE PALLET EXTENDIDO","button.jpg");
                Thread.Sleep(3000);
               showMessageAndImage("Esperamos que el OPERADOR COLOCAQUE EL HOUSING","pressure.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("SCANNER 1 LEE CODIGO SERIAL: ","part.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("PIDE A OPERADOR COLOCAR ULTRA CAP BOARD PAD Y ACTIVAR OPTO","button.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("ESPERA ACTIVACION DE OPTO ", "button.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("Fallo primer intento ESPERA ACTIVACION DE OPTO ","button.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("Fallo segundo intento ESPERA ACTIVACION DE OPTO ","button.jpg");
                Thread.Sleep(3000);
                showMessageAndImage("Fallaron los 3 intentos ");
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
                sessionApp.MessageOfProcess = "Fallaron los 3 intentos ";
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
                sessionApp.MessageOfProcess = "Fallaron los 3 intentos de atornillado";
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
                sessionApp.MessageOfProcess = "Fallaron los 3 intentos ";
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
                showMessageAndImage("Finaliza Proceso de atornillado",@"C:\Users\bas1s\OneDrive\Imágenes\Trabajo\CONINTEC\Success.gif",true);
                sessionApp.TaksRunExecuting = false;
            });
        }

        public override void CancelProcess()
        {
            showMessageAndImage("El proceso se ha finalizado a petición del usuario");
            sessionApp.TaksRunExecuting = false;
            isCancellationRequested = true;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose(); // Liberar los recursos del CancellationTokenSource
            _cancellationTokenSource = null; // Establecer el CancellationTokenSource en null para que pueda ser reasignado

            sensorsIO.StopWaiting();
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

        public override async Task StartProcess()
        {
            Scanner scanner;
            CommunicationFIS fIS;
            VisionSystem visionSystem;
            Screws screws;
            ErgoArm ergoArm;
            ScrewDriver screwdriver;


            string serial;
            string resultFIS;

            int quantityScrews;


            //sensorsIO.startRead();

            await CheckSensorAndWait(() => sensorsIO.PalletInStopper(), "Esperamos pallet en Pre-Stopper");
            if (isCancellationRequested) {return; };
            await CheckSensorAndWait(() => sensorsIO.ExtendedPalletClamp(), "Esperamos CLAMP DE PALLET EXTENDIDO");
            if (isCancellationRequested) { return; };
            await CheckSensorAndWait(() => sensorsIO.PlacedHousing(), "Esperamos que el OPERADOR COLOCAQUE EL HOUSING");
            if (isCancellationRequested) { return; };

            //if (!sensorsIO.PalletInStopper())
            //{
            //    Debug.WriteLine($"{DateTime.Now} - " + "Esperamos pallet en Pre-Stopper");
            //    _cancellationTokenSource = new CancellationTokenSource();
            //    await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.PalletInStopper());
            //}
            //if (isCancellationRequested) { return; } ;

            //if (!sensorsIO.ExtendedPalletClamp())
            //{
            //    Debug.WriteLine($"{DateTime.Now} - " + "Esperamos CLAMP DE PALLET EXTENDIDO");
            //    _cancellationTokenSource = new CancellationTokenSource();
            //    await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.ExtendedPalletClamp());                
            //}
            //if (isCancellationRequested) { return; };
            //if (!sensorsIO.PlacedHousing())
            //{
            //    Debug.WriteLine($"{DateTime.Now} - " + "Esperamos que el OPERADOR COLOCAQUE EL HOUSING");
            //    _cancellationTokenSource = new CancellationTokenSource();
            //    await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.PlacedHousing());                
            //}
            
            scanner = new Scanner(sessionApp, eTypeConnection.Scan_1);
            serial = scanner.ScanQR("LON");
            Debug.WriteLine($"{DateTime.Now} - "  + $"SCANNER 1 LEE CODIGO SERIAL: {serial}");

            if (isCancellationRequested) { return; };

            fIS = new CommunicationFIS(sessionApp);
            resultFIS = fIS.SendBREQToFIS(serial);

            if (resultFIS.Contains("PASS"))
            {
                Debug.WriteLine($"{DateTime.Now} - "  + "PIDE A OPERADOR COLOCAR ULTRA CAP BOARD PAD Y ACTIVAR OPTO");
                
                await CheckSensorAndWait(() => sensorsIO.UltraCapBoardPadinPlace(), "ESPERA ACTIVACION DE OPTO");
                if (isCancellationRequested) { return; };

                //if (!sensorsIO.UltraCapBoardPadinPlace())
                //{
                //    _cancellationTokenSource = new CancellationTokenSource();
                //    await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.UltraCapBoardPadinPlace());
                //    Debug.WriteLine($"{DateTime.Now} - "  + "ESPERA ACTIVACION DE OPTO ");
                //}

                visionSystem = new VisionSystem(sessionApp);
                if (!visionSystem.FirstInspectionAttempt())
                {
                    if (!sensorsIO.WasPressedOpto())
                    {
                        _cancellationTokenSource = new CancellationTokenSource();
                        await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                        Debug.WriteLine($"{DateTime.Now} - "  + "Fallo primer intento ESPERA ACTIVACION DE OPTO ");

                        if (!visionSystem.SecondInspectionAttempt())
                        {
                            if (!sensorsIO.WasPressedOpto())
                            {
                                _cancellationTokenSource = new CancellationTokenSource();
                                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                Debug.WriteLine($"{DateTime.Now} - "  + "Fallo segundo intento ESPERA ACTIVACION DE OPTO ");

                                if (!visionSystem.ThirdInspectionAttempt())
                                {
                                    Debug.WriteLine($"{DateTime.Now} - "  + "Fallaron los 3 intentos ");  ///Falta poner que hace en este caso
                                }
                            }
                        }
                    }
                }
                visionSystem.getNameImageResultFromCamera();
                visionSystem.Disconnect();

                Debug.WriteLine($"{DateTime.Now} - "  + "PIDE A OPERADOR TOMAR ULTRA CAP BOARD Y COLOCAR EN NIDO");

                if (!sensorsIO.UltraCapBoardReadyToScan())
                {
                    await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.UltraCapBoardReadyToScan());
                    Debug.WriteLine($"{DateTime.Now} - "  + "ESPERA ULTRA CAP BOARD SE COLOQUE EN NIDO ");
                }

                scanner = new Scanner(sessionApp, eTypeConnection.Scan_2);
                serial = scanner.ScanQR("LON");
                Debug.WriteLine($"{DateTime.Now} - "  + $"SCANNER 1 LEE CODIGO SERIAL: {serial}");

                fIS = new CommunicationFIS(sessionApp);
                resultFIS = fIS.SendBREQToFIS(serial);

                if (resultFIS.Contains("PASS"))
                {
                    Debug.WriteLine($"{DateTime.Now} - "  + "PIDE A OPERADOR TOMAR HARNESS, CONECTAR A ULTRA CAP BOARD, COLOCAR EN HOUSING, REALIZAR RUTEO DE HARNESS SOBRE HOUSING Y PRESIONAR OPTO");
                    if (!sensorsIO.UCBdConnected_RoutingHarness_PlaceInHousing())
                    {
                        await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.UCBdConnected_RoutingHarness_PlaceInHousing());
                        Debug.WriteLine($"{DateTime.Now} - "  + "ESPERA ACTIVACION DE OPTO ");
                    }

                    visionSystem = new VisionSystem(sessionApp);
                    if (!visionSystem.FirstInspectionAttempt())
                    {
                        if (!sensorsIO.WasPressedOpto())
                        {
                            await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                            Debug.WriteLine($"{DateTime.Now} - "  + "Fallo primer intento ESPERA ACTIVACION DE OPTO ");

                            if (!visionSystem.SecondInspectionAttempt())
                            {
                                if (!sensorsIO.WasPressedOpto())
                                {
                                    await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                    Debug.WriteLine($"{DateTime.Now} - "  + "Fallo segundo intento ESPERA ACTIVACION DE OPTO ");

                                    if (!visionSystem.ThirdInspectionAttempt())
                                    {
                                        Debug.WriteLine($"{DateTime.Now} - "  + "Fallaron los 3 intentos ");  ///Falta poner que hace en este caso
                                    }
                                }
                            }
                        }
                    }
                    visionSystem.getNameImageResultFromCamera();
                    visionSystem.Disconnect();

                    Debug.WriteLine($"{DateTime.Now} - "  + "2 PIDE A OPERADOR TOMAR TOMAR MASCARA Y COLOCAR SOBRE HOUSING");
                    if (!sensorsIO.PlacedHousing())
                    {
                        await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.PlacedHousing());
                        Debug.WriteLine($"{DateTime.Now} - "  + "Esperamos que el OPERADOR COLOCAQUE MASCARA SOBRE HOUSING");
                    }
                    sensorsIO.ActivateSignalToScrewDispenser();
                    Debug.WriteLine($"{DateTime.Now} - "  + "PIDE A OPERADOR TOMAR ATORNILLADOR Y REALIZAR ATORNILLADO CORRESPONDIENTE");

                    screws = new Screws(sessionApp);
                    quantityScrews = screws.retriveNumberScrewsPerModel(sessionApp.ModelScrewSelected);
                    List<Screw> lstScrewsToProcess = screws.retriveScrewsToProcess(sessionApp.ModelScrewSelected);
                    if(lstScrewsToProcess.Count != 0 && (quantityScrews == lstScrewsToProcess.Count))
                    {
                        ergoArm = new ErgoArm(sessionApp);
                        screwdriver = new ScrewDriver(sessionApp);
                        foreach (var screw in lstScrewsToProcess)
                        {
                            screw.tighteningprocess = new TighteningProcess();
                            ergoArm.startReadPositionRespectScrew(screw);
                            if(sessionApp.positionErgoArm.InPositionReadyToProcess)
                            {
                                Debug.WriteLine($"{DateTime.Now} - "  + "BRAZO ERGONOMICO EN POSICION");

                                if (!screwdriver.FirstTighteningAttempt(screw))
                                {
                                    if (!sensorsIO.WasPressedOpto())
                                    {
                                        await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                        Debug.WriteLine($"{DateTime.Now} - "  + "Fallo primer intento de atornillado  ESPERA ACTIVACION DE OPTO ");
                                        if (!screwdriver.SecondTighteningAttempt(screw))
                                        {
                                            if (!sensorsIO.WasPressedOpto())
                                            {
                                                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                                Debug.WriteLine($"{DateTime.Now} - "  + "Fallo segundo intento de atornillado ESPERA ACTIVACION DE OPTO ");

                                                if (screwdriver.ThirdTighteningAttempt(screw))
                                                {
                                                    Debug.WriteLine($"{DateTime.Now} - "  + "Fallaron los 3 intentos de atornillado");  ///Falta poner que hace en este caso
                                                }
                                            }
                                        }
                                    }
                                }                                
                            }
                        }
                        Debug.WriteLine($"{DateTime.Now} - "  + "PIDE A OPERADOR COLOCAR BRAZO EN HOME Y RETIRAR MASCARA");
                        if(!ergoArm.isInHome())
                        {
                            ergoArm.WaitingResponse(ergoArm.isInHome());
                            ergoArm.endReadPostion();
                        }                        

                        if(!sensorsIO.MaskInHolder())
                        {
                            await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.MaskInHolder());
                        }
                        Debug.WriteLine($"{DateTime.Now} - "  + "PIDE A OPERADOR TOMAR INSULADOR, COLOCAR SOBRE ULTRA CAP BOARD Y ACTIVAR OPTO");
                        if (!sensorsIO.WasPressedOpto())
                        {
                            await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                            Debug.WriteLine($"{DateTime.Now} - "  + "OPTO ACTIVADO");
                        }

                        visionSystem = new VisionSystem(sessionApp);
                        if (!visionSystem.FirstInspectionAttempt())
                        {
                            if (!sensorsIO.WasPressedOpto())
                            {
                                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                Debug.WriteLine($"{DateTime.Now} - "  + "Fallo primer intento ESPERA ACTIVACION DE OPTO ");

                                if (!visionSystem.SecondInspectionAttempt())
                                {
                                    if (!sensorsIO.WasPressedOpto())
                                    {
                                        await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                        Debug.WriteLine($"{DateTime.Now} - "  + "Fallo segundo intento ESPERA ACTIVACION DE OPTO ");

                                        if (!visionSystem.ThirdInspectionAttempt())
                                        {
                                            Debug.WriteLine($"{DateTime.Now} - "  + "Fallaron los 3 intentos ");  ///Falta poner que hace en este caso
                                        }
                                    }
                                }
                            }
                        }
                        visionSystem.getNameImageResultFromCamera();
                        visionSystem.Disconnect();
                        Debug.WriteLine($"{DateTime.Now} - "  + "INSPECCION OK ENVIA BCMP A FIS");

                        fIS = new CommunicationFIS(sessionApp);
                        resultFIS = fIS.BCMP(serial,true); //?Cual serial se envia y el pass que signifca?
                        
                        Debug.WriteLine($"{DateTime.Now} - "  + "DETECTA CLAMP DE PALLET RETRAIDO");
                        if (!sensorsIO.DetectsRetractedPalletClamp())
                        {
                            await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.DetectsRetractedPalletClamp());
                        }
                        Debug.WriteLine($"{DateTime.Now} - "  + "LIBERA PALLET");

                    }
                    else
                    {
                        Debug.WriteLine($"{DateTime.Now} - "  + "La informacion correspondiente a los tornillos esta incompleta");
                    }



                }//Falta si no PASS 2

            }//Falta si no PASS 
            sessionApp.TaksRunExecuting = false;
        }
        
    }
}

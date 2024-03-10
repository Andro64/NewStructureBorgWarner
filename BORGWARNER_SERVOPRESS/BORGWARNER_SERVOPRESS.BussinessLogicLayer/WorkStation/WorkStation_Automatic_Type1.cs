using BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
            showMessageAndImage("El proceso se ha finalizado a petición del usuario");
            sessionApp.TaksRunExecuting = false;
            isCancellationRequested = true;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose(); // Liberar los recursos del CancellationTokenSource
            _cancellationTokenSource = null; // Establecer el CancellationTokenSource en null para que pueda ser reasignado
            if (sensorsIO != null)
            {
                sensorsIO.StopWaiting();
            }
        }

        public void showMessageAndImage(string message, string nameimage = "", bool isImageInDiferentPath = false)
        {
            sessionApp.MessageOfProcess = message;
            sessionApp.ImageOfProcess = isImageInDiferentPath ? nameimage : sessionApp.PathOperationalImages + nameimage;          
            Debug.WriteLine($"{DateTime.Now} - " + "Msg:" + message + " -  Image show:" + nameimage);
        }
        public bool Validation_by_FIS(string serial, string show_message, eTypeSendToFIS typeSendToFIS, bool isPass = false)
        {
            CommunicationFIS fIS;
            DataFIS dataFIS;
            bool isPASS_From_FIS = false;

            if (bool.Parse(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("EneableFIS")).valueSetting))
            {
                showMessageAndImage(show_message);
                fIS = new CommunicationFIS(sessionApp);
                switch (typeSendToFIS)
                {
                    case eTypeSendToFIS.BREQ:
                        dataFIS = fIS.SendBREQToFIS(serial);
                        isPASS_From_FIS = dataFIS.from_fis.Contains("PASS");
                        break;
                    case eTypeSendToFIS.BCMP:
                        dataFIS = fIS.BCMP(serial, isPass);
                        isPASS_From_FIS = dataFIS.from_fis.Contains("PASS");
                        break;
                }
            }
            else
            {
                isPASS_From_FIS = true;
                Debug.WriteLine($"{DateTime.Now} - " + "Se simula el paso por FIS ");
            }
            return isPASS_From_FIS;
        }
        public override async Task StartProcess()
        {
            Debug.WriteLine($"{DateTime.Now} - " + "Path de Imagenes:" + sessionApp.PathOperationalImages);

            await Task.Run(() =>
            {
                showMessageAndImage("Inicia Proceso de atornillado", "GNC_HousingWithScanner.png");
                Thread.Sleep(3000);
                showMessageAndImage("Esperamos pallet en Pre-Stopper", @"C:\Users\MyUser\Desktop\COGNEX\Cognex_1\Conector2Bad\4203641232680057733.svg",true);
                Thread.Sleep(3000);
                showMessageAndImage("Esperamos CLAMP DE PALLET EXTENDIDO", "4203641232680057733.svg");
                Thread.Sleep(3000);
                //showMessageAndImage("Esperamos que el OPERADOR COLOCAQUE EL HOUSING", "GNC_Padlock.jpg");
                //Thread.Sleep(3000);
                //showMessageAndImage("SCANNER 1 LEE CODIGO SERIAL: ");
                //Thread.Sleep(3000);
                //showMessageAndImage("PIDE A OPERADOR COLOCAR ULTRA CAP BOARD PAD Y ACTIVAR OPTO", "GNC_PalletInStation.jpg");
                //Thread.Sleep(3000);
                //showMessageAndImage("ESPERA ACTIVACION DE OPTO ", "GNC_ScrewdriverInHome.png");
                //Thread.Sleep(3000);
                //showMessageAndImage("Fallo primer intento ESPERA ACTIVACION DE OPTO ", "GNC_SlidePalletOutOfStation.png");
                //Thread.Sleep(3000);
                //showMessageAndImage("ESPERA ULTRA CAP BOARD SE COLOQUE EN NIDO ", "GNC_ValidatePalletEnteringStation.jpg");
                //Thread.Sleep(3000);
                //showMessageAndImage("PIDE A OPERADOR TOMAR ULTRA CAP BOARD Y COLOCAR EN NIDO ", "GNC_WaitPallet.jpg");
                //Thread.Sleep(3000);
                //showMessageAndImage("SCANNER 1 LEE CODIGO SERIAL: ", "KYC_Scanner.jpg");
                //Thread.Sleep(3000);
                //showMessageAndImage("Fallaron los 3 intentos ");
                //Thread.Sleep(3000);  ///Falta poner que hace en este caso

                showMessageAndImage("La informacion correspondiente a los tornillos esta incompleta");
                Thread.Sleep(3000);
                showMessageAndImage("Finaliza Proceso de atornillado", "image_not_found.jpg");
                sessionApp.TaksRunExecuting = false;
            });
        }
        /*
        public override async Task StartProcess()
        {
            Scanner scanner;
            VisionSystem visionSystem;
            Screws screws;
            ErgoArm ergoArm;
            ScrewDriver screwdriver;
            BitmapImage bitmapImage = new BitmapImage();

            string serial;
            string resultFIS;
            int quantityScrews;



            showMessageAndImage("ESPERANDO PRODUCTO", "Housing.png");
            await CheckSensorAndWait(() => sensorsIO.PalletInStopper(), "Esperamos pallet en Pre-Stopper");

            await sensorsIO.SecurePallet(_cancellationTokenSource);
            if (isCancellationRequested) { return; };

            sensorsIO.ExtendedPalletClamp();

            if (sensorsIO.isExtendedClamp())
            {
                showMessageAndImage("Candado extendido", "MGPM25-10Z.png");
            }
            else
            {
                await CheckSensorAndWaitByTime(() => sensorsIO.isExtendedClamp(), "Esperamos CLAMP DE PALLET EXTENDIDO por 5 segundos", 5000);
                if (!sessionApp.TaksRunExecuting)
                {
                    isCancellationRequested = true;
                    showMessageAndImage("No se extendio correctamente el candado se cancela proceso, favor de verificar");
                }
                else
                {
                    showMessageAndImage("Candado extendido", "MGPM25-10Z.png");
                }
            }
            if (isCancellationRequested) { return; };

            showMessageAndImage("Laeyendo QR Housing");
            serial = new Scanner(sessionApp, eTypeConnection.Scan_1).ScanQR("LON");
            if (isCancellationRequested) { return; };
            sessionApp.QR.scan1 = serial;

            if (Validation_by_FIS(serial, "SE ENVIA BREQ DE HOUSING A FIS", eTypeSendToFIS.BREQ))
            {
                showMessageAndImage("Inspeccion realizada...");
                Thread.Sleep(300);
                showMessageAndImage("Tome HVDC COVER y coloque frente al escaner", "ScannerHVDCCover.jpg");
                await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperando HVDC cover");
                if (isCancellationRequested) { return; };

                showMessageAndImage("Leyendo QR HVDC Cover");
                serial = new Scanner(sessionApp, eTypeConnection.Scan_2).ScanQR("LON");
                if (isCancellationRequested) { return; };

                if (Validation_by_FIS(serial, "SE ENVIA BREQ A FIS", eTypeSendToFIS.BREQ))
                {
                    showMessageAndImage("Inspeccion realizada...");
                    Thread.Sleep(300);

                    showMessageAndImage("Tome cable arnés y coloquelo frente al escaner", "ScannerHarness.jpg");
                    await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperamos que el operador coloque el arnés en el scaner");
                    if (isCancellationRequested) { return; };

                    showMessageAndImage("Leyendo QR arnés", "ScannerHarness.jpg");
                    serial = new Scanner(sessionApp, eTypeConnection.Scan_2).ScanQR("LON");
                    if (isCancellationRequested) { return; };

                    if (Validation_by_FIS(serial, "SE ENVIA BREQ ÁRNES A FIS", eTypeSendToFIS.BREQ))
                    {
                        showMessageAndImage("Inspeccion realizada...");
                        Thread.Sleep(300);

                        showMessageAndImage("Conecte árness y realice su ruteo, despues presione el opto", "HousingRoute.png");
                        await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "Esperamos que el operador presione opto del árnes");
                        if (isCancellationRequested) { return; };

                        showMessageAndImage("Realizando Inspeccion 1...");
                        Thread.Sleep(300);

                        visionSystem = new VisionSystem(sessionApp, eTypeConnection.Camara_1);
                        if (!visionSystem.FirstInspectionAttempt(serial))
                        {
                            bitmapImage = visionSystem.getNameImageResultFromCamera(false);
                            visionSystem.Disconnect();
                            if (!sensorsIO.WasPressedOpto())
                            {
                                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                Debug.WriteLine($"{DateTime.Now} - " + "Fallo primer intento ESPERA ACTIVACION DE OPTO ");
                                showMessageAndImage("Fallo primer intento, reacomode y presione OPTO");
                                if (isCancellationRequested) { return; };

                                if (!visionSystem.SecondInspectionAttempt())
                                {
                                    bitmapImage = visionSystem.getNameImageResultFromCamera(false);
                                    visionSystem.Disconnect();
                                    if (!sensorsIO.WasPressedOpto())
                                    {
                                        await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                        Debug.WriteLine($"{DateTime.Now} - " + "Fallo segundo intento ESPERA ACTIVACION DE OPTO ");
                                        showMessageAndImage("Fallo segundo intento,reacomode y presione OPTO");
                                        if (isCancellationRequested) { return; };

                                        if (!visionSystem.ThirdInspectionAttempt())
                                        {
                                            bitmapImage = visionSystem.getNameImageResultFromCamera(false);
                                            visionSystem.Disconnect();
                                            showMessageAndImage("Fallaron los 3 intentos");
                                            Debug.WriteLine($"{DateTime.Now} - " + "Fallaron los 3 intentos");  ///Falta poner que hace en este caso
                                            isCancellationRequested = true;
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        bitmapImage = visionSystem.getNameImageResultFromCamera(true);
                        visionSystem.Disconnect();
                        showMessageAndImage("inspección número 1 ha sido exitosa.");
                        Thread.Sleep(300);
                        Debug.WriteLine($"{DateTime.Now} - " + "INSPECCION 1 DE VISION OK ");
                        if (isCancellationRequested) { return; };

                        showMessageAndImage("Realizando Inspeccion 2...");
                        Thread.Sleep(300);

                        visionSystem = new VisionSystem(sessionApp, eTypeConnection.Camara_2);
                        if (!visionSystem.FirstInspectionAttempt(serial))
                        {
                            bitmapImage = visionSystem.getNameImageResultFromCamera(false);
                            visionSystem.Disconnect();
                            if (!sensorsIO.WasPressedOpto())
                            {
                                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                Debug.WriteLine($"{DateTime.Now} - " + "Fallo primer intento ESPERA ACTIVACION DE OPTO ");
                                showMessageAndImage("Fallo primer intento, reacomode y presione OPTO");
                                if (isCancellationRequested) { return; };

                                if (!visionSystem.SecondInspectionAttempt())
                                {
                                    bitmapImage = visionSystem.getNameImageResultFromCamera(false);
                                    visionSystem.Disconnect();
                                    if (!sensorsIO.WasPressedOpto())
                                    {
                                        await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                        Debug.WriteLine($"{DateTime.Now} - " + "Fallo segundo intento ESPERA ACTIVACION DE OPTO ");
                                        showMessageAndImage("Fallo segundo intento,reacomode y presione OPTO");
                                        if (isCancellationRequested) { return; };

                                        if (!visionSystem.ThirdInspectionAttempt())
                                        {
                                            bitmapImage = visionSystem.getNameImageResultFromCamera(false);
                                            visionSystem.Disconnect();
                                            showMessageAndImage("Fallaron los 3 intentos");
                                            Debug.WriteLine($"{DateTime.Now} - " + "Fallaron los 3 intentos");  ///Falta poner que hace en este caso
                                            isCancellationRequested = true;
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        bitmapImage = visionSystem.getNameImageResultFromCamera(true);
                        visionSystem.Disconnect();
                        showMessageAndImage("inspección número 2 ha sido exitosa.");
                        Thread.Sleep(300);
                        Debug.WriteLine($"{DateTime.Now} - " + "INSPECCION 2 DE VISION OK ");
                        if (isCancellationRequested) { return; };

                        showMessageAndImage("Realizando Inspeccion 3...");
                        Thread.Sleep(300);

                        visionSystem = new VisionSystem(sessionApp, eTypeConnection.Camara_3);
                        if (!visionSystem.FirstInspectionAttempt(serial))
                        {
                            bitmapImage = visionSystem.getNameImageResultFromCamera(false);
                            visionSystem.Disconnect();
                            if (!sensorsIO.WasPressedOpto())
                            {
                                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                Debug.WriteLine($"{DateTime.Now} - " + "Fallo primer intento ESPERA ACTIVACION DE OPTO ");
                                showMessageAndImage("Fallo primer intento, reacomode y presione OPTO");
                                if (isCancellationRequested) { return; };

                                if (!visionSystem.SecondInspectionAttempt())
                                {
                                    bitmapImage = visionSystem.getNameImageResultFromCamera(false);
                                    visionSystem.Disconnect();
                                    if (!sensorsIO.WasPressedOpto())
                                    {
                                        await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                        Debug.WriteLine($"{DateTime.Now} - " + "Fallo segundo intento ESPERA ACTIVACION DE OPTO ");
                                        showMessageAndImage("Fallo segundo intento,reacomode y presione OPTO");
                                        if (isCancellationRequested) { return; };

                                        if (!visionSystem.ThirdInspectionAttempt())
                                        {
                                            bitmapImage = visionSystem.getNameImageResultFromCamera(false);
                                            visionSystem.Disconnect();
                                            showMessageAndImage("Fallaron los 3 intentos");
                                            Debug.WriteLine($"{DateTime.Now} - " + "Fallaron los 3 intentos");  ///Falta poner que hace en este caso
                                            isCancellationRequested = true;
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        bitmapImage = visionSystem.getNameImageResultFromCamera(true);
                        visionSystem.Disconnect();
                        showMessageAndImage("inspección número 3 ha sido exitosa.");
                        Thread.Sleep(300);
                        Debug.WriteLine($"{DateTime.Now} - " + "INSPECCION 3 DE VISION OK ");
                        if (isCancellationRequested) { return; };

                        showMessageAndImage("Coloque Mascara sobre Housing", "HousingWithMask.png");
                        await CheckSensorAndWait(() => sensorsIO.MaskOnHousing(), "Esperamos maskhousing");
                        if (isCancellationRequested) { return; };


                        screws = new Screws(sessionApp);
                        quantityScrews = screws.retriveNumberScrewsPerModel(sessionApp.ModelScrewSelected);
                        List<Screw> lstScrewsToProcess = screws.retriveScrewsToProcess(sessionApp.ModelScrewSelected);
                        if (lstScrewsToProcess.Count != 0 && (quantityScrews == lstScrewsToProcess.Count))
                        {
                            ergoArm = new ErgoArm(sessionApp);
                            screwdriver = new ScrewDriver(sessionApp);
                            int tightenincount = 1;
                            foreach (var screw in lstScrewsToProcess)
                            {
                                showMessageAndImage($"Realice el atornillado número: {tightenincount}");
                                screw.tighteningprocess = new TighteningProcess();
                                ergoArm.startReadPositionRespectScrew(screw);
                                if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                {
                                    Debug.WriteLine($"{DateTime.Now} - " + "BRAZO ERGONOMICO EN POSICION");                                    
                                    if (!screwdriver.FirstTighteningAttempt(screw))
                                    {
                                        sensorsIO.Turn_ON_Vacuumm();
                                        showMessageAndImage($"Fallo el atornillado número: {tightenincount}. Retire tornillo y coloque en desposito de tonrillos desechados", "Scrap2.jpg");
                                        await CheckSensorAndWait(() => sensorsIO.ScrewInScrap(), "Esperamos que el operador coloque el tornillo en el scrap");
                                        if (isCancellationRequested) { return; };

                                        //if (!sensorsIO.WasPressedOpto())
                                        //{
                                        //await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                        //Debug.WriteLine($"{DateTime.Now} - " + "Fallo primer intento de atornillado  ESPERA ACTIVACION DE OPTO ");
                                        if (!screwdriver.SecondTighteningAttempt(screw))
                                        {
                                            sensorsIO.Turn_ON_Vacuumm();
                                            showMessageAndImage($"Fallo el atornillado número: {tightenincount}. Retire tornillo y coloque en desposito de tonrillos desechados", "Scrap2.jpg");
                                            await CheckSensorAndWait(() => sensorsIO.ScrewInScrap(), "Esperamos que el operador coloque el tornillo en el scrap");
                                            if (isCancellationRequested) { return; };

                                            //if (!sensorsIO.WasPressedOpto())
                                            //{
                                            //    await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                            //    Debug.WriteLine($"{DateTime.Now} - " + "Fallo segundo intento de atornillado ESPERA ACTIVACION DE OPTO ");

                                            if (screwdriver.ThirdTighteningAttempt(screw))
                                            {
                                                Debug.WriteLine($"{DateTime.Now} - " + "Fallaron los 3 intentos de atornillado");  ///Falta poner que hace en este caso
                                            }
                                            //}
                                        }
                                        //}
                                    }
                                }
                                tightenincount++;
                                RequestCreateTextBox($"{screw.tighteningprocess.Torque} Nw | {screw.tighteningprocess.Angle} °", 0, -150);
                            }
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                            Debug.WriteLine($"{DateTime.Now} - " + "PIDE A OPERADOR COLOCAR BRAZO EN HOME Y RETIRAR MASCARA");
                            if (!ergoArm.isInHome())
                            {
                                ergoArm.WaitingResponse(ergoArm.isInHome());
                                ergoArm.endReadPostion();
                            }

                            if (!sensorsIO.MaskInHolder())
                            {
                                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.MaskInHolder());
                            }
                            Debug.WriteLine($"{DateTime.Now} - " + "PIDE A OPERADOR TOMAR INSULADOR, COLOCAR SOBRE ULTRA CAP BOARD Y ACTIVAR OPTO");
                            if (!sensorsIO.WasPressedOpto())
                            {
                                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                Debug.WriteLine($"{DateTime.Now} - " + "OPTO ACTIVADO");
                            }

                            visionSystem = new VisionSystem(sessionApp, eTypeConnection.Camara_3);
                            if (!visionSystem.FirstInspectionAttempt(serial))
                            {
                                if (!sensorsIO.WasPressedOpto())
                                {
                                    await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                    Debug.WriteLine($"{DateTime.Now} - " + "Fallo primer intento ESPERA ACTIVACION DE OPTO ");

                                    if (!visionSystem.SecondInspectionAttempt())
                                    {
                                        if (!sensorsIO.WasPressedOpto())
                                        {
                                            await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                            Debug.WriteLine($"{DateTime.Now} - " + "Fallo segundo intento ESPERA ACTIVACION DE OPTO ");

                                            if (!visionSystem.ThirdInspectionAttempt())
                                            {
                                                Debug.WriteLine($"{DateTime.Now} - " + "Fallaron los 3 intentos ");  ///Falta poner que hace en este caso
                                            }
                                        }
                                    }
                                }
                            }
                            //visionSystem.getNameImageResultFromCamera();
                            visionSystem.Disconnect();
                            Debug.WriteLine($"{DateTime.Now} - " + "INSPECCION OK ENVIA BCMP A FIS");


                            showMessageAndImage("TOMAR TOP COVER Y COLOCARLO DEBAJO DEL SCANNER 2", "KYC_Scanner.jpg");
                            //await CheckSensorAndWait(() => sensorsIO.ExtendedPalletClamp(), "Esperamos CLAMP DE PALLET EXTENDIDO");
                            if (isCancellationRequested) { return; };

                            showMessageAndImage("SCANNER 2 LEE CODIGO SERIAL", "KYC_Scanner.jpg");
                            scanner = new Scanner(sessionApp, eTypeConnection.Scan_2);
                            serial = scanner.ScanQR("LON");
                            if (isCancellationRequested) { return; };

                            showMessageAndImage("SE ENVIA BREQ DE TOP COVER A FIS", "");
                            //fIS = new CommunicationFIS(sessionApp);
                            //dataFIS = fIS.SendBREQToFIS(serial);
                            if (isCancellationRequested) { return; };

                            showMessageAndImage("COLOCAR TOP COVER EN EL HOUSING Y PRESIONAR OPTO", "");
                            //await CheckSensorAndWait(() => sensorsIO.ExtendedPalletClamp(), "Esperamos CLAMP DE PALLET EXTENDIDO");
                            if (isCancellationRequested) { return; };

                            showMessageAndImage("SE ENVIA BREQ DE TOP COVER A FIS", "");
                            //fIS = new CommunicationFIS(sessionApp);
                            //dataFIS = fIS.BCMP(serial, dataFIS.from_fis == "PASS");
                            if (isCancellationRequested) { return; };

                            showMessageAndImage("RETRAE CLAMP DE PALLET", "");
                            showMessageAndImage("DETECTA CLAMP DE PALLET RETRAIDO", "");
                            showMessageAndImage("LIBERA PALLET ", "");
                        }
                        else
                        {
                            Debug.WriteLine($"{DateTime.Now} - " + "La informacion correspondiente a los tornillos esta incompleta");
                        }
                    }
                    else
                    {
                        showMessageAndImage("Error: Fallo la confirmación de FIS");
                    }
                }
                else
                {
                    showMessageAndImage("Error: Fallo la confirmación de FIS");
                }
            }
            else
            {
                showMessageAndImage("Error: Fallo la confirmación de FIS");
            }
        }
            */
        public async Task CheckSensorAndWait(Func<bool> sensorCheck, string debugMessage)
        {
            if (!sensorCheck())
            {
                Debug.WriteLine($"{DateTime.Now} - {debugMessage}");
                _cancellationTokenSource = new CancellationTokenSource();
                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorCheck());
            }
        }
        public async Task CheckSensorAndWaitByTime(Func<bool> sensorCheck, string debugMessage, int time)
        {
            if (!sensorCheck())
            {
                Debug.WriteLine($"{DateTime.Now} - {debugMessage}");
                _cancellationTokenSource = new CancellationTokenSource();
                await sensorsIO.WaitingResponseByTime(_cancellationTokenSource, sensorCheck(), time);
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

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
            sensorsIO = new SensorsIO(sessionApp);
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

        public async Task showMessageAndImage(string message, string nameimage = "", bool isImageInDiferentPath = false)
        {
            await Task.Run(() =>
            {
                sessionApp.MessageOfProcess = message;
                sessionApp.ImageOfProcess = isImageInDiferentPath ? nameimage : sessionApp.PathOperationalImages + nameimage;
                Debug.WriteLine($"{DateTime.Now} - " + "Msg:" + message + " -  Image show:" + nameimage);
                Thread.Sleep(3000);
            });
        }
        public bool Validation_by_FIS(string serial, string show_message, eTypeSendToFIS typeSendToFIS, bool isPass = false)
        {
            CommunicationFIS fIS;
            DataFIS dataFIS;
            bool isPASS_From_FIS = false;

            if (sessionApp.settings.FirstOrDefault(x => x.setting.Equals("EneableFIS")).valueSetting == "1")
            {
                showMessageAndImage(show_message);
                fIS = new CommunicationFIS(sessionApp);
                switch (typeSendToFIS)
                {
                    case eTypeSendToFIS.BREQ:
                        dataFIS = fIS.SendBREQToFIS(serial);
                        sessionApp.QR.To_FIS = dataFIS.to_fis;
                        sessionApp.QR.From_FIS = dataFIS.from_fis;
                        isPASS_From_FIS = dataFIS.from_fis.Contains("PASS");

                        break;
                    case eTypeSendToFIS.BCMP:
                        dataFIS = fIS.BCMP(serial, isPass);
                        sessionApp.QR.To_FIS = dataFIS.to_fis;
                        sessionApp.QR.From_FIS = dataFIS.from_fis;
                        isPASS_From_FIS = dataFIS.from_fis.Contains("ACK");
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

        /*
        public override async Task StartProcess()
        {
            Debug.WriteLine($"{DateTime.Now} - " + "Path de Imagenes:" + sessionApp.PathOperationalImages);

            await Task.Run(() =>
            {

                RequestCreateTextBox($"18.5 Nw | 23 °", 340, -150);
                RequestCreateTextBox($"18.5 Nw | 23 °", 0, -150);
                RequestCreateTextBox($"18.5 Nw | 23 °", 305, -70);
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
                //showMessageAndImage("Los 3 intentos han fallado. ");
                //Thread.Sleep(3000);  ///Falta poner que hace en este caso

                showMessageAndImage("La informacion correspondiente a los tornillos esta incompleta");
                Thread.Sleep(3000);
                showMessageAndImage("Finaliza Proceso de atornillado", "image_not_found.jpg");
                sessionApp.TaksRunExecuting = false;
            });
        }
        */

        public override async Task StartProcess()
        {
            VisionSystem visionSystem;
            Screws screws;
            ErgoArm ergoArm;
            ScrewDriver screwdriver;
            string resultImageVisionSystem;

            string serial;
            string resultFIS;
            int quantityScrews;

            sensorsIO.startRead();
            _cancellationTokenSource = new CancellationTokenSource();


            await showMessageAndImage("A la espera del producto.", "Housing.png");
            await CheckSensorAndWait(() => sensorsIO.PalletInStopper(), "Esperamos pallet en Pre-Stopper");


            await sensorsIO.SecurePallet(_cancellationTokenSource);
            if (isCancellationRequested) { return; };

            await showMessageAndImage("Extendiendo el candado.", "MGPM25-10Z.png");
            sensorsIO.ExtendedPalletClamp();

            if (sensorsIO.isExtendedClamp())
            {
                await showMessageAndImage("Candado extendido.", "MGPM25-10Z.png");
            }
            else
            {
                await CheckSensorAndWaitByTime(() => sensorsIO.isExtendedClamp(), "Esperamos CLAMP DE PALLET EXTENDIDO por 5 segundos.", 5000);
                if (!sessionApp.TaksRunExecuting)
                {
                    isCancellationRequested = true;
                    await showMessageAndImage("El candado no se extendió correctamente. Se cancela el proceso. Favor de verificar.");
                }
                else
                {
                    await showMessageAndImage("Candado extendido.", "MGPM25-10Z.png");
                }
            }
            if (isCancellationRequested) { return; };

            await showMessageAndImage("Escaneando código QR del Housing.");
            Scanner scanner = new Scanner(sessionApp, eTypeConnection.Scan_1);
            serial = scanner.ScanQR("LON");
            sessionApp.QR.HOUSING = serial.Substring(0, (serial.Length - 1));
            if (isCancellationRequested) { return; };
            if (serial == "ERROR" || serial == string.Empty)
            {
                scanner.ScanQR("LOFF");
                scanner.DisconnectScanner();
            }


#if DEBUG
            if (true)
#else
            if (Validation_by_FIS(sessionApp.QR.HOUSING, "Se envía BREQ del housing a FIS.", eTypeSendToFIS.BREQ))
#endif

            {
                await showMessageAndImage("Inspección completada...");
                Thread.Sleep(300);
                await showMessageAndImage("Por favor, tome el HVDC COVER y colóquelo frente al escaner.", "ScannerHVDCCover.jpg");
                await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperando HVDC cover.");
                if (isCancellationRequested) { return; };

                await showMessageAndImage("Escaneando código QR del HVDC Cover.");
                serial = new Scanner(sessionApp, eTypeConnection.Scan_2).ScanQR("LON");
                sessionApp.QR.HVDC_BUSBAR = serial.Substring(0, (serial.Length - 1));
                if (isCancellationRequested) { return; };
                Thread.Sleep(00);
#if DEBUG
                if (true)
#else
                    if (Validation_by_FIS(sessionApp.QR.HOUSING, "Se envía BREQ a FIS.", eTypeSendToFIS.BREQ))
#endif

                {
                    await showMessageAndImage("Inspección completada...");
                    Thread.Sleep(300);

                    await CheckSensorAndWait(() => sensorsIO.isOutPieceHDVC(), "Esperando que quiten la pieza HVDC cover.");
                    await showMessageAndImage("Por favor, tome cable arnés y colóquelo frente al escaner.", "ScannerHarness.jpg");
                    await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperamos que el operador coloque el arnés en el scaner.");
                    if (isCancellationRequested) { return; };

                    await showMessageAndImage("Leyendo QR arnés.", "ScannerHarness.jpg");
                    serial = new Scanner(sessionApp, eTypeConnection.Scan_2).ScanQR("LON");
                    sessionApp.QR.HARNESS = serial.Substring(0, (serial.Length - 1));
                    if (isCancellationRequested) { return; };

#if DEBUG
                    if (true)
#else
                    if (Validation_by_FIS(sessionApp.QR.HOUSING, "Se envía BREQ árnes a FIS", eTypeSendToFIS.BREQ))
#endif
                    {
                        await showMessageAndImage("Inspección completada...");
                        Thread.Sleep(300);

                        await showMessageAndImage("Por favor, conecte árness y realice su ruteo, despues presione el opto.", "HousingRoute.png");
                        await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "Esperamos que el operador presione opto del árnes.");
                        if (isCancellationRequested) { return; };

                        await showMessageAndImage("Realizando inspección 1...");
                        Thread.Sleep(300);

                        visionSystem = new VisionSystem(sessionApp, eTypeConnection.Camara_1);
                        if (!visionSystem.FirstInspectionAttempt(serial))
                        {
                            resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(false);
                            visionSystem.Disconnect();
                            if (!sensorsIO.WasPressedOpto())
                            {


                                Debug.WriteLine($"{DateTime.Now} - " + "Fallo primer intento ESPERA ACTIVACION DE OPTO.");
                                await showMessageAndImage("El primer intento falló. Por favor, reacomode y presione el sensor óptico(OPTO).", resultImageVisionSystem, true);
                                await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "El primer intento falló. Por favor, reacomode y presione el sensor óptico(OPTO).");
                                if (isCancellationRequested) { return; };

                                if (!visionSystem.SecondInspectionAttempt(serial))
                                {
                                    resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(false);
                                    visionSystem.Disconnect();
                                    if (!sensorsIO.WasPressedOpto())
                                    {
                                        Debug.WriteLine($"{DateTime.Now} - " + "Fallo segundo intento ESPERA ACTIVACION DE OPTO.");
                                        await showMessageAndImage("El segundo intento falló. Por favor, reacomode y presione el sensor óptico(OPTO).", resultImageVisionSystem, true);
                                        await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "El segundo intento falló. Por favor, reacomode y presione el sensor óptico(OPTO).");
                                        if (isCancellationRequested) { return; };

                                        if (!visionSystem.ThirdInspectionAttempt(serial))
                                        {
                                            resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(false);
                                            visionSystem.Disconnect();
                                            await showMessageAndImage("Los 3 intentos han fallado.", resultImageVisionSystem, true);
                                            Thread.Sleep(3000);
                                            Debug.WriteLine($"{DateTime.Now} - " + "Los 3 intentos han fallado.");
                                            endOfProcess();
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(true);
                        visionSystem.Disconnect();
                        await showMessageAndImage("La inspección número 1 ha sido exitosa.", resultImageVisionSystem, true);
                        Thread.Sleep(3000);
                        Debug.WriteLine($"{DateTime.Now} - " + "INSPECCION 1 DE VISION OK.");
                        if (isCancellationRequested) { return; };

                        await showMessageAndImage("Realizando inspección 2...");
                        Thread.Sleep(300);

                        visionSystem = new VisionSystem(sessionApp, eTypeConnection.Camara_2);
                        if (!visionSystem.FirstInspectionAttempt(serial))
                        {
                            resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(false);
                            visionSystem.Disconnect();
                            if (!sensorsIO.WasPressedOpto())
                            {
                                Debug.WriteLine($"{DateTime.Now} - " + "Fallo primer intento ESPERA ACTIVACION DE OPTO. ");
                                await showMessageAndImage("El primer intento falló. Por favor, reacomode y presione el sensor óptico(OPTO).", resultImageVisionSystem, true);
                                await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "El primer intento falló. Por favor, reacomode y presione el sensor óptico(OPTO).");
                                if (isCancellationRequested) { return; };

                                if (!visionSystem.SecondInspectionAttempt(serial))
                                {
                                    resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(false);
                                    visionSystem.Disconnect();
                                    if (!sensorsIO.WasPressedOpto())
                                    {
                                        Debug.WriteLine($"{DateTime.Now} - " + "Fallo segundo intento ESPERA ACTIVACION DE OPTO.");
                                        await showMessageAndImage("El segundo intento falló. Por favor, reacomode y presione el sensor óptico(OPTO).", resultImageVisionSystem, true);
                                        await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "El segundo intento falló. Por favor, reacomode y presione el sensor óptico(OPTO).");
                                        if (isCancellationRequested) { return; };

                                        if (!visionSystem.ThirdInspectionAttempt(serial))
                                        {
                                            resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(false);
                                            visionSystem.Disconnect();
                                            await showMessageAndImage("Los 3 intentos han fallado.", resultImageVisionSystem, true);
                                            Thread.Sleep(3000);
                                            Debug.WriteLine($"{DateTime.Now} - " + "Los 3 intentos han fallado.");
                                            endOfProcess();
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(true);
                        visionSystem.Disconnect();
                        await showMessageAndImage("La inspección número 2 ha sido exitosa.", resultImageVisionSystem, true);
                        Thread.Sleep(3000);
                        Debug.WriteLine($"{DateTime.Now} - " + "INSPECCION 2 DE VISION OK.");
                        if (isCancellationRequested) { return; };

                        await showMessageAndImage("Realizando inspección 3...");
                        Thread.Sleep(300);

                        visionSystem = new VisionSystem(sessionApp, eTypeConnection.Camara_3);
                        if (!visionSystem.FirstInspectionAttempt(serial))
                        {
                            resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(false);
                            visionSystem.Disconnect();
                            if (!sensorsIO.WasPressedOpto())
                            {
                                Debug.WriteLine($"{DateTime.Now} - " + "Fallo primer intento ESPERA ACTIVACION DE OPTO.");
                                await showMessageAndImage("El primer intento falló. Reacomode y presione el sensor óptico(OPTO)", resultImageVisionSystem, true);
                                await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "El primer intento falló. Por favor, reacomode y presione el sensor óptico(OPTO).");
                                if (isCancellationRequested) { return; };

                                if (!visionSystem.SecondInspectionAttempt(serial))
                                {
                                    resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(false);
                                    visionSystem.Disconnect();
                                    if (!sensorsIO.WasPressedOpto())
                                    {
                                        Debug.WriteLine($"{DateTime.Now} - " + "Fallo segundo intento ESPERA ACTIVACION DE OPTO.");
                                        await showMessageAndImage("El segundo intento falló. Reacomode y presione el sensor óptico(OPTO).", resultImageVisionSystem, true);
                                        await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "El segundo intento falló. Por favor, reacomode y presione el sensor óptico(OPTO).");
                                        if (isCancellationRequested) { return; };

                                        if (!visionSystem.ThirdInspectionAttempt(serial))
                                        {
                                            resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(false);
                                            visionSystem.Disconnect();
                                            await showMessageAndImage("Los 3 intentos han fallado.", resultImageVisionSystem, true);
                                            Thread.Sleep(3000);
                                            Debug.WriteLine($"{DateTime.Now} - " + "Los 3 intentos han fallado.");
                                            endOfProcess();
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(true);
                        visionSystem.Disconnect();
                        await showMessageAndImage("La inspección número 3 ha sido exitosa.", resultImageVisionSystem, true);
                        Thread.Sleep(3000);
                        Debug.WriteLine($"{DateTime.Now} - " + "INSPECCION 3 DE VISION OK ");
                        if (isCancellationRequested) { return; };

                        await showMessageAndImage("Por favor, posicione la máscara sobre el housing.", "HousingWithMask.png");
                        await CheckSensorAndWait(() => sensorsIO.MaskOnHousing(), "Esperamos maskhousing");
                        if (isCancellationRequested) { return; };



                        screws = new Screws(sessionApp);
                        getModelScrew();
                        quantityScrews = screws.retriveNumberScrewsPerModel(sessionApp.ModelScrewSelected);
                        List<Screw> lstScrewsToProcess = screws.retriveScrewsToProcess(sessionApp.ModelScrewSelected);
                        if (lstScrewsToProcess.Count != 0 && (quantityScrews == lstScrewsToProcess.Count))
                        {
                            ergoArm = new ErgoArm(sessionApp);
                            ergoArm.Connect();
                            screwdriver = new ScrewDriver(sessionApp);
                            int tightenincount = 1;
                            foreach (var screw in lstScrewsToProcess)
                            {
                                sensorsIO.DispenseAScrew();
                                await showMessageAndImage($"Por favor, realice el atornillado número: {tightenincount}");
                                screw.tighteningprocess = new TighteningProcess();
                                if (ergoArm.isConected())
                                {
                                    ergoArm.startReadPositionRespectScrew(screw);
                                }
                                if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                {
                                    Debug.WriteLine($"{DateTime.Now} - " + "BRAZO ERGONOMICO EN POSICION");
                                    if (!screwdriver.FirstTighteningAttempt(screw))
                                    {
                                        sensorsIO.Turn_ON_Vacuumm();
                                        await showMessageAndImage($"El atornillado número : {tightenincount} ha fallado. Por favor, retire el tornillo y colóquelo en desposito de tonrillos desechados.", "Scrap2.jpg");
                                        await CheckSensorAndWait(() => sensorsIO.ScrewInScrap(), "Esperamos que el operador coloque el tornillo en el scrap");
                                        if (isCancellationRequested) { return; };

                                        //if (!sensorsIO.WasPressedOpto())
                                        //{
                                        //await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                        //Debug.WriteLine($"{DateTime.Now} - " + "Fallo primer intento de atornillado  ESPERA ACTIVACION DE OPTO ");
                                        await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "Fallo primer intento de atornillado  ESPERA ACTIVACION DE OPTO");
                                        if (!screwdriver.SecondTighteningAttempt(screw))
                                        {
                                            sensorsIO.Turn_ON_Vacuumm();
                                            await showMessageAndImage($"El atornillado número : {tightenincount} ha fallado. Por favor, retire el tornillo y colóquelo en desposito de tonrillos desechados.", "Scrap2.jpg");
                                            await CheckSensorAndWait(() => sensorsIO.ScrewInScrap(), "Esperamos que el operador coloque el tornillo en el scrap");
                                            if (isCancellationRequested) { return; };

                                            //if (!sensorsIO.WasPressedOpto())
                                            //{
                                            //    await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorsIO.WasPressedOpto());
                                            //    Debug.WriteLine($"{DateTime.Now} - " + "Fallo segundo intento de atornillado ESPERA ACTIVACION DE OPTO ");
                                            await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "Fallo segundo intento de atornillado  ESPERA ACTIVACION DE OPTO");
                                            if (screwdriver.ThirdTighteningAttempt(screw))
                                            {
                                                await showMessageAndImage($"Los 3 intentos de atornillado han fallado.");
                                                Debug.WriteLine($"{DateTime.Now} - " + "Los 3 intentos de atornillado han fallado.");  ///Falta poner que hace en este caso
                                                endOfProcess();
                                            }
                                            //}
                                        }
                                        //}
                                    }
                                }
                                tightenincount++;
                                RequestCreateTextBox($"{screw.tighteningprocess.Torque} Nw | {screw.tighteningprocess.Angle} °", screw.text_position_X, screw.text_position_Y);
                            }//Finaliza el proceso de atornillado                            
                            await showMessageAndImage("Por favor, coloque el atornillador en la posición 1.", "HousingWithMask.png");
                            Thread.Sleep(1000);
                            RequestRemoveTextBox();
                            //await showMessageAndImage("Por favor, retire la máscara y colóquela en su base", "MaskInHolder.jpg");                            
                            //await CheckSensorAndWait(() => sensorsIO.MaskInHolder(), "Esperamos maskhousing");
                            //if (isCancellationRequested) { return; };

                            await showMessageAndImage("Por favor, tome la cubierta superior y colóquela frente al escáner.", "TopCover_Scanner.jpg");
                            await CheckSensorAndWait(() => sensorsIO.MaskInHolder(), "Esperamos maskhousing");
                            if (isCancellationRequested) { return; };

                            await showMessageAndImage("Escaneando código QR de la cubierta superior.");
                            serial = new Scanner(sessionApp, eTypeConnection.Scan_2).ScanQR("LON");
                            if (isCancellationRequested) { return; };
                            sessionApp.QR.TOP_COVER = serial.Substring(0, (serial.Length - 1));

                            if (Validation_by_FIS(serial, "Se envía BREQ a FIS.", eTypeSendToFIS.BREQ))
                            {
                                await showMessageAndImage("Inspección completada...");
                                Thread.Sleep(300);
                                await showMessageAndImage("Por favor, ensamble la cubierta superior y presione el sensor óptico(OPTO).", "ScannerHVDCCover.jpg");
                                await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperando HVDC cover");
                                if (isCancellationRequested) { return; };

                                await showMessageAndImage("Enviando señal de finalización de tarea.");
                                Thread.Sleep(300);
                                if (Validation_by_FIS(serial, "Se envía BCMP a FIS.", eTypeSendToFIS.BCMP))
                                {
                                    await showMessageAndImage("Recepción de BACK completada.");
                                    Thread.Sleep(1000);
                                    if (isCancellationRequested) { return; };

                                    await showMessageAndImage("Retrayendo el candado.", "MGPM25-10Z.png");
                                    sensorsIO.RetractPalletClamp();
                                    await CheckSensorAndWaitByTime(() => sensorsIO.isRetractedClamp(), "Esperamos CLAMP DE PALLET Retraido por 5 segundos.", 5000);
                                    if (!sessionApp.TaksRunExecuting)
                                    {
                                        isCancellationRequested = true;
                                        await showMessageAndImage("El candado no se replegó correctamente. Por favor, reinicie y verifique.");
                                    }
                                    else
                                    {
                                        await showMessageAndImage("El candado se replegó correctamente.", "MGPM25-10Z.png");
                                        Thread.Sleep(1000);
                                        await showMessageAndImage("Esperando la estación 13.");
                                        await CheckSensorAndWait(() => sensorsIO.ST13Available(), "Esperamos la estación 13.");
                                        if (isCancellationRequested) { return; };

                                        await showMessageAndImage("Por favor, retire el pallet de la estación.");
                                        sensorsIO.StopCylinder();
                                        await CheckSensorAndWait(() => sensorsIO.PalletOutStopper(), "Esperamos la estación 13.");
                                        if (isCancellationRequested) { return; };
                                        await showMessageAndImage("Pallet Retirado.");
                                        Thread.Sleep(1000);
                                        await showMessageAndImage("¡El ciclo ha concluido exitosamente!");
                                    }
                                }
                                else
                                {
                                    await showMessageAndImage("Error: Falló la cubierta superior, BACK de FIS.");
                                }
                            }
                            else
                            {
                                await showMessageAndImage("Error: Falló la cubierta superior, BACK de FIS.");
                            }
                        }
                        else
                        {
                            Debug.WriteLine($"{DateTime.Now} - " + "La informacion correspondiente a los tornillos esta incompleta");
                        }
                    }
                    else
                    {
                        await showMessageAndImage("Error: Fallo la confirmación de FIS");
                    }
                }
                else
                {
                    await showMessageAndImage("Error: Fallo la confirmación de FIS");
                }
            }
            else
            {
                await showMessageAndImage("Error: Fallo la confirmación de FIS");
            }
            endOfProcess();


        }
        public void getModelScrew()
        {
            sessionApp.ModelScrewSelected = int.Parse(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("Model_Screw")).valueSetting);
        }

        public void endOfProcess()
        {
            sessionApp.TaksRunExecuting = false;
        }
        public async Task CheckSensorAndWait(Func<bool> sensorCheck, string debugMessage)
        {
            if (!sensorCheck())
            {
                Debug.WriteLine($"{DateTime.Now} - {debugMessage}");
                _cancellationTokenSource = new CancellationTokenSource();
                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorCheck);
            }
        }
        public async Task CheckSensorAndWaitByTime(Func<bool> sensorCheck, string debugMessage, int time)
        {
            if (!sensorCheck())
            {
                Debug.WriteLine($"{DateTime.Now} - {debugMessage}");
                _cancellationTokenSource = new CancellationTokenSource();
                await sensorsIO.WaitingResponseByTime(_cancellationTokenSource, sensorCheck, time);
            }
        }
        public override void RequestCreateTextBox(string msg, int PositionX, int PositionY)
        {
            OnCreateTextBoxRequested(new TextBoxInfoEventArgs
            {
                Text = msg,
                Position = new System.Windows.Point(PositionX, PositionY)
            });
        }

        public override void RequestRemoveTextBox()
        {
            RemoveTextBoxRequested?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnCreateTextBoxRequested(TextBoxInfoEventArgs e)
        {
            CreateTextBoxRequested?.Invoke(this, e);
        }
    }
}

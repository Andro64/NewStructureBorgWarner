using BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation
{
    class WorkStation_Manual_Type12 : Workstation
    {
        public override string Type => " ╭∩╮( •̀_•́ )╭∩╮ \n WS Automatica Tipo 12";

        SensorsIOGeneric sensorsIO;
        //SensorsIO sensorsIO;
        SessionApp sessionApp;
        private CancellationTokenSource _cancellationTokenSource;
        private bool isCancellationRequested = false;
        TighteningProcess tightening;
        private IMessageBoxService messageBoxService;

        public override event EventHandler<TextBoxInfoEventArgs> CreateTextBoxRequested;
        public override event EventHandler RemoveTextBoxRequested;

        public WorkStation_Manual_Type12(SessionApp _sessionApp)
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
                sensorsIO.endRead();
            }
            sessionApp.IOSensorsGenerics.ClearAllBooleanProperties();

            //sessionApp.Sensors_M1.Main_Pressure = false;
            //sessionApp.Sensors_M1.OptoBtn = false;
            //sessionApp.Sensors_M1.Pallet_Pre_Stopper = false;
            //sessionApp.Sensors_M1.Pallet_Stopper = false;
            //sessionApp.Sensors_M1.Screw_Present_Oth = false;
            //sessionApp.Sensors_M1.Screw_Level_Oth = false;
            //sessionApp.Sensors_M1.MaskInHolder = false;
            //sessionApp.Sensors_M1.SecurityOK = false;

            //sessionApp.Sensors_M2.Trigger_Scanner = false;
            //sessionApp.Sensors_M2.MaskatHousing = false;
            //sessionApp.Sensors_M2.PA2 = false;
            //sessionApp.Sensors_M2.PA3 = false;
            //sessionApp.Sensors_M2.Cyl_Fixing_Pall_Ext = false;
            //sessionApp.Sensors_M2.Cyl_Fixing_Pall_Ret = false;
            //sessionApp.Sensors_M2.PB2 = false;
            //sessionApp.Sensors_M2.PB3 = false;

            //sessionApp.Sensors_M3.PA0 = false;
            //sessionApp.Sensors_M3.PA1 = false;
            //sessionApp.Sensors_M3.PA2 = false;
            //sessionApp.Sensors_M3.ST13Available = false;
            //sessionApp.Sensors_M3.PB0 = false;
            //sessionApp.Sensors_M3.PB1 = false;
            //sessionApp.Sensors_M3.Scrap_presence = false;
            //sessionApp.Sensors_M3.PB3 = false;

            sessionApp.images = new List<string>();

        }

        public async Task showMessageAndImage(string message, string nameimage = "", bool isImageInDiferentPath = false, int timeshowing = 100)
        {
            await Task.Run(() =>
            {
                sessionApp.MessageOfProcess = message;
                if (!isImageInDiferentPath && nameimage == "")
                {
                    sessionApp.OnlyMessageOfProcess = true;
                }
                else
                {
                    sessionApp.OnlyMessageOfProcess = false;
                    sessionApp.ImageOfProcess = isImageInDiferentPath ? nameimage : sessionApp.PathOperationalImages + nameimage;
                }

                Debug.WriteLine($"{DateTime.Now} - " + "Msg:" + message + " -  Image show:" + nameimage);
                Thread.Sleep(timeshowing);
            });
        }
        public bool Validation_by_FIS(string serialParent, string serialChild, string show_message, eTypeSendToFIS typeSendToFIS, bool isTighteningOK = false, List<Screw> lstScrewsToProcess = null, int MaxNumberAttempts = 0)
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
                        dataFIS = fIS.SendBREQToFIS(serialParent == string.Empty ? serialChild : serialParent);
                        sessionApp.QR.To_FIS = dataFIS.to_fis;
                        sessionApp.QR.From_FIS = dataFIS.from_fis;
                        isPASS_From_FIS = dataFIS.from_fis.Contains("PASS");

                        break;
                    case eTypeSendToFIS.BCMP:
                        if (lstScrewsToProcess != null)
                        {
                            dataFIS = fIS.BCMP(serialParent, serialChild, isTighteningOK, lstScrewsToProcess, MaxNumberAttempts);
                        }
                        else
                        {
                            dataFIS = fIS.BCMP(serialParent, serialChild, isTighteningOK);
                        }
                        sessionApp.QR.To_FIS = dataFIS.to_fis;
                        sessionApp.QR.From_FIS = dataFIS.from_fis;
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
            VisionSystem visionSystem;
            Screws screws;
            ErgoArm ergoArm;
            ScrewDriver screwdriver;
            List<string> imagesVisionSystem = new List<string>();
            string resultImageVisionSystem;
            bool isFISEneable = false;
            bool isVisionEneable = false;
            Scanner scanner = new Scanner(sessionApp, eTypeConnection.Scan_1);

            Scanner scanner3 = new Scanner(sessionApp, eTypeConnection.Scan_2);
            List<string> serials = new List<string>();
            const int MaxNumberAttempts = 3;

            string serial;
            string resultFIS;
            int quantityScrews;

            isCancellationRequested = false;
            if (sensorsIO != null)
            {
                sensorsIO.StopWaiting();
                sensorsIO.endRead();
            }

            sensorsIO = new SensorsIOGeneric(sessionApp);
            //sensorsIO = new SensorsIO(sessionApp);

            sensorsIO.startRead();
            _cancellationTokenSource = new CancellationTokenSource();
            sessionApp.areImagePASSProcessFinished = false;
            sessionApp.images = new List<string>();

            isFISEneable = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("EneableFIS")).valueSetting == "1";
            isVisionEneable = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("EneableVision")).valueSetting == "1";

            sessionApp.MessageOfProcessDebug = "Inicializando sistema";
            
            sensorsIO.ResetScrap();
            sensorsIO.ReleScrapOFF();

            /***Prueba de etiquetas***/
            //await showMessageAndImage($"Por favor, realice el desatornillado del tornillo número:", "HousingWithMask.png");

            //await DrawingSight(265, -15, eStyleText.Normal);
            ////await DrawingSight(330, 10, eStyleText.InPosition);
            ////await DrawingSight(330, 10, eStyleText.Normal);
            ////await DrawingSight(330, 10, eStyleText.Success);
            //RequestCreateTextBox($"0000.00 Nw | 00°", 390, -15, 100, 30);

            ////RequestRemoveTextBox();
            //RequestCreateTextBox($"0000.00 Nw | 00°", 375, -215, 100, 30);

            ////await DrawingSight(250, -60, eStyleText.Normal);
            //await DrawingSight(250, -215, eStyleText.InPosition);
            ////await DrawingSight(250, -60, eStyleText.Normal);
            ////await DrawingSight(250, -60, eStyleText.Success);
            ////await DrawingSight(250, -60, eStyleText.Error);


            //await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperamos que el operador coloque el arnés en el scaner.");


            /******/

            await sensorsIO.Sequence_Stoper_PrestoperAsync(_cancellationTokenSource, false);
            if (isCancellationRequested) { return; };

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
            //Scanner scanner = new Scanner(sessionApp, eTypeConnection.Scan_1);
            serial = scanner.ScanQR("LON");
            //serial = await scanner.ScanningTrigger(_cancellationTokenSource, "LON");

            serial = serial == null ? string.Empty : serial;
            sessionApp.QR.HOUSING = serial;// != string.Empty ? serial.Substring(0, (serial.Length - 1)) : "";            

            if (serial == "ERROR\r" || serial == string.Empty)
            {
                await showMessageAndImage("Error al momento de escanear el código QR del Housing. Favor de verificar.");
                FinshProcessByErrors();
                await sensorsIO.Sequence_Stoper_PrestoperAsync(_cancellationTokenSource, true);
                return;
            }
            else
            {
                serials.Add(serial);
            }

            scanner.DisconnectScanner();
            if (isCancellationRequested) { return; };


            if (isFISEneable ? Validation_by_FIS(sessionApp.QR.HOUSING, string.Empty, "Se envía BREQ del housing a FIS.", eTypeSendToFIS.BREQ) : true)
            {
                await showMessageAndImage("Inspección completada...");
                Thread.Sleep(50); //Thread.Sleep(300);
                await showMessageAndImage("Por favor, tome el HVDC COVER y colóquelo frente al escaner.", "ScannerHVDCCover.jpg");
                await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperando HVDC cover.");
                if (isCancellationRequested) { return; };



                //if (isFISEneable)
                //{
                while (!isCancellationRequested)
                {
                    await showMessageAndImage("Escaneando código QR del HVDC Cover.", "", false, 50);
                    Scanner scanner2 = new Scanner(sessionApp, eTypeConnection.Scan_2);
                    //serial = scanner2.ScanQR("LON");
                    serial = await scanner2.ScanningTrigger(_cancellationTokenSource, "LON", serials);
                    serials.Add(serial);
                    sessionApp.QR.HVDC_BUSBAR = serial;//.Substring(0, (serial.Length - 1));
                    scanner2.DisconnectScanner();
                    if (isCancellationRequested) { return; };
                    Thread.Sleep(50); //Thread.Sleep(300);
                    if (serial != string.Empty)
                    {
                        if (isFISEneable ? Validation_by_FIS(string.Empty, sessionApp.QR.HVDC_BUSBAR, "Se envía BREQ a FIS.", eTypeSendToFIS.BREQ) : true)
                        {
                            break;
                        }
                    }
                    //Thread.Sleep(10);
                }
                if (isCancellationRequested) { return; };
                //}


                await showMessageAndImage("Inspección completada...");
                Thread.Sleep(50); //Thread.Sleep(300);

                await showMessageAndImage("Por favor,remueva el HVDC COVER.", "ScannerHVDCCover.jpg");
                await CheckSensorAndWait(() => sensorsIO.isOutPieceHDVC(), "Esperando que quiten la pieza HVDC cover.");
                if (isCancellationRequested) { return; };
                await showMessageAndImage("Por favor, tome cable arnés y colóquelo frente al escaner.", "ScannerHarness.jpg");
                await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperamos que el operador coloque el arnés en el scaner.");
                if (isCancellationRequested) { return; };

                await showMessageAndImage("Leyendo QR arnés.", "ScannerHarness.jpg", false, 50);
                //Scanner scanner3 = new Scanner(sessionApp, eTypeConnection.Scan_2);
                //serial = scanner3.ScanQR("LON");
                serial = await scanner3.ScanningTrigger(_cancellationTokenSource, "LON", serials);
                serials.Add(serial);
                sessionApp.QR.HARNESS = serial;//.Substring(0, (serial.Length - 1));
                scanner3.DisconnectScanner();
                if (isCancellationRequested) { return; };


                //if (isFISEneable ? Validation_by_FIS(sessionApp.QR.HARNESS, "Se envía BREQ árnes a FIS", eTypeSendToFIS.BREQ) : true)                    
                //{
                //    await showMessageAndImage("Inspección completada...");
                //    Thread.Sleep(50); //Thread.Sleep(300);

                await showMessageAndImage("Por favor, conecte árness y realice su ruteo, despues presione el opto.", "HousingRoute.png");
                await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "Esperamos que el operador presione opto del árnes.");
                if (isCancellationRequested) { return; };

                await showMessageAndImage("Realizando inspección 1...");
                Thread.Sleep(50); //Thread.Sleep(300);

                visionSystem = new VisionSystem(sessionApp, eTypeConnection.Camara_1);
                if (isVisionEneable ? !visionSystem.FirstInspectionAttempt(serial) : false)
                {
                    resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(false);
                    visionSystem.Disconnect();
                    if (!sensorsIO.WasPressedOpto() || resultImageVisionSystem == string.Empty)
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
                                    FinshProcessByErrors();
                                    return;
                                }
                            }
                        }
                    }
                }

                if (isVisionEneable)
                {
                    resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(true);
                    imagesVisionSystem.Add(resultImageVisionSystem);
                    sessionApp.images.Add(resultImageVisionSystem);
                    visionSystem.Disconnect();
                    await showMessageAndImage("La inspección número 1 ha sido exitosa.", resultImageVisionSystem, true);

                }
                else
                {
                    visionSystem.Disconnect();
                    await showMessageAndImage("La inspección número 1 ha sido exitosa.");
                }

                Thread.Sleep(3000);
                Debug.WriteLine($"{DateTime.Now} - " + "INSPECCION 1 DE VISION OK.");
                if (isCancellationRequested) { return; };

                await showMessageAndImage("Realizando inspección 2...");
                Thread.Sleep(300);

                visionSystem = new VisionSystem(sessionApp, eTypeConnection.Camara_2);
                if (isVisionEneable ? !visionSystem.FirstInspectionAttempt(serial) : false)
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
                                    FinshProcessByErrors();
                                    return;
                                }
                            }
                        }
                    }
                }
                if (isVisionEneable)
                {
                    resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(true);
                    imagesVisionSystem.Add(resultImageVisionSystem);
                    sessionApp.images.Add(resultImageVisionSystem);
                    visionSystem.Disconnect();
                    await showMessageAndImage("La inspección número 2 ha sido exitosa.", resultImageVisionSystem, true);
                }
                else
                {
                    visionSystem.Disconnect();
                    await showMessageAndImage("La inspección número 2 ha sido exitosa.");
                }

                Thread.Sleep(3000);
                Debug.WriteLine($"{DateTime.Now} - " + "INSPECCION 2 DE VISION OK.");
                if (isCancellationRequested) { return; };

                await showMessageAndImage("Realizando inspección 3...");
                Thread.Sleep(300);

                visionSystem = new VisionSystem(sessionApp, eTypeConnection.Camara_3);
                if (isVisionEneable ? !visionSystem.FirstInspectionAttempt(serial) : false)
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
                                    FinshProcessByErrors();
                                    return;
                                }
                            }
                        }
                    }
                }

                if (isVisionEneable)
                {
                    resultImageVisionSystem = visionSystem.getNameImageResultFromCamera(true);
                    imagesVisionSystem.Add(resultImageVisionSystem);
                    sessionApp.images.Add(resultImageVisionSystem);
                    visionSystem.Disconnect();
                    await showMessageAndImage("La inspección número 3 ha sido exitosa.", resultImageVisionSystem, true);
                    Thread.Sleep(3000);


                    sessionApp.areImagePASSProcessFinished = true;
                    await showMessageAndImage("Este es el resultado de la inspeccion.");
                    Thread.Sleep(5000);
                    sessionApp.areImagePASSProcessFinished = false;

                    Debug.WriteLine($"{DateTime.Now} - " + "INSPECCION 3 DE VISION OK ");
                    if (isCancellationRequested) { return; };
                }
                else
                {
                    visionSystem.Disconnect();
                    await showMessageAndImage("La inspección número 3 ha sido exitosa.");
                }

                await showMessageAndImage("Por favor, posicione la máscara sobre el housing.", "HousingWithMask.png");
                await CheckSensorAndWait(() => sensorsIO.MaskOnHousing(), "Esperamos maskhousing");
                if (isCancellationRequested) { return; };

                screws = new Screws(sessionApp);
                getModelScrew();
                quantityScrews = screws.retriveNumberScrewsPerModel(sessionApp.ModelScrewSelected);
                List<Screw> lstScrewsToProcess = screws.retriveScrewsToProcess(sessionApp.ModelScrewSelected);
                if (lstScrewsToProcess.Count == 0 && (quantityScrews != lstScrewsToProcess.Count))
                {
                    Debug.WriteLine($"{DateTime.Now} - " + "La informacion correspondiente a los tornillos esta incompleta");
                    return;
                }


                ergoArm = new ErgoArm(sessionApp);
                ergoArm.Connect();
                screwdriver = new ScrewDriver(sessionApp);
                int tightenincount = 1;
                foreach (var screw in lstScrewsToProcess)
                {
                    sensorsIO.DispenseAScrew();
                    await showMessageAndImage($"Por favor, realice el atornillado número: {tightenincount}", "HousingWithMask.png");
                    screw.tighteningprocess = new TighteningProcess();
                    if (ergoArm.isConected())
                    {
                        await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Normal);
                        await showMessageAndImage($"Por favor, posiciones el brazo ergonomico del tornillo número: {tightenincount}.", "HousingWithMask.png");
                        ergoArm.startReadPositionRespectScrew(screw);
                    }
                    if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                    {

                        await showMessageAndImage($"Por favor, realice el atornillado número: {tightenincount}", "HousingWithMask.png");
                        Debug.WriteLine($"-------Primer intento atronillado");
                        
                        /***************************************** 1 Primer intento de atornillado ***********************/
                        tightening = await screwdriver.FirstTighteningAttempt(ergoArm, screw, _cancellationTokenSource);

                        if (tightening == null)
                        {
                            RewriteResultsOfTightening(lstScrewsToProcess);
                            await showMessageAndImage($"Por favor, realice el desatornillado del tornillo número: {tightenincount}.", "HousingWithMask.png");
                            await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Error);

                            ergoArm.startReadPositionRespectScrew(screw);
                            if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                            {
                                sensorsIO.ResetScrap();
                                sensorsIO.ReleScrapOFF();
                                RequestRemoveTextBox();
                                await showMessageAndImage($"El atornillado del tornillo número : {tightenincount} ha fallado. Por favor, retire el tornillo y colóquelo en desposito de tornillos desechados.", "Scrap2.jpg");
                                await screwdriver.Unscrewing(ergoArm, screw, _cancellationTokenSource);

                                await CheckSensorAndWait(() => sensorsIO.ScrewInScrap(), "Esperamos que el operador coloque el tornillo en el scrap");
                                if (isCancellationRequested) { return; };

                                await showMessageAndImage($"Coloque la punta del atornillador en la punta de la aspiradora.", "Vacuum.jpg");

                                await CheckSensorAndWait(() => ergoArm.isInVacuumNozzle(), "Esperamos ErgoArm en punta de la aspiradora");
                                if (isCancellationRequested) { return; };

                                
                                await showMessageAndImage($"Por favor, posiciones el brazo ergonomico del tornillo número: {tightenincount}, en donde ha fallado el atornillado y aspire.", "HousingWithMask.png",false,500);                                
                                await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Normal);
                                

                                ergoArm.startReadPositionRespectScrew(screw);
                                if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                {
                                    Thread.Sleep(100);
                                    Debug.WriteLine("*Estpy en posicion de aspiradora Intento 1");
                                    sensorsIO.ActivateVacumm_by_time(1000);     //3000
                                }

                                if (isCancellationRequested) { return; };

                                RewriteResultsOfTightening(lstScrewsToProcess);
                                //await showMessageAndImage($"El primer intento de atornillado del tornillo número : {tightenincount} ha fallado.Presione OPTO para continuar", "HousingWithMask.png");
                                //await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "Fallo primer intento de atornillado  ESPERA ACTIVACION DE OPTO");


                                //sensorsIO.ResetScrap();
                                sensorsIO.DispenseAScrew();

                                
                                await showMessageAndImage($"Intento 2 - Por favor, posiciones el brazo ergonomico del tornillo número: {tightenincount}.", "HousingWithMask.png");
                                await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Normal);
                                ergoArm.startReadPositionRespectScrew(screw);
                                if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                {
                                    RewriteResultsOfTightening(lstScrewsToProcess);

                                    await showMessageAndImage($"Intento 2 - Por favor, realice nuevamente el atornillado del tornillo número: {tightenincount}.", "HousingWithMask.png");
                                    await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Normal);
                                    /***************************************** 2 Segundo intento de atornillado ***********************/
                                    tightening = await screwdriver.SecondTighteningAttempt(ergoArm, screw, _cancellationTokenSource);
                                    if (tightening == null)
                                    {
                                        await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Error);
                                        //RewriteResultsOfTightening(lstScrewsToProcess);
                                        //await showMessageAndImage($"Por favor, realice el desatornillado del tornillo número: {tightenincount}.", "HousingWithMask.png");

                                        ergoArm.startReadPositionRespectScrew(screw);
                                        if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                        {
                                            sensorsIO.ResetScrap();
                                            sensorsIO.ReleScrapOFF();
                                            RequestRemoveTextBox();
                                            await showMessageAndImage($"El atornillado del tornillo número : {tightenincount} ha fallado. Por favor, retire el tornillo y colóquelo en desposito de tornillos desechados.", "Scrap2.jpg");
                                            await screwdriver.Unscrewing(ergoArm, screw, _cancellationTokenSource);


                                            await CheckSensorAndWait(() => sensorsIO.ScrewInScrap(), "Esperamos que el operador coloque el tornillo en el scrap");
                                            if (isCancellationRequested) { return; };

                                            await showMessageAndImage($"Coloque la punta del atornillador en la punta de la aspiradora.", "Vacuum.jpg");
                                            await CheckSensorAndWait(() => ergoArm.isInVacuumNozzle(), "Esperamos ErgoArm en punta de la aspiradora");
                                            if (isCancellationRequested) { return; };

                                            
                                            await showMessageAndImage($"Por favor, posiciones el brazo ergonomico del tornillo número: {tightenincount}, en donde ha fallado el atornillado y aspire.", "HousingWithMask.png",false,500);
                                            
                                            await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Normal);
                                            

                                            ergoArm.startReadPositionRespectScrew(screw);
                                            if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                            {
                                                Thread.Sleep(100);
                                                Debug.WriteLine("*Estpy en posicion de aspiradora Intento 2");
                                                sensorsIO.ActivateVacumm_by_time(1000);
                                            }
                                            if (isCancellationRequested) { return; };

                                            RewriteResultsOfTightening(lstScrewsToProcess);

                                            //sensorsIO.ResetScrap();
                                            sensorsIO.DispenseAScrew();

                                            
                                            await showMessageAndImage($"Intento 3 - Por favor, posiciones el brazo ergonomico del tornillo número: {tightenincount}.", "HousingWithMask.png");
                                            await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Normal);
                                            ergoArm.startReadPositionRespectScrew(screw);
                                            if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                            {
                                                RewriteResultsOfTightening(lstScrewsToProcess);

                                                await showMessageAndImage($"Intento 3 - Por favor, realice nuevamente el atornillado del tornillo número: {tightenincount}.", "HousingWithMask.png");
                                                await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Normal);
                                                /*****************************************  3 Tercer intento de atornillado ***********************/
                                                tightening = await screwdriver.ThirdTighteningAttempt(ergoArm, screw, _cancellationTokenSource);
                                                if (tightening == null)
                                                {
                                                    await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Error);

                                                    ergoArm.startReadPositionRespectScrew(screw);
                                                    if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                                    {

                                                        //await showMessageAndImage($"Por favor, realice el desatornillado del tornillo número: {tightenincount}.", "HousingWithMask.png");
                                                        sensorsIO.ResetScrap();
                                                        sensorsIO.ReleScrapOFF();
                                                        RequestRemoveTextBox();
                                                        await showMessageAndImage($"El atornillado número : {tightenincount} ha fallado. Por favor, retire el tornillo y colóquelo en desposito de tornillos desechados.", "Scrap2.jpg");
                                                        await screwdriver.Unscrewing(ergoArm, screw, _cancellationTokenSource);



                                                        await CheckSensorAndWait(() => sensorsIO.ScrewInScrap(), "Esperamos que el operador coloque el tornillo en el scrap");
                                                        if (isCancellationRequested) { return; };

                                                        await showMessageAndImage($"Coloque la punta del atornillador en la punta de la aspiradora.", "Vacuum.jpg");
                                                        await CheckSensorAndWait(() => ergoArm.isInVacuumNozzle(), "Esperamos ErgoArm en punta de la aspiradora");
                                                        if (isCancellationRequested) { return; };

                                                        
                                                        await showMessageAndImage($"Por favor, posiciones el brazo ergonomico del tornillo número: {tightenincount}, en donde ha fallado el atornillado y aspire.", "HousingWithMask.png", false,500);                                                        
                                                        await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Normal);
                                                        

                                                        ergoArm.startReadPositionRespectScrew(screw);
                                                        if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                                        {
                                                            //Thread.Sleep(100);
                                                            Debug.WriteLine("*Estpy en posicion de aspiradora Intento 3");
                                                            sensorsIO.ActivateVacumm_by_time(1000);
                                                        }
                                                        if (isCancellationRequested) { return; };

                                                        RewriteResultsOfTightening(lstScrewsToProcess);
                                                        sensorsIO.ResetScrap();
                                                        sensorsIO.ReleScrapOFF();
                                                        RequestRemoveTextBox();
                                                        await showMessageAndImage($"Los 3 intentos de atornillado han fallado.");
                                                        Debug.WriteLine($"{DateTime.Now} - " + "Los 3 intentos de atornillado han fallado.");
                                                        ergoArm.endReadPostion();
                                                        Validation_by_FIS(sessionApp.QR.HOUSING, sessionApp.QR.HVDC_BUSBAR, "Se envía BCMP1 del housing a FIS correspondiente a Housing y HDVCCOVER.", eTypeSendToFIS.BCMP, false, lstScrewsToProcess, MaxNumberAttempts);
                                                        RequestRemoveTextBox();
                                                        FinshProcessByErrors();
                                                        return;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    await showMessageAndImage($"El atornillado del tornillo número : {tightenincount} se ha realizado con éxito.", "HousingWithMask.png");
                    //RequestCreateTextBox($"{tightening.Torque.Substring(0, 2)}.{tightening.Torque.Substring(2, 2)} Nw | {tightening.Angle.TrimStart('0')} °", screw.text_position_X, screw.text_position_Y);
                    RewriteResultsOfTightening(lstScrewsToProcess);
                    tightenincount++;
                }//Finaliza el proceso de atornillado


                await showMessageAndImage($"El atornillado se ha realizado con éxito.", "HousingWithMask.png");
                RequestRemoveTextBox();

                if (!Validation_by_FIS(sessionApp.QR.HOUSING, sessionApp.QR.HVDC_BUSBAR, "Se envía BCMP1 del housing a FIS correspondiente a Housing y HDVCCOVER.", eTypeSendToFIS.BCMP, true, lstScrewsToProcess))
                {
                    FinshProcessByErrors();
                    return;
                }
                await showMessageAndImage("Recepción de BACK completada.");

                RewriteResultsOfTightening(lstScrewsToProcess);
                await showMessageAndImage("Por favor, coloque el atornillador en su posición base.", "HousingWithMask.png");
                await CheckSensorAndWait(() => ergoArm.isInHome(), "Esperamos ErgoArm en Home");


                ergoArm.endReadPostion();

                Thread.Sleep(500);
                RequestRemoveTextBox();
                if (isCancellationRequested) { return; };
                await showMessageAndImage("Por favor, retire la máscara y colóquela en su base", "MaskInHolder.jpg");
                await CheckSensorAndWait(() => sensorsIO.MaskInHolder(), "Esperamos maskhousing");
                if (isCancellationRequested) { return; };

                if (isFISEneable ? !Validation_by_FIS(sessionApp.QR.HOUSING, string.Empty, "Se envía BREQ a FIS.", eTypeSendToFIS.BREQ) : true)
                {
                    FinshProcessByErrors();
                    return;
                }

                await showMessageAndImage("Por favor, tome la cubierta superior y colóquela frente al escáner.", "TopCover_Scanner.jpg");
                await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperamos maskhousing");
                if (isCancellationRequested) { return; };


                //if (isFISEneable)
                //{
                while (!isCancellationRequested)
                {
                    await showMessageAndImage("Escaneando código QR de la cubierta superior.", "", false, 50);
                    Scanner scanner4 = new Scanner(sessionApp, eTypeConnection.Scan_2);
                    serial = await scanner4.ScanningTrigger(_cancellationTokenSource, "LON", serials);
                    serials.Add(serial);
                    sessionApp.QR.TOP_COVER = serial;//.Substring(0, (serial.Length - 1));
                    scanner4.DisconnectScanner();
                    if (isCancellationRequested) { return; };
                    Thread.Sleep(50);
                    if (serial != string.Empty)
                    {
                        if (isFISEneable ? Validation_by_FIS(string.Empty, sessionApp.QR.TOP_COVER, "Se envía BREQ a FIS el TOPCOVER.", eTypeSendToFIS.BREQ) : true)
                        {
                            break;
                        }
                    }
                    //Thread.Sleep(10);
                }
                if (isCancellationRequested) { return; };
                //}



                //await showMessageAndImage("Escaneando código QR de la cubierta superior.");
                //    Scanner scanner4 = new Scanner(sessionApp, eTypeConnection.Scan_2);

                //    //serial = scanner4.ScanQR("LON");
                //    serial = await scanner4.ScanningTrigger(_cancellationTokenSource, "LON", serials);
                //    if (isCancellationRequested) { return; };
                //    sessionApp.QR.TOP_COVER = serial;//.Substring(0, (serial.Length - 1));
                //    scanner4.DisconnectScanner();


                //if (isFISEneable ? Validation_by_FIS(serial, "Se envía BREQ a FIS.", eTypeSendToFIS.BREQ) : true)
                //{
                await showMessageAndImage("Inspección completada...");
                Thread.Sleep(300);
                await showMessageAndImage("Por favor, ensamble la cubierta superior y presione el sensor óptico(OPTO).", "HousingTopCover.png");
                await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "Esperando HVDC cover");
                if (isCancellationRequested) { return; };

                await showMessageAndImage("Enviando señal de finalización de tarea.");
                Thread.Sleep(300);

                if (!Validation_by_FIS(sessionApp.QR.HOUSING, sessionApp.QR.TOP_COVER, "Se envía BCMP1 del housing a FIS correspondiente a Housing y TOPCOVER.", eTypeSendToFIS.BCMP, true))
                {
                    FinshProcessByErrors();
                    return;
                }
                //if (isFISEneable ? Validation_by_FIS(serial, "Se envía BCMP a FIS.", eTypeSendToFIS.BCMP) : true)
                //{
                //await showMessageAndImage("Recepción de BACK completada.");
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
                //}
                //else
                //{
                //    await showMessageAndImage("Error: Falló la cubierta superior, BACK de FIS.");
                //}
                //}
                //else
                //{
                //    await showMessageAndImage("Error: Falló la cubierta superior, BACK de FIS.");
                //}
                //}
                //else
                //{
                //    Debug.WriteLine($"{DateTime.Now} - " + "La informacion correspondiente a los tornillos esta incompleta");
                //}
                //}
                //else
                //{
                //    await showMessageAndImage("Error: Fallo la confirmación de FIS");
                //}
            }
            else
            {
                FinshProcessByErrors();
                await showMessageAndImage("Error: Fallo la confirmación de FIS");
            }

            await sensorsIO.UnsecurePallet(_cancellationTokenSource);
            endOfProcess();
        }

        public async void FinshProcessByErrors()
        {
            try
            {
                Debug.WriteLine($"{DateTime.Now} - FinshProcessByErrors");
                RequestRemoveTextBox();
                Thread.Sleep(10);
                await showMessageAndImage("Por favor, retire la máscara y colóquela en su base", "MaskInHolder.jpg");
                await CheckSensorAndWait(() => sensorsIO.MaskInHolder(), "Esperamos maskhousing");
                if (isCancellationRequested) { return; };

                await showMessageAndImage("Retrayendo el candado.", "MGPM25-10Z.png");
                Debug.WriteLine($"{DateTime.Now} - Retrayendo el candado.");
                Debug.WriteLine($"{DateTime.Now} - Retrayendo el candado2.");
                Thread.Sleep(10);
                sensorsIO.RetractPalletClamp();
                Debug.WriteLine($"{DateTime.Now} - Retraemos palletClamp.");
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
                    Debug.WriteLine($"{DateTime.Now} FinshProcessByErrors - ¡El ciclo ha concluido exitosamente!");
                }
                endOfProcess();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + $"Error: { ex.Message }");
            }
        }
        public async Task DrawingSight(int PositionX, int PositionY, eStyleText eStyleText)
        {
            Debug.WriteLine("Entre DrawingSight");
            await Task.Run(() =>
            {
                //RequestRemoveTextBox();
                switch (eStyleText)
                {
                    case eStyleText.None:
                        break;
                    case eStyleText.Normal:
                        RequestCreateTextBox($"X", PositionX, PositionY, 30, 30, true, eStyleText.Normal);
                        break;
                    case eStyleText.Error:
                        RequestCreateTextBox($"X", PositionX, PositionY, 30, 30, true, eStyleText.Error);
                        break;
                    case eStyleText.Success:
                        RequestCreateTextBox($"X", PositionX, PositionY, 30, 30, true, eStyleText.Success);
                        break;
                    case eStyleText.InPosition:
                        RequestCreateTextBox($"X", PositionX, PositionY, 30, 30, true, eStyleText.InPosition);
                        break;
                    default:
                        break;
                }
                Thread.Sleep(100);
            });

        }
        public async Task RewriteResultsOfTightening(List<Screw> lstScrewsToProcess)
        {
            RequestRemoveTextBox();
            if (lstScrewsToProcess != null)
            {
                foreach (var screw in lstScrewsToProcess)
                {
                    if (screw.tighteningprocess.Torque != null && screw.tighteningprocess.Torque != string.Empty)
                    {
                        //if (screw.tighteningprocess.Angle == "0000")
                        if (!screw.tighteningprocess.status)
                        {
                            RequestCreateTextBox($"{screw.tighteningprocess.Torque.Substring(0, 2)}.{screw.tighteningprocess.Torque.Substring(2, 2)} Nw | {screw.tighteningprocess.Angle.TrimStart('0')} °", screw.text_position_X, screw.text_position_Y, 100, 30, true);
                            await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Error);
                        }
                        else
                        {
                            RequestCreateTextBox($"{screw.tighteningprocess.Torque.Substring(0, 2)}.{screw.tighteningprocess.Torque.Substring(2, 2)} Nw | {screw.tighteningprocess.Angle.TrimStart('0')} °", screw.text_position_X, screw.text_position_Y, 100, 30);
                            await DrawingSight(screw.sight_position_X, screw.sight_position_Y, eStyleText.Success);
                        }

                    }
                }
            }
        }
        public void getModelScrew()
        {
            sessionApp.ModelScrewSelected = int.Parse(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("Model_Screw")).valueSetting);
        }

        public void endOfProcess()
        {
            Debug.WriteLine($"{DateTime.Now} - endOfProcess");
            sessionApp.QR.TOP_COVER = string.Empty;
            sessionApp.QR.HARNESS = string.Empty;
            sessionApp.QR.HOUSING = string.Empty;
            sessionApp.QR.HVDC_BUSBAR = string.Empty;
            sessionApp.TaksRunExecuting = false;
            sensorsIO.endRead();

        }
        public async Task CheckSensorAndWait(Func<bool> sensorCheck, string debugMessage)
        {
            if (!sensorCheck())
            {
                Debug.WriteLine($"{DateTime.Now} - {debugMessage}");
                sessionApp.MessageOfProcessDebug = $"{DateTime.Now} - {debugMessage}";
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
        public override void RequestCreateTextBox(string msg, int PositionX, int PositionY, int Width, int Height, bool hasError = false, eStyleText eStyleText = eStyleText.None)
        {
            OnCreateTextBoxRequested(new TextBoxInfoEventArgs
            {
                Text = msg,
                Position = new System.Windows.Point(PositionX, PositionY),
                HasError = hasError,
                eStyleText = eStyleText,
                Height = Height,
                Width = Width
            });
        }

        public override void RequestRemoveTextBox()
        {
            Debug.WriteLine($"{DateTime.Now} - Removemos cajas de Texto");
            RemoveTextBoxRequested?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnCreateTextBoxRequested(TextBoxInfoEventArgs e)
        {
            CreateTextBoxRequested?.Invoke(this, e);
        }
    }
}

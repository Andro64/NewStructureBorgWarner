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
    class WorkStation_Manual_Type12 : Workstation
    {
        public override string Type => " ╭∩╮( •̀_•́ )╭∩╮ \n WS Automatica Tipo 1";

        SensorsIO sensorsIO;
        SessionApp sessionApp;
        private CancellationTokenSource _cancellationTokenSource;
        private bool isCancellationRequested = false;
        TighteningProcess tightening;

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

            sessionApp.Sensors_M1.Main_Pressure = false;
            sessionApp.Sensors_M1.OptoBtn = false;
            sessionApp.Sensors_M1.Pallet_Pre_Stopper = false;
            sessionApp.Sensors_M1.Pallet_Stopper = false;
            sessionApp.Sensors_M1.Screw_Present_Oth = false;
            sessionApp.Sensors_M1.Screw_Level_Oth = false;
            sessionApp.Sensors_M1.MaskInHolder = false;
            sessionApp.Sensors_M1.SecurityOK = false;

            sessionApp.Sensors_M2.Trigger_Scanner = false;
            sessionApp.Sensors_M2.MaskatHousing = false;
            sessionApp.Sensors_M2.PA2 = false;
            sessionApp.Sensors_M2.PA3 = false;
            sessionApp.Sensors_M2.Cyl_Fixing_Pall_Ext = false;
            sessionApp.Sensors_M2.Cyl_Fixing_Pall_Ret = false;
            sessionApp.Sensors_M2.PB2 = false;
            sessionApp.Sensors_M2.PB3 = false;

            sessionApp.Sensors_M3.PA0 = false;
            sessionApp.Sensors_M3.PA1 = false;
            sessionApp.Sensors_M3.PA2 = false;
            sessionApp.Sensors_M3.ST13Available = false;
            sessionApp.Sensors_M3.PB0 = false;
            sessionApp.Sensors_M3.PB1 = false;
            sessionApp.Sensors_M3.Scrap_presence = false;
            sessionApp.Sensors_M3.PB3 = false;

            sessionApp.images = new List<string>();

        }

        public async Task showMessageAndImage(string message, string nameimage = "", bool isImageInDiferentPath = false)
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
            List<string> imagesVisionSystem = new List<string>();
            string resultImageVisionSystem;
            bool isFISEneable = false;
            bool isVisionEneable = false;

            string serial;
            string resultFIS;
            int quantityScrews;

            isCancellationRequested = false;
            if (sensorsIO != null)
            {
                sensorsIO.StopWaiting();
                sensorsIO.endRead();
            }

            sensorsIO = new SensorsIO(sessionApp);
            sensorsIO.startRead();
            _cancellationTokenSource = new CancellationTokenSource();
            sessionApp.areImagePASSProcessFinished = false;
            sessionApp.images = new List<string>();

            isFISEneable = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("EneableFIS")).valueSetting == "1";
            isVisionEneable = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("EneableVision")).valueSetting == "1";

            sessionApp.MessageOfProcessDebug = "Inicializando sistema";

            //await showMessageAndImage("A la espera del producto.", "Housing.png");
            //await CheckSensorAndWait(() => sensorsIO.WaitingForProduct(_cancellationTokenSource, false), "Esperamos pallet en Pre-Stopper");
            //await CheckSensorAndWait(() => sensorsIO.PalletInStopper(), "Esperamos pallet en Pre-Stopper");


            //if (!sensorsIO.PalletInStopper())
            //{
            //    await showMessageAndImage("A la espera del producto en prestoper.", "Housing.png");
            //    await sensorsIO.UnsecurePallet(_cancellationTokenSource);
            //}

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
            Scanner scanner = new Scanner(sessionApp, eTypeConnection.Scan_1);
            serial = scanner.ScanQR("LON");
            //serial = await scanner.ScanningTrigger(_cancellationTokenSource, "LON");
            
            serial = serial == null ? string.Empty : serial;
            sessionApp.QR.HOUSING = serial != string.Empty ? serial.Substring(0, (serial.Length - 1)) : "";            
            
            if (serial == "ERROR\r" || serial == string.Empty)
            {
                await showMessageAndImage("Error al momento de escanear el código QR del Housing. Favor de verificar.");
                FinshProcessByErrors();
                await sensorsIO.Sequence_Stoper_PrestoperAsync(_cancellationTokenSource, true);
                return;
            }

            scanner.DisconnectScanner();
            if (isCancellationRequested) { return; };

           
            if (isFISEneable ? Validation_by_FIS(sessionApp.QR.HOUSING, "Se envía BREQ del housing a FIS.", eTypeSendToFIS.BREQ) : true)
            {
                await showMessageAndImage("Inspección completada...");
                Thread.Sleep(300);
                await showMessageAndImage("Por favor, tome el HVDC COVER y colóquelo frente al escaner.", "ScannerHVDCCover.jpg");
                await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperando HVDC cover.");
                if (isCancellationRequested) { return; };

                await showMessageAndImage("Escaneando código QR del HVDC Cover.");
                Scanner scanner2 = new Scanner(sessionApp, eTypeConnection.Scan_2);
                //serial = scanner2.ScanQR("LON");
                serial = await scanner2.ScanningTrigger(_cancellationTokenSource, "LON");
                sessionApp.QR.HVDC_BUSBAR = serial.Substring(0, (serial.Length - 1));
                scanner2.DisconnectScanner();
                if (isCancellationRequested) { return; };
                Thread.Sleep(300);
                
                if (isFISEneable ? Validation_by_FIS(sessionApp.QR.HOUSING, "Se envía BREQ a FIS.", eTypeSendToFIS.BREQ) : true)
                {
                    await showMessageAndImage("Inspección completada...");
                    Thread.Sleep(300);

                    await showMessageAndImage("Por favor,remueva el HVDC COVER.", "ScannerHVDCCover.jpg");
                    await CheckSensorAndWait(() => sensorsIO.isOutPieceHDVC(), "Esperando que quiten la pieza HVDC cover.");
                    if (isCancellationRequested) { return; };
                    await showMessageAndImage("Por favor, tome cable arnés y colóquelo frente al escaner.", "ScannerHarness.jpg");
                    await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperamos que el operador coloque el arnés en el scaner.");
                    if (isCancellationRequested) { return; };

                    await showMessageAndImage("Leyendo QR arnés.", "ScannerHarness.jpg");
                    Scanner scanner3 = new Scanner(sessionApp, eTypeConnection.Scan_2);
                    //serial = scanner3.ScanQR("LON");
                    serial = await scanner3.ScanningTrigger(_cancellationTokenSource, "LON");
                    sessionApp.QR.HARNESS = serial.Substring(0, (serial.Length - 1));
                    scanner3.DisconnectScanner();
                    if (isCancellationRequested) { return; };

                    
                    if (isFISEneable ? Validation_by_FIS(sessionApp.QR.HOUSING, "Se envía BREQ árnes a FIS", eTypeSendToFIS.BREQ) : true)                    
                    {
                        await showMessageAndImage("Inspección completada...");
                        Thread.Sleep(300);

                        await showMessageAndImage("Por favor, conecte árness y realice su ruteo, despues presione el opto.", "HousingRoute.png");
                        await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "Esperamos que el operador presione opto del árnes.");
                        if (isCancellationRequested) { return; };

                        await showMessageAndImage("Realizando inspección 1...");
                        Thread.Sleep(300);

                        visionSystem = new VisionSystem(sessionApp, eTypeConnection.Camara_1);
                        if (isVisionEneable ? !visionSystem.FirstInspectionAttempt(serial) : false)
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
                        if (lstScrewsToProcess.Count != 0 && (quantityScrews == lstScrewsToProcess.Count))
                        {
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
                                    await showMessageAndImage($"Por favor, posiciones el brazo ergonomico del tornillo número: {tightenincount}.", "HousingWithMask.png");
                                    ergoArm.startReadPositionRespectScrew(screw);
                                }
                                if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                {
                                    Debug.WriteLine($"{DateTime.Now} - " + "BRAZO ERGONOMICO EN POSICION");
                                    Debug.WriteLine($"{DateTime.Now} - " + $"Ejecuta primer intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
                                    sessionApp.MessageOfProcessDebug = "BRAZO ERGONOMICO EN POSICION";
                                    Thread.Sleep(1000);
                                    sessionApp.MessageOfProcessDebug = $"Ejecuta primer intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";

                                    //if (!sensorsIO.MaskOnHousing())
                                    //{
                                    //    await showMessageAndImage("Por favor, vuelva a colocar la máscara sobre el housing.", "HousingWithMask.png");
                                    //    await CheckSensorAndWait(() => sensorsIO.MaskOnHousing(), "Esperamos maskhousing");
                                    //    sessionApp.MessageOfProcessDebug = "Esperamos maskhousing";
                                    //}
                                    //await Task.Run(() =>sensorsIO.continueScrewFunction.WaitOne());

                                    //await Task.Run(() => sensorsIO.WaitingMonitorMaskOnHousing());
                                    //Task.Run(() => screwdriver.FactoryScrewing(ergoArm,screw));

                                    await showMessageAndImage($"Por favor, realice el atornillado número: {tightenincount}", "HousingWithMask.png");
                                    Debug.WriteLine($"-------Primer intento atronillado");
                                    sessionApp.MessageOfProcessDebug = "-------Primer intento atronillado";
                                    tightening = await screwdriver.FirstTighteningAttempt(ergoArm,screw, _cancellationTokenSource);

                                    if (tightening == null)
                                    {
                                        Debug.WriteLine($"{DateTime.Now} - " + $"Fallo el primer intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
                                        sessionApp.MessageOfProcessDebug = $"Fallo el primer intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";
                                        //if (!sensorsIO.MaskOnHousing())
                                        //{
                                        //    await showMessageAndImage("Por favor, vuelva a colocar la máscara sobre el housing.", "HousingWithMask.png");
                                        //    await CheckSensorAndWait(() => sensorsIO.MaskOnHousing(), "Esperamos maskhousing");                                            
                                        //}
                                        //await Task.Run(() =>sensorsIO.continueScrewFunction.WaitOne());
                                        await Task.Run(() => sensorsIO.WaitingMonitorMaskOnHousing());
                                        RewriteResultsOfTightening(lstScrewsToProcess);
                                        await showMessageAndImage($"Por favor, realice el desatornillado del tornillo número: {tightenincount}.", "HousingWithMask.png");

                                        ergoArm.startReadPositionRespectScrew(screw);
                                        if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                        {
                                            sessionApp.MessageOfProcessDebug = $"Ejecuta primer intento el brazo esta en posicion el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";
                                            Debug.WriteLine($"{DateTime.Now} - " + $"Ejecuta primer intento el brazo esta en posicion el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
                                            //if (!sensorsIO.MaskOnHousing())
                                            //{
                                            //    await showMessageAndImage("Por favor, vuelva a colocar la máscara sobre el housing.", "HousingWithMask.png");
                                            //    await CheckSensorAndWait(() => sensorsIO.MaskOnHousing(), "Esperamos maskhousing");                                                
                                            //}
                                            //await Task.Run(() =>sensorsIO.continueScrewFunction.WaitOne());
                                            await Task.Run(() => sensorsIO.WaitingMonitorMaskOnHousing());
                                            Debug.WriteLine($"{DateTime.Now} - " + $"Vamos a desatornillar el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
                                            Debug.WriteLine($"-------desatornillado");
                                            sessionApp.MessageOfProcessDebug = $"Vamos a desatornillar el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";
                                            sessionApp.MessageOfProcessDebug = $"-------desatornillado";

                                            await screwdriver.Unscrewing(ergoArm, screw, _cancellationTokenSource);
                                            RequestRemoveTextBox();

                                            await showMessageAndImage($"El atornillado del tornillo número : {tightenincount} ha fallado. Por favor, retire el tornillo y colóquelo en desposito de tornillos desechados.", "Scrap2.jpg");
                                            await CheckSensorAndWait(() => sensorsIO.ScrewInScrap(), "Esperamos que el operador coloque el tornillo en el scrap");
                                            if (isCancellationRequested) { return; };

                                            await showMessageAndImage($"Coloque la punta del atornillador en la punta de la aspiradora.", "Vacuum.png");

                                            await CheckSensorAndWait(() => ergoArm.isInVacuumNozzle(), "Esperamos ErgoArm en punta de la aspiradora");
                                            if (isCancellationRequested) { return; };

                                            await showMessageAndImage($"Por favor, posiciones el brazo ergonomico del tornillo número: {tightenincount}, en donde ha fallado el atornillado.", "HousingWithMask.png");

                                            ergoArm.startReadPositionRespectScrew(screw);
                                            if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                            {
                                                sensorsIO.ActivateVacumm_by_time(3000);
                                            }

                                            if (isCancellationRequested) { return; };

                                            RewriteResultsOfTightening(lstScrewsToProcess);
                                            //await showMessageAndImage($"El primer intento de atornillado del tornillo número : {tightenincount} ha fallado.Presione OPTO para continuar", "HousingWithMask.png");
                                            //await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "Fallo primer intento de atornillado  ESPERA ACTIVACION DE OPTO");


                                            sensorsIO.ResetScrap();
                                            sensorsIO.DispenseAScrew();

                                            ergoArm.startReadPositionRespectScrew(screw);
                                            if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                            {
                                                Debug.WriteLine($"{DateTime.Now} - " + $"Ejecuta segundo intento intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
                                                sessionApp.MessageOfProcessDebug = $"Ejecuta segundo intento intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";
                                                //if (!sensorsIO.MaskOnHousing())
                                                //{
                                                //    await showMessageAndImage("Por favor, vuelva a colocar la máscara sobre el housing.", "HousingWithMask.png");
                                                //    await CheckSensorAndWait(() => sensorsIO.MaskOnHousing(), "Esperamos maskhousing");
                                                //}
                                                //await Task.Run(() =>sensorsIO.continueScrewFunction.WaitOne());
                                                await Task.Run(() => sensorsIO.WaitingMonitorMaskOnHousing());
                                                await showMessageAndImage($"Intento 2 - Por favor, realice nuevamente el atornillado del tornillo número: {tightenincount}.", "HousingWithMask.png");
                                                Debug.WriteLine($"-------Segundo intento atronillado");
                                                sessionApp.MessageOfProcessDebug = $"-------Segundo intento atronillado";

                                                tightening = await screwdriver.SecondTighteningAttempt(ergoArm, screw, _cancellationTokenSource);
                                                if (tightening == null)
                                                {
                                                    Debug.WriteLine($"{DateTime.Now} - " + $"Fallo segundo intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
                                                    sessionApp.MessageOfProcessDebug = $"Fallo segundo intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";
                                                    //if (!sensorsIO.MaskOnHousing())
                                                    //{
                                                    //    await showMessageAndImage("Por favor, vuelva a colocar la máscara sobre el housing.", "HousingWithMask.png");
                                                    //    await CheckSensorAndWait(() => sensorsIO.MaskOnHousing(), "Esperamos maskhousing");
                                                    //}
                                                    //await Task.Run(() =>sensorsIO.continueScrewFunction.WaitOne());
                                                    await Task.Run(() => sensorsIO.WaitingMonitorMaskOnHousing());
                                                    RewriteResultsOfTightening(lstScrewsToProcess);
                                                    //await showMessageAndImage($"Por favor, realice el desatornillado del tornillo número: {tightenincount}.", "HousingWithMask.png");

                                                    ergoArm.startReadPositionRespectScrew(screw);
                                                    if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                                    {
                                                        Debug.WriteLine($"{DateTime.Now} - " + $"Estamos en posicion del brazo segundo intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
                                                        sessionApp.MessageOfProcessDebug = $"Estamos en posicion del brazo segundo intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";
                                                        //if (!sensorsIO.MaskOnHousing())
                                                        //{
                                                        //    await showMessageAndImage("Por favor, vuelva a colocar la máscara sobre el housing.", "HousingWithMask.png");
                                                        //    await CheckSensorAndWait(() => sensorsIO.MaskOnHousing(), "Esperamos maskhousing");
                                                        //}
                                                        //await Task.Run(() =>sensorsIO.continueScrewFunction.WaitOne());
                                                        await Task.Run(() => sensorsIO.WaitingMonitorMaskOnHousing());
                                                        await showMessageAndImage($"Por favor, realice el desatornillado del tornillo número: {tightenincount}.", "HousingWithMask.png");
                                                        Debug.WriteLine($"{DateTime.Now} - " + $"Comenzamos desatornillado del segundo intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
                                                        Debug.WriteLine($"-------desatornillado");
                                                        sessionApp.MessageOfProcessDebug = $"Comenzamos desatornillado del segundo intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";
                                                        sessionApp.MessageOfProcessDebug = $"-------desatornillado";
                                                        await screwdriver.Unscrewing(ergoArm, screw, _cancellationTokenSource);
                                                        Debug.WriteLine($"{DateTime.Now} - " + $"Terminamos desatornillado segundo intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
                                                        sessionApp.MessageOfProcessDebug = $"Terminamos desatornillado segundo intento el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";
                                                        RequestRemoveTextBox();
                                                        await showMessageAndImage($"El atornillado del tornillo número : {tightenincount} ha fallado. Por favor, retire el tornillo y colóquelo en desposito de tornillos desechados.", "Scrap2.jpg");
                                                        await CheckSensorAndWait(() => sensorsIO.ScrewInScrap(), "Esperamos que el operador coloque el tornillo en el scrap");
                                                        if (isCancellationRequested) { return; };

                                                        await showMessageAndImage($"Coloque la punta del atornillador en la punta de la aspiradora.", "Vacuum.png");
                                                        await CheckSensorAndWait(() => ergoArm.isInVacuumNozzle(), "Esperamos ErgoArm en punta de la aspiradora");
                                                        if (isCancellationRequested) { return; };

                                                        await showMessageAndImage($"Por favor, posiciones el brazo ergonomico del tornillo número: {tightenincount}, en donde ha fallado el atornillado.", "HousingWithMask.png");

                                                        ergoArm.startReadPositionRespectScrew(screw);
                                                        if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                                        {
                                                            sensorsIO.ActivateVacumm_by_time(3000);
                                                        }
                                                        if (isCancellationRequested) { return; };

                                                        RewriteResultsOfTightening(lstScrewsToProcess);
                                                        //await showMessageAndImage($"El segundo intento de atornillado del tornillo número : {tightenincount} ha fallado.Presione OPTO para continuar", "HousingWithMask.png");
                                                        //RewriteResultsOfTightening(lstScrewsToProcess);
                                                        //await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "Fallo segundo intento de atornillado  ESPERA ACTIVACION DE OPTO");

                                                        sensorsIO.ResetScrap();
                                                        sensorsIO.DispenseAScrew();

                                                        ergoArm.startReadPositionRespectScrew(screw);
                                                        if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                                        {
                                                            RewriteResultsOfTightening(lstScrewsToProcess);
                                                            //if (!sensorsIO.MaskOnHousing())
                                                            //{
                                                            //    await showMessageAndImage("Por favor, vuelva a colocar la máscara sobre el housing.", "HousingWithMask.png");
                                                            //    await CheckSensorAndWait(() => sensorsIO.MaskOnHousing(), "Esperamos maskhousing");
                                                            //}
                                                            //await Task.Run(() =>sensorsIO.continueScrewFunction.WaitOne());
                                                            await Task.Run(() => sensorsIO.WaitingMonitorMaskOnHousing());
                                                            await showMessageAndImage($"Intento 3 - Por favor, realice nuevamente el atornillado del tornillo número: {tightenincount}.", "HousingWithMask.png");
                                                            Debug.WriteLine($"------Tercer intento atronillado");
                                                            sessionApp.MessageOfProcessDebug = $"------Tercer intento atronillado";
                                                            tightening = await screwdriver.ThirdTighteningAttempt(ergoArm, screw, _cancellationTokenSource);
                                                            if (tightening == null)
                                                            {
                                                                ergoArm.startReadPositionRespectScrew(screw);
                                                                if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                                                {
                                                                    //if (!sensorsIO.MaskOnHousing())
                                                                    //{
                                                                    //    await showMessageAndImage("Por favor, vuelva a colocar la máscara sobre el housing.", "HousingWithMask.png");
                                                                    //    await CheckSensorAndWait(() => sensorsIO.MaskOnHousing(), "Esperamos maskhousing");
                                                                    //}
                                                                    //await Task.Run(() =>sensorsIO.continueScrewFunction.WaitOne());
                                                                    await Task.Run(() => sensorsIO.WaitingMonitorMaskOnHousing());
                                                                    Debug.WriteLine($"-------desatornillado");
                                                                    sessionApp.MessageOfProcessDebug = $"-------desatornillado";
                                                                    await screwdriver.Unscrewing(ergoArm, screw, _cancellationTokenSource);
                                                                    RequestRemoveTextBox();

                                                                    await showMessageAndImage($"El atornillado número : {tightenincount} ha fallado. Por favor, retire el tornillo y colóquelo en desposito de tornillos desechados.", "Scrap2.jpg");
                                                                    await CheckSensorAndWait(() => sensorsIO.ScrewInScrap(), "Esperamos que el operador coloque el tornillo en el scrap");
                                                                    if (isCancellationRequested) { return; };

                                                                    await showMessageAndImage($"Coloque la punta del atornillador en la punta de la aspiradora.", "Vacuum.png");
                                                                    await CheckSensorAndWait(() => ergoArm.isInVacuumNozzle(), "Esperamos ErgoArm en punta de la aspiradora");
                                                                    if (isCancellationRequested) { return; };

                                                                    await showMessageAndImage($"Por favor, posiciones el brazo ergonomico del tornillo número: {tightenincount}, en donde ha fallado el atornillado.", "HousingWithMask.png");

                                                                    ergoArm.startReadPositionRespectScrew(screw);
                                                                    if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                                                                    {
                                                                        sensorsIO.ActivateVacumm_by_time(3000);
                                                                    }
                                                                    if (isCancellationRequested) { return; };

                                                                    RewriteResultsOfTightening(lstScrewsToProcess);
                                                                    sensorsIO.ResetScrap();

                                                                    await showMessageAndImage($"Los 3 intentos de atornillado han fallado.");
                                                                    Debug.WriteLine($"{DateTime.Now} - " + "Los 3 intentos de atornillado han fallado.");
                                                                    ergoArm.endReadPostion();

                                                                    FinshProcessByErrors();
                                                                    break;
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
                            RewriteResultsOfTightening(lstScrewsToProcess);
                            await showMessageAndImage("Por favor, coloque el atornillador en su posición base.", "HousingWithMask.png");
                            await CheckSensorAndWait(() => ergoArm.isInHome(), "Esperamos ErgoArm en Home");


                            ergoArm.endReadPostion();

                            Thread.Sleep(3000);
                            RequestRemoveTextBox();

                            await showMessageAndImage("Por favor, retire la máscara y colóquela en su base", "MaskInHolder.jpg");
                            await CheckSensorAndWait(() => sensorsIO.MaskInHolder(), "Esperamos maskhousing");
                            if (isCancellationRequested) { return; };

                            await showMessageAndImage("Por favor, tome la cubierta superior y colóquela frente al escáner.", "TopCover_Scanner.jpg");
                            await CheckSensorAndWait(() => sensorsIO.isTriggerScanner(), "Esperamos maskhousing");
                            if (isCancellationRequested) { return; };

                            await showMessageAndImage("Escaneando código QR de la cubierta superior.");
                            Scanner scanner4 = new Scanner(sessionApp, eTypeConnection.Scan_2);
                            
                            //serial = scanner4.ScanQR("LON");
                            serial = await scanner4.ScanningTrigger(_cancellationTokenSource, "LON");
                            if (isCancellationRequested) { return; };
                            sessionApp.QR.TOP_COVER = serial.Substring(0, (serial.Length - 1));
                            scanner4.DisconnectScanner();

                           
                            if (isFISEneable ? Validation_by_FIS(serial, "Se envía BREQ a FIS.", eTypeSendToFIS.BREQ) : true)
                            {
                                await showMessageAndImage("Inspección completada...");
                                Thread.Sleep(300);
                                await showMessageAndImage("Por favor, ensamble la cubierta superior y presione el sensor óptico(OPTO).", "HousingTopCover.png");
                                await CheckSensorAndWait(() => sensorsIO.WasPressedOpto(), "Esperando HVDC cover");
                                if (isCancellationRequested) { return; };

                                await showMessageAndImage("Enviando señal de finalización de tarea.");
                                Thread.Sleep(300);

                                
                                if (isFISEneable ? Validation_by_FIS(serial, "Se envía BCMP a FIS.", eTypeSendToFIS.BCMP) : true)                                
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

            await sensorsIO.UnsecurePallet(_cancellationTokenSource);
            endOfProcess();
        }


        /* 
        public override async Task StartProcess()
        {
            RequestCreateTextBox($"T1: 260 Nw | 50 °", 450, 25);
            RequestCreateTextBox($"T2: 460 Nw | 10 °",120, -55);
            await showMessageAndImage("Por favor, posicione la máscara sobre el housing.", @"D:\Repo3\BORGWARNER_SERVOPRESS\BORGWARNER_SERVOPRESS.UI\Resources\Operational_Images\WSAT1\HousingWithMask.png",true);
        }
        */
        public async void FinshProcessByErrors()
        {
            RequestRemoveTextBox();
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
            endOfProcess();
        }

        public void RewriteResultsOfTightening(List<Screw> lstScrewsToProcess)
        {
            RequestRemoveTextBox();
            if (lstScrewsToProcess != null)
            {
                foreach (var screw in lstScrewsToProcess)
                {
                    if (screw.tighteningprocess.Torque != null && screw.tighteningprocess.Torque != string.Empty)
                    {
                        if (screw.tighteningprocess.Angle == "0000")
                        {
                            RequestCreateTextBox($"{screw.tighteningprocess.Torque.Substring(0, 2)}.{screw.tighteningprocess.Torque.Substring(2, 2)} Nw | {screw.tighteningprocess.Angle.TrimStart('0')} °", screw.text_position_X, screw.text_position_Y, true);
                        }
                        else
                        {
                            RequestCreateTextBox($"{screw.tighteningprocess.Torque.Substring(0, 2)}.{screw.tighteningprocess.Torque.Substring(2, 2)} Nw | {screw.tighteningprocess.Angle.TrimStart('0')} °", screw.text_position_X, screw.text_position_Y);
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
        public override void RequestCreateTextBox(string msg, int PositionX, int PositionY, bool hasError = false)
        {
            OnCreateTextBoxRequested(new TextBoxInfoEventArgs
            {
                Text = msg,
                Position = new System.Windows.Point(PositionX, PositionY),
                HasError = hasError
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

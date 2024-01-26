using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class WorkStation_Manual_Type1
    {
        SensorsIO sensorsIO;
        SessionApp sessionApp;
        public WorkStation_Manual_Type1(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            sensorsIO = new SensorsIO(sessionApp);
        }
        public void start()
        {
            Scanner scanner;
            CommunicationFIS fIS;
            VisionSystem visionSystem;
            string serial;
            string resultFIS;

            sensorsIO.startRead();
            if (!sensorsIO.PalletInStopper())
            {
                sensorsIO.WaitingResponse(sensorsIO.PalletInStopper());
                Debug.WriteLine("Esperamos pallet en Pre-Stopper");
            }
            if (!sensorsIO.ExtendedPalletClamp())
            {
                sensorsIO.WaitingResponse(sensorsIO.ExtendedPalletClamp());
                Debug.WriteLine("Esperamos CLAMP DE PALLET EXTENDIDO");
            }
            if (!sensorsIO.PlacedHousing())
            {
                sensorsIO.WaitingResponse(sensorsIO.PlacedHousing());
                Debug.WriteLine("Esperamos que el OPERADOR COLOCAQUE EL HOUSING");
            }
            scanner = new Scanner(sessionApp, eTypeConnection.Scan_1);
            serial = scanner.ScanQR("LON");
            Debug.WriteLine($"SCANNER 1 LEE CODIGO SERIAL: {serial}");

            fIS = new CommunicationFIS(sessionApp);
            resultFIS = fIS.SendBREQToFIS(serial);

            if (resultFIS.Contains("PASS"))
            {
                Debug.WriteLine("PIDE A OPERADOR COLOCAR ULTRA CAP BOARD PAD Y ACTIVAR OPTO");

                if (!sensorsIO.UltraCapBoardPadinPlace())
                {
                    sensorsIO.WaitingResponse(sensorsIO.UltraCapBoardPadinPlace());
                    Debug.WriteLine("ESPERA ACTIVACION DE OPTO ");
                }

                visionSystem = new VisionSystem(sessionApp);
                if (!visionSystem.FirstInspectionAttempt())
                {
                    if (!sensorsIO.WasPressedOpto())
                    {
                        sensorsIO.WaitingResponse(sensorsIO.WasPressedOpto());
                        Debug.WriteLine("Fallo primer intento ESPERA ACTIVACION DE OPTO ");

                        if (!visionSystem.SecondInspectionAttempt())
                        {
                            if (!sensorsIO.WasPressedOpto())
                            {
                                sensorsIO.WaitingResponse(sensorsIO.WasPressedOpto());
                                Debug.WriteLine("Fallo segundo intento ESPERA ACTIVACION DE OPTO ");

                                if (!visionSystem.ThirdInspectionAttempt())
                                {
                                    Debug.WriteLine("Fallaron los 3 intentos ");  ///Falta poner que hace en este caso
                                }
                            }
                        }
                    }
                }
                visionSystem.getNameImageResultFromCamera();
                visionSystem.Disconnect();

                Debug.WriteLine("PIDE A OPERADOR TOMAR ULTRA CAP BOARD Y COLOCAR EN NIDO");

                if (!sensorsIO.UltraCapBoardReadyToScan())
                {
                    sensorsIO.WaitingResponse(sensorsIO.UltraCapBoardReadyToScan());
                    Debug.WriteLine("ESPERA ULTRA CAP BOARD SE COLOQUE EN NIDO ");
                }

                scanner = new Scanner(sessionApp, eTypeConnection.Scan_2);
                serial = scanner.ScanQR("LON");
                Debug.WriteLine($"SCANNER 1 LEE CODIGO SERIAL: {serial}");

                fIS = new CommunicationFIS(sessionApp);
                resultFIS = fIS.SendBREQToFIS(serial);

                if (resultFIS.Contains("PASS"))
                {
                    Debug.WriteLine("PIDE A OPERADOR TOMAR HARNESS, CONECTAR A ULTRA CAP BOARD, COLOCAR EN HOUSING, REALIZAR RUTEO DE HARNESS SOBRE HOUSING Y PRESIONAR OPTO");
                    if (!sensorsIO.UCBdConnected_RoutingHarness_PlaceInHousing())
                    {
                        sensorsIO.WaitingResponse(sensorsIO.UCBdConnected_RoutingHarness_PlaceInHousing());
                        Debug.WriteLine("ESPERA ACTIVACION DE OPTO ");
                    }

                    visionSystem = new VisionSystem(sessionApp);
                    if (!visionSystem.FirstInspectionAttempt())
                    {
                        if (!sensorsIO.WasPressedOpto())
                        {
                            sensorsIO.WaitingResponse(sensorsIO.WasPressedOpto());
                            Debug.WriteLine("Fallo primer intento ESPERA ACTIVACION DE OPTO ");

                            if (!visionSystem.SecondInspectionAttempt())
                            {
                                if (!sensorsIO.WasPressedOpto())
                                {
                                    sensorsIO.WaitingResponse(sensorsIO.WasPressedOpto());
                                    Debug.WriteLine("Fallo segundo intento ESPERA ACTIVACION DE OPTO ");

                                    if (!visionSystem.ThirdInspectionAttempt())
                                    {
                                        Debug.WriteLine("Fallaron los 3 intentos ");  ///Falta poner que hace en este caso
                                    }
                                }
                            }
                        }
                    }
                    visionSystem.getNameImageResultFromCamera();
                    visionSystem.Disconnect();

                    Debug.WriteLine("2 PIDE A OPERADOR TOMAR TOMAR MASCARA Y COLOCAR SOBRE HOUSING");
                    if (!sensorsIO.PlacedHousing())
                    {
                        sensorsIO.WaitingResponse(sensorsIO.PlacedHousing());
                        Debug.WriteLine("Esperamos que el OPERADOR COLOCAQUE MASCARA SOBRE HOUSING");
                    }
                    sensorsIO.ActivateSignalToScrewDispenser();
                    Debug.WriteLine("PIDE A OPERADOR TOMAR ATORNILLADOR Y REALIZAR ATORNILLADO CORRESPONDIENTE");

                }//Falta si no PASS 2

            }//Falta si no PASS 

        }

    }
}

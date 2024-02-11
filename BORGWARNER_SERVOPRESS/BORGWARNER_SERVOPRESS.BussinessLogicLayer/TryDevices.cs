using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class TryDevices
    {        
        SessionApp sessionApp;
        public TryDevices(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            
        }

        Scanner scanner;
        CommunicationFIS fIS;
        VisionSystem visionSystem;
        Screws screws;
        ErgoArm ergoArm;
        ScrewDriver screwdriver;

        string serial;
        string resultFIS;

        int quantityScrews;
        public void TryScanner()
        {
            scanner = new Scanner(sessionApp, eTypeConnection.Scan_1);
            serial = scanner.ScanQR("LON");
            Debug.WriteLine($"{DateTime.Now} - "  + $"SCANNER 1 LEE CODIGO SERIAL: {serial}");
        }
        public void TryFIS()
        {
            serial = "serial de prueba";
            fIS = new CommunicationFIS(sessionApp);
            resultFIS = fIS.SendBREQToFIS(serial);
            if (resultFIS.Contains("PASS"))
            {
                Debug.WriteLine($"{DateTime.Now} - "  + "Si paso");
            }
        }
        public string TryFIS(string numSerial)
        {
            string ResultFIS = string.Empty;
            try
            {
                ResultFIS = new CommunicationFIS(sessionApp).SendBREQToFIS(numSerial);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
            }
            
            return ResultFIS;           
        }

        public void TryErgoArm()
        {
            screws = new Screws(sessionApp);
            quantityScrews = screws.retriveNumberScrewsPerModel(sessionApp.ModelScrewSelected);
            List<Screw> lstScrewsToProcess = screws.retriveScrewsToProcess(sessionApp.ModelScrewSelected);
            if (lstScrewsToProcess.Count != 0 && (quantityScrews == lstScrewsToProcess.Count))
            {
                ergoArm = new ErgoArm(sessionApp);
                screwdriver = new ScrewDriver(sessionApp);
                foreach (var screw in lstScrewsToProcess)
                {
                    screw.tighteningprocess = new TighteningProcess();
                    ergoArm.startReadPositionRespectScrew(screw);
                    if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                    {
                        Debug.WriteLine($"{DateTime.Now} - "  + "BRAZO ERGONOMICO EN POSICION");

                        if (!screwdriver.FirstTighteningAttempt(screw))
                        {
                            Debug.WriteLine($"{DateTime.Now} - "  + "Fallo el atorniollado");
                        }
                        if (!ergoArm.isInHome())
                        {
                            ergoArm.WaitingResponse(ergoArm.isInHome());
                            ergoArm.endReadPostion();
                        }
                    }
                }
            }
        }
        public void TryVisionSystem()
        {
            visionSystem = new VisionSystem(sessionApp);
            if (!visionSystem.FirstInspectionAttempt())
            {
                Debug.WriteLine($"{DateTime.Now} - "  + "Fallo el intento");
            }
            else
            {
                Debug.WriteLine($"{DateTime.Now} - "  + "Si paso");
                visionSystem.getNameImageResultFromCamera();
                visionSystem.Disconnect();
            }
        }
        public void TryScrewdriver()
        {

        }
    }
}

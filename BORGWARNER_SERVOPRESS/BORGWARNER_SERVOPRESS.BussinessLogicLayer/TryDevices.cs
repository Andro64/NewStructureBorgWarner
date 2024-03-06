using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class TryDevices
    {        
        SessionApp sessionApp;
        public TryDevices(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            sensorsIO = new SensorsIO(_sessionApp);
        }

        Scanner scanner;
        CommunicationFIS fIS;
        VisionSystem visionSystem;
        Screws screws;
        ErgoArm ergoArm;
        ScrewDriver screwdriver;
        SensorsIO sensorsIO;
        DataFIS dataFIS;

        string serial;
        string resultFIS;

        int quantityScrews;
        public string TryScannerLON(eTypeConnection typeScanner)
        {
            try
            {
                scanner = new Scanner(sessionApp, typeScanner);
                serial = scanner.ScanQR("LON");
                Debug.WriteLine($"{DateTime.Now} - " + $"Prueba {typeScanner} LEE CODIGO SERIAL: {serial}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
            }
            return serial;
        }
        public void TryFIS()
        {
            serial = "serial de prueba";
            fIS = new CommunicationFIS(sessionApp);            
            dataFIS = fIS.SendBREQToFIS(serial);
            if (resultFIS.Contains("PASS"))
            {
                Debug.WriteLine($"{DateTime.Now} - "  + "Si paso");
            }
        }
        public string TryFIS_BREQToFIS(string numSerial)
        {            
            try
            {
                dataFIS = new CommunicationFIS(sessionApp).SendBREQToFIS(numSerial);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + ex.Message);
                return "Error";
            }
            
            return dataFIS.from_fis;           
        }
        public string TryFIS_BCMP(string numSerial)
        {
            string ResultFIS = string.Empty;
            try
            {
                dataFIS = new CommunicationFIS(sessionApp).BCMP(numSerial,true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
                return "Error";
            }

            return dataFIS.from_fis;
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
        public BitmapImage TryVisionSystem(eTypeConnection typeCamera, string serial)
        {
            BitmapImage bitmapImage =  new BitmapImage();             
          
            //#if !DEBUG
            visionSystem = new VisionSystem(sessionApp, typeCamera);
           
            if (!visionSystem.FirstInspectionAttempt(serial))
            {

                Debug.WriteLine($"{DateTime.Now} - " + "No paso");
                bitmapImage = visionSystem.getNameImageResultFromCamera(false);
                visionSystem.Disconnect();
                Debug.WriteLine($"{DateTime.Now} - "  + "Fallo el intento");
            }
            else
            {
                Debug.WriteLine($"{DateTime.Now} - "  + "Si paso");
                bitmapImage = visionSystem.getNameImageResultFromCamera(true);
                visionSystem.Disconnect();
            }
            //#endif
            return bitmapImage;
        }
        public void TryScrewdriver()
        {

        }
        public void TryStartSensor()
        {   
            sensorsIO.startRead();
        }
        public void TryEndSensor()
        {           
            sensorsIO.endRead();
        }
        public void TrySendDataSensorsM1()
        {
            sensorsIO.SendDataOutpusM1();
        }
        public void TrySendDataSensorsM2()
        {
            sensorsIO.SendDataOutpusM2();
        }
        public void TrySendDataSensorsM3()
        {
            sensorsIO.SendDataOutpusM3();
        }
    }
}

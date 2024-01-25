using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class WorkStation_Manual
    {
        
        SessionApp sessionApp;
        //Componentes de la Workstation
        ADUS adus;
        ErgoArm ergoArm;
        Screw screw;
        Robot robot;
        Scanner scanner;
        Fis fis;
        

        public WorkStation_Manual(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;

            adus = new ADUS(sessionApp);
            ergoArm = new ErgoArm(sessionApp);
            screw = new Screw(sessionApp);
            fis = new Fis();            
        }

        public void runWorkstation(IProgress<string> progress, ScannerDataProcessedEventArgs e)
        {            
            //1.-Inicializamos los ADUS para leer el estado de la estación
            initializeADUS(progress);
            //Obtenemos las ip de los scanners por que pueden existir varios
            string ipScanner_1 = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals(eTypeDevices.Scanner) && x.idTypeConnection.Equals(eTypeConnection.Main)).IP;
            string ipScanner_2 = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals(eTypeDevices.Scanner) && x.idTypeConnection.Equals(eTypeConnection.Main)).IP;
            //Conectamos los scanners 
            //scanner.Connect(ipScanner_1);
            //scanner.Connect(ipScanner_2);
            //Validamos si escaneo
            if(isScanned(e))
            {
                //Guardamos el modelo en BD

            }

        }
        public bool isScanned(ScannerDataProcessedEventArgs e)
        {
            return !scanner.isScannCompleted(e);
        }

        public void initializeADUS(IProgress<string> progress)
        {
            Task.Run(async () =>
            {
                await adus.startReadADUS(progress);
            }).Wait();
        }
       

    }
}

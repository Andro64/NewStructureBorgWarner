using BORGWARNER_SERVOPRESS.DataModel;
using Keyence.AutoID.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Threading;


namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class Scanner
    {
        ReaderAccessor scanner;
        SessionApp sessionApp;
        eTypeConnection _eTypeConnection;
               

        public Scanner(SessionApp _sessionApp, eTypeConnection typeConnection)
        {
            sessionApp = _sessionApp;
            scanner = new ReaderAccessor();
            _eTypeConnection = typeConnection;
            if (sessionApp.QR == null)
            {
                sessionApp.QR = new QRs();
            }
        }

        // Delegado y evento para notificar a la capa de presentación
        public delegate void DataProcessedEventHandler(object sender, ScannerDataProcessedEventArgs e);
        public event DataProcessedEventHandler DataProcessed;

        public void Connect()
        {
            string ipScanner = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals(eTypeDevices.Scanner) && x.idTypeConnection.Equals(_eTypeConnection)).IP;
            
            scanner.IpAddress = ipScanner;   
            scanner.Connect((data) =>
            {
                // Notificar a la capa de presentación
                OnDataProcessed(new ScannerDataProcessedEventArgs(Encoding.ASCII.GetString(data)));                
            });
        }
        protected virtual void OnDataProcessed(ScannerDataProcessedEventArgs e)
        {
            DataProcessed?.Invoke(this, e);
        }

        public void ExecuteCommand(string command)
        {
            scanner.ExecCommand(command);
        }

        public void DisconnectScanner()
        {
            scanner.Disconnect();            
        }
        public bool isScannCompleted(ScannerDataProcessedEventArgs e)
        {
            return !e.Result.Equals(String.Empty);
        }
        public string ScanQR(string command)
        {
            Connect();
            string serial = scanner.ExecCommand(command);
            switch (_eTypeConnection)
            {                
                case eTypeConnection.Scan_1:
                    sessionApp.QR.scan1 = serial;
                    break;
                case eTypeConnection.Scan_2:
                    sessionApp.QR.scan2 = serial;
                    break;
                default:
                    break;
            }
            DisconnectScanner();
            return serial;
            
        }

    }
}

using BORGWARNER_SERVOPRESS.DataModel;
using Keyence.AutoID.SDK;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            try
            {
                string ipScanner = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals((int)eTypeDevices.Scanner) && x.idTypeConnection.Equals((int)_eTypeConnection)).IP;
                int port = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals((int)eTypeDevices.Scanner) && x.idTypeConnection.Equals((int)_eTypeConnection)).Port;

                scanner.IpAddress = ipScanner;
                scanner.CommandPort = port;
                scanner.DataPort = port;

                scanner.Connect((data) =>
                {
                // Notificar a la capa de presentación
                OnDataProcessed(new ScannerDataProcessedEventArgs(Encoding.ASCII.GetString(data)));
                });
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
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
            string serial = string.Empty;
            Connect();
            if (scanner.LastErrorInfo.Equals(ErrorCode.None))
            {
                serial = scanner.ExecCommand(command);
                
                Thread.Sleep(500);

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
            }
            else
            {
                Debug.WriteLine("Error: " + scanner.LastErrorInfo);
            }
            DisconnectScanner();
            return serial;
            
        }
        public async Task<string> ScanningTrigger(CancellationTokenSource _cancellationTokenSource, string command)
        {
            string serial = string.Empty;
            Connect();
            if (scanner.LastErrorInfo.Equals(ErrorCode.None))
            {
                while (serial == string.Empty)
                {
                    await Task.Delay(1000);
                    serial = scanner.ExecCommand(command);
                }
            }
            else
            {
                Debug.WriteLine("Error: " + scanner.LastErrorInfo);
            }
            DisconnectScanner();
            return serial;

        }

    }
}

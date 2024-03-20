using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public class ViewManual : INotifyPropertyChanged
    {
        private SessionApp sessionApp;
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private string _profile;
        public string Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }
        private string _nameWorksation;
        public string NameWorksation
        {
            get { return _nameWorksation; }
            set { _nameWorksation = value; }
        }

        private string _timestamp;
        public string Timestamp
        {
            get { return _timestamp; }
            set
            {
                if (_timestamp != value)
                {
                    _timestamp = value;
                    OnPropertyChanged(nameof(Timestamp));
                }
            }
        }

        private string _encoder1;
        public string Encoder1
        {
            get { return _encoder1; }
            set
            {
                if (_encoder1 != value)
                {
                    _encoder1 = value;
                    OnPropertyChanged(nameof(Encoder1));
                }
            }
        }
        private string _encoder2;
        public string Encoder2
        {
            get { return _encoder2; }
            set
            {
                if (_encoder2 != value)
                {
                    _encoder2 = value;
                    OnPropertyChanged(nameof(Encoder2));
                }
            }
        }
        private string _MessageTorque;
        public string MessageTorque
        {
            get { return _MessageTorque; }
            set
            {
                if (_MessageTorque != value)
                {
                    _MessageTorque = value;
                    OnPropertyChanged(nameof(MessageTorque));
                }
            }
        }

        private string _scannerSelected;
        public string ScannerSelected
        {
            get { return _scannerSelected; }
            set
            {
                _scannerSelected = value;
                OnPropertyChanged(nameof(ScannerSelected));
            }
        }
        private string _cameraSelected;
        public string CameraSelected
        {
            get { return _cameraSelected; }
            set
            {
                _cameraSelected = value;
                OnPropertyChanged(nameof(CameraSelected));
            }
        }
        private string _programsSelected;
        public string ProgramsSelected
        {
            get { return _programsSelected; }
            set
            {
                _programsSelected = value;
                OnPropertyChanged(nameof(ProgramsSelected));
            }
        }

        public ObservableCollection<string> lstScanners { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> lstPrograms { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> lstCameras { get; set; } = new ObservableCollection<string>();

        public ViewManual(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;

            ShowDate();
            ShowData();
            populateScanners();
            populateCameras();
            populatePrograms();
        }

        private void populateScanners()
        {
            lstScanners = new ObservableCollection<string>();
            foreach (var item in sessionApp.connectionsWorkStation.Where(x => x.idTypeDevice.Equals((int)eTypeDevices.Scanner)).ToList()) //4 - Camaras
            {
                lstScanners.Add(item.TypeConnection);
            }
            ScannerSelected = lstScanners.FirstOrDefault();
        }
        private void populateCameras()
        {
            lstCameras = new ObservableCollection<string>();
            string typeCamara;
            foreach (var item in sessionApp.connectionsWorkStation.Where(x => x.idTypeDevice.Equals((int)eTypeDevices.Camara)).ToList()) //5 - Scanner
            {
                typeCamara = sessionApp.commandCamaras.FirstOrDefault(x=>x.des_type_connection.Equals(item.TypeConnection)).des_type_camara;
                lstCameras.Add(item.TypeConnection + " - " + typeCamara);
            }
            CameraSelected = lstCameras.FirstOrDefault();
        }
        private void populatePrograms()
        {
            lstPrograms = new ObservableCollection<string>();
            Programs_ScrewDriver programs_Screwdriver  = new CommunicationScrewDriver(sessionApp).getPrograms_ScrewDriver();
            lstPrograms.Add(programs_Screwdriver.screwing);
            lstPrograms.Add(programs_Screwdriver.rescrewing);
            lstPrograms.Add(programs_Screwdriver.unscrewing);
            lstPrograms.Add(programs_Screwdriver.simulated);

            ProgramsSelected = lstPrograms.FirstOrDefault();
        }
        public void ShowData()
        {
            UserName = sessionApp.user.userName;
            Profile = sessionApp.user.profile_description;
            NameWorksation = sessionApp.typeWorkstation.description;
            Encoder1 = "";
            Encoder2 = "";            
        }

        public void ShowDate()
        {
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, args) =>
            {
                Timestamp = DateTime.Now.ToString();
                //Por el momento metemos aqui los mensaje
                Encoder1 = sessionApp.positionErgoArm.encoder1.ToString();
                Encoder2 = sessionApp.positionErgoArm.encoder2.ToString();
                MessageTorque = sessionApp.messageTorque;
            };
            timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

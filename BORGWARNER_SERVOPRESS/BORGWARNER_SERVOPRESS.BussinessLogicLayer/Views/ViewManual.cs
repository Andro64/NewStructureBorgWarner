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

        public ViewManual(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;

            ShowDate();
            ShowData();
            populateScanners();
            populatePrograms();
        }

        private void populateScanners()
        {
            lstScanners = new ObservableCollection<string>();
            foreach (var item in sessionApp.connectionsWorkStation.Where(x => x.idTypeDevice.Equals(5)).ToList())
            {
                lstScanners.Add(item.TypeConnection);
            }
            ScannerSelected = lstScanners.FirstOrDefault();
        }
        private void populatePrograms()
        {
            lstPrograms = new ObservableCollection<string>();

            for (int i = 1; i <= 10; i++)
            {
                lstPrograms.Add(i.ToString().PadLeft(2, '0'));
            }

            ProgramsSelected = lstPrograms.FirstOrDefault();
        }
        public void ShowData()
        {
            UserName = sessionApp.user.userName;
            Profile = sessionApp.user.profile_description;
            NameWorksation = sessionApp.typeWorkstation.description;
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

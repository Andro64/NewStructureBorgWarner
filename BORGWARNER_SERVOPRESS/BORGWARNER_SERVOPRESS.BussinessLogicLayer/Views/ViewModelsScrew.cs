using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;


namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public class ViewModelsScrew : INotifyPropertyChanged
    {
        private SessionApp sessionApp;
        private Settings settingsGeneral;
        private ObservableCollection<ModelViewModelsScrew> _ResultModelsScrews;
        private ModelViewModelsScrew _registerSelected;
        private CommunicationScrew communicationScrew;
        private IMessageBoxService messageBoxService;

        public ObservableCollection<ModelViewModelsScrew> ResultModelsScrews
        {
            get { return _ResultModelsScrews; }
            set
            {
                if (_ResultModelsScrews != value)
                {
                    _ResultModelsScrews = value;
                    OnPropertyChanged(nameof(ResultModelsScrews));
                }
            }
        }
        public ObservableCollection<int> lstComboPages { get; set; } = new ObservableCollection<int>();
        public ObservableCollection<string> lstScanners { get; set; } = new ObservableCollection<string>();
        public ModelViewModelsScrew RegisterSelected
        {
            get { return _registerSelected; }
            set
            {
                if (_registerSelected != value)
                {
                    _registerSelected = value;                    
                    if (_registerSelected != null)
                    {                        
                        OnPropertyChanged(nameof(RegisterSelected));
                    }
                    
                }
            }
        }
        private int _pageSelected;
        public int PageSelected
        {
            get { return _pageSelected; }
            set
            {
                _pageSelected = value;
                OnPropertyChanged(nameof(PageSelected));
            }
        }
        private int _total_pages_grid;
        public int total_pages_grid
        {
            get { return _total_pages_grid; }
            set
            {
                _total_pages_grid = value;
                OnPropertyChanged(nameof(total_pages_grid));
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

        public ICommand SaveCommand { get; private set; }
        public ICommand CreateCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ReadCommand { get; private set; }
        public ICommand SelectComboPageCommand { get; }
        

        public ViewModelsScrew(SessionApp _sessionApp, IMessageBoxService messageBoxService)
        {
            sessionApp = _sessionApp;
            settingsGeneral = new Settings(sessionApp);

            SaveCommand = new RelayCommand<object>(Save, CanYouSave);
            CreateCommand = new RelayCommand<object>(Create, CanYouCreate);
            ReadCommand = new RelayCommand<object>(Read, CanYouRead);
            UpdateCommand = new RelayCommand<object>(Update, CanYouUpdate);
            DeleteCommand = new RelayCommand<object>(Delete, CanYouDelete);
            SelectComboPageCommand = new RelayCommand<int>(Page_SelectionChanged);
            

            ResultModelsScrews = new ObservableCollection<ModelViewModelsScrew>();            
            RegisterSelected = new ModelViewModelsScrew();
            communicationScrew = new CommunicationScrew(sessionApp);

            this.messageBoxService = messageBoxService;

            ShowDate();
            ShowData();
            InitializeGrid();            
            Read(null);
            
        }

        private void InitializeGrid()
        {
            sessionApp.lstTotalRegistersByTables = settingsGeneral.getTotalRegByTables();
            populatePages();
            populateScanners();
        }
        public void ShowData()
        {
            UserName = sessionApp.user.userName;
            Profile = sessionApp.user.profile_description;
            NameWorksation = sessionApp.typeWorkstation.description;
        }
        private void populatePages()
        {
            lstComboPages = new ObservableCollection<int>();
            foreach (var page in sessionApp.lstTotalRegistersByTables.FirstOrDefault(x => x.NameTable.Equals("models_screw")).Pages)
            {
                lstComboPages.Add(page);
            }

            total_pages_grid = sessionApp.lstTotalRegistersByTables.FirstOrDefault(x => x.NameTable.Equals("models_screw")).NumPages;
            PageSelected = 1;
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
        private void cleanControls()
        {
            if (RegisterSelected != null)
            {
                RegisterSelected.id = 0;
                RegisterSelected.partNumber = string.Empty;
                RegisterSelected.serial = string.Empty;
                RegisterSelected.name_model = string.Empty;
                RegisterSelected.description = string.Empty;
                RegisterSelected.quantity_screws = 0;
            }
            InitializeGrid();
        }
        private void Page_SelectionChanged(int pageSelected)
        {
            Read(pageSelected);
        }
        private void Save(object parameter)
        {
            MessageBoxResult result = messageBoxService.Show("¿Está seguro de guardar la información?", "Confirmación", MessageBoxButton.OKCancel,eMessageBoxIcon.Information);
            if (result == MessageBoxResult.OK)
            {
                communicationScrew.Ins_Upd_ModelViewModelsScrew(RegisterSelected);
                cleanControls();
                Read(PageSelected);
            }
        }

        private bool CanYouSave(object parameter)
        {            
            return RegisterSelected != null && RegisterSelected.IsValid(); // Agrega lógica de validación si es necesario
        }
        private void Create(object parameter)
        {
            cleanControls();
            Read(1);
        }

        private bool CanYouCreate(object parameter)
        {
            return true;
            //return RegisterSelected != null && RegisterSelected.IsValid(); // Agrega lógica de validación si es necesario
        }
        private void Update(object parameter)
        {
            MessageBoxResult result = messageBoxService.Show("¿Está seguro de actualizar la información?", "Confirmación", MessageBoxButton.OKCancel, eMessageBoxIcon.Information);
            if (result == MessageBoxResult.OK)
            {
                communicationScrew.Ins_Upd_ModelViewModelsScrew(RegisterSelected);
                cleanControls();
                Read(PageSelected);
            }
        }

        private bool CanYouUpdate(object parameter)
        {
            return RegisterSelected != null && RegisterSelected.IsValid(); // Agrega lógica de validación si es necesario
        }

        private void Delete(object parameter)
        {
            MessageBoxResult result = messageBoxService.Show("¿Está seguro de borrar la información?", "Confirmación", MessageBoxButton.OKCancel, eMessageBoxIcon.Information);
            if (result == MessageBoxResult.OK)
            {
                communicationScrew.Del_ModelViewModelsScrew(RegisterSelected);
                cleanControls();
                Read(PageSelected);
            }
        }

        private bool CanYouDelete(object parameter)
        {
            return RegisterSelected != null;
        }

        private void Read(object parameter)
        {            
            ResultModelsScrews.Clear();            
            ResultModelsScrews = new ObservableCollection<ModelViewModelsScrew>(communicationScrew.getModelViewModelsScrew(PageSelected));            
        }

        private bool CanYouRead(object parameter)
        {
            return true; 
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

using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public class ViewSettings : INotifyPropertyChanged
    {
        private SessionApp sessionApp;
        private Settings settingsGeneral;
        private ObservableCollection<ModelViewSettings> _ResultSettings;
        private ModelViewSettings _registerSelected;
        private CommunicationSettings CommunicationSettings;
        private IMessageBoxService messageBoxService;
        public ObservableCollection<ModelViewSettings> ResultSettings
        {
            get { return _ResultSettings; }
            set
            {
                if (_ResultSettings != value)
                {
                    _ResultSettings = value;
                    OnPropertyChanged(nameof(ResultSettings));
                }
            }
        }
        public ObservableCollection<int> lstComboPages { get; set; } = new ObservableCollection<int>();
        public ObservableCollection<ModelViewTypeWorkstation> lstTypeWorkstation { get; set; } = new ObservableCollection<ModelViewTypeWorkstation>();
        public ModelViewSettings RegisterSelected
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
        private int _TypeWorkstationSelected_Id;
        public int TypeWorkstationSelected_Id
        {
            get { return _TypeWorkstationSelected_Id; }
            set
            {
                _TypeWorkstationSelected_Id = value;
                OnPropertyChanged(nameof(TypeWorkstationSelected_Id));
            }
        }
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        private string _profile;
        public string Profile
        {
            get { return _profile; }
            set
            {
                _profile = value;
                OnPropertyChanged(nameof(Profile));
            }
        }
        private string _nameWorksation;
        public string NameWorksation
        {
            get { return _nameWorksation; }
            set
            {
                _nameWorksation = value;
                OnPropertyChanged(nameof(NameWorksation));
            }
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CreateCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ReadCommand { get; private set; }
        public ICommand SelectComboPageCommand { get; }

        public ViewSettings(SessionApp _sessionApp, IMessageBoxService messageBoxService)
        {
            sessionApp = _sessionApp;
            settingsGeneral = new Settings(sessionApp);

            SaveCommand = new RelayCommand<object>(Save, CanYouSave);
            CreateCommand = new RelayCommand<object>(Create, CanYouCreate);
            ReadCommand = new RelayCommand<object>(Read, CanYouRead);
            UpdateCommand = new RelayCommand<object>(Update, CanYouUpdate);
            DeleteCommand = new RelayCommand<object>(Delete, CanYouDelete);
            SelectComboPageCommand = new RelayCommand<int>(Page_SelectionChanged);


            ResultSettings = new ObservableCollection<ModelViewSettings>();
            RegisterSelected = new ModelViewSettings();
            CommunicationSettings = new CommunicationSettings(sessionApp);

            this.messageBoxService = messageBoxService;

            ShowData();
            ShowDate();
            InitializeGrid();
            Read(null);

        }

        private void InitializeGrid()
        {
            sessionApp.lstTotalRegistersByTables = settingsGeneral.getTotalRegByTables();
            populatePages();
            populateTypeWorkstation();
        }
        public void ShowData()
        {
            UserName = sessionApp.user.userName;
            Profile = sessionApp.user.profile_description;
            NameWorksation = sessionApp.typeWorkstation.description;
        }
        private void populatePages()
        {
            try
            {
                lstComboPages = new ObservableCollection<int>();
                foreach (var page in sessionApp.lstTotalRegistersByTables.FirstOrDefault(x => x.NameTable.Equals("settings")).Pages)
                {
                    lstComboPages.Add(page);
                }

                total_pages_grid = sessionApp.lstTotalRegistersByTables.FirstOrDefault(x => x.NameTable.Equals("settings")).NumPages;
                PageSelected = 1;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
            }
        }
        private void populateTypeWorkstation()
        {
            lstTypeWorkstation = new ObservableCollection<ModelViewTypeWorkstation>();
            List<ModelViewTypeWorkstation> lst_TypeWorkstation = CommunicationSettings.getModelViewTypeWorkstation();
            foreach (var item in lst_TypeWorkstation)
            {
                lstTypeWorkstation.Add(item);
            }
            TypeWorkstationSelected_Id = sessionApp.typeWorkstation.id;
        }
        private void cleanControls()
        {
            if (RegisterSelected != null)
            {
                RegisterSelected.id = 0;
                RegisterSelected.setting = string.Empty;
                RegisterSelected.valueSetting = string.Empty;
            }
            InitializeGrid();
        }
        private void Page_SelectionChanged(int pageSelected)
        {
            Read(pageSelected);
        }
        private void Save(object parameter)
        {
            MessageBoxResult result = messageBoxService.Show("¿Está seguro de guardar la información?", "Confirmación", MessageBoxButton.OKCancel, eMessageBoxIcon.Information);
            if (result == MessageBoxResult.OK)
            {
                CommunicationSettings.Ins_Upd_ModelViewSettings(RegisterSelected);
                UpdateSession();
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
                CommunicationSettings.Ins_Upd_ModelViewSettings(RegisterSelected);
                UpdateSession();
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
                CommunicationSettings.Del_ModelViewSettings(RegisterSelected);
                UpdateSession();
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
            ResultSettings.Clear();
            ResultSettings = new ObservableCollection<ModelViewSettings>(CommunicationSettings.getModelViewSettings(PageSelected));
        }

        private bool CanYouRead(object parameter)
        {
            return true;
        }

        private void UpdateSession()
        {
            sessionApp.settings = settingsGeneral.getSettings();
            sessionApp.typeWorkstation = settingsGeneral.getTypeWorksatiton();
            ShowData();
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

using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public class ViewSettings : INotifyPropertyChanged
    {
        private SessionApp sessionApp;
        private Settings settingsGeneral;
        private ObservableCollection<ModelViewSettings> _ResultSettings;
        private ModelViewSettings _registerSelected;
        private CommunicationSettings CommunicationSettings;
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
        public ObservableCollection<int> lstComboPages { get; } = new ObservableCollection<int>();
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

        public ICommand SaveCommand { get; private set; }
        public ICommand CreateCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ReadCommand { get; private set; }
        public ICommand SelectComboPageCommand { get; }

        public ViewSettings(SessionApp _sessionApp)
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


            InitializeGrid();
            Read(null);

        }

        private void InitializeGrid()
        {
            sessionApp.lstTotalRegistersByTables = settingsGeneral.getTotalRegByTables();
            //lstComboPages.Clear();
            foreach (var page in sessionApp.lstTotalRegistersByTables.FirstOrDefault(x => x.NameTable.Equals("settings")).Pages)
            {
                lstComboPages.Add(page);
            }

            total_pages_grid = sessionApp.lstTotalRegistersByTables.FirstOrDefault(x => x.NameTable.Equals("settings")).NumPages;
            PageSelected = 1;
        }
        private void cleanControls()
        {
            RegisterSelected.id = 0;
            RegisterSelected.setting = string.Empty;
            RegisterSelected.valueSetting = string.Empty;            
            InitializeGrid();
        }
        private void Page_SelectionChanged(int pageSelected)
        {
            Read(pageSelected);
        }
        private void Save(object parameter)
        {
            CommunicationSettings.Ins_Upd_ModelViewSettings(RegisterSelected);
            cleanControls();
            Read(PageSelected);
        }

        private bool CanYouSave(object parameter)
        {
            return RegisterSelected != null && RegisterSelected.IsValid(); // Agrega lógica de validación si es necesario
        }
        private void Create(object parameter)
        {
            //cleanControls();
            Read(1);
        }

        private bool CanYouCreate(object parameter)
        {
            return true;
            //return RegisterSelected != null && RegisterSelected.IsValid(); // Agrega lógica de validación si es necesario
        }
        private void Update(object parameter)
        {
            CommunicationSettings.Ins_Upd_ModelViewSettings(RegisterSelected);
            cleanControls();
            Read(PageSelected);
        }

        private bool CanYouUpdate(object parameter)
        {
            return RegisterSelected != null && RegisterSelected.IsValid(); // Agrega lógica de validación si es necesario
        }

        private void Delete(object parameter)
        {
            CommunicationSettings.Del_ModelViewSettings(RegisterSelected);
            cleanControls();
            Read(PageSelected);
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

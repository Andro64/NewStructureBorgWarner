using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public class ViewPositionScrew : INotifyPropertyChanged
    {
        private SessionApp sessionApp;
        private Settings settingsGeneral;
        private ObservableCollection<ModelViewPositionScrew> _ResultData;
        private ModelViewPositionScrew _registerSelected;
        private CommunicationScrew CommunicationScrew;
        
        public ObservableCollection<ModelViewPositionScrew> ResultData
        {
            get { return _ResultData; }
            set
            {
                if (_ResultData != value)
                {
                    _ResultData = value;
                    OnPropertyChanged(nameof(ResultData));
                }
            }
        }
        public ObservableCollection<int> lstComboPages { get; set; } = new ObservableCollection<int>();
        public ObservableCollection<ModelViewModelsScrew> lstModelScrew { get; } = new ObservableCollection<ModelViewModelsScrew>();
        
        public ModelViewPositionScrew RegisterSelected
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

        private int ModelScrewSelected;
        public int _modelScrewSelected
        {
            get { return _modelScrewSelected; }
            set
            {
                _modelScrewSelected = value;
                OnPropertyChanged(nameof(ModelScrewSelected));
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
        public ICommand SaveCommand { get; private set; }
        public ICommand CreateCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ReadCommand { get; private set; }
        public ICommand SelectComboPageCommand { get; }
        public ICommand SelectChangedModelScrew  { get; private set; }
        


        public ViewPositionScrew(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            settingsGeneral = new Settings(sessionApp);

            SaveCommand = new RelayCommand<object>(Save, CanYouSave);
            CreateCommand = new RelayCommand<object>(Create, CanYouCreate);
            ReadCommand = new RelayCommand<object>(Read, CanYouRead);
            UpdateCommand = new RelayCommand<object>(Update, CanYouUpdate);
            DeleteCommand = new RelayCommand<object>(Delete, CanYouDelete);
            SelectComboPageCommand = new RelayCommand<int>(Page_SelectionChanged);
            SelectChangedModelScrew = new RelayCommand<int>(ModelScrew_SelectionChanged);
            

            ResultData = new ObservableCollection<ModelViewPositionScrew>();
            RegisterSelected = new ModelViewPositionScrew();
            CommunicationScrew = new CommunicationScrew(sessionApp);

            ShowDate();
            InitializeGrid();
            Read(null);

        }

        private void InitializeGrid()
        {
            sessionApp.lstTotalRegistersByTables = settingsGeneral.getTotalRegByTables();
            populatePages();
            populateModelScrew();
        }
       
        private void populatePages()
        {
            try
            {
                lstComboPages = new ObservableCollection<int>();
                foreach (var page in sessionApp.lstTotalRegistersByTables.FirstOrDefault(x => x.NameTable.Equals("screws")).Pages)
                {
                    lstComboPages.Add(page);
                }

                total_pages_grid = sessionApp.lstTotalRegistersByTables.FirstOrDefault(x => x.NameTable.Equals("screws")).NumPages;
                PageSelected = 1;                
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private void populateModelScrew()
        {
            List<ModelViewModelsScrew> lst_modelScrew =  CommunicationScrew.getModelViewModelsScrew();            
            foreach (var item in lst_modelScrew)
            {
                lstModelScrew.Add(item);
            }
            ModelScrewSelected = lstModelScrew.FirstOrDefault().id;
        }
        private void cleanControls()
        {
            RegisterSelected.id = 0;
            RegisterSelected.id_screw = 0;
            RegisterSelected.encoder1 = 0;
            RegisterSelected.encoder2 = 0;
            RegisterSelected.tolerance = 0;
            RegisterSelected.id_model_screw = 0;
            InitializeGrid();
        }
        private void ModelScrew_SelectionChanged(int pageSelected)
        {
            //Read(pageSelected);
        }
        private void Page_SelectionChanged(int pageSelected)
        {
            Read(pageSelected);
        }
        private void Save(object parameter)
        {
            CommunicationScrew.Ins_Upd_ModelViewPositionScrew(RegisterSelected);
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
            CommunicationScrew.Ins_Upd_ModelViewPositionScrew(RegisterSelected);
            cleanControls();
            Read(PageSelected);
        }

        private bool CanYouUpdate(object parameter)
        {
            return RegisterSelected != null && RegisterSelected.IsValid(); // Agrega lógica de validación si es necesario
        }

        private void Delete(object parameter)
        {
            CommunicationScrew.Del_ModelViewPositionScrew(RegisterSelected);
            cleanControls();
            Read(PageSelected);
        }

        private bool CanYouDelete(object parameter)
        {
            return RegisterSelected != null;
        }

        private void Read(object parameter)
        {
            ResultData.Clear();
            ResultData = new ObservableCollection<ModelViewPositionScrew>(CommunicationScrew.getModelViewPositionScrew(PageSelected));
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

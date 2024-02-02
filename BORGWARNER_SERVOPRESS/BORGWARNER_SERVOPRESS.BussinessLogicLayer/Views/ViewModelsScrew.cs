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
    public class ViewModelsScrew : INotifyPropertyChanged
    {
        private SessionApp sessionApp;        
        private ObservableCollection<ModelViewModelsScrew> _ResultModelsScrews;
        private ModelViewModelsScrew _registerSelected;
        private CommunicationScrew communicationScrew;
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

        public ICommand SaveCommand { get; private set; }
        public ICommand CreateCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ReadCommand { get; private set; }

        public ViewModelsScrew(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;

            SaveCommand = new RelayCommand(Save, CanYouSave);
            CreateCommand = new RelayCommand(Create, CanYouCreate);
            ReadCommand = new RelayCommand(Read, CanYouRead);
            UpdateCommand = new RelayCommand(Update, CanYouUpdate);
            DeleteCommand = new RelayCommand(Delete, CanYouDelete);
            


            ResultModelsScrews = new ObservableCollection<ModelViewModelsScrew>();            
            RegisterSelected = new ModelViewModelsScrew();
            communicationScrew = new CommunicationScrew(sessionApp);

            Read(null);
        }

        private void Save(object parameter)
        {
            communicationScrew.Ins_Upd_ModelViewModelsScrew(RegisterSelected);
            Read(null);
        }

        private bool CanYouSave(object parameter)
        {            
            return RegisterSelected != null && RegisterSelected.IsValid(); // Agrega lógica de validación si es necesario
        }
        private void Create(object parameter)
        {            
            communicationScrew.Ins_Upd_ModelViewModelsScrew(RegisterSelected);
            Read(null);
        }

        private bool CanYouCreate(object parameter)
        {
            return RegisterSelected != null && RegisterSelected.IsValid(); // Agrega lógica de validación si es necesario
        }
        private void Update(object parameter)
        {            
            communicationScrew.Ins_Upd_ModelViewModelsScrew(RegisterSelected);
            Read(null);
        }

        private bool CanYouUpdate(object parameter)
        {
            return RegisterSelected != null && RegisterSelected.IsValid(); // Agrega lógica de validación si es necesario
        }

        private void Delete(object parameter)
        {
            communicationScrew.Del_ModelViewModelsScrew(RegisterSelected);
            Read(null);
        }

        private bool CanYouDelete(object parameter)
        {
            return RegisterSelected != null;
        }

        private void Read(object parameter)
        {            
            ResultModelsScrews.Clear();            
            ResultModelsScrews = new ObservableCollection<ModelViewModelsScrew>(communicationScrew.getModelViewModelsScrew());            
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

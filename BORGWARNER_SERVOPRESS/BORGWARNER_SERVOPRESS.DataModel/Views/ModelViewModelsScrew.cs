using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel.Views
{
    public class ModelViewModelsScrew: INotifyPropertyChanged
    {

        private int _id;
        private string _partNumber;
        private string _serial;
        private string _name_model;
        private string _description;
        private int _quantity_screws;
        public int id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(id));
                }
            }
        }
        public string partNumber
        {
            get { return _partNumber; }
            set
            {
                if (_partNumber != value)
                {
                    _partNumber = value;
                    OnPropertyChanged(nameof(partNumber));
                }
            }
        }
        public string serial
        {
            get { return _serial; }
            set
            {
                if (_serial != value)
                {
                    _serial = value;
                    OnPropertyChanged(nameof(serial));
                }
            }
        }
        public string name_model
        {
            get { return _name_model; }
            set
            {
                if (_name_model != value)
                {
                    _name_model = value;
                    OnPropertyChanged(nameof(name_model));
                }
            }
        }
        public string description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(description));
                }
            }
        }
        public int quantity_screws
        {
            get { return _quantity_screws; }
            set
            {
                if (_quantity_screws != value)
                {
                    _quantity_screws = value;
                    OnPropertyChanged(nameof(quantity_screws));
                }
            }
        }
       

        public bool IsValid()
        {
            // Agrega lógica de validación si es necesario
            return  !string.IsNullOrEmpty(partNumber) &&
                    !string.IsNullOrEmpty(serial) &&
                    !string.IsNullOrEmpty(name_model) &&
                    !string.IsNullOrEmpty(description);
                    
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

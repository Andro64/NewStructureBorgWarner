using System.ComponentModel;

namespace BORGWARNER_SERVOPRESS.DataModel.Views
{
    public class ModelViewSettings:INotifyPropertyChanged
    {
        private int _id;
        private string _setting;
        private string _valueSetting;

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
        public string setting
        {
            get { return _setting; }
            set
            {
                if (_setting != value)
                {
                    _setting = value;
                    OnPropertyChanged(nameof(setting));
                }
            }
        }
        public string valueSetting
        {
            get { return _valueSetting; }
            set
            {
                if (_valueSetting != value)
                {
                    _valueSetting = value;
                    OnPropertyChanged(nameof(valueSetting));
                }
            }
        }
     


        public bool IsValid()
        {
            // Agrega lógica de validación si es necesario
            return !string.IsNullOrEmpty(setting) &&
                    !string.IsNullOrEmpty(valueSetting);

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

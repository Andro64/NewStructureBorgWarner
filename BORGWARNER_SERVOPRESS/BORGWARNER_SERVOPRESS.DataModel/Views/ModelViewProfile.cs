using System.ComponentModel;

namespace BORGWARNER_SERVOPRESS.DataModel.Views
{
    public class ModelViewProfile : INotifyPropertyChanged
    {
        private int _id;
        private string _description;
        

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
        public bool IsValid()
        {
            // Agrega lógica de validación si es necesario
            return !string.IsNullOrEmpty(description);

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

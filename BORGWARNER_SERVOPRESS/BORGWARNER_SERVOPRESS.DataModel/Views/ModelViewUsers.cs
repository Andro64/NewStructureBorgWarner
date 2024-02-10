using System.ComponentModel;

namespace BORGWARNER_SERVOPRESS.DataModel.Views
{
    public class ModelViewUsers : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _lastName;
        private string _username;
        private string _password;
        private int _id_profile;
        private string _profile_description;

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
        public string name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(name));
                }
            }
        }
        public string lastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(lastName));
                }
            }
        }
        public string username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(username));
                }
            }
        }
        public string password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(password));
                }
            }
        }
        public int id_profile
        {
            get { return _id_profile; }
            set
            {
                if (_id_profile != value)
                {
                    _id_profile = value;
                    OnPropertyChanged(nameof(id_profile));
                }
            }
        }

        public string profile_description
        {
            get { return _profile_description; }
            set
            {
                if (_profile_description != value)
                {
                    _profile_description = value;
                    OnPropertyChanged(nameof(profile_description));
                }
            }
        }

        public bool IsValid()
        {
            // Agrega lógica de validación si es necesario
            return  !string.IsNullOrEmpty(name) &&
                    !string.IsNullOrEmpty(lastName) &&
                    !string.IsNullOrEmpty(username) &&
                    !string.IsNullOrEmpty(password);

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

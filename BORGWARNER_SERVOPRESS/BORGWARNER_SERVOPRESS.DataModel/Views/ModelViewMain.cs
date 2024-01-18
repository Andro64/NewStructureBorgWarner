using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel.Views
{    
    public class ModelViewMain: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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


        private int _numero;
        public int NumeroAleatorio
        {
            get { return _numero; }
            set
            {
                if (_numero != value)
                {
                    _numero = value;
                    OnPropertyChanged(nameof(NumeroAleatorio));
                }
            }
        }

        private string _messageScrew;
        public string MessageScrew
        {
            get { return _messageScrew; }
            set
            {
                if (_messageScrew != value)
                {
                    _messageScrew = value;
                    OnPropertyChanged(nameof(MessageScrew));
                }
            }
        }
    }
}

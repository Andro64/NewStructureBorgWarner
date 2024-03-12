using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

        private string _messageProcess;
        public string MessageProcess
        {
            get { return _messageProcess; }
            set
            {
                if (_messageProcess != value)
                {
                    _messageProcess = value;
                    OnPropertyChanged(nameof(MessageProcess));
                }
            }
        }
        private string _imageProcess;
        public string ImageOfProcess
        {
            get { return _imageProcess; }
            set
            {
                if (_imageProcess != value)
                {
                    _imageProcess = value;
                    OnPropertyChanged(nameof(ImageOfProcess));
                }
            }
        }
        private BitmapImage _BitMapImageOfProcess;

        public BitmapImage BitMapImageOfProcess
        {
            get { return _BitMapImageOfProcess; }
            set
            {
                if (_BitMapImageOfProcess != value)
                {
                    _BitMapImageOfProcess = value;
                    OnPropertyChanged(nameof(BitMapImageOfProcess));
                }
            }
        }

        private QRs _QRs_Scanned;
        public QRs QRs_Scanned
        {
            get { return _QRs_Scanned; }
            set
            {
                if (_QRs_Scanned != value)
                {
                    _QRs_Scanned = value;
                    OnPropertyChanged(nameof(QRs_Scanned));
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

        private int milliseconds;
        public int Milliseconds
        {
            get { return milliseconds; }
            set
            {
                milliseconds = value;
                OnPropertyChanged(nameof(Milliseconds));
            }
        }
        private double seconds;

        public double Seconds
        {
            get { return seconds; }
            set
            {
                seconds = value;
                OnPropertyChanged(nameof(Seconds));
            }
        }
    }
}

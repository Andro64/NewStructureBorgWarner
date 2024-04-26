using System;
using System.ComponentModel;

namespace BORGWARNER_SERVOPRESS.DataModel.Views
{
    public class ModelViewRunHistory: INotifyPropertyChanged
    {
        private int _id;
        private string _partNumber;
        private string _serial;
        private string _serial2;
        private DateTime _date;
        private string _result;
        private string _screw1Torque;
        private string _screw1Angle;
        private string _screw2Torque;
        private string _screw2Angle;
        private string _screw3Torque;
        private string _screw3Angle;
        private string _screw4Torque;
        private string _screw4Angle;        
        private string _screw5Torque;
        private string _screw5Angle;
        private string _ultracappadinspect;
        private string _ultracapboardinspect;
        private string _insulatorinspect;

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
        public string serial2
        {
            get { return _serial2; }
            set
            {
                if (_serial2 != value)
                {
                    _serial2 = value;
                    OnPropertyChanged(nameof(serial2));
                }
            }
        }
        public DateTime date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged(nameof(date));
                }
            }
        }
        public string result
        {
            get { return _result; }
            set
            {
                if (_result != value)
                {
                    _result = value;
                    OnPropertyChanged(nameof(result));
                }
            }
        }
        public string screw1Torque
        {
            get { return _screw1Torque; }
            set
            {
                if (_screw1Torque != value)
                {
                    _screw1Torque = value;
                    OnPropertyChanged(nameof(screw1Torque));
                }
            }
        }

        public string screw1Angle
        {
            get { return _screw1Angle; }
            set
            {
                if (_screw1Angle != value)
                {
                    _screw1Angle = value;
                    OnPropertyChanged(nameof(screw1Angle));
                }
            }
        }
        public string screw2Torque
        {
            get { return _screw2Torque; }
            set
            {
                if (_screw2Torque != value)
                {
                    _screw2Torque = value;
                    OnPropertyChanged(nameof(screw2Torque));
                }
            }
        }
        public string screw2Angle
        {
            get { return _screw2Angle; }
            set
            {
                if (_screw2Angle != value)
                {
                    _screw2Angle = value;
                    OnPropertyChanged(nameof(screw2Angle));
                }
            }
        }

        public string screw3Torque
        {
            get { return _screw3Torque; }
            set
            {
                if (_screw3Torque != value)
                {
                    _screw3Torque = value;
                    OnPropertyChanged(nameof(screw3Torque));
                }
            }
        }
        public string screw3Angle
        {
            get { return _screw3Angle; }
            set
            {
                if (_screw3Angle != value)
                {
                    _screw3Angle = value;
                    OnPropertyChanged(nameof(screw3Angle));
                }
            }
        }
        public string screw4Torque
        {
            get { return _screw4Torque; }
            set
            {
                if (_screw4Torque != value)
                {
                    _screw4Torque = value;
                    OnPropertyChanged(nameof(screw4Torque));
                }
            }
        }

        public string screw4Angle
        {
            get { return _screw4Angle; }
            set
            {
                if (_screw4Angle != value)
                {
                    _screw4Angle = value;
                    OnPropertyChanged(nameof(screw4Angle));
                }
            }
        }
        public string screw5Torque
        {
            get { return _screw5Torque; }
            set
            {
                if (_screw5Torque != value)
                {
                    _screw5Torque = value;
                    OnPropertyChanged(nameof(screw5Torque));
                }
            }
        }

        public string screw5Angle
        {
            get { return _screw5Angle; }
            set
            {
                if (_screw5Angle != value)
                {
                    _screw5Angle = value;
                    OnPropertyChanged(nameof(screw5Angle));
                }
            }
        }

        public string ultracappadinspect
        {
            get { return _ultracappadinspect; }
            set
            {
                if (_ultracappadinspect != value)
                {
                    _ultracappadinspect = value;
                    OnPropertyChanged(nameof(ultracappadinspect));
                }
            }
        }
        public string ultracapboardinspect
        {
            get { return _ultracapboardinspect; }
            set
            {
                if (_ultracapboardinspect != value)
                {
                    _ultracapboardinspect = value;
                    OnPropertyChanged(nameof(ultracapboardinspect));
                }
            }
        }
        public string insulatorinspect
        {
            get { return _insulatorinspect; }
            set
            {
                if (_insulatorinspect != value)
                {
                    _insulatorinspect = value;
                    OnPropertyChanged(nameof(insulatorinspect));
                }
            }
        }



        public bool IsValid()
        {
            // Agrega lógica de validación si es necesario
            return !string.IsNullOrEmpty(partNumber) &&                    
                    !string.IsNullOrEmpty(result);

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

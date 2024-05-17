using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel.Views
{
    public class ModelViewPositionScrew : INotifyPropertyChanged
    {
        private int _id;
        private int _id_screw;
        private double _encoder1;
        private double _encoder2;
        private double _tolerance;
        private int _id_model_screw;
        private int _text_position_X;
        private int _text_position_Y;

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
        public int id_screw
        {
            get { return _id_screw; }
            set
            {
                if (_id_screw != value)
                {
                    _id_screw = value;
                    OnPropertyChanged(nameof(id_screw));
                }
            }
        }
        public double encoder1
        {
            get { return _encoder1; }
            set
            {
                if (_encoder1 != value)
                {
                    _encoder1 = value;
                    OnPropertyChanged(nameof(encoder1));
                }
            }
        }
        public double encoder2
        {
            get { return _encoder2; }
            set
            {
                if (_encoder2 != value)
                {
                    _encoder2 = value;
                    OnPropertyChanged(nameof(encoder2));
                }
            }
        }
        public double tolerance
        {
            get { return _tolerance; }
            set
            {
                if (_tolerance != value)
                {
                    _tolerance = value;
                    OnPropertyChanged(nameof(tolerance));
                }
            }
        }
        public int id_model_screw
        {
            get { return _id_model_screw; }
            set
            {
                if (_id_model_screw != value)
                {
                    _id_model_screw = value;
                    OnPropertyChanged(nameof(id_model_screw));
                }
            }
        }
        public int text_position_X
        {
            get { return _text_position_X; }
            set
            {
                if (_text_position_X != value)
                {
                    _text_position_X = value;
                    OnPropertyChanged(nameof(text_position_X));
                }
            }
        }
        public int text_position_Y
        {
            get { return _text_position_Y; }
            set
            {
                if (_text_position_Y != value)
                {
                    _text_position_Y = value;
                    OnPropertyChanged(nameof(text_position_Y));
                }
            }
        }



        public bool IsValid()
        {
            // Agrega lógica de validación si es necesario
            //return !_id_screw == null &&
            //        !string.IsNullOrEmpty(valueSetting);
            return true;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

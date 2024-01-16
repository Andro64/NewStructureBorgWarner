using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    class IOCard_Type3
    {
        SessionApp sessionApp;
        private bool[] CardInputs;
        private bool[] CardOutput;
        private string NumSerial;
        public IOCard_Type3(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            initialize();
        }

        #region Input                
        bool _pA0;
        bool _pA1;
        bool _pA2;
        bool _pA3;
        bool _pB0;
        bool _pB1;
        bool _pB2;
        bool _pB3;
        public bool St11_Available { get { return _pA0; } }
        public bool PA1 { get { return _pA1; } }
        public bool PA2 { get { return _pA2; } }
        public bool PA3 { get { return _pA3; } }
        public bool PB0 { get { return _pB0; } }
        public bool PB1 { get { return _pB1; } }
        public bool PB2 { get { return _pB2; } }
        public bool PB3 { get { return _pB3; } }
        #endregion

        #region OutPuts        
        bool _k0;
        bool _k1;
        bool _k2;
        bool _k3;
        bool _k4;
        bool _k5;
        bool _k6;
        bool _k7;
        public bool St10_Available { get { return _k0; } set { _k0 = value; } }
        public bool K1 { get { return _k1; } set { _k1 = value; } }
        public bool K2 { get { return _k2; } set { _k2 = value; } }
        public bool K3 { get { return _k3; } set { _k3 = value; } }
        public bool K4 { get { return _k4; } set { _k4 = value; } }
        public bool K5 { get { return _k5; } set { _k5 = value; } }
        public bool K6 { get { return _k6; } set { _k6 = value; } }
        public bool K7 { get { return _k7; } set { _k7 = value; } }
        #endregion

        public void initialize()
        {
            NumSerial = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("ADU_SERIAL_3")).valueSetting;
        }
        //Este si se debe estar leyendo constantemente en un ciclo de lectura infinito
        public void getDataInput()
        {
            ADU ioADUCard = new ADU(NumSerial);
            CardInputs = ioADUCard.MapADUInput();

            _pA0 = CardInputs[0];
            _pA1 = CardInputs[1];
            _pA2 = CardInputs[2];
            _pA3 = CardInputs[3];
            _pB0 = CardInputs[4];
            _pB1 = CardInputs[5];
            _pB2 = CardInputs[6];
            _pB3 = CardInputs[7];
        }
        //Cada clic en el boton deberia enviar la info dentro del evento clic
        public void sendDataOutput()
        {
            ADU ioADUCard = new ADU(NumSerial);

            CardOutput[0] = _k0;
            CardOutput[1] = _k1;
            CardOutput[2] = _k2;
            CardOutput[3] = _k3;
            CardOutput[4] = _k4;
            CardOutput[5] = _k5;
            CardOutput[6] = _k6;
            CardOutput[7] = _k7;

            ioADUCard.MapADUOutput(CardOutput);
        }
    }
}

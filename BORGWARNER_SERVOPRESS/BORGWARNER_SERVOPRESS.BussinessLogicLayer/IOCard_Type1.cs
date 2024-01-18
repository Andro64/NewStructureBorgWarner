using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class IOCard_Type1
    {
        SessionApp sessionApp;
        Views.ViewMain viewMain;

        private bool[] CardInputs;
        private bool[] CardOutputs;
        private string NumSerial;
        public IOCard_Type1(SessionApp _sessionApp, Views.ViewMain _viewMain)
        {
            sessionApp = _sessionApp;
            viewMain = _viewMain;
            initialize();
        }

        public IOCard_Type1(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;            
            initialize();
        }

        #region Input                
        bool _pressure_Sensor;
        bool _prestoper_Pallet_Present;
        bool _stopper_Pallet_Present;
        bool _pA3;
        bool _toolActive;
        bool _pB1;
        bool _pB2;
        bool _e_Stop_Active;                       
        public bool Pressure_Sensor { get { return _pressure_Sensor; } }
        public bool Prestoper_Pallet_Present { get { return _prestoper_Pallet_Present; } }
        public bool Stopper_Pallet_Present { get { return _stopper_Pallet_Present; } }
        public bool PA3 { get { return _pA3; } }
        public bool ToolActive { get { return _toolActive; } }
        public bool PB1 { get { return _pB1; } }
        public bool PB2 { get { return _pB2; } }
        public bool E_Stop_Active { get { return _e_Stop_Active; }  }
        #endregion

        #region OutPuts
        bool _lamp_Grn;
        bool _lamp_Wht;
        bool _lamp_Red;
        bool _k3;
        bool _cyl_Stopper_Act;
        bool _cyl_Prestopper_Act;
        bool _k6;
        bool _reset_Safety;
        bool _cardOutputs;
        public bool Lamp_Grn { get { return _lamp_Grn; } set { _lamp_Grn = value; } }
        public bool Lamp_Wht { get { return _lamp_Wht; } set { _lamp_Wht = value; } }
        public bool Lamp_Red { get { return _lamp_Red; } set { _lamp_Red = value; } }
        public bool K3_1 { get { return _k3; } set { _k3 = value; } }
        public bool Cyl_Stopper_Act { get { return _cyl_Stopper_Act; } set { _cyl_Stopper_Act = value; } }
        public bool Cyl_Prestopper_Act { get { return _cyl_Prestopper_Act; } set { _cyl_Prestopper_Act = value; } }
        public bool K6_1 { get { return _k6; } set { _k6 = value; } }
        public bool Reset_Safety { get { return _reset_Safety; } set { _reset_Safety = value; } }        
        #endregion

        public void initialize()
        {
            NumSerial = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("ADU_SERIAL_1")).valueSetting;
        }
        //Este si se debe estar leyendo constantemente en un ciclo de lectura infinito hasta que se le envia
        //un token de cancelacion
        public async Task getDataInput(CancellationToken cancellationToken,IProgress<string> progressScrew)
        {
            int cont = 0;
            ADU ioADUCard = new ADU(NumSerial);
            while (!cancellationToken.IsCancellationRequested)
            {
                CardInputs = ioADUCard.MapADUInput();

                _pressure_Sensor = CardInputs[0];
                _prestoper_Pallet_Present = CardInputs[1];
                _stopper_Pallet_Present = CardInputs[2];
                _pA3 = CardInputs[3];
                _toolActive = CardInputs[4];
                _pB1 = CardInputs[5];
                _pB2 = CardInputs[6];
                _e_Stop_Active = CardInputs[7];


                string report = "Leyendo Sensores " + cont.ToString();
                Debug.WriteLine(report); // + cont.ToString());
                //viewMain.getStatusScrew("Leyendo Sensores veces:");// + cont.ToString());                
                progressScrew.Report(report);

                await Task.Delay(5); //Tiempo entre cada lectura 5mls
                cont++;
            }
        }
        //Cada clic en el boton deberia enviar la info dentro del evento clic
        public void sendDataOutput()
        {
            ADU ioADUCard = new ADU(NumSerial);
            
            CardOutputs[0] = _lamp_Grn;
            CardOutputs[1] = _lamp_Wht;
            CardOutputs[2] = _lamp_Red;
            CardOutputs[3] = _k3;
            CardOutputs[4] = _cyl_Stopper_Act;
            CardOutputs[5] = _cyl_Prestopper_Act;
            CardOutputs[6] = _k6;
            CardOutputs[7] = _reset_Safety;            

            ioADUCard.MapADUOutput(CardOutputs);
        }
    }
}

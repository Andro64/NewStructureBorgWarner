using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class IOCards
    {
        SessionApp sessionApp;
        Views.ViewMain viewMain;
        object _ioCard;

        private bool[] CardInputs;
        private bool[] CardOutputs;
        private string NumSerial;
        public IOCards(SessionApp _sessionApp, object ioCard, Views.ViewMain _viewMain)
        {
            sessionApp = _sessionApp;
            viewMain = _viewMain;
            initialize();
        }

        public IOCards(SessionApp _sessionApp, object ioCard)
        {
            sessionApp = _sessionApp;
            _ioCard = ioCard;
            initialize();
        }
        
        public void initialize()
        {
            if (_ioCard is IOCardType_M1)
            {
                NumSerial = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("ADU_SERIAL_1")).valueSetting;
                sessionApp.Sensors_M1 = new IOCardType_M1();
            }
            if (_ioCard is IOCardType_M2)
            {
                NumSerial = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("ADU_SERIAL_2")).valueSetting;
                sessionApp.Sensors_M2 = new IOCardType_M2();
            }
            if (_ioCard is IOCardType_M3)
            {
                NumSerial = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("ADU_SERIAL_3")).valueSetting;
                sessionApp.Sensors_M3 = new IOCardType_M3();
            }
            
        }

        private PropertyInfo[] GetPropertyInfos()
        {
            PropertyInfo[] properties = typeof(IOCardType_M1).GetProperties();
         
            if (_ioCard is IOCardType_M1)
            {
                properties = typeof(IOCardType_M1).GetProperties();
            }
            if (_ioCard is IOCardType_M2)
            {
                properties = typeof(IOCardType_M2).GetProperties();
            }
            if (_ioCard is IOCardType_M3)
            {
                properties = typeof(IOCardType_M3).GetProperties();
            }
            return properties;
        }
        //Este si se debe estar leyendo constantemente en un ciclo de lectura infinito hasta que se le envia
        //un token de cancelacion
        public async Task getDataInput(CancellationToken cancellationToken, IProgress<string> progressScrew)
        {
            int cont = 0;
            int numPropertiesByIn = 7;
            string report = string.Empty;
            ADU ioADUCard = new ADU(NumSerial);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                CardInputs = ioADUCard.MapADUInput();
                PropertyInfo[] properties = GetPropertyInfos();

                for (int i = 0; i <= numPropertiesByIn; i++)
                {
                    if(properties[i].PropertyType == typeof(bool))
                    {
                        if (_ioCard is IOCardType_M1)
                        {
                            properties[i].SetValue(sessionApp.Sensors_M1, CardInputs[i]);
                            report = "Leyendo Sensores ioCard1:  " + cont.ToString();
                        }
                        if (_ioCard is IOCardType_M2)
                        {
                            properties[i].SetValue(sessionApp.Sensors_M2, CardInputs[i]);
                            report = "Leyendo Sensores ioCard2:  " + cont.ToString();
                        }
                        if (_ioCard is IOCardType_M3)
                        {
                            properties[i].SetValue(sessionApp.Sensors_M3, CardInputs[i]);
                            report = "Leyendo Sensores ioCard3:  " + cont.ToString();
                        }
                    }
                }
                
                Debug.WriteLine($"{DateTime.Now} - "  + report);                 
                progressScrew.Report(report);
                cont++;
                await Task.Delay(5); //Tiempo entre cada lectura 5mls
                
            }
        }

        public void initializeSessionIOCard()
        {
            if (_ioCard is IOCardType_M1)
            {
                sessionApp.Sensors_M1 = new IOCardType_M1();
            }
            if (_ioCard is IOCardType_M2)
            {
                sessionApp.Sensors_M2 = new IOCardType_M2();
            }
            if (_ioCard is IOCardType_M3)
            {
                sessionApp.Sensors_M3 = new IOCardType_M3();
            }
        }
        public async Task getDataInput(CancellationToken cancellationToken)
        {
            int cont = 0;
            int numPropertiesByIn = 7;
            ADU ioADUCard = new ADU(NumSerial);

            initializeSessionIOCard();

            while (!cancellationToken.IsCancellationRequested)
            {
                CardInputs = ioADUCard.MapADUInput();

                PropertyInfo[] properties = GetPropertyInfos();

                for (int i = 0; i <= numPropertiesByIn; i++)
                {
                    if (properties[i].PropertyType == typeof(bool))
                    {
                        if (_ioCard is IOCardType_M1)
                        {
                            properties[i].SetValue(sessionApp.Sensors_M1, CardInputs[i]);
                            Debug.WriteLine($"{DateTime.Now} - "  + "Leyendo Sensores ioCard1: " + cont.ToString());
                        }
                        if (_ioCard is IOCardType_M2)
                        {
                            properties[i].SetValue(sessionApp.Sensors_M2, CardInputs[i]);
                            Debug.WriteLine($"{DateTime.Now} - "  + "Leyendo Sensores ioCard2: " + cont.ToString());
                        }
                        if (_ioCard is IOCardType_M3)
                        {
                            properties[i].SetValue(sessionApp.Sensors_M3, CardInputs[i]);
                            Debug.WriteLine($"{DateTime.Now} - "  + "Leyendo Sensores ioCard3: " + cont.ToString());
                        }                        
                    }
                }
                await Task.Delay(5); //Tiempo entre cada lectura 5mls
                cont++;
            }
        }
        //Cada clic en el boton deberia enviar la info dentro del evento clic
        public void sendDataOutput()
        {
            ADU ioADUCard = new ADU(NumSerial);
            int numPropertiesByOut = 8;
            PropertyInfo[] properties = typeof(IOCardType_M1).GetProperties();

            for (int i = numPropertiesByOut; i < properties.Length; i++)
            {                
                if (_ioCard is IOCardType_M1)
                {
                    CardOutputs[i] = (bool)properties[i].GetValue(sessionApp.Sensors_M1);
                }
                if (_ioCard is IOCardType_M2)
                {
                    CardOutputs[i] = (bool)properties[i].GetValue(sessionApp.Sensors_M2);
                }
                if (_ioCard is IOCardType_M3)
                {
                    CardOutputs[i] = (bool)properties[i].GetValue(sessionApp.Sensors_M3);
                }
            }

            ioADUCard.MapADUOutput(CardOutputs);
        }
    }
}

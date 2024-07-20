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
    public class IOCardsGeneric
    {
        SessionApp sessionApp;
        Views.ViewMain viewMain;
        object _ioCard;

        private bool[] CardInputs1;
        private bool[] CardInputs2;
        private bool[] CardInputs3;
        //private bool[] CardOutputs;
        private string NumSerial1;
        private string NumSerial2;
        private string NumSerial3;
        public IOCardsGeneric(SessionApp _sessionApp, object ioCard, Views.ViewMain _viewMain)
        {
            sessionApp = _sessionApp;
            viewMain = _viewMain;
            initialize();
        }

        public IOCardsGeneric(SessionApp _sessionApp, object ioCard)
        {
            sessionApp = _sessionApp;
            _ioCard = ioCard;
            initialize();
        }
        public IOCardsGeneric(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }

        public void initialize()
        {

            if (_ioCard is IOCardType_M1)
            {
                NumSerial1 = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("ADU_SERIAL_1")) != null ?
                    sessionApp.settings.FirstOrDefault(x => x.setting.Contains("ADU_SERIAL_1")).valueSetting : string.Empty;
            }
            if (_ioCard is IOCardType_M2)
            {
                NumSerial2 = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("ADU_SERIAL_2")) != null ?
                    sessionApp.settings.FirstOrDefault(x => x.setting.Contains("ADU_SERIAL_2")).valueSetting : string.Empty;
            }
            if (_ioCard is IOCardType_M3)
            {
                NumSerial3 = sessionApp.settings.FirstOrDefault(x => x.setting.Contains("ADU_SERIAL_3")) != null ?
                    sessionApp.settings.FirstOrDefault(x => x.setting.Contains("ADU_SERIAL_3")).valueSetting : string.Empty;
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
            ADU ioADUCard1 = new ADU(NumSerial1);
            ADU ioADUCard2 = new ADU(NumSerial2);
            ADU ioADUCard3 = new ADU(NumSerial3);

            while (!cancellationToken.IsCancellationRequested)
            {
                CardInputs1 = ioADUCard1.MapADUInput();
                CardInputs2 = ioADUCard2.MapADUInput();
                CardInputs3 = ioADUCard3.MapADUInput();
                PropertyInfo[] properties = GetPropertyInfos();

                for (int i = 0; i <= numPropertiesByIn; i++)
                {
                    if (properties[i].PropertyType == typeof(bool))
                    {
                        if (_ioCard is IOCardType_M1)
                        {
                            properties[i].SetValue(sessionApp.Sensors_M1, CardInputs1[i]);
                            report = "Leyendo Sensores ioCard1:  " + cont.ToString();
                        }
                        if (_ioCard is IOCardType_M2)
                        {
                            properties[i].SetValue(sessionApp.Sensors_M2, CardInputs2[i]);
                            report = "Leyendo Sensores ioCard2:  " + cont.ToString();
                        }
                        if (_ioCard is IOCardType_M3)
                        {
                            properties[i].SetValue(sessionApp.Sensors_M3, CardInputs3[i]);
                            report = "Leyendo Sensores ioCard3:  " + cont.ToString();
                        }
                    }
                }

                Debug.WriteLine($"{DateTime.Now} - " + report);
                progressScrew.Report(report);
                cont++;
                await Task.Delay(5); //Tiempo entre cada lectura 5mls

            }
        }

       

        public async Task getDataInput(CancellationToken cancellationToken)
        {

            string numSerial;
            bool[] ioADUCard;
            int idADU = 1;

            try
            {
                //while (!cancellationToken.IsCancellationRequested)
                //{

                List<ADU> lstADU = new List<ADU>();
                for (int numADU = 1; numADU <= sessionApp.amountADUPorts; numADU++)
                {
                    numSerial = sessionApp.settings.FirstOrDefault(x => x.setting.Contains($"ADU_SERIAL_{numADU}")) != null ?
                       sessionApp.settings.FirstOrDefault(x => x.setting.Contains($"ADU_SERIAL_{numADU}")).valueSetting : string.Empty;
                    lstADU.Add(new ADU(numSerial));
                }

                foreach (var item in lstADU)
                {
                    ioADUCard = item.MapADUInput();

                    for (int i = 0; i < ioADUCard.Length; i++)
                    {
                        sessionApp.ADUPorts.Where(x => x.id_ADU.Equals(idADU) && x.id_index.Equals(i)).First().Value = ioADUCard[i];

                    }
                    idADU++;
                }

                PropertyInfo[] propertiesIOSensors = typeof(IOSensors).GetProperties();
                //var valueProperty;
                for (int indexProperty = 0; indexProperty < propertiesIOSensors.Count(); indexProperty++)
                {
                    if (propertiesIOSensors[indexProperty].PropertyType == typeof(bool))
                    {
                        bool valueProperty = sessionApp.ADUPorts.Where(x => x.keySensor.Equals(propertiesIOSensors[indexProperty].Name))
                                                                .Select(z => (bool?)z.Value)
                                                                .FirstOrDefault() ?? false;
                        propertiesIOSensors[indexProperty].SetValue(sessionApp.IOSensorsGenerics, valueProperty);
                    }
                }

                //    await Task.Delay(5); //Tiempo entre cada lectura 5mls
                //}

                sessionApp.ADUPorts.Where(x=>x.keySensor.Equals("Opto_Grn")).First().Value = true;
                sendDataOutput("Opto_Grn");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + "Error:" + ex.Message);
            }





            //int cont = 0;
            //int numPropertiesByIn = 7;
            //ADU ioADUCard1 = new ADU(NumSerial1);
            //ADU ioADUCard2 = new ADU(NumSerial2);
            //ADU ioADUCard3 = new ADU(NumSerial3);

            //while (!cancellationToken.IsCancellationRequested)
            //{
            //    CardInputs1 = ioADUCard1.MapADUInput();
            //    CardInputs2 = ioADUCard2.MapADUInput();
            //    CardInputs3 = ioADUCard3.MapADUInput();

            //    PropertyInfo[] properties = GetPropertyInfos();

            //    for (int i = 0; i <= numPropertiesByIn; i++)
            //    {
            //        if (properties[i].PropertyType == typeof(bool))
            //        {
            //            if (_ioCard is IOCardType_M1)
            //            {
            //                properties[i].SetValue(sessionApp.Sensors_M1, CardInputs1[i]);
            //                //Debug.WriteLine($"{DateTime.Now} - "  + "Leyendo Sensores ioCard1: " + cont.ToString());
            //            }
            //            if (_ioCard is IOCardType_M2)
            //            {
            //                properties[i].SetValue(sessionApp.Sensors_M2, CardInputs2[i]);
            //                //Debug.WriteLine($"{DateTime.Now} - "  + "Leyendo Sensores ioCard2: " + cont.ToString());
            //            }
            //            if (_ioCard is IOCardType_M3)
            //            {
            //                properties[i].SetValue(sessionApp.Sensors_M3, CardInputs3[i]);
            //                //Debug.WriteLine($"{DateTime.Now} - "  + "Leyendo Sensores ioCard3: " + cont.ToString());
            //            }
            //        }
            //    }
            //    await Task.Delay(5); //Tiempo entre cada lectura 5mls
            //    cont++;
            //}
        }

        public List<string> getInputSensors()
        {
            List<string> lstCardOutputs = new List<string>();
            for (int numADU = 0; numADU <= sessionApp.amountADUPorts; numADU++)
            {
                var ADUPortsElements = sessionApp.ADUPorts.Where(x => x.id_ADU.Equals(numADU) && x.IOCard.Contains("Output"));
                foreach (var ADUElement in ADUPortsElements)
                {
                    lstCardOutputs.Add($"e{ADUElement.IOCard}");
                }
            }
            return lstCardOutputs;
        }
        private void CleanOutputsSession()
        {
            var ADUPortsElements = sessionApp.ADUPorts.Where(x => x.IOCard.Contains("Output")).OrderBy(z => z.id_index);
            foreach (var ADUPort in ADUPortsElements)
            {
                ADUPort.Value = false;
            }
        }
        public void sendDataOutput(string keySensor)
        {
            try
            {
                string numSerial;
                bool[] CardOutputs = new bool[8];
                ADU ADUOutput;
                int index = 0;

                int numADU = sessionApp.ADUPorts.Where(x => x.keySensor.Equals(keySensor)).First().id_ADU;
                numSerial = sessionApp.settings.FirstOrDefault(x => x.setting.Contains($"ADU_SERIAL_{numADU}")).valueSetting;
                ADUOutput = new ADU(numSerial);

                var ADUPortsElements = sessionApp.ADUPorts.Where(x => x.id_ADU.Equals(numADU) && x.IOCard.Contains("Output")).OrderBy(z => z.id_index);
                foreach (var ADUElement in ADUPortsElements)
                {
                    CardOutputs[index] = ADUElement.Value;
                    index++;
                }

                //ADUOutput.MapADUOutput(CardOutputs);
                Debug.Write(CardOutputs);
                CleanOutputsSession();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + "Error:" + ex.Message);
            }



            //ADU ioADUCard1 = new ADU(NumSerial1);
            //ADU ioADUCard2 = new ADU(NumSerial2);
            //ADU ioADUCard3 = new ADU(NumSerial3);
            ////bool[] CardOutputs = new bool[8];

            //try
            //{

            //    if (_ioCard is IOCardType_M1)
            //    {
            //        CardOutputs = matchCardOutput(typeof(IOCardType_M1), sessionApp.Sensors_M1);
            //        ioADUCard1.MapADUOutput(CardOutputs);
            //    }
            //    if (_ioCard is IOCardType_M2)
            //    {
            //        CardOutputs = matchCardOutput(typeof(IOCardType_M2), sessionApp.Sensors_M2);
            //        ioADUCard2.MapADUOutput(CardOutputs);

            //    }
            //    if (_ioCard is IOCardType_M3)
            //    {
            //        CardOutputs = matchCardOutput(typeof(IOCardType_M3), sessionApp.Sensors_M3);
            //        ioADUCard3.MapADUOutput(CardOutputs);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine($"{DateTime.Now} - " + "Error:" + ex.Message);
            //}
        }

        public bool[] matchCardOutput(Type IOCardType, object Sensor)
        {
            bool[] CardOutputs = new bool[8];
            try
            {
                int numPropertiesByOut = 8;
                PropertyInfo[] properties = IOCardType.GetProperties();
                for (int i = numPropertiesByOut; i < properties.Length; i++)
                {
                    CardOutputs[i - numPropertiesByOut] = (bool)properties[i].GetValue(Sensor);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + "Error:" + ex.Message);
            }
            return CardOutputs;
        }
    }
}

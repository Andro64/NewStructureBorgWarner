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

        public IOCardsGeneric(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }

        public async Task GetDataInput(CancellationToken cancellationToken)
        {

            string numSerial;
            //bool[] ioADUCard;
            int idADU = 1;
            const int limitIndexReadADU = 7;

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    idADU = 1;
                    //ioADUCard = new bool[0];
                    List<ADU> lstADU = new List<ADU>();
                    for (int numADU = 1; numADU <= sessionApp.amountADUPorts; numADU++)
                    {
                        numSerial = sessionApp.settings.FirstOrDefault(x => x.setting.Contains($"ADU_SERIAL_{numADU}")) != null ?
                           sessionApp.settings.FirstOrDefault(x => x.setting.Contains($"ADU_SERIAL_{numADU}")).valueSetting : string.Empty;
                        lstADU.Add(new ADU(numSerial));
                    }

                    foreach (var item in lstADU)
                    {
                        //ioADUCard = new bool[];
                        bool[] ioADUCard;
                        ioADUCard = item.MapADUInput();

                        //for (int i = 0; i < ioADUCard.Length; i++)
                        for (int i = 0; i < limitIndexReadADU; i++)
                        {
                            //Debug.WriteLine($"{DateTime.Now} - " + $"ioADUCard.Length {ioADUCard.Length}   i {i}    idADU {idADU}");
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

                    await Task.Delay(5); //Tiempo entre cada lectura 5mls
                    //Debug.WriteLine($"{DateTime.Now} - " + $"Estoy leyendo los sensores {idADU}");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + $"Error #1:{idADU}" + ex.Message);
            }
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

                Debug.WriteLine($"{DateTime.Now} - " + $"Key Sensor {keySensor}");
                int numADU = sessionApp.ADUPorts.Where(x => x.keySensor.Equals(keySensor)).First().id_ADU;
                numSerial = sessionApp.settings.FirstOrDefault(x => x.setting.Contains($"ADU_SERIAL_{numADU}")).valueSetting;
                ADUOutput = new ADU(numSerial);
                Debug.WriteLine($"{DateTime.Now} - " + $"Numero de ADU {numADU} con serial {numSerial}");

                var ADUPortsElements = sessionApp.ADUPorts.Where(x => x.id_ADU.Equals(numADU) && x.IOCard.Contains("Output")).OrderBy(z => z.id_index);
                foreach (var ADUElement in ADUPortsElements)
                {
                    CardOutputs[index] = ADUElement.Value;
                    index++;
                    Debug.WriteLine($"{DateTime.Now} - " + $"ADU Posicion {index} = valor {ADUElement.Value}");
                }

                ADUOutput.MapADUOutput(CardOutputs);
                CleanOutputsSession();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + "Error: #2" + ex.Message);
            }
        }
        public void sendDataOutputSameADU(List<keySensorValue> keySensorvalue)
        {
            try
            {
                string numSerial;
                bool[] CardOutputs = new bool[8];
                ADU ADUOutput;
                int index = 0;

                int numADU = sessionApp.ADUPorts.Where(x => x.keySensor.Equals(keySensorvalue.First().keySensor)).First().id_ADU;
                numSerial = sessionApp.settings.FirstOrDefault(x => x.setting.Contains($"ADU_SERIAL_{numADU}")).valueSetting;
                ADUOutput = new ADU(numSerial);

                var ADUPortsElements = sessionApp.ADUPorts.Where(x => x.id_ADU.Equals(numADU) && x.IOCard.Contains("Output")).OrderBy(z => z.id_index);
                foreach (var ADUElement in ADUPortsElements)
                {
                    var sensorValue = keySensorvalue.FirstOrDefault(x => x.keySensor.Equals(ADUElement.keySensor));

                    if (sensorValue != null)
                    {
                        CardOutputs[index] = sensorValue.value;
                    }
                    else
                    {
                        CardOutputs[index] = false;
                    }

                    index++;
                }

                ADUOutput.MapADUOutput(CardOutputs);
                //Debug.Write(CardOutputs);
                CleanOutputsSession();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + "Error: #3" + ex.Message);
            }
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
                Debug.WriteLine($"{DateTime.Now} - " + "Error: #4" + ex.Message);
            }
            return CardOutputs;
        }
    }
}

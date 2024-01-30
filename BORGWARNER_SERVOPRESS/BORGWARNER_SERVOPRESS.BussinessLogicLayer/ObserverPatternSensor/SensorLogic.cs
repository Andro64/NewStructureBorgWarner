using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class SensorLogic
    {
        private SensorSubject sensorSubject = new SensorSubject();
        private Dictionary<string, string> sensorData = new Dictionary<string, string>();

        public SensorLogic()
        {
            // Inicializa los estados de los sensores
            for (int i = 1; i <= 5; i++)
            {
                string sensorName = $"Sensor {i}";
                sensorData.Add(sensorName, $"Estado inicial: {DateTime.Now}");
            }
        }

        public void ToggleSensorStatus(string sensorName)
        {
            // Cambia el estado del sensor
            if (sensorData.ContainsKey(sensorName))
            {
                sensorData[sensorName] = $"Nuevo estado: {DateTime.Now}";
                // Notifica a los observadores sobre el cambio en el sensor
                sensorSubject.NotifyStatusChange(sensorName, sensorData[sensorName]);
            }
        }

        public string GetSensorStatus(string sensorName)
        {
            // Obtiene el estado actual del sensor
            return sensorData.ContainsKey(sensorName) ? sensorData[sensorName] : "Sensor no encontrado";
        }

        public void Attach(ISensorObserver observer)
        {
            sensorSubject.StatusChanged += (sender, args) => observer.UpdateStatus(args.Item1, args.Item2);
        }

        public void Detach(ISensorObserver observer)
        {
            sensorSubject.StatusChanged -= (sender, args) => observer.UpdateStatus(args.Item1, args.Item2);
        }

        public void SimulateSensorChange()
        {
            // Simula obtener el nuevo estado de los sensores
            Dictionary<string, string> newSensorData = GetNewSensorStatus();

            // Notifica a los observadores sobre el cambio en cada sensor
            foreach (var sensor in newSensorData)
            {
                sensorSubject.NotifyStatusChange(sensor.Key, sensor.Value);
            }
        }

        private Dictionary<string, string> GetNewSensorStatus()
        {
            // Simula obtener el nuevo estado de los sensores
            Dictionary<string, string> newSensorData = new Dictionary<string, string>();

            for (int i = 1; i <= 5; i++)
            {
                string sensorName = $"Sensor {i}";
                newSensorData.Add(sensorName, $"Nuevo estado: {DateTime.Now}");
            }

            return newSensorData;
        }
    }
}


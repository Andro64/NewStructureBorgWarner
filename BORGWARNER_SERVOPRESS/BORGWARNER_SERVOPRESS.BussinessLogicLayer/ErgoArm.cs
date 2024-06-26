using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class ErgoArm
    {
        SessionApp sessionApp;
        TcpClient tcpClient;

        CancellationTokenSource cancellationToken_ErgoArm;
        CommunicationErgoArm communicationErgoArm;
        DataPosition_ErgoArm home_ErgoArm;
        DataPosition_ErgoArm nozzle_ErgoArm;

        public ErgoArm(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            tcpClient = new TcpClient();
            communicationErgoArm = new CommunicationErgoArm(sessionApp);
            home_ErgoArm = SetPositionHomeErgoArm("ErgoArm_HomeEncoder1", "ErgoArm_HomeEncoder2", "ErgoArm_HomeTolerancia");
            nozzle_ErgoArm = SetPositionHomeErgoArm("ErgoArm_NozzleEncoder1", "ErgoArm_NozzleEncoder2", "ErgoArm_NozzleTolerancia");
        }
        public DataPosition_ErgoArm SetPositionHomeErgoArm(string settingEncoder1, string settingEncoder2, string settingTolerance)
        {
            DataPosition_ErgoArm data_position_ErgoArm = new DataPosition_ErgoArm()
            {
                Encoder1 = double.Parse(sessionApp.settings.FirstOrDefault(x => x.setting.Equals(settingEncoder1)).valueSetting),
                Encoder2 = double.Parse(sessionApp.settings.FirstOrDefault(x => x.setting.Equals(settingEncoder2)).valueSetting),
                Tolerance = double.Parse(sessionApp.settings.FirstOrDefault(x => x.setting.Equals(settingTolerance)).valueSetting)
            };
            data_position_ErgoArm.Encoder1_Max = data_position_ErgoArm.Encoder1 + data_position_ErgoArm.Tolerance;
            data_position_ErgoArm.Encoder1_Min = data_position_ErgoArm.Encoder1 - data_position_ErgoArm.Tolerance;
            data_position_ErgoArm.Encoder2_Max = data_position_ErgoArm.Encoder2 + data_position_ErgoArm.Tolerance;
            data_position_ErgoArm.Encoder2_Min = data_position_ErgoArm.Encoder2 - data_position_ErgoArm.Tolerance;
            return data_position_ErgoArm;
        }
        public void Connect()
        {
            communicationErgoArm.Connect();
        }
        public bool isConected()
        {
           return communicationErgoArm.isConnect();
        }
        public void startReadPositionRespectScrew(Screw screw)
        {
            cancellationToken_ErgoArm = new CancellationTokenSource();
            Task.Run(async () =>
            {
                communicationErgoArm.getDataPositionScrew(cancellationToken_ErgoArm.Token, screw);
            }).Wait();
        }
        public void startReadPosition(Screw screw)
        {
            cancellationToken_ErgoArm = new CancellationTokenSource();
            Task.Run(async () =>
            {
                communicationErgoArm.getPositionErgoArm(cancellationToken_ErgoArm.Token, screw);
            }).Wait();
        }
        public async Task WaitiningMonitorPositionScrew_and_Mask(Screw screw)
        {
            cancellationToken_ErgoArm = new CancellationTokenSource();

            while (!cancellationToken_ErgoArm.IsCancellationRequested)
            {
                startReadPositionRespectScrew(screw);
                if (sessionApp.positionErgoArm.InPositionReadyToProcess)
                {
                    sessionApp.MessageOfProcess = $"Por favor, realice el atornillado número: {screw.id_screw}";
                    Debug.WriteLine("La función principal se ha reanudado por que esta en posicion del tornillo");
                }
                else
                {
                    sessionApp.MessageOfProcess = "Por favor, posicione el brazo ergonomico en el tornillo.";
                    //await Task.Delay(500);
                    Debug.WriteLine("Por favor, posiciones el brazo ergonomico del tornillo.");
                    while (!sessionApp.positionErgoArm.InPositionReadyToProcess)
                    {
                        Task.Run(async () =>
                        {
                            await Task.Delay(500);
                        }).Wait();
                        startReadPositionRespectScrew(screw);
                    }
                    

                }
                if (sessionApp.Sensors_M2.MaskatHousing)
                {
                    sessionApp.MessageOfProcess = $"Por favor, realice el atornillado ";
                    Debug.WriteLine("La función principal se ha reanudado por que tiene la mascara");
                }
                else
                {
                    sessionApp.MessageOfProcess = "Por favor, vuelva a colocar la máscara sobre el housing.";
                    //await Task.Delay(500);
                    Debug.WriteLine("La función principal se ha detenido por que no tiene la mascara");
                    while (!sessionApp.Sensors_M2.MaskatHousing)
                    {
                        Task.Run(async () =>
                        {
                            await Task.Delay(500);
                        }).Wait();
                    }
                }

            }
        }
        public void startReadPosition()
        {
            cancellationToken_ErgoArm = new CancellationTokenSource();
            Task.Run(async () =>
            {
                communicationErgoArm.getDataPosition(cancellationToken_ErgoArm.Token);
            });
        }
        public void endReadPostion()
        {
            cancellationToken_ErgoArm.Cancel();
            communicationErgoArm.Disconnect();
            Debug.WriteLine($"{DateTime.Now} - "  + "Termine de leer la posicion del ErgoArm");
        }
        public bool isInHome()
        {
            startReadPosition();
            if ((sessionApp.positionErgoArm.encoder1 > home_ErgoArm.Encoder1_Min) && (sessionApp.positionErgoArm.encoder1 < home_ErgoArm.Encoder1_Max) && (sessionApp.positionErgoArm.encoder2 > home_ErgoArm.Encoder2_Min) && (sessionApp.positionErgoArm.encoder2 < home_ErgoArm.Encoder2_Max))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> isInHomeAsync()
        {
            // Simula una espera asíncrona para emular el comportamiento de verificar las condiciones.
            await Task.Delay(5);

            // Verifica las condiciones después de la espera.
            while(!((sessionApp.positionErgoArm.encoder1 > home_ErgoArm.Encoder1_Min) &&
                (sessionApp.positionErgoArm.encoder1 < home_ErgoArm.Encoder1_Max) &&
                (sessionApp.positionErgoArm.encoder2 > home_ErgoArm.Encoder2_Min) &&
                (sessionApp.positionErgoArm.encoder2 < home_ErgoArm.Encoder2_Max)))
            {
                //Task.Run(async () =>
                //{
                    await Task.Delay(5);
                //}).Wait();
            }

            return true; // Alguna de las condiciones no se cumple.
        }
        public bool isInVacuumNozzle()
        {
            startReadPosition();
            if ((sessionApp.positionErgoArm.encoder1 > nozzle_ErgoArm.Encoder1_Min) && (sessionApp.positionErgoArm.encoder1 < nozzle_ErgoArm.Encoder1_Max) && (sessionApp.positionErgoArm.encoder2 > nozzle_ErgoArm.Encoder2_Min) && (sessionApp.positionErgoArm.encoder2 < nozzle_ErgoArm.Encoder2_Max))
            {
                return true;
            }
            return false;
        }
        public async Task<bool> isInVacuumNozzleAsync()
        {
            // Verifica las condiciones después de la espera.
            while (!((sessionApp.positionErgoArm.encoder1 > nozzle_ErgoArm.Encoder1_Min) &&
                (sessionApp.positionErgoArm.encoder1 < nozzle_ErgoArm.Encoder1_Max) &&
                (sessionApp.positionErgoArm.encoder2 > nozzle_ErgoArm.Encoder2_Min) &&
                (sessionApp.positionErgoArm.encoder2 < nozzle_ErgoArm.Encoder2_Max)))
            {
                //Task.Run(async () =>
                //{
                    await Task.Delay(5);
                //}).Wait();
            }

            return true; 
        }
        public void WaitingResponse(bool sensorToCheck)
        {
            while (!sensorToCheck)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(5);
                }).Wait();
            }
        }
    }
}

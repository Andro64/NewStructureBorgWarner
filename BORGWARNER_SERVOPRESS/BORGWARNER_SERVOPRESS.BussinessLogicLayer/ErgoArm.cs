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
                communicationErgoArm.getDataPosition(cancellationToken_ErgoArm.Token, screw);
            }).Wait();
        }
        public void startReadPositionRespectScrew()
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
            //if ((sessionApp.positionErgoArm.encoder1 > 105) && (sessionApp.positionErgoArm.encoder1 < 120) && (sessionApp.positionErgoArm.encoder2 > -40) && (sessionApp.positionErgoArm.encoder2 < -30))
            if ((sessionApp.positionErgoArm.encoder1 > home_ErgoArm.Encoder1_Min) && (sessionApp.positionErgoArm.encoder1 < home_ErgoArm.Encoder1_Max) && (sessionApp.positionErgoArm.encoder2 > home_ErgoArm.Encoder2_Min) && (sessionApp.positionErgoArm.encoder2 < home_ErgoArm.Encoder2_Max))
            {
                return true;
            }
            return false;
        }
        public bool isInVacuumNozzle()
        {
            if ((sessionApp.positionErgoArm.encoder1 > nozzle_ErgoArm.Encoder1_Min) && (sessionApp.positionErgoArm.encoder1 < nozzle_ErgoArm.Encoder1_Max) && (sessionApp.positionErgoArm.encoder2 > nozzle_ErgoArm.Encoder2_Min) && (sessionApp.positionErgoArm.encoder2 < nozzle_ErgoArm.Encoder2_Max))
            {
                return true;
            }
            return false;
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

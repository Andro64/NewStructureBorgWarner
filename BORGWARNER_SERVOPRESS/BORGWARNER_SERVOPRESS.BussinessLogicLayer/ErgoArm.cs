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

        public ErgoArm(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            tcpClient = new TcpClient();
            communicationErgoArm = new CommunicationErgoArm(sessionApp);
        }
        public void startReadPositionRespectScrew(Screw screw)
        {
            cancellationToken_ErgoArm = new CancellationTokenSource();
            Task.Run(async () =>
            {
                communicationErgoArm.getDataPosition(cancellationToken_ErgoArm.Token, screw);
            }).Wait();
        }
        public void endReadPostion()
        {
            cancellationToken_ErgoArm.Cancel();
            communicationErgoArm.Disconnect();
            Debug.WriteLine("Termine de leer la posicion del ErgoArm");
        }
        public bool isInHome()
        {
            if ((sessionApp.positionErgoArm.encoder1 > 105) && (sessionApp.positionErgoArm.encoder1 < 120) && (sessionApp.positionErgoArm.encoder2 > -40) && (sessionApp.positionErgoArm.encoder2 < -30))
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

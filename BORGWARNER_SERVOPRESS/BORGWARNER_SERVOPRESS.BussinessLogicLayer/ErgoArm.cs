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
            Debug.WriteLine("Termine de leer los sensores ioCard1");
        }       
    }
}

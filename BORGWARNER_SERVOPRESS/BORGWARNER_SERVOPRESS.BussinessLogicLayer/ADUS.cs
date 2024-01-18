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
    public class ADUS
    {
        SessionApp sessionApp;
        IOCard_Type1 ioCard_Type1;
        CancellationTokenSource cancellationToken_ioCard1;
        public ADUS(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }


        public async Task startReadADUS(IProgress<string> progressScrew)
        {
            ioCard_Type1 = new IOCard_Type1(sessionApp);
            cancellationToken_ioCard1 = new CancellationTokenSource();
            progressScrew.Report("Inicia lectura de los sensores");
            Debug.WriteLine("Inicia lectura de los sensores");
            Task sensorTask = ioCard_Type1.getDataInput(cancellationToken_ioCard1.Token, progressScrew);
        }

        public void endReadADUS()
        {
            cancellationToken_ioCard1.Cancel();
            Debug.WriteLine("Termine de leer los sensores");
        }
        public void sendDataADUS()
        {
            ioCard_Type1.K3_1 = true;
            ioCard_Type1.sendDataOutput();
        }
    }
}

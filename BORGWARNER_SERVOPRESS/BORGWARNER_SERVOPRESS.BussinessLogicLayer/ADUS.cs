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
        IOCard_Type2 ioCard_Type2;
        IOCard_Type3 ioCard_Type3;
        CancellationTokenSource cancellationToken_ioCard1;
        CancellationTokenSource cancellationToken_ioCard2;
        CancellationTokenSource cancellationToken_ioCard3;
        public ADUS(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }


        public async Task startReadADUS(IProgress<string> progressScrew)
        {
            ioCard_Type1 = new IOCard_Type1(sessionApp);
            cancellationToken_ioCard1 = new CancellationTokenSource();
            await ioCard_Type1.getDataInput(cancellationToken_ioCard1.Token, progressScrew);

            ioCard_Type2 = new IOCard_Type2(sessionApp);
            cancellationToken_ioCard1 = new CancellationTokenSource();
            await ioCard_Type2.getDataInput(cancellationToken_ioCard2.Token);

            ioCard_Type3 = new IOCard_Type3(sessionApp);
            cancellationToken_ioCard1 = new CancellationTokenSource();
            await ioCard_Type3.getDataInput(cancellationToken_ioCard3.Token);

            progressScrew.Report("Inicia lectura de los sensores");
            Debug.WriteLine("Inicia lectura de los sensores");
        }

        public void endReadADUS()
        {
            cancellationToken_ioCard1.Cancel();
            cancellationToken_ioCard2.Cancel();
            cancellationToken_ioCard2.Cancel();

            Debug.WriteLine("Termine de leer los sensores");
        }
        public void sendDataADUS()
        {
            ioCard_Type1.K3_1 = true;
            ioCard_Type1.sendDataOutput();
        }
    }
}

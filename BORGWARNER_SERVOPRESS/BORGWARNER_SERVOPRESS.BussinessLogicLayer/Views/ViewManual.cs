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
    public class ViewManual
    {
        SessionApp sessionApp;
        ADUS adus;
      
        public ViewManual(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            adus = new ADUS(sessionApp);
        }
        public async Task startReadADUS(IProgress<string> progressScrew)
        {
            await adus.startReadADUS(progressScrew);
        }

        public void endReadADUS()
        {
            adus.endReadADUS();
        }
        public void sendDataADUS()
        {
            
        }

    }
}

using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public class ViewFIS
    {
        private ModelViewMain _modelViewMain;
        SessionApp sessionApp;

        public ViewFIS(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            _modelViewMain = new ModelViewMain();
        }
        public ModelViewMain GetModel()
        {
            return _modelViewMain;
        }
        public void ShowMessage()
        {
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, args) =>
            {
                _modelViewMain.Timestamp = DateTime.Now.ToString();
                _modelViewMain.MessageProcess = sessionApp.MessageOfProcess;
                _modelViewMain.ImageOfProcess = sessionApp.ImageOfProcess;
            };
            timer.Start();
        }
        public async Task getStatusScrew(string messageScrew)
        {
            _modelViewMain.MessageProcess = messageScrew;
        }
        public void ShowData()
        {
            _modelViewMain.UserName = sessionApp.user.userName;
            _modelViewMain.Profile = sessionApp.user.profile_description;
            _modelViewMain.NameWorksation = sessionApp.typeWorkstation.description;
        }

       

      
    }
}

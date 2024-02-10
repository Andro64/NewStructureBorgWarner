using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public class ViewMain
    {
        private ModelViewMain _modelViewMain;
        SessionApp sessionApp;
        private DispatcherTimer timer;
       
        public ViewMain(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            _modelViewMain = new ModelViewMain();

            timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            timer.Interval = TimeSpan.FromMilliseconds(1); // Actualizar cada milisegundo
            timer.Tick += Timer_Tick;
        }
        public ModelViewMain GetModel()
        {
            return _modelViewMain;
        }
        public void ShowDate()
        {
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += (sender, args) =>
            {
                _modelViewMain.Timestamp = DateTime.Now.ToString();
                //_modelViewMain.NumeroAleatorio = new Random().Next(1, 1000000);
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
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //_modelViewMain.Milliseconds++;

            // Actualiza los segundos y las décimas de segundo
            //_modelViewMain.Seconds += 0.01;
            //if (_modelViewMain.Seconds >= 60)
            //{
            //    _modelViewMain.Seconds = 0;
            //}
            _modelViewMain.Seconds++;
        }

        public void StartTimer()
        {
            ResetTimer();
            timer.Start();
        }

        public void StopTimer()
        {            
            timer.Stop();            
        }
        public void ResetTimer()
        {
            _modelViewMain.Milliseconds = 0;
            _modelViewMain.Seconds = 0;
        }

    }
}

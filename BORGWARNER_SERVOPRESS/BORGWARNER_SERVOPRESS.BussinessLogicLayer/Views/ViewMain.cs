using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public class ViewMain
    {
        private ModelViewMain _modelViewMain;
        SessionApp sessionApp;        
       
        public ViewMain(SessionApp _sessionApp)
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
                _modelViewMain.QRs_Scanned = sessionApp.QR;
                _modelViewMain.ImageOfProcess = sessionApp.ImageOfProcess;
                if (sessionApp.ImageOfProcess != null)
                {
                    if (File.Exists(sessionApp.ImageOfProcess))
                    {
                        if (sessionApp.ImageOfProcess.Contains(".svg"))
                        {
                            _modelViewMain.BitMapImageOfProcess = new ImageProcess().TransformSVGtoPNG(sessionApp.ImageOfProcess);
                        }
                        else
                        {                            
                            _modelViewMain.BitMapImageOfProcess = new BitmapImage(new Uri(sessionApp.ImageOfProcess));                            
                        }
                    }

                }
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

        public (Point position, string text) GetTextData()
        {
            // Lógica para determinar la posición y el texto
            Point position = new Point(100, 100);
            string text = "Texto desde capa de negocio";

            return (position, text);
        }
        
    }
}

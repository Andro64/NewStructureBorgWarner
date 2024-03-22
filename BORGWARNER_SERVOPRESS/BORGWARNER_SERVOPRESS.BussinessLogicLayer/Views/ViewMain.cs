using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using System;
using System.Collections.Generic;
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
        string namefileLast;
       
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
                if (sessionApp.QR != null)
                {
                    _modelViewMain.QRs_Scanned = sessionApp.QR;
                    _modelViewMain.HOUSING = sessionApp.QR.HOUSING;
                    _modelViewMain.HVDC_BUSBAR = sessionApp.QR.HVDC_BUSBAR;
                    _modelViewMain.HARNESS = sessionApp.QR.HARNESS;
                    _modelViewMain.TOP_COVER = sessionApp.QR.TOP_COVER;
                }
                _modelViewMain.ImageOfProcess = sessionApp.ImageOfProcess;
                if (sessionApp.ImageOfProcess != null)
                {
                    if (File.Exists(sessionApp.ImageOfProcess))
                    {
                        if (namefileLast != sessionApp.ImageOfProcess)
                        {
                            if (sessionApp.ImageOfProcess.Contains(".svg"))
                            {
                                if (!isFileInUse(sessionApp.ImageOfProcess))
                                {
                                    _modelViewMain.BitMapImageOfProcess = new ImageProcess().TransformSVGtoPNG(sessionApp.ImageOfProcess);                                    
                                }
                                namefileLast = sessionApp.ImageOfProcess;
                            }
                            else
                            {
                                if (!isFileInUse(sessionApp.ImageOfProcess))
                                {
                                    _modelViewMain.BitMapImageOfProcess = new BitmapImage(new Uri(sessionApp.ImageOfProcess));                                    
                                }
                                namefileLast = sessionApp.ImageOfProcess;
                            }
                        }
                    }

                }
                if(sessionApp.areImagePASSProcessFinished)
                {
                    _modelViewMain.BitMapImageOfProcess = new ImageProcess().CombineImages(sessionApp.images);
                }
            };
            timer.Start();
        }
        
        private bool isFileInUse(string  filePath)
        {
            try
            {
                using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {                    
                    return false;
                }
            }
            catch (IOException)
            {                
                return true;
            }
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

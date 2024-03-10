using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using SkiaSharp;
using Svg.Skia;
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
                _modelViewMain.ImageOfProcess = sessionApp.ImageOfProcess;
                if (sessionApp.ImageOfProcess != null)
                {
                    if (sessionApp.ImageOfProcess.Contains(".svg"))
                    {
                        _modelViewMain.BitMapImageOfProcess = TransformSVGtoPNG(sessionApp.ImageOfProcess);
                    }
                    else
                    {
                        if (File.Exists(sessionApp.ImageOfProcess))
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
        private BitmapImage TransformSVGtoPNG(string svgFilePath)
        {
            // Load SVG file
            using (var svg = new SKSvg())
            {
                svg.Load(svgFilePath);

                // Create a SKBitmap with the desired dimensions
                var width = (int)svg.Picture.CullRect.Width;
                var height = (int)svg.Picture.CullRect.Height;
                var bitmap = new SKBitmap(width, height);

                // Render the SVG to the SKBitmap
                using (var canvas = new SKCanvas(bitmap))
                {
                    canvas.Clear(SKColors.Transparent);
                    canvas.DrawPicture(svg.Picture);
                }

                // Convert SKBitmap to BitmapImage
                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    var stream = new MemoryStream();
                    data.SaveTo(stream);

                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();

                    return bitmapImage;
                }
            }
        }
    }
}

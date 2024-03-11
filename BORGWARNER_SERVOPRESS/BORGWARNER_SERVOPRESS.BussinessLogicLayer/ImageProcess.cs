using SkiaSharp;
using Svg.Skia;
using System.IO;
using System.Windows.Media.Imaging;


namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class ImageProcess
    {
        public BitmapImage TransformSVGtoPNG(string svgFilePath)
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

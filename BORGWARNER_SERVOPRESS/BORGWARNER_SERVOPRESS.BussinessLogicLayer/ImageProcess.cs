using SkiaSharp;
using Svg.Skia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
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
        public BitmapImage CombineImages(List<string> PathImages)
        {
            List<BitmapImage> images = new List<BitmapImage>();
            foreach (string img in PathImages)
            {
                images.Add(new BitmapImage(new Uri(img)));
            }
            return CombineImages(images);
        }

        public BitmapImage CombineImages(List<BitmapImage> images)
        {            
            // Calcular el ancho total y la altura máxima de las imágenes
            int totalWidth = 0;
            int maxHeight = 0;
            foreach (BitmapImage img in images)
            {
                totalWidth += img.PixelWidth;
                maxHeight = Math.Max(maxHeight, img.PixelHeight);
            }

            // Crear un nuevo WriteableBitmap con el tamaño suficiente para contener todas las imágenes
            WriteableBitmap combinedBitmap = new WriteableBitmap(totalWidth, maxHeight, 96, 96, PixelFormats.Pbgra32, null);

            // Dibujar las imágenes en el nuevo WriteableBitmap
            int x = 0;
            foreach (BitmapImage img in images)
            {
                int stride = img.PixelWidth * 4;
                int size = img.PixelHeight * stride;
                byte[] pixels = new byte[size];
                img.CopyPixels(pixels, stride, 0);

                combinedBitmap.WritePixels(new System.Windows.Int32Rect(x, 0, img.PixelWidth, img.PixelHeight), pixels, stride, 0);

                x += img.PixelWidth;
            }

            // Convertir el WriteableBitmap a BitmapImage
            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(combinedBitmap));
            encoder.Save(stream);

            BitmapImage result = new BitmapImage();
            result.BeginInit();
            result.CacheOption = BitmapCacheOption.OnLoad;
            result.StreamSource = stream;
            result.EndInit();

            return result;
        }
    }
}

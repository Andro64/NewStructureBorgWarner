﻿using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using SkiaSharp;
using Svg;
using Svg.Skia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class VisionSystem
    {
        SessionApp sessionApp;
        Camara camara;
        TCP_IP TCPcamara;
        eTypeConnection typeCamera;
        CommandCamara commands;
        DateTime readingTime;

        public VisionSystem(SessionApp _sessionApp, eTypeConnection _typeCamera)
        {
            sessionApp = _sessionApp;
            typeCamera = _typeCamera;
            commands = new CommandCamara();
            Initialize();
        }
        public void Initialize()
        {
            try
            {
                commands = sessionApp.commandCamaras.FirstOrDefault(x => x.id_type_connection.Equals((int)typeCamera));
                camara = new Camara()
                {
                    IP = commands.ip,
                    Port = commands.port
                };

                TCPcamara = new TCP_IP(camara.IP, camara.Port);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
            }
        }
        private void Connect()
        {
            try
            {
                TCPcamara.Conectar();
                if (commands.command_user != "")
                {
                    TCPcamara.EnviarComando(commands.command_user + (char)13 + (char)10);
                    TCPcamara.EnviarComando("" + (char)13 + (char)10);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
            }
        }
        public bool isConnect()
        {
            return TCPcamara.conectado;
        }
        private bool ReadingBait(string serial)
        {
            try
            {
                string FileName = string.Empty;
                string result = string.Empty;
                if (commands.command_setstring != "")
                {
                    if (serial != string.Empty)
                    {
                        FileName = serial.Substring(0, (serial.Length - 1));
                        TCPcamara.EnviarComando(commands.command_setstring + FileName + (char)13 + (char)10);
                        //Thread.Sleep(150);
                    }
                }

                TCPcamara.EnviarComando(commands.command_setevent + (char)13 + (char)10);
                TCPcamara.EnviarComando(commands.command_getvalue_test + (char)13 + (char)10);
                //Thread.Sleep(500);
                result = TCPcamara.Leer();
                return ValidateResponse(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
            }
            return false;
        }

        private bool ReadingReal()
        {
            try
            {
                string result = string.Empty;
                TCPcamara.EnviarComando(commands.command_getvalue_real + (char)13 + (char)10);
                //Thread.Sleep(500);
                result = TCPcamara.Leer();
                readingTime = DateTime.Now;
                return ValidateResponse(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - " + ex.Message);
            }
            return false;
        }

        public BitmapImage getImageResultFromCamera(bool pass)
        {
            //Thread.Sleep(300);
            string path = pass ? commands.path_image : commands.path_image_show_errors;
            BitmapImage file = GetLatestCreatedImage(path);
            //Thread.Sleep(500);            
            return file;
        }
        public string getNameImageResultFromCamera(bool pass)
        {
            //Thread.Sleep(300);
            string path = pass ? commands.path_image : commands.path_image_show_errors;
            string file = GetLatestCreatedImagePath(path);
            //Thread.Sleep(500);            
            return file;
        }
        public void Disconnect()
        {
            TCPcamara.Desconectar();
        }

        public bool FirstInspectionAttempt(string serial)
        {
            Connect();
            if (!isConnect())
            {
                return false;
            }
            if (!ReadingBait(serial))
            {
                return false;
            }
            return ReadingReal();


            /*
            string readingReuslt;
            readingReuslt = tryCommunicationCamera("SFEXP 5.00" + (char)13 + (char)10);
            Thread.Sleep(100);

            if (ValidateResponse(readingReuslt)) 
            { 
                Thread.Sleep(10); 
            }

            return validateInspectionResult("GVOutput" + (char)13 + (char)10);       */

        }

        public bool SecondInspectionAttempt()
        {
            string readingReuslt;

            readingReuslt = tryCommunicationCamera("SFEXP 20.00" + (char)13 + (char)10);
            Thread.Sleep(100);

            if (ValidateResponse(readingReuslt))
            {
                Thread.Sleep(10);
            }

            if (validateConnectorCable() && validateRountingCable())
            {
                return true;
            }
            return false;
        }


        public bool ThirdInspectionAttempt()
        {
            string readingReuslt;
            readingReuslt = tryCommunicationCamera("SFEXP 10.00" + (char)13 + (char)10);
            Thread.Sleep(100);

            if (ValidateResponse(readingReuslt))
            {
                Thread.Sleep(10);
            }
            return validateInspectionResult("GVResultado" + (char)13 + (char)10);
        }

        private bool validateConnectorCable()
        {
            string readingReuslt;
            readingReuslt = getResultCamera("GVOutput3" + (char)13 + (char)10);
            Thread.Sleep(100);
            return ValidateResponse(readingReuslt);
        }
        private bool validateRountingCable()
        {
            string readingReuslt;
            readingReuslt = getResultCamera("GVOutput2" + (char)13 + (char)10);
            Thread.Sleep(100);
            return ValidateResponse(readingReuslt);
        }

        private bool validateInspectionResult(string command)
        {
            string readingReuslt;
            readingReuslt = getResultCamera(command);
            Thread.Sleep(100);
            return ValidateResponse(readingReuslt);
        }
        private bool ValidateResponse(string readingReuslt)
        {
            if (readingReuslt.Contains("1\r\n") || readingReuslt.Contains("1.000\r\n"))
            {
                Thread.Sleep(500);
                return true;
            }
            return false;
        }
        private string tryCommunicationCamera(string commandAttempt)
        {
            TCPcamara.EnviarComando(commandAttempt);
            Thread.Sleep(100);
            TCPcamara.EnviarComando("SE8" + (char)13 + (char)10);
            TCPcamara.EnviarComando("GVOutput1" + (char)13 + (char)10);
            return TCPcamara.Leer();
        }
        private string getResultCamera(string command)
        {
            TCPcamara.EnviarComando(command);
            return TCPcamara.Leer();
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
        private BitmapImage GetLatestCreatedImage(string path)
        {
            BitmapImage bitmapImage = new BitmapImage();
            string filename = string.Empty;
            string file = string.Empty;            
            DirectoryInfo dir = new DirectoryInfo(path);
            var files = dir.GetFiles().OrderByDescending(f => f.LastWriteTime).ToList();


            if (files.Count > 0)
            {
                filename = files.First().FullName;
                file = Path.GetFileNameWithoutExtension(filename);
                int numfiles = files.Count(x => x.Name.Contains(file));

                FileInfo fileInfo = new FileInfo(filename);
                TimeSpan OneMinute = TimeSpan.FromMinutes(1);
                TimeSpan createdSpamFile = readingTime - fileInfo.CreationTime;
                if (createdSpamFile > OneMinute) //Para revisar que el archivo sea el creado por la camara y no un respaldo
                {
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(sessionApp.PathOperationalImages + "image_not_found.jpg");
                    bitmapImage.EndInit();
                    Debug.WriteLine($"{DateTime.Now} - " + $"El tiempo entre la toma de la imagen y la lectura es mayor a un minuto: {createdSpamFile}");
                    return bitmapImage;
                }
                if (numfiles >= 2)
                {
                    filename = filename.Replace(".bmp", ".svg");
                    filename = filename.Replace(".jpg", ".svg");
                    filename = filename.Replace(".jpeg", ".svg");
                }
                if (filename.Contains(".svg"))
                {                    
                    bitmapImage = TransformSVGtoPNG(filename);
                }
                else
                {
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(filename);
                    bitmapImage.EndInit();
                }
            }
            else
            {
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(sessionApp.PathOperationalImages + "image_not_found.jpg");
                bitmapImage.EndInit();
                Debug.WriteLine($"{DateTime.Now} - " + "No encontre archivos que mostrar");
            }
            return bitmapImage;
        }
        private string GetLatestCreatedImagePath(string path)
        {            
            string filename = string.Empty;
            string file = string.Empty;
            DirectoryInfo dir = new DirectoryInfo(path);
            var files = dir.GetFiles().OrderByDescending(f => f.LastWriteTime).ToList();


            if (files.Count > 0)
            {
                filename = files.First().FullName;
                file = Path.GetFileNameWithoutExtension(filename);
                int numfiles = files.Count(x => x.Name.Contains(file));

                FileInfo fileInfo = new FileInfo(filename);
                TimeSpan OneMinute = TimeSpan.FromMinutes(1);
                TimeSpan createdSpamFile = readingTime - fileInfo.CreationTime;
                if (createdSpamFile > OneMinute) //Para revisar que el archivo sea el creado por la camara y no un respaldo
                {                    
                    Debug.WriteLine($"{DateTime.Now} - " + $"El tiempo entre la toma de la imagen y la lectura es mayor a un minuto: {createdSpamFile}");
                    return sessionApp.PathOperationalImages + "image_not_found.jpg";
                }
                if (numfiles >= 2)
                {
                    filename = filename.Replace(".bmp", ".svg");
                    filename = filename.Replace(".jpg", ".svg");
                    filename = filename.Replace(".jpeg", ".svg");
                }
                return filename;
            }
            else
            {
                Debug.WriteLine($"{DateTime.Now} - " + "No encontre archivos que mostrar");
                return sessionApp.PathOperationalImages + "image_not_found.jpg";
            }            
        }
    }
}

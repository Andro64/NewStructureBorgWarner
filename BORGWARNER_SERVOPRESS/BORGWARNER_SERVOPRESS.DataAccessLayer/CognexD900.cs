using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    class CognexD900
    {

        public static bool Inspection_Result = false;
        public static bool Inspection_ResultD1 = false;
        public static bool Inspection_ResultD2 = false;

        public static bool ready;

        public static void cambiarJob(TCP_IP _Camara, string _IP)
        {
            TCP_IP camara = new TCP_IP(23);
            string Inspection_IP = "";

            camara = _Camara;
            Inspection_IP = _IP;

            string Lectura_trabajo = "";
            string Aux_trabajo = "";

            camara.Conectar(Inspection_IP);

            camara.EnviarComando("admin" + (char)13 + (char)10);

            camara.EnviarComando("SW8" + (char)13 + (char)10);

            camara.EnviarComando("GVOUTPUT" + (char)13 + (char)10);

            camara.EnviarComando("GJ" + (char)13 + (char)10);

            //camara.EnviarComando("SW8" + (char)13 + (char)10);

            Thread.Sleep(25);

            Lectura_trabajo = camara.Leer();

            Thread.Sleep(25);

            if (Lectura_trabajo.Contains("1\r"))
            {
                Aux_trabajo = Lectura_trabajo.Substring(3, 2);

                if (Aux_trabajo != "96")
                {
                    camara.EnviarComando("SJ96" + (char)13 + (char)10);

                    Thread.Sleep(20);

                    camara.Desconectar();
                }
            }
            else if (Lectura_trabajo.Contains("-2"))
            {
                camara.EnviarComando("SJ96" + (char)13 + (char)10);

                Thread.Sleep(20);

                camara.Desconectar();
            }

            camara.Desconectar();

            return;

        }

        public static void Trigger(TCP_IP _Camara, string _IP)
        {
            TCP_IP camara = new TCP_IP(23);
            TCP_IP camaraD1 = new TCP_IP(23);
            TCP_IP camaraD2 = new TCP_IP(23);
            camara = _Camara;
            camaraD1 = _Camara;
            camaraD2 = _Camara;
            string Inspection_IP = "";
            Inspection_IP = _IP;

            string Lectura = "";
            string Herramienta = "";

            string LecturaD1 = "";
            string HerramientaD1 = "";

            string LecturaD2 = "";
            string HerramientaD2 = "";

            camara.Conectar(Inspection_IP);

            camara.EnviarComando("admin" + (char)13 + (char)10);

            camara.EnviarComando("" + (char)13 + (char)10);

            camara.EnviarComando("SE8" + (char)13 + (char)10); // comando telnet que da triguer a la camara

            camara.EnviarComando("GVMath_1.B1" + (char)13 + (char)10); // comando telnet que obtiene resultado de la camara

            if (camara.conectado)
            {
                Lectura = camara.Leer();
                Thread.Sleep(500);
                if (Lectura.Contains("1\r\n"))
                {
                    Herramienta = Lectura.Substring(3, 5);

                    if (Herramienta == "1.000")
                    {
                        Inspection_Result = true;
                        ready = true;
                    }
                    else
                    {
                        Inspection_Result = false;
                        ready = true;
                    }
                }

                camara.Desconectar();
            }

            camaraD1.Conectar(Inspection_IP);

            camaraD1.EnviarComando("admin" + (char)13 + (char)10);

            camaraD1.EnviarComando("" + (char)13 + (char)10);

            camaraD1.EnviarComando("GVMath_1.B2" + (char)13 + (char)10); // comando telnet que obtiene resultado de la camara

            if (camaraD1.conectado)
            {
                LecturaD1 = camaraD1.Leer();
                Thread.Sleep(500);
                if (LecturaD1.Contains("1\r\n"))
                {
                    HerramientaD1 = LecturaD1.Substring(3, 5);

                    if (HerramientaD1 == "1.000")
                    {
                        Inspection_ResultD1 = true;
                        ready = true;
                    }
                    else
                    {
                        Inspection_ResultD1 = false;
                        ready = true;
                    }
                }

                camaraD1.Desconectar();
            }

            camaraD2.Conectar(Inspection_IP);

            camaraD2.EnviarComando("admin" + (char)13 + (char)10);

            camaraD2.EnviarComando("" + (char)13 + (char)10);

            camaraD2.EnviarComando("GVMath_1.B3" + (char)13 + (char)10); // comando telnet que obtiene resultado de la camara

            if (camaraD2.conectado)
            {
                LecturaD2 = camaraD2.Leer();
                Thread.Sleep(500);
                if (LecturaD2.Contains("1\r\n"))
                {
                    HerramientaD2 = LecturaD2.Substring(3, 5);

                    if (HerramientaD2 == "1.000")
                    {
                        Inspection_ResultD2 = true;
                        ready = true;
                    }
                    else
                    {
                        Inspection_ResultD2 = false;
                        ready = true;
                    }
                }

                camaraD2.Desconectar();
                Thread.Sleep(50);
            }
        }


        public static string GetLatestFile(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            string file = dir.GetFiles()
                .OrderByDescending(f => f.LastWriteTime)
                .First().ToString();

            return $"{path}\\{file}";
        }
    }
}

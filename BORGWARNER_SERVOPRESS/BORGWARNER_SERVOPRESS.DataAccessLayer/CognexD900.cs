using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using BORGWARNER_SERVOPRESS.DataModel;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    public class CognexD900
    {
        SessionApp sessionApp;
        Camara _camara;

        public static bool Inspection_Result = false;
        public static bool Inspection_ResultD1 = false;
        public static bool Inspection_ResultD2 = false;

        public static bool ready;
        public CognexD900(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            Initialize();
        }
      
        public void Initialize()
        {
            _camara = new Camara() { 
                IP = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals(eTypeDevices.Scanner) && x.idTypeConnection.Equals(eTypeConnection.CognexD900)).IP,
                Port = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals(eTypeDevices.Scanner) && x.idTypeConnection.Equals(eTypeConnection.CognexD900)).Port
            };            
        }
       

        public void Trigger(TCP_IP _Camara)
        {
            TCP_IP camara = new TCP_IP(_camara.IP, _camara.Port);
            TCP_IP camaraD1 = new TCP_IP(_camara.IP, _camara.Port);
            TCP_IP camaraD2 = new TCP_IP(_camara.IP, _camara.Port);
            camara = _Camara;
            camaraD1 = _Camara;
            camaraD2 = _Camara;
            string Inspection_IP = "";
            Inspection_IP = _camara.IP;

            string Lectura = "";
            string Herramienta = "";

            string LecturaD1 = "";
            string HerramientaD1 = "";

            string LecturaD2 = "";
            string HerramientaD2 = "";

            camara.Conectar();

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

            camaraD1.Conectar();

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

            camaraD2.Conectar();

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

        public void TriggerPad(TCP_IP _Camara, string _IP)
        {
            TCP_IP camara = new TCP_IP(_camara.IP, _camara.Port);
            camara = _Camara;
            string Inspection_IP = "";
            Inspection_IP = _IP;

            string Lectura1 = "";
            string Lectura2 = "";
            string Lectura3 = "";
            string Lectura4 = "";

            bool RuteoCable;
            bool ConectorCable;

            if (!camara.conectado)
            {
                camara.Conectar();

                camara.EnviarComandoSinRespuesta("admin" + (char)13 + (char)10);

                camara.EnviarComandoSinRespuesta("" + (char)13 + (char)10);
            }

            if (camara.conectado)
            {
                if(true)//if (G.status == 315)  Inspeccionando pieza
                {
                    camara.EnviarComando("SFEXP 5.00" + (char)13 + (char)10);

                    Thread.Sleep(100);

                    camara.EnviarComando("SE8" + (char)13 + (char)10);

                    camara.EnviarComando("GVOutput1" + (char)13 + (char)10);

                    Lectura2 = camara.Leer();

                    Thread.Sleep(100);

                    if (Lectura2.Contains("1\r\n") || Lectura2.Contains("1.000\r\n"))
                    {
                        Thread.Sleep(10);
                    }

                    //camara.EnviarComando("SE8" + (char)13 + (char)10); // comando telnet que da trigger a la camara

                    camara.EnviarComando("GVOutput" + (char)13 + (char)10); // comando telnet que obtiene resultado de la camara              

                    Lectura2 = camara.Leer();

                    Thread.Sleep(100);

                    if (Lectura2.Contains("1\r\n") || Lectura2.Contains("1.000\r\n"))
                    {
                        Inspection_Result = true;
                        ready = true;
                    }
                    else
                    {
                        Inspection_Result = true;
                        ready = true;
                    }
                    Lectura2 = "";
                    Thread.Sleep(50);
                }

                else if(true)//if (G.status == 337) Inspeccionando piezas
                {

                    camara.EnviarComando("SFEXP 20.00" + (char)13 + (char)10);

                    Thread.Sleep(150);

                    camara.EnviarComando("SE8" + (char)13 + (char)10);

                    //Thread.Sleep(50);

                    camara.EnviarComando("GVOutput1" + (char)13 + (char)10);

                    Lectura3 = camara.Leer();

                    Thread.Sleep(100);

                    if (Lectura3.Contains("1\r\n") || Lectura3.Contains("1.000\r\n"))
                    {
                        Thread.Sleep(100);
                    }

                    camara.EnviarComando("GVOutput3" + (char)13 + (char)10);// comando telnet que obtiene resultado de la camara              

                    Lectura3 = camara.Leer();

                    Thread.Sleep(100);

                    if (Lectura3.Contains("1\r\n") || Lectura3.Contains("1.000\r\n"))
                    {
                        ConectorCable = true;
                        //Inspection_Result = true;
                        //ready = true;
                    }
                    else
                    {
                        ConectorCable = false;
                        //Inspection_Result = false;
                        //ready = true;
                    }

                    camara.EnviarComando("GVOutput2" + (char)13 + (char)10);// comando telnet que obtiene resultado de la camara              

                    Lectura1 = camara.Leer();

                    Thread.Sleep(100);

                    if (Lectura1.Contains("1\r\n") || Lectura1.Contains("1.000\r\n"))
                    {
                        RuteoCable = true;
                        //Inspection_Result = true;
                        //ready = true;
                    }
                    else
                    {
                        RuteoCable = false;
                        //Inspection_Result = false;
                        //ready = true;
                    }

                    if (RuteoCable && ConectorCable)
                    {
                        Inspection_Result = true;
                        ready = true;
                    }
                    else
                    {
                        Inspection_Result = true;
                        ready = true;
                    }
                    Lectura1 = "";
                    Lectura3 = "";
                    Thread.Sleep(50);

                }

                else if(true)//if (G.status == 339) Insepeccionando piezas
                {
                    camara.EnviarComando("SFEXP 10.00" + (char)13 + (char)10);

                    Thread.Sleep(150);

                    camara.EnviarComando("SE8" + (char)13 + (char)10);

                    //Thread.Sleep(50);

                    camara.EnviarComando("GVOutput1" + (char)13 + (char)10);

                    Lectura4 = camara.Leer();

                    Thread.Sleep(100);

                    if (Lectura4.Contains("1\r\n") || Lectura4.Contains("1.000\r\n"))
                    {
                        Thread.Sleep(100);
                    }

                    camara.EnviarComando("GVResultado" + (char)13 + (char)10);// comando telnet que obtiene resultado de la camara              

                    Lectura4 = camara.Leer();

                    Thread.Sleep(100);

                    if (Lectura4.Contains("1\r\n") || Lectura4.Contains("1.000\r\n"))
                    {
                        Inspection_Result = true;
                        ready = true;
                    }
                    else
                    {
                        Inspection_Result = true;
                        ready = true;
                    }
                    Lectura4 = "";
                    Thread.Sleep(50);
                }
                camara.Desconectar();
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

using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class VisionSystem
    {

        public  bool Inspection_Result = false;
        public  bool Inspection_ResultD1 = false;
        public  bool Inspection_ResultD2 = false;
        public  bool ready;

        SessionApp sessionApp;
        Camara camara;
        TCP_IP TCPcamara;

        public VisionSystem(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            Initialize();
        }
        public void Initialize()
        {
            TCPcamara = new TCP_IP(camara.Port);
            camara = new Camara()
            {
                IP = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals(eTypeDevices.Scanner) && x.idTypeConnection.Equals(eTypeConnection.CognexD900)).IP,
                Port = sessionApp.connectionsWorkStation.FirstOrDefault(x => x.idTypeDevice.Equals(eTypeDevices.Scanner) && x.idTypeConnection.Equals(eTypeConnection.CognexD900)).Port
            };
        }
        public bool InspecitionUltraCapBoardPad()
        {
            
            TriggerPad();
            Thread.Sleep(300);
            string file = GetLatestFile(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("Cognex_Camara_Path")).valueSetting);
            Thread.Sleep(300);
            string nameFile = file.Remove(file.Length - 3) + "jpg";
            Thread.Sleep(500);
            return Inspection_Result;
        }
        public void TriggerPad()
        {          
           
            string Lectura1 = "";
            string Lectura2 = "";
            string Lectura3 = "";
            string Lectura4 = "";

            bool RuteoCable;
            bool ConectorCable;

            if (!TCPcamara.conectado)
            {
                TCPcamara.Conectar(camara.IP);

                TCPcamara.EnviarComandoSinRespuesta("admin" + (char)13 + (char)10);

                TCPcamara.EnviarComandoSinRespuesta("" + (char)13 + (char)10);
            }

            if (TCPcamara.conectado)
            {
                if (true)//if (G.status == 315)
                {
                    TCPcamara.EnviarComando("SFEXP 5.00" + (char)13 + (char)10);

                    Thread.Sleep(100);

                    TCPcamara.EnviarComando("SE8" + (char)13 + (char)10);

                    TCPcamara.EnviarComando("GVOutput1" + (char)13 + (char)10);

                    Lectura2 = TCPcamara.Leer();

                    Thread.Sleep(100);

                    if (Lectura2.Contains("1\r\n") || Lectura2.Contains("1.000\r\n"))
                    {
                        Thread.Sleep(10);
                    }

                    //camara.EnviarComando("SE8" + (char)13 + (char)10); // comando telnet que da trigger a la camara

                    TCPcamara.EnviarComando("GVOutput" + (char)13 + (char)10); // comando telnet que obtiene resultado de la camara              

                    Lectura2 = TCPcamara.Leer();

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

                else if (true)//if (G.status == 337)
                {

                    TCPcamara.EnviarComando("SFEXP 20.00" + (char)13 + (char)10);

                    Thread.Sleep(150);

                    TCPcamara.EnviarComando("SE8" + (char)13 + (char)10);

                    //Thread.Sleep(50);

                    TCPcamara.EnviarComando("GVOutput1" + (char)13 + (char)10);

                    Lectura3 = TCPcamara.Leer();

                    Thread.Sleep(100);

                    if (Lectura3.Contains("1\r\n") || Lectura3.Contains("1.000\r\n"))
                    {
                        Thread.Sleep(100);
                    }

                    TCPcamara.EnviarComando("GVOutput3" + (char)13 + (char)10);// comando telnet que obtiene resultado de la camara              

                    Lectura3 = TCPcamara.Leer();

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

                    TCPcamara.EnviarComando("GVOutput2" + (char)13 + (char)10);// comando telnet que obtiene resultado de la camara              

                    Lectura1 = TCPcamara.Leer();

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

                else if (true)//if (G.status == 339)
                {
                    TCPcamara.EnviarComando("SFEXP 10.00" + (char)13 + (char)10);

                    Thread.Sleep(150);

                    TCPcamara.EnviarComando("SE8" + (char)13 + (char)10);

                    //Thread.Sleep(50);

                    TCPcamara.EnviarComando("GVOutput1" + (char)13 + (char)10);

                    Lectura4 = TCPcamara.Leer();

                    Thread.Sleep(100);

                    if (Lectura4.Contains("1\r\n") || Lectura4.Contains("1.000\r\n"))
                    {
                        Thread.Sleep(100);
                    }

                    TCPcamara.EnviarComando("GVResultado" + (char)13 + (char)10);// comando telnet que obtiene resultado de la camara              

                    Lectura4 = TCPcamara.Leer();

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
                TCPcamara.Desconectar();
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
        public string EnviarComandos(string commando1, string commando2, string commando3)
        {
            TCPcamara.EnviarComando("SFEXP 5.00" + (char)13 + (char)10);
            Thread.Sleep(100);
            TCPcamara.EnviarComando("SE8" + (char)13 + (char)10);
            TCPcamara.EnviarComando("GVOutput1" + (char)13 + (char)10);
            return TCPcamara.Leer();
        }
    }
}

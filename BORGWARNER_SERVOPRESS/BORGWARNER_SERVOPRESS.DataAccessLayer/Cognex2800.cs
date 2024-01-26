using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    public class Cognex2800
    {
        public static bool Inspection_Result = false;
        public static bool Inspection_Result_Down = false;
        public static bool Inspection_Result2 = false;
        public static bool Inspection_Result3 = false;
        public static bool Inspection_Result4 = false;
        public static bool CONECTOR_1 = false;
        public static bool CONECTOR_2 = false;
        public static bool CONECTOR_3 = false;
        public static bool CONECTOR_4 = false;
        public static bool ready;
        public static int Inspection_Result_Value = 0;

        public static void cambiarJob(TCP_IP _Camara, string _IP, int port)
        {
            TCP_IP camara = new TCP_IP(_IP,port);
            string Inspection_IP = "";

            camara = _Camara;
            Inspection_IP = _IP;

            string Lectura_trabajo = "";
            string Aux_trabajo = "";

            camara.Conectar();

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

        public static void Trigger(TCP_IP _Camara, string _IP, int port)
        {
            TCP_IP camara = new TCP_IP(_IP, port);
            camara = _Camara;
            string Inspection_IP = "";
            Inspection_IP = _IP;

            string Lectura = "";
            string Lectura2 = "";
            string Lectura3 = "";
            string Lectura4 = "";
            string Lectura5 = "";
            string Lectura6 = "";
            string Lectura7 = "";

            string Herramienta = "";
            string Herramienta2 = "";
            string Herramienta3 = "";
            string Herramienta4 = "";
            string Herramienta5 = "";
            string Herramienta6 = "";
            string Herramienta7 = "";

            camara.Conectar();
            //Aqui empieza hacer inspeccion PARA EL BYPASS  de las demas inspecciones
            camara.EnviarComando("admin" + (char)13 + (char)10);

            camara.EnviarComando("" + (char)13 + (char)10);

            if (camara.conectado)
            {
                Thread.Sleep(100);

                camara.EnviarComando("SW8" + (char)13 + (char)10);

                Thread.Sleep(50);

                if (Inspection_IP == "192.168.1.120")
                {
                    camara.EnviarComando("GVLOGICA2" + (char)13 + (char)10);

                    Thread.Sleep(50);

                    Lectura4 = camara.Leer();

                    Thread.Sleep(50);

                    if (Lectura4.Contains("1.000\r\n"))
                    {
                        Herramienta4 = Lectura4.Substring(3, 5);

                        if (Herramienta4 == "1.000\r\n")
                        {
                            CONECTOR_4 = true;
                        }
                        else
                        {
                            Inspection_Result = false;
                            CONECTOR_4 = false;
                            Thread.Sleep(50);
                            Inspection_Result_Value = 4;
                            ready = true;
                        }

                    }


                    camara.EnviarComando("GVLOGICA1" + (char)13 + (char)10);

                    Thread.Sleep(50);

                    Lectura = camara.Leer();

                    Thread.Sleep(50);

                    if (Lectura.Contains("1.000\r\n"))
                    {
                        CONECTOR_1 = true;
                        ready = true;

                    }
                    else
                    {
                        Inspection_Result = false;
                        CONECTOR_1 = false;
                        Thread.Sleep(50);
                        Inspection_Result_Value = 1;
                        ready = true;
                    }

                    camara.EnviarComando("GVLOGICA3" + (char)13 + (char)10);

                    Thread.Sleep(50);

                    Lectura7 = camara.Leer();

                    Thread.Sleep(50);

                    if (Lectura7.Contains("1.000\r\n"))
                    {
                        CONECTOR_2 = true;
                        ready = true;

                    }
                    else
                    {
                        Inspection_Result = false;
                        CONECTOR_2 = false;
                        Thread.Sleep(50);
                        Inspection_Result_Value = 2;
                        ready = true;
                    }

                    camara.EnviarComando("GVLOGICA4" + (char)13 + (char)10);

                    Thread.Sleep(50);

                    Lectura2 = camara.Leer();

                    Thread.Sleep(50);

                    if (Lectura2.Contains("1.000\r\n"))
                    {
                        CONECTOR_3 = true;
                        ready = true;

                    }
                    else
                    {
                        Inspection_Result = false;
                        CONECTOR_3 = false;
                        Thread.Sleep(50);
                        Inspection_Result_Value = 3;
                        ready = true;
                    }


                    if (CONECTOR_1 && CONECTOR_2 && CONECTOR_3)
                    {
                        Inspection_Result = true;
                        Inspection_Result_Value = 0;

                    }
                    //else
                    //{
                    //    MessageBox.Show("ENTRE");

                    //}


                }
                else
                {
                    Thread.Sleep(50);

                    if (Inspection_IP == "192.168.1.130")
                    {


                        camara.EnviarComando("GVLOGICA4" + (char)13 + (char)10);

                        Thread.Sleep(50);

                        Lectura4 = camara.Leer();

                        Thread.Sleep(50);

                        if (Lectura4.Contains("1.000\r\n"))
                        {
                            CONECTOR_4 = true;
                            ready = true;
                        }
                        else
                        {
                            Inspection_Result = false;
                            CONECTOR_4 = false;
                            Thread.Sleep(50);
                            Inspection_Result_Value = 4;
                            ready = true;
                        }
                        camara.EnviarComando("GVLOGICA1" + (char)13 + (char)10);

                        Lectura3 = camara.Leer();

                        Thread.Sleep(50);

                        if (Lectura3.Contains("1.000\r\n"))
                        {
                            CONECTOR_1 = true;
                            ready = true;
                        }
                        else
                        {
                            Inspection_Result = false;
                            CONECTOR_1 = false;
                            Thread.Sleep(50);
                            Inspection_Result_Value = 1;
                            ready = true;
                        }

                        camara.EnviarComando("GVLOGICA2" + (char)13 + (char)10);

                        Thread.Sleep(50);

                        Lectura5 = camara.Leer();

                        Thread.Sleep(50);

                        if (Lectura5.Contains("1.000\r\n"))
                        {
                            CONECTOR_2 = true;
                            ready = true;
                        }
                        else
                        {
                            Inspection_Result = false;
                            CONECTOR_2 = false;
                            Thread.Sleep(50);
                            Inspection_Result_Value = 2;
                            ready = true;
                        }

                        camara.EnviarComando("GVLOGICA3" + (char)13 + (char)10);

                        Thread.Sleep(50);

                        Lectura6 = camara.Leer();

                        Thread.Sleep(50);

                        if (Lectura6.Contains("1.000\r\n"))
                        {
                            CONECTOR_3 = true;
                            ready = true;
                        }
                        else
                        {
                            Inspection_Result = false;
                            CONECTOR_3 = false;
                            Thread.Sleep(50);
                            Inspection_Result_Value = 3;
                            ready = true;
                        }
                        if (CONECTOR_1 && CONECTOR_2 && CONECTOR_3)
                        {
                            Inspection_Result = true;
                            Inspection_Result_Value = 0;

                        }
                        ////////////////////////////////////////////////////////////////////////////////////////////////

                    }
                    else
                    {
                        Thread.Sleep(50);

                        if (Inspection_IP == "192.168.1.140")
                        {

                            Thread.Sleep(50);

                            camara.EnviarComando("GVMath_1.B4" + (char)13 + (char)10);

                            Thread.Sleep(50);

                            Lectura4 = camara.Leer();

                            Thread.Sleep(50);

                            if (Lectura4.Contains("1.000\r\n"))
                            {
                                CONECTOR_4 = true;
                                ready = true;
                            }
                            else
                            {
                                Inspection_Result = false;
                                CONECTOR_4 = false;
                                Thread.Sleep(50);
                                Inspection_Result_Value = 4;
                                ready = true;
                            }
                            camara.EnviarComando("GVMath_1.B1" + (char)13 + (char)10);

                            Thread.Sleep(50);

                            Lectura3 = camara.Leer();

                            Thread.Sleep(50);

                            if (Lectura3.Contains("1.000\r\n") || Lectura3.Contains("1\r\n"))
                            {
                                CONECTOR_1 = true;
                                ready = true;
                            }
                            else
                            {
                                Inspection_Result = false;
                                CONECTOR_1 = false;
                                Thread.Sleep(50);
                                Inspection_Result_Value = 1;
                                ready = true;
                            }

                            camara.EnviarComando("GVMath_1.B2" + (char)13 + (char)10);

                            Thread.Sleep(50);

                            Lectura5 = camara.Leer();

                            Thread.Sleep(50);

                            if (Lectura5.Contains("1.000\r\n") || Lectura5.Contains("1\r\n"))
                            {
                                CONECTOR_2 = true;
                                ready = true;
                            }
                            else
                            {
                                Inspection_Result = false;
                                CONECTOR_2 = false;
                                Thread.Sleep(50);
                                Inspection_Result_Value = 2;
                                ready = true;
                            }

                            camara.EnviarComando("GVMath_1.B3" + (char)13 + (char)10);

                            Thread.Sleep(50);

                            Lectura6 = camara.Leer();

                            Thread.Sleep(50);

                            if (Lectura6.Contains("1.000\r\n") || Lectura6.Contains("1\r\n"))
                            {
                                CONECTOR_3 = true;
                                ready = true;
                            }
                            else
                            {
                                Inspection_Result = false;
                                CONECTOR_3 = false;

                                Inspection_Result_Value = 3;
                                ready = true;
                            }
                            Thread.Sleep(50);
                            if (CONECTOR_1 && CONECTOR_2 && CONECTOR_3)
                            {
                                Inspection_Result = true;
                                Inspection_Result_Value = 0;
                                Thread.Sleep(50);

                            }
                            //else
                            //{
                            //    MessageBox.Show("ENTRE");

                            //}



                        }


                    }


                }


                //camara.EnviarComando("GVLOG_4" + (char)13 + (char)10);

                //if (prueba3.Contains("1\r\n"))
                //{
                //    Herramienta4 = prueba3.Substring(3, 5);
                //    if (Herramienta == "1.000")
                //    {
                //        Inspection_Result4 = true;
                //        ready = true;
                //    }
                //    else
                //    {
                //        Inspection_Result4 = false;
                //        ready = true;
                //    }
                //}
                //camara4.Desconectar();
                //Thread.Sleep(25);
            }

            //Aqui empieza hacer inspeccion la primera herramienta del congnex inspecciona el conector de refrigeracion 
            //camara.Conectar(Inspection_IP);            
            //camara.EnviarComando("admin" + (char)13 + (char)10);
            //camara.EnviarComando("" + (char)13 + (char)10);           
            //camara.EnviarComando("GVLOG_1" + (char)13 + (char)10);           

            //if (camara.conectado)
            //{                              
            //    Lectura = camara.Leer();
            //    Thread.Sleep(300);
            //    if (Lectura.Contains("1\r\n"))
            //    {
            //        Herramienta = Lectura.Substring(3, 5);
            //        if (Herramienta == "1.000")
            //        {
            //            Inspection_Result = true;
            //            ready = true;
            //        }
            //        else
            //        {
            //            Inspection_Result = false;
            //            ready = true;
            //        }

            //    }          

            //    camara.Desconectar();
            //    Thread.Sleep(25);
            //}

            ////Aqui empieza hacer inspeccion segunda herramienta del congnex aqui inspecciona las barras de los conectores
            //camara2.Conectar(Inspection_IP);
            //camara2.EnviarComando("admin" + (char)13 + (char)10);
            //camara2.EnviarComando("" + (char)13 + (char)10);          
            //camara2.EnviarComando("GVLOG_2" + (char)13 + (char)10);

            //if (camara2.conectado)
            //{                           
            //    prueba1 = camara2.Leer2();
            //    Thread.Sleep(300);              

            //    if (prueba1.Contains("1\r\n"))
            //    {
            //        Herramienta2 = prueba1.Substring(3, 5);
            //        if (Herramienta2 == "1.000")
            //        {
            //           // MessageBox.Show("APROBE");
            //            Inspection_Result2 = true;
            //            ready = true;
            //        }
            //        else
            //        {
            //           // MessageBox.Show("NO APROBE");
            //            Inspection_Result2 = false;
            //            ready = true;
            //        }
            //    }
            //    camara2.Desconectar();
            //    Thread.Sleep(25);
            //}
            //aqui empeiza hacer inspeccion la tercera herramienta del cognex
            //camara3.Conectar(Inspection_IP);
            //camara3.EnviarComando("admin" + (char)13 + (char)10);
            //camara3.EnviarComando("" + (char)13 + (char)10);
            //camara3.EnviarComando("GVLOG_3" + (char)13 + (char)10);      

            //if (camara3.conectado)
            //{
            //    prueba2 = camara3.Leer2();
            //    Thread.Sleep(300);               

            //    if (prueba2.Contains("1\r\n"))
            //    {
            //        Herramienta3 = prueba2.Substring(3, 5);
            //        if (Herramienta3 == "1.000")
            //        {
            //            // MessageBox.Show("APROBE");
            //            Inspection_Result3 = true;
            //            ready = true;
            //        }
            //        else
            //        {
            //            // MessageBox.Show("NO APROBE");
            //            Inspection_Result3 = false;
            //            ready = true;
            //        }
            //    }
            //    camara3.Desconectar();
            //    Thread.Sleep(25);

            //}
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

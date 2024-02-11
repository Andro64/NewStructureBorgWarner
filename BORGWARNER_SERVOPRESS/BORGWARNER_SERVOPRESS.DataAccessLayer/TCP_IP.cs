using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    public class TCP_IP
    {
        private const int RECV_DATA_MAX = 1024000;
        public ClientSocket clientSocketInstance;
        public bool conectado = false;        
        int puerto = 0;
        string ip;

        public TCP_IP(string _ip,int puertoConexion)
        {
            puerto = puertoConexion;
            ip = _ip;
        }

        public bool Conectar()
        {
            bool ok = false;

            if (!conectado)
            {
                try
                {
                    conectado = false;

                    //Configurar
                    string[] _ip = ip.Split(".".ToCharArray());
                    byte[] ipAddress = { byte.Parse(_ip[0]), byte.Parse(_ip[1]), byte.Parse(_ip[2]), byte.Parse(_ip[3]) };

                    IPAddress iTACIpAddress = new IPAddress(ipAddress);

                    clientSocketInstance = new ClientSocket(ipAddress, puerto);

                    //Conectar socket
                    try
                    {
                        if (clientSocketInstance.commandSocket != null)
                        {
                            clientSocketInstance.commandSocket.Close();
                        }

                        clientSocketInstance.commandSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        clientSocketInstance.commandSocket.Connect(clientSocketInstance.readerCommandEndPoint);
                        clientSocketInstance.commandSocket.ReceiveTimeout = 1000;

                        conectado = true;
                    }
                    catch (SocketException ex)
                    {
                        Debug.WriteLine($"{DateTime.Now} - "  + "Error en TCP_IP function Conectar: " + ex.Message, "Error TCP_IP");
                        conectado = false;
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{DateTime.Now} - "  + "Error en TCP_IP function Conectar: " + ex.Message, "Error TCP_IP");
                    conectado = false;
                    throw;
                }
            }
            else
            {
                ok = true;
            }

            return ok;
        }

        public string EnviarComando(string comando)
        {
            string respuesta = "";
            bool ok = true;

            if (conectado)
            {
                byte[] command = ASCIIEncoding.ASCII.GetBytes(comando);

                if (ok && conectado)
                {

                    try
                    {

                        if (clientSocketInstance.commandSocket != null)
                        {

                            //Enviar comando
                            try
                            {
                                clientSocketInstance.commandSocket.Send(command);
                            }
                            catch (Exception exc)
                            {
                                Debug.WriteLine($"{DateTime.Now} - "  + "Error en TCP_IP function EnviarComando: " + exc.Message, "Error TCP_IP");
                                throw;
                            }

                            //Esperar respuesta

                            byte[] recvBytes = new byte[RECV_DATA_MAX];
                            int recvSize = 0;

                            if (clientSocketInstance.commandSocket != null)
                            {
                                try
                                {
                                    recvSize = clientSocketInstance.commandSocket.Receive(recvBytes);
                                }
                                catch (SocketException exc)
                                {
                                    try
                                    {
                                        recvSize = clientSocketInstance.commandSocket.Receive(recvBytes);
                                    }
                                    catch (Exception ex2)
                                    {
                                        respuesta = "Error de conexion";
                                    }
                                }
                            }
                            else
                            {
                                respuesta = "0";
                            }


                            if (recvSize == 0)
                            {
                                respuesta = "No responde";
                            }
                            else
                            {
                                //Codificar datos
                                respuesta = "";
                                for (int i = 0; i < recvSize && i < recvBytes.Length; i++)
                                {
                                    respuesta += (char)recvBytes[i];
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        respuesta = "Error general al enviar comando";
                    }
                }

            }
            else
            {
                respuesta = "No conectado";
            }

            return respuesta;
        }

        public void EnviarComandoSinRespuesta(string comando)
        {
            bool ok = true;

            if (conectado)
            {
                byte[] command = ASCIIEncoding.ASCII.GetBytes(comando);

                if (ok && conectado)
                {
                    try
                    {
                        if (clientSocketInstance.commandSocket != null)
                        {
                            //Enviar comando
                            try
                            {
                                clientSocketInstance.commandSocket.Send(command);
                            }
                            catch (Exception exc)
                            {
                               Debug.WriteLine($"{DateTime.Now} - "  + "Error en TCP_IP function EnviarComandoSinRespuesta: " + exc.Message, "Error TCP_IP");
                               throw;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

            }
        }

        public string Leer()
        {
            string respuesta = "";

            //Esperar respuesta

            byte[] recvBytes = new byte[RECV_DATA_MAX];
            int recvSize = 0;

            if (clientSocketInstance.commandSocket != null)
            {
                try
                {

                    recvSize = clientSocketInstance.commandSocket.Receive(recvBytes);
                }
                catch (SocketException exc)
                {
                    try
                    {
                        recvSize = clientSocketInstance.commandSocket.Receive(recvBytes);
                    }
                    catch (Exception ex2)
                    {
                        respuesta = "Error de conexion";
                    }
                }
            }
            else
            {
                respuesta = "0";
            }


            if (recvSize == 0)
            {
                respuesta = "No responde";
            }
            else
            {
                //Codificar datos Shift-JIS.
                recvBytes[recvSize] = 0;
                respuesta = Encoding.GetEncoding("Shift_JIS").GetString(recvBytes);
            }

            return respuesta;
        }

        public string Leer2()
        {
            string respuesta2 = "";

            //Esperar respuesta

            byte[] recvBytes = new byte[RECV_DATA_MAX];
            int recvSize = 0;

            if (clientSocketInstance.commandSocket != null)
            {
                try
                {

                    recvSize = clientSocketInstance.commandSocket.Receive(recvBytes);
                }
                catch (SocketException exc)
                {
                    try
                    {
                        recvSize = clientSocketInstance.commandSocket.Receive(recvBytes);
                    }
                    catch (Exception ex2)
                    {
                        respuesta2 = "Error de conexion";
                    }
                }
            }
            else
            {
                respuesta2 = "0";
            }


            if (recvSize == 0)
            {
                respuesta2 = "No responde";
            }
            else
            {
                //Codificar datos Shift-JIS.
                recvBytes[recvSize] = 0;
                respuesta2 = Encoding.GetEncoding("Shift_JIS").GetString(recvBytes);
            }

            return respuesta2;
        }

        public void Desconectar()
        {

            //Cerrar socket
            if (conectado)
            {
                try
                {
                    clientSocketInstance.commandSocket.Close();
                    clientSocketInstance.commandSocket.Disconnect(true);
                    clientSocketInstance.commandSocket = null;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"{DateTime.Now} - "  + "Error en TCP_IP function Desconectar: " + ex.Message, "Error TCP_IP");
                    throw;
                }

            }
            conectado = false;
        }

        public class ClientSocket
        {
            public Socket commandSocket;
            public IPEndPoint readerCommandEndPoint;

            public ClientSocket(byte[] ipAddress, int readerCommandPort)
            {
                IPAddress readerIpAddress = new IPAddress(ipAddress);
                readerCommandEndPoint = new IPEndPoint(readerIpAddress, readerCommandPort);
                commandSocket = null;
            }
        }


    }
}

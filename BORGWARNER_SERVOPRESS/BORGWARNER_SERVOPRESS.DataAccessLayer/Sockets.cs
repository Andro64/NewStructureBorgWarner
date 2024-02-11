using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    class Sockets
    {
        public static string Client(string ip, int port, string message)
        {
            string result;
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];
            IPAddress ipAddress;
            IPEndPoint remoteEP;

            // Connect to a remote device.  
            try
            {
                ipAddress = IPAddress.Parse(ip);
                remoteEP = new IPEndPoint(ipAddress, port);
            }
            catch
            {
                return "IP error";
            }

            // Create a TCP/IP  socket.  
            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {
                sender.Connect(remoteEP);

                Console.WriteLine($"{DateTime.Now} - "  + "Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.  
                byte[] msg = Encoding.ASCII.GetBytes(message + "\r");

                // Send the data through the socket.  
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device.  
                int bytesRec = sender.Receive(bytes);
                result = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                Console.WriteLine($"{DateTime.Now} - "  + "Recieved=" + result);

                // Release the socket.  
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                // Comment all cathch, since this are supposed to be handled one upper level
                //    }
                //    catch (ArgumentNullException ane)
                //    {
                //        result = "ERROR";
                //        Console.WriteLine($"{DateTime.Now} - "  + "ArgumentNullException : {0}", ane.ToString());
                //    }
                //    catch (SocketException se)
                //    {
                //        result = "ERROR";
                //        Console.WriteLine($"{DateTime.Now} - "  + "SocketException : {0}", se.ToString());
                //    }
                //    catch (Exception e)
                //    {
                //        result = "ERROR";
                //        Console.WriteLine($"{DateTime.Now} - "  + "Unexpected exception : {0}", e.ToString());
                //    }

            }
            catch (Exception e)
            {
                result = "ERROR";
                Console.WriteLine($"{DateTime.Now} - "  + e.ToString());
            }
            return result;
        }
    }
}

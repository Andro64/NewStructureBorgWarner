using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class ScrewDriver
    {
        SessionApp sessionApp;
        Socket connection;
        CommunicationScrewDriver communicationScrewDriver;
        CancellationTokenSource CancellationToken_Screwing;
        private bool connectedScrewDriver;
        public ScrewDriver(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            communicationScrewDriver = new CommunicationScrewDriver(sessionApp);
            sessionApp.programs_ScrewDriver = communicationScrewDriver.getPrograms_ScrewDriver();
        }

        //public async Task<bool> ScrewingCompletedAsync(Screw screw)
        //{
        //    Debug.WriteLine("Entre: ScrewingCompletedAsync.");
        //    int i = 0;
        //    while (!screw.tighteningprocess.result)
        //    {
        //        try
        //        {
        //            //await Task.Delay(500);
        //            Debug.WriteLine($"Esperando atornillado.{i}");
        //            string response = await communicationScrewDriver.ResponseScrewDriverAsync(connection);
        //            i++;

        //            if (string.IsNullOrEmpty(response))
        //            {
        //                Debug.WriteLine("La respuesta del controlador de tornillo está vacía.");
        //                continue; // Vuelve al inicio del bucle y solicita la respuesta nuevamente
        //            }

        //            screw.tighteningprocess.resultResponse = response.Substring(4, 4);
        //            screw.tighteningprocess.result = screw.tighteningprocess.resultResponse == "0061";
        //            screw.tighteningprocess.id = response.Substring(221, 10);

        //            Debug.WriteLine($"{DateTime.Now} - " + response);
        //            if (screw.tighteningprocess.result)
        //            {
        //                screw.tighteningprocess.Torque = response.Substring(142, 4);
        //                screw.tighteningprocess.Angle = response.Substring(170, 4);
        //                screw.tighteningprocess.status = response.Substring(107, 1) == "2";
        //                return true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine($"Se produjo una excepción en ScrewingCompletedAsync: {ex.Message}");
        //            // Manejar la excepción según tus necesidades
        //            // Puedes lanzarla nuevamente si quieres que la aplicación la maneje en un nivel superior
        //            // throw;
        //        }

        //        await Task.Delay(5); // Esperar un breve período antes de la próxima solicitud al socket
        //        Debug.WriteLine("Esperar un breve período antes de la próxima solicitud al socket.");

        //    }
        //    return false;
        //}
      

        public async Task<ScrewingResult> ScrewingCompletedAsync(Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewingResult screwingResult = new ScrewingResult(); 
            
            await Task.Delay(500);
            Debug.WriteLine("Entre: ScrewingCompletedAsync.");
            
            bool timeoutReached = false;
            
            // Configuración del temporizador para medio minuto
            TimeSpan timeout = TimeSpan.FromSeconds(30);
            Timer timer = new Timer((state) => { timeoutReached = true; }, null, timeout, TimeSpan.FromMilliseconds(-1));

            while (!screw.tighteningprocess.result && !timeoutReached)
            {
                try
                {                    
                    
                    //Debug.WriteLine($"Esperando atornillado.");
//#if DEBUG
//                    string response = "02310061001 0000    010000020003STLA_AUTO_L1S12          04                         050006001070000080000090100111120002501300031014000280150000241600000170039618000001900000202024-03-19:18:44:02212024-01-13:20:58:28222230000001525";
//#else
                    string response = await communicationScrewDriver.ResponseScrewDriverAsync(connection);
//#endif

                    if (string.IsNullOrEmpty(response))
                    {
                        //Debug.WriteLine("La respuesta del controlador de tornillo está vacía.");
                        continue; // Vuelve al inicio del bucle y solicita la respuesta nuevamente
                    }

                    screw.tighteningprocess.resultResponse = response.Substring(4, 4);
                    screw.tighteningprocess.result = screw.tighteningprocess.resultResponse == "0061";
                    screw.tighteningprocess.id = response.Substring(221, 10);

                    Debug.WriteLine($"{DateTime.Now} - " + response);
                    if (screw.tighteningprocess.result)
                    {
                        screw.tighteningprocess.Torque = response.Substring(142, 4);
                        screw.tighteningprocess.Angle = response.Substring(170, 4);
                        screw.tighteningprocess.status = response.Substring(107, 1) == "2";
                        screwingResult.status = true;
                        timeoutReached = false;
                        break; // Salir del bucle cuando se reciba una respuesta satisfactoria
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Se produjo una excepción en ScrewingCompletedAsync: {ex.Message}");
                    throw;
                }

                await Task.Delay(5); // Esperar un breve período antes de la próxima solicitud al socket
                Debug.WriteLine("Esperar un breve período antes de la próxima solicitud al socket.");
            }

            timer.Dispose(); // Detener el temporizador

            if (timeoutReached)
            {
                sessionApp.messageTorque = "Se ha alcanzado el tiempo de espera. \r Por favor vuelva ejecutar el proceso";                
                await Task.Delay(500);
                screwingResult.timeout = true;
                Debug.WriteLine("Se ha alcanzado el tiempo de espera.");
            }

            return screwingResult;
        }

        //private async Task ScrewingCompleted(Screw screw, CancellationTokenSource cancellationTokenSource)
        //{
        //    try
        //    {                
        //        await Task.Run(async () =>
        //        {
        //            while (!screw.tighteningprocess.result)
        //            {
        //                sessionApp.messageTorque = $"{DateTime.Now} - " + "Entre al proceso ScrewingCompleted";
        //                Debug.WriteLine($"{DateTime.Now} - " + "Entre al proceso ScrewingCompleted");
        //                string response = communicationScrewDriver.responseScrewDriver(connection);
        //                Thread.Sleep(1000);
        //                Debug.WriteLine($"{DateTime.Now} - " + "Aqui obtuve el reponse");
        //                screw.tighteningprocess.resultResponse = response.Substring(4, 4).ToString();
        //                Debug.WriteLine($"{DateTime.Now} - " + "Hago el sustring" + response);
        //                screw.tighteningprocess.result = screw.tighteningprocess.resultResponse == "0061";
        //                screw.tighteningprocess.id = response.Substring(221, 10).ToString();

        //                Debug.WriteLine($"{DateTime.Now} - Respuesta del proceso" + response);
        //                if (screw.tighteningprocess.result)
        //                {
        //                    string torque = response.Substring(142, 4).ToString();
        //                    string angle = response.Substring(170, 4).ToString();

        //                    screw.tighteningprocess.Torque = $"{torque.Substring(0, 2)}.{torque.Substring(2, 2)}Nm";
        //                    screw.tighteningprocess.Angle = $"{angle.TrimStart('0')}°";
        //                    screw.tighteningprocess.status = response.Substring(107, 1).ToString().Equals("2");
        //                    //return true;
        //                }
                        
        //            }
        //        }, cancellationTokenSource.Token);
        //        if(!screw.tighteningprocess.result)
        //        {
        //            sessionApp.messageTorque = "Se ha concluido el atornillado";
        //            sessionApp.isScrewingFinished = true;
        //            Debug.WriteLine($"{DateTime.Now} - " + "Ya sali del proceso");
        //            //Ya termino
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.WriteLine("Error en ScrewingCompleted:" + ex.Message);
        //    }
        //    //return false;
        //}
        public async Task<ScrewingResult> Screwing(Screw screw, CancellationTokenSource _cancellationTokenSource, string programValue = "")
        {
            ScrewingResult result;
            //sessionApp.messageTorque = "...";
            await Task.Delay(500);
            connect();
            if (isScrewDriverConnected())
            {
                if (InRange())
                {
                    enableScrewdriver();
                    if (ScrewingProgram_by_Model(eTypePrograms.screwing, programValue) == "0005")
                    {
                        if (screwingSubscription() == "0005")
                        {
                            sessionApp.messageTorque = "Por favor proceda a atornillar.";
                            Debug.WriteLine("Por favor proceda a atornillar." );                            
                            result = await ScrewingCompletedAsync(screw, _cancellationTokenSource);
                            disconnect();
                            await DeployMessageScrewing(result);
                            return result;                                                                                 
                        }
                    }
                }
            }
            disconnect();
            return  new ScrewingResult();
        }
        public async Task DeployMessageScrewing(ScrewingResult result)
        {
            if (result.canceled_by_user)
            {
                sessionApp.messageTorque = "El atornillado ha sido cancelado por solicitud del usuario.";
            }
            else if (result.timeout)
            {
                sessionApp.messageTorque = "El atornillado ha sido cancelado debido a que se excedió el tiempo de espera.\rPor favor vuelva a intentarlo";
            }
            else if (!result.status)
            {
                sessionApp.messageTorque = "El proceso de atornillado ha finalizado con errores.";
            }
            else
            {
                sessionApp.messageTorque = "El proceso de atornillado ha sido completado exitosamente.";
            }
            await Task.Delay(500);
        }


        public async Task<ScrewingResult> Rescrewing(Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewingResult result;
            //sessionApp.messageTorque = "...";
            await Task.Delay(500);
            connect();
            if (isScrewDriverConnected())
            {
                if (InRange())
                {
                    enableScrewdriver();
                    if (ScrewingProgram_by_Model(eTypePrograms.screwing, sessionApp.programs_ScrewDriver.rescrewing) == "0005")
                    {
                        if (screwingSubscription() == "0005")
                        {
                            sessionApp.messageTorque = "Por favor proceda a atornillar.";
                            Debug.WriteLine("Por favor proceda a atornillar.");
                            result = await ScrewingCompletedAsync(screw, _cancellationTokenSource);
                            disconnect();
                            await DeployMessageScrewing(result);
                            return result;
                        }
                    }
                }
            }
            disconnect();
            return new ScrewingResult();
        }
        public async Task<ScrewingResult> Unscrewing(Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewingResult result;
            //sessionApp.messageTorque = "...";
            await Task.Delay(500);
            connect();
            if (isScrewDriverConnected())
            {
                if (InRange())
                {
                    enableScrewdriver();
                    if (ScrewingProgram_by_Model(eTypePrograms.screwing, sessionApp.programs_ScrewDriver.unscrewing) == "0005")
                    {
                        if (screwingSubscription() == "0005")
                        {
                            sessionApp.messageTorque = "Por favor proceda a atornillar.";
                            Debug.WriteLine("Por favor proceda a atornillar.");
                            result = await ScrewingCompletedAsync(screw, _cancellationTokenSource);
                            disconnect();
                            await DeployMessageScrewing(result);
                            return result;
                        }
                    }
                }
            }
            disconnect();
            return new ScrewingResult();
        }
        public async Task<TighteningProcess> tryScrewDriver(Screw screw, CancellationTokenSource _cancellationTokenSource, string programValue)
        {
            try
            {
                Debug.WriteLine("Entre: tryScrewDriver.");
                ScrewingResult result = await Screwing(screw, _cancellationTokenSource, programValue);
                if (result.status && !result.timeout && !result.canceled_by_user)
                    return screw.tighteningprocess;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Se produjo una excepción en tryScrewDriver: {ex.Message}");
               // throw;
                return null; // Otra opción podría ser devolver un valor predeterminado o realizar otra acción apropiada
            }
        }

        public async Task<TighteningProcess> FirstTighteningAttempt(Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewDriver screwdriver = new ScrewDriver(sessionApp);
            TighteningProcess  tightening = await tryScrewDriver(screw, _cancellationTokenSource,string.Empty);            
            return tightening;
        }
        public async Task<TighteningProcess> SecondTighteningAttempt(Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewDriver screwdriver = new ScrewDriver(sessionApp);
            TighteningProcess tightening = await tryScrewDriver(screw, _cancellationTokenSource, string.Empty);
            return tightening;
        }
        public async Task<TighteningProcess> ThirdTighteningAttempt(Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewDriver screwdriver = new ScrewDriver(sessionApp);
            TighteningProcess tightening = await tryScrewDriver(screw, _cancellationTokenSource, string.Empty);
            return tightening;
        }
        public void disconnect()
        {
            connection.Shutdown(SocketShutdown.Both);
            connection.Close();
        }
        public bool InRange()
        {
            return CleansFiledsTorqueAndAngle() == "0002";
        }
        public void connect()
        {
            connection = communicationScrewDriver.connectScrewDriver(eTypeDevices.Screw, eTypeConnection.Main);
            connectedScrewDriver = connection.Connected;
        }
        public bool isScrewDriverConnected()
        {
            return connectedScrewDriver;
        }
        public string CleansFiledsTorqueAndAngle()
        {
            if (connection.Connected)
            {
                communicationScrewDriver.sendCodesScrewDriver(connection, "00200001001000000000\0");
                string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
                Debug.WriteLine($"{DateTime.Now} - "  + response);
                return response;
            }
            return string.Empty;
        }
        public string ScrewingProgram_by_Model(eTypePrograms eTypePrograms, string programValue)
        {
            string ScrewingProgram = programValue == string.Empty ? getProgramScrewDriver(eTypePrograms): programValue;
            communicationScrewDriver.sendCodesScrewDriver(connection, "002300180010000000000" + ScrewingProgram + "\0");
            string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
            return response;
        }
        public string enableScrewdriver()
        {
            communicationScrewDriver.sendCodesScrewDriver(connection, "00200043000000000000\0");
            string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
            Debug.WriteLine($"{DateTime.Now} - "  + response);
            return response;
        }
        public string screwingSubscription()
        {
            //communicationScrewDriver.sendCodesScrewDriver(connection, @"00200043000000000000\0");
            communicationScrewDriver.sendCodesScrewDriver(connection, "00200060000000000000\0");
            string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
            Debug.WriteLine($"{DateTime.Now} - "  + response);
            return response;
        }

        public string getProgramScrewDriver(eTypePrograms eTypePrograms)
        {
            string program = string.Empty;
            switch (eTypePrograms)
            {
                case eTypePrograms.screwing:
                    program = sessionApp.programs_ScrewDriver.screwing;
                    break;
                case eTypePrograms.rescrewing:
                    program = sessionApp.programs_ScrewDriver.rescrewing;
                    break;
                case eTypePrograms.unscrewing:
                    program = sessionApp.programs_ScrewDriver.unscrewing;
                    break;
                case eTypePrograms.simulated:
                    program = sessionApp.programs_ScrewDriver.simulated;
                    break;
                default:
                    break;
            }
            return program;
        }
       

       


    }
}

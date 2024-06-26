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
        private CancellationTokenSource _cancellationTokenSource;
        CancellationTokenSource cancellationToken_ErgoArm;
        private bool connectedScrewDriver;
        SensorsIO sensorsIO;
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

        public async Task stopScrewDriverByOutPositionErgoArm(ErgoArm ergoArm, Screw screw,CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ergoArm.startReadPosition(screw);
                if (!sessionApp.positionErgoArm.InPositionReadyToProcess || !sessionApp.Sensors_M2.MaskatHousing)
                {
                    await Disable();
                }
                Thread.Sleep(500);
                if (sessionApp.positionErgoArm.endRead)
                {
                    break;
                }
            }
        }

        public async Task<ScrewingResult> ScrewingCompletedAsync(ErgoArm ergoArm,Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewingResult screwingResult = new ScrewingResult();

            stopScrewDriverByOutPositionErgoArm(ergoArm, screw, _cancellationTokenSource.Token);

            await Task.Delay(500);
            //Debug.WriteLine("Entre: ScrewingCompletedAsync.");

            bool timeoutReached = false;

            // Configuración del temporizador para medio minuto
            TimeSpan timeout = TimeSpan.FromSeconds(10000);
            Timer timer = new Timer((state) => { timeoutReached = true; }, null, timeout, TimeSpan.FromMilliseconds(-1));

            while (!screw.tighteningprocess.result && !timeoutReached)
            {
                try
                {
                   
                    //Debug.WriteLine($"Esperando atornillado.");
                    //#if DEBUG
                    //                    string response = "02310061001 0000    010000020003STLA_AUTO_L1S12          04                         050006001070000080000090100111120002501300031014000280150000241600000170039618000001900000202024-03-19:18:44:02212024-01-13:20:58:28222230000001525";
                    //#else
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
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
                        screw.tighteningprocess.status = response.Substring(107, 1) == "1" ? true : false;
                        screwingResult.status = screw.tighteningprocess.status;
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

        public async Task FactoryScrewing(ErgoArm ergoArm, Screw screw)
        {
            cancellationToken_ErgoArm = new CancellationTokenSource();

            while (!cancellationToken_ErgoArm.IsCancellationRequested)
            {
                Debug.WriteLine("Estoy en la factory"); ;
                ergoArm.startReadPositionRespectScrew(screw);

                if (sessionApp.positionErgoArm.InPositionReadyToProcess && sessionApp.Sensors_M2.MaskatHousing)
                {
                    //sessionApp.MessageOfProcess = $"Por favor, realice el atornillado número: {screw.id_screw}";
                    //Debug.WriteLine("La función principal se ha reanudado por que esta en posicion del tornillo y tiene la mascara");
                    //ScrewDriver screwdriver = new ScrewDriver(sessionApp);
                    //TighteningProcess tightening = await tryScrewDriver(screw, _cancellationTokenSource, string.Empty);
                }

                if (!sessionApp.positionErgoArm.InPositionReadyToProcess || !sessionApp.Sensors_M2.MaskatHousing)
                {
                    while (!sessionApp.positionErgoArm.InPositionReadyToProcess || !sessionApp.Sensors_M2.MaskatHousing)
                    {
                        await Disable();
                        //disconnect();
                        Task.Run(async () =>
                        {
                            await Task.Delay(500);
                        }).Wait();
                        ergoArm.startReadPositionRespectScrew(screw);

                        if (!sessionApp.positionErgoArm.InPositionReadyToProcess && sessionApp.Sensors_M2.MaskatHousing)
                        {
                            sessionApp.MessageOfProcess = "Por favor, posicione el brazo ergonomico en el tornillo.";
                            Debug.WriteLine("Por favor, posiciones el brazo ergonomico del tornillo."); ;
                        }
                        if (!sessionApp.Sensors_M2.MaskatHousing && sessionApp.positionErgoArm.InPositionReadyToProcess)
                        {
                            sessionApp.MessageOfProcess = "Por favor, vuelva a colocar la máscara sobre el housing.";
                            Debug.WriteLine("La función principal se ha detenido por que no tiene la mascara");
                        }
                        if (!sessionApp.positionErgoArm.InPositionReadyToProcess && !sessionApp.Sensors_M2.MaskatHousing)
                        {
                            sessionApp.MessageOfProcess = "Por favor, vuelva a colocar la máscara sobre el housing y posicione el brazo ergonomico en el tornillo.";
                            Debug.WriteLine("La función principal se ha detenido por que no tiene la mascara ni esta en la posicion el ergoarm");
                        }
                    }
                }
            }
        }
        public async Task<ScrewingResult> Screwing(ErgoArm ergoArm,Screw screw, CancellationTokenSource _cancellationTokenSource, string programValue = "")
        {
            ScrewingResult result;
            //sessionApp.messageTorque = "...";
            screw.tighteningprocess.result = false;
            await Task.Delay(500);
            connect();
            if (isConnected())
            {
                if (isStartScredriver())
                {
                    Eneable();
                    if (Program_by_Model(eTypePrograms.screwing, programValue) == "0005")
                    {
                        if (await Subscription() == "0005")
                        {
                            sessionApp.messageTorque = "Por favor proceda a atornillar.";
                            Debug.WriteLine("Por favor proceda a atornillar.");
                            result = await ScrewingCompletedAsync(ergoArm ,screw, _cancellationTokenSource);
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


        public async Task<ScrewingResult> Rescrewing(ErgoArm ergoArm,Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewingResult result;
            //sessionApp.messageTorque = "...";
            screw.tighteningprocess.result = false;
            await Task.Delay(500);
            connect();
            if (isConnected())
            {
                if (isStartScredriver())
                {
                    Eneable();
                    if (Program_by_Model(eTypePrograms.rescrewing, string.Empty) == "0005")
                    {
                        if (await Subscription() == "0005")
                        {
                            sessionApp.messageTorque = "Por favor proceda a atornillar.";
                            Debug.WriteLine("Por favor proceda a atornillar.");
                            result = await ScrewingCompletedAsync(ergoArm,screw, _cancellationTokenSource);
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
        public async Task<ScrewingResult> Unscrewing(ErgoArm ergoArm,Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewingResult result;
            sensorsIO = new SensorsIO(sessionApp);
            _cancellationTokenSource = new CancellationTokenSource();
            //sessionApp.messageTorque = "...";
            Debug.WriteLine("***** Entre: Unscrewing.");
            Debug.WriteLine($"{DateTime.Now} - " + $"Entramos en desatornillar el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
            sessionApp.MessageOfProcessDebug = "***** Entre: Unscrewing.";
            sessionApp.MessageOfProcessDebug = $"Entramos en desatornillar el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";

            Debug.WriteLine($"{DateTime.Now} - " + $"Salimos de la validacion de la mascara o el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
            sessionApp.MessageOfProcessDebug = $"Salimos de la validacion de la mascara o el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";
            screw.tighteningprocess.result = false;
            await Task.Delay(500);
            Debug.WriteLine($"{DateTime.Now} - " + $"Procedemos a conectar el atornillador sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
            sessionApp.MessageOfProcessDebug = $"Procedemos a conectar el atornillador sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";
            connect();
            if (isConnected())
            {
                if (isStartScredriver())
                {
                    Eneable();
                    if (Program_by_Model(eTypePrograms.unscrewing, string.Empty) == "0005")
                    {
                        if (await Subscription() == "0005")
                        {
                            sessionApp.messageTorque = "Por favor proceda a atornillar.";
                            Debug.WriteLine("Por favor proceda a atornillar.");
                            result = await ScrewingCompletedAsync(ergoArm,screw, _cancellationTokenSource);
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
        public async Task<TighteningProcess> tryScrewDriver(ErgoArm ergoArm,Screw screw, CancellationTokenSource _cancellationTokenSource, string programValue)
        {
            sensorsIO = new SensorsIO(sessionApp);
            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                Debug.WriteLine("***** Entre: tryScrewDriver.");
                Debug.WriteLine($"{DateTime.Now} - " + $"Entramos a atornillar el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
                sessionApp.MessageOfProcessDebug = "***** Entre: tryScrewDriver.";
                sessionApp.MessageOfProcessDebug = $"Entramos a atornillar el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";


                sessionApp.MessageOfProcessDebug = $"Salimos de la validacion de la mascara o el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ";
                Debug.WriteLine($"{DateTime.Now} - " + $"Salimos de la validacion de la mascara o el sensor de la mascara es: { sensorsIO.MaskOnHousing() } ");
                ScrewingResult result = await Screwing(ergoArm,screw, _cancellationTokenSource, programValue);
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

        public async Task<TighteningProcess> FirstTighteningAttempt(ErgoArm ergoArm,Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewDriver screwdriver = new ScrewDriver(sessionApp);
            TighteningProcess tightening = await tryScrewDriver( ergoArm,screw, _cancellationTokenSource, string.Empty);
            return tightening;
        }
        public async Task<TighteningProcess> SecondTighteningAttempt(ErgoArm ergoArm,Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewDriver screwdriver = new ScrewDriver(sessionApp);
            TighteningProcess tightening = await tryScrewDriver(ergoArm,screw, _cancellationTokenSource, string.Empty);
            return tightening;
        }
        public async Task<TighteningProcess> ThirdTighteningAttempt(ErgoArm ergoArm,Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewDriver screwdriver = new ScrewDriver(sessionApp);
            TighteningProcess tightening = await tryScrewDriver(ergoArm,screw, _cancellationTokenSource, string.Empty);
            return tightening;
        }
        public void disconnect()
        {
            sessionApp.MessageOfProcessDebug = $"Desconectamos el atornillador - Sesor:{sessionApp.Sensors_M2.MaskatHousing}";
            if (connection != null)
            {

                connection.Shutdown(SocketShutdown.Both);
                connection.Close();
            }
        }
        public bool isStartScredriver()
        {
            sessionApp.MessageOfProcessDebug = $"Esta en rango 0002 - Sesor:{sessionApp.Sensors_M2.MaskatHousing}";
            return startScrewdriver() == "0002";
        }
        public async void connect()
        {
            sessionApp.MessageOfProcessDebug = $"Conectamos atornillador - Sesor:{sessionApp.Sensors_M2.MaskatHousing}";
            connection = communicationScrewDriver.connectScrewDriver(eTypeDevices.Screw, eTypeConnection.Main);
            connectedScrewDriver = connection.Connected;
        }
        public bool isConnected()
        {
            return connectedScrewDriver;
        }
        /// <summary>
        /// //Aplication Communicacion start
        /// </summary>
        /// <param MID="0001"></param>
        /// <returns MID="0002">OK</returns>
        public string startScrewdriver()
        {
            if (connection.Connected)
            {
                communicationScrewDriver.sendCodesScrewDriver(connection, "00200001001000000000\0");
                string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
                Debug.WriteLine($"{DateTime.Now} - " + response);
                return response;
            }
            return string.Empty;
        }
        /// <summary>
        /// //Select Parameter set 
        /// </summary>
        /// <param MID="0018"></param>
        /// <returns MID="0005">OK</returns>

        public string Program_by_Model(eTypePrograms eTypePrograms, string programValue)
        {
            string ScrewingProgram = programValue == string.Empty ? getProgramScrewDriver(eTypePrograms) : programValue;
            communicationScrewDriver.sendCodesScrewDriver(connection, "002300180010000000000" + ScrewingProgram + "\0");
            string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
            return response;
        }
        /// <summary>
        /// //Eneable tool 
        /// </summary>
        /// <param MID="0043"></param>
        /// <returns MID="0005">OK</returns>
        public async Task<string> Eneable()
        {
            sessionApp.MessageOfProcessDebug = $"Habilitamos el atornillador - Sesor:{sessionApp.Sensors_M2.MaskatHousing}";
            communicationScrewDriver.sendCodesScrewDriver(connection, "00200043000000000000\0");
            string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
            Debug.WriteLine($"{DateTime.Now} - " + response);
            return response;
        }

        /// <summary>
        /// //Disable tool 
        /// </summary>
        /// <param MID="0042"></param>
        /// <returns MID="0005">OK</returns>
        public async Task<string> Disable()
        {
            if (connection != null)
            {
                if (connection.Connected)
                {
                    sessionApp.MessageOfProcessDebug = $"Habilitamos el atornillador - Sesor:{sessionApp.Sensors_M2.MaskatHousing}";
                    communicationScrewDriver.sendCodesScrewDriver(connection, "00200042000000000000\0");
                    string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
                    Debug.WriteLine($"{DateTime.Now} - " + response);
                    return response;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// //Last tightening  result data subscribe
        /// </summary>
        /// <param MID="0060"></param>
        /// <returns MID="0005">OK</returns>
        public async Task<string> Subscription()
        {
            sessionApp.MessageOfProcessDebug = $"Suscribimos el atornillador - Sesor:{sessionApp.Sensors_M2.MaskatHousing}";

            if (!sessionApp.positionErgoArm.InPositionReadyToProcess || !sessionApp.Sensors_M2.MaskatHousing)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(500);
                }).Wait();
                sessionApp.MessageOfProcessDebug = $"Esperamos a que coloquen la mascara o el brazo de nuevo";
            }
            communicationScrewDriver.sendCodesScrewDriver(connection, "00200060000000000000\0");
            string response = communicationScrewDriver.responseScrewDriver(connection, 4, 4);
            Debug.WriteLine($"{DateTime.Now} - " + response);
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


        public async Task CheckSensorAndWait(Func<bool> sensorCheck, string debugMessage)
        {
            if (!sensorCheck())
            {
                Debug.WriteLine($"{DateTime.Now} - {debugMessage}");
                _cancellationTokenSource = new CancellationTokenSource();
                await sensorsIO.WaitingResponse(_cancellationTokenSource, sensorCheck);
            }
        }


    }
}

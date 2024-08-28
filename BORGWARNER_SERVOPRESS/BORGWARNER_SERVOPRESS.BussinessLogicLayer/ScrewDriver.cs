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
        //SensorsIO sensorsIO;
        SensorsIOGeneric sensorsIO;
        public ScrewDriver(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            communicationScrewDriver = new CommunicationScrewDriver(sessionApp);
            sessionApp.programs_ScrewDriver = communicationScrewDriver.getPrograms_ScrewDriver();
        }

        public async Task stopScrewDriverByOutPositionErgoArm(ErgoArm ergoArm, Screw screw, CancellationToken cancellationToken, eTypePrograms typePrograms)
        {
            bool isdisable = false;
            while (!cancellationToken.IsCancellationRequested)
            {
                ergoArm.startReadPositionRespectScrewGetOut(screw);
                if (!sessionApp.positionErgoArm.InPositionReadyToProcess || !sessionApp.IOSensorsGenerics.MaskatHousing)
                {
                    if (!sessionApp.IOSensorsGenerics.MaskatHousing && !sessionApp.positionErgoArm.InPositionReadyToProcess)
                    {
                        switch (typePrograms)
                        {
                            case eTypePrograms.screwing:
                                sessionApp.MessageOfProcess = "Se ha deshabilitado el atornillador.  Por favor, vuelva a colocar la máscara sobre el housing.";
                                break;
                            case eTypePrograms.rescrewing:
                                break;
                            case eTypePrograms.unscrewing:
                                sessionApp.MessageOfProcess = "Se ha deshabilitado el desatornillador.  Por favor, vuelva a colocar la máscara sobre el housing.";
                                break;
                            case eTypePrograms.simulated:
                                break;
                            default:
                                break;
                        }
                        
                    }
                    if (!sessionApp.IOSensorsGenerics.MaskatHousing && sessionApp.positionErgoArm.InPositionReadyToProcess)
                    {
                        switch (typePrograms)
                        {
                            case eTypePrograms.screwing:
                                sessionApp.MessageOfProcess = "Se ha deshabilitado el atornillador.  Por favor, vuelva a colocar la máscara sobre el housing.";
                                break;
                            case eTypePrograms.rescrewing:
                                break;
                            case eTypePrograms.unscrewing:
                                sessionApp.MessageOfProcess = "Por favor, vuelva a colocar la máscara sobre el housing. Si ya realizó el desatornillado despositalo en tornillos desechados.";
                                break;
                            case eTypePrograms.simulated:
                                break;
                            default:
                                break;
                        }
                        
                    }
                    if (sessionApp.IOSensorsGenerics.MaskatHousing && !sessionApp.positionErgoArm.InPositionReadyToProcess)
                    {
                        switch (typePrograms)
                        {
                            case eTypePrograms.screwing:
                                sessionApp.MessageOfProcess = "Se ha deshabilitado el atornillador.  Por favor, posicione el brazo ergonomico en el tornillo.";
                                break;
                            case eTypePrograms.rescrewing:
                                break;
                            case eTypePrograms.unscrewing:
                                sessionApp.MessageOfProcess = "Si ya realizó el desatornillado despositalo en tornillos desechados. De lo contrario por favor, posicione el brazo ergonomico en el tornillo y desatornille. ";
                                break;
                            case eTypePrograms.simulated:
                                break;
                            default:
                                break;
                        }
                        
                    }

                    if (!isdisable)
                    {   
                        await Disable();
                        isdisable = true;                   
                    }                    
                }
                else
                {
                    if (isdisable)
                    {
                        switch (typePrograms)
                        {
                            case eTypePrograms.screwing:
                                sessionApp.MessageOfProcess = "Se ha habilitado el atornillador.  Por favor, proceda a atornillar";
                                break;
                            case eTypePrograms.rescrewing:
                                break;
                            case eTypePrograms.unscrewing:
                                sessionApp.MessageOfProcess = "Por favor, proceda a desatornillar y colóquelo en desposito de tornillos desechados.";
                                break;
                            case eTypePrograms.simulated:
                                break;
                            default:
                                break;
                        }
                        
                        await Eneable();                        
                        isdisable = false;                        
                    }
                }

                if (sessionApp.positionErgoArm.endRead)
                {
                    break;
                }
            }
        }
        public async Task restoreConectionWithScrewdriverAsync(string programValue, eTypePrograms typePrograms)
        {
            connect();
            if (isConnected())
            {
                if (isStartScredriver())
                {
                    await Eneable();
                    if (Program_by_Model(typePrograms, programValue) == "0005")
                    {
                        if (await Subscription() == "0005")
                        {

                        }
                    }
                }
            }
        }
        public async Task<ScrewingResult> ScrewingCompletedAsync(ErgoArm ergoArm, Screw screw, CancellationTokenSource _cancellationTokenSource, string programValue)
        {
            ScrewingResult screwingResult = new ScrewingResult();
          

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
                    if (connection == null || !isConnected())
                    {
                       await restoreConectionWithScrewdriverAsync(programValue, eTypePrograms.screwing);
                        Debug.WriteLine($"{DateTime.Now} - Reconecta con Estrategia de atornillado");
                    }

                    //Debug.WriteLine($"Esperando atornillado.");
                    //#if DEBUG
                    //                    string response = "02310061001 0000    010000020003STLA_AUTO_L1S12          04                         050006001070000080000090100111120002501300031014000280150000241600000170039618000001900000202024-03-19:18:44:02212024-01-13:20:58:28222230000001525";
                    //#else
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    string response = await communicationScrewDriver.ResponseScrewDriverAsync(connection);
                    //#endif

                    screw.tighteningprocess.resultResponse = string.IsNullOrEmpty(response) ? string.Empty : response.Substring(4, 4);

                    if (string.IsNullOrEmpty(response) || screw.tighteningprocess.resultResponse == "0005")
                    {                        
                        //Debug.WriteLine("La respuesta del controlador de tornillo está vacía.");
                        continue; // Vuelve al inicio del bucle y solicita la respuesta nuevamente
                        
                    }


                    screw.tighteningprocess.result = screw.tighteningprocess.resultResponse == "0061";

                    Debug.WriteLine($"{DateTime.Now} - " + response);
                    if (screw.tighteningprocess.result)
                    {
                        Debug.WriteLine("El atornillado fue EXITOSO.");
                        sessionApp.positionErgoArm.endRead = true;
                        screw.tighteningprocess.id = response.Substring(221, 10);
                        screw.tighteningprocess.Torque = response.Substring(142, 4);
                        screw.tighteningprocess.Angle = response.Substring(170, 4);
                        screw.tighteningprocess.status = response.Substring(107, 1) == "1" ? true : false;
                        screwingResult.status = screw.tighteningprocess.status;
                        timeoutReached = false;
                        break; // Salir del bucle cuando se reciba una respuesta satisfactoria
                    }
                    else
                    {
                        Debug.WriteLine("El atornillado FALLO.");
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

        public async Task<ScrewingResult> UnscrewingCompletedAsync(ErgoArm ergoArm, Screw screw, CancellationTokenSource _cancellationTokenSource, string programValue)
        {
            ScrewingResult screwingResult = new ScrewingResult();


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
                    if (connection == null || !isConnected())
                    {
                        await restoreConectionWithScrewdriverAsync(programValue, eTypePrograms.unscrewing);
                        Debug.WriteLine($"{DateTime.Now} - Reconecta con Estrategia de Desatornillado");
                    }
                                        
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    string response = await communicationScrewDriver.ResponseScrewDriverAsync(connection);
                    
                    screw.tighteningprocess.resultResponse = string.IsNullOrEmpty(response) ? string.Empty : response.Substring(4, 4);
                                       
                    if (sessionApp.IOSensorsGenerics.Scrap_presence)
                    {
                        screw.tighteningprocess.result = true;
                        sessionApp.positionErgoArm.endRead = true;                    
                        screwingResult.status = true;
                        timeoutReached = false;
                        break; // Salir del bucle cuando se reciba una respuesta satisfactoria
                    }
                    continue;
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
        public async Task<ScrewingResult> Screwing(ErgoArm ergoArm, Screw screw, CancellationTokenSource _cancellationTokenSource, string programValue = "")
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
                    await Eneable();
                    if (Program_by_Model(eTypePrograms.screwing, programValue) == "0005")
                    {
                        if (await Subscription() == "0005")
                        {
                            Task.Run(async () =>
                            {
                                stopScrewDriverByOutPositionErgoArm(ergoArm, screw, _cancellationTokenSource.Token,eTypePrograms.screwing);
                            });

                            //sessionApp.messageTorque = "Por favor proceda a atornillar.";
                            Debug.WriteLine("Por favor proceda a atornillar.");
                            result = await ScrewingCompletedAsync(ergoArm, screw, _cancellationTokenSource, programValue);
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


        public async Task<ScrewingResult> Rescrewing(ErgoArm ergoArm, Screw screw, CancellationTokenSource _cancellationTokenSource)
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
                            result = await ScrewingCompletedAsync(ergoArm, screw, _cancellationTokenSource, string.Empty);
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
        public async Task<ScrewingResult> Unscrewing(ErgoArm ergoArm, Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewingResult result;
            sensorsIO = new SensorsIOGeneric(sessionApp);
            _cancellationTokenSource = new CancellationTokenSource();          
            screw.tighteningprocess.result = false;
          
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
                            Task.Run(async () =>
                            {
                                stopScrewDriverByOutPositionErgoArm(ergoArm, screw, _cancellationTokenSource.Token,eTypePrograms.unscrewing);
                            });

                            sessionApp.messageTorque = "Por favor proceda a desatornillar.";
                            Debug.WriteLine("Por favor proceda a desatornillar.");
                            result = await UnscrewingCompletedAsync(ergoArm, screw, _cancellationTokenSource,string.Empty);
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
        public async Task<TighteningProcess> tryScrewDriver(ErgoArm ergoArm, Screw screw, CancellationTokenSource _cancellationTokenSource, string programValue)
        {
            sensorsIO = new SensorsIOGeneric(sessionApp);
            _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                ScrewingResult result = await Screwing(ergoArm, screw, _cancellationTokenSource, programValue);
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

        public async Task<TighteningProcess> FirstTighteningAttempt(ErgoArm ergoArm, Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewDriver screwdriver = new ScrewDriver(sessionApp);
            TighteningProcess tightening = await tryScrewDriver(ergoArm, screw, _cancellationTokenSource, string.Empty);
            if (tightening != null)
            {
                tightening.Attempt = 1;
            }
            return tightening;
        }
        public async Task<TighteningProcess> SecondTighteningAttempt(ErgoArm ergoArm, Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewDriver screwdriver = new ScrewDriver(sessionApp);
            TighteningProcess tightening = await tryScrewDriver(ergoArm, screw, _cancellationTokenSource, string.Empty);
            if (tightening != null)
            {
                tightening.Attempt = 2;
            }
            return tightening;
        }
        public async Task<TighteningProcess> ThirdTighteningAttempt(ErgoArm ergoArm, Screw screw, CancellationTokenSource _cancellationTokenSource)
        {
            ScrewDriver screwdriver = new ScrewDriver(sessionApp);
            TighteningProcess tightening = await tryScrewDriver(ergoArm, screw, _cancellationTokenSource, string.Empty);
            if (tightening != null)
            {
                tightening.Attempt = 3;
            }
            return tightening;
        }
        public void disconnect()
        {
            sessionApp.MessageOfProcessDebug = $"Desconectamos el atornillador - Sesor:{sessionApp.IOSensorsGenerics.MaskatHousing}";
            if (connection != null)
            {

                connection.Shutdown(SocketShutdown.Both);
                connection.Close();
            }
        }
        public bool isStartScredriver()
        {
            sessionApp.MessageOfProcessDebug = $"Esta en rango 0002 - Sesor:{sessionApp.IOSensorsGenerics.MaskatHousing}";
            return startScrewdriver() == "0002";
        }
        public async void connect()
        {
            sessionApp.MessageOfProcessDebug = $"Conectamos atornillador - Sesor:{sessionApp.IOSensorsGenerics.MaskatHousing}";
            connection = communicationScrewDriver.connectScrewDriver(eTypeDevices.Screw, eTypeConnection.Main);
            connectedScrewDriver = connection.Connected;
        }
        public bool isConnected()
        {
            return connection.Connected;
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
            Debug.WriteLine($"{DateTime.Now} - 002300180010000000000 [ " + ScrewingProgram + " ]\0");
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
            if (connection == null || !isConnected())
            {
                connect();
            }
            if (connection != null)
            {
                if (connection.Connected)
                {
                    //sessionApp.MessageOfProcessDebug = $" Habilitamos el atornillador - Sesor:{sessionApp.Sensors_M2.MaskatHousing}";
                    //sessionApp.MessageOfProcess = "Se ha habilitado el atornillador. Por favor, proceda a atornillar";
                    communicationScrewDriver.sendCodesScrewDriver(connection, "00200043000000000000\0");
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// //Disable tool 
        /// </summary>
        /// <param MID="0042"></param>
        /// <returns MID="0005">OK</returns>
        public async Task<string> Disable()
        {
            if(connection == null || !isConnected())
            {
                connect();
            }
            if (connection != null)
            {
                if (connection.Connected)
                {
                    //sessionApp.MessageOfProcessDebug = $"Desabilitamos el atornillador - Sesor:{sessionApp.Sensors_M2.MaskatHousing}";
                    //sessionApp.MessageOfProcess = "Se ha deshabilitado el atornillador. Por favor, posicione el brazo ergonomico en el tornillo.";
                    communicationScrewDriver.sendCodesScrewDriver(connection, "00200042000000000000\0");                    
                    return string.Empty;
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
            sessionApp.MessageOfProcessDebug = $"Suscribimos el atornillador - Sesor:{sessionApp.IOSensorsGenerics.MaskatHousing}";

            if (!sessionApp.positionErgoArm.InPositionReadyToProcess || !sessionApp.IOSensorsGenerics.MaskatHousing)
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

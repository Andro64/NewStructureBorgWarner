using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BORGWARNER_SERVOPRESS.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManualWindow : Window
    {
        private SessionApp sessionApp;        
        private PageManager pageManager;
        private ViewManual viewManual;
        List<string> controlNames;
        
        private TryDevices tryDevices;
        
        private CancellationTokenSource cancelllationToken_Brushes_Sensor = new CancellationTokenSource();
        private SolidColorBrush ellipseBrush = new SolidColorBrush(Colors.Black);
        private bool isRunningBrushes = false;

        public ManualWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            InitializeComponent();
            initialize();
        }

        public void initialize()
        {
            viewManual = new ViewManual(sessionApp);
            DataContext = viewManual;
            pageManager = new PageManager(this);
            tryDevices = new TryDevices(sessionApp);

            viewManual.ShowData();
            viewManual.ShowDate();
            pageManager.IsReadOnlyControls(new List<string>() { "serialCode", "Screwdriver_Torque", "Screwdriver_Angle", "Encoder1", "Encoder2" });
            
        }             
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

       
        private void settings_option_btn_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow(sessionApp).ShowDialog();
            this.Close();
        }

        private void Btn_exit_click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(sessionApp);
            loginWindow.Show();
            this.Close();
        }

        #region Menu
        private void mn_btn_run_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(sessionApp).ShowDialog();
            this.Close();
        }

        private void mn_btn_fis_Click(object sender, RoutedEventArgs e)
        {
            new FISWindow(sessionApp).ShowDialog();
            this.Close();
        }

        private void mn_btn_history_Click(object sender, RoutedEventArgs e)
        {
            new RunHistoryWindow(sessionApp).ShowDialog();
            this.Close();
        }

        private void mn_btn_modelos_screw_Click(object sender, RoutedEventArgs e)
        {
            new ModelsScrewWindow(sessionApp).ShowDialog();
            this.Close();
        }

        private void mn_btn_manual_Click(object sender, RoutedEventArgs e)
        {
            new ManualWindow(sessionApp).ShowDialog();
            this.Close();
        }

        private void mn_btn_positions_Click(object sender, RoutedEventArgs e)
        {
            new PositionScrewWindow(sessionApp).ShowDialog();
            this.Close();
        }

        private void users_option_btn_Click(object sender, RoutedEventArgs e)
        {
            new UsersWindow(sessionApp).ShowDialog();
            this.Close();
        }

        private void home_option_btn_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(sessionApp).ShowDialog();
            this.Close();
        }
        #endregion

        private void StartCycle_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WorkStation_Manual_Type1 workStation_Manual_Type1 = new WorkStation_Manual_Type1(sessionApp);
                //workStation_Manual_Type1.start();
                 workStation_Manual_Type1.MensajesPantalla();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\nSource: " + ex.Source + "\nInner: " + ex.InnerException, "Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StopCycle_btn_Click(object sender, RoutedEventArgs e)
        {
            pageManager.DisableControls(controlNames);            
            MessageBox.Show("Cerrando ciclos...");
        }

        private void Screw_Scrap_Click(object sender, RoutedEventArgs e)
        {
            pageManager.HideControls(controlNames);
        }

        private void showMenu(string profile)
        {
           
        }


        #region CARD1
        private bool isPressed_Grn = false;
        private void Card1_ouput0_Click_1(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M1 = new IOCardType_M1();

            if (!isPressed_Grn)
            {
                sessionApp.Sensors_M1.Opto_Grn = true;
                
                Card1_ouput0.Style = FindResource("GreenButton") as Style;                                
            }
            else
            {
                sessionApp.Sensors_M1.Opto_Grn = false;                
                Card1_ouput0.Style = FindResource("BaseButton") as Style;                                
            }

            tryDevices.TrySendDataSensorsM1();
            isPressed_Grn = !isPressed_Grn; // Invierte el estado del botón

        }

        private bool isPressed_Yllw = false;
        private void Card1_ouput1_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M1 = new IOCardType_M1();
            if (!isPressed_Yllw)
            {
                sessionApp.Sensors_M1.Opto_Yllw = true;

                Card1_ouput1.Style = FindResource("YellowButton") as Style;
            }
            else
            {
                sessionApp.Sensors_M1.Opto_Yllw = false;
                Card1_ouput1.Style = FindResource("BaseButton") as Style;
            }

            tryDevices.TrySendDataSensorsM1();
            isPressed_Yllw = !isPressed_Yllw; // Invierte el estado del botón
        }

        private bool isPressed_Red = false;
        private void Card1_ouput2_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M1 = new IOCardType_M1();
            if (!isPressed_Red)
            {
                sessionApp.Sensors_M1.Opto_Red = true;

                Card1_ouput2.Style = FindResource("RedButton") as Style;
            }
            else
            {
                sessionApp.Sensors_M1.Opto_Red = false;
                Card1_ouput2.Style = FindResource("BaseButton") as Style;
            }

            tryDevices.TrySendDataSensorsM1();
            isPressed_Red = !isPressed_Red; // Invierte el estado del botón
        }
        private void Card1_ouput3_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M1 = new IOCardType_M1();
            sessionApp.Sensors_M1.Reset_Signal = true;
            tryDevices.TrySendDataSensorsM1();
        }
        private void Card1_ouput4_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M1 = new IOCardType_M1();
            sessionApp.Sensors_M1.K4 = true;
            tryDevices.TrySendDataSensorsM1();
        }
        private void Card1_ouput5_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M1 = new IOCardType_M1();
            sessionApp.Sensors_M1.K5 = true;
            tryDevices.TrySendDataSensorsM1();
        }

        private void Card1_ouput6_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M1 = new IOCardType_M1();
            sessionApp.Sensors_M1.ScrewDispenser = true;
            tryDevices.TrySendDataSensorsM1();
        }      
        private void Card1_ouput7_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M1 = new IOCardType_M1();
            sessionApp.Sensors_M1.K7 = true;
            tryDevices.TrySendDataSensorsM1();
        }
        #endregion
        #region CARD2
        private void Card2_ouput0_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M2 = new IOCardType_M2();
            sessionApp.Sensors_M2.CylFixingExt = true;
            tryDevices.TrySendDataSensorsM2();
        }
        private void Card2_ouput1_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M2 = new IOCardType_M2();
            sessionApp.Sensors_M2.CylFixingRet = true;
            tryDevices.TrySendDataSensorsM2();
        }
        private void Card2_ouput2_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M2 = new IOCardType_M2();
            sessionApp.Sensors_M2.PalletStopperRet = true;
            tryDevices.TrySendDataSensorsM2();
        }
        private void Card2_ouput3_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M2 = new IOCardType_M2();
            sessionApp.Sensors_M2.Prestopper = true;
            tryDevices.TrySendDataSensorsM2();
        }
        private void Card2_ouput4_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M2 = new IOCardType_M2();
            sessionApp.Sensors_M2.K4 = true;
            tryDevices.TrySendDataSensorsM2();
        }
        private void Card2_ouput5_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M2 = new IOCardType_M2();
            sessionApp.Sensors_M2.K5 = true;
            tryDevices.TrySendDataSensorsM2();
        }
        private void Card2_ouput6_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M2 = new IOCardType_M2();
            sessionApp.Sensors_M2.K6 = true;
            tryDevices.TrySendDataSensorsM2();
        }
        private void Card2_ouput7_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M2 = new IOCardType_M2();
            sessionApp.Sensors_M2.K7 = true;
            tryDevices.TrySendDataSensorsM2();
        }
        #endregion
        #region CARD3
        private void Card3_ouput0_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M3 = new IOCardType_M3();
            sessionApp.Sensors_M3.STAvailable = true;
            tryDevices.TrySendDataSensorsM3();
        }
        private void Card3_ouput1_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M3 = new IOCardType_M3();
            sessionApp.Sensors_M3.LampCamaraR = true;
            tryDevices.TrySendDataSensorsM3();
        }

        private void Card3_ouput2_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M3 = new IOCardType_M3();
            sessionApp.Sensors_M3.LampCamaraL = true;
            tryDevices.TrySendDataSensorsM3();
        }
        private void Card3_ouput3_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M3 = new IOCardType_M3();
            sessionApp.Sensors_M3.K3 = true;
            tryDevices.TrySendDataSensorsM3();
        }
        private void Card3_ouput4_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M3 = new IOCardType_M3();
            sessionApp.Sensors_M3.K4 = true;
            tryDevices.TrySendDataSensorsM3();
        }
        private void Card3_ouput5_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M3 = new IOCardType_M3();
            sessionApp.Sensors_M3.K5 = true;
            tryDevices.TrySendDataSensorsM3();
        }
        private void Card3_ouput6_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M3 = new IOCardType_M3();
            sessionApp.Sensors_M3.ReleScrap = true;
            tryDevices.TrySendDataSensorsM3();
        }
        private void Card3_ouput7_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M3 = new IOCardType_M3();
            sessionApp.Sensors_M3.K7 = true;
            tryDevices.TrySendDataSensorsM3();
        }
        #endregion
        
        private void Screwdriver_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Camara_1_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tryDevices.TryVisionSystem();              
                Uri uri = new Uri(sessionApp.PathImageResultFromCamera);
                Camara_1_Manual_Image.Source = new BitmapImage(uri);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - Error" + ex.Message);
            }
        }

        private void TestScrewdriver_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private bool isPressed_CardIO = false;
        private void btnActivarCardIO_Click(object sender, RoutedEventArgs e)
        {
            if (!isPressed_CardIO)
            {                
                btnActivarCardIO.Content = "Desactivar Cards I/O";
                btnActivarCardIO.Style = FindResource("SelectedButton") as Style;
                tryDevices.TryStartSensor();
                BrushesSensors(3000);
            }
            else
            {                
                btnActivarCardIO.Content = "Activar Cards I/O";
                btnActivarCardIO.Style = FindResource("BaseButton") as Style;
                tryDevices.TryEndSensor();
                EndBrushesSensors();
            }

            isPressed_CardIO = !isPressed_CardIO; // Invierte el estado del botón
        }

        public void EndBrushesSensors()
        {
            cancelllationToken_Brushes_Sensor.Cancel();
            isRunningBrushes = false;
            Debug.WriteLine($"{DateTime.Now} - " + "Termine de pintar los sensores en la pantalla");
        }
     
        private async void BrushesSensors(int sensingTime)
        {
            isRunningBrushes = true;
            Debug.WriteLine($"{DateTime.Now} - " + "Pinta los sensores en la pantalla");
            while (isRunningBrushes)
            {
                // Obtener el color basado en la variable global (en este caso, un color aleatorio)
                Color randomColor = GetRandomColor();
                Debug.WriteLine($"{DateTime.Now} - " + $"Cambie de color .....................");
                // Actualizar el color de la elipse en el subproceso de la interfaz de usuario
                await Dispatcher.InvokeAsync(() =>
                {
                    ellipseBrush.Color = randomColor;
                    Card1_Input_0.Fill = ellipseBrush;

                    Debug.WriteLine($"{DateTime.Now} - " + $"**********Pintando *******************");
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M1.PA0 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card1_Input_0" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M1.OptoBtn ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card1_Input_1" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M1.PalletatPreStation ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card1_Input_2" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M1.PalletatStation ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card1_Input_3" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M1.PB0 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card1_Input_4" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M1.PB1 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card1_Input_5" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M1.MaskatHolder ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card1_Input_6" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M1.SecurityOK ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card1_Input_7" });

                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M2.HousingatPallet ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card2_Input_0" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M2.MaskatHousing ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card2_Input_1" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M2.UltraCapBoardReadytoScan ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card2_Input_2" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M2.PA3 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card2_Input_3" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M2.CylFixingExtd ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card2_Input_4" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M2.CylFixingRetd ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card2_Input_5" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M2.PB2 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card2_Input_6" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M2.PB3 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card2_Input_7" });                    

                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M3.ST02Available ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card3_Input_0" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M3.PA1 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card3_Input_1" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M3.PA2 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card3_Input_2" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M3.PA3 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card3_Input_3" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M3.PB0 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card3_Input_4" });                    
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M3.PB1 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card3_Input_5" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M3.ScrewPresence ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card3_Input_6" });
                    pageManager.ChangeBackgroundColor(sessionApp.Sensors_M3.PB3 ? Brushes.Blue : Brushes.LightBlue, new List<string> { "Card3_Input_7" });

                });

                //Este  es el tiempo del sensado
                await Task.Delay(sensingTime);
            }
        }

              

        private Random random = new Random();
        private Color GetRandomColor()
        {
            byte[] rgb = new byte[3];
            random.NextBytes(rgb);
            return Color.FromRgb(rgb[0], rgb[1], rgb[2]);
        }

        private void btnCambiaEstado_Click(object sender, RoutedEventArgs e)
        {
            sessionApp.Sensors_M2.PA3 = true;
            sessionApp.Sensors_M3.PB0 = true;

            Debug.WriteLine($"{DateTime.Now} - " + "Le di Click y ahora soy verdadero ");
            Debug.WriteLine($"{DateTime.Now} - " + $"sessionApp.Sensors_M2.PA3 = true: {sessionApp.Sensors_M2.PA3}");
            Debug.WriteLine($"{DateTime.Now} - " + $"sessionApp.Sensors_M3.PB0 = true: {sessionApp.Sensors_M3.PB0} ");
        }

        private void Scanner_Click(object sender, RoutedEventArgs e)
        {
            if (cboScanners.SelectedItem != null)
            {
                eTypeConnection typeConnection;
                if (Enum.TryParse(cboScanners.SelectedItem.ToString(), out typeConnection))
                {
                    serialCode.Text = tryDevices.TryScannerLON(typeConnection);
                }
                else
                {
                    MessageBox.Show("No existe el escaner.");
                }
            }
            else
            {                
                MessageBox.Show("No se ha seleccionado ningún elemento.");
            }
        }
    }
}

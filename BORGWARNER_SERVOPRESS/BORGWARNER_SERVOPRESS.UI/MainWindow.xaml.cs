using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BORGWARNER_SERVOPRESS.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SessionApp sessionApp;
        private PageManager pageManager;
        private ViewMain viewMain;
        List<string> controlNames;        
        private Workstation workstation;
        private bool isRequestedStopProcess;

        //Timer Varibles 
        private System.Timers.Timer timer;
        private TimeSpan elapsedTime;
        private DateTime startTime;
        private bool isRunning;

        public MainWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;

            WorkstationFactory.injectionSession(sessionApp);
            workstation = WorkstationFactory.CreateWorkstation();
            //MessageBox.Show("La estacion de trabajo es: " + workstation.Type);            
            
            InitializeComponent();
            initialize();
            InitializeTimer();

            workstation.CreateTextBoxRequested += BusinessLayer_CreateTextBoxRequested;
            workstation.RemoveTextBoxRequested += TextBoxRemoveContentGrid;
            Loaded += MainWindow_Loaded;

            
        }

        private void BusinessLayer_CreateTextBoxRequested(object sender, TextBoxInfoEventArgs e)
        {
            contentgrid.Dispatcher.Invoke(() =>
            {
                // Crear una nueva caja de texto
                var textBox = new Label
                {
                    Content = e.Text,
                    Width = 100,
                    Height = 30,
                    Style = (Style)Application.Current.Resources["RoundLabel"]
                    //Style = (Style)Application.Current.Resources["BaseIsFocused"]

                };

                // Establecer la posición de la caja de texto
                textBox.Margin = new Thickness(e.Position.X, e.Position.Y, 0, 0);

                Grid.SetRow(textBox, 1);
                Grid.SetColumn(textBox, 0);
                Grid.SetColumnSpan(textBox, 1); // O el número de columnas que ocupa tu imagen

                // Agregar la caja de texto al contenedor en la interfaz de usuario
                contentgrid.Children.Add(textBox);
            
            });
        }

        private void TextBoxRemoveContentGrid(object sender, EventArgs e)
        {            
            contentgrid.Dispatcher.Invoke(() =>
            {
                //Elimina todos menos el primero
                //var textBoxesToRemove = contentgrid.Children.OfType<Label>().Skip(1).ToList();
                //var textBoxesToRemove = contentgrid.Children.OfType<Label>().ToList();
                var textBoxesToRemove = contentgrid.Children.OfType<Label>().Where(lbl => lbl != lblRunCycle).ToList();
                foreach (var textBox in textBoxesToRemove)
                {
                    contentgrid.Children.Remove(textBox);
                }
            });
        }

        public void initialize()
        {
            viewMain = new ViewMain(sessionApp);
            DataContext = viewMain.GetModel();
            pageManager = new PageManager(this);
                        
            viewMain.ShowData();
            viewMain.ShowMessage();
            pageManager.IsReadOnlyControls(new List<string> { "from_fis_textblock", "to_fis_textblock", "txtHousing" , "txt_HVDC_BUSBAR", "txtHarness", "txtTopCover", "cycletime" });
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //workstation.RequestCreateTextBox("Hola", 100, 100);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
      

        private void settings_option_btn_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow(sessionApp).Show();
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
            new MainWindow(sessionApp).Show();
            this.Close();

        }

        private void mn_btn_fis_Click(object sender, RoutedEventArgs e)
        {
            new FISWindow(sessionApp).Show();
            this.Close();
        }

        private void mn_btn_history_Click(object sender, RoutedEventArgs e)
        {
            new RunHistoryWindow(sessionApp).Show();
            this.Close();
        }

        private void mn_btn_modelos_screw_Click(object sender, RoutedEventArgs e)
        {
            new ModelsScrewWindow(sessionApp).Show();
            this.Close();
        }

        private void mn_btn_manual_Click(object sender, RoutedEventArgs e)
        {
            new ManualWindow(sessionApp).Show();
            this.Close();
        }

        private void mn_btn_positions_Click(object sender, RoutedEventArgs e)
        {
            new PositionScrewWindow(sessionApp).Show();
            this.Close();
        }

        private void users_option_btn_Click(object sender, RoutedEventArgs e)
        {
            new UsersWindow(sessionApp).Show();
            this.Close();
        }

        private void home_option_btn_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(sessionApp).Show();
            this.Close();
        }
        #endregion

        public async void EneableControlsWhenEndTaskRun()
        {
            await Task.Run(async () =>
            {
                while (sessionApp.TaksRunExecuting)
                {                    
                    await Task.Delay(100); 
                }
            });
            ProcessFinished();
        }
        
        private void ProcessFinished()
        {
            StopChronometer();
            pageManager.EnableControls(new List<string> { "startCycle_btn", "mn_btn_run", "mn_btn_fis", "mn_btn_history", "mn_btn_modelos_screw", "mn_btn_manual", "mn_btn_positions" });
            pageManager.DisableControls(new List<string> { "stopCycle_btn" });
            pageManager.ChangeBackgroundColor(Brushes.Aqua, new List<string> { "Fis_enabled_display" });

            if (!isRequestedStopProcess)
            {
                pageManager.CleanControls(new List<string> { "from_fis_textblock", "to_fis_textblock", "txtHousing", "txt_HVDC_BUSBAR", "txtHarness", "txtTopCover", "cycletime" });
                StartCycle();
            }
        }
        
        private void StartCycle_btn_Click(object sender, RoutedEventArgs e)
        {
            StartCycle();
        }

        private void StartCycle()
        {
            isRequestedStopProcess = false;
            pageManager.DisableControls(new List<string> { "startCycle_btn", "mn_btn_run", "mn_btn_fis", "mn_btn_history", "mn_btn_modelos_screw", "mn_btn_manual", "mn_btn_positions" });
            pageManager.CleanControls(new List<string> { "from_fis_textblock", "to_fis_textblock", "txtHousing", "txt_HVDC_BUSBAR", "txtHarness", "txtTopCover", "cycletime" });
            pageManager.EnableControls(new List<string> { "stopCycle_btn" });
            pageManager.ChangeBackgroundColor(Brushes.Red, new List<string> { "Fis_enabled_display" });

            sessionApp.TaksRunExecuting = true;
            try
            {
                StartChronometer();
                workstation.StartProcess();
                EneableControlsWhenEndTaskRun();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\nSource: " + ex.Source + "\nInner: " + ex.InnerException, "Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StopCycle_btn_Click(object sender, RoutedEventArgs e)
        {
            isRequestedStopProcess = true;
            pageManager.ChangeBackgroundColor(Brushes.Aqua, new List<string> { "Fis_enabled_display" });
            workstation.CancelProcess();
            StopChronometer();
        }
        
        private void Screw_Scrap_Click(object sender, RoutedEventArgs e)
        {
            var textBoxesToRemove = contentgrid.Children.OfType<Label>().Skip(1).ToList();
            foreach (var textBox in textBoxesToRemove)
            {
                contentgrid.Children.Remove(textBox);
            }
        }

        private void showMenu(string profile)
        {
            
        }
        #region chronometer
        private void StartChronometer()
        {
            startTime = DateTime.Now;
            timer.Start();
            isRunning = true;
        }
        private void StopChronometer()
        {
            if (isRunning)
            {
                timer.Stop();
                isRunning = false;
            }
        }
        private void InitializeTimer()
        {
            timer = new System.Timers.Timer(10); // Intervalo de 10 milisegundos para centésimas
            timer.Elapsed += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - startTime;
            UpdateTimeLabel();
        }

        private void UpdateTimeLabel()
        {
            Dispatcher.Invoke(() =>
            {
                cycletime.Text = $"{elapsedTime.Hours:D2}:{elapsedTime.Minutes:D2}:{elapsedTime.Seconds:D2}.{elapsedTime.Milliseconds / 10:D2}";                
            });
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            timer.Stop(); // Detiene el temporizador al cerrar la ventana
        }

        private void ResetTimer()
        {
            elapsedTime = TimeSpan.Zero;
            UpdateTimeLabel();
        }
        #endregion

    }
}

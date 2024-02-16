using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
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

        public MainWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;

            WorkstationFactory.injectionSession(sessionApp);
            workstation = WorkstationFactory.CreateWorkstation();
            MessageBox.Show("La estacion de trabajo es: " + workstation.Type);            
            
            InitializeComponent();
            initialize();
        }

        public void initialize()
        {
            viewMain = new ViewMain(sessionApp);
            DataContext = viewMain.GetModel();
            pageManager = new PageManager(this);
                        
            viewMain.ShowData();
            viewMain.ShowDate();
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

        public async void EneableControlsWhenEndTaskRun()
        {
            await Task.Run(async () =>
            {
                while (sessionApp.TaksRunExecuting)
                {                    
                    await Task.Delay(100); 
                }
            });

            viewMain.StopTimer();
            pageManager.EnableControls(new List<string> { "startCycle_btn", "mn_btn_run", "mn_btn_fis", "mn_btn_history", "mn_btn_modelos_screw", "mn_btn_manual", "mn_btn_positions" });
            pageManager.DisableControls(new List<string> { "stopCycle_btn" });
            pageManager.ChangeBackgroundColor(Brushes.Aqua, new List<string> { "Fis_enabled_display" });
        }
        
        
        private void StartCycle_btn_Click(object sender, RoutedEventArgs e)
        {
            pageManager.DisableControls(new List<string> { "startCycle_btn", "mn_btn_run", "mn_btn_fis", "mn_btn_history", "mn_btn_modelos_screw", "mn_btn_manual", "mn_btn_positions" });
            pageManager.EnableControls(new List<string> { "stopCycle_btn" });
            pageManager.ChangeBackgroundColor(Brushes.Red, new List<string> { "Fis_enabled_display" });

            sessionApp.TaksRunExecuting = true;
            try
            {                
                viewMain.StartTimer();
                workstation.StartProcess();               
                EneableControlsWhenEndTaskRun();                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\nSource: " + ex.Source + "\nInner: " + ex.InnerException, "Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                
            }
        }

        private void StopCycle_btn_Click(object sender, RoutedEventArgs e)
        {
            viewMain.StopTimer();
            workstation.CancelProcess();          
        }

        private void Screw_Scrap_Click(object sender, RoutedEventArgs e)
        {
            pageManager.HideControls(controlNames);
        }

        private void showMenu(string profile)
        {

        }


    }
}

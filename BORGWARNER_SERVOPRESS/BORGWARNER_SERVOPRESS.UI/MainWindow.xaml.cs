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
        //private WorkStation_Manual_Type1 workStation_Manual_Type1;
        private Workstation workstation;

        public MainWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;

            //WorkstationFactory.injectionSession(sessionApp);
            //Workstation WSAutoType1 = WorkstationFactory.CreateWorkstation("AutoTipo1");
            //MessageBox.Show(WSAutoType1.Type);

            WorkstationFactory.injectionSession(sessionApp);
            Workstation workstation = WorkstationFactory.CreateWorkstation("ManualTipo1");
            MessageBox.Show(workstation.Type);
            workstation.StartProcess();

            //workStation_Manual_Type1 = new WorkStation_Manual_Type1(sessionApp);
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
                //workStation_Manual_Type1.StartProcess();
                //workStation_Manual_Type1.MensajesPantalla();
                EneableControlsWhenEndTaskRun();
                //Task.Run(() => WaitingEndTaskRun().GetAwaiter().GetResult());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\nSource: " + ex.Source + "\nInner: " + ex.InnerException, "Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                //pageManager.EnableControls(new List<string> { "mn_btn_run", "mn_btn_fis", "mn_btn_history", "mn_btn_modelos_screw", "mn_btn_manual", "mn_btn_positions" });
            }
        }

        private void StopCycle_btn_Click(object sender, RoutedEventArgs e)
        {
            viewMain.StopTimer();
            workstation.CancelProcess();
            //workStation_Manual_Type1.CancelProcess();
            //MessageBox.Show("Cerrando ciclos...");
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

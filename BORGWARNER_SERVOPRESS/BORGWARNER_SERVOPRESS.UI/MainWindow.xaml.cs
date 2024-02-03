using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BORGWARNER_SERVOPRESS.UI.Pages;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Threading;
using System.Diagnostics;

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
        public MainWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
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

            controlNames = new List<string> { "startCycle_btn", "export_btn", "mn_btn_positions", "positions_separator", "from_fis_textblock" };
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void home_option_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settings_option_btn_Click(object sender, RoutedEventArgs e)
        {

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
            new HistoryWindow(sessionApp).ShowDialog();
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

            pageManager.EnableControls(new List<string> { "mn_btn_run", "mn_btn_fis", "mn_btn_history", "mn_btn_modelos_screw", "mn_btn_manual", "mn_btn_positions" });
        }
        
        private void StartCycle_btn_Click(object sender, RoutedEventArgs e)
        {
            pageManager.DisableControls(new List<string> { "mn_btn_run", "mn_btn_fis", "mn_btn_history", "mn_btn_modelos_screw", "mn_btn_manual", "mn_btn_positions" });
            sessionApp.TaksRunExecuting = true;
            try
            {
                WorkStation_Manual_Type1 workStation_Manual_Type1 = new WorkStation_Manual_Type1(sessionApp);
                //workStation_Manual_Type1.start();
                workStation_Manual_Type1.MensajesPantalla();
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


    }
}

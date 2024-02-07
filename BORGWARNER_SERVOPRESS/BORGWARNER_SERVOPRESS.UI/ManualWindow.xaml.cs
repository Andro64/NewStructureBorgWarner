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

namespace BORGWARNER_SERVOPRESS.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ManualWindow : Window
    {
        private SessionApp sessionApp;        
        private PageManager pageManager;
        private ViewMain viewMain;
        List<string> controlNames;
        public ManualWindow(SessionApp _sessionApp)
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

            controlNames = new List<string> { "startCycle_btn",  "export_btn", "mn_btn_positions", "positions_separator", "from_fis_textblock" };           
        }

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void home_option_btn_Click(object sender, RoutedEventArgs e)
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

        private void Card3_ouput4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card3_ouput5_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card3_ouput7_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card3_ouput3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card1_ouput0_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Card1_ouput5_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card1_ouput6_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card1_ouput2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card1_ouput7_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card2_ouput0_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card2_ouput4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card2_ouput1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card2_ouput5_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card2_ouput2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card2_ouput6_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card2_ouput3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card2_ouput7_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card3_ouput0_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card3_ouput1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card3_ouput2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card3_ouput6_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card1_ouput1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Scanner_1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Scanner_2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Screwdriver_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Camara_1_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TestScrewdriver_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card1_ouput4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Card1_ouput3_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

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
using BORGWARNER_SERVOPRESS.DataModel.Views;

namespace BORGWARNER_SERVOPRESS.UI
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private SessionApp sessionApp;
        private ViewSettings viewSettings;
        private PageManager pageManager;        
        List<string> controlNames;
        public SettingsWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            InitializeComponent();
            initialize();
        }
        public void initialize()
        {
            viewSettings = new ViewSettings(sessionApp);            
            DataContext = viewSettings;
            pageManager = new PageManager(this);   
            
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


        private void cboPage_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {   
                (DataContext as ViewSettings)?.SelectComboPageCommand.Execute(comboBox.SelectedItem);                
            }
        }

        private void btnToAdd_Click(object sender, RoutedEventArgs e)
        {
            pageManager.CleanControls(new List<string> { "txtPartNumber", "txtSerial", "txtNamemodel", "txtDescription", "txtQuantityScrews" });
        }

        private void btnToCancel_Click(object sender, RoutedEventArgs e)
        {
            pageManager.CleanControls(new List<string> { "txtPartNumber", "txtSerial", "txtNamemodel", "txtDescription", "txtQuantityScrews" });
        }
    }
}

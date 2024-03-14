using BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views;
using BORGWARNER_SERVOPRESS.DataModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            var messageBoxService = new MessageBoxService();
            viewSettings = new ViewSettings(sessionApp, messageBoxService);            
            DataContext = viewSettings;
            pageManager = new PageManager(this);   
            
            

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


        private void cboPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {   
                (DataContext as ViewSettings)?.SelectComboPageCommand.Execute(comboBox.SelectedItem);                
            }
        }

        private void btnToClean_Click(object sender, RoutedEventArgs e)
        {
            pageManager.CleanControls(new List<string> { "txtPartNumber", "txtSerial", "txtNamemodel", "txtDescription", "txtQuantityScrews" });
            pageManager.EnableControls(new List<string> { "btnToSave" });
        }

        private void btnToCancel_Click(object sender, RoutedEventArgs e)
        {
            pageManager.CleanControls(new List<string> { "txtPartNumber", "txtSerial", "txtNamemodel", "txtDescription", "txtQuantityScrews" });
            pageManager.EnableControls(new List<string> { "btnToSave" });
        }

        private void cbo_Type_Workstation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {                
                (DataContext as ViewSettings)?.SelectComboTypeWorkstationCommand.Execute(comboBox.SelectedValue);                
            }
        }
    }
}

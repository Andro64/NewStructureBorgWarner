﻿using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views;
using BORGWARNER_SERVOPRESS.DataModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace BORGWARNER_SERVOPRESS.UI
{
    /// <summary>
    /// Interaction logic for FISWindow.xaml
    /// </summary>
    public partial class RunHistoryWindow : Window
    {
        private SessionApp sessionApp;        
        private PageManager pageManager;
        private ViewRunHistory ViewRunHistory;
        List<string> controlNames;
        public RunHistoryWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            InitializeComponent();
            initialize();
        }

        public void initialize()
        {
            var messageBoxService = new MessageBoxService();
            ViewRunHistory = new ViewRunHistory(sessionApp, messageBoxService);
            DataContext = ViewRunHistory;
            pageManager = new PageManager(this);

          

            controlNames = new List<string> { "startCycle_btn",  "export_btn", "mn_btn_positions", "positions_separator", "from_fis_textblock" };
           

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

        private void btnToCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnToAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cboPage_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                (DataContext as ViewRunHistory)?.SelectComboPageCommand.Execute(comboBox.SelectedItem);
            }
        }
       
    }
}

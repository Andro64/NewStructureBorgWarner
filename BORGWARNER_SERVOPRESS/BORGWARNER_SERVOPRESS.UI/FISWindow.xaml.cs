﻿using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Windows;

namespace BORGWARNER_SERVOPRESS.UI
{
    /// <summary>
    /// Interaction logic for FISWindow.xaml
    /// </summary>
    public partial class FISWindow : Window
    {
        private SessionApp sessionApp;        
        private PageManager pageManager;
        private ViewFIS viewFIS;
        List<string> controlNames;
        public FISWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            InitializeComponent();
            initialize();
        }

        public void initialize()
        {
            viewFIS = new ViewFIS(sessionApp);
            DataContext = viewFIS.GetModel();
            pageManager = new PageManager(this);

            viewFIS.ShowData();
            viewFIS.ShowMessage();
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
            MessageBox.Show("Cerrando ciclos...");
        }

        private void Screw_Scrap_Click(object sender, RoutedEventArgs e)
        {
            pageManager.HideControls(controlNames);
        }

        private void showMenu(string profile)
        {
            
        }

        private void btnSendToFIS_Click(object sender, RoutedEventArgs e)
        {
            TryDevices tryDevices = new TryDevices(sessionApp);

            if (!string.IsNullOrEmpty(txtBREQ.Text))
            {
                txtBCNF.Text = tryDevices.TryFIS_BREQToFIS(txtBREQ.Text);
            }
            if (!string.IsNullOrEmpty(txtBCMP.Text))
            {
                txtBACK.Text = tryDevices.TryFIS_BCMP(txtBCMP.Text);
            }
        }
    }
}

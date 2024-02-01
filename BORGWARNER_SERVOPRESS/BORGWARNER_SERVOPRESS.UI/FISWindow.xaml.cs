﻿using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
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
    /// Interaction logic for FISWindow.xaml
    /// </summary>
    public partial class FISWindow : Window
    {
        private SessionApp sessionApp;        
        private PageManager pageManager;
        private ViewMain viewMain;
        List<string> controlNames;
        public FISWindow(SessionApp _sessionApp)
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

            controlNames = new List<string> { "startCycle_btn",  "export_btn", "positions_btn", "positions_separator", "from_fis_textblock" };
            //pageManager.DisableControls(controlNames);
            //pageManager.HideControls(controlNames);

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

        private void run_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void fis_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void history_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void export_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void manual_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void positions_btn_Click(object sender, RoutedEventArgs e)
        {

        }

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

        
    }
}

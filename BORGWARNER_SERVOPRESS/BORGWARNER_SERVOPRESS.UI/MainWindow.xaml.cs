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
    public partial class MainWindow : Window
    {
        private SessionApp sessionApp;

        homeDashboard _homeDashboard;
        LoginWindow loginWindow;
        public MainWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            showMenu(sessionApp.user.profile);

            lblUsername.Content = sessionApp.user.userName;
            lblProfile.Content = sessionApp.user.profile;
        }

        private void home_option_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settings_option_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_exit_click(object sender, RoutedEventArgs e)
        {
            loginWindow = new LoginWindow(sessionApp);
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

        }

        private void StopCycle_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Screw_Scrap_Click(object sender, RoutedEventArgs e)
        {

        }

        private void showMenu(string profile)
        {
            if(profile == "Operario")
            {
                fis_btn.Visibility = Visibility.Hidden;
                history_btn.Visibility = Visibility.Hidden;
                export_btn.Visibility = Visibility.Hidden;
                manual_btn.Visibility = Visibility.Hidden;
                positions_btn.Visibility = Visibility.Hidden;
            }
        }

        
    }
}

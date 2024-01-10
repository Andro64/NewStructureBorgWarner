using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BORGWARNER_SERVOPRESS.UI
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SessionApp sessionApp;
        public MainWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UsersAdmin usersAdmin = new UsersAdmin(sessionApp);
                //bool blAccess = usersAdmin.authentication(txtUser.Text, txtPassword.Text);
                bool blAccess = usersAdmin.authenticationSP(txtUser.Text, txtPassword.Text);

                lblAccess.Content = blAccess ? "Acceso consedido" : "Acceso denegado";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\nSource: " + ex.Source + "\nInner: " + ex.InnerException, "Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnScrews_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SCREWS screws = new SCREWS(sessionApp);
                DataTable dtScrews = screws.getScrewsSP(int.Parse(txtPagination.Text), 2); //el 2 son los registros que trae                
                dtgScrews.ItemsSource = dtScrews.AsDataView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\nSource: " + ex.Source + "\nInner: " + ex.InnerException, "Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnInitialize_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Robot robot = new Robot(sessionApp);
                //bool blinitialize = robot.startConnectionRobot();                
                CtrlErgoArms ctrlErgoArms = new CtrlErgoArms(sessionApp);

                for (int i = 0; i < 8; i++)
                {
                    lblMessageScrew.Content = "Atronillando: Tronillo " + i.ToString();
                    ctrlErgoArms.Ejecutatorque();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message + "\nSource: " + ex.Source + "\nInner: " + ex.InnerException, "Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

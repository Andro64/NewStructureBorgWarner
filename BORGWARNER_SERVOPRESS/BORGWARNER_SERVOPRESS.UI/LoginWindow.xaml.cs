using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BORGWARNER_SERVOPRESS.UI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private SessionApp sessionApp;

        public LoginWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            UsersAdmin usersAdmin = new UsersAdmin(sessionApp);
            bool bAccess = usersAdmin.authentication(username_field.Text, password_field.Password);
            if (bAccess)
            {
                MainWindow mainWindow = new MainWindow(sessionApp);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("User or password invalid!");
            }
        }
    }
}

using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System.Windows;


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
            
            bool bAccess = new UserAdmin(sessionApp).Authentication(username_field.Text, password_field.Password);
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

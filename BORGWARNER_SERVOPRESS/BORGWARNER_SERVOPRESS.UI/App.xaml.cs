using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace BORGWARNER_SERVOPRESS.UI
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        { 
            SessionApp sessionApp = new SessionApp();
            sessionApp.connStr = ConfigurationManager.ConnectionStrings["conn_str"].ToString();            
            sessionApp.settings = new BussinessLogicLayer.Settings(sessionApp).getSettings();
            string occupation = ConfigurationManager.AppSettings["occupation"];
            LoginWindow _loginWindow = new LoginWindow(sessionApp);
            _loginWindow.Show();
            //MainWindow mainWindow = new MainWindow(sessionApp);
            //mainWindow.Title = "BORGWARNER SERVOPRENSA";
            //mainWindow.Show();
        }
    }
}

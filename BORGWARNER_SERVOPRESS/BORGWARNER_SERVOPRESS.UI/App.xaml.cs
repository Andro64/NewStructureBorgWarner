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
            //string occupation = ConfigurationManager.AppSettings["occupation"];
            //Obtiene la información de Appconfig
            SessionApp sessionApp = new SessionApp();
            sessionApp.connStr = ConfigurationManager.ConnectionStrings["conn_str"].ToString();
            
            //Obtiene la información de configuracion de la BD
            BussinessLogicLayer.Settings settingsGeneral = new BussinessLogicLayer.Settings(sessionApp);
            sessionApp.settings = settingsGeneral.getSettings();
            sessionApp.connectionsWorkStation = settingsGeneral.getConnections();
            
            //Inicial la ventana Main
            MainWindow mainWindow = new MainWindow(sessionApp);
            mainWindow.Title = "BORGWARNER SERVOPRENSA";
            mainWindow.Show();
        }
    }
}

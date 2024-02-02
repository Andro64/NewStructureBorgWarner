using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.IO;

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

            Assembly exec = Assembly.GetExecutingAssembly();
            string pathExec = exec.Location;
            sessionApp.PathDirectoryResourcesOfThisProyect = Path.GetDirectoryName(pathExec) + @"\Resources\";
            sessionApp.PathOperationalImages = sessionApp.PathDirectoryResourcesOfThisProyect + @"Operational_Images\";
            sessionApp.user = new User();

            //Inicial la ventana Login
            //LoginWindow _loginWindow = new LoginWindow(sessionApp);
            //_loginWindow.Show();

            //Inicial la ventana Main
            //MainWindow mainWindow = new MainWindow(sessionApp);
            //mainWindow.Title = "BORGWARNER SERVOPRENSA";
            //mainWindow.Show();


            //FISWindow fisWindow = new FISWindow(sessionApp);
            //fisWindow.Title = "BORGWARNER SERVOPRENSA";
            //fisWindow.Show();

            //HistoryWindow historyWindow = new HistoryWindow(sessionApp);
            //historyWindow.Title = "BORGWARNER SERVOPRENSA";
            //historyWindow.Show();

            ModelsScrewWindow modelsScrewWindow = new ModelsScrewWindow(sessionApp);
            modelsScrewWindow.Title = "BORGWARNER SERVOPRENSA";
            modelsScrewWindow.Show();
        }
    }
}

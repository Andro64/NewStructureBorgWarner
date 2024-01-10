using BORGWARNER_SERVOPRESS.DataModel;
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
            sessionApp.settings = new Settings()
            {
                adus_serial = int.Parse(ConfigurationManager.AppSettings["adus_serial"]),
                scanners = int.Parse(ConfigurationManager.AppSettings["scanners"]),
                camaras = int.Parse(ConfigurationManager.AppSettings["camaras"]),
                robots = int.Parse(ConfigurationManager.AppSettings["robots"]),
                screwdrivers = int.Parse(ConfigurationManager.AppSettings["screwdrivers"]),
                FISs = int.Parse(ConfigurationManager.AppSettings["ErgoArms"]),
                conexions = int.Parse(ConfigurationManager.AppSettings["conexions"]),
            };
            string occupation = ConfigurationManager.AppSettings["occupation"];
            MainWindow mainWindow = new MainWindow(sessionApp);
            mainWindow.Title = "BORGWARNER SERVOPRENSA";
            mainWindow.Show();
        }
    }
}

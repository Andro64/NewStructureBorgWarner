using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
            sessionApp.typeWorkstation = settingsGeneral.getTypeWorksatiton();
            sessionApp.settings = settingsGeneral.getSettings(sessionApp.typeWorkstation.id);
            sessionApp.connectionsWorkStation = settingsGeneral.getConnections(sessionApp.typeWorkstation.id);
            sessionApp.commandCamaras = settingsGeneral.getCommandCamera();

            if (sessionApp.settings.FirstOrDefault(x => x.setting.Equals("Path_LOG")) != null)
            {
                Logger.SetLogFilePath(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("Path_LOG")).valueSetting);
                
            }
            else
            {
                MessageBox.Show("La configuracion \"Path_LOG\" es requerida para el correcto funcionamiento de la aplicación");
            }
            if(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("GRID_Number_Reg_by_Page")) == null)
            {
                MessageBox.Show("La configuracion \"GRID_Number_Reg_by_Page\" es requerida para el correcto funcionamiento de la aplicación");
            }


            Logger.Instance.Log("Iniciando sistema");
            Debug.WriteLine("BORGWARNER_SERVOPRESS iniciado...");
            
            Assembly exec = Assembly.GetExecutingAssembly();
            string pathExec = exec.Location;
            sessionApp.PathDirectoryResourcesOfThisProyect = Path.GetDirectoryName(pathExec) + @"\Resources\";
            string pathWorkstationImage = settingsGeneral.GetImageFolderName(sessionApp.typeWorkstation.description);
            sessionApp.PathOperationalImages = sessionApp.PathDirectoryResourcesOfThisProyect + @"Operational_Images\" + pathWorkstationImage;
            sessionApp.user = new User();

            //Inicial la ventana Login
            LoginWindow _loginWindow = new LoginWindow(sessionApp);
            _loginWindow.Show();

            //Inicial la ventana Main
            //ManualWindow mainWindow = new ManualWindow(sessionApp);
            //mainWindow.Title = "BORGWARNER SERVOPRENSA";
            //mainWindow.Show();


            //FISWindow fisWindow = new FISWindow(sessionApp);
            //fisWindow.Title = "BORGWARNER SERVOPRENSA";
            //fisWindow.Show();

            //RunHistoryWindow RunHistoryWindow = new RunHistoryWindow(sessionApp);
            //RunHistoryWindow.Title = "BORGWARNER SERVOPRENSA";
            //RunHistoryWindow.Show();

            //ModelsScrewWindow modelsScrewWindow = new ModelsScrewWindow(sessionApp);
            //modelsScrewWindow.Title = "BORGWARNER SERVOPRENSA";
            //modelsScrewWindow.Show();

            //PositionScrewWindow positionScrewWindow = new PositionScrewWindow(sessionApp);
            //positionScrewWindow.Show();
        }
    }
}

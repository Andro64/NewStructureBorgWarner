using BORGWARNER_SERVOPRESS.BussinessLogicLayer;
using BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views;
using BORGWARNER_SERVOPRESS.DataModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BORGWARNER_SERVOPRESS.UI
{
    /// <summary>
    /// Interaction logic for FISWindow.xaml
    /// </summary>
    public partial class PositionScrewWindow : Window
    {
        private SessionApp sessionApp;
        private TryDevices tryDevices;
        private ViewPositionScrew ViewPositionScrew;
        private PageManager pageManager;        
        List<string> controlNames;
        public PositionScrewWindow(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            InitializeComponent();
            initialize();
        }
        public void initialize()
        {
            var messageBoxService = new MessageBoxService();
            ViewPositionScrew = new ViewPositionScrew(sessionApp, messageBoxService);
            tryDevices = new TryDevices(sessionApp);
            //Se carga el modelo
            DataContext = ViewPositionScrew;
            pageManager = new PageManager(this);          
            

        }        
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
               

        private void settings_option_btn_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow(sessionApp).Show();
            this.Close();
        }

        private void Btn_exit_click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(sessionApp);
            loginWindow.Show();
            this.Close();
        }
        #region Menu
        private void mn_btn_run_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(sessionApp).Show();
            this.Close();
        }

        private void mn_btn_fis_Click(object sender, RoutedEventArgs e)
        {
            new FISWindow(sessionApp).Show();
            this.Close();
        }

        private void mn_btn_history_Click(object sender, RoutedEventArgs e)
        {
            new RunHistoryWindow(sessionApp).Show();
            this.Close();
        }

        private void mn_btn_modelos_screw_Click(object sender, RoutedEventArgs e)
        {
            new ModelsScrewWindow(sessionApp).Show();
            this.Close();
        }

        private void mn_btn_manual_Click(object sender, RoutedEventArgs e)
        {
            new ManualWindow(sessionApp).Show();
            this.Close();
        }

        private void mn_btn_positions_Click(object sender, RoutedEventArgs e)
        {
            new PositionScrewWindow(sessionApp).Show();
            this.Close();
        }

        private void users_option_btn_Click(object sender, RoutedEventArgs e)
        {
            new UsersWindow(sessionApp).Show();
            this.Close();
        }

        private void home_option_btn_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(sessionApp).Show();
            this.Close();
        }
        #endregion


        private void cboPage_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {   
                (DataContext as ViewPositionScrew)?.SelectComboPageCommand.Execute(comboBox.SelectedItem);                
            }
        }

        private void btnToClean_Click(object sender, RoutedEventArgs e)
        {
            pageManager.CleanControls(new List<string> { "txtPartNumber", "txtSerial", "txtNamemodel", "txtDescription", "txtQuantityScrews" });
        }

        private void btnToCancel_Click(object sender, RoutedEventArgs e)
        {
            pageManager.CleanControls(new List<string> { "txtPartNumber", "txtSerial", "txtNamemodel", "txtDescription", "txtQuantityScrews" });
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Permite solo dígitos, el símbolo "-" (solo al inicio) y el símbolo "."
            if (!char.IsDigit(e.Text, e.Text.Length - 1) &&
                e.Text != "-" && e.Text != "." ||
                e.Text == "-" && textBox.Text.Contains("-") ||
                e.Text == "." && textBox.Text.Contains("."))
            {
                e.Handled = true;
            }

            // Permitir solo un "-" al inicio de la cadena
            if (e.Text == "-" && textBox.SelectionStart != 0)
            {
                e.Handled = true;
            }

            // Permitir solo un "." y asegurarse de que no haya más de un "." en la cadena
            if (e.Text == "." && (textBox.SelectionStart == 0 || textBox.Text.Contains(".")))
            {
                e.Handled = true;
            }
        }

        private bool isPressed_ErgoArm= false;
        private void btnActivar_ErgoArm_Click(object sender, RoutedEventArgs e)
        {
            if (!isPressed_ErgoArm)
            {
                btnActiveErgoArm.Content = "Desactivar ErgoArm";
                btnActiveErgoArm.Style = FindResource("SelectedButton") as Style;
                tryDevices.TryErgoArm();                
            }
            else
            {
                btnActiveErgoArm.Content = "Activar ErgoArm";
                btnActiveErgoArm.Style = FindResource("BaseButton") as Style;
                tryDevices.FinishTestErgoArm();                
            }

            isPressed_ErgoArm = !isPressed_ErgoArm; // Invierte el estado del botón
        }
    }
}

using BORGWARNER_SERVOPRESS.DataModel;
using System.Windows;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public interface IMessageBoxService
    {
        MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, eMessageBoxIcon icon);
    }
}

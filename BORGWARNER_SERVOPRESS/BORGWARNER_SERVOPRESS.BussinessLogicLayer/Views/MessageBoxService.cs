using BORGWARNER_SERVOPRESS.DataModel;
using System.Windows;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public class MessageBoxService : IMessageBoxService
    {
        public MessageBoxResult Show(string message, string caption, MessageBoxButton buttons, eMessageBoxIcon icon)
        {
            MessageBoxImage image;

            switch (icon)
            {
                case eMessageBoxIcon.Information:
                    image = MessageBoxImage.Information;
                    break;
                case eMessageBoxIcon.Question:
                    image = MessageBoxImage.Question;
                    break;
                case eMessageBoxIcon.Warning:
                    image = MessageBoxImage.Warning;
                    break;
                case eMessageBoxIcon.Error:
                    image = MessageBoxImage.Error;
                    break;
                default:
                    image = MessageBoxImage.None;
                    break;
            }

            return MessageBox.Show(message, caption, buttons, image);
        }
    }
}

using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.Views
{
    public class ViewModelsScrew
    {
        private ModelViewModelsScrew _modelViewModelsScrew;
        SessionApp sessionApp;
        public ViewModelsScrew(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            _modelViewModelsScrew = new ModelViewModelsScrew();
        }
        public ModelViewModelsScrew GetModel()
        {
            getDataModelScrew();
            return _modelViewModelsScrew;
        }
        public void getDataModelScrew()
        {
            CommunicationScrew communicationScrew = new CommunicationScrew(sessionApp);
            _modelViewModelsScrew.ModelsScrews =  communicationScrew.getModelsScrew();
        }
    }
}

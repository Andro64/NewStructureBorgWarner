using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel.Views
{
    public class ModelViewModelsScrew
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private string _profile;
        public string Profile
        {
            get { return _profile; }
            set { _profile = value; }
        }
        private List<ModelScrew> _modelsScrews;
        public List<ModelScrew> ModelsScrews
        {
            get { return _modelsScrews; }
            set { _modelsScrews = value; }
        }

    }
}

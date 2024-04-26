using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using BORGWARNER_SERVOPRESS.DataModel.Views;
using System.Collections.Generic;
using System.Linq;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class UserAdmin
    {
        private SessionApp sessionApp;

        public UserAdmin(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;            
        }
        public bool Authentication(string user, string password)
        {
            CommunicationUsers communicationUsers = new CommunicationUsers(sessionApp);
            
            List<ModelViewUsers> lstUsers  =  communicationUsers.ValidateUsers(user, password);
            if (lstUsers.Count > 0)
            {
                ModelViewUsers modeluser = lstUsers.FirstOrDefault();
                sessionApp.user.userName = modeluser.username;
                sessionApp.user.name = modeluser.name;
                sessionApp.user.lastName = modeluser.lastName;
                sessionApp.user.profile = modeluser.id_profile;
                sessionApp.user.profile_description = modeluser.profile_description;
                return true;
            }
            return false;
        }
    }
}

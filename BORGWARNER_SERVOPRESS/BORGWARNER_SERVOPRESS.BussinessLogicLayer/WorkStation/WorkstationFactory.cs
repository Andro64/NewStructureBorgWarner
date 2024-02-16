using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer.WorkStation
{
    public static class WorkstationFactory
    {
        static SessionApp sessionApp;

        public static void injectionSession(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }

        public static Workstation CreateWorkstation(string type)
        {
            switch (type.ToLower())
            {
                case "autotipo1":
                    return new WorkStation_Automatic_Type1();
                case "manualtipo1":
                    return new WorkStation_Manual_Type1(sessionApp);
                default:
                    throw new ArgumentException("Tipo de estación de trabajo no válido", nameof(type));
            }
        }
    }
}


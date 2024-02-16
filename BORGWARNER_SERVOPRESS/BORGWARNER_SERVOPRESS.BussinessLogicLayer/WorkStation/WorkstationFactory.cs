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

        public static Workstation CreateWorkstation()
        {
            string type = sessionApp.settings.FirstOrDefault(x => x.setting.Equals("TYPE_WORKSTATION")).valueSetting;
            switch (type)
            {
                case "1":
                    return new WorkStation_Manual_Type1(sessionApp);                    
                case "2":
                    return new WorkStation_Manual_Type2();
                case "3":
                    return new WorkStation_Manual_Type17A();
                case "4":
                    return new WorkStation_Manual_Type17B();
                case "5":
                    return new WorkStation_Manual_Type17C();
                case "6":
                    return new WorkStation_Automatic_Type1(sessionApp);
                case "7":
                    return new WorkStation_Automatic_Type2();
                case "8":
                    return new WorkStation_Automatic_Type3();
                case "9":
                    return new WorkStation_Automatic_Type4();
                default:
                    throw new ArgumentException("Tipo de estación de trabajo no válido", nameof(type));
            }
        }
    }
}


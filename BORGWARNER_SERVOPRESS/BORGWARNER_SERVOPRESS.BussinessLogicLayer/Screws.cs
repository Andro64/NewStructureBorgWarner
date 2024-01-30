using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class Screws
    {
        SessionApp sessionApp;
        CommunicationScrew communicationScrew;
       
        public Screws(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            communicationScrew = new CommunicationScrew(sessionApp);            
        }
        public int Quantity { get { return int.Parse(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("Number_Screws_Process")).valueSetting); } }        
        public List<Screw> getScrews()        
        {             
            return communicationScrew.getScrews();
        }
        public List<ModelScrew> getModelScrews()
        {   
            return communicationScrew.getModelsScrew();
        }
        public List<Screw> retriveScrewsToProcess(int ModelScrewSelected)
        {
            List<Screw> lstRetriveInfoScrew = new List<Screw>();
            try
            {
                List<Screw> lstScrews = getScrews();
                int quantityScrews = retriveNumberScrewsPerModel(ModelScrewSelected);
                lstRetriveInfoScrew = lstScrews.Where(x => x.id_model_screw.Equals(ModelScrewSelected)).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }
            return lstRetriveInfoScrew;
        }
        public int retriveNumberScrewsPerModel(int ModelScrewSelected)
        {
            List<ModelScrew> lstModelsScrew = getModelScrews();
            return lstModelsScrew.FirstOrDefault(x=>x.id.Equals(ModelScrewSelected)).quantity_screws;
        }
        private int getQuantity()
        {
            return int.Parse(sessionApp.settings.FirstOrDefault(x => x.setting.Equals("Number_Screws_Process")).valueSetting);
        }
    }
}

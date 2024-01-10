using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    internal class DevicesBL
    {
        SessionApp sessionApp;
        
        public DevicesBL(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
        }

        public ADU initializeCard(string serial)
        {
            ADU iocard = new ADU(serial);
            return iocard;  
        }
        public Devices getDevices()
        {
            //Devices devices;
            Devices devices = new Devices();
            //devices.ioCard = new IOCard();
            //devices.camara = new Camara();            
            //devices.epsonRobot =  new EpsonRobot();
            MYSQL_DB mYSQL_DB = new MYSQL_DB(sessionApp.connStr);
            try
            {
                DataTable dtDevices = mYSQL_DB.ExecuteSP("SP_GET_SETTINGS");
                if (dtDevices.Rows.Count > 0)
                {

                    for (int i = 0; i <= sessionApp.settings.adus_serial; i++)
                    {
                        devices.lstIOCard = new List<IOCard>() { new IOCard() { NumSerial = dtDevices.Rows[0]["adu1_serial"].ToString() } };

                    }
                    //devices = new Devices()
                    //{

                    //camara = new Camara()
                    //{
                    //    IP =dtDevices.Rows[0]["camara1_ip"].ToString(),
                    //    Port= int.Parse(dtDevices.Rows[0]["fis_port"].ToString())
                    //},
                    //epsonRobot = new EpsonRobot() 
                    //{
                    //    IP =dtDevices.Rows[0]["Robot1_ip"].ToString(),
                    //    Port= int.Parse(dtDevices.Rows[0]["fis_port"].ToString())
                    //},
                    //ioCard = new IOCard() 
                    //{
                    //    NumSerial = dtDevices.Rows[0]["adu1_serial"].ToString()
                    //}
                    //};
                }
                
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                throw;
            }

            return devices;
        }
    }
}

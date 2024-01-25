using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BORGWARNER_SERVOPRESS.DataAccessLayer;
using BORGWARNER_SERVOPRESS.DataModel;
using MySql.Data.MySqlClient;

namespace BORGWARNER_SERVOPRESS.BussinessLogicLayer
{
    public class Screw
    {
        SessionApp sessionApp;
        ADUS adus;
        public Screw(SessionApp _sessionApp)
        {
            sessionApp = _sessionApp;
            adus = new ADUS(sessionApp);
        }
        public async Task startReadADUS(IProgress<string> progressScrew)
        {
            await adus.startReadADUS(progressScrew);
        }
        public void endReadADUS()
        {
            adus.endReadADUS();
        }




        //public DataTable getScrewsSP(int page, int size)
        //{
        //    DataTable resultData = new DataTable();
        //    try
        //    {
        //        MYSQL_DB mYSQL = new MYSQL_DB(sessionApp.connStr);
        //        resultData = mYSQL.ExecuteSP("SP_GET_CREWS",
        //                                                        new MySqlParameter("page_p", page),
        //                                                        new MySqlParameter("size_p", size));
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        throw;
        //    }
        //    return resultData;
        //}
    }
}

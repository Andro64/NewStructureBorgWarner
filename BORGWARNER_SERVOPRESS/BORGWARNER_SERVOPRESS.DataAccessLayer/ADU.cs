using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    public class ADU
    {
        public int aduHandle;
        public string SerialNumber;
        public ADU(string serial)
        {
            SerialNumber = serial;
            aduHandle = ModuleADUImport.OpenAduDeviceBySerialNumber(SerialNumber, 1);

        }

        public bool[] MapADUInput()
        {
            bool[] result = new bool[8] { false, false, false, false, false, false, false, false };
            bool[] tempResult1;
            bool[] tempResult2;

            try
            {
                int iRC;
                int iBytesWritten = default;
                string msg = "RPA";
                var arglepBuffer = msg;
                iRC = ModuleADUImport.WriteAduDevice(aduHandle, arglepBuffer, msg.Length, ref iBytesWritten, 500);
                int iBytesRead = 0;
                StringBuilder sResponse = new StringBuilder(32);
                String Response;
                sResponse.Append("No Data");

                // The preloaded string is "+++No Data+++" which will be displayed if there is no returned data.
                iRC = ModuleADUImport.ReadAduDevice(aduHandle, sResponse, 7, iBytesRead, 500);
                Response = sResponse.ToString();
                tempResult1 = Response.Select(c => c == '1').ToArray();
                Array.Reverse(tempResult1);


                iBytesWritten = default;
                msg = "RPB";
                arglepBuffer = msg;
                iRC = ModuleADUImport.WriteAduDevice(aduHandle, arglepBuffer, msg.Length, ref iBytesWritten, 500);
                iBytesRead = 0;
                sResponse = new StringBuilder(32);
                sResponse.Append("No Data");
                // The preloaded string is "+++No Data+++" which will be displayed if there is no returned data.
                iRC = ModuleADUImport.ReadAduDevice(aduHandle, sResponse, 7, iBytesRead, 500);
                Response = sResponse.ToString();
                tempResult2 = Response.Select(c => c == '1').ToArray();
                Array.Reverse(tempResult2);
                result = tempResult1.Concat(tempResult2).ToArray();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{DateTime.Now} - "  + "Error: " + ex.Message);
                //[Falta_variable]
                //G.status = 207;
            }

            return result;
            //Displays the received ASCII string in the Textbox
        }
        public void MapADUOutput(bool[] outputs)
        {
            try
            {
                string msg = "MK";
                int OutputDecimalValue = 0;
                OutputDecimalValue = Boolean_to_decimal(outputs);
                msg += OutputDecimalValue.ToString();
                int iRC;
                int iBytesWritten = default;

                var arglepBuffer = msg;
                iRC = ModuleADUImport.WriteAduDevice(aduHandle, arglepBuffer, msg.Length, ref iBytesWritten, 500);
                int iBytesRead = 0;
                StringBuilder sResponse = new StringBuilder(32);
                String Response;
                sResponse.Append("No Data");
                // The preloaded string is "+++No Data+++" which will be displayed if there is no returned data.
                iRC = ModuleADUImport.ReadAduDevice(aduHandle, sResponse, 7, iBytesRead, 500);
                Response = sResponse.ToString();
                //Displays the received ASCII string in the Textbox
            }
            catch (Exception ex)
            {
                //[Falta_variables 2]
                //G.status = 311;
                //G.ActiveAlarm = true;
                Debug.WriteLine($"{DateTime.Now} - "  + "Error: " + ex.Message);
            }

        }
        public static int Boolean_to_decimal(bool[] inArray)
        {
            bool[] bol = inArray.Clone() as bool[];
            Array.Reverse(bol);
            int somme = 0;
            for (int i = 0; i < bol.Length; i++)
            {
                somme += bol[i] ? (1 << (bol.Length - 1 - i)) : 0;
            }
            return somme;
        }
    }
}

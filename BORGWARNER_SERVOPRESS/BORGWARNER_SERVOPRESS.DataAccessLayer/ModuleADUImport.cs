using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataAccessLayer
{
    static class ModuleADUImport
    {
        public struct ADU_DEVICE_ID
        {
            public short iVendorId;
            public short iProductId;

            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 7)]
            public string sSerialNumber;
        }

        [System.Runtime.InteropServices.DllImport("Resources\\AduHid.DLL")]
        public static extern int OpenAduDevice(int iTimeout);

        [System.Runtime.InteropServices.DllImport("Resources\\AduHid.DLL")]
        public static extern int WriteAduDevice(int aduHandle, string IpBuffer, int lNumberOfBytesToWrite, ref int lBytesWritten, int iTimeout);

        [System.Runtime.InteropServices.DllImport("Resources\\AduHid.DLL")]
        public static extern int ReadAduDevice(int aduHandle, StringBuilder lpBuffer, int lNumberOfBytesToRead, int lBytesRead, int iTimeout);

        [System.Runtime.InteropServices.DllImport("Resources\\AduHid.DLL")]
        public static extern int CloseAduDevice(int iOverlapped);

        [System.Runtime.InteropServices.DllImport("Resources\\AduHid.DLL")]
        public static extern int ShowAduDeviceList(ref ADU_DEVICE_ID pAduDeviceId, string sPrompt);

        [System.Runtime.InteropServices.DllImport("Resources\\AduHid.DLL")]
        public static extern int OpenAduDeviceBySerialNumber(string pSerialNumber, int iTimeout);

        [System.Runtime.InteropServices.DllImport("Resources\\AduHid.DLL")]
        public static extern int ADUCount(int iTimeout);

        [System.Runtime.InteropServices.DllImport("Resources\\AduHid.DLL")]
        public static extern int GetADU(ref ADU_DEVICE_ID pAduDeviceId, int iIndex, int iTimeout);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BORGWARNER_SERVOPRESS.DataModel
{
    public class IOSensors
    {
        public bool Cyl_Fixing_Pall_Ext { get; set; }
        public bool Cyl_Fixing_Pall_Ret { get; set; }
        public bool Cyl_Lifter_Ext { get; set; }
        public bool Cyl_Lifter_Ret { get; set; }
        public bool Cyl_Pin_LH_Ext_Sensor { get; set; }
        public bool Cyl_Pin_LH_Ret_Sensor { get; set; }
        public bool Cyl_Pin_RH_Ext_Sensor { get; set; }
        public bool Cyl_Pin_RH_Ret_Sensor { get; set; }
        //public bool Cyl_Pres_Stopper { get; set; }
        public bool CylFixingExtd { get; set; }
        public bool CylFixingRetd { get; set; }
        public bool E_Stop { get; set; }
        public bool E_Stop_Active { get; set; }
        public bool Estop_signal { get; set; }
        public bool HousingatPallet { get; set; }
        public bool Interlock_Back { get; set; }
        public bool Interlock_Front { get; set; }
        public bool Interlock_Signal { get; set; }
        public bool Main_Pressure { get; set; }
        public bool MaskInHolder { get; set; }
        //public bool Mask_In_Holder { get; set; }
        public bool MaskatHolder { get; set; }
        public bool MaskatHousing { get; set; }
        public bool NOk_DigiForce1 { get; set; }
        public bool NOk_DigiForce2 { get; set; }
        public bool NOk_DigiForce3 { get; set; }
        public bool Ok_DigiForce1 { get; set; }
        public bool Ok_DigiForce2 { get; set; }
        public bool Ok_DigiForce3 { get; set; }
        public bool Ok_DigiForce4 { get; set; }
        public bool OptoBtn { get; set; }
        public bool Pallet_Pre_Stopper { get; set; }
        public bool Pallet_Stopper { get; set; }
        public bool PalletLifter2Extd { get; set; }
        public bool PalletLifter2Retd { get; set; }
        public bool PalletLifterExtd { get; set; }
        public bool PalletLifterFTExtd { get; set; }
        public bool PalletLifterFTExtd2 { get; set; }
        public bool PalletLifterFTRetd { get; set; }
        public bool PalletLifterFTRetd2 { get; set; }
        public bool PalletLifterRetd { get; set; }
        public bool PCBPresence { get; set; }
        public bool PiezaNOK_ST03_IN { get; set; }
        public bool PiezaNOK_ST04 { get; set; }
        public bool PrestopperInTunnel { get; set; }
        public bool Prestopper_Pallet_Present { get; set; }
        public bool Pressure_Sensor { get; set; }
        public bool Rework_Screw { get; set; }
        public bool Ready_Digi1 { get; set; }
        public bool Ready_Digi2 { get; set; }
        public bool Ready_Digi3 { get; set; }
        public bool Ready_Digi4 { get; set; }
        public bool Scrap_presence { get; set; }
        public bool Screw_Level_Oth { get; set; }
        public bool Screw_Present_Oth { get; set; }
        public bool ScrewPresence { get; set; }
        public bool SecurityOK { get; set; }
        public bool Sta14_Available { get; set; }
        public bool StartRobot_Epson { get; set; }
        public bool ST02Available { get; set; }
        public bool ST03Available { get; set; }
        public bool ST05Available { get; set; }
        public bool ST06Available { get; set; }
        public bool ST13Available { get; set; }
        public bool ST15Available { get; set; }
        public bool ST17AAvailable { get; set; }
        public bool Stopper_Pallet_Present { get; set; }
        public bool Trigger_Scanner { get; set; }
        public bool UltraCapBoardReadytoScan { get; set; }

        public bool PA1 { get; set; }
        public bool PA2 { get; set; }
        public bool PA3 { get; set; }
        public bool PA4 { get; set; }

        public bool PB1 { get; set; }
        public bool PB2 { get; set; }
        public bool PB3 { get; set; }
        public bool PB4 { get; set; }

        public bool PA0_1 { get; set; }
        public bool PA0_2 { get; set; }
        public bool PA0_3 { get; set; }
        public bool PA0_4 { get; set; }

        public bool PA1_1 { get; set; }
        public bool PA1_2 { get; set; }
        public bool PA1_3 { get; set; }
        public bool PA1_4 { get; set; }

        public bool PA2_1 { get; set; }
        public bool PA2_2 { get; set; }
        public bool PA2_3 { get; set; }
        public bool PA2_4 { get; set; }

        public bool PA3_1 { get; set; }
        public bool PA3_2 { get; set; }
        public bool PA3_3 { get; set; }
        public bool PA3_4 { get; set; }

        public bool PB0_1 { get; set; }
        public bool PB0_2 { get; set; }
        public bool PB0_3 { get; set; }
        public bool PB0_4 { get; set; }

        public bool PB1_1 { get; set; }
        public bool PB1_2 { get; set; }
        public bool PB1_3 { get; set; }
        public bool PB1_4 { get; set; }

        public bool PB2_1 { get; set; }
        public bool PB2_2 { get; set; }
        public bool PB2_3 { get; set; }
        public bool PB2_4 { get; set; }

        public bool PB3_1 { get; set; }
        public bool PB3_2 { get; set; }
        public bool PB3_3 { get; set; }
        public bool PB3_4 { get; set; }


        #region OutPuts
        public bool Opto_Grn { get; set; }
        public bool Opto_Yllw { get; set; }
        public bool Opto_Red { get; set; }
        public bool Reset_Signal { get; set; }
        public bool K4_1 { get; set; }
        public bool ScrewDispenser { get; set; }
        public bool Vacuum { get; set; }
        public bool K7_1 { get; set; }

        public bool PalletFixingExt { get; set; }
        public bool PalletFixingRet { get; set; }
        public bool Cyl_Stopper { get; set; }
        public bool Cyl_Pres_Stopper { get; set; }
        public bool K4_2 { get; set; }
        public bool K5_2 { get; set; }
        public bool K6_2 { get; set; }
        public bool K7_2 { get; set; }

        public bool ST12Available { get; set; }
        public bool LampRCam { get; set; }
        public bool LampLCam { get; set; }
        public bool K3_3 { get; set; }
        public bool K4_3 { get; set; }
        public bool K5_3 { get; set; }
        public bool K6_3 { get; set; }
        public bool ReleScrap { get; set; }
        #endregion

        public void ClearAllBooleanProperties()
        {
            // Obtiene el tipo de la instancia actual
            var type = this.GetType();

            // Itera sobre todas las propiedades del tipo
            foreach (var property in type.GetProperties())
            {
                // Verifica si la propiedad es de tipo booleano y tiene un setter
                if (property.PropertyType == typeof(bool) && property.CanWrite)
                {
                    // Asigna false a la propiedad
                    property.SetValue(this, false);
                }
            }
        }

    }
}

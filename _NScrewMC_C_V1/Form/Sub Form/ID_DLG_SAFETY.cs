using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormSafety : Form
    {
        System.Windows.Forms.Timer UiUpdate;
        public FormSafety()
        {
            InitializeComponent();
            MSystem.m_pTrsBuzzer.BuzzerOn();

            // UI Update Timer
            UiUpdate = new System.Windows.Forms.Timer();
            UiUpdate.Interval = 100;
            UiUpdate.Tick += UiUpdate_Tick;
            UiUpdate.Start();
        }
        private void UiUpdate_Tick(object sender, EventArgs e)
        {
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_FRONT_LEFT_DOOR_DETECT"]))
            {
                if (LB_SAFETY_IO_1.BackColor != Color.LawnGreen)
                    LB_SAFETY_IO_1.BackColor = Color.LawnGreen;
            }
            else
            {
                if (LB_SAFETY_IO_1.BackColor != Color.Gray)
                    LB_SAFETY_IO_1.BackColor = Color.Gray;
            }
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_FRONT_RIGHT_DOOR_DETECT"]))
            {
                if (LB_SAFETY_IO_2.BackColor != Color.LawnGreen)
                    LB_SAFETY_IO_2.BackColor = Color.LawnGreen;
            }
            else
            {
                if (LB_SAFETY_IO_2.BackColor != Color.Gray)
                    LB_SAFETY_IO_2.BackColor = Color.Gray;
            }
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_REAR_LEFT_DOOR_DETECT"]))
            {
                if (LB_SAFETY_IO_3.BackColor != Color.LawnGreen)
                    LB_SAFETY_IO_3.BackColor = Color.LawnGreen;
            }
            else
            {
                if (LB_SAFETY_IO_3.BackColor != Color.Gray)
                    LB_SAFETY_IO_3.BackColor = Color.Gray;
            }
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_REAR_RIGHT_DOOR_DETECT"]))
            {
                if (LB_SAFETY_IO_4.BackColor != Color.LawnGreen)
                    LB_SAFETY_IO_4.BackColor = Color.LawnGreen;
            }
            else
            {
                if (LB_SAFETY_IO_4.BackColor != Color.Gray)
                    LB_SAFETY_IO_4.BackColor = Color.Gray;
            }
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_LEFT_DOOR_1_DETECT"]))
            {
                if (LB_SAFETY_IO_5.BackColor != Color.LawnGreen)
                    LB_SAFETY_IO_5.BackColor = Color.LawnGreen;
            }
            else
            {
                if (LB_SAFETY_IO_5.BackColor != Color.Gray)
                    LB_SAFETY_IO_5.BackColor = Color.Gray;
            }
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_RIGHT_DOOR_2_DETECT"]))
            {
                if (LB_SAFETY_IO_6.BackColor != Color.LawnGreen)
                    LB_SAFETY_IO_6.BackColor = Color.LawnGreen;
            }
            else
            {
                if (LB_SAFETY_IO_6.BackColor != Color.Gray)
                    LB_SAFETY_IO_6.BackColor = Color.Gray;
            }
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_DOOR_SENSOR"]))
            {
                if (LB_SAFETY_IO_8.BackColor != Color.LawnGreen)
                    LB_SAFETY_IO_8.BackColor = Color.LawnGreen;
            }
            else
            {
                if (LB_SAFETY_IO_8.BackColor != Color.Gray)
                    LB_SAFETY_IO_8.BackColor = Color.Gray;
            }
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_FRONT_OP_BOX_E_STOP_SW"]))
            {
                if (LB_SAFETY_IO_7.BackColor != Color.LawnGreen)
                    LB_SAFETY_IO_7.BackColor = Color.LawnGreen;
            }
            else
            {
                if (LB_SAFETY_IO_7.BackColor != Color.Gray)
                    LB_SAFETY_IO_7.BackColor = Color.Gray;
            }
        }
        private void BT_BUZZR_OFF_Click(object sender, EventArgs e)
        {
            MSystem.m_pDIO.OutPutOff(IOMap.OUT["OUT_BUZZER"]);
        }

        private void BT_EXIT_Click(object sender, EventArgs e)
        {
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_FRONT_OP_BOX_E_STOP_SW"])
                && MSystem.m_pDIO.IsOn(IOMap.IN["IN_DOOR_SENSOR"])
                && MSystem.m_pDIO.IsOn(IOMap.IN["IN_FRONT_LEFT_DOOR_DETECT"])
                && MSystem.m_pDIO.IsOn(IOMap.IN["IN_FRONT_RIGHT_DOOR_DETECT"])
                && MSystem.m_pDIO.IsOn(IOMap.IN["IN_REAR_LEFT_DOOR_DETECT"])
                && MSystem.m_pDIO.IsOn(IOMap.IN["IN_REAR_RIGHT_DOOR_DETECT"])
                && MSystem.m_pDIO.IsOn(IOMap.IN["IN_LEFT_DOOR_1_DETECT"])
                && MSystem.m_pDIO.IsOn(IOMap.IN["IN_RIGHT_DOOR_2_DETECT"]))
            {
                MSystem.m_pDIO.OutPutOff(IOMap.OUT["OUT_BUZZER"]);
                if (UiUpdate != null)
                {
                    UiUpdate.Stop();
                    UiUpdate.Dispose();
                }
                this.Close();
            }
            else
            {
                MSystem.MyMsgMemo("Pless Check Door and E-STOP", "WARNING");
            }
        }

        private async void BT_RESET_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                MSystem.m_pDIO.OutPutOn(IOMap.OUT["OUT_SAFETY_PLC_RESET"]);
                Thread.Sleep(1000);
                MSystem.m_pDIO.OutPutOff(IOMap.OUT["OUT_SAFETY_PLC_RESET"]);
            });
            
        }
    }
}

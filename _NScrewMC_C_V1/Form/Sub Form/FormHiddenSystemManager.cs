using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormHiddenSystemManager : System.Windows.Forms.Form
    {
        public FormHiddenSystemManager()
        {
            InitializeComponent();
            if (MSystem.IsDoorSkip == true)
                CB_DOOR.Text = "Not Use";
            else
                CB_DOOR.Text = "Use";
            // Vision Check Door
            if (MSystem.IsVisionCheckDoor == false)
                CB_VS_CHECK_DOOR.Text = "Not Use";
            else
                CB_VS_CHECK_DOOR.Text = "Use";
            // Vision Check SS Safety
            if (MSystem.IsVisionCheckSSSafety == false)
                CB_VS_CHECK_SS_SAFETY.Text = "Not Use";
            else
                CB_VS_CHECK_SS_SAFETY.Text = "Use";
            // Angle Control
            if (InforManager.Instance.IsAngleControl == true)
                CB_ANGLE_CONTROL.Text = "Use";
            else
                CB_ANGLE_CONTROL.Text = "Not Use";

            if (MSystem.m_pBcr[0].IsConnected && MSystem.m_pBcr[1].IsConnected)
                MSystem.IsConnectScanner = true;
            else
                MSystem.IsConnectScanner = false;
            if (MSystem.IsConnectScanner == false)
                CB_SCANNER_CONTROL.Text = "Disconnect";
            else
                CB_SCANNER_CONTROL.Text = "Connect";
        }
        private void colorButton2_Click(object sender, EventArgs e)
        {
            MSystem.IsDoorSkip = !MSystem.IsDoorSkip;
            if (MSystem.IsDoorSkip == true)
            {
                CB_DOOR.Text = "Not Use";
                MSystem.DoorSkipStartTime = DateTime.Now;
            }
            else
                CB_DOOR.Text = "Use";
        }
        private void BtExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CB_VS_CHECK_SS_SAFETY_Click(object sender, EventArgs e)
        {
            MSystem.IsVisionCheckSSSafety = !MSystem.IsVisionCheckSSSafety;
            if (MSystem.IsVisionCheckSSSafety == false)
                CB_VS_CHECK_SS_SAFETY.Text = "Not Use";
            else
                CB_VS_CHECK_SS_SAFETY.Text = "Use";
        }

        private void CB_VS_CHECK_DOOR_Click(object sender, EventArgs e)
        {
            MSystem.IsVisionCheckDoor = !MSystem.IsVisionCheckDoor;
            if (MSystem.IsVisionCheckDoor == false)
                CB_VS_CHECK_DOOR.Text = "Not Use";
            else
                CB_VS_CHECK_DOOR.Text = "Use";
        }

        private void CB_ANGLE_CONTROL_Click(object sender, EventArgs e)
        {
            InforManager.Instance.IsAngleControl = !InforManager.Instance.IsAngleControl;
            if (InforManager.Instance.IsAngleControl == true)
                CB_ANGLE_CONTROL.Text = "Use";
           
            else
                CB_ANGLE_CONTROL.Text = "Not Use";
        }

        private void CB_SCANNER_CONTROL_Click(object sender, EventArgs e)
        {
            MSystem.IsConnectScanner = !MSystem.IsConnectScanner;
            if (MSystem.IsConnectScanner == true)
            {
                MSystem.m_pBcr[0].Open();
                MSystem.m_pBcr[1].Open();
                CB_SCANNER_CONTROL.Text = "Connect";
            }
            else
            {
                MSystem.m_pBcr[0].DisconnectBcrPort();
                MSystem.m_pBcr[1].DisconnectBcrPort();
                CB_SCANNER_CONTROL.Text = "Disconnect";
            }
        }
    }
}

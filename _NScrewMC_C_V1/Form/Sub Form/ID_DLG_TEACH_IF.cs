using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class ID_DLG_TEACH_IF : Form
    {
        #region //Timer Update Status
        System.Windows.Forms.Timer TimUpdate = new System.Windows.Forms.Timer();
        #endregion
        public ID_DLG_TEACH_IF()
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + (FormMain.m_Width - this.Width) / 2, FormMain.GetLocation().Y + (FormMain.m_Height - this.Height) / 2);

            TimUpdate.Tick += new EventHandler(UpdateData);
            TimUpdate.Interval = 50;
            TimUpdate.Start();
        }
        private void PreLoadEvent(object sender, EventArgs e)
        {
            if ((sender as SUserControls.ColorButton).Text == "Exit")
            {
                this.Close();
                return;
            }
        }
        void UpdateData(object sender, EventArgs e) 
        {
            //MSystem.IsSensorOn(IDC_PRESS_FRONT_JIG_START, MSystem.m_pTrsTransfer.IsPressRightStartOn());
            //MSystem.IsSensorOn(IDC_PRESS_REAR_JIG_START, MSystem.m_pTrsTransfer.IsPressLeftStartOn());

            //MSystem.IsSensorOn(IDC_FRONT_PRESS_BWD, MSystem.m_pTrsTransfer.IsPressRightBWD());
            //MSystem.IsSensorOn(IDC_FRONT_PRESS_TILT, MSystem.m_pTrsTransfer.IsPressRightTilt());
            //MSystem.IsSensorOn(IDC_FRONT_PRESS_FWD, MSystem.m_pTrsTransfer.IsPressRightFWD());
            ////MSystem.IsSensorOn(IDC_REAR_PRESS_BWD, MSystem.m_pTrsTransfer.IsPressLeftBWD());
            //MSystem.IsSensorOn(IDC_REAR_PRESS_TILT, MSystem.m_pTrsTransfer.IsPressLeftTilt());
            //MSystem.IsSensorOn(IDC_REAR_PRESS_FWD, MSystem.m_pTrsTransfer.IsPressLeftFWD());
        }

        private void IDC_PRESS_REAR_JIG_START_Click(object sender, EventArgs e)
        {
            //MSystem.m_pTrsTransfer.PressLeftStartOn();
            //Thread.Sleep(300);
            //MSystem.m_pTrsTransfer.PressLeftStartOff();

            //if (MSystem.m_pDIO.IsOn(IO.OUT_SCREW_1_MC_R_SHUTTLE_IF))
            //    MSystem.m_pTrsTransfer.PressLeftStartOff();
            //else
            //    MSystem.m_pTrsTransfer.PressLeftStartOn();
        }

        private void IDC_PRESS_FRONT_JIG_START_Click(object sender, EventArgs e)
        {
            //MSystem.m_pTrsTransfer.PressRightStartOn();
            //Thread.Sleep(300);
            //MSystem.m_pTrsTransfer.PressRightStartOff();
        }

        private void ID_DLG_TEACH_IF_FormClosing(object sender, FormClosingEventArgs e)
        {
            TimUpdate.Stop();
            TimUpdate.Dispose();
        }
    }
}

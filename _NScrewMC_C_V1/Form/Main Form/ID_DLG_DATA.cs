using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormData : Form
    {
        #region //Variable
       
        #endregion
        
        #region //Form Init
        public FormData()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            //////////////////////////////////////////////////////////////////////
        }
        private void LoadAllData(object sender, EventArgs e) {
          
        }
        #endregion

        private void IDC_PORT_SETTING_ClickEvent(object sender, EventArgs e)
        {
            FormPortSetting dlg = new FormPortSetting();
            dlg.ShowDialog();
        }

        private void IDC_MODEL_DATA_ClickEvent(object sender, EventArgs e)
        {
            FormModel dlg = new FormModel();
            dlg.ShowDialog();
        }

        private void IDC_SYSTEM_MANAGER_ClickEvent(object sender, EventArgs e)
        {
            FormPassword dlgvalue = new FormPassword("1111");
            DialogResult result = dlgvalue.ShowDialog();
            if (dlgvalue.IsOK == false)
                return;

            FormManagerInfor dlg = new FormManagerInfor();
            dlg.ShowDialog();

            //string Pass = MSystem.GetPass();
            //if (Pass == null) return;
            //if (Pass != "1111")
            //{
            //    MSystem.MyMsgMemo("Password incorrect!", "Error", msgButton.OK, msgIcon.Error);
            //    return;
            //}

            //if (MSystem.NumDisplay != (int)NumViewMain.eViewTeach)
            //{
                
            //}
        }

        private void IDC_TIME_DELAY_ClickEvent(object sender, EventArgs e)
        {
            FormTimeAndDelay dlg = new FormTimeAndDelay();
            dlg.ShowDialog();
        }

        private void IDC_JOG_VELOCITY_ClickEvent(object sender, EventArgs e)
        {
            FormJogVelocity dlg = new FormJogVelocity();
            dlg.ShowDialog();
        }

        private void IDC_MOTOR_VELOCITY_ClickEvent(object sender, EventArgs e)
        {
            FormMotorVelocity dlg = new FormMotorVelocity();
            dlg.ShowDialog();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            FormPassword dlgvalue = new FormPassword("YJ");
            DialogResult result = dlgvalue.ShowDialog();
            if (dlgvalue.IsOK == false)
                return;

            FormHiddenSystemManager formHiddenSystemManager = new FormHiddenSystemManager();
            formHiddenSystemManager.ShowDialog();
        }
        private void IDC_POSITION_EDIT_ClickEvent(object sender, EventArgs e)
        {
            
        }

        private void IDC_POSITION_EDIT_Click(object sender, EventArgs e)
        {
            PositionListEdit dlg = new PositionListEdit();
            dlg.ShowDialog();
        }

        private void IDC_DEMO_SET_LIST_Click(object sender, EventArgs e)
        {
            DemoSetPosList dlg = new DemoSetPosList();
            dlg.ShowDialog();
        }
    }
}

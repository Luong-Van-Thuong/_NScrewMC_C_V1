using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace _NScrewMC_C_V1
{
    public partial class FormAlarm : Form
    {
        #region //Initial Form
        static FormAlarm MsgBox;
        public static int data = 0;

        public FormAlarm()
        {
            InitializeComponent();
            BtExit.DialogResult = DialogResult.OK;
            MSystem.m_bIsTeachDlg = true;

        } 
        public static DialogResult Show(string contentErr, string Action,string strName)
        {
            MsgBox = new FormAlarm();
            MsgBox.MessagerError.Text = contentErr;
            MsgBox.MessagerAction.Text = Action;
            MsgBox.BT_UNIT_NAME.Text = strName;
            MsgBox.ShowDialog();
           // MSystem.SysStatus = StatusRun.STOP;
            return DialogResult.OK;
        }
        #endregion

        #region//Event Form
        public void BtBuzzOff_Click(object sender, EventArgs e)
        {
            //MSystem.m_pTrsBuzzer.StopAllBuzzers();
            //MSystem.m_pDIO.OutPutOff(IO.OUT_BUZZER);
        }

        #endregion

        private void MyMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            MSystem.SysStatus = StatusRun.STOP;
            MSystem.m_bIsTeachDlg = false;
        }

        private void BtExit_Click(object sender, EventArgs e)
        {
            MSystem.m_bIsTeachDlg = false;
            MSystem.m_pTrsBuzzer.BuzzerOff();
        }
    }
}

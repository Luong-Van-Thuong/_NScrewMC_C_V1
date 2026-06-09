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
    public partial class FormBottom : Form
    {
        #region //Form Initial 

        Image[] ImgSelect = { Properties.Resources.NV_MainUI_Auto_Click, Properties.Resources.NV_MainUI_Teach_Click,
                              Properties.Resources.NV_MainUI_Data_Click, Properties.Resources.NV_MainUI_Log_Click};

        Image[] ImgNormal = { Properties.Resources.NV_MainUI_Auto_Default, Properties.Resources.NV_MainUI_Teach_Default,
                              Properties.Resources.NV_MainUI_Data_Default,Properties.Resources.NV_MainUI_Log_Default};

        SUserControls.ColorButton[] m_btButton = new SUserControls.ColorButton[6]; 

        void AllImgNomarl() {
            for (int i = 0; i < 4; i++) {
                m_btButton[i].BackgroundImage = ImgNormal[i];
            }
        }
        public void SetImgSelect(int _index) {
            AllImgNomarl();
            m_btButton[_index - 1].BackgroundImage = ImgSelect[_index - 1];
        }
        public FormBottom()
        {
            InitializeComponent();
        }
        #endregion

        #region //Event form
        private void BtExit_Click(object sender, EventArgs e)
        {
            if (MSystem.SysStatus == StatusRun.RUN) return;
            if (MSystem.MyMsgMemo("Do You Want Exit Program?", "Exit PGM", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
            {
                if (MSystem.MyMsgMemo("Confirm! Are You Ready Exit Program?", "Confirm Exit PGM", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
                {
                    MSystem.vinterface.Server[0].TcpClientServerStop();
                    MSystem.vinterface.Server[1].TcpClientServerStop();
                    MSystem.MyMessagerBottom("Exit Program");
                    MSystem.NumDisplay = (int)NumViewMain.eViewExit;
                }
            }
        }
        private void BtHiden_Click(object sender, EventArgs e)
        {
            //this.m_bIsOnUIData = false;
            //this.m_bIsOnUITeach = false;
            MSystem.NumDisplay = (int)NumViewMain.eViewHiden;
        }
        private void BtTeach_Click(object sender, EventArgs e)
        {
            if (MSystem.SysStatus == StatusRun.RUN) return;
            if (MSystem.TeachForm.Visible) return;

            //if (m_bIsOnUIData == false)
            //{
            //    string Pass = MSystem.GetPass();
            //    if (Pass == null) return;
            //    if (Pass != "1111")
            //    {
            //        MSystem.MyMsgMemo("Password incorrect!", "Error", msgButton.OK, msgIcon.Error);
            //        return;
            //    }
            //    if (MSystem.NumDisplay != (int)NumViewMain.eViewTeach)
            //    {
            //        this.m_bIsOnUIData = true;
            //        MSystem.NumDisplay = (int)NumViewMain.eViewTeach;
            //        SetImgSelect(MSystem.NumDisplay);
            //    }
            //}
            //else
            {
                if (MSystem.NumDisplay != (int)NumViewMain.eViewTeach)
                {
                    MSystem.NumDisplay = (int)NumViewMain.eViewTeach;
                    SetImgSelect(MSystem.NumDisplay);

                    MSystem.IsOriginInitFlag = true;
                }
            }
            
        }
        private void BtAuto_Click(object sender, EventArgs e)
        {

            if (MSystem.NumDisplay != (int)NumViewMain.eViewAuto)
            {
                MSystem.NumDisplay = (int)NumViewMain.eViewAuto;
                SetImgSelect(MSystem.NumDisplay);
                
                if (MSystem.IsAutoInitillize == true)
                {
                    FormUnitInit dlg = new FormUnitInit();
                    dlg.ShowDialog();
                    MSystem.IsAutoInitillize = false;
                    // Auto Form UI Update
                    FormAuto formAuto = Application.OpenForms
                               .OfType<FormAuto>()
                               .FirstOrDefault();
                    if (formAuto != null)
                    {
                        formAuto.AutoItemUpdate();
                    }
                }
            }
        }
        private void BtLog_Click(object sender, EventArgs e)
        {
            if (MSystem.NumDisplay != (int)NumViewMain.eViewLog)
            {
                MSystem.NumDisplay = (int)NumViewMain.eViewLog;
                SetImgSelect(MSystem.NumDisplay);
            }
        }
        private void BtData_Click(object sender, EventArgs e)
        {
            if (MSystem.SysStatus == StatusRun.RUN) return;
            if (MSystem.DataForm.Visible) return;
            //if (m_bIsOnUIData == false)
            //{
            //    string Pass = MSystem.GetPass();
            //    if (Pass == null) return;
            //    if (Pass != "1111")
            //    {
            //        MSystem.MyMsgMemo("Password incorrect!", "Error", msgButton.OK, msgIcon.Error);
            //        return;
            //    }
            //    if (MSystem.NumDisplay != (int)NumViewMain.eViewData)
            //    {
            //        this.m_bIsOnUIData = true;
            //        MSystem.NumDisplay = (int)NumViewMain.eViewData;
            //        SetImgSelect(MSystem.NumDisplay);
            //    }
            //}
            //else
            {
                if (MSystem.NumDisplay != (int)NumViewMain.eViewData)
                {
                    MSystem.NumDisplay = (int)NumViewMain.eViewData;
                    SetImgSelect(MSystem.NumDisplay);
                }
            }
        }

        #endregion
        #region Messenger
        public void ShowTime(string messenger, String _Action, string strUnit)
        {
            // Don't do anything if the form's handle hasn't been created 
            // or the form has been disposed.
            if (!this.IsHandleCreated || this.IsDisposed) return;

            // Invoke an anonymous method on the thread of the form.
            this.Invoke((MethodInvoker)delegate
            {
                FormAlarm.Show(messenger, _Action, strUnit);
            });

            return;
        }
        public DialogResult msgMemo(string strmsg, string strTitle, msgButton _bt, msgIcon _icon)
        {
            DialogResult dlgresult = new DialogResult();
            // Don't do anything if the form's handle hasn't been created 
            // or the form has been disposed.
            if (!this.IsHandleCreated || this.IsDisposed)
                return DialogResult.Cancel;

            // Invoke an anonymous method on the thread of the form.
            this.Invoke((MethodInvoker)delegate
            {
                dlgresult = FMsgMemo.Show(strmsg, strTitle, _bt, _icon);
            });
            return dlgresult;
        }
        public void msgDisplay(string strmsg)
        {
            // Don't do anything if the form's handle hasn't been created 
            // or the form has been disposed.
            if (!this.IsHandleCreated || this.IsDisposed) return;

            // Invoke an anonymous method on the thread of the form.
            this.Invoke((MethodInvoker)delegate
            {
                FMsgDisplay.ShowMsg(strmsg);
            });
            return;
        }
        #endregion
        private void FormBottom_Load(object sender, EventArgs e)
        {
            int _index = 0;
            m_btButton[_index++] = BtAuto;
            m_btButton[_index++] = BtTeach;
            m_btButton[_index++] = BtData;
            m_btButton[_index++] = BtLog;
            m_btButton[_index++] = BtHiden;
            m_btButton[_index++] = BtExit;
        }
    }
}

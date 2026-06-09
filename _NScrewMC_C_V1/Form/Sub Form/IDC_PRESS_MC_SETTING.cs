using Newtonsoft.Json.Linq;
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
using System.Windows.Navigation;

namespace _NScrewMC_C_V1
{
    public partial class ID_DLG_PRESSMC_PARAMETER : Form
    {
        public const bool USE = true;
        public const bool NOT_USE = false;
        System.Timers.Timer _UpdateData = new System.Timers.Timer();
        TimerDelay m_iTestCMD = new TimerDelay();
        public ID_DLG_PRESSMC_PARAMETER()
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + (FormMain.m_Width - this.Width) / 2, FormMain.GetLocation().Y + (FormMain.m_Height - this.Height) / 2);
            _UpdateData.Elapsed += new System.Timers.ElapsedEventHandler(UpdateData);
            this.FormClosed += new FormClosedEventHandler(CloseForm);
            _UpdateData.Interval = 500;
            _UpdateData.Start();
                
            //showValueUse(0, InforManager.Instance.PressMCUse[0]);
            //showValueUse(1, InforManager.Instance.PressMCUse[1]);
            //if (InforManager.Instance.HingeUse) TB_HINGER_USE.Text = "USE";
            //else TB_HINGER_USE.Text = "NOT_USE";
            //MSystem.m_pPressMCPort[0].GetTimeAndPressureCMD();
            //MSystem.m_pPressMCPort[1].GetTimeAndPressureCMD();

        }
        private void CloseForm(object sender, FormClosedEventArgs e) {
            _UpdateData.Stop();
            _UpdateData = null;
        }
        private void UpdateData(object sender, EventArgs e) {
            SetData();
        }
        private void SetData() {
           // for (int i = 0; i < (int)PRESSMC.eMC_MAX; i++) {
           //     if (InforManager.Instance.PressMCUse[i])
           //     {
           //         ShowValueTime(i, MSystem.m_pPressMCPort[i].m_dPressTime);
           //         ShowValueKg(i, MSystem.m_pPressMCPort[i].m_dPressPressure);
           //     }
           // }
           //if(InforManager.Instance.HingeUse)
           //     TB_HINGER_STATUS.Text = MSystem.m_pTrsHingerMC.m_iMCState.ToString().Remove(0,1);

        }
        private void BT_EXIT_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void showValueUse(int _tb, bool _value) {
            //string strMsg = "";
            //if (_value) strMsg = "USE";
            //else strMsg = "NOT_USE";
            //if (_tb == 0) TB_USE_L.Text = strMsg;
            //else TB_USE_R.Text = strMsg;
        }
        void ShowValueTime(int _tb, double dValue)
        {
            //string strMsg = "";
            //if (dValue == 0)
            //    strMsg = "Wait Response";
            //else
            //    strMsg = dValue.ToString("f1");

            //if(_tb == 0) TB_TIME_L.Text = strMsg;
            //else TB_TIME_R.Text = strMsg;
        }
        void ShowValueKg(int _tb, double dValue)
        {
            //string strMsg = "";

            //if (dValue == 0)
            //    strMsg = "Wait Response";
            //else
            //    strMsg = dValue.ToString("f2");

            //if (_tb == 0) TB_PRESSURE_L.Text = strMsg;
            //else TB_PRESSURE_R.Text = strMsg;
        }

        #region //Event Left click
        private void TB_USE_L_Click(object sender, EventArgs e)
        {
            //InforManager.Instance.PressMCUse[0] = !InforManager.Instance.PressMCUse[0];
            
            //if (InforManager.Instance.PressMCUse[0]) {
            //    MSystem.m_pPressMCPort[0].GetTimeAndPressureCMD();
            //}
            //InforManager.Instance.SaveSettings();
            //showValueUse(0, InforManager.Instance.PressMCUse[0]);
        }

        private void BT_SET_TIME_L_Click(object sender, EventArgs e)
        {
            //float _fValue = 0;
            //if (MSystem.GetFloat(TB_TIME_L, ref _fValue))
            //{
            //    if (_fValue < 0) {
            //        MSystem.MyMsgMemo($"Wrong data!", "Error", msgButton.OK, msgIcon.Error);
            //        return;
            //    }
            //    if (MSystem.MyMsgMemo($"Are you set this time({_fValue.ToString("f1")}s)?", "Set Time Press", msgButton.YESNO, msgIcon.Question) == DialogResult.No) {
            //        return;
            //    }

            //    int iTime = (int)(_fValue * 10);
            //    MSystem.m_pPressMCPort[0].SetTimeCMD(iTime);
            //    MSystem.MyMsgMemo($"Send CMD to Press M/C Left!", "Infor", msgButton.OK, msgIcon.Infor);
            //    MSystem.m_pPressMCPort[0].GetTimeAndPressureCMD();
            //}
        }

        private void BT_SET_PRESSURE_L_Click(object sender, EventArgs e)
        {
            //float _fValue = 0;
            //if (MSystem.GetFloat(TB_PRESSURE_L, ref _fValue))
            //{
            //    if (_fValue < 0)
            //    {
            //        MSystem.MyMsgMemo($"Wrong data!", "Error", msgButton.OK, msgIcon.Error);
            //        return;
            //    }
            //    if (MSystem.MyMsgMemo($"Are you set this Pressure({_fValue.ToString("f2")} Kgf)?", "Set Time Press", msgButton.YESNO, msgIcon.Question) == DialogResult.No)
            //    {
            //        return;
            //    }

            //    int iTime = (int)(_fValue * 100);
            //    MSystem.m_pPressMCPort[0].SetPressureCMD(iTime);
            //    MSystem.MyMsgMemo($"Send CMD to Press M/C Left!", "Infor", msgButton.OK, msgIcon.Infor);
            //    MSystem.m_pPressMCPort[0].GetTimeAndPressureCMD();
            //}
        }

        private void BT_SET_CALIB_L_Click(object sender, EventArgs e)
        {
            //MSystem.m_pPressMCPort[0].SetCalibration();
            //TimerDelay _timDelay = new TimerDelay();
            //while (true) {
            //    Thread.Sleep(1);
            //    if (MSystem.m_pPressMCPort[0].m_iResult[(int)MCPRESS_STEP.ePRESS_MC_STEP_SET_CALIBRATION] == MC_RESULT.ePASS) {
            //        MSystem.MyMsgMemo($"Calibaration Press M/C Left Success!", "Info", msgButton.OK, msgIcon.Infor);
            //        break;
            //    }
            //    if (_timDelay.MoreThan(30)) {
            //        MSystem.MyMsgMemo($"Press MC Left Not response!", "Error", msgButton.OK, msgIcon.Infor);
            //        break;
            //    }
            //}
        }
        #endregion

        #region //Event Right click
        private void TB_USE_R_Click(object sender, EventArgs e)
        {
            //InforManager.Instance.PressMCUse[1] = !InforManager.Instance.PressMCUse[1];

            //if (InforManager.Instance.PressMCUse[1])
            //{
            //    MSystem.m_pPressMCPort[1].GetTimeAndPressureCMD();
            //}
            //InforManager.Instance.SaveSettings();
            //showValueUse(1, InforManager.Instance.PressMCUse[1]);
        }

        private void BT_SET_TIME_R_Click(object sender, EventArgs e)
        {
            //float _fValue = 0;
            //if (MSystem.GetFloat(TB_TIME_R, ref _fValue))
            //{
            //    if (_fValue < 0)
            //    {
            //        MSystem.MyMsgMemo($"Wrong data!", "Error", msgButton.OK, msgIcon.Error);
            //        return;
            //    }
            //    if (MSystem.MyMsgMemo($"Are you set this time({_fValue.ToString("f1")}s)?", "Set Time Press", msgButton.YESNO, msgIcon.Question) == DialogResult.No)
            //    {
            //        return;
            //    }

            //    int iTime = (int)(_fValue * 10);
            //    MSystem.m_pPressMCPort[1].SetTimeCMD(iTime);
            //    MSystem.MyMsgMemo($"Send CMD to Press M/C Right!", "Infor", msgButton.OK, msgIcon.Infor);
            //    MSystem.m_pPressMCPort[1].GetTimeAndPressureCMD();
            //}
        }

        private void BT_SET_PRESSURE_R_Click(object sender, EventArgs e)
        {
            //float _fValue = 0;
            //if (MSystem.GetFloat(TB_PRESSURE_R, ref _fValue))
            //{
            //    if (_fValue < 0)
            //    {
            //        MSystem.MyMsgMemo($"Wrong data!", "Error", msgButton.OK, msgIcon.Error);
            //        return;
            //    }
            //    if (MSystem.MyMsgMemo($"Are you set this Pressure({_fValue.ToString("f2")} Kgf)?", "Set Time Press", msgButton.YESNO, msgIcon.Question) == DialogResult.No)
            //    {
            //        return;
            //    }

            //    int iTime = (int)(_fValue * 100);
            //    MSystem.m_pPressMCPort[1].SetPressureCMD(iTime);
            //    MSystem.MyMsgMemo($"Send CMD to Press M/C Right!", "Infor", msgButton.OK, msgIcon.Infor);
            //    MSystem.m_pPressMCPort[1].GetTimeAndPressureCMD();
            //}
        }

        private void BT_SET_CALIB_R_Click(object sender, EventArgs e)
        {

            //MSystem.m_pPressMCPort[1].SetCalibration();
            //TimerDelay _timDelay = new TimerDelay();
            //while (true)
            //{
            //    Thread.Sleep(1);
            //    if (MSystem.m_pPressMCPort[1].m_iResult[(int)MCPRESS_STEP.ePRESS_MC_STEP_SET_CALIBRATION] == MC_RESULT.ePASS)
            //    {
            //        MSystem.MyMsgMemo($"Calibaration Press M/C Right Success!", "Info", msgButton.OK, msgIcon.Infor);
            //        break;
            //    }
            //    if (_timDelay.MoreThan(30))
            //    {
            //        MSystem.MyMsgMemo($"Press MC Right Not response!", "Error", msgButton.OK, msgIcon.Infor);
            //        break;
            //    }
            //}
        }
        #endregion

        #region //Event Hinger Click
        private void TB_HINGER_USE_Click(object sender, EventArgs e)
        {
            //InforManager.Instance.HingeUse = !InforManager.Instance.HingeUse;

            //if (InforManager.Instance.HingeUse)
            //{
            //    MSystem.m_pHingerMCPort.GetReadyCMD();
            //}
            //InforManager.Instance.SaveSettings();
            //if (InforManager.Instance.HingeUse) TB_HINGER_USE.Text = "USE";
            //else TB_HINGER_USE.Text = "NOT_USE";
        }

        private void BT_GET_HINGER_READY_Click(object sender, EventArgs e)
        {
            //MSystem.m_pHingerMCPort.GetReadyCMD();
            //m_iTestCMD.StartTimer();
            //while (true)
            //{
            //    if (MSystem.m_pHingerMCPort.m_iResult[(int)MCHINGER_STEP.eHINGER_MC_STEP_GET_READY] == MC_RESULT.ePASS)
            //    {
            //        MSystem.MyMsgMemo("Check interace OK!!", "Memo", msgButton.OK, msgIcon.Infor);
            //        break;
            //    }
            //    if (m_iTestCMD.MoreThan(5))
            //    {
            //        MSystem.MyMsgMemo("Hinger MC Not READY", "Memo", msgButton.OK, msgIcon.Infor);
            //        break;
            //    }
            //}
        }

        private void TB_HINGER_RUN_Click(object sender, EventArgs e)
        {
            //MSystem.m_pHingerMCPort.SetRunCMD();
            //m_iTestCMD.StartTimer();
            //while (true) {
            //    if (MSystem.m_pHingerMCPort.m_iResult[(int)MCHINGER_STEP.eHINGER_MC_STEP_SET_RUN] == MC_RESULT.ePASS) {
            //        MSystem.MyMsgMemo("Hinger MC Running....!!","Memo",msgButton.OK,msgIcon.Infor);
            //        break;
            //    }
            //    if (m_iTestCMD.MoreThan(5)) {
            //        MSystem.MyMsgMemo("Hinger MC Not Response", "Memo", msgButton.OK, msgIcon.Infor);
            //        break;
            //    }
            //}
        }
        #endregion
    }
}

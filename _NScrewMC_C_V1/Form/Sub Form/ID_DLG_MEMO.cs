using System;
using System.Drawing;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FMsgMemo : Form
    {
        Image _ImgQuestion = Properties.Resources.question; //Image.FromFile("Image/res/question.bmp");
        Image _ImgInfor = Properties.Resources.warning; //Image.FromFile("Image/res/warning.bmp");
        Image _ImgError = Properties.Resources.error1; //Image.FromFile("Image/res/error.bmp");
        Image _ImgSuccess = Properties.Resources.start;
        public msgIcon _Icon = new msgIcon();
        public msgButton _Btmsg = new msgButton();
        public string strmsg = "";
        private static FMsgMemo MsgBox;
        public static bool KillLife = false;
        public TimerDelay m_TimeShiftFullAlarm = new TimerDelay();
        Timer CheckCondition = new Timer();

        public FMsgMemo(msgIcon _icon)
        {
            InitializeComponent();

            BT_YES.DialogResult = DialogResult.Yes;
            BT_NO.DialogResult = DialogResult.No;
            BT_OK.DialogResult = DialogResult.OK;

            CheckCondition.Tick += new EventHandler(UpdateStatus);
            CheckCondition.Interval = 50;
            CheckCondition.Start();
            this.TopMost = true;
        }

        void UpdateStatus(object sender, EventArgs e)
        {
            if (KillLife)
            {
                CheckCondition.Stop();
                KillLife = false;
                this.Close();
            }
        }

        public static DialogResult Show(string contentErr, string strTitle, msgButton _bt, msgIcon _icon)
        {
            MsgBox = new FMsgMemo(_icon);
            MsgBox.strmsg = contentErr;
            MsgBox._Btmsg = _bt;
            MsgBox._Icon = _icon;
            MsgBox.Text = strTitle;
            MsgBox.InitMsg(_icon);
            return MsgBox.ShowDialog();
        }

        private void MyMessenger_Load(object sender, EventArgs e)
        {

        }

        public void InitMsg(msgIcon _icon)
        {
            //Image Memo
            switch (_icon)
            {
                case msgIcon.Question:
                    BT_IMAGE.BackgroundImage = _ImgQuestion;
                    break;
                case msgIcon.Infor:
                    BT_IMAGE.BackgroundImage = _ImgInfor;
                    break;
                case msgIcon.Error:
                    BT_IMAGE.BackgroundImage = _ImgError;
                    break;
                case msgIcon.Success:
                    BT_IMAGE.BackgroundImage = _ImgSuccess;
                    break;
                case msgIcon.eMessgMax:
                default:
                    BT_IMAGE.BackgroundImage = null;
                    break;
            }
            //Button Result
            switch (_Btmsg)
            {
                case msgButton.YESNO:
                    BT_YES.Visible = true;
                    BT_NO.Visible = true;
                    BT_OK.Visible = false;
                    break;
                case msgButton.OK:
                    BT_YES.Visible = false;
                    BT_NO.Visible = false;
                    BT_OK.Visible = true;
                    break;
                case msgButton.eBtMax:
                default:
                    BT_YES.Visible = false;
                    BT_NO.Visible = false;
                    BT_OK.Visible = true;
                    break;
            }
            LB_MSG.Text = strmsg;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (MSystem.isStartForMemo) // add 250705 jlyoon
            {
                MSystem.isStartForMemo = false;
                if (strmsg.Contains("Light Curtain Detected"))
                    Close();           
            }

        }

        private void FMsgMemo_FormClosed(object sender, FormClosedEventArgs e)
        {
            MSystem.m_pTrsBuzzer.BuzzerOff();
            CheckCondition.Stop();
            CheckCondition.Dispose();
        }
    }
}

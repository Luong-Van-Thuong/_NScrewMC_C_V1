using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FMsgDisplay : Form
    {
        public static string strMsg = "";
        public static bool KillLife = false;
       
        private static FMsgDisplay MsgBox;
        Timer CheckCondition = new Timer();
        public FMsgDisplay()
        {
            InitializeComponent();
            CheckCondition.Tick += new EventHandler(UpdateStatus);
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + ((FormMain.m_Width - this.Width) / 2)
                                   , FormMain.GetLocation().Y + ((FormMain.m_Height - this.Height) / 2));
            CheckCondition.Interval = 50;
            CheckCondition.Start();
        }
        void UpdateStatus(object sender, EventArgs e)
        {
            if (KillLife)
            {
                CheckCondition.Stop();
                KillLife = false;
                this.Close();
                this.Dispose();
                return;
            }
            if (LB_msg.Text.Trim() != strMsg)
            {
                LB_msg.Text = strMsg;
            }
        }
        public static void  ShowMsg(string contentErr)
        {
            FMsgDisplay.KillLife = false;
            MsgBox = new FMsgDisplay();
            strMsg = contentErr;
            MsgBox.ShowDialog();
        }

        private void FMsgDisplay_Load(object sender, EventArgs e)
        {

        }
    }
}

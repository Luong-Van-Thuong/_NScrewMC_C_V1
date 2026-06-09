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
    public partial class FormMode : Form
    {
        #region//Form Initial
        CBbutton[] m_btmode = new CBbutton[2];
        ModeRun OldMode = new ModeRun();
        public FormMode()
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + (FormMain.m_Width - this.Width) / 2, FormMain.GetLocation().Y + (FormMain.m_Height - this.Height) / 2);
            //GrBoxHand.Paint += PaintBorderlessGroupBox;
            //GrBoxCV.Paint += PaintBorderlessGroupBox;
            int idx = 0;
            m_btmode[idx++] = new CBbutton(btauto, false);
            m_btmode[idx++] = new CBbutton(btdry, false);
            for (int i = 0; i < 2; i++) {
                m_btmode[i].Button.Click += new EventHandler(PreLoadEvent);
            }
       
            m_btmode[(int)MSystem.SysMode].SetValue(CBbutton.SelectColor);

            MSystem.m_bIsTeachDlg = true;
        }
        private void PreLoadEvent(object sender, EventArgs e)
        {
            for (int i = 0; i < 2; i++) {
                if ((sender as Button) == m_btmode[i].Button)
                    m_btmode[i].SetValue(CBbutton.SelectColor);
                else
                    m_btmode[i].SetValue(CBbutton.NormalColor);
            }
        }
        private void FormOrigin_FormClosed(object sender, FormClosedEventArgs e)
        {
            MSystem.m_bIsTeachDlg = false;
        }
        #endregion

        #region //Event Form
        //Draw again Boder style groupbox
        private void PaintBorderlessGroupBox(object sender, PaintEventArgs e)
        {
            Size textSize = TextRenderer.MeasureText((sender as GroupBox).Text, new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel));
            Graphics gfx = e.Graphics;
            Pen p = new Pen(Color.BlueViolet, 2);
            int heightTex = textSize.Height / 2 + 5;
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            gfx.DrawLine(p, 0, heightTex, 0, e.ClipRectangle.Height - 2);
            gfx.DrawLine(p, 0, heightTex , 10, heightTex);
            gfx.DrawLine(p, 10 + textSize.Width + 3, heightTex, e.ClipRectangle.Width - 2, heightTex);
            gfx.DrawLine(p, e.ClipRectangle.Width - 2, 12, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);
            gfx.DrawLine(p, e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2, 0, e.ClipRectangle.Height - 2);
        }
        private void BtExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void ChangeMode_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                if (m_btmode[i].m_iSel) {
                    if (OldMode == (ModeRun)i) {

                        Close();
                        return;
                    }

                    if (MSystem.MyMsgMemo($"Are You Want Change Mode Run To {((ModeRun)i).ToString()} ?", "Memo", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
                    {
                        /***************************************/
                        //lock (MSystem.LockTactime) {
                        //    InforProduct.Instance.LoadSetting();
                        //    MSystem._tactime.Clear();
                        //}
                        /***************************************/
                        //InforProduct.Instance.UpdateTactimeDisplay();
                        MSystem.SysMode = (ModeRun)i;

                        // Log Moode
                        if (MSystem.SysMode == ModeRun.Auto)
                            MSystem.m_pLogSave.ErrorStopTimeLogSave("Auto Run,,");
                        else if (MSystem.SysMode == ModeRun.DryRun)
                            MSystem.m_pLogSave.ErrorStopTimeLogSave("Dry Run,,");

                        MSystem.MyMsgMemo($"Please Initial affter change mode run!!!", "Memo", msgButton.OK, msgIcon.Infor);
                        this.Close();
                        return;
                    }
                    else {
                        return;
                    }
                }
            }
        }

        private void FormMode_Load(object sender, EventArgs e)
        {
            OldMode = MSystem.SysMode;
        }
    }
}

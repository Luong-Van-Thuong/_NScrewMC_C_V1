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
    public partial class ID_DLG_DATA_REGULATOR : Form
    {
        public ID_DLG_DATA_REGULATOR()
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + (FormMain.m_Width - this.Width) / 2, FormMain.GetLocation().Y + (FormMain.m_Height - this.Height) / 2);
        }
        private void PreLoadEvent(object sender, EventArgs e)
        {
            if ((sender as SUserControls.ColorButton).Text == "Exit")
            {
                this.Close();
                return;
            }
        }

        private void TXT_PRESSURE_Click(object sender, EventArgs e)
        {
            KeyboardNum getValue = new KeyboardNum();
            //
            getValue.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
            DialogResult Result = getValue.ShowDialog();
            if (Result == DialogResult.OK)
            {
                double TEMP;
                if (double.TryParse(getValue.TxValue.Text, out TEMP))
                    (sender as SUserControls.ColorButton).Text = TEMP.ToString();//getValue.TxValue.Text;
            }
            //MSystem.m_pITV.SendStartCMD(double.Parse(TXT_PRESSURE.Text));

            //TimerDelay _timeCheck = new TimerDelay();
            //while (true) 
            //{
            //    if (_timeCheck.MoreThan(3.0) || MSystem.m_pITV.m_bReply) break;
            //    Thread.Sleep(20);
            //}
            //if (MSystem.m_pITV.m_bReply)
            //{
            //    MSystem.MyMsgMemo("Set Value for ITV OK", "Memo", msgButton.OK, msgIcon.Infor);
            //    InforManager.Instance.ReguratorValue = double.Parse(TXT_PRESSURE.Text);
            //    InforManager.Instance.SaveData();
            //}
            //else 
            //{
            //    MSystem.MyMsgMemo("Set Value for ITV fail. Not Responed", "Error", msgButton.OK, msgIcon.Infor);
            //    TXT_PRESSURE.Text = $"{InforManager.Instance.ReguratorValue}";
            //}
        }

        private void ID_DLG_DATA_REGULATOR_Load(object sender, EventArgs e)
        {
            //TXT_PRESSURE.Text = $"{InforManager.Instance.ReguratorValue}";
            //MSystem.m_pITV.SendStartCMD(double.Parse(TXT_PRESSURE.Text));
        }
    }
}

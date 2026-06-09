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
    public partial class FormTimeAndDelay : Form
    {
        public FormTimeAndDelay()
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + ((FormMain.m_Width - this.Width) / 2)
                                    , FormMain.GetLocation().Y + ((FormMain.m_Height - this.Height) / 2));
            LoadUI();
            
        }
        public void LoadUI()
        {
            
        }
        private void SaveDataUI()
        {
           
        }
        void PreLoadEvent(object sender, EventArgs e)
        {
            float _fValue = 0;
            //int _IValue = 0;
            switch ((sender as SUserControls.ColorButton).Name)
            {
                case "BtSave":
                    SaveDataUI();
                    InforManager.Instance.SaveData();
                    MSystem.MyMsgMemo("Save Data Complete", "Save Data", msgButton.OK, msgIcon.Success);
                    this.Close();
                    break;
                case "BtExit":
                    this.Close();
                    break;
                case "TB_SHUTTLE_CENTERING_ON":
                case "TB_SHUTTLE_CENTERING_OFF":
                case "TB_SHUTTLE_SET_ON":
                case "TB_TRANSFER_GRIP_ON":
                case "TB_TRANSFER_GRIP_OFF":
                case "TB_BG_VACUUM_ON":
                case "TB_BG_VACUUM_OFF":
                case "TB_ATT_PUSH_DELAY":
                case "TB_ATT_VACUUM_ON":
                case "TB_ATT_VACUUM_OFF":
                case "TB_ATT_JIG_CENTERING_ON":
                case "TB_ATT_JIG_CENTERING_OFF":
                case "TB_TIME_FEEDER_RD":
                case "TB_TIME_BUZZER":
                    string _tmp = (sender as SUserControls.ColorButton).Text;
                    if (MSystem.GetFloat(sender, ref _fValue))
                    {
                        (sender as SUserControls.ColorButton).Text = $"{_fValue.ToString("f02")} sec";//$"{_fValue}";
                    }
                    else
                        (sender as SUserControls.ColorButton).Text = _tmp;
                    break;
                default: break;
            }
        }
       
    }
}

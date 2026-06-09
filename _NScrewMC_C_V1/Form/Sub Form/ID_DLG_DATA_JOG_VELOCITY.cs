using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormJogVelocity : Form
    {
        SUserControls.ColorButton[] m_btnLow = new SUserControls.ColorButton[(int)Axis.eAXIS_MAX];
        SUserControls.ColorButton[] m_btnMiddle = new SUserControls.ColorButton[(int)Axis.eAXIS_MAX];
        SUserControls.ColorButton[] m_btnHigh = new SUserControls.ColorButton[(int)Axis.eAXIS_MAX];

        public FormJogVelocity()
        {
            InitializeComponent();

            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + (FormMain.m_Width - this.Width) / 2, FormMain.GetLocation().Y + (FormMain.m_Height - this.Height) / 2);

            m_btnLow[(int)Axis.AXIS_X1]                     = Axis0SpeedLow;
            m_btnLow[(int)Axis.AXIS_Y1]                     = Axis1SpeedLow;
            m_btnLow[(int)Axis.AXIS_Z1]                     = Axis2SpeedLow;
            m_btnLow[(int)Axis.AXIS_X2]                     = Axis3SpeedLow;
            m_btnLow[(int)Axis.AXIS_Y2]                     = Axis4SpeedLow;
            m_btnLow[(int)Axis.AXIS_Z2]                     = Axis5SpeedLow;

            m_btnMiddle[(int)Axis.AXIS_X1]                  = Axis0SpeedMiddle;
            m_btnMiddle[(int)Axis.AXIS_Y1]                  = Axis1SpeedMiddle;
            m_btnMiddle[(int)Axis.AXIS_Z1]                  = Axis2SpeedMiddle;
            m_btnMiddle[(int)Axis.AXIS_X2]                  = Axis3SpeedMiddle;
            m_btnMiddle[(int)Axis.AXIS_Y2]                  = Axis4SpeedMiddle;
            m_btnMiddle[(int)Axis.AXIS_Z2]                  = Axis5SpeedMiddle;

            m_btnHigh[(int)Axis.AXIS_X1]                    = Axis0SpeedHight;
            m_btnHigh[(int)Axis.AXIS_Y1]                    = Axis1SpeedHight;
            m_btnHigh[(int)Axis.AXIS_Z1]                    = Axis2SpeedHight;
            m_btnHigh[(int)Axis.AXIS_X2]                    = Axis3SpeedHight;
            m_btnHigh[(int)Axis.AXIS_Y2]                    = Axis4SpeedHight;
            m_btnHigh[(int)Axis.AXIS_Z2]                    = Axis5SpeedHight;

        }
        private void FLoad(object sender, EventArgs e)
        {
            LoadUI();
        }

        private void PreLoadEvent(object sender, EventArgs e)
        {
            if ((sender as SUserControls.ColorButton).Text == "Exit")
            {
                this.Close();
                return;
            }
            else if ((sender as SUserControls.ColorButton).Text == "Save")
            {
                SaveData();
                return;
            }

            KeyboardNum getValue = new KeyboardNum();
            
            getValue.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
            DialogResult Result = getValue.ShowDialog();
            if (Result == DialogResult.OK){   
                int TEMP;
                if (int.TryParse(getValue.TxValue.Text, out TEMP))
                    (sender as SUserControls.ColorButton).Text = TEMP.ToString();//   getValue.TxValue.Text;
            }
            getValue.Dispose();
            getValue = null;
        }

        private void SaveData() {
            if (MSystem.MyMsgMemo("Are you want saves all parameter?","Save Data", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(100);

                    for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
                    {
                        ServoParam.Instance.m_sAxisInfo[i].dJogSlowSpeed = double.Parse(m_btnLow[i].Text.Trim());
                        ServoParam.Instance.m_sAxisInfo[i].dJogMidSpeed = double.Parse(m_btnMiddle[i].Text.Trim());
                        ServoParam.Instance.m_sAxisInfo[i].dJogFastSpeed = double.Parse(m_btnHigh[i].Text.Trim());

                        if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i] != null)
                        {
                            MSystem.SetMsgDisplay($"Save Data {MSystem.m_pAxisManager.strAxisName[i]} ... ");
                            MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].SetAxisInfo(ServoParam.Instance.m_sAxisInfo[i]);
                            MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].SaveData();
                        }
                    }

                    ServoParam.Instance.SaveSettings();
                    MSystem.MyMessagerBottom("Change Parameter Servo Jog Speed");
                    MSystem.KillMsgDisplay();
                    await Task.Delay(100);
                    MSystem.MyMsgMemo("Save Data OK", "Save Data", msgButton.OK, msgIcon.Infor);
                    
                });
                MSystem.MyMsgDisplay("Wait a moment...");
            }
        
        }
        private void LoadUI() {
            for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
            {
                m_btnLow[i].Text = ServoParam.Instance.m_sAxisInfo[i].dJogSlowSpeed.ToString();
                m_btnMiddle[i].Text = ServoParam.Instance.m_sAxisInfo[i].dJogMidSpeed.ToString();
                m_btnHigh[i].Text = ServoParam.Instance.m_sAxisInfo[i].dJogFastSpeed.ToString();
            }
        }
    }
}

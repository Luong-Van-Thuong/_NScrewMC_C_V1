using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormMotorVelocity : Form
    {
        #region // Form Init
        SUserControls.ColorButton[] m_btnVelocity = new SUserControls.ColorButton[(int)Axis.eAXIS_MAX];
        SUserControls.ColorButton[] m_btnAccelerate = new SUserControls.ColorButton[(int)Axis.eAXIS_MAX];

        public FormMotorVelocity()
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + (FormMain.m_Width - this.Width) / 2, FormMain.GetLocation().Y + (FormMain.m_Height - this.Height) / 2);
          
            m_btnVelocity[(int)Axis.AXIS_X1] = Axis0WorkSpeed;
            m_btnVelocity[(int)Axis.AXIS_X2] = Axis1WorkSpeed;
            m_btnVelocity[(int)Axis.AXIS_Y1] = Axis2WorkSpeed;
            m_btnVelocity[(int)Axis.AXIS_Y2] = Axis3WorkSpeed;
            m_btnVelocity[(int)Axis.AXIS_Z1] = Axis4WorkSpeed;
            m_btnVelocity[(int)Axis.AXIS_Z2] = Axis5WorkSpeed;

            m_btnAccelerate[(int)Axis.AXIS_X1] = Axis0AccSpeed;
            m_btnAccelerate[(int)Axis.AXIS_X2] = Axis1AccSpeed;
            m_btnAccelerate[(int)Axis.AXIS_Y1] = Axis2AccSpeed;
            m_btnAccelerate[(int)Axis.AXIS_Y2] = Axis3AccSpeed;
            m_btnAccelerate[(int)Axis.AXIS_Z1] = Axis4AccSpeed;
            m_btnAccelerate[(int)Axis.AXIS_Z2] = Axis5AccSpeed;
        }
        private void FLoad(object sender, EventArgs e)
        {
            LoadUI();
        }
        #endregion

        private void PreLoadEvent(object sender, EventArgs e)
        {
            var btn = (sender as SUserControls.ColorButton);

            if ((sender as SUserControls.ColorButton).Name == "BtExit")
            {
                this.Close();
                return;
            }
            else if ((sender as SUserControls.ColorButton).Name == "BtSave")
            {
                SaveData();
                return;
            }
            else
            {
                using (var kbDlg = new KeyboardNum())
                {
                    
                    kbDlg.TxCurrent.Text = btn.Text;
                    if (kbDlg.ShowDialog() == DialogResult.OK)
                    {
                        if (double.TryParse(kbDlg.TxValue.Text, out var dValue))
                        {
                            double min = btn.Name == "BT_SPEED_SCREW" ? 10 : 50;
                            double max = btn.Name == "BT_SPEED_SCREW" ? 150 : 2500;
                            if (dValue < min || dValue > max)
                            {
                                MSystem.MyMsgMemo($"Input Data Not Correct.\nPlease input value in range [{min} - {max}]", "Error",
                                    msgButton.OK, msgIcon.Error);
                                return;
                            }


                            btn.Text = $"{dValue}";//   getValue.TxValue.Text;
                        }
                    }
                }
            }
        }

        private void SaveData()
        {
            if (MSystem.MyMsgMemo("Are you want saves all parameter?", "Save Data", msgButton.YESNO, msgIcon.Question) == DialogResult.Yes)
            {
                InforManager.Instance.m_dSpeedScrew = double.Parse(BT_SPEED_SCREW.Text.Trim());
                InforManager.Instance.SaveData();

                Task.Run(async () =>
                {
                    await Task.Delay(100);
                    for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
                    {
                        ServoParam.Instance.m_sAxisInfo[i].dAxisMaxSpeed = double.Parse(m_btnVelocity[i].Text.Trim());
                        ServoParam.Instance.m_sAxisInfo[i].dAxisAcc = double.Parse(m_btnAccelerate[i].Text.Trim());
                        if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i] != null)
                        {
                            MSystem.SetMsgDisplay($"Save Data {MSystem.m_pAxisManager.strAxisName[i]} ... ");
                            MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].SetAxisInfo(ServoParam.Instance.m_sAxisInfo[i]);
                            MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].SaveData();
                        }
                    }

                    ServoParam.Instance.SaveSettings();
                    MSystem.MyMessagerBottom("Change Parameter Servo Run Speed");
                    MSystem.KillMsgDisplay();
                    await Task.Delay(100);
                    MSystem.MyMsgMemo("Save Data OK", "Save Data", msgButton.OK, msgIcon.Infor);
                });
                MSystem.MyMsgDisplay("Wait a moment ... ");
            }
        }
        private void LoadUI()
        {
            for (int i = 0; i < MSystem.eAXIS_MAX; i++)
            {
                m_btnVelocity[i].Text = ServoParam.Instance.m_sAxisInfo[i].dAxisMaxSpeed.ToString();
                m_btnAccelerate[i].Text = ServoParam.Instance.m_sAxisInfo[i].dAxisAcc.ToString();
            }
            BT_SPEED_SCREW.Text = InforManager.Instance.m_dSpeedScrew.ToString("f02");
        }
    }
}

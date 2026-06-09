using System;
using System.Drawing;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormPortSetting : Form
    {
        public FormPortSetting()
        {
            InitializeComponent();
            LoadUI();
        }
        public void LoadUI()
        {
            TB_JOG_COM.Text = InforManager.Instance.JogPort;
            TB_SCREW_1_DRIVER_PORT.Text = InforManager.Instance.HantasPort[0];
            TB_SCREW_2_DRIVER_PORT.Text = InforManager.Instance.HantasPort[1];
            BarcodeLeftPort.Text = InforManager.Instance.BarcodePort[0];
            BarcodeRightPort.Text = InforManager.Instance.BarcodePort[1];
        }

        void PreLoadEvent(object sender, EventArgs e)
        {
            //float _fValue = 0;
            int _IValue = 0;
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

                case "TB_JOG_COM":
                case "TB_SCREW_1_DRIVER_PORT":
                case "TB_SCREW_2_DRIVER_PORT":
                case "BarcodeLeftPort":
                case "BarcodeRightPort":
                    if (MSystem.GetIn2(sender, ref _IValue))
                    {
                        (sender as SUserControls.ColorButton).Text = $"COM{_IValue}";
                    }
                    break;
                case "button_BarcodeOpenL":
                    if (MSystem.m_pBcr[0].Open() == true)
                    {
                        MessageBox.Show($"Barcode {InforManager.Instance.BarcodePort[0]} Open Success");
                    }
                    else
                    {
                        MessageBox.Show($"Barcode {InforManager.Instance.BarcodePort[0]} Open Faile");
                    }
                    break;
                case "button_BarcodeTrigL":
                    BarcodeTrig(0);
                    break;
                case "button_BarcodeOpenR":
                    if (MSystem.m_pBcr[1].Open() == true)
                    {
                        MessageBox.Show($"Barcode {InforManager.Instance.BarcodePort[1]} Open Success");
                    }
                    else
                    {
                        MessageBox.Show($"Barcode {InforManager.Instance.BarcodePort[1]} Open Faile");
                    }
                    break;
                case "button_BarcodeTrigR":
                    BarcodeTrig(1);
                    break;
                case "TcpClientUse":
                    if ((sender as SUserControls.ColorButton).Text == "Not Use")
                    {
                        (sender as SUserControls.ColorButton).Text = "Use";
                    }
                    else
                    {
                        (sender as SUserControls.ColorButton).Text = "Not Use";
                    }
                    break;
                default:
                    break;
            }
        }
        private void SaveDataUI()
        {
            InforManager.Instance.JogPort = TB_JOG_COM.Text;
            InforManager.Instance.HantasPort[0] = TB_SCREW_1_DRIVER_PORT.Text;
            InforManager.Instance.HantasPort[1] = TB_SCREW_2_DRIVER_PORT.Text;
            InforManager.Instance.BarcodePort[0] = BarcodeLeftPort.Text;
            InforManager.Instance.BarcodePort[1] = BarcodeRightPort.Text;
            InforManager.Instance.SaveData();
        }
        private void BarcodeTrig(int Index)
        {
            if (Index == 0)
                label_BarcodeL.Text = "";
            else if (Index == 1)
                label_BarcodeR.Text = "";
            MSystem.m_pBcr[Index].SendBcrTrig();
            TimerDelay td = new TimerDelay();
            td.StartTimer();
            while (true)
            {
                if (MSystem.m_pBcr[Index].IsDataReceived == true)
                {
                    if (Index == 0)
                        label_BarcodeL.Text = MSystem.m_pBcr[Index].BarcodeData;
                    else if (Index == 1)
                        label_BarcodeR.Text = MSystem.m_pBcr[Index].BarcodeData;
                    break;
                }
                else if (td.MoreThan(2) == true)
                {
                    if (Index == 0)
                        label_BarcodeL.Text = "FAIL";
                    else if (Index == 1)
                        label_BarcodeR.Text = "FAIL";
                    break;
                }
            }
        }
    }
}

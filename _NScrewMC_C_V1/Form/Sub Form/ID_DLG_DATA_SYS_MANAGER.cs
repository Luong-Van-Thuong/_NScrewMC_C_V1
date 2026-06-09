using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormManagerInfor : Form
    {
        bool IsSaveButtonStatus = false;    
        public FormManagerInfor()
        {
            InitializeComponent();

            LoadUI();
            timer1.Start();
        }
        public void LoadUI()
        {
            if (InforManager.Instance.IsAngleControl == true)
            { 
                TB_A2_ANGLE.Visible= true;
                TB_A2_RETRY_ANGLE.Visible= true;
                LB_A2_RETRY_ANGLE.Visible = true;
                LB_A2_ANGLE.Visible = true;
            }
           else
            {

                TB_A2_ANGLE.Visible = false;
                TB_A2_RETRY_ANGLE.Visible = false;
                LB_A2_RETRY_ANGLE.Visible = false;
                LB_A2_ANGLE.Visible = false;

            }
            // System
            buttonSmartKit.Text = InforManager.Instance.IsSmartKitUse == true ? "Use" : "Not Use";
            buttonIsScrewPickupReady.Text = InforManager.Instance.IsScrewPickupReady == true ? "Use" : "Not Use";
            //buttonAutoAlign.Text = InforManager.Instance.IsAutoAlign == true ? "Use" : "Not Use";
            buttonLeftUnitUse.Text = InforManager.Instance.IsLeftUnitUse == true ? "Use" : "Not Use";
            buttonRightUnitUse.Text = InforManager.Instance.IsRightUnitUse == true ? "Use" : "Not Use";
            buttonGeimUse.Text = InforManager.Instance.GeimUse == true ? "Use" : "Not Use";
            //TB_CONTINUES_SCREW.Text = InforManager.Instance.ContinuesScrew == true ? "Use" : "Not Use";
            TB_RUN_SCREW_ALIGN.Text = InforManager.Instance.RunScrewAlign == true ? "Use" : "Not Use";
            buttonBarcodeUse.Text = InforManager.Instance.IsBarcodeUse == true ? "Use" : "Not Use";
            buttonDetectSetUse.Text = InforManager.Instance.IsDetectSetUse == true ? "Use" : "Not Use";
			buttonReworkUse.Text = InforManager.Instance.IsUseRework == true ? "Use" : "Not Use";
            //buttonAlignType.Text = InforManager.Instance.IsAlignType == true ? "2 Points" : "All Points";
            TB_VISION_TYPE.Text = InforManager.Instance.m_bVisionType == true ? "Library" : "OpenCV";
            buttonInspectionType.Text = InforManager.Instance.InspectionType;
            buttonPickupVaccumMode.Text = InforManager.Instance.IsPickupVaccumMode == true ? "Use" : "Not Use";
            buttonScrewNgVaccumOff.Text = InforManager.Instance.IsScrewNgVaccumOff == true ? "Use" : "Not Use";
            buttonYAxisReverce.Text = InforManager.Instance.IsYAxisReverce == true ? "Use" : "Not Use";
            // Vision Align
            if (InforManager.Instance.AutoVisionAlignMode == 0)
                buttonAutoAlign.Text = "Not Use";
            else if (InforManager.Instance.AutoVisionAlignMode == 1)
                buttonAutoAlign.Text = "2 Points";
            else if (InforManager.Instance.AutoVisionAlignMode == 2)
                buttonAutoAlign.Text = "All Points";

            BT_VISION_ALIGN_Z_UP.Text = InforManager.Instance.IsVisionAlignZUpUse == true ? "Use" : "Not Use";
            BT_VISION_SCAN_NG_OUT.Text = InforManager.Instance.IsVisionScanNgOut == true ? "Use" : "Not Use";

            if (InforManager.Instance.InspectionType == "VST"
                || InforManager.Instance.InspectionType == "Tablet Normal")
            {
                InforManager.Instance.AutoVisionAlignMode = 0;
                buttonAutoAlign.Text = "Not Use";
                buttonAutoAlign.Enabled = false;
                buttonAutoAlign.GradientBottom = Color.Gray;
                buttonAutoAlign.GradientTop = Color.Gray;

                InforManager.Instance.IsSmartKitUse = false;
                buttonSmartKit.Text = "Not Use";
                buttonSmartKit.Enabled = false;
                buttonSmartKit.GradientBottom = Color.Gray;
                buttonSmartKit.GradientTop = Color.Gray;

                InforManager.Instance.IsBarcodeUse = false;
                buttonBarcodeUse.Text = "Not Use";
                buttonBarcodeUse.Enabled = false;
                buttonBarcodeUse.GradientBottom = Color.Gray;
                buttonBarcodeUse.GradientTop = Color.Gray;

                InforManager.Instance.IsDetectSetUse = false;
                buttonDetectSetUse.Text = "Not Use";
                buttonDetectSetUse.Enabled = false;
                buttonDetectSetUse.GradientBottom = Color.Gray;
                buttonDetectSetUse.GradientTop = Color.Gray;

                buttonAlignType.Enabled = false;
                buttonAlignType.GradientBottom = Color.Gray;
                buttonAlignType.GradientTop = Color.Gray;
            }
            else
            {
                buttonAutoAlign.Enabled = true;
                buttonAutoAlign.GradientBottom = Color.White;
                buttonAutoAlign.GradientTop = Color.White;

                buttonSmartKit.Enabled = true;
                buttonSmartKit.GradientBottom = Color.White;
                buttonSmartKit.GradientTop = Color.White;

                buttonBarcodeUse.Enabled = true;
                buttonBarcodeUse.GradientBottom = Color.White;
                buttonBarcodeUse.GradientTop = Color.White;

                buttonAlignType.Enabled = true;
                buttonAlignType.GradientBottom = Color.White;
                buttonAlignType.GradientTop = Color.White;

                buttonDetectSetUse.Enabled = true;
                buttonDetectSetUse.GradientBottom = Color.White;
                buttonDetectSetUse.GradientTop = Color.White;
            }

            // Screw Move
            TB_SCREW_SPEED.Text = InforManager.Instance.m_dSpeedScrew.ToString("f02");
            TB_SCREW_START_Z_OFFSET.Text = $"{InforManager.Instance.m_dScrewOffset.ToString("f02")}";
            TB_SCREW_PICKUP_Z_OFFSET.Text = $"{InforManager.Instance.m_dPickupOffset.ToString("f02")}";
            TB_LIMIT_Z_P.Text = $"{InforManager.Instance.m_dLimitZPlus.ToString("f02")}";
            TB_SCREW_VACCUM_OFF_Z_LEVEL.Text = $"{InforManager.Instance.m_dScrewVaccumOffLevelZ.ToString("f02")}";
       
            //Delay Time
            buttonBlowTime.Text = $"{InforManager.Instance.BlowTime:F2}";
            buttonScrewOverTime.Text = $"{InforManager.Instance.ScrewOverTime:F2}";
            TB_TIMEOUT_FEEDER_RD.Text = $"{InforManager.Instance.TimeOutFeederReady:F2}";
            TB_TIMEOUT_BUZZER.Text = $"{InforManager.Instance.TimeBuzze:F2}";
            TB_PICKUP_DELAY.Text = $"{InforManager.Instance.PickupDelayTime:F2}";
            TB_A2_ANGLE.Text = $"{InforManager.Instance.m_iHantasA2AngleControl.ToString("f02")}";
            TB_A2_RETRY_ANGLE.Text = $"{InforManager.Instance.m_iHantasA2AngleRetryControl.ToString("f02")}";

            // Screw Setup
        }

        public void SaveUI()
        {
            bool ProgramRestart = false;
            if (InforManager.Instance.InspectionType != buttonInspectionType.Text)
            {
                ProgramRestart = true;
            }
            //System
            InforManager.Instance.IsSmartKitUse = buttonSmartKit.Text == "Use";
            InforManager.Instance.IsScrewPickupReady = buttonIsScrewPickupReady.Text == "Use";
            //InforManager.Instance.IsAutoAlign = buttonAutoAlign.Text == "Use";
            InforManager.Instance.IsLeftUnitUse = buttonLeftUnitUse.Text == "Use";
            InforManager.Instance.IsRightUnitUse = buttonRightUnitUse.Text == "Use";
            InforManager.Instance.GeimUse = buttonGeimUse.Text == "Use";
            //InforManager.Instance.ContinuesScrew = TB_CONTINUES_SCREW.Text == "Use";
            InforManager.Instance.RunScrewAlign = TB_RUN_SCREW_ALIGN.Text == "Use";
			InforManager.Instance.IsUseRework = buttonReworkUse.Text == "Use";
            InforManager.Instance.IsBarcodeUse = buttonBarcodeUse.Text =="Use";
            InforManager.Instance.IsDetectSetUse = buttonDetectSetUse.Text == "Use";
            //InforManager.Instance.IsAlignType = buttonAlignType.Text == "2 Points";
			InforManager.Instance.InspectionType = buttonInspectionType.Text;
            InforManager.Instance.m_bVisionType = TB_VISION_TYPE.Text == "Library";
            InforManager.Instance.IsPickupVaccumMode = buttonPickupVaccumMode.Text == "Use";
            InforManager.Instance.IsScrewNgVaccumOff = buttonScrewNgVaccumOff.Text == "Use";
            InforManager.Instance.IsYAxisReverce = buttonYAxisReverce.Text == "Use";

            // Vision Align
            if (buttonAutoAlign.Text == "Not Use")
                InforManager.Instance.AutoVisionAlignMode = 0;
            else if (buttonAutoAlign.Text == "2 Points")
                InforManager.Instance.AutoVisionAlignMode = 1;
            else if (buttonAutoAlign.Text == "All Points")
                InforManager.Instance.AutoVisionAlignMode = 2;

            InforManager.Instance.IsVisionAlignZUpUse = BT_VISION_ALIGN_Z_UP.Text == "Use";
            InforManager.Instance.IsVisionScanNgOut = BT_VISION_SCAN_NG_OUT.Text == "Use";

            //Screw Move
            InforManager.Instance.m_dSpeedScrew = double.Parse(TB_SCREW_SPEED.Text.Trim());
            InforManager.Instance.m_dScrewOffset = double.Parse((TB_SCREW_START_Z_OFFSET.Text).Split(' ')[0]);
            InforManager.Instance.m_dPickupOffset = double.Parse((TB_SCREW_PICKUP_Z_OFFSET.Text).Split(' ')[0]);
            InforManager.Instance.m_dLimitZPlus = double.Parse((TB_LIMIT_Z_P.Text).Split(' ')[0]);
            InforManager.Instance.m_dScrewVaccumOffLevelZ = double.Parse((TB_SCREW_VACCUM_OFF_Z_LEVEL.Text).Split(' ')[0]);

            // Delay time
            InforManager.Instance.BlowTime = double.Parse(buttonBlowTime.Text);
            InforManager.Instance.ScrewOverTime = double.Parse(buttonScrewOverTime.Text);
            InforManager.Instance.TimeOutFeederReady = double.Parse(TB_TIMEOUT_FEEDER_RD.Text);
            InforManager.Instance.TimeBuzze = double.Parse(TB_TIMEOUT_BUZZER.Text);
            InforManager.Instance.PickupDelayTime = double.Parse(TB_PICKUP_DELAY.Text);

            InforManager.Instance.m_iHantasA2AngleControl = double.Parse(TB_A2_ANGLE.Text.Trim());
            InforManager.Instance.m_iHantasA2AngleRetryControl = double.Parse(TB_A2_RETRY_ANGLE.Text.Trim());


            // Screw Setup
            if (!InforManager.Instance.IsLeftUnitUse)
                MSystem.m_pLogSave.AddTail(LogIndex.eLogGEIM9020, $"{PortStatus.STATUS}*{_PStatusDetail.NOTUSE}*{_PResult.MAX}*1");
            else
                MSystem.m_pLogSave.AddTail(LogIndex.eLogGEIM9020, $"{PortStatus.STATUS}*{_PStatusDetail.NORMAL}*{_PResult.MAX}*1");
            if (!InforManager.Instance.IsRightUnitUse)
                MSystem.m_pLogSave.AddTail(LogIndex.eLogGEIM9020, $"{PortStatus.STATUS}*{_PStatusDetail.NOTUSE}*{_PResult.MAX}*2");
            else
                MSystem.m_pLogSave.AddTail(LogIndex.eLogGEIM9020, $"{PortStatus.STATUS}*{_PStatusDetail.NORMAL}*{_PResult.MAX}*2");
            
            InforTeaching.Instance.SaveSettings();//test 
            LoadUI();
            MSystem.MyMsgMemo("Save Data Complete", "Save Data", msgButton.OK, msgIcon.Success);
            if (ProgramRestart == true)
            {
                MSystem.MyMsgMemo($"Machine Type Change to [{buttonInspectionType.Text}].\r\nPlease Restart PGM", "PGM Restart", msgButton.OK, msgIcon.Infor);
                Application.ExitThread();
                Environment.Exit(0);
            }
        }
        void PreLoadEvent(object sender, EventArgs e)
        {
            IsSaveButtonStatus = true;
            switch ((sender as SUserControls.ColorButton).Name)
            {
                case "BtSave":
                    SaveUI();
                    MSystem.IsAutoInitillize = true;
                    this.Close();
                    break;
                case "BtExit":
                    this.Close();
                    break;

                case "TB_GMES":
                case "TB_GEIM":
                case "TB_IF_HANDLE":
                case "TB_SCREW_1":
                case "TB_SCREW_2":
                case "buttonSmartKit":
                case "buttonIsScrewPickupReady":
                case "buttonLeftUnitUse":
                case "buttonRightUnitUse":
                case "buttonGeimUse":
                case "TB_CONTINUES_SCREW":
                case "TB_RUN_SCREW_ALIGN":
                case "buttonBarcodeUse":
                case "buttonDetectSetUse":
                case "BT_VISION_ALIGN_Z_UP":
                case "buttonPickupVaccumMode":
                case "BT_VISION_SCAN_NG_OUT":
                case "buttonScrewNgVaccumOff":
                case "buttonYAxisReverce":
                    if ((sender as SUserControls.ColorButton).Text == "Use")
                    {
                        (sender as SUserControls.ColorButton).Text = "Not Use";
                    }
                    else
                    {
                        (sender as SUserControls.ColorButton).Text = "Use";
                    }
                    break;
                case "buttonReworkUse": //Lock Rework Mode -Add Tung 250807
                    FormPassword dlgvalue = new FormPassword("0000"); // Pass word Change 담당자:박정주프로 cnz 260204
                    DialogResult result = dlgvalue.ShowDialog();
                    if (dlgvalue.IsOK == false)
                        return;
                    if ((sender as SUserControls.ColorButton).Text == "Use")
                    {
                        (sender as SUserControls.ColorButton).Text = "Not Use";
                    }
                    else
                    {
                        (sender as SUserControls.ColorButton).Text = "Use";
                    }
                    break;
                case "buttonBlowTime":
                case "buttonScrewOverTime":
                case "TB_TIMEOUT_FEEDER_RD":
                case "TB_TIMEOUT_BUZZER":
                case "TB_PICKUP_DELAY":
                case "TB_SCREW_VACCUM_OFF_Z_LEVEL":
                    KeyboardNum getValue = new KeyboardNum();

                    getValue.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
                    DialogResult Result = getValue.ShowDialog();
                    if (Result == DialogResult.OK)
                    {
                        double temp;
                        if (double.TryParse(getValue.TxValue.Text, out temp))
                            (sender as SUserControls.ColorButton).Text = $"{temp:F2}";
                    }
                    getValue.Dispose();
                    getValue = null;
                    break;

                case "TB_LIMIT_Z_P":
                    using (var kbDlg = new KeyboardNum())
                    {
                        kbDlg.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
                        if (kbDlg.ShowDialog() == DialogResult.OK)
                        {
                            if (double.TryParse(kbDlg.TxValue.Text, out var dValue))
                            {
                                if (dValue < 0)
                                    MSystem.MyMsgMemo($"Input Data Not Correct.\nPlease input value in range [15 - 30]", "Error",
                                        msgButton.OK, msgIcon.Error);
                                else
                                    (sender as SUserControls.ColorButton).Text = $"{dValue}";//   getValue.TxValue.Text;
                            }
                        }
                    }
                    break;

                case "TB_SCREW_MOVE_Z_OFFSET":
                    {
                        using (var kbDlg = new KeyboardNum())
                        {
                            kbDlg.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
                            if (kbDlg.ShowDialog() == DialogResult.OK)
                            {
                                if (double.TryParse(kbDlg.TxValue.Text, out var dValue))
                                {
                                    if (dValue < 15 || dValue > 30)
                                        MSystem.MyMsgMemo($"Input Data Not Correct.\nPlease input value in range [15 - 30]", "Error",
                                            msgButton.OK, msgIcon.Error);
                                    else
                                        (sender as SUserControls.ColorButton).Text = $"{dValue}";//   getValue.TxValue.Text;
                                }
                            }
                        }
                    }
                    break;

                case "TB_SCREW_SPEED":
                    {
                        using (var kbDlg = new KeyboardNum())
                        {
                            kbDlg.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
                            if (kbDlg.ShowDialog() == DialogResult.OK)
                            {
                                if (double.TryParse(kbDlg.TxValue.Text, out var dValue))
                                {
                                    if (dValue < 10 || dValue > 50)
                                        MSystem.MyMsgMemo($"Input Data Not Correct.\nPlease input value in range [10 - 50]", "Error",
                                            msgButton.OK, msgIcon.Error);
                                    else
                                        (sender as SUserControls.ColorButton).Text = $"{dValue}";//   getValue.TxValue.Text;
                                }
                            }
                        }
                    }
                    break;

                case "TB_SCREW_START_Z_OFFSET":
                    {
                        using(var kbDlg = new KeyboardNum())
                        {
                            kbDlg.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
                            if (kbDlg.ShowDialog() == DialogResult.OK)
                            {
                                if (double.TryParse(kbDlg.TxValue.Text, out var dValue))
                                {
                                    if (dValue < 5 || dValue > 50)
                                        MSystem.MyMsgMemo($"Input Data Not Correct.\nPlease input value in range [5 - 50]", "Error",
                                            msgButton.OK, msgIcon.Error);
                                    else
                                        (sender as SUserControls.ColorButton).Text = $"{dValue}";//   getValue.TxValue.Text;
                                }
                            }
                        }
                    }
                    break;

                case "TB_SCREW_PICKUP_Z_OFFSET":
                    {
                        using (var kbDlg = new KeyboardNum())
                        {
                            kbDlg.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
                            if (kbDlg.ShowDialog() == DialogResult.OK)
                            {
                                if (double.TryParse(kbDlg.TxValue.Text, out var dValue))
                                {
                                    // 마이너스 값 입력 가능하도록 수정. 260323 cnz
                                    //if (dValue < 0 || dValue > 5)
                                    //    MSystem.MyMsgMemo($"Input Data Not Correct.\nPlease input value in range [0 - 5]", "Error",
                                    //        msgButton.OK, msgIcon.Error);
                                    //else
                                        (sender as SUserControls.ColorButton).Text = $"{dValue}";//   getValue.TxValue.Text;
                                }
                            }
                        }
                    }
                    break;

                case "TB_A2_ANGLE":
                    {
                        using (var kbDlg = new KeyboardNum())
                        {
                            kbDlg.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
                            if (kbDlg.ShowDialog() == DialogResult.OK)
                            {
                                if (double.TryParse(kbDlg.TxValue.Text, out var dValue))
                                {
                                    if (dValue <= 0)
                                        MSystem.MyMsgMemo($"Input Data Not Correct.\nPlease input value > 0", "Error",
                                            msgButton.OK, msgIcon.Error);
                                    else
                                        (sender as SUserControls.ColorButton).Text = $"{dValue}";//   getValue.TxValue.Text;
                                }
                            }
                        }
                    }
                    break;
                case "TB_A2_RETRY_ANGLE":
                    {
                        using (var kbDlg = new KeyboardNum())
                        {
                            kbDlg.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
                            if (kbDlg.ShowDialog() == DialogResult.OK)
                            {
                                if (double.TryParse(kbDlg.TxValue.Text, out var dValue))
                                {
                                    if (dValue <= 0)
                                        MSystem.MyMsgMemo($"Input Data Not Correct.\nPlease input value > 0", "Error",
                                            msgButton.OK, msgIcon.Error);
                                    else
                                        (sender as SUserControls.ColorButton).Text = $"{dValue}";//   getValue.TxValue.Text;
                                }
                            }
                        }
                    }
                    break;

                case "TB_IDX_MC":
                    getValue = new KeyboardNum();

                    getValue.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
                    Result = getValue.ShowDialog();
                    if (Result == DialogResult.OK)
                    {
                        double TEMP;
                        if (double.TryParse(getValue.TxValue.Text, out TEMP))
                            (sender as SUserControls.ColorButton).Text = $"{TEMP.ToString()}";//   getValue.TxValue.Text;
                    }
                    getValue.Dispose();
                    getValue = null;
                    break;

                case "TB_MODE_MOVE":
                    if ((sender as SUserControls.ColorButton).Text == "Line")
                        (sender as SUserControls.ColorButton).Text = "Circle";
                    else
                        (sender as SUserControls.ColorButton).Text = "Line";
                    break;

                case "TB_VISION_TYPE":
                    if ((sender as SUserControls.ColorButton).Text == "OpenCV")
                        (sender as SUserControls.ColorButton).Text = "Library";
                    else
                        (sender as SUserControls.ColorButton).Text = "OpenCV";
                    break;
                case "buttonInspectionType":
                    if ((sender as SUserControls.ColorButton).Text == "Mobile")
                    {
                        (sender as SUserControls.ColorButton).Text = "Tablet";
                        buttonAutoAlign.Enabled = true;
                        buttonSmartKit.Enabled = true;
                        buttonBarcodeUse.Enabled = true;
                    }
                    else if ((sender as SUserControls.ColorButton).Text == "Tablet")
                    {
                        (sender as SUserControls.ColorButton).Text = "Tablet Normal";
                        InforManager.Instance.AutoVisionAlignMode = 0;
                        buttonAutoAlign.Text = "Not Use";
                        buttonAutoAlign.Enabled = false;

                        InforManager.Instance.IsSmartKitUse = false;
                        buttonSmartKit.Text = "Not Use";
                        buttonSmartKit.Enabled = false;

                        InforManager.Instance.IsBarcodeUse = false;
                        buttonBarcodeUse.Text = "Not Use";
                        buttonBarcodeUse.Enabled = false;

                        InforManager.Instance.IsDetectSetUse = false;
                        buttonDetectSetUse.Text = "Not Use";
                        buttonDetectSetUse.Enabled = false;
                    }
                    else if ((sender as SUserControls.ColorButton).Text == "Tablet Normal")
                    {
                        (sender as SUserControls.ColorButton).Text = "VST";
                    }
                    else if ((sender as SUserControls.ColorButton).Text == "VST")
                    {
                        (sender as SUserControls.ColorButton).Text = "H8";
                    }
                    else
                    {
                        (sender as SUserControls.ColorButton).Text = "Mobile";
                        buttonAutoAlign.Enabled = true;
                        buttonSmartKit.Enabled = true;
                        buttonBarcodeUse.Enabled = true;
                        buttonDetectSetUse.Enabled = true;
                    }
                    break;

                case "buttonProductChk":
                    if ((sender as SUserControls.ColorButton).Text == "Use")
                    {
                        (sender as SUserControls.ColorButton).Text = "Not Use";
                    }
                    else
                    {
                        (sender as SUserControls.ColorButton).Text = "Use";
                    }
                    break;
                case "buttonAutoAlign":
                    if ((sender as SUserControls.ColorButton).Text == "Not Use")
                        (sender as SUserControls.ColorButton).Text = "2 Points";
                    else if ((sender as SUserControls.ColorButton).Text == "2 Points")
                        (sender as SUserControls.ColorButton).Text = "All Points";
                    else
                        (sender as SUserControls.ColorButton).Text = "Not Use";
                    break;

                default: break;
            }
        }

        private void comboIsScrewPickupReady_DrawItem(object sender, DrawItemEventArgs e)
        {
            // By using Sender, one method could handle multiple ComboBoxes
            ComboBox cbx = sender as ComboBox;
            if (cbx != null)
            {
                // Always draw the background
                e.DrawBackground();

                // Drawing one of the items?
                if (e.Index >= 0)
                {
                    // Set the string alignment.  Choices are Center, Near and Far
                    StringFormat sf = new StringFormat();
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Alignment = StringAlignment.Center;

                    // Set the Brush to ComboBox ForeColor to maintain any ComboBox color settings
                    // Assumes Brush is solid
                    Brush brush = new SolidBrush(cbx.ForeColor);

                    // If drawing highlighted selection, change brush
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                        brush = SystemBrushes.HighlightText;

                    // Draw the string
                    e.Graphics.DrawString(cbx.Items[e.Index].ToString(), cbx.Font, brush, e.Bounds, sf);
                }
            }
        }

        private void FormManagerInfor_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (IsSaveButtonStatus == true)
            {
                if (BtSave.GradientBottom != Color.Red)
                {
                    BtSave.GradientBottom = Color.Red;
                    BtSave.GradientTop = Color.Red;
                }
                else
                {
                    BtSave.GradientBottom = Color.Gainsboro;
                    BtSave.GradientTop = Color.WhiteSmoke;
                }
            }
        }
    }
}

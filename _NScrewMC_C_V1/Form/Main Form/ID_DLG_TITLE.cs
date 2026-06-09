using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormTitle : Form
    {
        #region//Variable Form
        Timer timerUpdate = new Timer();
        Stopwatch sw = new Stopwatch();
        TimeSpan remainTime = TimeSpan.FromMinutes(60);
        #endregion

        #region //Form Initial
        public FormTitle()
        {
            InitializeComponent();
            
            MSystem.LoadParameter();
            if (InforManager.Instance.InspectionType == "VST")
                Program.ModelName = $"SCREW-VST-V7.0";
            else if (InforManager.Instance.InspectionType == "Tablet")
                Program.ModelName = $"SCREW-C-V7.0";
            else if (InforManager.Instance.InspectionType == "Tablet Normal")
                Program.ModelName = $"NSCREW-T-V1.0";
            else
                Program.ModelName = $"SCREW-C-V7.0";

            this.timerUpdate.Tick += new EventHandler(UpdateStatus);
            this.Closed += new EventHandler(CloseAllData);
            timerUpdate.Interval = 1000;
            timerUpdate.Enabled = true;

            // Title Name Setting
            BtGetStepJob.Text = Program.ModelName;

        }
        private void CloseAllData(object sender, EventArgs e)
        {
            timerUpdate = null;
        }
        #endregion

        #region //Update Status
        private void UpdateStatus(object sender, EventArgs e)
        {
            if (!Visible) return;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        MSystem.DisPlayString(LB_MODEL, $"Model : {InforManager.Instance.ModelName}");
                        MSystem.DisPlayString(Txt_DateTime, $"{DateTime.Now:yyyy-MM-dd}, {DateTime.Now:HH:mm:ss}");
                        MSystem.DisPlayString(Text_Version, $"Ver : {Program.Version}");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Titile Form Error Logic");
                    }
                }));
            }
            else
            {
                try
                {
                    MSystem.DisPlayString(LB_MODEL, $"Model : {InforManager.Instance.ModelName}");
                    MSystem.DisPlayString(Txt_DateTime, $"{DateTime.Now:yyyy-MM-dd}, {DateTime.Now:HH:mm:ss}");
                    MSystem.DisPlayString(Text_Version, $"Ver : {Program.Version}");
                }
                catch (Exception)
                {
                    MessageBox.Show("Titile Form Error Logic");
                }
            }
            // Door Skip off after 60min auto set use 2025-11-18 cnz
            if (MSystem.IsDoorSkip == true)
            {
                TimeSpan ts = DateTime.Now - MSystem.DoorSkipStartTime;
                if (ts.TotalMinutes >= 60)
                {
                    if (MSystem.m_pDIO.IsOff(IOMap.OUT["OUT_SAFETY_PLC_RESET"]))
                    {
                        MSystem.m_pDIO.OutPutOn(IOMap.OUT["OUT_SAFETY_PLC_RESET"]);
                        sw.Restart();
                    }
                    if (sw.ElapsedMilliseconds > 1000)
                    {
                        MSystem.m_pDIO.OutPutOff(IOMap.OUT["OUT_SAFETY_PLC_RESET"]);
                        System.Threading.Thread.Sleep(500);
                        MSystem.IsDoorSkip = false;
                        sw.Reset();
                    }
                    LABEL_DOOR_OPEN_TIMER.Text = string.Empty;
                    remainTime = TimeSpan.FromMinutes(60);
                }
                else
                {
                    remainTime = remainTime.Subtract(TimeSpan.FromSeconds(1));
                    string tim = remainTime.ToString(@"mm\:ss");
                    LABEL_DOOR_OPEN_TIMER.Text = tim;
                    if (BtGetStepJob.GradientBottom == Color.Orange)
                    {
                        BtGetStepJob.GradientBottom = Color.DodgerBlue;
                        BtGetStepJob.GradientTop = Color.DodgerBlue;
                        LABEL_DOOR_OPEN_TIMER.BackColor = Color.DodgerBlue;
                        LB_MODEL.BackColor = Color.DodgerBlue;
                        Text_Version.BackColor = Color.DodgerBlue;
                        Txt_DateTime.BackColor = Color.DodgerBlue;
                    }
                    else
                    {
                        BtGetStepJob.GradientBottom = Color.Orange;
                        BtGetStepJob.GradientTop = Color.Orange;
                        LABEL_DOOR_OPEN_TIMER.BackColor = Color.Orange;
                        LB_MODEL.BackColor = Color.Orange;
                        Text_Version.BackColor = Color.Orange;
                        Txt_DateTime.BackColor = Color.Orange;
                    }
                }
            }
            else
            {
                if (BtGetStepJob.GradientBottom == Color.Orange)
                {
                    BtGetStepJob.GradientBottom = Color.DodgerBlue;
                    BtGetStepJob.GradientTop = Color.DodgerBlue;
                    LABEL_DOOR_OPEN_TIMER.BackColor = Color.DodgerBlue;
                    LB_MODEL.BackColor = Color.DodgerBlue;
                    Text_Version.BackColor = Color.DodgerBlue;
                    Txt_DateTime.BackColor = Color.DodgerBlue;
                }
                if (!string.IsNullOrEmpty(LABEL_DOOR_OPEN_TIMER.Text))
                    LABEL_DOOR_OPEN_TIMER.Text = "";

                remainTime = TimeSpan.FromMinutes(60);
            }
        }

        #endregion

        private void IDC_THREAD_STEP_Click(object sender, EventArgs e)
        {
            if (!Visible) return;
            this.Invoke((MethodInvoker)delegate
            {
                FormUnitStep dlgUnitStep = new FormUnitStep();
                dlgUnitStep.ShowDialog();
                dlgUnitStep = null;
            });

        }
    }
}

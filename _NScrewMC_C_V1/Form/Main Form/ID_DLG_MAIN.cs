using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace _NScrewMC_C_V1
{
    public partial class FormMain : Form
    {
        #region //Variable
        Timer TimerUpdate = new Timer();
        TimerDelay _InitTime = new TimerDelay();
        Timer TimerOrigin = new Timer();

        public static readonly int m_Width = 1024;
        public static readonly int m_Height = 600;
        #endregion

        public static Point P_Pos = new Point();

        #region //Form Initial
        public FormMain()
        {
            InitializeComponent();
            TimerUpdate.Tick += new EventHandler(UpdateStatus);
            Control.CheckForIllegalCrossThreadCalls = false;

            /*Auto Clean Log*/
            LogCleaner.StartAutoCleanup();

            /*Init all system mode*/
            MSystem.SysMode = ModeRun.Auto;
            MSystem.SysStatus = StatusRun.STOP;

            /*Init Servo Board IO*/
            MSystem.m_pAxisManager.Initialize();
            MSystem.m_pDatabaseLog.CreateTB();
            MSystem.RunAllThread();
            _InitTime.StartTimer();

            TimerUpdate.Tick += UpdateStatus;
            TimerOrigin.Tick += Originfirst;

            this.FormClosing += Formclose;
            MSystem.m_pLogSave.AddTail(LogIndex.eLogGEIM, "9200");
            //this.Text = Program.ModelName;
        }
        public static Point GetLocation()
        {
            Point T = new Point(P_Pos.X, P_Pos.Y);
            return T;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //Initial Form
            // MSystem.
            MSystem.InitialMainForm(PanelMain);
            MSystem.DisplayMain(PanelMain, MSystem.NumDisplay);
            MSystem.DisplayForm(PanelTitle, MSystem.TitleForm);
            MSystem.DisplayForm(PanelButton, MSystem.BottomForm);

            TimerUpdate.Interval = 100;
            TimerUpdate.Enabled = true;

            TimerOrigin.Interval = 1000;
            TimerOrigin.Enabled = true;
            TimerOrigin.Start();

            Control.CheckForIllegalCrossThreadCalls = false;
            MSystem.m_pLogSave.DevLogSave($"Start Program .......");

            MSystem.BottomForm.SetImgSelect((int)NumViewMain.eViewAuto);
            // Log Mode
            MSystem.m_pLogSave.ErrorStopTimeLogSave("Auto Run,,");
        }
        private void Formclose(object sender, EventArgs e)
        {
            MSystem.m_bLife = false;
            MSystem.m_pLogSave.DevLogSave($"Exit program");
        }
        #endregion

        #region //Update Status
        private void UpdateStatus(object Sender, EventArgs e)
        {

            if (MSystem.NumDisplay != 0 && MSystem.NumDisplay != MSystem.NumDisplayOld)
            {
                if (MSystem.NumDisplay != 5)
                {
                    MSystem.NumDisplayOld = MSystem.NumDisplay;
                }

                if (MSystem.NumDisplay == 6)
                    Environment.Exit(0);
                else if (MSystem.NumDisplay == 5)
                {
                    MSystem.NumDisplay = 0;
                    this.WindowState = FormWindowState.Minimized;
                }
                else
                    MSystem.DisplayMain(PanelMain, MSystem.NumDisplay);
            }
        }
        private void Originfirst(object Sender, EventArgs e)
        {
            TimerOrigin.Stop();
            MSystem.SysStatus = StatusRun.STOP;
            FormOrigin formOrigin = new FormOrigin();
            if (MSystem.SysStatus == StatusRun.ERROR)
            {
                TimerOrigin.Stop();
                return;
            }
            for (int i = 0; i < MSystem.eAXIS_MAX; i++)
            {
                formOrigin.m_BtAxis[i].SetValue(CBbutton.SelectColor);
            }
            
            if (MSystem.SysStatus == StatusRun.ERROR) return;

            //for (int i = 0; i < MSystem.eAXIS_MAX; i++)
            //{
            //    formOrigin.m_BtAxis[i].SetValue(CBbutton.NormalColor);
            //}
            formOrigin.ShowDialog();
            MSystem.m_bIsTeachDlg = false;
            MSystem.IsOriginInitFlag = true;
        }
        #endregion
    }

}

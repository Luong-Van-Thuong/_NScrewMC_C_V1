using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class ID_JIG_SCREW_MANUAL : Form
    {
        public int m_hidx = 0;
        System.Timers.Timer m_timer = new System.Timers.Timer();
        public ID_JIG_SCREW_MANUAL(int _idx)
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + (FormMain.m_Width - this.Width) / 2, FormMain.GetLocation().Y + (FormMain.m_Height - this.Height) / 2);
            this.FormClosed += new FormClosedEventHandler(CloseForm);
            m_hidx = _idx;
            m_timer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateStatus);
            m_timer.Interval = 200;
            m_timer.Start();

            if (MSystem.SysStatus == StatusRun.RUN)
            {
                BT_PRESS_RESET.Enabled = false;
                BT_START_MANUAL.Enabled = false;
                IDC_VACUM_ON.Enabled = false;
                IDC_VACUM_OFF.Enabled = false;
                IDC_BLOW_ON.Enabled = false;
                IDC_BLOW_OFF.Enabled = false;
                IDC_DRIVER_RUN.Enabled = false;
                IDC_DRIVER_STOP.Enabled = false;
                IDC_COVER_UP.Enabled = false;
                IDC_COVER_DOWN.Enabled = false;
                IDC_FIX_UP.Enabled = false;
                IDC_FIX_DOWN.Enabled = false;
            }

            UnitUseNotuseStatusUpdate();
        }
        private void CloseForm(object sender, EventArgs e)
        {
            m_timer.Stop();
            m_timer = null;
        }

        private void UpdateStatus(object sender, EventArgs e)
        {
            if (m_hidx < 3)
            {
                // Update Sensor
                MSystem.IsSensorOn(IDC_VACUM_ON, MSystem.m_pTrsScrew[m_hidx].IsVacuumSensorOn());
                MSystem.IsSensorOn(IDC_VACUM_OFF, !MSystem.m_pTrsScrew[m_hidx].IsVacuumSensorOn());
                MSystem.IsSensorOn(IDC_BLOW_ON, MSystem.m_pTrsScrew[m_hidx].IsBlowSolOn());
                MSystem.IsSensorOn(IDC_BLOW_OFF, !MSystem.m_pTrsScrew[m_hidx].IsBlowSolOn());
                MSystem.IsSensorOn(IDC_DRIVER_RUN, MSystem.m_pTrsScrew[m_hidx].IsDriverRun());
                MSystem.IsSensorOn(IDC_DRIVER_STOP, !MSystem.m_pTrsScrew[m_hidx].IsDriverRun());
                //
                MSystem.IsSensorOn(IDC_COVER_UP, MSystem.m_pTrsJig[m_hidx].IsCoverUPFront());
                MSystem.IsSensorOn(IDC_COVER_DOWN, MSystem.m_pTrsJig[m_hidx].IsCoverDownFront());
                MSystem.IsSensorOn(IDC_FIX_UP, MSystem.m_pTrsJig[m_hidx].IsFixUPSol());
                MSystem.IsSensorOn(IDC_FIX_DOWN, MSystem.m_pTrsJig[m_hidx].IsFixDownSol());
            }
        }
        private void BT_CANCEL_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BT_PRESS_RESET_Click(object sender, EventArgs e)
        {
            if (m_hidx < 3)
            {
                MSystem.m_pTrsJig[m_hidx].SetUnitInitialize();
                MSystem.m_pTrsScrew[m_hidx].SetUnitInitialize();
            }
            //this.Close();
        }

        private void IDC_VACUM_ON_Click(object sender, EventArgs e)
        {
            if (m_hidx == MSystem.eLEFT)
            {
                MSystem.m_pTrsScrew[MSystem.eLEFT].VacuumOn();
            }
            else
            {
                MSystem.m_pTrsScrew[MSystem.eRIGHT].VacuumOn();
            }
        }

        private void IDC_VACUM_OFF_Click(object sender, EventArgs e)
        {
            if (m_hidx == MSystem.eLEFT)
            {
                MSystem.m_pTrsScrew[MSystem.eLEFT].VacuumOff();
            }
            else
            {
                MSystem.m_pTrsScrew[MSystem.eRIGHT].VacuumOff();
            }
        }

        private void IDC_BLOW_ON_Click(object sender, EventArgs e)
        {
            if (m_hidx == MSystem.eLEFT)
            {
                MSystem.m_pTrsScrew[MSystem.eLEFT].BlowOn();
            }
            else
            {
                MSystem.m_pTrsScrew[MSystem.eRIGHT].BlowOn();
            }
        }

        private void IDC_BLOW_OFF_Click(object sender, EventArgs e)
        {
            if (m_hidx == MSystem.eLEFT)
            {
                MSystem.m_pTrsScrew[MSystem.eLEFT].BlowOff();
            }
            else
            {
                MSystem.m_pTrsScrew[MSystem.eRIGHT].BlowOff();
            }
        }

        private void IDC_DRIVER_RUN_Click(object sender, EventArgs e)
        {
            if (m_hidx == MSystem.eLEFT)
            {
                MSystem.m_pTrsScrew[MSystem.eLEFT].DriverRun(InforTeaching.Instance.P_Screw[0][0].Channel);
            }
            else
            {
                MSystem.m_pTrsScrew[MSystem.eRIGHT].DriverRun(InforTeaching.Instance.P_Screw[1][0].Channel);
            }
        }

        private void IDC_DRIVER_STOP_Click(object sender, EventArgs e)
        {
            if (m_hidx == MSystem.eLEFT)
            {
                MSystem.m_pTrsScrew[MSystem.eLEFT].DriverStop();
            }
            else
            {
                MSystem.m_pTrsScrew[MSystem.eRIGHT].DriverStop();
            }
        }

        private void IDC_COVER_UP_Click(object sender, EventArgs e)
        {
            if (m_hidx == MSystem.eLEFT)
            {
                MSystem.m_pTrsJig[MSystem.eLEFT].CoverUp();
            }
            else
            {
                MSystem.m_pTrsJig[MSystem.eRIGHT].CoverUp();
            }
        }

        private void IDC_COVER_DOWN_Click(object sender, EventArgs e)
        {
            if (m_hidx == MSystem.eLEFT)
            {
                MSystem.m_pTrsJig[MSystem.eLEFT].CoverDown();
            }
            else
            {
                MSystem.m_pTrsJig[MSystem.eRIGHT].CoverDown();
            }
        }

        private void IDC_FIX_UP_Click(object sender, EventArgs e)
        {
            if (m_hidx == MSystem.eLEFT)
            {
                MSystem.m_pTrsJig[MSystem.eLEFT].FixUP();
            }
            else
            {
                MSystem.m_pTrsJig[MSystem.eRIGHT].FixUP();
            }
        }

        private void IDC_FIX_DOWN_Click(object sender, EventArgs e)
        {
            if (m_hidx == MSystem.eLEFT)
            {
                MSystem.m_pTrsJig[MSystem.eLEFT].FixDown();
            }
            else
            {
                MSystem.m_pTrsJig[MSystem.eRIGHT].FixDown();
            }
        }
        private void ID_JIG_SCREW_MANUAL_Load(object sender, EventArgs e)
        {
            if(m_hidx == MSystem.eLEFT)
                IDC_TITLE_JIG_SCREW.Text = $"Jig Screw Left";
            else if(m_hidx == MSystem.eRIGHT)
                IDC_TITLE_JIG_SCREW.Text = $"Jig Screw Right";
        }

        private void BT_START_MANUAL_Click(object sender, EventArgs e)
        {
            MSystem.m_pTrsOP.OnStartButton();
            this.Close();
        }

        private void buttonUseNotuse_Click(object sender, EventArgs e)
        {
            if  (m_hidx == MSystem.eLEFT)
            {
                if (InforManager.Instance.IsLeftUnitUse != true)
                {
                    InforManager.Instance.IsLeftUnitUse = true;
                }
                else
                {
                    InforManager.Instance.IsLeftUnitUse = false;
                }
            }
            else if (m_hidx == MSystem.eRIGHT)
            {
                if (InforManager.Instance.IsRightUnitUse != true)
                {
                    InforManager.Instance.IsRightUnitUse = true;
                }
                else
                {
                    InforManager.Instance.IsRightUnitUse = false;
                }
            }
            
            UnitUseNotuseStatusUpdate();
        }
        private void UnitUseNotuseStatusUpdate()
        {
            bool isUse = false;
            if (m_hidx == MSystem.eLEFT)
            {
                isUse = InforManager.Instance.IsLeftUnitUse;
            }
            else
            {
                isUse = InforManager.Instance.IsRightUnitUse;
            }
            if (isUse == false)
            {
                buttonUseNotuse.Text = "Not Use";
                buttonUseNotuse.BackColor = Color.Gray;
                MSystem.m_pTrsJig[m_hidx].JigStatus = 2;
            }
            else
            {
                buttonUseNotuse.Text = "Use";
                buttonUseNotuse.BackColor = Color.Green;
                MSystem.m_pTrsJig[m_hidx].JigStatus = 0;
            }
            if (!InforManager.Instance.IsUseRework)
            {
                Rework_NG_BT.Text = "RW Not Use";

                Rework_NG_BT.BackColor = Color.Gray;

            }
        }

        private void BT_ALL_NOT_SKIP_Click(object sender, EventArgs e)
        {
            var screws = InforTeaching.Instance.P_Screw[m_hidx];
            MSystem._Rework[m_hidx] = false;
            for (int i = 0; i < screws.Count; i++)
            {
                screws[i].Skip = false; 
            }

            InforTeaching.Instance.SaveSettings(); //test delete saving
        }

        private void Rework_NG_BT_Click(object sender, EventArgs e)
        {
           
            if(!InforManager.Instance.IsUseRework)//Lock Rework Mode -Add Tung 250807
            {


                Task.Run(async () =>
                {
                    await Task.Delay(10);
                    string msg = " Rework Mode Not Use!";
                     MSystem.MyMsgMemo(msg, "Error", msgButton.OK, msgIcon.Error); 
                  
                });
                return;
                //FormPassword dlgvalue = new FormPassword("11112222");
                //DialogResult result = dlgvalue.ShowDialog();
                //if (dlgvalue.IsOK == false)
                //    return;

            }

            var screws = InforTeaching.Instance.P_Screw[m_hidx];
            MSystem._Rework[m_hidx] = true;
            for (int i = 0; i < screws.Count; i++)
            {
                if (screws[i].status == 2)
                {
                    screws[i].Skip = false;
                }
                else
                {
                    screws[i].Skip=true;
                }
            }
          InforTeaching.Instance.SaveSettings();//test delete saving
        }

        private void BT_REWORK_SEL_Click(object sender, EventArgs e)
        {
            if (!InforManager.Instance.IsUseRework)
            {
                var screws = InforTeaching.Instance.P_Screw[m_hidx];
                MSystem._Rework[m_hidx] = true;
                for (int i = 0; i < screws.Count; i++)
                {
                    if (screws[i].status == 2)
                    {
                        break;
                    }
                    else
                    {
                        screws[i].Skip = true;
                    }
                }
                InforTeaching.Instance.SaveSettings();
            }
        }
    }
}

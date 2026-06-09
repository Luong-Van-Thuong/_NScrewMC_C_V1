using Modbus.Device;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace _NScrewMC_C_V1
{
    public partial class ScrewTest : Form
    {
        private int[] m_StepIndex = { 0,0 };
        private bool[] m_StepRun = { false,false };
        private Stopwatch[] m_sw = { new Stopwatch(), new Stopwatch() };
        private int[] m_PickupCount = { 0,0 };
        private bool[] m_StepPauseStop = { false,false };

        public ScrewTest()
        {
            InitializeComponent();

        }

        private void BT_LEFT_PICUP_TEST_START_Click(object sender, EventArgs e)
        {
            BT_LEFT_PICUP_TEST_START.Enabled = false;
            
            labelPickupCountL.Text = "--";
            PickupTest_Logic(0);
        }

        private void BT_LEFT_PICUP_TEST_STOP_Click(object sender, EventArgs e)
        {
            m_StepRun[0] = false;
            BT_LEFT_PICUP_TEST_START.Enabled = true;
        }

        private void BT_RIGHT_PICUP_TEST_START_Click(object sender, EventArgs e)
        {
            BT_RIGHT_PICUP_TEST_START.Enabled = false;
            labelPickupCountR.Text = "--";
            PickupTest_Logic(1);
        }

        private void BT_RIGHT_PICUP_TEST_STOP_Click(object sender, EventArgs e)
        {
            m_StepRun[1] = false;
            BT_RIGHT_PICUP_TEST_START.Enabled = true;
        }

        private void PickupTest_Logic(int index)
        {
            m_StepIndex[index] = 0;
            m_StepRun[index] = true;
            m_PickupCount[index] = 0;
            int m_RetryCount = 0;
            if (index == 0)
                labelPickupCountL.Text = m_PickupCount[index].ToString();
            else
                labelPickupCountR.Text = m_PickupCount[index].ToString();
            Task.Run(() =>
            {
                while (m_StepRun[index])
                {
                    if (m_StepPauseStop[index] == true)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    if (MSystem.IsDetectDoorOpen(out string m) == true)
                    {
                        MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                        m_StepPauseStop[index] = true;
                        continue;
                    }

                    if (MSystem.IsDetectEmergency() == true)
                    {
                        MSystem.MyMsgMemo("EMG Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                        m_StepRun[index] = false;
                        break;
                    }
                    if (MSystem.IsLightCurtainDetected() == true)
                    {
                        MSystem.MyMsgMemo("Light Curtain Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                        m_StepPauseStop[index] = true;
                        if (index ==0)
                        {
                            labelPickupCountL.BackColor = Color.LightBlue;
                        }
                        else
                        {
                            labelPickupCountR.BackColor = Color.LightBlue;
                        }
                        continue;
                    }
                    switch (m_StepIndex[index])
                    {
                        case 0:
                            MSystem.m_pTrsScrew[index].MoveReadyPosZ();
                            m_StepIndex[index] = 1;
                            m_sw[index].Restart();
                            break;
                        case 1:
                            if (MSystem.m_pTrsScrew[index].IsReadyPosZ())
                            {
                                MSystem.m_pTrsScrew[index].MoveTrashPosX();
                                m_StepIndex[index] = 2;
                                //if (MSystem.m_pTrsScrew[index].IsVacuumSensorOn())
                                //{
                                //    MSystem.m_pTrsScrew[index].MoveTrashPosX();
                                //    m_StepIndex[index] = 2;
                                //}
                                //else
                                //{
                                //    m_StepIndex[index] = 4;
                                //}
                                m_sw[index].Restart();
                            }
                            else if (m_sw[index].ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move Z Ready Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                m_StepRun[index] = false;
                            }
                            break;
                        case 2:
                            if (MSystem.m_pTrsScrew[index].IsTrashPosX())
                            {
                                MSystem.m_pTrsScrew[index].MoveTrashPosZ();
                                m_sw[index].Restart();
                                m_StepIndex[index] = 3;
                            }
                            else if (m_sw[index].ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move X Trash Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                m_StepRun[index] = false;
                            }
                            break;
                        case 3:
                            if (MSystem.m_pTrsScrew[index].IsTrashPosZ())
                            {
                                MSystem.m_pTrsScrew[index].BlowOn();
                                Thread.Sleep(500);
                                MSystem.m_pTrsScrew[index].BlowOff();

                                MSystem.m_pTrsScrew[index].MoveReadyPosZ();
                                m_sw[index].Restart();
                                m_StepIndex[index] = 4;
                            }
                            else if (m_sw[index].ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move Z Trash Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                m_StepRun[index] = false;
                            }
                            break;
                        case 4:
                            if (MSystem.m_pTrsScrew[index].IsReadyPosZ())
                            {
                                MSystem.m_pTrsScrew[index].MovePickUpPosX();
                                m_sw[index].Restart();
                                m_StepIndex[index] = 5;
                            }
                            else if (m_sw[index].ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move Z Ready Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                m_StepRun[index] = false;
                            }
                            break;
                        case 5:
                            if (MSystem.m_pTrsScrew[index].IsPickUpPosX()
                            && MSystem.m_pTrsScrew[index].IsFeederReady())
                            {
                                m_sw[index].Restart();
                                m_StepIndex[index] = 6;
                                MSystem.m_pTrsScrew[index].MovePickUpPosZ();
                                MSystem.m_pTrsScrew[index].VacuumOn();
                            }
                            else if (m_sw[index].ElapsedMilliseconds > 5000)
                            {
                                MSystem.MyMsgMemo("Move X Pickup & Feeder Ready Fail", "Error", msgButton.OK, msgIcon.Error);
                                m_StepRun[index] = false;
                            }
                            break;
                        case 6:
                            if (MSystem.m_pTrsScrew[index].IsPickUpPosZ()
                            && MSystem.m_pTrsScrew[index].IsVacuumSensorOn()
                            && m_sw[index].ElapsedMilliseconds > (InforManager.Instance.PickupDelayTime*1000))
                            {
                                m_sw[index].Restart();
                                m_StepIndex[index] = 7;
                                m_RetryCount = 0;

                            }
                            else if (m_sw[index].ElapsedMilliseconds > (((InforManager.Instance.PickupDelayTime * 1000)+5000)))
                            {
                                //if (m_RetryCount < 3)
                                //{
                                //    m_sw[index].Restart();
                                //    m_StepIndex[index] = 0;
                                //    m_RetryCount++;
                                //}
                                //else
                                {
                                    m_RetryCount = 0;
                                    MSystem.MyMsgMemo("Move Z Pickup & Vaccum Check Fail", "Error", msgButton.OK, msgIcon.Error);
                                    m_StepRun[index] = false;
                                    MSystem.m_pTrsScrew[index].MoveReadyPosZ();
                                }
                            }
                            break;
                        case 7:
                            if (m_sw[index].ElapsedMilliseconds > (InforManager.Instance.PickupDelayTime*1000))
                            {
                                m_sw[index].Restart();
                                m_StepIndex[index] = 8;

                                MSystem.m_pTrsScrew[index].MoveReadyPosZ();
                            }
                            break;
                        case 8:
                            if (MSystem.m_pTrsScrew[index].IsReadyPosZ())
                            {
                                m_sw[index].Restart();
                                m_PickupCount[index]++;

                                this.Invoke(new Action(() =>
                                {
                                    if (index == 0)
                                        labelPickupCountL.Text = m_PickupCount[index].ToString();
                                    else
                                        labelPickupCountR.Text = m_PickupCount[index].ToString();
                                }));

                                if (m_PickupCount[index] > 1000)
                                    m_StepRun[index] = false;
                                m_StepIndex[index] = 0;
                            }
                            else if (m_sw[index].ElapsedMilliseconds > 3000)
                            {
                                MSystem.MyMsgMemo("Move Z Ready Position Fail", "Error", msgButton.OK, msgIcon.Error);
                                m_StepRun[index] = false;
                            }
                            break;
                        
                    }
                    Thread.Sleep(10);
                }
                if (MSystem.m_pTrsScrew[index].IsBlowSolOn())
                    MSystem.m_pTrsScrew[index].BlowOff();
                MSystem.m_pTrsScrew[index].MoveReadyPosZ();
            });
        }

        private void labelPickupCountL_Click(object sender, EventArgs e)
        {
            if (m_StepPauseStop[0] == false)
            {
                labelPickupCountL.BackColor = Color.LightBlue;
                m_StepPauseStop[0] = true;
            }
            else
            {
                labelPickupCountL.BackColor = Color.White;
                m_StepPauseStop[0] = false;
                m_sw[0].Restart();
            }
        }

        private void labelPickupCountR_Click(object sender, EventArgs e)
        {
            if (m_StepPauseStop[1] == false)
            {
                labelPickupCountR.BackColor = Color.LightBlue;
                m_StepPauseStop[1] = true;
            }
            else
            {
                labelPickupCountR.BackColor = Color.White;
                m_StepPauseStop[1] = false;
                m_sw[1].Restart();
            }
        }

        private void BT_LEFT_VACCUM_Click(object sender, EventArgs e)
        {
            if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_LEFT_Z_VACUM_ONOFF_SOL"]))
                MSystem.m_pTrsScrew[0].VacuumOff();
            else
                MSystem.m_pTrsScrew[0].VacuumOn();
        }

        private void BT_LEFT_BLOW_Click(object sender, EventArgs e)
        {
            if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_LEFT_Z_VACCUM_BLOW_SOL"]))
                MSystem.m_pTrsScrew[0].BlowOff();
            else
                MSystem.m_pTrsScrew[0].BlowOn();
        }

        private void BT_LEFT_READY_Click(object sender, EventArgs e)
        {
            if (MSystem.IsDetectDoorOpen(out string m) == true)
            {
                //MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                //MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                MSystem.MySafetyAlarm();
                return;
            }
            if (MSystem.SetMove((int)Axis.AXIS_Z1, 0.0) != MSystem.MMC_OK)
            {
                MSystem.MyMsgMemo($"Right Z-UP Home Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
            }
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            while (true)
            {
                if (MSystem.m_pTrsScrew[Constants.left].IsReadyZ())
                {
                    MSystem.m_pTrsScrew[Constants.left].MoveReadyPosX();
                    break;
                }
                else if (sw.ElapsedMilliseconds > 3000)
                {
                    MSystem.MyMsgMemo($"Left Z-UP Home Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    break;
                }
                Thread.Sleep(10);
            }
        }

        private void BT_RIGHT_VACCUM_Click(object sender, EventArgs e)
        {
            if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_RIGHT_Z_VACUM_ONOFF_SOL"]))
                MSystem.m_pTrsScrew[1].VacuumOff();
            else
                MSystem.m_pTrsScrew[1].VacuumOn();
        }

        private void BT_RIGHT_BLOW_Click(object sender, EventArgs e)
        {
            if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_RIGHT_Z_VACCUM_BLOW_SOL"]))
                MSystem.m_pTrsScrew[1].BlowOff();
            else
                MSystem.m_pTrsScrew[1].BlowOn();
        }

        private void BT_RIGHT_READY_Click(object sender, EventArgs e)
        {
            if (MSystem.IsDetectDoorOpen(out string m) == true)
            {
                //MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                //MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                MSystem.MySafetyAlarm();
                return;
            }
            if (MSystem.SetMove((int)Axis.AXIS_Z2, 0.0) != MSystem.MMC_OK)
            {
                MSystem.MyMsgMemo($"Right Z-UP Home Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
            }
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            while (true)
            {
                if (MSystem.m_pTrsScrew[Constants.right].IsReadyZ())
                {
                    MSystem.m_pTrsScrew[Constants.right].MoveReadyPosX();
                    break;
                }
                else if (sw.ElapsedMilliseconds > 3000)
                {
                    MSystem.MyMsgMemo($"Right Z-UP Home Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                    break;
                }
                Thread.Sleep(10);
            }
        }
    }
}

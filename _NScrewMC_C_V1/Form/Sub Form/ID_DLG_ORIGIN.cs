using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public partial class FormOrigin : Form
    {
        public readonly object LockUpdateStatus = new object();
        public CBbutton[] m_BtAxis = new CBbutton[(int)Axis.eAXIS_MAX];
        System.Timers.Timer _Timupdate = new System.Timers.Timer();
        int m_iOriginError = 0;
        //int _index = 0;
        private int autoOrigineSequence = 0;
        private int autoOrigineResult = 0;
        private bool autoOrigineStartFlag = false;
        #region//Form Initial
        public FormOrigin()
        {
            InitializeComponent();
            Point check = FormMain.GetLocation();
            this.Location = new Point(FormMain.GetLocation().X + (FormMain.m_Width - this.Width) / 2, FormMain.GetLocation().Y + (FormMain.m_Height - this.Height) / 2);

            //convert type button
            m_BtAxis[(int)Axis.AXIS_X1] = new CBbutton(BT_AXIS_1, false);
            m_BtAxis[(int)Axis.AXIS_X2] = new CBbutton(BT_AXIS_2, false);
            m_BtAxis[(int)Axis.AXIS_Y1] = new CBbutton(BT_AXIS_3, false);
            m_BtAxis[(int)Axis.AXIS_Y2] = new CBbutton(BT_AXIS_4, false);
            m_BtAxis[(int)Axis.AXIS_Z1] = new CBbutton(BT_AXIS_5, false);
            m_BtAxis[(int)Axis.AXIS_Z2] = new CBbutton(BT_AXIS_6, false);

            for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
            {
                m_BtAxis[i].SetValue(CBbutton.NormalColor);
                m_BtAxis[i].Button.Click += new EventHandler(PreLoadEvent);
            }

            //CMD button
            ORG_BT_SELECT_ALL.Click += new EventHandler(PreLoadEvent);
            ORG_BT_CANCLE.Click += new EventHandler(PreLoadEvent);
            ORG_BT_SERVO_ON.Click += new EventHandler(PreLoadEvent);
            ORG_BT_SERVO_OFF.Click += new EventHandler(PreLoadEvent);
            ORG_BT_ORIGIN.Click += new EventHandler(PreLoadEvent);
            ORG_BT_AL_RESET.Click += new EventHandler(PreLoadEvent);
            BT_AL_UNIT_INIT.Click += new EventHandler(PreLoadEvent);

            _Timupdate = new System.Timers.Timer(200);
            _Timupdate.Elapsed += UpdateStatus;
            _Timupdate.AutoReset = true;
            _Timupdate.Enabled = true;

            this.FormClosed += new FormClosedEventHandler(CloseForm);
            MSystem.m_bIsTeachDlg = true;
            MSystem.m_bOrigin = true;

            //timer1.Interval = 500;
            //timer1.Start();
            MSystem.CheckSafety();
            if (InforManager.Instance.IsLeftUnitUse == false)
            {
                groupBox3.Enabled = false;
                colorButton1.Enabled = false;
            }
            if (InforManager.Instance.IsRightUnitUse == false)
            {
                groupBox1.Enabled = false;
                colorButton1.Enabled = false;
            }
        }
        ~FormOrigin()
        {

        }

        private void CloseForm(object sender, EventArgs e)
        {

        }

        private void FormOrigin_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                MSystem.m_bIsTeachDlg = false;
                MSystem.m_bOrigin = false;
                _Timupdate.Stop();
                _Timupdate.Dispose();
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("Form ORG Close fail");
            }
        }
        private void UpdateStatus(Object source, ElapsedEventArgs e)
        {
            try
            {
                UpdateAlarm();
            }
            catch (Exception)
            {

            }
        }
        #endregion

       
        void ServorOn()
        {
            // SERVO ON
            bool bSelectFlag = false;

            for (int i = 0; i < MSystem.eAXIS_MAX; i++)
            {
                if (m_BtAxis[i].m_iSel)
                    bSelectFlag = true;
            }

            if (!bSelectFlag)
            {
                MSystem.MyMsgMemo("No Select.", "Select Servo", msgButton.OK, msgIcon.Infor);
                return;
            }
            if (autoOrigineStartFlag == false)
            {
                DialogResult dlg = MSystem.MyMsgMemo("Are you want on Servo?", "Servo On", msgButton.YESNO, msgIcon.Question);
                if (dlg != DialogResult.Yes)
                {
                    return;
                }
            }
            Task.Run(async () =>
            {
                await Task.Delay(100);
                for (int i = 0; i < MSystem.eAXIS_MAX; i++)
                {
                    if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i] == null)
                        continue;
                    if (m_BtAxis[i].m_iSel)
                    {
                        MSystem.SetMsgDisplay($"Servo On {MSystem.m_pAxisManager.strAxisName[i]} ... ");
                        MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].PowerOff();
                        Thread.Sleep(200);
                        if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].PowerOn() == false)
                        {
                            MSystem.KillMsgDisplay();
                            MSystem.MyMsgMemo("Servo On Fail!", "Servo Error", msgButton.OK, msgIcon.Error);
                            autoOrigineResult = 2;
                            return;
                        }
                        //m_BtAxis[i].SetValue(CBbutton.NormalColor);
                    }
                    await Task.Delay(100);
                }
                MSystem.KillMsgDisplay();
                await Task.Delay(10);
                autoOrigineResult = 1;
            });
            MSystem.MyMsgDisplay("Servo On ....");
        }

        void ServorOFF()
        {
            bool bSelectFlag = false;
            for (int i = 0; i < MSystem.eAXIS_MAX; i++)
            {
                if (m_BtAxis[i].m_iSel)
                    bSelectFlag = true;
            }

            if (!bSelectFlag)
            {
                MSystem.MyMsgMemo("No Select.", "Select Servo", msgButton.OK, msgIcon.Infor);
                return;
            }
            if (autoOrigineStartFlag == false)
            {
                if (MSystem.MyMsgMemo("Are you want OFF Servo?", "Servo OFF", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }
            Task.Run(async () =>
            {
                await Task.Delay(100);
                for (int i = 0; i < MSystem.eAXIS_MAX; i++)
                {
                    if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i] == null)
                        continue;

                    if (m_BtAxis[i].m_iSel)
                    {
                        MSystem.SetMsgDisplay($"Servo Off {MSystem.m_pAxisManager.strAxisName[i]} ... ");
                        if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].PowerOff() == false)
                        {
                            MSystem.KillMsgDisplay();
                            MSystem.MyMsgMemo("Servo Off Fail!", "Servo Error", msgButton.OK, msgIcon.Error);
                            autoOrigineResult = 2;
                            return;
                        }
                        MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].ClearAxis();
                        MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].ResetOrigin();
                        //m_BtAxis[i].SetValue(CBbutton.NormalColor);
                    }
                    await Task.Delay(100);
                }
                MSystem.KillMsgDisplay();
                autoOrigineResult = 1;
            });
            MSystem.MyMsgDisplay("Servo Off ....");
        }

        void ServoReset()
        {
            // SERVO ON
            bool bSelectFlag = false;
            for (int i = 0; i < MSystem.eAXIS_MAX; i++)
            {
                if (m_BtAxis[i].m_iSel || m_BtAxis[i].GetColor == CBbutton.ErrorColor)
                    bSelectFlag = true;
            }

            if (!bSelectFlag)
            {
                MSystem.MyMsgMemo("No Select", "Select Servo", msgButton.OK, msgIcon.Question);
                return;
            }
            if (autoOrigineStartFlag == false)
            {
                if (MSystem.MyMsgMemo("Are you want Resets Servo?", "Reset Servo", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }
            Task.Run(async () =>
            {
                await Task.Delay(300);
                for (int i = 0; i < MSystem.eAXIS_MAX; i++)
                {
                    if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i] == null)
                        continue;


                    if (m_BtAxis[i].m_iSel || m_BtAxis[i].GetColor == CBbutton.ErrorColor)
                    {
                        MSystem.SetMsgDisplay($"Servo Reset {MSystem.m_pAxisManager.strAxisName[i]} ... ");
                        if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].ClearAxis() == false)
                        {
                            MSystem.MyMsgMemo("Reset Servo Error !", "Servo Error", msgButton.YESNO, msgIcon.Question);
                            MSystem.KillMsgDisplay();
                            autoOrigineResult = 2;
                            return;
                        }

                        MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].ClearAxis();
                        MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].ResetOrigin();

                        //m_BtAxis[i].SetValue(CBbutton.NormalColor);
                    }
                    await Task.Delay(100);
                }
                MSystem.KillMsgDisplay();
                autoOrigineResult = 1;
            });
            MSystem.MyMsgDisplay("Servo Start Reset ...");
        }

        public void ServoOrigin()
        {
            
            MSystem.IsOriginStatus = false;
            MSystem.IsInitillizeStatus = false;


            this.Invoke(new Action(() =>
            {
                bool bSelectFlag = false;

                for (int i = 0; i < MSystem.eAXIS_MAX; i++)
                {
                    if (m_BtAxis[i].m_iSel)
                        bSelectFlag = true;
                }

                if (!bSelectFlag)
                {
                    MSystem.MyMsgMemo("No Select Axis", "Select Servo", msgButton.OK, msgIcon.Question);
                    return;
                }
                if (autoOrigineStartFlag == false)
                {
                    if (MSystem.MyMsgMemo("Are you want Origins Axis Selected?", "Origin Axis", msgButton.YESNO, msgIcon.Question) != DialogResult.Yes)
                        return;
                }
                if (MSystem.CheckSafety() == true)
                {
                    MSystem.MySafetyAlarm();

                    autoOrigineResult = 2;
                    return;
                }


                if (ReturnOrigin() == false)
                {
                    Thread.Sleep(100);
                    MSystem.SetAllStop();
                    autoOrigineResult = 2;
                    return;
                }
                autoOrigineResult = 1;
                MSystem.IsOriginStatus = true;
                MSystem.IsInitillizeStatus = true;
                MSystem.MyMsgMemo("Origin Done! Ready for run", "Origin", msgButton.OK, msgIcon.Success);
            }));
        }

        private void BtExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 
        public bool ReturnOrigin()
        {
            bool needExits = false;
            int iError = 0;
            int iComplete = 0;
            bool[] bAxisSel = new bool[MSystem.eAXIS_MAX];
            bool[] bMove = new bool[MSystem.eAXIS_MAX];

            TimerDelay _TimeCheckInit = new TimerDelay();

            MSystem.m_bInitWorkFlag = true;
            /**********************************************/
            for (int i = 0; i < MSystem.eAXIS_MAX; i++)
            {
                if (!m_BtAxis[i].m_iSel)
                {
                    MSystem.m_bInitWorkFlag = false;
                    continue;
                }
                bAxisSel[i] = true;
            }

            if (bAxisSel[(int)Axis.AXIS_Y1] || bAxisSel[(int)Axis.AXIS_X1] == true)
            {
                if (!bAxisSel[(int)Axis.AXIS_Z1])
                {
                    if (MSystem.IsAxisOrigin(Axis.AXIS_Z1))
                    {
                        if (!MSystem.m_pTrsScrew[0].IsReadyZ())
                        {
                            MSystem.MyMsgMemo("Screw Rear Z-Axis Not UP", "Error");
                            return false;
                        }
                    }
                    else
                    {
                        MSystem.MyMsgMemo("(Screw Rear) Please Origin Z-Axis Before Origin Jig-Axis", "Error");
                        return false;
                    }
                }
            }
            if (bAxisSel[(int)Axis.AXIS_Y2] || bAxisSel[(int)Axis.AXIS_X2] == true)
            {
                if (!bAxisSel[(int)Axis.AXIS_Z2])
                {
                    if (MSystem.IsAxisOrigin(Axis.AXIS_Z2))
                    {
                        if (!MSystem.m_pTrsScrew[1].IsReadyZ())
                        {
                            MSystem.MyMsgMemo("Screw Middle Z-Axis Not UP", "Error");
                            return false;
                        }
                    }
                    else
                    {
                        MSystem.MyMsgMemo("(Screw Middle) Please Origin Z-Axis Before Origin Jig-Axis", "Error");
                        return false;
                    }
                }
            }

            m_iOriginError = -1;
            bool ResultOrigin = true;
            MSystem.waitServoInitOk();
            //Start Origin
            var resultRun = Task.Run(async () =>
            {
                await Task.Delay(1);
                /*Clear and reset origin*/
                for (int k = 0; k < MSystem.eAXIS_MAX; k++)
                {
                    int i = k;

                    if (!m_BtAxis[i].m_iSel)
                        continue;
                    if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i] == null)
                    {
                        ResultOrigin = false;
                        break;
                    }
                    MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].ClearAxis();
                    MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].ResetOrigin();

                    bMove[i] = true;
                    bAxisSel[i] = true;
                }

                int[] iPrior = new int[MSystem.eAXIS_MAX];
                int[] iStep = new int[MSystem.eAXIS_MAX];
                bool[] bDone = new bool[MSystem.eAXIS_MAX];
                for (int i = 0; i < MSystem.eAXIS_MAX; i++)
                {
                    iPrior[i] = iStep[i] = 0;
                    bDone[i] = false;
                }

                //Start Origin and check Prior
                for (int iPriority = 0; iPriority < 5; iPriority++)
                {
                    if (needExits)
                    {
                        ResultOrigin = false;
                        break;
                    }

                    while (true)
                    {
                        if (needExits)
                        {
                            ResultOrigin = false;
                            break;
                        }
                        for (int i = 0; i < MSystem.eAXIS_MAX; i++)
                        {
                            if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i] == null)
                            {
                                MSystem.KillMsgDisplay();
                                MSystem.MyMsgMemo($"Connect to RNET ECAT Port Fail!", "Error", msgButton.OK, msgIcon.Error);
                                ResultOrigin = false;
                                needExits = true;
                                break;
                            }

                            iPrior[i] = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].GetOriginPriority();
                            iStep[i] = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].GetOriginStep();

                            if (m_BtAxis[i].m_iSel
                                && (iPrior[i] == iPriority)
                                && (iStep[i] != 2000))
                            {
                                if (MSystem.IsDetectEmergency() == true)//(Check_EStop())
                                {
                                    StopOrigin();
                                    MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                                    MSystem.MyMsgMemo("EMERGENCY!!", "Error", msgButton.OK, msgIcon.Error);
                                    ResultOrigin = false;
                                    break;
                                }

                                if (Check_Stop())
                                {
                                    MSystem.KillMsgDisplay();
                                    ResultOrigin = false;
                                    break;
                                }

                                if (MSystem.IsDetectDoorOpen(out string m) == true)
                                {
                                    StopOrigin();
                                    //MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                                    //MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                                    MSystem.MySafetyAlarm();
                                    ResultOrigin = false;
                                    break;
                                }

                                if (MSystem.IsLightCurtainDetected() == true)
                                {
                                    StopOrigin();
                                    MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                                    MSystem.MyMsgMemo("Light Curtain Detect", "Error", msgButton.OK, msgIcon.Error);
                                    ResultOrigin = false;
                                    break;
                                }

                                MSystem.SetMsgDisplay($"{MSystem.m_pAxisManager.strAxisName[i]} is executing the Motor Origin...\n Wait a moment");


                                MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].OriginReturn(bMove[i]);

                                iStep[i] = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].GetOriginStep();


                                if (999 == iStep[i])
                                {
                                    for (int j = 0; j < MSystem.eAXIS_MAX; j++)
                                    {
                                        if (m_BtAxis[j].m_iSel)
                                        {
                                            MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].Stop();
                                        }
                                    }

                                    string errorName = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].OriginErrorName;
                                    //MSystem.SetMsgDisplay($"Origin Fail!!! {((Axis)i).ToString()},0x {errorName}");
                                    //MSystem.KillMsgDisplay();
                                    //Thread.Sleep(100);
                                    MSystem.MyMsgMemo($"Origin Fail!!! {((Axis)i).ToString()} {errorName}", "Error", msgButton.OK, msgIcon.Error);
                                    ResultOrigin = false;
                                    break;
                                }
                            }

                            //Reset Status button
                            if (iStep[i] == 2000)
                            {
                                if (m_BtAxis[i].m_iSel && bDone[i] == false)
                                {
                                    bDone[i] = true;
                                    OnOriginDone(i);
                                    break;
                                }
                            }
                            Thread.Sleep(3);
                        }//For End

                        if ((iComplete = IsOriginComplete(iPriority)) > 0)
                        {
                            if (iComplete == 0)
                            {
                                MSystem.KillMsgDisplay();
                                ResultOrigin = false;
                                break;
                            }

                            break;
                        }
                        if (!ResultOrigin) break;
                    }
                    Thread.Sleep(10);
                }
                MSystem.KillMsgDisplay();
            });

            MSystem.MyMsgDisplay("Start Origin ......");

            MSystem.KillMsgDisplay();
            if (MSystem.SIMULATION) MSystem.SetAllStop();

            if (MSystem.m_bInitWorkFlag && ResultOrigin)
            {
                if (MSystem.UnitInitAll() != 100)
                {
                    ResultOrigin = false;
                }
                MSystem.m_bInitWorkFlag = false;
            }
            return ResultOrigin;
        }
        #endregion

        public bool Check_Stop()
        {
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_FRONT_OP_BOX_STOP_SW"]))
            {
                StopOrigin();
                MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                return true;
            }
            return false;
        }

        void StopOrigin(bool bReset = true)
        {
            for (int k = 0; k < MSystem.eAXIS_MAX; k++)
            {
                if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[k] == null)
                    continue;
                int j = k;
                if (m_BtAxis[j].m_iSel)
                {
                    MSystem.m_pAxisManager.m_pMmcEtherCatAxis[j].Stop();
                    if (bReset)
                    {
                        MSystem.m_pAxisManager.m_pMmcEtherCatAxis[j].ResetOrigin();
                    }
                }
            }
        }

        void OnOriginDone(int iAxis)
        {
            if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[iAxis] == null)
                return;

            m_BtAxis[iAxis].SetValue(CBbutton.NormalColor);
        }
        public int IsOriginComplete(int nPriority)
        {
            int[] iStep = new int[MSystem.eAXIS_MAX];
            int[] iPrior = new int[MSystem.eAXIS_MAX];

            for (int k = 0; k < MSystem.eAXIS_MAX; k++)
            {
                if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[k] == null)
                    continue;
                iPrior[k] = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[k].GetOriginPriority();
                if (m_BtAxis[k].m_iSel //
                    && nPriority == iPrior[k])
                {
                    iStep[k] = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[k].GetOriginStep();
                    if (2000 != iStep[k])   //2000
                    {
                        return 0;
                    }
                }
            }
            return 1;
        }

        void UpdateAlarm()
        {
            try
            {
                bool bState = false;

                for (int i = 0; i < MSystem.eAXIS_MAX; i++)
                {
                    if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i] == null)
                        continue;
                    bState = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].GetAmpFault();

                    // SERVO ALARM
                    if ((bState || !MSystem.IsServoOn(i)) && !m_BtAxis[i].m_iSel)
                    {
                        m_BtAxis[i].SetValue(CBbutton.ErrorColor); //RED
                        continue;
                    }

                    //ORIGIN DONE
                    if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].IsOrigin()
                        && MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].IsAxisDone() && !m_BtAxis[i].m_iSel)
                    {
                        m_BtAxis[i].SetValue(CBbutton.StatusOK); //GREEN
                    }
                    Thread.Sleep(15);
                }
            }
            catch (Exception)
            {

            }

        }

        private void PreLoadEvent(object sender, EventArgs e)
        {
            switch ((sender as Button).Name.ToString())
            {
                case "ORG_BT_SELECT_ALL":
                    for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
                    {
                        if (i == (int)Axis.AXIS_X1 || i == (int)Axis.AXIS_Y1 || i == (int)Axis.AXIS_Z1)
                        {
                            if (InforManager.Instance.IsLeftUnitUse == false)
                                continue;
                        }
                        else
                        {
                            if (InforManager.Instance.IsRightUnitUse == false)
                                continue;
                        }
                        m_BtAxis[i].SetValue(CBbutton.SelectColor);
                    }
                    break;
                case "ORG_BT_CANCLE":
                    for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
                    {
                        m_BtAxis[i].SetValue(CBbutton.NormalColor);
                    }
                    break;
                case "ORG_BT_SERVO_ON":
                    ServorOn();
                    break;
                case "ORG_BT_SERVO_OFF":
                    ServorOFF();
                    break;
                case "ORG_BT_ORIGIN":
                    ServoOrigin();
                    break;
                case "ORG_BT_AL_RESET":
                    ServoReset();
                    break;

                case "BT_AXIS_1":
                    if (!m_BtAxis[0].m_iSel) m_BtAxis[0].SetValue(CBbutton.SelectColor);
                    else m_BtAxis[0].SetValue(CBbutton.NormalColor);
                    break;
                case "BT_AXIS_2":
                    if (!m_BtAxis[1].m_iSel) m_BtAxis[1].SetValue(CBbutton.SelectColor);
                    else m_BtAxis[1].SetValue(CBbutton.NormalColor);
                    break;
                case "BT_AXIS_3":
                    if (!m_BtAxis[2].m_iSel) m_BtAxis[2].SetValue(CBbutton.SelectColor);
                    else m_BtAxis[2].SetValue(CBbutton.NormalColor);
                    break;
                case "BT_AXIS_4":
                    if (!m_BtAxis[3].m_iSel) m_BtAxis[3].SetValue(CBbutton.SelectColor);
                    else m_BtAxis[3].SetValue(CBbutton.NormalColor);
                    break;
                case "BT_AXIS_5":
                    if (!m_BtAxis[4].m_iSel) m_BtAxis[4].SetValue(CBbutton.SelectColor);
                    else m_BtAxis[4].SetValue(CBbutton.NormalColor);
                    break;
                case "BT_AXIS_6":
                    if (!m_BtAxis[5].m_iSel) m_BtAxis[5].SetValue(CBbutton.SelectColor);
                    else m_BtAxis[5].SetValue(CBbutton.NormalColor);
                    break;

                case "BT_AL_UNIT_INIT":
                    MSystem.UnitInitAll();
                    break;

                default: break;
            }
        }

        private void PreLoadEvent(object sender, AxBTNENHLib4._DBtnEnhEvents_MouseDownEvent e)
        {
            if(sender is AxBTNENHLib4.AxBtnEnh btn)
            {
                int btnIndex = -1;
                switch (btn.Name)
                {
                    case "BT_AXIS_1":
                        btnIndex = (int)Axis.AXIS_X1;
                        break;
                    case "BT_AXIS_2":
                        btnIndex = (int)Axis.AXIS_Y1;
                        break;
                    case "BT_AXIS_3":
                        btnIndex = (int)Axis.AXIS_Z1;
                        break;
                    case "BT_AXIS_4":
                        btnIndex = (int)Axis.AXIS_X2;
                        break;
                    case "BT_AXIS_5":
                        btnIndex = (int)Axis.AXIS_Y2;
                        break;
                    case "BT_AXIS_6":
                        btnIndex = (int)Axis.AXIS_Z2;
                        break;

                    default: break;
                }

                if (btnIndex >= 0)
                {
                    if (!m_BtAxis[btnIndex].m_iSel)
                    {
                        m_BtAxis[btnIndex].m_iSel = true;
                        m_BtAxis[btnIndex].SetValue(CBbutton.SelectColor);
                    }
                    else
                    {
                        m_BtAxis[btnIndex].SetValue(CBbutton.NormalColor);
                        m_BtAxis[btnIndex].m_iSel = false;
                    }
                }
            }
        }

        private void BtExit_ClickEvent(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ORG_BT_ORIGIN_ClickEvent(object sender, EventArgs e)
        {
            ServoOrigin();
        }

        private void ORG_BT_SELECT_ALL_ClickEvent(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
            {
                m_BtAxis[i].m_iSel = true;
                m_BtAxis[i].SetValue(CBbutton.SelectColor);
            }
        }

        private void ORG_BT_CANCLE_ClickEvent(object sender, EventArgs e)
        {
            for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
            {
                m_BtAxis[i].m_iSel = false;
                m_BtAxis[i].SetValue(CBbutton.NormalColor);
            }
        }

        private void ORG_BT_SERVO_ON_ClickEvent(object sender, EventArgs e)
        {
            ServorOn();
        }

        private void ORG_BT_SERVO_OFF_ClickEvent(object sender, EventArgs e)
        {
            ServorOFF();
        }

        private void ORG_BT_AL_RESET_ClickEvent(object sender, EventArgs e)
        {
            ServoReset();
        }

        private void BT_AL_UNIT_INIT_ClickEvent(object sender, EventArgs e)
        {
            MSystem.UnitInitAll();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (autoOrigineSequence)
            {
                case 0:
                    {
                        for (int i = 0; i < MSystem.eAXIS_MAX; i++)
                        {
                            m_BtAxis[i].m_iSel = true;
                        }
                        //ORG_BT_SELECT_ALL.Enabled = false;
                        //ORG_BT_CANCLE.Enabled = false;
                        //ORG_BT_SERVO_ON.Enabled = false;
                        //ORG_BT_SERVO_OFF.Enabled = false;
                        //ORG_BT_ORIGIN.Enabled = false;
                        //ORG_BT_AL_RESET.Enabled = false;
                        //BtExit.Enabled = false;
                        autoOrigineStartFlag = true;
                        autoOrigineSequence++;
                    }
                    break;
                case 1:
                    {
                        autoOrigineResult = 0;
                        ServorOFF();
                        autoOrigineSequence++;
                    }
                    break;
                case 2:
                    {
                        if (autoOrigineResult == 1)
                        {
                            autoOrigineSequence++;
                        }
                        else if (autoOrigineResult == 2)
                        {
                            autoOrigineSequence = 9;
                            MSystem.MyMsgMemo("Servo Off Fail!", "Servo Error", msgButton.OK, msgIcon.Error);
                        }
                    }
                    break;
                case 3:
                    {
                        for (int i = 0; i < MSystem.eAXIS_MAX; i++)
                        {
                            m_BtAxis[i].m_iSel = true;
                        }
                        autoOrigineResult = 0;
                        ServoReset();
                        autoOrigineSequence++;

                    }
                    break;
                case 4:
                    {
                        if (autoOrigineResult == 1)
                        {
                            autoOrigineSequence++;
                        }
                        else if (autoOrigineResult == 2)
                        {
                            autoOrigineSequence = 9;
                            MSystem.MyMsgMemo("Servo Reset Fail!", "Servo Error", msgButton.OK, msgIcon.Error);
                        }
                    }
                    break;
                case 5:
                    {
                        for (int i = 0; i < MSystem.eAXIS_MAX; i++)
                        {
                            m_BtAxis[i].m_iSel = true;
                        }
                        autoOrigineResult = 0;
                        ServorOn();
                        autoOrigineSequence++;

                    }
                    break;
                case 6:
                    {
                        if (autoOrigineResult == 1)
                        {
                            autoOrigineSequence++;
                        }
                        else if (autoOrigineResult == 2)
                        {
                            autoOrigineSequence = 9;
                            MSystem.MyMsgMemo("Servo On Fail!", "Servo Error", msgButton.OK, msgIcon.Error);
                        }
                    }
                    break;
                case 7:
                    {
                        for (int i = 0; i < MSystem.eAXIS_MAX; i++)
                        {
                            m_BtAxis[i].m_iSel = true;
                        }
                        autoOrigineResult = 0;
                        ServoOrigin();
                        autoOrigineSequence++;

                    }
                    break;
                case 8:
                    {
                        if (autoOrigineResult == 1)
                        {
                            autoOrigineSequence++;
                        }
                        else if (autoOrigineResult == 2)
                        {
                            autoOrigineSequence = 9;
                            MSystem.MyMsgMemo("Servo Origin Fail!", "Servo Error", msgButton.OK, msgIcon.Error);
                        }
                    }
                    break;
                case 9:
                    {
                        autoOrigineSequence = 0;
                        autoOrigineStartFlag = false;
                        timer1.Stop();
                    }
                    break;
            }
        }

        private void colorButton1_Click(object sender, EventArgs e)
        {
            autoOrigineStartFlag = true;
            for (int i = 0; i < MSystem.eAXIS_MAX; i++)
            {
                if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].IsPlusLimit() == true)
                {
                    MSystem.MyMsgMemo("Axis " + MSystem.m_pAxisManager.strAxisName[i] + " Plus Limit is ON, Please Check!", "Origin Error", msgButton.OK, msgIcon.Error);
                    autoOrigineStartFlag = false;
                    return;
                }
                if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].IsMinusLimit() == true)
                {
                    MSystem.MyMsgMemo("Axis " + MSystem.m_pAxisManager.strAxisName[i] + " Minus Limit is ON, Please Check!", "Origin Error", msgButton.OK, msgIcon.Error);
                    autoOrigineStartFlag = false;
                    return;
                }
                m_BtAxis[i].m_iSel = true;
            }
            autoOrigineResult = 0;
            ServorOFF();
            if (autoOrigineResult == 2)
            {
                MSystem.MyMsgMemo("Servo On Fail!", "Servo Error", msgButton.OK, msgIcon.Error);
                autoOrigineStartFlag = false;
                return;
            }
            for (int i = 0; i < MSystem.eAXIS_MAX; i++)
            {
                m_BtAxis[i].m_iSel = true;
            }
            Thread.Sleep(200);
            autoOrigineResult = 0;
            ServoReset();
            if (autoOrigineResult == 2)
            {
                MSystem.MyMsgMemo("Servo Reset Fail!", "Servo Error", msgButton.OK, msgIcon.Error);
                autoOrigineStartFlag = false;
                return ;
            }
            for (int i = 0; i < MSystem.eAXIS_MAX; i++)
            {
                m_BtAxis[i].m_iSel = true ;
            }
            Thread.Sleep(200);
            autoOrigineResult = 0;
            ServorOn();
            if (autoOrigineResult == 2)
            {
                MSystem.MyMsgMemo("Servo On Fail!", "Servo Error", msgButton.OK, msgIcon.Error);
                autoOrigineStartFlag = false;
                return;
            }
            for (int i = 0; i < MSystem.eAXIS_MAX; i++)
            {
                m_BtAxis[i].m_iSel = true;
            }
            Thread.Sleep(200);
            autoOrigineResult = 0;
            ServoOrigin();
            if (autoOrigineResult == 2)
            {
                MSystem.MyMsgMemo("Servo Origin Fail!", "Servo Error", msgButton.OK, msgIcon.Error);
                autoOrigineStartFlag = false;
                return;
            }
            autoOrigineStartFlag = false;
        }
    }
}

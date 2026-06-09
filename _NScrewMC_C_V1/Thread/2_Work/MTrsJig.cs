using Basler.Pylon;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public class MTrsJig : MSystem
    {
        #region Variable
        public int m_idx = 0;
        public bool m_bReady = false;
        public bool m_bCallLoader = false;
        public bool m_bCallUnloader = false;
        public bool m_bCallScrew = false; // call screw 와 index point 이 두개로 동작을 한다
        public bool IsVisionAlignDone = false;
        public bool[] ProductNG = {false,false};
        public int m_iIndexPoint = 0;
        public int m_iRetry = 0;
        public ProductState PR_STATE = new ProductState();

        public UNITINFOR _Infor = new UNITINFOR();
        public JIG_STATUS _JigState = new JIG_STATUS();

        public int m_iCurrentStep = 0;
        public int m_iNextStep = 0;
        public int m_iPreviewStep = 0;

        public TimerDelay m_iTimer = new TimerDelay();
        public TimerDelay m_iTimCheck = new TimerDelay();

        public TimerDelay m_MTestTimer = new TimerDelay();
        public TimerDelay m_MWaitTimer = new TimerDelay();

        public ProductState m_State = new ProductState();

        public PointScrew m_iCurrentScrewPoint = new PointScrew();
        public PointScrew m_iTargetScrewPoint = new PointScrew();

        public bool m_bIsOnBarcodeFail = false;
        private bool _isStopOnce = true;
        private bool _isFirstMove = true;
        private int _iStopCount = 0; // add 250704 jlyoon
        public TimerDelay m_iStopTimer = new TimerDelay();

        private Axis _axisY;

        private MMCEtherCATAxis _mmceAxisY;

        private int LightCurtainSensor;

        private int JigDetectSensor;
        private int JigFwdSensor;
        private int JigBwdSensor;
        private int JigDownSensor;
        private int JigCenteringSensor;

        private int JigFwdSol;
        private int JigBwdSol;
        private int JigUpDownSol;
        private int JigCenteringSol;

        private int DI_JigDetectSensor;
        private int DI_JigFrontCoverFixSensor1_2;
        private int DI_JigRearCoverFixSensor3_4;
        private int DI_JigLeftCoverUpSensor;
        private int DI_JigLeftCoverDownSensor;
        private int DI_JigRightCoverUpSensor;
        private int DI_JigRightCoverDownSensor;
        private int DI_JigSleeveDetect; //

        private int DO_JigCoverFixSol;
        private int DO_JigCoverUnFixSol;
        private int DO_JigCoverUpSol;
        private int DO_JigCoverDownSol;

        private int LightCurtainSensor2;
        private int DI_FeederAlarm;


        public Stopwatch ScrewTime = new Stopwatch();

        public int JigStatus { get; set; } = 0; // 0 = ready,  1 = test,  2 = not use,  3 = good,  4 = ng , 8 = NG
        public int JigStatusOrg { get; set; } = 0; // for Except JigStatus==8

        public Task<string> PrevInspInfoTask { get; set; }

        public double[] m_dVisionOffsetY = new double[50];
        #endregion

        #region Class Initial
        public MTrsJig(int _idx)
        {
            m_idx = _idx;
            if (m_idx == Constants.left)
            {
                _axisY = Axis.AXIS_Y1;
                LightCurtainSensor = IOMap.IN["IN_LEFT_LIGHT_CURTAIN_DETECT"];//IO.IN_LEFT_LIGHT_CURTAIN_DETECT;
                // Screw IO Map
                if (InforManager.Instance.InspectionType == "Mobile")
                {
                    JigDetectSensor = IOMap.IN["IN_LEFT_JIG_DETECT_SENSOR"];//IO.IN_LEFT_JIG_DETECT_SENSOR;
                    JigFwdSensor = IOMap.IN["IN_LEFT_JIG_FWD_SENSOR"];//IO.IN_LEFT_JIG_FWD_SENSOR;
                    JigBwdSensor = IOMap.IN["IN_LEFT_JIG_BWD_SENSOR"];//IO.IN_LEFT_JIG_BWD_SENSOR;
                    JigDownSensor = IOMap.IN["IN_LEFT_JIG_DOWN_SENSOR"];//IO.IN_LEFT_JIG_DOWN_SENSOR;
                    JigCenteringSensor = IOMap.IN["IN_LEFT_JIG_CENTER_SENSOR"];//IO.IN_LEFT_JIG_CENTER_SENSOR;

                    JigFwdSol = IOMap.OUT["OUT_LEFT_JIG_FWD_D_SOL"];//IO.OUT_LEFT_JIG_FWD_D_SOL;
                    JigBwdSol = IOMap.OUT["OUT_LEFT_JIG_BWD_D_SOL"]; //IO.OUT_LEFT_JIG_BWD_D_SOL;
                    JigUpDownSol = IOMap.OUT["OUT_LEFT_JIG_DOWN_S_SOL"]; //IO.OUT_LEFT_JIG_DOWN_S_SOL;
                    JigCenteringSol = IOMap.OUT["OUT_LEFT_JIG_CENTERING_S_SOL"]; //IO.OUT_LEFT_JIG_CENTERING_S_SOL;
                }
                // Tablet IO Map
                else if (InforManager.Instance.InspectionType == "Tablet"
                    || InforManager.Instance.InspectionType == "Tablet Normal")
                {
                    DI_JigDetectSensor = IOMap.IN["IN_LEFT_JIG_COVER_DETECT_SS"];//IO.IN_LEFT_JIG_COVER_DETECT_SS;
                    DI_JigFrontCoverFixSensor1_2 = IOMap.IN["IN_LEFT_JIG_FRONT_COVER_FIX_SS_1_2"];//IO.IN_LEFT_JIG_FRONT_COVER_FIX_SS_1_2;
                    DI_JigRearCoverFixSensor3_4 = IOMap.IN["IN_LEFT_JIG_REAR_COVER_FIX_SS_3_4"]; //IO.IN_LEFT_JIG_REAR_COVER_FIX_SS_3_4;
                    DI_JigLeftCoverUpSensor = IOMap.IN["IN_LEFT_JIG_LEFT_COVER_UP_SS"]; //IO.IN_LEFT_JIG_LEFT_COVER_UP_SS;
                    DI_JigLeftCoverDownSensor = IOMap.IN["IN_LEFT_JIG_LEFT_COVER_DOWN_SS"]; //IO.IN_LEFT_JIG_LEFT_COVER_DOWN_SS;
                    DI_JigRightCoverUpSensor = IOMap.IN["IN_LEFT_JIG_RIGHT_COVER_UP_SS"]; //IO.IN_LEFT_JIG_RIGHT_COVER_UP_SS;
                    DI_JigRightCoverDownSensor = IOMap.IN["IN_LEFT_JIG_RIGHT_COVER_DOWN_SS"]; //IO.IN_LEFT_JIG_RIGHT_COVER_DOWN_SS;
                    DI_JigSleeveDetect = IOMap.IN["IN_LEFT_SLEEVE_DETECT"]; //IO.IN_LEFT_SLEEVE_DETECT;

                    DO_JigCoverFixSol = IOMap.OUT["OUT_LEFT_JIG_COVER_FIX_SOL"]; //IO.OUT_LEFT_JIG_COVER_FIX_SOL;
                    DO_JigCoverUnFixSol = IOMap.OUT["OUT_LEFT_JIG_COVER_UNFIX_SOL"]; //IO.OUT_LEFT_JIG_COVER_UNFIX_SOL;
                    DO_JigCoverUpSol = IOMap.OUT["OUT_LEFT_JIG_LR_COVER_UP_SOL"]; //IO.OUT_LEFT_JIG_LR_COVER_UP_SOL;
                    DO_JigCoverDownSol = IOMap.OUT["OUT_LEFT_JIG_LR_COVER_DOWN_SOL"]; //IO.OUT_LEFT_JIG_LR_COVER_DOWN_SOL;
                }
                // VST IO Map
                else if (InforManager.Instance.InspectionType == "VST")
                {
                    LightCurtainSensor2 = IOMap.IN["IN_LEFT_CURTAIN_DETECT_2"]; //IO.IN_LEFT_CURTAIN_DETECT_2;
                    DI_JigSleeveDetect = IOMap.IN["IN_LEFT_SLEEVE_DETECT"]; //IO.IN_LEFT_SLEEVE_DETECT;
                    DI_FeederAlarm = IOMap.IN["IN_LEFT_FEEDER_ALARM"]; //IO.IN_LEFT_FEEDER_ALARM;
                }
                else if (InforManager.Instance.InspectionType == "H8")
                {
                    JigDetectSensor = IOMap.IN["IN_LEFT_JIG_DETECT_SENSOR"];//IO.IN_LEFT_JIG_DETECT_SENSOR;
                    JigFwdSensor = IOMap.IN["IN_LEFT_JIG_FWD_SENSOR"];//IO.IN_LEFT_JIG_FWD_SENSOR;
                    JigBwdSensor = IOMap.IN["IN_LEFT_JIG_BWD_SENSOR"];//IO.IN_LEFT_JIG_BWD_SENSOR;
                    JigDownSensor = IOMap.IN["IN_LEFT_JIG_DOWN_SENSOR"];//IO.IN_LEFT_JIG_DOWN_SENSOR;
                    JigCenteringSensor = IOMap.IN["IN_LEFT_JIG_CENTER_SENSOR"];//IO.IN_LEFT_JIG_CENTER_SENSOR;

                    JigFwdSol = IOMap.OUT["OUT_LEFT_JIG_FWD_D_SOL"];//IO.OUT_LEFT_JIG_FWD_D_SOL;
                    JigBwdSol = IOMap.OUT["OUT_LEFT_JIG_BWD_D_SOL"]; //IO.OUT_LEFT_JIG_BWD_D_SOL;
                    JigUpDownSol = IOMap.OUT["OUT_LEFT_JIG_DOWN_S_SOL"]; //IO.OUT_LEFT_JIG_DOWN_S_SOL;
                    JigCenteringSol = IOMap.OUT["OUT_LEFT_JIG_CENTERING_S_SOL"]; //IO.OUT_LEFT_JIG_CENTERING_S_SOL;

                    LightCurtainSensor2 = IOMap.IN["IN_LEFT_CURTAIN_DETECT_2"]; //IO.IN_LEFT_CURTAIN_DETECT_2;
                }
            }
            else
            {
                _axisY = Axis.AXIS_Y2;
                LightCurtainSensor = IOMap.IN["IN_RIGHT_LIGHT_CURTAIN_DETECT"]; //IO.IN_RIGHT_LIGHT_CURTAIN_DETECT;

                // Screw IO Map
                if (InforManager.Instance.InspectionType == "Mobile")
                {
                    JigDetectSensor = IOMap.IN["IN_RIGHT_JIG_DETECT_SENSOR"]; //IO.IN_RIGHT_JIG_DETECT_SENSOR;
                    JigFwdSensor = IOMap.IN["IN_RIGHT_JIG_FWD_SENSOR"]; //IO.IN_RIGHT_JIG_FWD_SENSOR;
                    JigBwdSensor = IOMap.IN["IN_RIGHT_JIG_BWD_SENSOR"]; //IO.IN_RIGHT_JIG_BWD_SENSOR;
                    JigDownSensor = IOMap.IN["IN_RIGHT_JIG_DOWN_SENSOR"]; //IO.IN_RIGHT_JIG_DOWN_SENSOR;
                    JigCenteringSensor = IOMap.IN["IN_RIGHT_JIG_CENTER_SENSOR"]; //IO.IN_RIGHT_JIG_CENTER_SENSOR;

                    JigFwdSol = IOMap.OUT["OUT_RIGHT_JIG_FWD_D_SOL"]; //IO.OUT_RIGHT_JIG_FWD_D_SOL;
                    JigBwdSol = IOMap.OUT["OUT_RIGHT_JIG_BWD_D_SOL"]; //IO.OUT_RIGHT_JIG_BWD_D_SOL;
                    JigUpDownSol = IOMap.OUT["OUT_RIGHT_JIG_DOWN_S_SOL"]; //IO.OUT_RIGHT_JIG_DOWN_S_SOL;
                    JigCenteringSol = IOMap.OUT["OUT_RIGHT_JIG_CENTERING_S_SOL"]; //IO.OUT_RIGHT_JIG_CENTERING_S_SOL;
                }
                // Tablet IO Map
                else if (InforManager.Instance.InspectionType == "Tablet"
                   || InforManager.Instance.InspectionType == "Tablet Normal")
                {
                    DI_JigDetectSensor = IOMap.IN["IN_RIGHT_JIG_COVER_DETECT_SS"]; //IO.IN_RIGHT_JIG_COVER_DETECT_SS;
                    DI_JigFrontCoverFixSensor1_2 = IOMap.IN["IN_RIGHT_JIG_FRONT_COVER_FIX_SS_1_2"]; //IO.IN_RIGHT_JIG_FRONT_COVER_FIX_SS_1_2;
                    DI_JigRearCoverFixSensor3_4 = IOMap.IN["IN_RIGHT_JIG_REAR_COVER_FIX_SS_3_4"]; //IO.IN_RIGHT_JIG_REAR_COVER_FIX_SS_3_4;
                    DI_JigLeftCoverUpSensor = IOMap.IN["IN_RIGHT_JIG_LEFT_COVER_UP_SS"]; //IO.IN_RIGHT_JIG_LEFT_COVER_UP_SS;
                    DI_JigLeftCoverDownSensor = IOMap.IN["IN_RIGHT_JIG_LEFT_COVER_DOWN_SS"]; //IO.IN_RIGHT_JIG_LEFT_COVER_DOWN_SS;
                    DI_JigRightCoverUpSensor = IOMap.IN["IN_RIGHT_JIG_RIGHT_COVER_UP_SS"]; //IO.IN_RIGHT_JIG_RIGHT_COVER_UP_SS;
                    DI_JigRightCoverDownSensor = IOMap.IN["IN_RIGHT_JIG_RIGHT_COVER_DOWN_SS"]; //IO.IN_RIGHT_JIG_RIGHT_COVER_DOWN_SS;
                    DI_JigSleeveDetect = IOMap.IN["IN_RIGHT_SLEEVE_DETECT"]; //IO.IN_RIGHT_SLEEVE_DETECT;

                    DO_JigCoverFixSol = IOMap.OUT["OUT_RIGHT_JIG_COVER_FIX_SOL"]; //IO.OUT_RIGHT_JIG_COVER_FIX_SOL;
                    DO_JigCoverUnFixSol = IOMap.OUT["OUT_RIGHT_JIG_COVER_UNFIX_SOL"]; //IO.OUT_RIGHT_JIG_COVER_UNFIX_SOL;
                    DO_JigCoverUpSol = IOMap.OUT["OUT_RIGHT_JIG_LR_COVER_UP_SOL"]; //IO.OUT_RIGHT_JIG_LR_COVER_UP_SOL;
                    DO_JigCoverDownSol = IOMap.OUT["OUT_RIGHT_JIG_LR_COVER_DOWN_SOL"]; //IO.OUT_RIGHT_JIG_LR_COVER_DOWN_SOL;
                }
                // VST IO Map
                else if (InforManager.Instance.InspectionType == "VST")
                {
                    LightCurtainSensor2 = IOMap.IN["IN_RIGHT_CURTAIN_DETECT_2"]; //IO.IN_RIGHT_CURTAIN_DETECT_2;
                    DI_JigSleeveDetect = IOMap.IN["IN_RIGHT_SLEEVE_DETECT"]; //IO.IN_RIGHT_SLEEVE_DETECT;
                    DI_FeederAlarm = IOMap.IN["IN_RIGHT_FEEDER_ALARM"]; //IO.IN_RIGHT_FEEDER_ALARM;
                }
                else if (InforManager.Instance.InspectionType == "H8")
                {
                    JigDetectSensor = IOMap.IN["IN_RIGHT_JIG_DETECT_SENSOR"]; //IO.IN_RIGHT_JIG_DETECT_SENSOR;
                    JigFwdSensor = IOMap.IN["IN_RIGHT_JIG_FWD_SENSOR"]; //IO.IN_RIGHT_JIG_FWD_SENSOR;
                    JigBwdSensor = IOMap.IN["IN_RIGHT_JIG_BWD_SENSOR"]; //IO.IN_RIGHT_JIG_BWD_SENSOR;
                    JigDownSensor = IOMap.IN["IN_RIGHT_JIG_DOWN_SENSOR"]; //IO.IN_RIGHT_JIG_DOWN_SENSOR;
                    JigCenteringSensor = IOMap.IN["IN_RIGHT_JIG_CENTER_SENSOR"]; //IO.IN_RIGHT_JIG_CENTER_SENSOR;

                    JigFwdSol = IOMap.OUT["OUT_RIGHT_JIG_FWD_D_SOL"]; //IO.OUT_RIGHT_JIG_FWD_D_SOL;
                    JigBwdSol = IOMap.OUT["OUT_RIGHT_JIG_BWD_D_SOL"]; //IO.OUT_RIGHT_JIG_BWD_D_SOL;
                    JigUpDownSol = IOMap.OUT["OUT_RIGHT_JIG_DOWN_S_SOL"]; //IO.OUT_RIGHT_JIG_DOWN_S_SOL;
                    JigCenteringSol = IOMap.OUT["OUT_RIGHT_JIG_CENTERING_S_SOL"]; //IO.OUT_RIGHT_JIG_CENTERING_S_SOL;

                    LightCurtainSensor2 = IOMap.IN["IN_RIGHT_CURTAIN_DETECT_2"]; //IO.IN_RIGHT_CURTAIN_DETECT_2;
                }
            }

            m_bReady = false;
            m_bCallLoader = false;
            m_bCallUnloader = false;
            m_bCallScrew = false;
            m_iIndexPoint = 0;
            PR_STATE = new ProductState();
            _JigState = JIG_STATUS.eREADY;

            _mRun = new Thread(ThreadJob);
            _mRun.IsBackground = true;
            _mRun.Start();
        }


        public int SetUnitInitialize()
        {
            Stopwatch timeout = new Stopwatch();

            if (InforManager.Instance.InspectionType == "Tablet")
            {
                if (IsCoverDownFront() && IsDetectCover() && IsMoveDoneReadyPosY())
                {
                    CoverUp();
                    timeout.Restart();
                    while (true)
                    {
                        if (IsCoverUPFront() == true)
                            break;

                        if (IsDetectDoorOpen(out _) == true || IsJigLightCurtainDetect() == true || IsPressStop() == true)
                        {
                            return 1;
                        }

                        if (timeout.ElapsedMilliseconds > 5000)
                            return 1;

                        Thread.Sleep(15);
                    }
                }
                if (IsFixDownSol() /*&& !IsDetectCover()*/)
                {
                    if (IsCoverUPFront() && !IsMoveDoneCoverPosY())
                    {
                        FixUP();
                        timeout.Restart();
                        while (true)
                        {
                            if (IsFixUPSol() == true)
                                break;

                            if (IsDetectDoorOpen(out _) == true || IsJigLightCurtainDetect() == true || IsPressStop() == true)
                            {
                                return 1;
                            }

                            if (timeout.ElapsedMilliseconds > 5000)
                                return 1;

                            Thread.Sleep(15);
                        }
                    }

                    if (IsCoverDownFront() && (!IsDetectCover() || (IsDetectCover() && !IsMoveDoneCoverPosY())))
                    {

                        MoveCoverPosY();
                        timeout.Restart();
                        while (true)
                        {
                            if (IsMoveDoneCoverPosY() == true)
                                break;

                            if (IsDetectDoorOpen(out _) == true || IsJigLightCurtainDetect() == true || IsPressStop() == true)
                            {
                                MSystem.MoveStop(_axisY);
                                return 1;
                            }

                            if (timeout.ElapsedMilliseconds > 5000)
                                return 1;

                            Thread.Sleep(15);
                        }

                    }
                }

                FixUP();
                timeout.Restart();
                while (true)
                {
                    if (IsFixUPSol() == true)
                        break;

                    if (IsDetectDoorOpen(out _) == true || IsJigLightCurtainDetect() == true || IsPressStop() == true)
                    {
                        return 1;
                    }

                    if (timeout.ElapsedMilliseconds > 5000)
                        return 1;

                    Thread.Sleep(15);
                }

                CoverUp();
                timeout.Restart();
                while (true)
                {
                    if (IsCoverUPFront() == true)
                        break;

                    if (IsDetectDoorOpen(out _) == true || IsJigLightCurtainDetect() == true || IsPressStop() == true)
                    {
                        return 1;
                    }

                    if (timeout.ElapsedMilliseconds > 5000)
                        return 1;

                    Thread.Sleep(15);
                }

                MoveReadyPosY();
                timeout.Restart();
                while (true)
                {
                    if (IsMoveDoneReadyPosY() == true)
                        break;

                    if (IsDetectDoorOpen(out _) == true || IsJigLightCurtainDetect() == true || IsPressStop() == true)
                    {
                        MSystem.MoveStop(_axisY);
                        return 1;
                    }

                    if (timeout.ElapsedMilliseconds > 5000)
                        return 1;

                    Thread.Sleep(15);
                }
            }
            else
            {
                Up();
                timeout.Restart();
                while (true)
                {
                    if (IsUp() == true)
                        break;

                    if (IsDetectDoorOpen(out _) == true || IsJigLightCurtainDetect() == true || IsPressStop() == true)
                    {
                        return 1;
                    }

                    if (timeout.ElapsedMilliseconds > 5000)
                        return 1;

                    Thread.Sleep(15);
                }

                MoveReadyPosY();
                Bwd();
                //if (InforManager.Instance.InspectionType == "H8")
                //    Uncentering();
                timeout.Restart();
                while (true)
                {
                    if (IsMoveDoneReadyPosY() == true && IsBwd() == true)
                    {
                        //if (InforManager.Instance.InspectionType == "H8")
                        //{
                        //    if (IsUnCentering() == true)
                        //        break;
                        //}
                        //else
                            break;
                    }
                    if (IsDetectDoorOpen(out _) == true || IsJigLightCurtainDetect() == true || IsPressStop() == true)
                    {
                        MSystem.MoveStop(_axisY);
                        return 1;
                    }

                    if (timeout.ElapsedMilliseconds > 5000)
                        return 1;

                    Thread.Sleep(15);
                }

            }


            PR_STATE = new ProductState();
            InitData();
            m_MTestTimer.PauseTimer();
            m_MWaitTimer.PauseTimer();
            m_MTestTimer.ResetTimer();
            m_MWaitTimer.ResetTimer();
            ScrewTime.Stop();
            return (int)InitResult.UNIT_INIT_SUCCESS;
        }
      

        public void InitData()
        {
            m_State = new ProductState();
            m_iNextStep = 0;
            m_iCurrentStep = 0;
            m_iPreviewStep = 0;
            _Infor = new UNITINFOR();

            m_bIsOnBarcodeFail = false;
            m_bReady = false;
            m_bCallLoader = false;
            m_bCallUnloader = false;
            m_bCallScrew = false;
            m_iIndexPoint = 0;
            JigStatus = 0;
            JigStatusOrg = 0;
            _iStopCount = 0;
            isStart[m_idx] = false; // add 0719 jlyoon
            ResetScrewResult();
        }
        #endregion

        /******************************************************/
        #region // Tool Get Step and Timer
        public void SetStep(int _step)
        {
            _isFirstMove = true;
            m_iCurrentStep = _step;
            m_iTimer.StartTimer();
            LogStep(_step);
        }
        public int GetStep()
        {
            return m_iCurrentStep;
        }
        public virtual void SetError(int _idError)
        {
            if (m_idx == Constants.left)
                MSystem.SetError(1000 + _idError, $"Left Jig");
            else
                MSystem.SetError(2000 + _idError, $"Right Jig");
        }
        public bool IsOverTime(double _tim)
        {
            if (m_iTimer.MoreThan(_tim))
                return true;
            else
                return false;
        }
        #endregion

        /******************************************************/
        #region Main Run
        public void ThreadJob()
        {
            Thread.Sleep(200);
            while (true)
            {
                Thread.Sleep(10);
                try
                {
                    if (InforManager.Instance.InspectionType == "Tablet"
                        && InforManager.Instance.IsSmartKitUse == true)
                        dorunStepTablet();
                    else
                        dorunStep();
                }
                catch (Exception ex)
                {
                    MyMessagerBottom($"Logic Jig Fail {m_iCurrentStep} - {m_idx} - {ex.ToString()}");
                    Thread.Sleep(10);
                }
            }
        }


        public void dorunStep()
        {
            if (_mmceAxisY == null)
            {
                if (m_idx == Constants.left)
                    _mmceAxisY = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)Axis.AXIS_Y1];
                else
                    _mmceAxisY = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)Axis.AXIS_Y2];
                return;
            }
            
            if (SysStatus != StatusRun.RUN || isStart[m_idx] == false)
            {
                if (_isStopOnce == false) 
                {
                    _isStopOnce = true;
                    _isFirstMove = true;
                    _iStopCount = 1;
                    m_iStopTimer.StartTimer();
                    StopFwdBwd();
                    MSystem.MoveStop(_axisY);
                }

                if (JigStatus == 1 && ScrewTime.IsRunning == true)
                    ScrewTime.Stop();

                m_bCallScrew = false;

                if (_isStopOnce == true && _isFirstMove == true)  // add 250704 jlyoon
                {
                    if (m_iStopTimer.MoreThan(0.5))
                    {
                        if ((m_idx == Constants.right && m_pDIO.IsOn(IOMap.IN["IN_RIGHT_JIG_STOP_SW_R"]) == true)
                            || (m_idx == Constants.left && m_pDIO.IsOn(IOMap.IN["IN_LEFT_JIG_STOP_SW_L"]) == true))
                        {
                            _iStopCount++;
                            m_iStopTimer.StartTimer();
                        }
                    }

                    if (_iStopCount >= 3) // Second STOP Button or Long Press STOP Button
                    {
						_iStopCount = 0; // add 0719 jlyoon
                        MSystem.m_pTrsScrew[m_idx].SetUnitInitialize();
                        SetUnitInitialize();
                    }
                }
                if (isStart[m_idx] == false)
                {
                    return;
                }
                m_iTimer.StartTimer();

                return;
            }

            // 250703 jlyoon : isStart condition add
            if (IsJigLightCurtainDetect() == true && isStart[m_idx] != false)
            {
                isStart[m_idx] = false;
                MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                ScrewTime.Start();
                string msg = m_idx == Constants.left ? "Left" : "Right";
                msg += " Light Curtain Detected!!";
                MSystem.MyMsgMemo(msg, "Error", msgButton.OK, msgIcon.Error); // update 250707 jlyoon
                m_pTrsBuzzer.StopAllBuzzers();
                
                return;
            }

            if (JigStatus == 1 && ScrewTime.IsRunning == false)
                ScrewTime.Start();

            CheckLimitSensor();
            _isStopOnce = false;
            _iStopCount = 0;

            switch (m_iCurrentStep)
            {
                case 0:
                    InitData();
                    SetStep(10);
                    break;

                case 10:
                    m_iIndexPoint = 0;
                    ResetScrewResult();

                    // THÊM LOG BẮT ĐẦU SẢN PHẨM
                    string reworkStatus = MSystem._Rework[m_idx] ? $"LogScrew_{DateTime.Now:yyyyMMdd}-REWORK" : $"LogScrew_{DateTime.Now:yyyyMMdd}-NORMAL";
                    if (m_idx == Constants.left)
                        m_pLogSave.AddTail(LogIndex.eLogScrew1, reworkStatus);
                    else
                        m_pLogSave.AddTail(LogIndex.eLogScrew2, reworkStatus);

                   
                    string startLog = $"LogScrew_{DateTime.Now:yyyyMMdd}- PRODUCT_START:{DateTime.Now:yyyyMMdd_HHmmss}";
                   // string startLog = $"LOG:PRODUCT_START | Total_Screws: {InforTeaching.Instance.P_Screw[m_idx].Count}";
                    if (m_idx == Constants.left)
                        m_pLogSave.AddTail(LogIndex.eLogScrew1, startLog);
                    else
                        m_pLogSave.AddTail(LogIndex.eLogScrew2, startLog);

                    
                    // todo : add interlock crash screw unit
                    if (ScrewTime.IsRunning == false)
                    {
                        ScrewTime.Restart();
                        MSystem.m_pTrsScrew[m_idx].swScriewFlowTime.Reset();
                    }
                        
                    JigStatus = 1;
                    MSystem.AutoForm.ClearAlignImage(m_idx); // add 250702

                    if (InforManager.Instance.AutoVisionAlignMode != 0)
                    {
                        MSystem.CheckVisionProgramRun();// check pgm vision
                    }
                    MSystem.INC_ProductInput();
                    MSystem.INC_JigInOutStatus9020(m_idx, true);
                     SetStep(20);
                    break;
                case 20:
                    if (IsUp() == true)
                    {
                        if (InforManager.Instance.AutoVisionAlignMode != 0)
                        {
                            // Vision Light On 260530 cnz
                            MSystem.vinterface.SendToLIGHT_CTRL(m_idx, true);
                        }
                        SetStep(30);
                    }
                    else
                    {
                        Up();
                        if (m_iTimer.MoreThan(5) == true)
                            SetError(6); // Not detected sensor Up
                    }                       
                    break;
                case 30:
                    if (IsFwd() == true)
                    {
                        if (InforManager.Instance.IsBarcodeUse == true)
                        {
                            if (m_idx == 0)
                            {
                                MSystem.AutoForm.LBL_RESULT_BCR_L.ForeColor = Color.Black;
                                MSystem.AutoForm.LBL_RESULT_BCR_L.Text = "";
                            }
                            else
                            {
                                MSystem.AutoForm.LBL_RESULT_BCR_R.ForeColor = Color.Black;
                                MSystem.AutoForm.LBL_RESULT_BCR_R.Text = "";
                            }
                            MoveBarcodePosY();
                            SetStep(35);
                        }
                        else
                        {
                            if (m_idx == 0)
                            {
                                MSystem.AutoForm.LBL_RESULT_BCR_L.ForeColor = Color.Black;
                                MSystem.AutoForm.LBL_RESULT_BCR_L.Text = "BCR NOT USE";
                            }
                            else
                            {
                                MSystem.AutoForm.LBL_RESULT_BCR_R.ForeColor = Color.Black;
                                MSystem.AutoForm.LBL_RESULT_BCR_R.Text = "BCR NOT USE";
                            }
                            SetStep(45);
                        }
                    }
                    else
                    {
                        Fwd();
                        if (m_iTimer.MoreThan(5) == true)
                            SetError(3); // Not detected sensor fwd 
                    }
                    break;

                case 35:
                    if (IsMoveDoneBarcodePosY() == true)// && m_pTrsScrew[m_idx].m_bMovedoneBarcode)
                    {
                        MSystem.m_pTrsScrew[m_idx].m_bBcrResult = BCR_RESULT.START;
                        SetStep(40);
                    }
                    break;
                case 40:
                    JigStatusOrg = 0; // add 0719 jlyoon
                    if (MSystem.m_pTrsScrew[m_idx].m_bBcrResult == BCR_RESULT.PASS
                        || IsDry())
                    {
                        MSystem.m_pTrsScrew[m_idx].m_bBcrResult = BCR_RESULT.IDLE;

                        SetStep(45);
                    }
                    else if (MSystem.m_pTrsScrew[m_idx].m_bBcrResult == BCR_RESULT.FAIL)
                    {
                        _isFirstMove = true;
                        JigStatus = 9;
                        JigStatusOrg = JigStatus;
                        MSystem.m_pTrsScrew[m_idx].m_bBcrResult = BCR_RESULT.IDLE;
                        SetStep(310);
                    }
                    break;

                case 45:
                    if (IsDown() == true)
                    {
                        if (InforManager.Instance.AutoVisionAlignMode != 0)
                        {
                            MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.START;
                            SetStep(50);
                        }
                        else
                        {
                            _isFirstMove = true;
                            SetStep(100);
                        }
                    }
                    else
                    {
                        Down();

                        if (m_iTimer.MoreThan(5) == true)
                            SetError(5);
                    }
                    break;

                case 50:
                    JigStatusOrg = 0; // add 0719 jlyoon
                    if (MSystem.vinterface.m_VisionResult[m_idx] == VS_RESULT.PASS)
                    {
                        // Vision Light On 260530 cnz
                        MSystem.vinterface.SendToLIGHT_CTRL(m_idx, false);
                        _isFirstMove = true;
                        SetStep(100);
                    }
                    else if (MSystem.vinterface.m_VisionResult[m_idx] == VS_RESULT.FAIL)//m_iTimer.MoreThan(20) == true || 
                    {
                        // Vision Light On 260530 cnz
                        MSystem.vinterface.SendToLIGHT_CTRL(m_idx, false);
                        MSystem.m_pTrsScrew[m_idx].m_bOnAlignFail = false;
                        JigStatus = 8; // Align Fail...!
                        JigStatusOrg = JigStatus;
                        SetStep(310);
                    }

                    break;
                case 100:
                    if (InforTeaching.Instance.P_Screw[m_idx][m_iIndexPoint].Skip == true)
                    {
                        m_pTrsScrew[m_idx].IsScrewOK = true;
                        SetStep(300);
                        break;
                    }
                    if (_isFirstMove == true)
                    {
                        if (MoveScrewPosY(m_iIndexPoint) != -1)
                            _isFirstMove = false;
                    }

                    if (IsMoveDoneScrewPosY(m_iIndexPoint) == true && m_pTrsScrew[m_idx].IsScrewDone == false && IsFwd() == true  && IsDown() == true)
                    {
                        SetStep(200);
                    }
                    if (IsOverTime(1.0))// add  jlyoon to fix vision bug
                    {
                        MoveScrewPosY(m_iIndexPoint);
                    }
                    break;
                //case 150:
                //    if (IsDown() == true)
                //        SetStep(200);
                //    else
                //    {
                //        Down();

                //        if (m_iTimer.MoreThan(3) == true)
                //            SetError(5);
                //    }
                //    break;

                case 200:
                    if (IsScrewPosY(m_iIndexPoint) == false)
                    {
                        //m_bCallScrew = false;
                        SetStep(100);
                        break;
                    }

                    if (m_pTrsScrew[m_idx].IsScrewDone == true) // WAIT SCREW DONE
                    {
                        m_pTrsScrew[m_idx].IsScrewDone = false;
                        m_bCallScrew = false;
                        SetStep(300);
                        break;
                    }

                    m_bCallScrew = true;
                    break;

                case 300:
                    
                    if (m_pTrsScrew[m_idx].IsScrewOK == false ) // CHECK RESULT
                    {
                        MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.IDLE;
                        MSystem.INC_JigInOutStatus9020(m_idx, false, false);
                        MSystem.INC_ProductNG();
                        SetStep(310);
                        break;
                    }
                    m_iIndexPoint += 1;
                    if (m_iIndexPoint >= InforTeaching.Instance.P_Screw[m_idx].Count)
                    {
                        SetStep(310);
                        //m_iIndexPoint -=1;// add to clear exception. 
                        MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.IDLE;
                        MSystem.INC_JigInOutStatus9020(m_idx, false);
                        MSystem.INC_ProductPass();
                    }
                    else
                    {
                        SetStep(100);
                    }
                    break;

                case 310:
                    if (IsUp() == true)
                    {
                        _isFirstMove = false;
                        SetStep(320);
                    }
                    else
                    {
                        Up();
                        if (m_iTimer.MoreThan(5) == true)
                            SetError(6); // Not detected sensor Up
                    }
                    break;

                case 320:
                    if (_isFirstMove == true)
                    {
                        _isFirstMove = false;
                        MoveReadyPosY();
                    }

                    if (IsBwd() == false)
                        Bwd();

                    if (IsMoveDoneReadyPosY() == true && IsBwd() == true)
                    {
                        //if (InforManager.Instance.InspectionType == "H8")
                        //{
                        //    Uncentering();
                        //}
                        if (m_pTrsScrew[m_idx].IsScrewOK == true && !m_pTrsScrew[m_idx].m_bLastResultScrewNG)
                        {
                            if (JigStatus != 8 && JigStatus != 9) // 8: Vision NG, 9: BCR NG
                                JigStatus = 3;
                            InforProduct.Instance.ProductScrew_Pass[m_idx] += 1;
                            //if (InforManager.Instance.AutoVisionAlignMode != 0)       
                            //    InforProduct.Instance.ProductScrew_Pass_Vision[m_idx] += 1;

                            if (MSystem._Rework[m_idx])
                            {
                                MSystem._Rework[m_idx] = false;
                                if (InforProduct.Instance.ProductScrew_Fail[m_idx] > 0)
                                { InforProduct.Instance.ProductScrew_Fail[m_idx] -= 1; }

                                // Rework 모드일때 한번 진행하고 skip 취소 20251117 czn
                                var screws = InforTeaching.Instance.P_Screw[m_idx];
                                for (int i = 0; i < screws.Count; i++)
                                {
                                    screws[i].Skip = false;
                                }
                                InforTeaching.Instance.SaveSettings();//test delete saving
                            }
                        }
                        else
                        {
                            if (JigStatus != 8 && JigStatus != 9) // 8: Vision NG, 9: BCR NG
                                JigStatus = 4;
                            // VST 설비는 Rework 상관없이 NG 카운트 증가. 김영배 프로요청 251107 cnz
                            if (!MSystem._Rework[m_idx] || InforManager.Instance.InspectionType == "VST")
                            {
                                try
                                {
                                    ProductNG[m_idx] = true;
                                    InforProduct.Instance.ProductDetail[m_idx][m_iIndexPoint].m_dCountFail += 1;
                                }
                                catch (Exception ex)
                                {
                                    MyMessagerBottom($"DataRunning.json Check: {ex.Message}");
                                }
                            }
                            m_pTrsScrew[m_idx].m_bLastResultScrewNG = false;
                            m_pTrsBuzzer.SetBuzzerPattern(0);
                        }
                        // VST 설비는 Rework 상관없이 NG 카운트 증가. 김영배 프로요청 251107 cnz
                        if (ProductNG[m_idx] && (!MSystem._Rework[m_idx] || InforManager.Instance.InspectionType == "VST"))
                        {
                            if (JigStatusOrg != 8 && JigStatusOrg != 9)  // 8: Vision NG, 9: BCR NG
                            {
                                InforProduct.Instance.ProductScrew_Fail[m_idx] += 1;
							}
                            ProductNG[m_idx] = false;
                        }
                        InforProduct.Instance.SaveSettings();
                        InforTeaching.Instance.SaveSettings();//test delete savesetting

                        ScrewTime.Stop();
                         // THÊM LOG HOÀN THÀNH SẢN PHẨM
                        int ngCount = 0;
                        for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                        {
                            if (InforTeaching.Instance.P_Screw[m_idx][i].status == 2) // NG = 2
                                ngCount++;
                        }
                        int okCount = 0;
                        for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                        {
                            if (InforTeaching.Instance.P_Screw[m_idx][i].status ==1 ) // OK = 1
                                okCount++;
                        }
                        string finalResult = (JigStatus == 3) ? "OK" : "NG";
                        double totalTimeSeconds = ScrewTime.ElapsedMilliseconds / 1000.0;

                        string completeLog = $"LogScrew_{DateTime.Now:yyyyMMdd}- PRODUCT_COMPLETE:{DateTime.Now:yyyyMMdd_HHmmss} | Total_Time: {totalTimeSeconds:F2}s | Result: {finalResult} |OK_Count:{okCount}| NG_Count: {ngCount}| Total: {okCount+ ngCount}";

                        if (m_idx == Constants.left)
                            m_pLogSave.AddTail(LogIndex.eLogScrew1, completeLog);
                        else
                            m_pLogSave.AddTail(LogIndex.eLogScrew2, completeLog);

                        if (IsDry() == false)
                        {
                            isStart[m_idx] = false;
                        }
                        
                        SetStep(10);
                    }
                    break;

                default:
                    break;
            }
        }

        public void dorunStepTablet()
        {
            if (_mmceAxisY == null)
            {
                if (m_idx == Constants.left)
                    _mmceAxisY = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)Axis.AXIS_Y1];
                else
                    _mmceAxisY = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)Axis.AXIS_Y2];
                return;
            }
            
            if (SysStatus != StatusRun.RUN || isStart[m_idx] == false )
            {
                if (_isStopOnce == false) 
                {
                    _isStopOnce = true;
                    _isFirstMove = true;
                    _iStopCount = 1;
                    m_iStopTimer.StartTimer();
                    StopFwdBwd();
                    MSystem.MoveStop(_axisY);
                }

                if (JigStatus == 1 && ScrewTime.IsRunning == true)
                    ScrewTime.Stop();

                m_bCallScrew = false;

                if (_isStopOnce == true && _isFirstMove == true)  // add 250704 jlyoon
                {
                    if (m_iStopTimer.MoreThan(0.5))
                    {
                        if ((m_idx == Constants.right && m_pDIO.IsOn(IOMap.IN["IN_RIGHT_JIG_STOP_SW_R"]) == true)
                            || (m_idx == Constants.left && m_pDIO.IsOn(IOMap.IN["IN_LEFT_JIG_STOP_SW_L"]) == true))
                        {
                            _iStopCount++;
                            m_iStopTimer.StartTimer();
                        }
                    }

                    if (_iStopCount >= 3) // Second STOP Button or Long Press STOP Button
                    {
                        _iStopCount = 0; // add 0719 jlyoon
                        MSystem.m_pTrsScrew[m_idx].SetUnitInitialize();
                        SetUnitInitialize();
                    }
                }

                if (isStart[m_idx] == false)
                {
                    return;
                }
                m_iTimer.StartTimer();

                return;
            }

            // 250703 jlyoon : isStart condition add
            if (IsJigLightCurtainDetect() == true && isStart[m_idx] != false)
            {
                isStart[m_idx] = false;
                MSystem.m_pTrsBuzzer.SetBuzzerPattern(1);
                Task.Run(async () =>
                {
                    await Task.Delay(10);
                    string msg = m_idx == Constants.left ? "Left" : "Right";
                    msg += " Light Curtain Detected!!";
                    MSystem.MyMsgMemo(msg, "Error", msgButton.OK, msgIcon.Error); // update 250707 jlyoon
                    m_pTrsBuzzer.StopAllBuzzers();
                });
                ScrewTime.Start();
                return;
            }

            if (JigStatus == 1 && ScrewTime.IsRunning == false)
                ScrewTime.Start();

            CheckLimitSensor();
            _isStopOnce = false;
            _iStopCount = 0;

            switch (m_iCurrentStep)
            {
                case 0:
                    InitData();
                    SetStep(10);
                    break;
                case 10:
                    m_iIndexPoint = 0;
                  
                    ResetScrewResult();
                    #region Log Start
                    // THÊM LOG BẮT ĐẦU SẢN PHẨM
                    string reworkStatus = MSystem._Rework[m_idx] ? $"LogScrew_{DateTime.Now:yyyyMMdd}-REWORK" : $"LogScrew_{DateTime.Now:yyyyMMdd}-NORMAL";
                    if (m_idx == Constants.left)
                        m_pLogSave.AddTail(LogIndex.eLogScrew1, reworkStatus);
                    else
                        m_pLogSave.AddTail(LogIndex.eLogScrew2, reworkStatus);


                    string startLog = $"LogScrew_{DateTime.Now:yyyyMMdd}- PRODUCT_START:{DateTime.Now:yyyyMMdd_HHmmss}";
                    // string startLog = $"LOG:PRODUCT_START | Total_Screws: {InforTeaching.Instance.P_Screw[m_idx].Count}";
                    if (m_idx == Constants.left)
                        m_pLogSave.AddTail(LogIndex.eLogScrew1, startLog);
                    else
                        m_pLogSave.AddTail(LogIndex.eLogScrew2, startLog);
                    #endregion
                    SetStep(20);
                    // todo : add interlock crash screw unit
                    if (ScrewTime.IsRunning == false)
                    {
                        ScrewTime.Restart();
                        MSystem.m_pTrsScrew[m_idx].swScriewFlowTime.Reset();
                    }
                        
                    JigStatus = 1;
                    MSystem.AutoForm.ClearAlignImage(m_idx); // add 250702

                    if (InforManager.Instance.AutoVisionAlignMode != 0)
                    {
                        MSystem.CheckVisionProgramRun();
                    }
                    break;

                case 20:
                    if (IsCoverUPFront() == true)
                    {
                        if (InforManager.Instance.AutoVisionAlignMode != 0)
                        {
                            // Vision Light On 260530 cnz
                            MSystem.vinterface.SendToLIGHT_CTRL(m_idx, true);
                        }
                        SetStep(30);
                    }
                    else
                    {
                        CoverUp();
                        if (m_iTimer.MoreThan(5) == true)
                            SetError(5); // Not detected sensor Up
                    }
                    break;
                case 30:
                    if (IsFixUPSol() == true)
                    {
                        if (InforManager.Instance.IsBarcodeUse == true)
                        {
                            if (m_idx == 0)
                            {
                                MSystem.AutoForm.LBL_RESULT_BCR_L.ForeColor = Color.Black;
                                MSystem.AutoForm.LBL_RESULT_BCR_L.Text = "";
                            }
                            else
                            {
                                MSystem.AutoForm.LBL_RESULT_BCR_R.ForeColor = Color.Black;
                                MSystem.AutoForm.LBL_RESULT_BCR_R.Text = "";
                            }
                            MoveBarcodePosY();
                            SetStep(35);
                        }
                        else
                        {
                            if (m_idx == 0)
                            {
                                MSystem.AutoForm.LBL_RESULT_BCR_L.ForeColor = Color.Black;
                                MSystem.AutoForm.LBL_RESULT_BCR_L.Text = "BCR NOT USE";
                            }
                            else
                            {
                                MSystem.AutoForm.LBL_RESULT_BCR_R.ForeColor = Color.Black;
                                MSystem.AutoForm.LBL_RESULT_BCR_R.Text = "BCR NOT USE";
                            }
                            MoveCoverPosY();
                            SetStep(45);
                        }
                    }
                    else
                    {
                        FixUP();
                        if (m_iTimer.MoreThan(5) == true)
                            SetError(5); // Not detected sensor fwd 
                    }
                    break;

                case 35:
                    if (IsMoveDoneBarcodePosY() == true)
                    {
                        MSystem.m_pTrsScrew[m_idx].m_bBcrResult = BCR_RESULT.START;
                        SetStep(40);
                    }
                    break;
                case 40:
                    if (MSystem.m_pTrsScrew[m_idx].m_bBcrResult == BCR_RESULT.PASS)
                    {
                        MSystem.m_pTrsScrew[m_idx].m_bBcrResult = BCR_RESULT.IDLE;

                        MoveCoverPosY();
                        SetStep(45);
                        break;
                    }
                    else if (MSystem.m_pTrsScrew[m_idx].m_bBcrResult == BCR_RESULT.FAIL)
                    {
                        m_bIsOnBarcodeFail = true;
                        MSystem.m_pTrsScrew[m_idx].m_bBcrResult = BCR_RESULT.IDLE;
                        MoveReadyPosY();
                        SetStep(330);
                        break;
                    }
                   
                    break;
                case 42:
                    var mesTask = PrevInspInfoTask;
                    if (mesTask != null && mesTask.IsCompleted && !mesTask.IsFaulted && !mesTask.IsCanceled)
                    {
                        string response = mesTask.Result;
                        string rsltCode = MSystem.HttpRestClient[m_idx].GetRsltCode(response);

                        if (rsltCode == "PASS")
                        {
                            SetStep(45);
                        }
                        else
                        {
                            string errorCode = MSystem.HttpRestClient[m_idx].GetErrorCode(response);
                            m_pTrsScrew[m_idx].IsScrewOK = false;
                            m_pTrsBuzzer.SetBuzzerPattern(0);
                            string message = $@"{(m_idx == 0 ? "LEFT" : "RIGHT")} {errorCode}";
                            //MSystem.MyMsgMemo(message, "Auto Aline Fail", msgButton.YESNO);
                            m_pTrsScrew[m_idx].IsScrewOK = false;
                            m_pTrsBuzzer.StopAllBuzzers();
                            SetStep(300);
                        }
                        // 다음 시퀀스로 이동
                        PrevInspInfoTask = null;
                    }
                    else if (m_iTimer.MoreThan(2) == true)
                        SetError(6); // Not detected sensor Up
                    break;
                case 45:
                    if (IsMoveDoneCoverPosY() == true)
                    {
                        Thread.Sleep(50);
                        CoverDown();
                        SetStep(50);
                    }
                    break;
                case 50:
                    if (IsCoverDownFront() == true)
                    {
                        Thread.Sleep(100);
                        FixDown();
                        SetStep(60);
                    }
                    else if (m_iTimer.MoreThan(5) == true)
                        SetError(5);
                    break;
                case 60:
                    if (IsFixDownSol() == true)
                    {
                        if (InforManager.Instance.AutoVisionAlignMode != 0)
                        {
                            MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.START;
                            SetStep(70);
                        }
                        else
                        {
                            _isFirstMove = true;
                            SetStep(100);
                        }
                    }
                    else if (m_iTimer.MoreThan(5) == true)
                            SetError(5);
                    break;

                case 70:
                    JigStatusOrg = 0; // add 0719 jlyoon
                    if (MSystem._Rework[m_idx] || MSystem.vinterface.m_VisionResult[m_idx] == VS_RESULT.PASS) //|| IsDry())
                    {
                        // Vision Light On 260530 cnz
                        MSystem.vinterface.SendToLIGHT_CTRL(m_idx, false);
                        
                        _isFirstMove = true;
                        SetStep(100);
                        break;
                    }
                    else if (MSystem.vinterface.m_VisionResult[m_idx] == VS_RESULT.FAIL)
                    {
                        // Vision Light On 260530 cnz
                        MSystem.vinterface.SendToLIGHT_CTRL(m_idx, false);
                        MSystem.m_pTrsScrew[m_idx].m_bOnAlignFail = false;
                        JigStatus = 8; // Align Fail...!
                        JigStatusOrg = JigStatus;
                        MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.IDLE;
                        MoveCoverPosY();
                        SetStep(310);
                        break;
                    }

                    break;
                case 100:
                    if (InforTeaching.Instance.P_Screw[m_idx][m_iIndexPoint].Skip == true)
                    {
                        m_pTrsScrew[m_idx].IsScrewOK = true;
                        SetStep(300);
                        break;
                    }
                    if (_isFirstMove == true)
                    {
                        _isFirstMove = false;
                        MoveScrewPosY(m_iIndexPoint);
                    }

                    if (IsMoveDoneScrewPosY(m_iIndexPoint) == true && m_pTrsScrew[m_idx].IsScrewDone == false && IsCoverDownFront() == true && IsFixDownSol() == true)
                    {
                        SetStep(200);
                    }

                    if (IsOverTime(1.5))// add  jlyoon to fix vision bug
                    {
                        SetStep(100);
                        MoveScrewPosY(m_iIndexPoint);
                    }
                    break;
                case 200:
                    if (IsScrewPosY(m_iIndexPoint) == false)
                    {
                        m_bCallScrew = false;
                        SetStep(100);
                        break;
                    }

                    if (m_pTrsScrew[m_idx].IsScrewDone == true) // WAIT SCREW DONE
                    {
                        MSystem.m_pTrsScrew[m_idx].m_bBcrResult = BCR_RESULT.IDLE;
                        m_pTrsScrew[m_idx].IsScrewDone = false;
                        m_bCallScrew = false;
                        SetStep(300);
                        break;
                    }

                    m_bCallScrew = true;
                    break;

                case 300:
                    if (m_pTrsScrew[m_idx].IsScrewOK == false && !InforManager.Instance.ContinuesScrew) // CHECK RESULT
                    {
                        Console.WriteLine("End_1");
                        MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.IDLE;
                        MoveCoverPosY();
                        SetStep(310);
                        break;
                    }
                    else if(m_pTrsScrew[m_idx].IsScrewOK == false && 
                        InforManager.Instance.ContinuesScrew && !MSystem._Rework[m_idx])
                    {
                        try
                        {
                            ProductNG[m_idx] = true;
                            InforProduct.Instance.ProductDetail[m_idx][m_iIndexPoint].m_dCountFail += 1;
                            InforProduct.Instance.SaveSettings();
                            InforTeaching.Instance.SaveSettings(); //Test Delete SaveSetting
                        }
                        catch (Exception ex)
                        {
                            MyMessagerBottom($"DataRunning.json Check: {ex.Message}");
                        }
                    }
                    m_iIndexPoint += 1;
                    if (m_iIndexPoint >= InforTeaching.Instance.P_Screw[m_idx].Count)
                    {
                        Console.WriteLine("End_2");
                        MoveCoverPosY(); //Insert khjo
                        SetStep(310);
                        MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.IDLE;
                    }
                    else
                    {
                        _isFirstMove = true;
                        SetStep(100);
                    }
                    break;

                case 310:
                    if (IsMoveDoneCoverPosY() == true)
                    {
                        FixUP();
                        SetStep(315);
                    }
                    break;

                case 315:
                    if (IsFixUPSol() == true)
                    {
                        Thread.Sleep(100);
                        CoverUp();
                        SetStep(320);
                    }
                    break;
                case 320:
                    if (IsCoverUPFront() == true)
                    {
                        MoveReadyPosY();
                        SetStep(330);
                    }
                    break;
                case 330:
                    if (IsMoveDoneReadyPosY() == true)
                    {
                        if ((InforManager.Instance.IsBarcodeUse == true && !m_bIsOnBarcodeFail)
                            || InforManager.Instance.IsBarcodeUse == false)
                        {
                            if (m_pTrsScrew[m_idx].IsScrewOK == true && !m_pTrsScrew[m_idx].m_bLastResultScrewNG)
                            {
                                if (JigStatus != 8 && JigStatus != 9)
                                    JigStatus = 3;
                                InforProduct.Instance.ProductScrew_Pass[m_idx] += 1;
                                if (InforManager.Instance.AutoVisionAlignMode != 0)
                                    InforProduct.Instance.ProductScrew_Pass_Vision[m_idx] += 1;

                                if (MSystem._Rework[m_idx])
                                {
                                    MSystem._Rework[m_idx] = false;
                                    if (InforProduct.Instance.ProductScrew_Fail[m_idx] > 0)
                                    { InforProduct.Instance.ProductScrew_Fail[m_idx] -= 1; }
                                }

                            }
                            else
                            {
                                JigStatus = 4;
                                if (!MSystem._Rework[m_idx])
                                {
                                    try
                                    {
                                        ProductNG[m_idx] = true;
                                        InforProduct.Instance.ProductDetail[m_idx][m_iIndexPoint].m_dCountFail += 1;
                                    }
                                    catch (Exception ex)
                                    {
                                        MyMessagerBottom($"DataRunning.json Check: {ex.Message}");
                                    }
                                }
                                m_pTrsScrew[m_idx].m_bLastResultScrewNG = false;
                                m_pTrsBuzzer.SetBuzzerPattern(0);
                            }
                            if (ProductNG[m_idx] && !MSystem._Rework[m_idx])
                            {
                                if (JigStatusOrg != 8 && JigStatusOrg != 9)
                                {
                                    InforProduct.Instance.ProductScrew_Fail[m_idx] += 1;
                                }
                                ProductNG[m_idx] = false;
                            }
                            InforProduct.Instance.SaveSettings();
                            InforTeaching.Instance.SaveSettings();//Delete Save Setting

                        }
                        m_bIsOnBarcodeFail = false;
                        ScrewTime.Stop();

                        // THÊM LOG HOÀN THÀNH SẢN PHẨM
                        int ngCount = 0;
                        for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                        {
                            if (InforTeaching.Instance.P_Screw[m_idx][i].status == 2) // NG = 2
                            {
                                if (InforManager.Instance.IsBarcodeUse)
                                {
                                    string log;
                                    log = $"{DateTime.Now.ToString("HH:mm:ss fff")} Barcode={MSystem.m_pBcr[m_idx].BarcodeData} Point=P{(i + 1).ToString()} Result=NG Torque={MSystem.m_pTrsScrew[m_idx]._LastTorque.ToString("F2")}";
                                    MSystem.m_pLogSave.AddTail(LogIndex.eLogBarcodeResultL + m_idx, log);
                                }
                                ngCount++;
                            }
                        }
                        int okCount = 0;
                        for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                        {
                            if (InforTeaching.Instance.P_Screw[m_idx][i].status == 1) // OK = 1
                                okCount++;
                        }
                        string finalResult = (JigStatus == 3) ? "OK" : "NG";
                        double totalTimeSeconds = ScrewTime.ElapsedMilliseconds / 1000.0;

                        string completeLog = $"LogScrew_{DateTime.Now:yyyyMMdd}- PRODUCT_COMPLETE:{DateTime.Now:yyyyMMdd_HHmmss} | Total_Time: {totalTimeSeconds:F2}s | Result: {finalResult} |OK_Count:{okCount}| NG_Count: {ngCount}| Total: {okCount + ngCount}";

                        if (m_idx == Constants.left)
                            m_pLogSave.AddTail(LogIndex.eLogScrew1, completeLog);
                        else
                            m_pLogSave.AddTail(LogIndex.eLogScrew2, completeLog);

                        if (IsDry() == false)
                        {
                            isStart[m_idx] = false;
                        }
                        // 일단 주석 안쓰고 있음. 260529 cnz
                        //if (InforManager.Instance.IsBarcodeUse == true
                        //    && InforManager.Instance.GeimUse == true)
                        //{
                        //    PrevInspInfoTask = MSystem.HttpRestClient[m_idx].SendSaveInspInfo(m_idx);
                        //    SetStep(340);
                        //}
                        //else
                        {
                            SetStep(10);
                        }
                    }
                    break;
                case 340:
                    var saveTask = PrevInspInfoTask;
                    if (saveTask != null && saveTask.IsCompleted && !saveTask.IsFaulted && !saveTask.IsCanceled)
                    {
                        string response = saveTask.Result;
                        string rsltCode = MSystem.HttpRestClient[m_idx].GetRsltCode(response);

                        if (rsltCode == "PASS")
                        {
                            SetStep(10);
                        }
                        else
                        {
                            string errorCode = MSystem.HttpRestClient[m_idx].GetErrorCode(response);
                            m_pTrsScrew[m_idx].IsScrewOK = false;
                            m_pTrsBuzzer.SetBuzzerPattern(0);
                            string message = $@"{(m_idx == 0 ? "LEFT" : "RIGHT")} {errorCode}";
                            MSystem.MyMsgMemo(message, "Auto Aline Fail", msgButton.YESNO);
                            m_pTrsScrew[m_idx].IsScrewOK = false;
                            m_pTrsBuzzer.StopAllBuzzers();
                        }
                        // 다음 시퀀스로 이동
                        PrevInspInfoTask = null;
                    }
                    break;
                default:
                    break;
            }
        }
#endregion

        private void CheckLimitSensor()
        {
            if (_mmceAxisY.IsPlusLimit() == true)
                SetError(0);

            if (_mmceAxisY.IsMinusLimit() == true)
                SetError(1);

            if (IsAxisError(_axisY) == true)
                SetError(2);
        }
        public void LogStep(int step)
        {
            string Log = $"LogJigStep_{DateTime.Now:yyyyMMdd}- {DateTime.Now:HH:mm:ss fff} Step : {step}";

            m_pLogSave.AddTail(LogIndex.eLogScrew1 + m_idx, Log);
        }
        private void ResetScrewResult()
        {
            for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                InforTeaching.Instance.P_Screw[m_idx][i].status = 0;
        }
        /******************************************************/
        #region IF Function
        public int GetFirstPoint()
        {
            int i = 0;
            foreach (var _PCheck in InforTeaching.Instance.P_Screw[(int)m_idx])
            {
                if (!_PCheck.Skip)
                    return i;
                i++;
            }
            return -1;
        }
        #endregion

        #region Control IO Function
        public bool IsJigLightCurtainDetect()
        {
            if (m_idx == Constants.left)
            {
                if (InforManager.Instance.InspectionType == "VST" || InforManager.Instance.InspectionType == "H8")
                {
                    if (m_pDIO.IsOff(LightCurtainSensor) || m_pDIO.IsOff(LightCurtainSensor2))
                        return true;
                    else
                        return false;
                }
                else
                    return m_pDIO.IsOff(LightCurtainSensor);
            }
            else if (m_idx == Constants.right)
            {
                if (InforManager.Instance.InspectionType == "VST" || InforManager.Instance.InspectionType == "H8")
                {
                    if (m_pDIO.IsOff(LightCurtainSensor) || m_pDIO.IsOff(LightCurtainSensor2))
                        return true;
                    else
                        return false;
                }
                else
                    return m_pDIO.IsOff(LightCurtainSensor);
            }
            return false;
        }

        public bool IsPressStop()
        {
            if (m_pDIO.IsOn(IOMap.IN["IN_FRONT_OP_BOX_STOP_SW"]) == true)
                return true;

            if (m_idx == Constants.left && m_pDIO.IsOn(IOMap.IN["IN_LEFT_JIG_STOP_SW_L"]) == true)
                return true;

            if (m_idx == Constants.right && m_pDIO.IsOn(IOMap.IN["IN_RIGHT_JIG_STOP_SW_R"]) == true)
                return true;

            return false;
        }

        ///////////////////////////////////////////////////////////////////
        
        #region Smart Kit
        public bool IsDetected()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;

            return m_pDIO.IsOn(JigDetectSensor);
        }

        public bool IsFwd()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;

            return m_pDIO.IsOn(JigFwdSensor);
        }

        public bool IsBwd()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;

            return m_pDIO.IsOn(JigBwdSensor);
        }

        public bool IsUp()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;

            return m_pDIO.IsOff(JigDownSensor);
        }

        public bool IsDown()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;

            return m_pDIO.IsOn(JigDownSensor);
        }

        public bool IsCentering()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;

            return m_pDIO.IsOn(JigCenteringSensor);
        }
        public bool IsUnCentering()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;

            return m_pDIO.IsOff(JigCenteringSensor);
        }

        public void Fwd()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return;

            m_pDIO.OutPutOn(JigFwdSol);
            m_pDIO.OutPutOff(JigBwdSol);
        }

        public void Bwd()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return;

            m_pDIO.OutPutOff(JigFwdSol);
            m_pDIO.OutPutOn(JigBwdSol);
        }

        public bool IsFwdSol()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;

            return m_pDIO.IsOn(JigFwdSol);
        }

        public void StopFwdBwd()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return;

            m_pDIO.OutPutOff(JigFwdSol);
            m_pDIO.OutPutOff(JigBwdSol);
        }

        public void Up()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return;

            m_pDIO.OutPutOff(JigUpDownSol);
        }

        public void Down()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return;

            m_pDIO.OutPutOn(JigUpDownSol);
        }

        public bool IsDownSol()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;

            return m_pDIO.IsOn(JigUpDownSol);
        }

        public void Centering()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return;

            m_pDIO.OutPutOn(JigCenteringSol);
        }

        public void Uncentering()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return;

            m_pDIO.OutPutOff(JigCenteringSol);
        }
        #endregion
        public void CoverUp()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return ;
            m_pDIO.OutPutOn(DO_JigCoverUpSol);
            m_pDIO.OutPutOff(DO_JigCoverDownSol);
        }
        public bool IsCoverUPRear()
        {
            return false;
        }

        public bool IsCoverUPFront()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;
            if (m_pDIO.IsOn(DI_JigLeftCoverUpSensor) && m_pDIO.IsOn(DI_JigRightCoverUpSensor))
                return true;
            return false;
        }

        ///////////////////////////////////////////////////////////////////
        public void CoverDown()
        {
            m_pDIO.OutPutOn(DO_JigCoverDownSol);
            m_pDIO.OutPutOff(DO_JigCoverUpSol);
        }

        public bool IsCoverDownRear()
        {
            return false;
        }
        public bool IsCoverDownFront()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;
            if (m_pDIO.IsOn(DI_JigLeftCoverDownSensor) && m_pDIO.IsOn(DI_JigRightCoverDownSensor))
                return true;
            return false;
        }

        ///////////////////////////////////////////////////////////////////
        public void FixUP()
        {
            m_pDIO.OutPutOn(DO_JigCoverUnFixSol);
            m_pDIO.OutPutOff(DO_JigCoverFixSol);
        }
        public bool IsFixUPSol()
        {
            if (InforManager.Instance.IsSmartKitUse == false)
                return true;
            if (m_pDIO.IsOff(DI_JigFrontCoverFixSensor1_2) && m_pDIO.IsOff(DI_JigRearCoverFixSensor3_4))
                return true;
            return false;
        }

        ///////////////////////////////////////////////////////////////////
        public void FixDown()
        {
            m_pDIO.OutPutOn(DO_JigCoverFixSol);
            m_pDIO.OutPutOff(DO_JigCoverUnFixSol);
        }
        public bool IsFixDownSol()
        {
            if (m_pDIO.IsOn(DI_JigFrontCoverFixSensor1_2) && m_pDIO.IsOn(DI_JigRearCoverFixSensor3_4))
                return true;
            return false;
        }

        public bool IsDetectCover()
        {
            if (m_pDIO.IsOn(DI_JigDetectSensor))
                return true;
            return false;
        }

        public bool IsSleeveDetect() //khjo
        {
            if (m_pDIO.IsOn(DI_JigSleeveDetect))
                return true;
            return false;
        }
#endregion

        #region SERVOR MOVE
        public int MoveReadyPosY()
        {
            double dY = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_READY].Y;
            return MovePosition(_axisY, dY); //InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_READY].Y);
        }

        public bool IsMoveDoneReadyPosY()
        {
            return _mmceAxisY.IsReady() && _mmceAxisY.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_READY].Y);
        }

        public bool IsReadyPosY()
        {
            double dY = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_READY].Y;
            return _mmceAxisY.IsMoveDone(dY); //InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_READY].Y);
        }


        public int MoveScrewPosY(int index)
        {
            if (MSystem.m_pTrsScrew[m_idx].IsSafetyZ() == false) // add 250704 jlyoon
                return -1;

            try
            {
                if (InforManager.Instance.AutoVisionAlignMode != 0)
                    return _mmceAxisY.StartMove(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y + m_dVisionOffsetY[index]);
                return _mmceAxisY.StartMove(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y);
            }
            catch (Exception)
            {
                Thread.Sleep(10000);
                return -1;
            }
        }

        public bool IsMoveDoneScrewPosY(int index)
        {
            if (InforManager.Instance.AutoVisionAlignMode != 0)
                return _mmceAxisY.IsReady() && _mmceAxisY.IsMoveDone(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y + m_dVisionOffsetY[index]);
            return _mmceAxisY.IsReady() && _mmceAxisY.IsMoveDone(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y);
        }

        public bool IsScrewPosY(int index)
        {
            if (InforManager.Instance.AutoVisionAlignMode != 0)
                return _mmceAxisY.IsReady() && _mmceAxisY.IsMoveDone(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y + m_dVisionOffsetY[index]);
            return _mmceAxisY.IsMoveDone(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y);
        }

        /******************************************************/
        public int MovePosition(Axis iAxis, double dPos)
        {
            return SetMove((int)iAxis, dPos);
        }
        public bool IsMoveComplete(Axis iAxis, double dPos)
        {
            return IsAxisPositionCheck((int)iAxis, dPos);
        }
        public int MoveBarcodePosY()
        {
            return MovePosition(_axisY, InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_BARCODE].Y);
        }

        public bool IsMoveDoneBarcodePosY()
        {
            return _mmceAxisY.IsReady() && _mmceAxisY.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_BARCODE].Y);
        }
        public int MoveCoverPosY()
        {
            return MovePosition(_axisY, InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_COVER].Y);
        }

        public bool IsMoveDoneCoverPosY()
        {
            return _mmceAxisY.IsReady() && _mmceAxisY.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_COVER].Y);
        }
        #endregion
    }
}

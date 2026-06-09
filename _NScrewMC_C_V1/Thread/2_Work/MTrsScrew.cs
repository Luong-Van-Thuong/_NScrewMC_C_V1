using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Net.Mime.MediaTypeNames;

namespace _NScrewMC_C_V1
{
    public class MTrsScrew : MSystem
    {
        public enum MES_RESULT
        {
            NONE,
            PASS,
            FAIL
        }
        #region Variable
        private int count_t = 0;
        int _Result = MMC_OK;
        /*   ushort[] temp; //Event Hantas Data
           ushort[] temp2;//Realtime Hantas Data*/
        ushort[] realtimeData;
        ushort[] eventData;// Event Hantas Data
        int lastA2Angle = 0;
        public int m_idx = 0;
        public bool m_bCallJig = false;
        public bool m_bIsCheckSleverDone = false;
        public bool[] freeder_not_ready = { false, false };
        public bool[] freeder_ready = { true, true };
        public int _ScrewPickRetry = 0;
        public int _ScrewNoneCount = 0;
        public bool m_bOnPickupError = false;
        public bool m_bOnAlignFail = false;
        public UNITINFOR _Infor = new UNITINFOR();
        public bool m_bMovedoneBarcode = false;
        public bool m_bReqScanBcr = false;
        public BCR_RESULT m_bBcrResult = new BCR_RESULT();
        public double _Torque = 0.0;

        public int m_iCurrentStep = 0;
        public int m_iNextStep = 0;
        public int m_iPreviewStep = 0;
        public int m_iTempStep = 0;
        int m_RetryBCR = 0;

        //Fastening variable
        private bool m_bScrewSuccessDetected = false;
        private bool m_bReFasteningRequested = false;
        private bool m_bReFasteningCompleted = false;
        public int _CurrentReFasteningCount = 0;
        private DateTime reFasStartTime;
        private DateTime successLockTime;

        public bool m_bFasteningOk = false;



        public bool _isInProcess = false;


        public bool m_bLastResultScrewNG = false;
        public int m_bMesResult = (int)MES_RESULT.NONE;
        public double TargetTorque { get; set; } = 0.0;
        public double _LastTorque = 0.0;

        public TimerDelay m_iTimer = new TimerDelay();
        public ProductState m_State = new ProductState();

        public PScrewMain m_iTargetPos = PScrewMain.ePOS_MASTER_SCREW;
        public PScrewMain m_iCurrentPos = PScrewMain.ePOS_MAX;
        public PointScrew m_iCurrentScrewPoint = new PointScrew();
        public int targetIndex = 0;
        public PointScrew m_iTargetScrewPoint = new PointScrew();


        private double _dPosX = 0.0;
        private double _dPosY = 0.0;
        private double _dPosZ = 0.0;
        private double _dryOffsetM = 0.0;
        private double _dryOffsetP = 0.0;
        private double _dOffsetGun = 15.0;

        private bool _isStopOnce = true;
        private bool _isFirstMoveX = true;
        private bool _isFirstMoveZ = true;

        private Axis _axisX;
        private Axis _axisZ;
        private Axis _axisY;

        private MMCEtherCATAxis _mmceAxisX;
        private MMCEtherCATAxis _mmceAxisZ;
        private MMCEtherCATAxis _mmceAxisY;

        private int VacuumSensor;
        private int DriverFastenOKSensor;
        private int FeederReadySensor;
        private int ReturnSensor;
        private int GripSensor;
        private int UngripSensor;

        private int VacuumOnOffSol;
        private int RunFeeder;
        private int FeederVaccumOn;
        private int BlowOnOffSol;
        private int GripSol;
        private int UngripSol;

        private int LightCurtainSensor;

        private bool _needScrewDone = false;
        public bool IsScrewDone { get; set; } = false;
        public bool IsScrewOK { get; set; } = false;

        public Stopwatch swScriewFlowTime = new Stopwatch();
        public Stopwatch SingleScrewTime = new Stopwatch();

        private PointAxis p1Old;
        private PointAxis p2Old;
        private PointAxis p1New;
        private PointAxis p2New;
        public double[] m_dVisionOffsetX = new double[50];

        private const bool _USE_ = true;
        private const bool _NOTUSE_ = false;

        private int iAutoAlignFailCnt = 0;
        #endregion

        /******************************************************/
        #region Define STEP Run
        const int STEP_INIT = 0;
        const int STEP_WORK_WAIT = 500;
        const int STEP_PICKUP_SCREW = 2000;
        const int STEP_BARCODE = 3000;

        const int STEP_GUN_SCREW = 4000;
        const int STEP_GUN_SCREW_REFASTENING = 4000 + 500 + 10;
        const int STEP_GUN_SCREW_SUCCESS = 4000 + 500 + 20;
        const int STEP_TRASH_SCREW = 6000;
        const int STEP_AUTO_ALIGE = 7000;

        const int STEP_PICKUP_CIRCLE = 8000;
        const int STEP_GUN_CIRCLE = 10000;

        const int STEP_SERVOR_MOVE_XZ = 20000;
        const int STEP_SERVOR_MOVE_X = 21000;
        const int STEP_SERVOR_MOVE_Z = 23000;


        const int STEP_SERVOR_MOVE_SCREW_X = 25000;
        const int STEP_SERVOR_MOVE_SCREW_Z = 27000;

        const int STEP_MOVE_READY_Z = 31000;
        const int STEP_MOVE_SAFETY_Z = 32000;

        const int STEP_MOVE_CIRCLE_ST = 33000;
        const int STEP_MOVE_CIRCLE_TS = 34000;

        const int STEP_SERVOR_MOVE_SCREW_XZ = 40000;

        const int STEP_SERVOR_MOVE_LINE_XY = 41000;
        const int STEP_SERVOR_MOVE_LINE_XZ = 42000;

        const int STEP_CHECK_SLEVER = 50000;


        #endregion

        #region Class Initial
        public MTrsScrew(int _idx)
        {
            m_idx = _idx;

            if (m_idx == Constants.left)
            {
                _axisX = Axis.AXIS_X1;
                _axisZ = Axis.AXIS_Z1;
                _axisY = Axis.AXIS_Y1;

                VacuumSensor = IOMap.IN["IN_LEFT_Z_SCREW_VACUM_SENS"];//IO.IN_LEFT_Z_SCREW_VACUM_SENS;
                DriverFastenOKSensor = IOMap.IN["IN_LEFT_SCREW_DRV_FASTEN_OK"];//IO.IN_LEFT_SCREW_DRV_FASTEN_OK;
                FeederReadySensor = IOMap.IN["IN_LEFT_FEEDER_READY"];//IO.IN_LEFT_FEEDER_READY;
                LightCurtainSensor = IOMap.IN["IN_LEFT_LIGHT_CURTAIN_DETECT"];//IO.IN_LEFT_LIGHT_CURTAIN_DETECT;

                VacuumOnOffSol = IOMap.OUT["OUT_LEFT_Z_VACUM_ONOFF_SOL"];//IO.OUT_LEFT_Z_VACUM_ONOFF_SOL;
                BlowOnOffSol = IOMap.OUT["OUT_LEFT_Z_VACCUM_BLOW_SOL"];//IO.OUT_LEFT_Z_VACCUM_BLOW_SOL;
                
            }
            else
            {
                _axisX = Axis.AXIS_X2;
                _axisZ = Axis.AXIS_Z2;
                _axisY = Axis.AXIS_Y2;

                VacuumSensor = IOMap.IN["IN_RIGHT_Z_SCREW_VACUM_SENS"];//IO.IN_RIGHT_Z_SCREW_VACUM_SENS;
                DriverFastenOKSensor = IOMap.IN["IN_RIGHT_SCREW_DRV_FASTEN_OK"];//IO.IN_RIGHT_SCREW_DRV_FASTEN_OK;
                FeederReadySensor = IOMap.IN["IN_RIGHT_FEEDER_READY"]; //IO.IN_RIGHT_FEEDER_READY;
                LightCurtainSensor = IOMap.IN["IN_RIGHT_LIGHT_CURTAIN_DETECT"]; //IO.IN_RIGHT_LIGHT_CURTAIN_DETECT;

                VacuumOnOffSol = IOMap.OUT["OUT_RIGHT_Z_VACUM_ONOFF_SOL"];//IO.OUT_RIGHT_Z_VACUM_ONOFF_SOL;
                BlowOnOffSol = IOMap.OUT["OUT_RIGHT_Z_VACCUM_BLOW_SOL"];//IO.OUT_RIGHT_Z_VACCUM_BLOW_SOL;
            }
            m_bCallJig = false;

            _mRun = new Thread(ThreadJob);
            _mRun.IsBackground = true;
            _mRun.Start();

        }
        public int SetUnitInitialize()
        {
            int currentStep = 0;
            bool isFirstMoveX = true;
            bool isFirstMoveZ = true;
            TimerDelay timer = new TimerDelay();
            // Vision Light Off 260530 cnz
            MSystem.vinterface.SendToLIGHT_CTRL(m_idx, false);
            while (true)
            {
                if (IsDetectDoorOpen(out _) == true || m_pDIO.IsOn(IOMap.IN["IN_FRONT_OP_BOX_STOP_SW"]) == true)
                {
                    MSystem.MoveStop(_axisX);
                    MSystem.MoveStop(_axisZ);
                    return 1;
                }

                switch (currentStep)
                {
                    case 0:
                        DriverStop();
                        currentStep += 1;
                        timer.StartTimer();
                        break;

                    case 1:
                        if (isFirstMoveZ == true)
                        {
                            isFirstMoveZ = false;
                            MoveReadyPosZ();
                        }

                        if (IsSafetyZ() == true)
                        {
                            if (_needScrewDone == true)
                            {
                                _needScrewDone = false;
                                IsScrewDone = true;
                            }

                            //if (isFirstMoveX == true)
                            //{
                            //    isFirstMoveX = false;
                            //    _distX = Math.Abs(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].X - _mmceAxisX.GetActualPos());
                            //    MoveTrashPosX();
                            //}
                        }

                        if (IsMoveDoneReadyPosZ() == true)
                        {
                            isFirstMoveZ = true;
                            currentStep += 1;
                            timer.StartTimer();
                            break;
                        }

                        if (timer.MoreThan(10) == true)
                        {
                            // ready z move fail
                            MSystem.MoveStop(_axisX);
                            MSystem.MoveStop(_axisZ);
                            return 1;
                        }
                        break;

                    case 2:
                        if (isFirstMoveX == true)
                        {
                            isFirstMoveX = false;
                            _distX = Math.Abs(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].X - _mmceAxisX.GetActualPos());
                            MoveTrashPosX();
                        }

                        //_targetZ = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].Z;
                        //_safetyZ = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z;
                        //if (IsSafetyZDown(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].X) == true)
                        //{
                        //    if (isFirstMoveZ == true)
                        //    {
                        //        isFirstMoveZ = false;
                        //        MoveTrashPosZ();
                        //    }
                        //}

                        if (IsMoveDoneTrashPosX() == true)
                        {
                            isFirstMoveX = true;
                            currentStep += 1;
                        }

                        if (timer.MoreThan(10) == true)
                        {
                            // trash x move fail
                            MSystem.MoveStop(_axisX);
                            MSystem.MoveStop(_axisZ);
                            return 1;
                        }
                        break;

                    case 3:
                        if (isFirstMoveZ == true)
                        {
                            isFirstMoveZ = false;
                            MoveTrashPosZ();
                        }

                        if (IsMoveDoneTrashPosZ() == true)
                        {
                            isFirstMoveZ = true;
                            currentStep += 1;
                            timer.StartTimer();
                        }

                        if (timer.MoreThan(10) == true)
                        {
                            // trash z move fail
                            MSystem.MoveStop(_axisX);
                            MSystem.MoveStop(_axisZ);
                            return 1;
                        }
                        break;

                    case 4:
                        BlowOn();
                        if (timer.MoreThan(InforManager.Instance.BlowTime) == true)
                        {
                            BlowOff();
                            currentStep += 1;
                        }
                        break;

                    case 5:
                        if (isFirstMoveZ == true)
                        {
                            isFirstMoveZ = false;
                            MoveReadyPosZ();
                        }

                        if (IsMoveDoneReadyPosZ() == true)
                        {
                            isFirstMoveZ = true;
                            currentStep += 1;
                            break;
                        }

                        if (timer.MoreThan(10) == true)
                        {
                            // ready z move fail
                            MSystem.MoveStop(_axisX);
                            MSystem.MoveStop(_axisZ);
                            return 1;
                        }
                        break;

                    case 6:
                        if (m_idx == 0)
                        {
                            //Left
                            double _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eLEFT, (int)PScrewMain.ePOS_READY].X;

                            if (MSystem.SetMove((int)Axis.AXIS_X1, _dX) != MSystem.MMC_OK)
                            {
                                MSystem.MyMsgMemo($"Left X Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                                return 1;
                            }

                        }

                        else
                        {

                            //Right
                            double _dX = InforTeaching.Instance.P_MainScrew[(int)_NSC.eRIGHT, (int)PScrewMain.ePOS_READY].X;

                            if (MSystem.SetMove((int)Axis.AXIS_X2, _dX ) != MSystem.MMC_OK)
                            {
                                MSystem.MyMsgMemo($"Right X Position Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                                return 1;
                            }

                        }
                        currentStep += 1;
                        break;

                    case 7:
                        m_pTrsJig[m_idx].m_MTestTimer.ResetTimer();
                        m_pTrsJig[m_idx].m_MWaitTimer.ResetTimer();
                        IsScrewDone = false;
                        InitData();
                        return (int)InitResult.UNIT_INIT_SUCCESS;
                }

                Thread.Sleep(1);
            }
        }

        public void InitData()
        {
            count_t = 0;
            _LastTorque = 0.0;
            m_State = new ProductState();
            m_iNextStep = 0;
            m_iCurrentStep = 0;
            m_iPreviewStep = 0;
            _Infor = new UNITINFOR();

            m_bIsCheckSleverDone = false;
            m_bCallJig = false;
            m_bOnPickupError = false;
            freeder_ready[m_idx] = true;
            m_bOnAlignFail = false;
            _ScrewPickRetry = 0;
            lastA2Angle = 0;
            MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.IDLE;
            m_bLastResultScrewNG = false;
            m_bReqScanBcr = false;

            ResetFasteningState();
            freeder_ready[m_idx] = true;
            freeder_not_ready[m_idx] = false;
            m_bMovedoneBarcode = false;
        }
#endregion
        /******************************************************/
        #region // Tool Get Step and Timer
        public void SetStep(int _step)
        {
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
                MSystem.SetError(3000 + _idError, $"Left Screw");
            else
                MSystem.SetError(4000 + _idError, $"Right Screw");
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
                Thread.Sleep(1);
                try
                {
                    dorunStep();
                }
                catch (Exception ex)
                {
                    MyMessagerBottom($"Logic Screw Unit Fail {m_iCurrentStep} - {m_idx} - {ex.ToString()}");
                    MSystem.m_pLogSave.DevLogSave($"Logic Screw Unit Fail {m_iCurrentStep} - {m_idx} - {targetIndex} - {ex.ToString()}- {ex.StackTrace.ToString()}");
                    Thread.Sleep(1000);
                }
            }
        }

        public void dorunStep()
        {
            if (_mmceAxisX == null)
            {
                if (m_idx == Constants.left)
                    _mmceAxisX = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)Axis.AXIS_X1];
                else
                    _mmceAxisX = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)Axis.AXIS_X2];
                return;
            }

            if (_mmceAxisZ == null)
            {
                if (m_idx == Constants.left)
                    _mmceAxisZ = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)Axis.AXIS_Z1];
                else
                    _mmceAxisZ = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)Axis.AXIS_Z2];
                return;
            }
            // add 260402 cnz
            if (_mmceAxisY == null)
            {
                if (m_idx == Constants.left)
                    _mmceAxisY = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)Axis.AXIS_Y1];
                else
                    _mmceAxisY = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)Axis.AXIS_Y2];
                return;
            }
           
            if (isStart[m_idx] == false)
            {
                if (m_iCurrentStep < STEP_GUN_SCREW + 400
                    || m_iCurrentStep > STEP_GUN_SCREW_SUCCESS)
                {
                    if (_isStopOnce == false)
                    {
                        _isStopOnce = true;
                        _isFirstMoveX = true;
                        _isFirstMoveZ = true;
                        MSystem.MoveStop(_axisX);
                        MSystem.MoveStop(_axisZ);
                        BlowOff();
                        swScriewFlowTime.Stop();
                    }
                    return;
                }
            }

            CheckLimitSensor();
            _isStopOnce = false;

            switch (m_iCurrentStep)
            {
                case STEP_INIT:
                    InitData();
                    SetStep(STEP_WORK_WAIT);
                    break;

                case STEP_WORK_WAIT:
                    {
                        if (IsVacuumSensorOn())
                        {
                            if (InforManager.Instance.IsBarcodeUse == true && !MSystem._Rework[m_idx])
                            {
                                _isFirstMoveZ = true;
                                SetStep(STEP_BARCODE);
                                break;
                            }
                            if (InforManager.Instance.AutoVisionAlignMode != 0
                                && !MSystem._Rework[m_idx]) // add 250705 jlyoon
                            {
                                if (MSystem.vinterface.m_VisionResult[m_idx] != VS_RESULT.PASS && MSystem.vinterface.m_VisionResult[m_idx] != VS_RESULT.FAIL)
                                    SetStep(STEP_AUTO_ALIGE);
                                else
                                    SetStep(STEP_GUN_SCREW);
                            }
                            else
                                SetStep(STEP_GUN_SCREW);
                        }
                        else
                        {
                            if (InforManager.Instance.IsBarcodeUse == true && !MSystem._Rework[m_idx])
                            {
                                _isFirstMoveZ = true;
                                SetStep(STEP_BARCODE);
                                break;
                            }
                            m_iNextStep = STEP_GUN_SCREW;
                            SetStep(STEP_TRASH_SCREW);
                        }
                    }
                    break;

                #region Align Check
                case STEP_AUTO_ALIGE:
                    {
                        if (IsMoveDoneReadyPosZ() == false)
                        {
                            MoveReadyPosZ();
                        }
                        else
                        {
                            if (_needScrewDone == true)
                            {
                                _needScrewDone = false;
                                IsScrewDone = true;
                            }
                            SetStep(STEP_AUTO_ALIGE + 10);
                        }
                    }
                    break;
                case STEP_AUTO_ALIGE + 1:
                    if (IsMoveDoneReadyPosZ() == false)
                    {
                        MoveReadyPosZ();
                    }
                    SetStep(STEP_AUTO_ALIGE + 2);
                    break;
                case STEP_AUTO_ALIGE + 2:
                    if (IsMoveDoneReadyPosZ() == true)
                    {
                        MoveScrewPosX(0);
                        SetStep(STEP_AUTO_ALIGE + 3);
                    }
                    break;
                case STEP_AUTO_ALIGE + 3:
                    if (IsMoveDoneScrewPosX(0) == true)
                    {
                        SetStep(STEP_AUTO_ALIGE + 10);
                    }
                    break;
                case STEP_AUTO_ALIGE + 10:
                    {
                        if (MSystem._Rework[m_idx]) // add 250722 jlyoon
                        {
                            //if (InforManager.Instance.IsScrewPickupReady == _USE_)
                            //    SetStep(STEP_GUN_SCREW);
                            //else
                                SetStep(STEP_PICKUP_SCREW);
                            break;
                        }
                        // 251106 cnz
                        if (InforManager.Instance.IsBarcodeUse == true && m_bBcrResult == BCR_RESULT.START)
                        {
                            SetStep(STEP_BARCODE);
                            break;
                        }

                        if (MSystem.vinterface.m_VisionResult[m_idx] == VS_RESULT.START)
                        {
                            if (VisionAlign() == true) //if (await VisionAlignStart() == true)
                            {
                                if (isStart[m_idx] == false)
                                {
                                    MSystem.SetMove((int)Axis.AXIS_Z1 + m_idx, 0.0);
                                    for (int j = 0; j < InforTeaching.Instance.P_Screw[m_idx].Count; j++)
                                    {
                                        m_dVisionOffsetX[j] = 0.0;
                                        m_pTrsJig[m_idx].m_dVisionOffsetY[j] = 0.0;
                                    }
                                    SetStep(STEP_WORK_WAIT);
                                    break;
                                }
                                iAutoAlignFailCnt = 0;
                                _isFirstMoveZ = true;
                                SetStep(STEP_PICKUP_SCREW);
                                break;
                            }
                            else
                            {
                                //iAutoAlignFailCnt++;

                                MoveReadyPosZ();
                                m_bOnAlignFail = true;
                                IsScrewOK = false;
                                IsScrewDone = true;
                                //SetStep(STEP_AUTO_ALIGE + 20);
                                SetUnitInitialize();
                                //MSystem.m_pTrsJig[m_idx].SetUnitInitialize();
                                SetStep(STEP_INIT);

                                //MoveReadyPosZ();
                                //m_pTrsBuzzer.SetBuzzerPattern(0);
                                //string message = $@"{(m_idx == 0 ? "LEFT" : "RIGHT")} Vision Aline Fail";
                                //MSystem.MyMsgMemo(message, "Auto Aline Fail", msgButton.OK, msgIcon.Error);

                                //m_pTrsScrew[m_idx].IsScrewOK = false;
                                //IsScrewDone = true;
                                //SetStep(STEP_AUTO_ALIGE);
                                //m_pTrsBuzzer.StopAllBuzzers();
                            }
                        }
                        //else if (MSystem.vinterface.m_VisionResult[m_idx] == VS_RESULT.PASS)
                        //{
                        //    if (InforManager.Instance.IsScrewPickupReady == _USE_)
                        //        SetStep(STEP_GUN_SCREW);
                        //    else
                        //        SetStep(STEP_PICKUP_SCREW);
                        //    break;
                        //}

                        //if (MSystem.vinterface.m_VisionResult[m_idx] == VS_RESULT.READY || IsDry())
                        //{
                        //    if(!IsDry())
                        //        MSystem.vinterface.SendSRS_Start(m_idx);
                        //    SetStep(STEP_AUTO_ALIGE + 20);
                        //    break;
                        //}
                        //else if (MSystem.vinterface.m_VisionResult[m_idx] == VS_RESULT.PASS)
                        //{
                        //    if (InforManager.Instance.IsScrewPickupReady == _USE_)
                        //        SetStep(STEP_GUN_SCREW);
                        //    else
                        //        SetStep(STEP_PICKUP_SCREW);
                        //    break;
                        //}
                    }
                    break;
                case STEP_AUTO_ALIGE + 20:
                    {
                        if (MSystem.vinterface.m_VisionResult[m_idx] == VS_RESULT.FAIL
                             || IsOverTime(7.0)) // add 250704 jlyoon
                        {
                            iAutoAlignFailCnt++;

                            MoveReadyPosZ();
                            m_bOnAlignFail = true;
                            m_pTrsScrew[m_idx].IsScrewOK = false;
                            IsScrewDone = false;//fix loi delay sau vision fail
                            SetStep(STEP_AUTO_ALIGE + 10);
                            if (iAutoAlignFailCnt > 3)
                            {
                                SetUnitInitialize();
                                //MSystem.m_pTrsJig[m_idx].SetUnitInitialize();
                                SetStep(STEP_INIT);
                            }
                            break;

                            //MoveReadyPosZ();
                            //m_pTrsBuzzer.SetBuzzerPattern(0);
                            //string message = $@"{(m_idx == 0 ? "LEFT" : "RIGHT")} Vision Aline Fail";
                            //MSystem.MyMsgMemo(message, "Auto Aline Fail", msgButton.OK, msgIcon.Error);
                            //MSystem.vinterface.SendSRS_Stop(m_idx);
                            //m_pTrsScrew[m_idx].IsScrewOK = false;
                            //IsScrewDone = true;
                            //SetStep(STEP_AUTO_ALIGE);
                            //m_pTrsBuzzer.StopAllBuzzers();
                        }
                    }
                    break;

                //           case STEP_AUTO_ALIGE + 25:
                //               if(m_bOnAlignFail == true)
                //{
                //                   if (iAutoAlignFailCnt > 3)
                //                   {
                //                       InforManager.Instance.IsAutoAlign = false;
                //                   }
                //}
                //               SetStep(STEP_AUTO_ALIGE + 30);
                //               break;

                //           case STEP_AUTO_ALIGE + 30:
                //               if (isStart[m_idx] == false)
                //               {
                //                   SetUnitInitialize();
                //                   //MSystem.m_pTrsJig[m_idx].SetUnitInitialize();
                //                   SetStep(STEP_INIT);
                //                   break;
                //               }
                //               break;
                #endregion

                #region PICKUP_SCREW
                case STEP_PICKUP_SCREW:
                    if (_isFirstMoveZ == true)
                    {
                        _isFirstMoveZ = false;
                        MoveReadyPosZ();
                    }
                    if (IsSafetyZ() == true)
                    {
                        if (_needScrewDone == true)
                        {
                            _needScrewDone = false;
                            IsScrewDone = true;
                        }

                        if (_isFirstMoveX == true)
                        {
                            _isFirstMoveX = false;
                            _distX = Math.Abs(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_PICKUP].X - _mmceAxisX.GetActualPos());
                            MovePickUpPosX();
                        }
                    }

                    if (IsMoveDoneReadyPosZ() == true)
                    {
                        if (m_pTrsJig[m_idx].m_bCallScrew == true)
                        {
                            _isFirstMoveZ = true;
                            SetStep(STEP_PICKUP_SCREW + 10);
                        }
                        
                        break;
                    }
                    break;

                case STEP_PICKUP_SCREW + 10:
                    if (_isFirstMoveX == true)
                    {
                        _isFirstMoveX = false;
                        _distX = Math.Abs(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_PICKUP].X - _mmceAxisX.GetActualPos());
                        MovePickUpPosX();
                    }

                    if (IsFeederReady() == true || IsDry() == true)
                    {
                        _targetZ = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_PICKUP].Z;
                        _safetyZ = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z;
                        if (IsSafetyZDown(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_PICKUP].X) == true)
                        {
                            if (_isFirstMoveZ == true)
                            {
                                _isFirstMoveZ = false;
                                MovePickUpPosZ();
                            }
                        }

                        if (IsMoveDonePickUpPosX() == true)
                        {
                            if (InforManager.Instance.IsPickupVaccumMode == false)
                                VacuumOn();
                            _isFirstMoveX = true;
                            SetStep(STEP_PICKUP_SCREW + 20);
                        }
                    }
                    if (IsOverTime(InforManager.Instance.TimeOutFeederReady))
                    {
                        //SetError(5);
                        string msg = m_idx == Constants.left ? "Left" : "Right";
                        msg += " Feeder not ready!!";
                        MSystem.MyMsgMemo(msg, "Error", msgButton.OK, msgIcon.Error); // update 250707 jlyoon
                        //freeder_ready[m_idx] = true;
                        //freeder_not_ready[m_idx] = false;
                        m_pTrsBuzzer.StopAllBuzzers();
                        isStart[m_idx] = false;
                        SetStep(STEP_PICKUP_SCREW + 10);
                        break;
                    }
                    break;

                case STEP_PICKUP_SCREW + 20:
                    if (_isFirstMoveZ == true)
                    {
                        _isFirstMoveZ = false;
                        MovePickUpPosZ();
                    }

                    if (IsMoveDonePickUpPosZ() == true)
                    {
                        if (InforManager.Instance.IsPickupVaccumMode == true)
                        {
                            if (IsVacuumSolOn() == false)
                                VacuumOn();
                        }
                        
                        if (IsVacuumSensorOn() || IsDry())// && IsOverTime(InforManager.Instance.m_dVacuumOn) // delete 260416 cnz
                        {
                            _isFirstMoveZ = true;
                            if (_ScrewNoneCount > 5)
                            {
                                SetStep(STEP_PICKUP_SCREW + 30);
                            }
                            else
                            {
                                _ScrewPickRetry = 0;
                                SetStep(STEP_PICKUP_SCREW + 25);
                            }

                            break;
                        }
                        if (IsOverTime(3.0))
                        {
                            SetStep(STEP_PICKUP_SCREW + 30);
                            break;
                        }
                    }
                    if (IsOverTime(3.0))
                    {
                        SetStep(STEP_PICKUP_SCREW + 30);
                        break;
                    }
                    break;
                case STEP_PICKUP_SCREW + 25:
                    if (IsOverTime(InforManager.Instance.PickupDelayTime))
                    {
                        // Pickup Ready Option 때문에 추가했었는데 필요 없음. 260526 cnz
                        //if (_Rework[m_idx] == true)
                        //{
                        //    SetStep(STEP_GUN_SCREW);
                        //}
                        //else if (InforManager.Instance.AutoVisionAlignMode != 0
                        //    && MSystem.vinterface.m_VisionResult[m_idx] == VS_RESULT.PASS)
                        ////&& InforManager.Instance.IsScrewPickupReady == _USE_)
                        //{
                        //    //if (MSystem.vinterface.m_VisionResult[m_idx] == VS_RESULT.PASS)
                        //    SetStep(STEP_GUN_SCREW);
                        //    //else
                        //    //SetStep(STEP_AUTO_ALIGE);
                        //}
                        //else if (InforManager.Instance.AutoVisionAlignMode != 0
                        //    && MSystem.vinterface.m_VisionResult[m_idx] != VS_RESULT.PASS)
                        //{
                        //    SetStep(STEP_AUTO_ALIGE);
                        //}
                        //else
                        {
                            _isFirstMoveX = true;
                            _isFirstMoveZ = true;
                            SetStep(STEP_GUN_SCREW);
                        }
                        
                    }
                    break;
                case STEP_PICKUP_SCREW + 30:
                    if (_ScrewPickRetry > 5 || _ScrewNoneCount > 5)
                    {
                        _ScrewNoneCount = 0;
                        _ScrewPickRetry = 0;
                        MoveReadyPosZ();
                        SetStep(STEP_TRASH_SCREW);
                        m_bOnPickupError = true;
                        m_pTrsScrew[m_idx].IsScrewOK = false;
                        m_pTrsScrew[m_idx].IsScrewDone = true;

                        // m_bOnPickupError == true 일때 알람은 MessengerPoll()에서 처리.
                        /*
                        m_pTrsBuzzer.SetBuzzerPattern(0);
                        string message = "";
                        IniErrorCodeRead(4, ref message);
                        message += $"\r\nYES : Continue   NO : Out";
                        if (MSystem.MyMsgMemo(message, "Pickup Error", msgButton.YESNO, msgIcon.Error) == System.Windows.Forms.DialogResult.No)
                        {
                            m_pTrsScrew[m_idx].IsScrewOK = false;
                            m_pTrsScrew[m_idx].IsScrewDone = true;
                            m_pTrsJig[m_idx].m_iIndexPoint = 0;
                        }
                        m_pTrsBuzzer.StopAllBuzzers();
                        //SetError(4);
                        */
                    }
                    else
                    {
                        _isFirstMoveZ = true;
                        _ScrewPickRetry++;
                        m_bOnPickupError = false;
                        SetStep(STEP_TRASH_SCREW);
                    }
                    break;
                #endregion

                #region GUN SCREW
                case STEP_GUN_SCREW:
                    _isInProcess = true;
                    if (_isFirstMoveZ == true)
                    {
                        _isFirstMoveZ = false;
                        MoveReadyPosZ();
                    }
                    if (m_pTrsJig[m_idx].m_iIndexPoint >= InforTeaching.Instance.P_Screw[m_idx].Count)
                        break;

                    if (targetIndex != m_pTrsJig[m_idx].m_iIndexPoint)
                    {
                        if (_mmceAxisX.IsAxisDone() == false)
                            break;

                        _isFirstMoveX = true;
                    }

                    if (IsSafetyZ() == true)
                    {
                        if (_isFirstMoveX == true)
                        {
                            _isFirstMoveX = false;
                            if (m_pTrsJig[m_idx].m_bCallScrew)
                            {
                                targetIndex = m_pTrsJig[m_idx].m_iIndexPoint;
                                m_iTargetScrewPoint = InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].m_iIndexPoint];

                            }
                            else
                            {
                                if (_isInProcess)
                                {
                                    targetIndex = m_pTrsJig[m_idx].m_iIndexPoint;
                                    m_iTargetScrewPoint = InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].m_iIndexPoint];

                                }
                                else
                                {
                                    targetIndex = m_pTrsJig[m_idx].GetFirstPoint();
                                    m_iTargetScrewPoint = InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].GetFirstPoint()];

                                }
                            }
                            _distX = Math.Abs(m_iTargetScrewPoint.PScrew.X - _mmceAxisX.GetActualPos());
                            MoveScrewPosX(targetIndex);

                        }
                    }

                    if (IsMoveDoneReadyPosZ() == true)
                    {
                        _isFirstMoveZ = true;
                        SetStep(STEP_GUN_SCREW + 10);
                        if (swScriewFlowTime.IsRunning == false)
                            swScriewFlowTime.Restart();
                        break;
                    }
                    break;

                case STEP_GUN_SCREW + 10:
                    if (targetIndex != m_pTrsJig[m_idx].m_iIndexPoint)
                    {
                        if (_mmceAxisX.IsAxisDone() == false)
                            break;

                        _isFirstMoveX = true;
                    }

                    if (_isFirstMoveX == true)
                    {
                        _isFirstMoveX = false;
                        if (m_pTrsJig[m_idx].m_bCallScrew)
                        {
                            targetIndex = m_pTrsJig[m_idx].m_iIndexPoint;
                            m_iTargetScrewPoint = InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].m_iIndexPoint];
                        }
                        else
                        {
                            if (_isInProcess)
                            {
                                targetIndex = m_pTrsJig[m_idx].m_iIndexPoint;
                                m_iTargetScrewPoint = InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].m_iIndexPoint];
                            }
                            else
                            {
                                targetIndex = m_pTrsJig[m_idx].GetFirstPoint();
                                m_iTargetScrewPoint = InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].GetFirstPoint()];
                            }
                        }
                        _distX = Math.Abs(m_iTargetScrewPoint.PScrew.X - _mmceAxisX.GetActualPos());
                        MoveScrewPosX(targetIndex);

                    }

                    _targetZ = m_iTargetScrewPoint.PScrew.Z - InforManager.Instance.m_dScrewOffset;
                    _safetyZ = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z;
                    if (IsSafetyZDown(m_iTargetScrewPoint.PScrew.X) == true)
                    {
                        if (m_pTrsJig[m_idx].m_bCallScrew)
                        {
                            if (_isFirstMoveZ == true)
                            {
                                _isFirstMoveZ = false;
                                DriverRun(InforTeaching.Instance.P_Screw[m_idx][targetIndex].Channel);
                                MoveScrewReadyPosZ(targetIndex);
                            }
                        }
                    }

                    if (IsMoveDoneScrewPosX(targetIndex) == true)
                    {
                        if (m_pTrsJig[m_idx].m_bCallScrew)
                        {
                            _isFirstMoveX = true;
                            SetStep(STEP_GUN_SCREW + 20);
                        }
                    }
                    
                    // 251106 cnz
                    if (InforManager.Instance.IsBarcodeUse == true && m_bBcrResult == BCR_RESULT.START)
                    {
                        SetStep(STEP_INIT);
                        break;
                    }
                    if (IsOverTime(10.0)  && !m_pTrsJig[m_idx].m_bCallScrew)
                    {
                        SetStep(STEP_INIT);
                        break;
                    }
                    break;

                case STEP_GUN_SCREW + 20:
                    if (_isFirstMoveZ == true)
                    {
                        _isFirstMoveZ = false;
                        DriverRun(InforTeaching.Instance.P_Screw[m_idx][targetIndex].Channel);
                        MoveScrewReadyPosZ(targetIndex);
                    }

                    if (IsMoveDoneScrewReadyPosZ(targetIndex) == true)
                    {
                        _isFirstMoveZ = true;
                        SetStep(STEP_GUN_SCREW + 30);
                    }
                    break;

                case STEP_GUN_SCREW + 30:
                    if (IsVacuumSensorOn() == false && IsDry() == false)
                    {
                        _ScrewNoneCount++;
                        SetStep(STEP_TRASH_SCREW);
                        break;
                    }
                    _ScrewNoneCount = 0;
                    SetStep(STEP_GUN_SCREW + 400);
                    break;

                case STEP_GUN_SCREW + 400:
                    {
                        //VacuumOn();
                        DriverRun(InforTeaching.Instance.P_Screw[m_idx][targetIndex].Channel);// Channel Select Delete 251118 cnz

                        _dPosZ = InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].m_iIndexPoint].PScrew.Z;

                        SingleScrewTime.Restart();

                        if (IsDry())
                        {
                            _dPosZ -= _dryOffsetP;
                            if (_dPosZ < 0)
                                _dPosZ = 0;
                        }

                        _Result = MovePosition(_axisZ, _dPosZ, InforManager.Instance.m_dSpeedScrew);
                        SetStep(STEP_GUN_SCREW + 500);
                    }
                    break;
                case STEP_GUN_SCREW + 500:
                    {
                        //VacuumOn();
                        _dPosZ = InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].m_iIndexPoint].PScrew.Z;
                        if (IsDry())
                        {
                            _dPosZ = _dPosZ - _dryOffsetP;
                            if (_dPosZ < 0)
                                _dPosZ = 0;
                        }
                        if (InforManager.Instance.m_dScrewVaccumOffLevelZ > 0.0)
                        {
                            if (_mmceAxisZ.GetCurrentPos() >= InforManager.Instance.m_dScrewVaccumOffLevelZ)
                                VacuumOff();
                        }

                        int _idxScrew = m_pTrsJig[m_idx].m_iIndexPoint;

                        if (IsDriverFastenOK() && IsDry() == false)
                        {
                            if (InforManager.Instance.IsAngleControl)
                            {
                                #region Angle Control

                                if (OverAngleA2FirstCheck())
                                {
                                    DriverStop();
                                    MSystem.MoveStop(Axis.AXIS_Z1 + m_idx);
                                    //VacuumOff();
                                    // DriverStop();

                                    if (IsDry() == true)
                                    {
                                        SetStep(STEP_GUN_SCREW_SUCCESS);
                                        SetScrewResult(m_pTrsJig[m_idx].m_iIndexPoint, 1);
                                        IsScrewOK = true;
                                    }
                                    else
                                    {
                                        //  MSystem.vinterface.SendScrewNG(m_idx, m_pTrsJig[m_idx].m_iIndexPoint + 1); // add 250702



                                        //if (InforManager.Instance.ContinuesScrew)
                                        //{
                                        //    //SetStep(STEP_GUN_SCREW + 600);
                                        //    SetStep(STEP_TRASH_SCREW);
                                        //    SetScrewResult(m_pTrsJig[m_idx].m_iIndexPoint, 2);
                                        //    IsScrewOK = false;
                                        //    _needScrewDone = true;
                                        //    m_bLastResultScrewNG = true;

                                        //}
                                        //else
                                        {
                                            _needScrewDone = true;
                                            SetStep(STEP_TRASH_SCREW);
                                            SetScrewResult(m_pTrsJig[m_idx].m_iIndexPoint, 2);
                                            IsScrewOK = false;
                                            // ************* torque data save *************/
                                            // temp = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3200, 14);

                                            InforProduct.Instance.SaveTorqueValue(m_idx, m_pTrsJig[m_idx].m_iIndexPoint, MSystem.m_pTrsScrew[m_idx]._LastTorque);
                                            // ********************************************/
                                        }
                                        // THÊM LOG CHI TIẾT VÍT THẤT BẠI

                                        //ResetFasteningState();
                                        LogScrew(false, "NG over A2 First Check");
                                        LogAllHantasData(true, "NG Over A2 First Check");
                                    }
                                    break;
                                }
                                #endregion

                            }
                            successLockTime = DateTime.Now;
                            DriverStop();
                            MSystem.MoveStop(Axis.AXIS_Z1 + m_idx);

                            if (InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].m_iIndexPoint].Retry == true)
                            {
                                if (_CurrentReFasteningCount < InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].m_iIndexPoint].FasNumbers)
                                {
                                    SetStep(STEP_GUN_SCREW_REFASTENING);
                                    break;
                                }
                            }
                            double x = _mmceAxisX.GetCurrentPos();
                            double y = _mmceAxisY.GetCurrentPos();
                            double z = _mmceAxisZ.GetActualPos();

                            string PointLog = $"LogScrew_{DateTime.Now:yyyyMMdd}- {DateTime.Now:HHmmss.fff} | P{_idxScrew + 1} | X : {x:F2} | Y : {y:F2} | Z :{z:F2}";

                            if (m_idx == Constants.left)
                                m_pLogSave.AddTail(LogIndex.eLogScrew1, PointLog);
                            else
                                m_pLogSave.AddTail(LogIndex.eLogScrew2, PointLog);

                            MyMessagerBottom($"Screw{m_idx + 1}-{_idxScrew + 1}:Success detected at {successLockTime:HH:mm:ss.fff}");
                            LogAllHantasData(true, "SCREW_SUCCESS");// Log Hantas First OK

                            SetStep(STEP_GUN_SCREW_SUCCESS);
                            break;
                        }

                        if (IsDry() == true && IsMoveComplete(Axis.AXIS_Z1 + m_idx, _dPosZ, 0.1) == true)
                        {
                            MSystem.MoveStop(Axis.AXIS_Z1 + m_idx);
                            DriverStop();

                            SetStep(STEP_GUN_SCREW_SUCCESS);
                            SetScrewResult(m_pTrsJig[m_idx].m_iIndexPoint, 1);
                            IsScrewOK = true;
                            break;
                        }

                        if (IsOverTime(InforManager.Instance.ScrewOverTime) == true)
                        {
                            DriverStop();
                            MSystem.MoveStop(Axis.AXIS_Z1 + m_idx);
                            if (InforManager.Instance.IsScrewNgVaccumOff == true) // 260401 cnz
                                VacuumOff();
                            // DriverStop();
                            realtimeData = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3300, 13);
                            if (IsDry() == true)
                            {
                                SetStep(STEP_GUN_SCREW_SUCCESS);
                                SetScrewResult(m_pTrsJig[m_idx].m_iIndexPoint, 1);
                                IsScrewOK = true;
                            }
                            else
                            {
                                VacuumOn(); // add 260514 cnz
                                _needScrewDone = true;
                                SetStep(STEP_TRASH_SCREW);
                                SetScrewResult(m_pTrsJig[m_idx].m_iIndexPoint, 2);
                                IsScrewOK = false;
                                // ************* torque data save *************/
                                // temp = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3200, 14);

                                InforProduct.Instance.SaveTorqueValue(m_idx, m_pTrsJig[m_idx].m_iIndexPoint, MSystem.m_pTrsScrew[m_idx]._LastTorque);
                                // ********************************************/
                                // THÊM LOG CHI TIẾT VÍT THẤT BẠI
                                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.IDLE;
                                //ResetFasteningState();
                                LogScrew(false, "NG");
                                LogAllHantasData(false, "SCREW_TIMEOUT");
                            }
                        }
                    }
                    break;

                case STEP_GUN_SCREW_REFASTENING:
                    {
                        if (IsDriverReady())
                        {
                            _CurrentReFasteningCount++;
                            DriverRun(InforTeaching.Instance.P_Screw[m_idx][targetIndex].Channel);

                            SetStep(STEP_GUN_SCREW + 500);
                        }
                        break;
                    }
                case STEP_GUN_SCREW_SUCCESS:
                    {
                        int _idxScrew = m_pTrsJig[m_idx].m_iIndexPoint;
                        /****************************************************************************************/
                        /*****************************************************************************************/
                        IsScrewOK = true;
                        SetScrewResult(m_pTrsJig[m_idx].m_iIndexPoint, 1);
                        VacuumOff();
                        DriverStop();
                        MSystem.MoveStop(Axis.AXIS_Z1 + m_idx);


                        //  temp = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3200, 14);


                       // InforProduct.Instance.SaveTorqueValue(m_idx, m_pTrsJig[m_idx].m_iIndexPoint, MSystem.m_pTrsScrew[m_idx]._LastTorque);

                        //THÊM LOG CHI TIẾT VÍT THÀNH CÔNG

                        LogScrew(true, "OK");
                        ResetFasteningState();
                        SetStep(STEP_GUN_SCREW + 600);
                        MSystem.MoveStop(Axis.AXIS_Z1 + m_idx);

                        m_iTargetScrewPoint = m_iCurrentScrewPoint;

                    }
                    break;


                case STEP_GUN_SCREW + 600:
                    if (m_pTrsJig[m_idx].m_iIndexPoint >= InforTeaching.Instance.P_Screw[m_idx].Count)
                    {
                        _isInProcess = false;
                        count_t++;
                        m_pTrsJig[m_idx].m_iIndexPoint = 0;
                        VacuumOff();
                        m_pTrsJig[m_idx]._Infor = _Infor;
                        _Infor = new UNITINFOR();
                        m_bIsCheckSleverDone = false;
                        //    m_pTrsJig[m_idx].m_bCallScrew = false;
                        m_bMovedoneBarcode = false;
                    }
                    if (m_pTrsJig[m_idx].m_iIndexPoint >= InforTeaching.Instance.P_Screw[m_idx].Count - 1)
                    {
                        _isInProcess = false;
                        //    m_bMovedoneBarcode = false;
                        swScriewFlowTime.Stop();
                        VacuumOff();

                        // 260526 cnz : Barcode Use Step Modify
                        if (InforManager.Instance.IsBarcodeUse == true)
                        {
                            SetStep(STEP_BARCODE);
                        }
                        else if(InforManager.Instance.AutoVisionAlignMode != 0)
                        {
                            MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.IDLE;
                            SetStep(STEP_AUTO_ALIGE);
                        }
                        else
                        {
                            SetStep(STEP_PICKUP_SCREW);
                        }
                    }
                    else
                    {
                        SetStep(STEP_PICKUP_SCREW);
                    }
                    _needScrewDone = true;
                    // ************* torque data save *************/
                    InforProduct.Instance.SaveTorqueValue(m_idx, m_pTrsJig[m_idx].m_iIndexPoint, MSystem.m_pTrsScrew[m_idx]._LastTorque);
                    // ********************************************/
                    break;
                #endregion

                #region TRASH SCREW
                case STEP_TRASH_SCREW:
                    //********************* 
                    if (m_bOnPickupError)
                        break;
                    //*********************
                    if (_isFirstMoveZ == true)
                    {
                        _isFirstMoveZ = false;
                        MoveReadyPosZ();
                    }
                    /*
                    //if (IsSafetyZ() == true)
                    //{
                    //    if (_needScrewDone == true)
                    //    {
                    //        _needScrewDone = false;
                    //        IsScrewDone = true;
                    //    }

                    //    if (_isFirstMoveX == true)
                    //    {
                    //        _isFirstMoveX = false;
                    //        _distX = Math.Abs(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].X - _mmceAxisX.GetActualPos());
                    //        MoveTrashPosX();
                    //    }
                    //}
                    */
                    if (IsMoveDoneReadyPosZ() == true)
                    {
                        _isFirstMoveZ = true;
                        _isFirstMoveX = true;
                        SetStep(STEP_TRASH_SCREW + 10);
                        break;
                    }
                    break;

                case STEP_TRASH_SCREW + 10:
                    if (_isFirstMoveX == true)
                    {
                        _isFirstMoveX = false;
                        _distX = Math.Abs(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].X - _mmceAxisX.GetActualPos());
                        MoveTrashPosX();
                    }
                    /*
                    //_targetZ = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].Z;
                    //_safetyZ = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z;
                    //if (IsSafetyZDown(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].X) == true)
                    //{
                    //    if (_isFirstMoveZ == true)
                    //    {
                    //        _isFirstMoveZ = false;
                    //        MoveTrashPosZ();
                    //    }
                    //}
                    */
                    if (IsMoveDoneTrashPosX() == true)
                    {
                        _isFirstMoveX = true;
                        SetStep(STEP_TRASH_SCREW + 20);
                    }
                     break;

                case STEP_TRASH_SCREW + 20:
                    if (_isFirstMoveZ == true)
                    {
                        _isFirstMoveZ = false;
                        MoveTrashPosZ();
                    }

                    if (IsMoveDoneTrashPosZ() == true)
                    {
                        _isFirstMoveZ = true;
                        SetStep(STEP_TRASH_SCREW + 100);
                    }
                    break;

                case STEP_TRASH_SCREW + 100:
                    {
                        BlowOn();

                        if (IsOverTime(InforManager.Instance.BlowTime) == true)
                        {
                            BlowOff();
                            //if (InforManager.Instance.IsBarcodeUse == true && !MSystem._Rework[m_idx])
                            //{
                            //    _isFirstMoveZ = true;
                            //    SetStep(STEP_BARCODE);
                            //    break;
                            //}
                            if (InforManager.Instance.AutoVisionAlignMode != 0
                                && !MSystem._Rework[m_idx]) // add 250705 jlyoon
                            {
                                //if (InforManager.Instance.IsScrewPickupReady == _USE_)
                                //{
                                //    SetStep(STEP_PICKUP_SCREW);
                                //}
                                //else
                                //    SetStep(STEP_AUTO_ALIGE);
                                if (MSystem.vinterface.m_VisionResult[m_idx] != VS_RESULT.PASS
                                    && MSystem.vinterface.m_VisionResult[m_idx] != VS_RESULT.FAIL)
                                    SetStep(STEP_AUTO_ALIGE);
                                else
                                    SetStep(STEP_PICKUP_SCREW);
                            }
                            else
                            {
                                SetStep(STEP_PICKUP_SCREW);
                            }
                        }
                    }
                    break;

                #endregion
                #region STEP BARCODE
                case STEP_BARCODE:
                    {
                        if (_isFirstMoveZ == true)
                        {
                            _isFirstMoveZ = false;
                            MoveReadyPosZ();
                        }

                        if (IsMoveDoneReadyPosZ() == true)
                        {
                            IsScrewDone = true;

                            _isFirstMoveX = true;
                            SetStep(STEP_BARCODE + 10);
                            break;
                        }
                    }
                    break;
                case STEP_BARCODE + 10:
                    {
                        if (_isFirstMoveX == true)
                        {
                            _isFirstMoveX = false;
                            _distX = Math.Abs(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_BARCODE].X - _mmceAxisX.GetActualPos());
                            MoveBarcodePosX();
                        }

                        if (IsMoveDoneBarcodePosX() == true)
                        {
                            _isFirstMoveZ = true;
                            SetStep(STEP_BARCODE + 20);
                        }
                    }
                    break;
                case STEP_BARCODE + 20:
                    if (m_bBcrResult == BCR_RESULT.START)
                    {
                        if (_isFirstMoveZ == true)
                        {
                            _isFirstMoveZ = false;
                            MoveBarcodePosZ();
                        }

                        if (IsMoveDoneBarcodePosZ() == true)
                        {
                            IsScrewDone = false;
                            m_bMovedoneBarcode = true;
                            SetStep(STEP_BARCODE + 100);
                        }
                    }
                    break;

                case STEP_BARCODE + 100:
                    if (ReadBarcode() == true)
                    {
                        m_bReqScanBcr = false;
                        _isFirstMoveX = true;
                        _isFirstMoveZ = true;
                        m_bMovedoneBarcode = false;

                        if (InforManager.Instance.AutoVisionAlignMode != 0 && !MSystem._Rework[m_idx]) // add 250705 jlyoon
                        {
                            if (MSystem.vinterface.m_VisionResult[m_idx] != VS_RESULT.PASS || MSystem.vinterface.m_VisionResult[m_idx] != VS_RESULT.FAIL)
                                SetStep(STEP_AUTO_ALIGE);
                            else
                            {
                                if (IsVacuumSensorOn())
                                    SetStep(STEP_TRASH_SCREW);
                                else
                                    SetStep(STEP_PICKUP_SCREW);
                            }
                        }
                        else
                        {
                            if (IsVacuumSensorOn())
                                SetStep(STEP_TRASH_SCREW);
                            else
                                SetStep(STEP_PICKUP_SCREW);
                        }
                    }
                    else
                    {
                        m_bReqScanBcr = false;
                        m_bMovedoneBarcode = false;
                        MoveReadyPosZ();
                        m_pTrsScrew[m_idx].IsScrewOK = false;
                        IsScrewDone = true;
                        if (m_idx == 0)
                            InforProduct.Instance.ProductBCR_Fail_L += 1;
                        else
                            InforProduct.Instance.ProductBCR_Fail_R += 1;
                        InforProduct.Instance.SaveSettings();
                        InforTeaching.Instance.SaveSettings();//Delete Save Setting
                        SetStep(STEP_BARCODE + 400);
                    }
                    break;
                case STEP_BARCODE + 400: // READ BCR FAIL
                    if (m_bBcrResult == BCR_RESULT.FAIL || IsOverTime(5.0))
                    {
                        MoveReadyPosZ();
                        m_pTrsScrew[m_idx].IsScrewOK = false;
                        IsScrewDone = false;
                        SetUnitInitialize();
                        SetStep(STEP_INIT);
                    }
                    break;

                #endregion

                default:
                    break;
            }
        }
        #endregion
        /******************************************************/

        #region IF Function
        public void DriverRun(int _CH)
        {
            switch (m_idx)
            {
                case (int)_NSC.eLEFT:
                    {
                        if (_CH == 1)
                        {
                            m_pDIO.OutPutOn(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_1"]);
                            m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_2"]);
                            m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_3"]);
                            //Thread.Sleep(20);
                        }
                        else if (_CH == 2)
                        {
                            m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_1"]);
                            m_pDIO.OutPutOn(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_2"]);
                            m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_3"]);
                            // Thread.Sleep(20);
                        }
                        else if (_CH == 3)
                        {
                            m_pDIO.OutPutOn(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_1"]);
                            m_pDIO.OutPutOn(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_2"]);
                            m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_3"]);
                            //Thread.Sleep(20);
                        }
                        m_pDIO.OutPutOn(IOMap.OUT["OUT_LEFT_SCREW_DRV_START"]);
                        m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_SCREW_DRV_FASTEN_LOOSEN"]);
                    }
                    break;
                case (int)_NSC.eRIGHT:
                    {
                        if (_CH == 1)
                        {
                            m_pDIO.OutPutOn(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_1"]);
                            m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_2"]);
                            m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_3"]);
                            // Thread.Sleep(20);
                        }
                        else if (_CH == 2)
                        {
                            m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_1"]);
                            m_pDIO.OutPutOn(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_2"]);
                            m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_3"]);
                            // Thread.Sleep(20);
                        }
                        else if (_CH == 3)
                        {
                            m_pDIO.OutPutOn(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_1"]);
                            m_pDIO.OutPutOn(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_2"]);
                            m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_3"]);
                            //Thread.Sleep(20);
                        }
                        m_pDIO.OutPutOn(IOMap.OUT["OUT_RIGHT_SCREW_DRV_START"]);
                        m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_SCREW_DRV_FASTEN_LOOSEN"]);
                    }
                    break;
                default: break;
            }
        }

        public void DriverStop()
        {
            switch (m_idx)
            {
                case Constants.left:
                    m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_SCREW_DRV_START"]);

                    m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_SCREW_DRV_FASTEN_LOOSEN"]);

                    m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_1"]);
                    m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_2"]);
                    m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_DRV_TORQUE_SELECT_3"]);
                    break;

                case Constants.right:
                    m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_SCREW_DRV_START"]);

                    m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_1"]);
                    m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_2"]);
                    m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_DRV_TORQUE_SELECT_3"]);
                    m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_SCREW_DRV_FASTEN_LOOSEN"]);
                    break;

                default:
                    break;
            }
        }

        public bool IsDriverRun()
        {
            switch (m_idx)
            {
                case (int)_NSC.eLEFT:
                    return m_pDIO.IsOn(IOMap.IN["IN_LEFT_SCREW_DRV_MOTOR_RUN"]);
                case (int)_NSC.eRIGHT:
                    return m_pDIO.IsOn(IOMap.IN["IN_RIGHT_SCREW_DRV_MOTOR_RUN"]);
                default: return false;
            }
        }
        public bool IsDriverRunOut()
        {
            switch (m_idx)
            {
                case Constants.left:
                    return m_pDIO.IsOn(IOMap.OUT["OUT_LEFT_SCREW_DRV_START"]);

                case Constants.right:
                    return m_pDIO.IsOn(IOMap.OUT["OUT_RIGHT_SCREW_DRV_START"]);

                default:
                    return false;
            }
        }
        public void DriverResetAlarm(bool _ONOFF)
        {
            switch (m_idx)
            {
                case (int)_NSC.eLEFT:
                    if (_ONOFF)
                        m_pDIO.OutPutOn(IOMap.OUT["OUT_LEFT_SCREW_DRV_ALARM_RESET"]);
                    else
                        m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_SCREW_DRV_ALARM_RESET"]);
                    break;
                case (int)_NSC.eRIGHT:
                    if (_ONOFF)
                        m_pDIO.OutPutOn(IOMap.OUT["OUT_RIGHT_SCREW_DRV_ALARM_RESET"]);
                    else
                        m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_SCREW_DRV_ALARM_RESET"]);
                    break;
                default: break;
            }
        }
        public bool IsDriverResetAlarm()
        {
            switch (m_idx)
            {
                case (int)_NSC.eLEFT:
                    return m_pDIO.IsOn(IOMap.OUT["OUT_LEFT_SCREW_DRV_ALARM_RESET"]);
                case (int)_NSC.eRIGHT:
                    return m_pDIO.IsOn(IOMap.OUT["OUT_RIGHT_SCREW_DRV_ALARM_RESET"]);
                default: return false;
            }
        }
        public void DriverFastenLoosen(bool _ONOFF)
        {
            switch (m_idx)
            {
                case (int)_NSC.eLEFT:
                    if (_ONOFF)
                        m_pDIO.OutPutOn(IOMap.OUT["OUT_LEFT_SCREW_DRV_FASTEN_LOOSEN"]);
                    else
                        m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_SCREW_DRV_FASTEN_LOOSEN"]);
                    break;
                case (int)_NSC.eRIGHT:
                    if (_ONOFF)
                        m_pDIO.OutPutOn(IOMap.OUT["OUT_RIGHT_SCREW_DRV_FASTEN_LOOSEN"]);
                    else
                        m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_SCREW_DRV_FASTEN_LOOSEN"]);
                    break;
                default: break;
            }
        }
        public bool IsDriverFastenLoosen()
        {
            switch (m_idx)
            {
                case (int)_NSC.eLEFT:
                    return m_pDIO.IsOn(IOMap.OUT["OUT_LEFT_SCREW_DRV_FASTEN_LOOSEN"]);
                case (int)_NSC.eRIGHT:
                    return m_pDIO.IsOn(IOMap.OUT["OUT_RIGHT_SCREW_DRV_FASTEN_LOOSEN"]);
                default: return false;
            }
        }
        #endregion

        private void SetScrewResult(int index, int result)
        {
            // Index over flow prevent 251127 cnz
            if (index >= InforTeaching.Instance.P_Screw[m_idx].Count)
                index -= 1;
            InforTeaching.Instance.P_Screw[m_idx][index].status = result;
            InforTeaching.Instance.SaveSettings();//Test Delete Save Setting
        }

        #region Control IO 

        public void StartFeeder()//test new feeder
        {
            if (m_idx == 0)
                RunFeeder = IOMap.OUT["OUT_RUN_LEFT_FEEDER"];//IO.OUT_RUN_LEFT_FEEDER;
            else
                RunFeeder = IOMap.OUT["OUT_RUN_RIGHT_FEEDER"];//IO.OUT_RUN_RIGHT_FEEDER;
            if (m_pDIO.IsOff(RunFeeder))
            {
                m_pDIO.OutPutOn(RunFeeder);
                FeederVaccum(true);
            }
        }
        public void StopFeeder()//test new feeder
        {
            if (m_idx == 0)
                RunFeeder = IOMap.OUT["OUT_RUN_LEFT_FEEDER"];//IO.OUT_RUN_LEFT_FEEDER;
            else
                RunFeeder = IOMap.OUT["OUT_RUN_RIGHT_FEEDER"];//IO.OUT_RUN_RIGHT_FEEDER;
            if (m_pDIO.IsOn(RunFeeder))
                m_pDIO.OutPutOff(RunFeeder);
        }
        public void FeederVaccum(bool isOnOff)//test new feeder
        {
            if (m_idx==0)
                FeederVaccumOn = IOMap.OUT["OUT_LEFT_FEEDER_VACCUM_ON"];//IO.OUT_RUN_LEFT_FEEDER;
            else
                FeederVaccumOn = IOMap.OUT["OUT_RIGHT_FEEDER_VACCUM_ON"];//IO.OUT_RUN_RIGHT_FEEDER;
            if (isOnOff)
            {
                if (m_pDIO.IsOff(FeederVaccumOn))
                    m_pDIO.OutPutOn(FeederVaccumOn);
            }
            else
            {
                if (m_pDIO.IsOn(FeederVaccumOn))
                    m_pDIO.OutPutOff(FeederVaccumOn);
            }
        }
        public void VacuumOn()
        {
            if (IsDry() == true && !MSystem.m_bIsTeachDlg)
                return;
            m_pDIO.OutPutOff(BlowOnOffSol);
            m_pDIO.OutPutOn(VacuumOnOffSol);
        }

        public void VacuumOff()
        {
            m_pDIO.OutPutOff(VacuumOnOffSol);
            m_pDIO.OutPutOff(BlowOnOffSol);
        }

        public bool IsVacuumSolOn()
        {
            return m_pDIO.IsOn(VacuumOnOffSol);
        }

        public bool IsVacuumSensorOn()
        {
            return m_pDIO.IsOn(VacuumSensor);
        }

        public void BlowOn()
        {
            if (IsDry() == true && !MSystem.m_bIsTeachDlg)
                return;

            m_pDIO.OutPutOff(VacuumOnOffSol);
            m_pDIO.OutPutOn(BlowOnOffSol);
        }

        public void BlowOff()
        {
            m_pDIO.OutPutOff(BlowOnOffSol);
            m_pDIO.OutPutOff(VacuumOnOffSol);
        }

        public bool IsBlowSolOn()
        {
            return m_pDIO.IsOn(BlowOnOffSol);
        }

        public bool IsDriverReady()
        {
            if (m_idx == (int)_NSC.eLEFT)
                return m_pDIO.IsOn(IOMap.IN["IN_LEFT_SCREW_DRV_READY"]);
            else if (m_idx == (int)_NSC.eRIGHT)
                return m_pDIO.IsOn(IOMap.IN["IN_RIGHT_SCREW_DRV_READY"]);
            return false;
        }

        public bool IsDriverFastenOK()
        {
            return m_pDIO.IsOn(DriverFastenOKSensor);
        }

        public bool IsFeederReady()
        {
            return m_pDIO.IsOn(FeederReadySensor);
        }

        public bool IsDriverAlarm()
        {
            if (m_idx == Constants.left)
                return m_pDIO.IsOn(IOMap.IN["IN_LEFT_SCREW_DRV_ALARM"]);
            else if (m_idx == Constants.right)
                return m_pDIO.IsOn(IOMap.IN["IN_RIGHT_SCREW_DRV_ALARM"]);
            return false;
        }
        #endregion


        #region Retry Method
        private void ResetFasteningState()
        {
            m_bScrewSuccessDetected = false;
            m_bReFasteningRequested = false;
            m_bReFasteningCompleted = false;
            _CurrentReFasteningCount = 1;
            successLockTime = DateTime.MinValue;
            reFasStartTime = DateTime.MinValue;
        }

        private bool ShouldPerformReFastening()
        {
            try
            {
                bool retryEnabled = InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].m_iIndexPoint].Retry;
                bool belowMaxCount = _CurrentReFasteningCount < InforTeaching.Instance.P_Screw[m_idx][m_pTrsJig[m_idx].m_iIndexPoint].FasNumbers;
                bool notCompleted = !m_bReFasteningCompleted;
                return retryEnabled && belowMaxCount && notCompleted;
            }
            catch (Exception ex)
            {
                MyMessagerBottom($"Error Checking ReFastening Condition: {ex.Message}");
                return false;
            }
        }



        #endregion


        #region SERVOR MOVE
        public int MoveReadyPosX()
        {
            return _mmceAxisX.StartMove(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_READY].X);
        }

        public bool IsMoveDoneReadyPosX()
        {
            return _mmceAxisX.IsReady() && _mmceAxisX.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_READY].X);
        }

        public bool IsReadyPosX()
        {
            return _mmceAxisX.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_READY].X);
        }
        public int MoveReadyPosZ()
        {
            return _mmceAxisZ.StartMove(0);
            //return _mmceAxisZ.StartMove(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_READY].Z);
        }

        public bool IsMoveDoneReadyPosZ()
        {
            return _mmceAxisZ.IsReady() && _mmceAxisZ.IsMoveDone(0);
            //return _mmceAxisZ.IsReady() && _mmceAxisZ.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_READY].Z);
        }

        public bool IsReadyPosZ()
        {
            return _mmceAxisZ.IsMoveDone(0);
            //return _mmceAxisZ.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_READY].Z);
        }

        public int MoveTrashPosX()
        {
            return _mmceAxisX.StartMove(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].X);
        }

        public bool IsMoveDoneTrashPosX()
        {
            return _mmceAxisX.IsReady() && _mmceAxisX.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].X);
        }

        public bool IsTrashPosX()
        {
            return _mmceAxisX.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].X);
        }



        public int MoveTrashPosZ()
        {
            double pos = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].Z;
            return _mmceAxisZ.StartMove(pos);
        }

        public bool IsMoveDoneTrashPosZ()
        {
            double pos = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].Z;
            return _mmceAxisZ.IsReady() && _mmceAxisZ.IsMoveDone(pos);
        }

        public bool IsTrashPosZ()
        {
            double pos = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_TRASH].Z;
            return _mmceAxisZ.IsMoveDone(pos);
        }

        public bool IsMoveDoneBarcodePosX()
        {
            return _mmceAxisX.IsReady() && _mmceAxisX.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_BARCODE].X);
        }


        public int MoveBarcodePosX()
        {
            return _mmceAxisX.StartMove(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_BARCODE].X);
        }

        public int MoveBarcodePosZ()
        {
            double pos = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_BARCODE].Z;
            return _mmceAxisZ.StartMove(pos);
        }

        public bool IsMoveDoneBarcodePosZ()
        {
            double pos = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_BARCODE].Z;
            return _mmceAxisZ.IsReady() && _mmceAxisZ.IsMoveDone(pos);
        }
        public int MovePickUpPosX()
        {
            return _mmceAxisX.StartMove(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_PICKUP].X);
        }

        public bool IsMoveDonePickUpPosX()
        {
            return _mmceAxisX.IsReady() && _mmceAxisX.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_PICKUP].X);
        }

        public bool IsPickUpPosX()
        {
            return _mmceAxisX.IsMoveDone(InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_PICKUP].X);
        }

        public int MovePickUpPosZ()
        {
            double pos = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_PICKUP].Z;
            if (IsDry() == true)
                pos -= 5;
            else
                pos += InforManager.Instance.m_dPickupOffset;
            return _mmceAxisZ.StartMove(pos);
        }

        public bool IsMoveDonePickUpPosZ()
        {
            double pos = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_PICKUP].Z;
            if (IsDry() == true)
                pos -= 5;
            else
                pos += InforManager.Instance.m_dPickupOffset;

            return _mmceAxisZ.IsReady() && _mmceAxisZ.IsMoveDone(pos);
        }

        public bool IsPickUpPosZ()
        {
            double pos = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_PICKUP].Z;
            if (IsDry() == true)
                pos -= 10;
            else
                pos += InforManager.Instance.m_dPickupOffset;

            return _mmceAxisZ.IsMoveDone(pos);
        }

        public int MoveScrewPosX(int index)
        {
            try
            {
                if (InforManager.Instance.AutoVisionAlignMode != 0)
                    return _mmceAxisX.StartMove(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.X + m_dVisionOffsetX[index]);
                return _mmceAxisX.StartMove(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.X);
            }
            catch (Exception)
            {
                Thread.Sleep(10000);
                return -1;
            }
        }

        public bool IsMoveDoneScrewPosX(int index)
        {
            if (InforManager.Instance.AutoVisionAlignMode != 0)
                return _mmceAxisX.IsReady() && _mmceAxisX.IsMoveDone(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.X + m_dVisionOffsetX[index]);
            return _mmceAxisX.IsReady() && _mmceAxisX.IsMoveDone(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.X);
        }

        public bool IsScrewPosX(int index)
        {
            return _mmceAxisX.IsMoveDone(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.X);
        }

        public int MoveScrewReadyPosZ(int index)
        {
            try
            {
                double pos = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z - InforManager.Instance.m_dScrewOffset;
                if (pos < 0) pos = 0;
                return _mmceAxisZ.StartMove(pos);
            }
            catch (Exception)
            {
                Thread.Sleep(10000);
                return -1;
            }
        }

        public bool IsMoveDoneScrewReadyPosZ(int index)
        {
            double pos = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z - InforManager.Instance.m_dScrewOffset;
            if (pos < 0) pos = 0;
            return _mmceAxisZ.IsReady() && _mmceAxisZ.IsMoveDone(pos);
        }

        public bool IsScrewReadyPosZ(int index)
        {
            double pos = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z - InforManager.Instance.m_dScrewOffset;
            if (pos < 0) pos = 0;
            return _mmceAxisZ.IsMoveDone(pos);
        }

        public int MoveScrewPosZ(int index)
        {
            try
            {
                return _mmceAxisZ.StartMove(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z);
            }
            catch (Exception)
            {
                Thread.Sleep(10000);
                return -1;
            }
        }

        public bool IsMoveDoneScrewPosZ(int index)
        {
            return _mmceAxisZ.IsReady() && _mmceAxisZ.IsMoveDone(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z);
        }

        public bool IsScrewPosZ(int index)
        {
            return _mmceAxisZ.IsMoveDone(InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z);
        }

        public bool IsSafetyZ()
        {
            double actualPos = _mmceAxisZ.GetActualPos();
            double safetyPos = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)PScrewMain.ePOS_Z_SAFETY].Z;
            if (safetyPos <= 1)
                safetyPos = 1;
            if (actualPos < safetyPos)
                return true;

            // todo : 미리 이동하도록.. 지금은 좀 느림
            //safetyPos = CalculateZUpDist()
            //

            return false;
        }

        public bool IsSafetyZDown(double targetX)
        {
            double candowndist = GetZDownDist(_distX, _targetZ, _safetyZ);
            double actualPos = _mmceAxisX.GetActualPos();
            double remainPos = Math.Abs(targetX - actualPos);
            if (remainPos < candowndist)
                return true;

            return false;
        }

        /******************************************************/
        /******************************************************/
        public bool IsMoveCompleteZ(PScrewMain _pos)
        {
            double dZ = InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)_pos].Z;
            return IsMoveComplete(Axis.AXIS_Z1 + (int)m_idx, dZ); //InforTeaching.Instance.P_MainScrew[(int)m_idx, (int)_pos].Z);
        }

        /******************************************************/
        public bool IsMoveCompleteLineXY(PointScrew _Pscrew)
        {
            return IsMoveComplete(Axis.AXIS_X1 + m_idx, _Pscrew.PScrew.X, 0.1) && IsMoveComplete(Axis.AXIS_Y1 + m_idx, _Pscrew.PScrew.Y, 0.1);
        }
        public int MovePosition(Axis iAxis, double dPos)
        {
            return SetMove((int)iAxis, dPos);
        }

        public int MovePosition(Axis iAxis, double dPos, double _speed)
        {
            return SetMove((int)iAxis, dPos, _speed);
        }

        public bool IsMoveComplete(Axis iAxis, double dPos)
        {
            return IsAxisPositionCheck((int)iAxis, dPos);
        }

        public bool IsMoveComplete(Axis iAxis, double dPos, double _tolerance)
        {
            return IsAxisPositionCheck((int)iAxis, dPos, _tolerance);
        }

        public bool IsReadyZ()
        {
            return IsMoveCompleteZ(PScrewMain.ePOS_READY);
        }
        public int MoveVisionPosY(int index)
        {
            try
            {
                double y = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y;
                y += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                y -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                return _mmceAxisY.StartMove(y);
            }
            catch (Exception)
            {
                Thread.Sleep(10000);
                return -1;
            }
        }

        public bool IsMoveDoneVisionPosY(int index)
        {
            double y = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Y;
            y += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
            y -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
            return _mmceAxisY.IsReady() && _mmceAxisY.IsMoveDone(y);
        }
        public int MoveVisionPosZ(int index)
        {
            try
            {
                double z = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z;
                z += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Z;
                z -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Z;
                return _mmceAxisZ.StartMove(z);
            }
            catch (Exception)
            {
                Thread.Sleep(10000);
                return -1;
            }
        }

        public bool IsMoveDoneVisionPosZ(int index)
        {
            double z = InforTeaching.Instance.P_Screw[m_idx][index].PScrew.Z;
            z += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Z;
            z -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Z;
            return _mmceAxisZ.IsReady() && _mmceAxisY.IsMoveDone(z);
        }
        #endregion

        private void CheckLimitSensor()
        {
            if (_mmceAxisX.IsPlusLimit() == true)
                SetError(0);

            if (_mmceAxisX.IsMinusLimit() == true)
                SetError(1);

            if (IsAxisError(_axisX) == true)
                SetError(2);

            if (IsAxisError(_axisZ) == true)
                SetError(3);
        }

        private double _distZ;
        private double _safetyUpZ;
        private double CalculateZUpDist(double targetZ, double totalZ)
        {
            double candowndist = GetZDownDist(50, totalZ, targetZ);
            double ZTimeTest = GetZTimeToTarget(totalZ, targetZ);
            double XTimeTest = GetXTime(candowndist);

            double distX = 10;
            double XTime = GetXTime(distX);
            XTime = GetXTime(distX * 2) / 2;

            double vel = ServoParam.Instance.m_sAxisInfo[(int)_axisZ].dAxisMaxSpeed; //_mmceAxisZ.m_sAxisInfo.dAxisMaxSpeed;
            double accTime = ServoParam.Instance.m_sAxisInfo[(int)_axisZ].dAxisAcc / 1000.0;  //_mmceAxisZ.m_sAxisInfo.dAxisAcc / 1000.0;

            double ZTime = GetZTime(totalZ);
            double ZTargetTime = ZTime - GetZTimeToTarget(totalZ, targetZ);
            double ZHalfTime = ZTime / 2;

            double z1 = 0.0;
            double z2 = 0.0;
            double velZ1Start, velZ1End;
            double velZ2Start, velZ2End;
            double maxVel = ZHalfTime * vel / accTime;
            double ZStartTime;
            if (ZTargetTime > ZHalfTime)
            {
                ZStartTime = ZTargetTime - XTime;
                if (ZStartTime > ZHalfTime)
                {
                    velZ2Start = (ZTime - ZStartTime) * maxVel / ZHalfTime;
                    velZ2End = (ZTime - ZTargetTime) * maxVel / ZHalfTime;
                    z2 = (ZTargetTime - ZStartTime) * (velZ2Start + velZ2End) / 2;
                }
                else
                {
                    velZ1Start = maxVel * ZStartTime / ZHalfTime;
                    velZ1End = maxVel;
                    velZ2Start = maxVel;
                    velZ2End = (ZTime - ZTargetTime) * maxVel / ZHalfTime;
                    z1 = (ZHalfTime - ZStartTime) * (velZ1Start + maxVel) / 2;
                    z2 = (ZTargetTime - ZHalfTime) * (velZ2Start + velZ2End) / 2;
                }
            }
            else
            {
                ZStartTime = ZTargetTime - XTime;
                velZ1Start = maxVel * ZStartTime / ZHalfTime;
                velZ1End = maxVel * ZTargetTime / ZHalfTime;
                z1 = XTime * (velZ1Start + velZ1End) / 2;
            }

            double test = z1 + z2;

            return test;
        }

        private double _distX;
        private double _targetZ;
        private double _safetyZ;
        private double GetZDownDist(double distanceX, double totalZ, double targetZ)
        {
            double ZTime = GetZTimeToTarget(totalZ, targetZ);
            double vel = ServoParam.Instance.m_sAxisInfo[(int)_axisX].dAxisMaxSpeed; //_mmceAxisZ.m_sAxisInfo.dAxisMaxSpeed;
            double accTime = ServoParam.Instance.m_sAxisInfo[(int)_axisX].dAxisAcc / 1000.0;  //_mmceAxisZ.m_sAxisInfo.dAxisAcc / 1000.0;

            double XTime = GetXTime(distanceX);
            double refX = vel * accTime;
            if (distanceX > refX)
            {
                if (ZTime > XTime)
                {
                    return distanceX;
                }
                else if (ZTime > XTime - accTime)
                {
                    return distanceX;
                }
                else if (ZTime < accTime)
                {
                    double temp = 0.5 * vel * accTime;
                    return temp * Math.Pow(ZTime / accTime, 2);
                }
                else
                {
                    double temp = 0.5 * vel * accTime;
                    return temp * Math.Pow(ZTime / accTime, 2) + (ZTime - accTime) * vel;
                }
            }
            else
            {
                if (ZTime > XTime)
                {
                    return distanceX;
                }
                else if (ZTime < XTime / 2)
                {
                    return 0.5 * distanceX * Math.Pow(ZTime / accTime, 2);
                }
                else
                {
                    return 0.5 * distanceX + 0.5 * 0.5 * distanceX * Math.Pow(ZTime / accTime, 2);
                }
            }
        }

        private double GetXTime(double totalX)
        {
            double vel = ServoParam.Instance.m_sAxisInfo[(int)_axisX].dAxisMaxSpeed; //_mmceAxisZ.m_sAxisInfo.dAxisMaxSpeed;
            double accTime = ServoParam.Instance.m_sAxisInfo[(int)_axisX].dAxisAcc / 1000.0;  //_mmceAxisZ.m_sAxisInfo.dAxisAcc / 1000.0;

            double refX = vel * accTime;
            if (totalX > refX)
            {
                return accTime * 2 + ((totalX - refX) / vel);
            }
            else
            {
                return 2 * accTime * Math.Sqrt(totalX / refX);
            }
        }

        private double GetZTimeToTarget(double totalZ, double targetZ)
        {
            double vel = ServoParam.Instance.m_sAxisInfo[(int)_axisZ].dAxisMaxSpeed; //_mmceAxisZ.m_sAxisInfo.dAxisMaxSpeed;
            double accTime = ServoParam.Instance.m_sAxisInfo[(int)_axisZ].dAxisAcc / 1000.0;  //_mmceAxisZ.m_sAxisInfo.dAxisAcc / 1000.0;

            double totalZTime = GetZTime(totalZ);
            double refZ = vel * accTime;
            if (totalZ > refZ)
            {
                return 0;
            }
            else
            {
                if (targetZ > totalZ)
                {
                    return totalZTime;
                }
                else if (targetZ < totalZ / 2)
                {
                    double temp = targetZ / (totalZ / 2); //(totalZ / 2) - targetZ;
                    return 0.5 * totalZTime * Math.Sqrt(temp);
                }
                else
                {
                    double temp = (targetZ - (totalZ / 2)) / (totalZ / 2);
                    return 0.5 * totalZTime + 0.5 * totalZTime * Math.Sqrt(temp);
                }
            }
        }

        private double GetZTime(double totalZ)
        {
            double vel = ServoParam.Instance.m_sAxisInfo[(int)_axisZ].dAxisMaxSpeed; //_mmceAxisZ.m_sAxisInfo.dAxisMaxSpeed;
            double accTime = ServoParam.Instance.m_sAxisInfo[(int)_axisZ].dAxisAcc / 1000.0;  //_mmceAxisZ.m_sAxisInfo.dAxisAcc / 1000.0;

            double refZ = vel * accTime;
            if (totalZ > refZ)
            {
                return accTime * 2 + ((totalZ - refZ) / vel);
            }
            else
            {
                return 2 * accTime * Math.Sqrt(totalZ / refZ);
            }
        }


        public void IniErrorCodeRead(int errorcode, ref string message)
        {
            string path = $@"{Program.Directory}\Data\";
            DataReadWrite m_pDataSys = new DataReadWrite(path, $"ErrorMessage.ini");
            string key = "messager";
            string section = $"400{errorcode}";
            string data = "Empty";

            m_pDataSys.ReadStringAnsi(section, key, ref data, "");
            message += data;
            message += "\r\n";
            key += 2;
            m_pDataSys.ReadStringAnsi(section, key, ref data, "Empty");
            message += data;
        }
        private bool Graph_DataReceived()
        {
            MSystem.m_pHantas[m_idx].getTorpueGraphData(0);
            byte[] buffer = MSystem.m_pHantas[m_idx].SerialPort_DataReceived();

            if (buffer == null || buffer.Length < 62)
            {
                return false;
            }
            int packetLen = buffer[2]; // 패킷 길이
            int frameCount = buffer[3]; // 총 프레임 카운트
            int frameNumber = buffer[4];
            MSystem.m_pHantas[m_idx].GraphData.Clear();

            // 첫번째 프레임은 35번지부터 2byte씩 graph data가 들어옴
            if (frameNumber == 1)
            {
                for (int i = 35; i < packetLen - 2; i++)
                {
                    MSystem.m_pHantas[m_idx].GraphData.Add(buffer[i]);
                }
            }

            for (int i = 1; i < frameCount; i++)
            {
                MSystem.m_pHantas[m_idx].getTorpueGraphData(i);
                // 두번째 프레임부터는 5번지부터 2byte씩 graph data가 들어옴
                buffer = MSystem.m_pHantas[m_idx].SerialPort_DataReceived();
                if (buffer == null)
                {
                    return false;
                }
                packetLen = buffer[2];
                for (int j = 0; j < packetLen - 2; j++)
                {
                    MSystem.m_pHantas[m_idx].GraphData.Add(buffer[j + 5]);
                }
            }
            return true;
        }

        public bool VisionAlign(bool mode = true) //Insert khjo
        {
            // 상태 변경 삭제 260526 cnz
            //MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.READY;
            if (InforManager.Instance.m_bVisionType == false) // Open CV
            {
                if (MSystem.vinterface.Server[0].IsClient_Connected == false)
                {
                    MSystem.MyMsgMemo("Vision Program is Disconnected.", "Alarm", msgButton.OK, msgIcon.Error);
                    MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                    return false;
                }
            }
            else // Library
            {
                if ((InforManager.Instance.IsLeftUnitUse == true && MSystem.vinterface.Server[0].IsClient_Connected == false) 
                    || (InforManager.Instance.IsRightUnitUse == true && MSystem.vinterface.Server[1].IsClient_Connected == false))
                {
                    MSystem.MyMsgMemo("Vision Program is Disconnected.", "Alarm", msgButton.OK, msgIcon.Error);
                    MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                    return false;
                }
            }

                TimerDelay TimCheck = new TimerDelay();
            // CHECK SAFETY
            #region
            if (MSystem.IsDetectEmergency())
            {
                MSystem.MyMsgMemo("EMG Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                return false;
            }
            if (!MSystem.IsOriginAll())
            {
                MSystem.MyMsgMemo("Please Origin All Servo before run Program", "Error", msgButton.OK, msgIcon.Error);
                return false;
            }
            #endregion
            double _dx = 0.00;
            double _dy = 0.00;
            double _dz = 0.00;
            bool LastPos = false;

            // Move Z UP
            if (MSystem.SetMove((int)Axis.AXIS_Z1 + m_idx, 0.0) != MSystem.MMC_OK)
            {
                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                string lr = m_idx == 0 ? "Left" : "Right";
                MSystem.MyMsgMemo($"{lr} Z-UP Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                return false;
            }

            TimCheck.StartTimer();
            //Start Move 
            MSystem.MyMessagerBottom("Vision Align position move start");
            while (true)
            {
                if (MSystem.IsAxisPositionCheck((int)Axis.AXIS_Z1 + m_idx, 0.0) == true)
                {
                    break;
                }
                if (TimCheck.MoreThan(3) == true)
                {
                    MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                    string lr = m_idx == 0 ? "Left" : "Right";
                    MSystem.MyMsgMemo($"Screw {lr} Z Ready Move TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
                    return false;
                }
                Thread.Sleep(1);//await Task.Delay(1);
            }

            for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
            {
                m_dVisionOffsetX[i] = 0.0;
                m_pTrsJig[m_idx].m_dVisionOffsetY[i] = 0.0;

                if (isStart[m_idx] == false && mode == true)
                {
                    return true;
                }

                if ((InforTeaching.Instance.P_Screw[m_idx][i].Vision == true && InforManager.Instance.AutoVisionAlignMode == 1) 
                    || InforManager.Instance.AutoVisionAlignMode == 2)
                {
                    // 마지막 좌표가 비젼 스킵일때
                    if (InforManager.Instance.AutoVisionAlignMode == 2 && InforTeaching.Instance.P_Screw[m_idx][i].Skip == true)
                    {
                        if (i == InforTeaching.Instance.P_Screw[m_idx].Count - 1)
                        {
                            MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.PASS;
                            return true;
                        }
                        continue;
                    }
                    _dx = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X;
                    _dy = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y;
                    _dz = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.VisionZ;

                    // Screw 좌표를 Vision 좌표로 변환-- chuyen toa do screw và vision
                    _dx += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                    _dy += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                    _dx -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                    _dy -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;

                    if (InforManager.Instance.IsVisionAlignZUpUse == true)
                    {
                        // Move Z UP
                        if (MSystem.SetMove((int)Axis.AXIS_Z1 + m_idx, 0.0) != MSystem.MMC_OK)
                        {
                            MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                            string lr = m_idx == 0 ? "Left" : "Right";
                            MSystem.MyMsgMemo($"{lr} Z-UP Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                            return false;
                        }

                        TimCheck.StartTimer();
                        //Start Move 
                        MSystem.MyMessagerBottom("Vision Align position move start");
                        while (true)
                        {
                            if (MSystem.IsAxisPositionCheck((int)Axis.AXIS_Z1 + m_idx, 0.0) == true)
                            {
                                break;
                            }
                            if (TimCheck.MoreThan(3) == true)
                            {
                                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                                string lr = m_idx == 0 ? "Left" : "Right";
                                MSystem.MyMsgMemo($"Screw {lr} Z Ready Move TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
                                return false;
                            }
                            Thread.Sleep(1);//await Task.Delay(1);
                        }
                    }

                    TimCheck.StartTimer();
                    if (MSystem.SetMove((int)Axis.AXIS_X1 + m_idx, _dx) != MSystem.MMC_OK)
                    {
                        MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                        string lr = m_idx == 0 ? "Left" : "Right";
                        MSystem.MyMsgMemo($"Screw {lr} X Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    if (MSystem.SetMove((int)Axis.AXIS_Y1 + m_idx, _dy) != MSystem.MMC_OK)
                    {
                        MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                        string lr = m_idx == 0 ? "Left" : "Right";
                        MSystem.MyMsgMemo($"Screw {lr} Y Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    while (true)
                    {
                        if (MSystem.IsAxisPositionCheck((int)Axis.AXIS_X1 + m_idx, _dx)
                        && MSystem.IsAxisPositionCheck((int)Axis.AXIS_Y1 + m_idx, _dy))
                        {
                            double dY = MSystem.GetCurrentPos((int)Axis.AXIS_Y1 + m_idx);
                            break;
                        }
                        if (TimCheck.MoreThan(3) == true)
                        {
                            MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                            string lr = m_idx == 0 ? "Left" : "Right";
                            MSystem.MyMsgMemo($"Screw {lr} XY Move TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
                            return false;
                        }
                        Thread.Sleep(10); //await Task.Delay(1);
                    }
                    #region Move FWD & Kit Down
                    if (InforManager.Instance.InspectionType != "Tablet")
                    {
                        TimCheck.StartTimer();
                        if (MSystem.m_pTrsJig[m_idx].IsFwd() != true)
                            MSystem.m_pTrsJig[m_idx].Fwd();

                        while (true)
                        {
                            if (MSystem.m_pTrsJig[m_idx].IsFwd() == true)
                            {
                                break;
                            }
                            if (TimCheck.MoreThan(3) == true)
                            {
                                string lr = m_idx == 0 ? "Left" : "Right";
                                MSystem.MyMsgMemo($"Screw {lr} Fwd MoveTimeOut", "Alarm", msgButton.OK, msgIcon.Error);
                                return false;
                            }
                            Thread.Sleep(1); //await Task.Delay(1);
                        }
                        if (MSystem.m_pTrsJig[m_idx].IsDown() != true)
                            MSystem.m_pTrsJig[m_idx].Down();
                        while (true)
                        {
                            if (MSystem.m_pTrsJig[m_idx].IsDown() == true)
                            {
                                break;
                            }
                            if (TimCheck.MoreThan(3) == true)
                            {
                                string lr = m_idx == 0 ? "Left" : "Right";
                                MSystem.MyMsgMemo($"Screw {lr} Kit Down TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
                                return false;
                            }
                            Thread.Sleep(1); //await Task.Delay(1);
                        }
                    }
                    #endregion
                    TimCheck.StartTimer();

                    if (MSystem.SetMove((int)Axis.AXIS_Z1 + m_idx, _dz) != MSystem.MMC_OK)
                    {
                        MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                        string lr = m_idx == 0 ? "Left" : "Right";
                        MSystem.MyMsgMemo($"Screw {lr} Z Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    while (true)
                    {
                        if (MSystem.IsAxisPositionCheck((int)Axis.AXIS_Z1 + m_idx, _dz))
                        {
                            break;
                        }
                        if (TimCheck.MoreThan(3) == true)
                        {
                            MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                            string lr = m_idx == 0 ? "Left" : "Right";
                            MSystem.MyMsgMemo($"Screw {lr} Z Move TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
                            return false;
                        }
                        Thread.Sleep(10); //await Task.Delay(1);
                    }

                    Thread.Sleep(10);
                    if (MSystem.vinterface.SendToSET_T(m_idx, i, true) == true)
                    {
                        MSystem.AutoForm.UpdateAlignImage(m_idx, i);
                        
                        MSystem.MyMessagerBottom($"Vision Align Success. X : {_dx:F2} Y : {_dy:F2}");
                        
                        if (InforManager.Instance.AutoVisionAlignMode == 2) // All Position
                        {
                            _dx = MSystem.vinterface.VR[m_idx].X;
                            _dy = (MSystem.vinterface.VR[m_idx].Y);// * -1.0);

                            m_dVisionOffsetX[i] = _dx;
                            m_pTrsJig[m_idx].m_dVisionOffsetY[i] = _dy;

                            if (i == InforTeaching.Instance.P_Screw[m_idx].Count - 1)
                            {
                                if (mode == true)
                                {
                                    InforProduct.Instance.ProductScrew_Pass_Vision[m_idx] += 1;
                                    InforProduct.Instance.SaveSettings();
                                }
                                
                                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.PASS;
                                return true;
                            }
                        } 
                        else if(InforManager.Instance.AutoVisionAlignMode == 1) // 2 Position
                        {
                            _dx +=  MSystem.vinterface.VR[m_idx].X;
                            _dy += (MSystem.vinterface.VR[m_idx].Y);// * -1.0);
                            // Vision 좌표를 Screw 좌표로 변환
                            _dx += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
                            _dy += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
                            _dx -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
                            _dy -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
                            _dz = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z; // Z는 원복
                            if (LastPos == false)
                            {
                                SetFirstPos(i, _dx, _dy, _dz);
                                LastPos = true;
                            }
                            else
                            {
                                if (mode == true)
                                {
                                    InforProduct.Instance.ProductScrew_Pass_Vision[m_idx] += 1;
                                    InforProduct.Instance.SaveSettings();
                                }
                                   
                                SetLastPos(i, _dx, _dy, _dz);
                                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.PASS;
                                return true;
                            }
                        }
                    }
                    else
                    {
                        MSystem.AutoForm.UpdateAlignImage(m_idx, i);
                        if (mode == true)
                        {
                            InforTeaching.Instance.P_Screw[m_idx][i].status = 4;
                            InforProduct.Instance.ProductDetail[m_idx][i].m_dCountFailVision += 1;
                            InforProduct.Instance.DetailDataAdd(m_idx, i);
                            InforProduct.Instance.ProductScrew_Fail_Vision[m_idx] += 1;
                            InforProduct.Instance.SaveSettings();
                        }
                            
                        if (InforManager.Instance.AutoVisionAlignMode == 1)
                        {
                            for (int j = 0; j < InforTeaching.Instance.P_Screw[m_idx].Count; j++)
                            {
                                m_dVisionOffsetX[j] = 0.0;
                                m_pTrsJig[m_idx].m_dVisionOffsetY[j] = 0.0;
                            }
                            MSystem.MyMessagerBottom($"Vision Align Fail.");
                            if (MSystem.m_bManual_Align == true
                                || InforManager.Instance.IsVisionScanNgOut == true)
                            {
                                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                                return false;
                            }
                            else
                            {
                                
                                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.PASS;
                                return true;
                            }
                            
                        }
                        else if (InforManager.Instance.AutoVisionAlignMode == 2)
                        {
                            m_dVisionOffsetX[i] = 0.0;
                            m_pTrsJig[m_idx].m_dVisionOffsetY[i] = 0.0;
                            if (i == InforTeaching.Instance.P_Screw[m_idx].Count -1)
                            {
                                MSystem.MyMessagerBottom($"Vision Align Fail.");
                                if (MSystem.m_bManual_Align == true || InforManager.Instance.IsVisionScanNgOut == true)
                                {
                                    MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                                    return false;
                                }
                                else
                                {
                                    MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.PASS;
                                    return true;
                                }
                            }
                            else
                            {
                                if (InforManager.Instance.IsVisionScanNgOut == true)
                                {
                                    MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            
            return false;
        }
        public bool VisionPointMove(int _pos) //Insert khjo
        {
            TimerDelay TimCheck = new TimerDelay();
            // CHECK SAFETY
            #region
            if (MSystem.IsDetectEmergency())
            {
                MSystem.MyMsgMemo("EMG Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                return false;
            }
            if (!MSystem.IsOriginAll())
            {
                MSystem.MyMsgMemo("Please Origin All Servo before run Program", "Error", msgButton.OK, msgIcon.Error);
                return false;
            }
            if (MSystem.SysStatus == StatusRun.RUN)
            {
                MSystem.MyMsgMemo("Machine Status is Running", "Error", msgButton.OK, msgIcon.Error);
                return false;
            }
            #endregion
            double _dx = 0.00;
            double _dy = 0.00;
            double _dz = 0.00;
            
            _dx = InforTeaching.Instance.P_Screw[m_idx][_pos].PScrew.X;
            _dy = InforTeaching.Instance.P_Screw[m_idx][_pos].PScrew.Y;
            _dz = InforTeaching.Instance.P_Screw[m_idx][_pos].PScrew.VisionZ;

            // Screw 좌표를 Vision 좌표로 변환-- chuyen toa do screw và vision
            _dx += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
            _dy += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
            //_dz += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_VISION].Z;
            _dx -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
            _dy -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
            //_dz -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_SCREW].Z;
            // Move Z UP
            if (MSystem.SetMove((int)Axis.AXIS_Z1 + m_idx, 0.0) != MSystem.MMC_OK)
            {
                string lr = m_idx == 0 ? "Left" : "Right";
                MSystem.MyMsgMemo($"{lr} Z-UP Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                return false;
            }

            TimCheck.StartTimer();
            //Start Move 
            MSystem.MyMessagerBottom("Vision Align position move start");
            while (true)
            {
                if (MSystem.IsAxisPositionCheck((int)Axis.AXIS_Z1 + m_idx, 0.0) == true)
                {
                    break;
                }
                if (TimCheck.MoreThan(3) == true)
                {
                    string lr = m_idx == 0 ? "Left" : "Right";
                    MSystem.MyMsgMemo($"Screw {lr} Z Ready Move TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
                    return false;
                }
                Thread.Sleep(1);//await Task.Delay(1);
            }

            TimCheck.StartTimer();
            if (MSystem.SetMove((int)Axis.AXIS_X1 + m_idx, _dx) != MSystem.MMC_OK)
            {
                string lr = m_idx == 0 ? "Left" : "Right";
                MSystem.MyMsgMemo($"Screw {lr} X Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                return false;
            }
            if (MSystem.SetMove((int)Axis.AXIS_Y1 + m_idx, _dy) != MSystem.MMC_OK)
            {
                string lr = m_idx == 0 ? "Left" : "Right";
                MSystem.MyMsgMemo($"Screw {lr} Y Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                return false;
            }
            while (true)
            {
                if (MSystem.IsAxisPositionCheck((int)Axis.AXIS_X1 + m_idx, _dx)
                && MSystem.IsAxisPositionCheck((int)Axis.AXIS_Y1 + m_idx, _dy))
                {
                    double dY = MSystem.GetCurrentPos((int)Axis.AXIS_Y1 + m_idx);
                    break;
                }
                if (TimCheck.MoreThan(3) == true)
                {
                    string lr = m_idx == 0 ? "Left" : "Right";
                    MSystem.MyMsgMemo($"Screw {lr} XY Move TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
                    return false;
                }
                Thread.Sleep(1); //await Task.Delay(1);
            }
            #region Move FWD & Kit Down
            if (InforManager.Instance.InspectionType != "Tablet")
            {
                TimCheck.StartTimer();
                if (MSystem.m_pTrsJig[m_idx].IsFwd() != true)
                    MSystem.m_pTrsJig[m_idx].Fwd();

                while (true)
                {
                    if (MSystem.m_pTrsJig[m_idx].IsFwd() == true)
                    {
                        break;
                    }
                    if (TimCheck.MoreThan(3) == true)
                    {
                        string lr = m_idx == 0 ? "Left" : "Right";
                        MSystem.MyMsgMemo($"Screw {lr} Fwd MoveTimeOut", "Alarm", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    Thread.Sleep(1); //await Task.Delay(1);
                }
                if (MSystem.m_pTrsJig[m_idx].IsDown() != true)
                    MSystem.m_pTrsJig[m_idx].Down();
                while (true)
                {
                    if (MSystem.m_pTrsJig[m_idx].IsDown() == true)
                    {
                        break;
                    }
                    if (TimCheck.MoreThan(3) == true)
                    {
                        string lr = m_idx == 0 ? "Left" : "Right";
                        MSystem.MyMsgMemo($"Screw {lr} Kit Down TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
                        return false;
                    }
                    Thread.Sleep(1); //await Task.Delay(1);
                }
            }
            #endregion
            TimCheck.StartTimer();

            if (MSystem.SetMove((int)Axis.AXIS_Z1 + m_idx, _dz) != MSystem.MMC_OK)
            {
                string lr = m_idx == 0 ? "Left" : "Right";
                MSystem.MyMsgMemo($"Screw {lr} Z Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
                return false;
            }
            while (true)
            {
                if (MSystem.IsAxisPositionCheck((int)Axis.AXIS_Z1 + m_idx, _dz))
                {
                    break;
                }
                if (TimCheck.MoreThan(3) == true)
                {
                    string lr = m_idx == 0 ? "Left" : "Right";
                    MSystem.MyMsgMemo($"Screw {lr} Z Move TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
                    return false;
                }
                Thread.Sleep(1); //await Task.Delay(1);
            }

            return true;
        }
        public bool ReadBarcode() //Insert khjo 
        {
            TimerDelay TimCheck = new TimerDelay();
            // CHECK SAFETY
            if (MSystem.IsDetectEmergency())
            {
                MSystem.MyMsgMemo("EMG Detect!", "Alarm", msgButton.OK, msgIcon.Error);
                return false;
            }
            if (!MSystem.IsOriginAll())
            {
                MSystem.MyMsgMemo("Please Origin All Servo before run Program", "Error", msgButton.OK, msgIcon.Error);
                return false;
            }
            if (m_idx == 0)
                MSystem.AutoForm.LBL_RESULT_BCR_L.Text = "";
            else if (m_idx == 1)
                MSystem.AutoForm.LBL_RESULT_BCR_R.Text = "";

            MSystem.m_pBcr[m_idx].SendBcrTrig();
            TimerDelay td = new TimerDelay();
            td.StartTimer();
            while (true)
            {
                if (MSystem.m_pBcr[m_idx].IsDataReceived == true)
                {
                    m_bBcrResult = BCR_RESULT.PASS;
                    _Infor.BCR_Data = MSystem.m_pBcr[m_idx].BarcodeData;
                    if (m_idx == 0)
                    {
                        MSystem.AutoForm.LBL_RESULT_BCR_L.ForeColor = Color.Green;
                        MSystem.AutoForm.LBL_RESULT_BCR_L.Text = MSystem.m_pBcr[m_idx].BarcodeData;
                    }
                    else
                    {
                        MSystem.AutoForm.LBL_RESULT_BCR_R.ForeColor = Color.Green;
                        MSystem.AutoForm.LBL_RESULT_BCR_R.Text = MSystem.m_pBcr[m_idx].BarcodeData;
                    }
                    return true;
                }
                else if (td.MoreThan(2.0) == true)
                {
                    m_bBcrResult = BCR_RESULT.FAIL;
                    _Infor.BCR_Data = "";
                    if (m_idx == 0)
                    {
                        MSystem.AutoForm.LBL_RESULT_BCR_L.ForeColor = Color.Red;
                        MSystem.AutoForm.LBL_RESULT_BCR_L.Text = "FAIL BCR";
                    }
                    else
                    {
                        MSystem.AutoForm.LBL_RESULT_BCR_R.ForeColor = Color.Red;
                        MSystem.AutoForm.LBL_RESULT_BCR_R.Text = "FAIL BCR";
                    } 
                    return false;
                }
                Thread.Sleep(10);
            }
        }


        //public async Task<bool> VisionAlignStart()
        //{
        //    MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.READY;
        //    if (MSystem.vinterface.Server.IsClient_Connected == false)
        //    {
        //        MSystem.MyMessagerBottom("Vision Program is Disconnected.");
        //        MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
        //        return false;
        //    }
        //    TimerDelay TimCheck = new TimerDelay();
        //    // CHECK SAFETY
        //    #region
        //    if (MSystem.IsDetectEmergency())
        //    {
        //        MSystem.MyMsgMemo("EMG Detect!", "Alarm", msgButton.OK, msgIcon.Error);
        //        return false;
        //    }
        //    if (!MSystem.IsOriginAll())
        //    {
        //        MSystem.MyMsgMemo("Please Origin All Servo before run Program", "Error", msgButton.OK, msgIcon.Error);
        //        return false;
        //    }
        //    #endregion
        //    double _dx = 0.00;
        //    double _dy = 0.00;
        //    double _dz = 0.00;
        //    bool LastPos = false;
        //    for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
        //    {
        //        if (InforTeaching.Instance.P_Screw[m_idx][i].Vision == true)
        //        {
        //            _dx = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X;
        //            _dy = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y;
        //            _dz = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.VisionZ;

        //            // Screw 좌표를 Vision 좌표로 변환
        //            _dx += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
        //            _dy += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
        //            //_dz += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_VISION].Z;
        //            _dx -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
        //            _dy -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
        //            //_dz -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_SCREW].Z;
        //            // Move Z UP
        //            if (MSystem.SetMove((int)Axis.AXIS_Z1 + m_idx, 0.0) != MSystem.MMC_OK)
        //            {
        //                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
        //                string lr = m_idx == 0 ? "Left" : "Right";
        //                MSystem.MyMsgMemo($"{lr} Z-UP Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
        //                return false;
        //            }

        //            TimCheck.StartTimer();
        //            //Start Move 
        //            MSystem.MyMessagerBottom("Vision Aligne position move start");
        //            while (true)
        //            {
        //                if (MSystem.IsAxisPositionCheck((int)Axis.AXIS_Z1 + m_idx, 0.0) == true)
        //                {
        //                    break;
        //                }
        //                if (TimCheck.MoreThan(3) == true)
        //                {
        //                    MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
        //                    string lr = m_idx == 0 ? "Left" : "Right";
        //                    MSystem.MyMsgMemo($"Screw {lr} Z Ready Move TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
        //                    return false;
        //                }
        //                await Task.Delay(1);
        //            }

        //            TimCheck.StartTimer();
        //            if (MSystem.SetMove((int)Axis.AXIS_X1 + m_idx, _dx) != MSystem.MMC_OK)
        //            {
        //                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
        //                string lr = m_idx == 0 ? "Left" : "Right";
        //                MSystem.MyMsgMemo($"Screw {lr} X Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
        //                return false;
        //            }
        //            if (MSystem.SetMove((int)Axis.AXIS_Y1 + m_idx, _dy) != MSystem.MMC_OK)
        //            {
        //                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
        //                string lr = m_idx == 0 ? "Left" : "Right";
        //                MSystem.MyMsgMemo($"Screw {lr} Y Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
        //                return false;
        //            }
        //            while (true)
        //            {
        //                if (MSystem.IsAxisPositionCheck((int)Axis.AXIS_X1 + m_idx, _dx)
        //                && MSystem.IsAxisPositionCheck((int)Axis.AXIS_Y1 + m_idx, _dy))
        //                {
        //                    double dY = MSystem.GetCurrentPos((int)Axis.AXIS_Y1 + m_idx);
        //                    break;
        //                }
        //                if (TimCheck.MoreThan(3) == true)
        //                {
        //                    MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
        //                    string lr = m_idx == 0 ? "Left" : "Right";
        //                    MSystem.MyMsgMemo($"Screw {lr} XY Move TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
        //                    return false;
        //                }
        //                await Task.Delay(1);
        //            }
        //            #region Move FWD & Kit Down
        //            if (InforManager.Instance.IsInspectionType == false)
        //            {
        //                TimCheck.StartTimer();
        //                if (MSystem.m_pTrsJig[m_idx].IsFwd() != true)
        //                    MSystem.m_pTrsJig[m_idx].Fwd();

        //                while (true)
        //                {
        //                    if (MSystem.m_pTrsJig[m_idx].IsFwd() == true)
        //                    {
        //                        break;
        //                    }
        //                    if (TimCheck.MoreThan(3) == true)
        //                    {
        //                        string lr = m_idx == 0 ? "Left" : "Right";
        //                        MSystem.MyMsgMemo($"Screw {lr} Fwd MoveTimeOut", "Alarm", msgButton.OK, msgIcon.Error);
        //                        return false;
        //                    }
        //                    await Task.Delay(1);
        //                }
        //                if (MSystem.m_pTrsJig[m_idx].IsDown() != true)
        //                    MSystem.m_pTrsJig[m_idx].Down();
        //                while (true)
        //                {
        //                    if (MSystem.m_pTrsJig[m_idx].IsDown() == true)
        //                    {
        //                        break;
        //                    }
        //                    if (TimCheck.MoreThan(3) == true)
        //                    {
        //                        string lr = m_idx == 0 ? "Left" : "Right";
        //                        MSystem.MyMsgMemo($"Screw {lr} Kit Down TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
        //                        return false;
        //                    }
        //                    await Task.Delay(1);
        //                }
        //            }
        //            #endregion
        //            TimCheck.StartTimer();

        //            if (MSystem.SetMove((int)Axis.AXIS_Z1 + m_idx, _dz) != MSystem.MMC_OK)
        //            {
        //                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
        //                string lr = m_idx == 0 ? "Left" : "Right";
        //                MSystem.MyMsgMemo($"Screw {lr} Z Move Fail", "Alarm", msgButton.OK, msgIcon.Error);
        //                return false;
        //            }
        //            while (true)
        //            {
        //                if (MSystem.IsAxisPositionCheck((int)Axis.AXIS_Z1 + m_idx, _dz))
        //                {
        //                    break;
        //                }
        //                if (TimCheck.MoreThan(3) == true)
        //                {
        //                    MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
        //                    string lr = m_idx == 0 ? "Left" : "Right";
        //                    MSystem.MyMsgMemo($"Screw {lr} Z Move TimeOut", "Alarm", msgButton.OK, msgIcon.Error);
        //                    return false;
        //                }
        //                await Task.Delay(1);
        //            }
        //            await Task.Delay(10);
        //            // Vision Data Request
        //            if (await MSystem.vinterface.SendToTcpClient(m_idx, i, true) == true)
        //            {
        //                MSystem.AutoForm.UpdateAlignImage(m_idx,i);

        //                _dx = _dx + (MSystem.vinterface.VR[m_idx].X * -1.0);
        //                _dy = _dy + (MSystem.vinterface.VR[m_idx].Y * -1.0);
        //                MSystem.MyMessagerBottom($"Vision Align Success. X : {_dx.ToString("F2")} Y : {_dy.ToString("F2")}");
        //                // Vision 좌표를 Screw 좌표로 변환
        //                _dx += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].X;
        //                _dy += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_SCREW].Y;
        //                //_dz += InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_SCREW].Z;
        //                _dx -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].X;
        //                _dy -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_MASTER_VISION].Y;
        //                //_dz -= InforTeaching.Instance.P_MainScrew[m_idx, (int)PScrewMain.ePOS_VISION].Z;
        //                _dz = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z; // Z는 원복
        //                if (LastPos == false)
        //                {
        //                    SetFirstPos(i, _dx, _dy, _dz);
        //                    LastPos = true;
        //                }
        //                else
        //                {
        //                    SetLastPos(i, _dx, _dy, _dz);
        //                    MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.PASS;
        //                    return true;
        //                }
        //            }
        //            else
        //            {
        //                //if (LastPos == false)
        //                //{
        //                //    LastPos = true;
        //                //}
        //                //else
        //                //{
        //                //    MSystem.vInterface.m_VisionResult[m_idx] = VS_RESULT.PASS;
        //                //    return true;
        //                //}
        //                MSystem.MyMessagerBottom($"Vision Align Fail.");
        //                MSystem.vinterface.m_VisionResult[m_idx] = VS_RESULT.FAIL;
        //                return false;
        //            }
        //        }
        //    }

        //    return true;
        //}

        private void SetFirstPos(int Screwindex, double x, double y, double z)
        {
            p1Old.X = InforTeaching.Instance.P_Screw[m_idx][Screwindex].PScrew.X;
            p1Old.Y = InforTeaching.Instance.P_Screw[m_idx][Screwindex].PScrew.Y;
            p1Old.Z = InforTeaching.Instance.P_Screw[m_idx][Screwindex].PScrew.Z;

            p1New.X = x;
            p1New.Y = y;
            p1New.Z = z;
        }

        private void SetLastPos(int Screwindex, double _x, double _y, double _z)
        {
            p2Old.X = InforTeaching.Instance.P_Screw[m_idx][Screwindex].PScrew.X;
            p2Old.Y = InforTeaching.Instance.P_Screw[m_idx][Screwindex].PScrew.Y;
            p2Old.Z = InforTeaching.Instance.P_Screw[m_idx][Screwindex].PScrew.Z;

            p2New.X = _x;
            p2New.Y = _y;
            p2New.Z = _z;

            try
            {
                /*///////////////////////////////////Rotation///////////////////////////////
                //double p2NewXDist = p2New.X - p1New.X;
                //double p2NewYDist = p2New.Y - p1New.Y;
                //double p2OldXDist = p2Old.X - p1Old.X;
                //double p2OldYDist = p2Old.Y - p1Old.Y;

                //double p2NewXDistSquare = Math.Pow(p2NewXDist, 2);
                //double p2NewYDistSquare = Math.Pow(p2NewYDist, 2);
                //double p2OldXDistSquare = Math.Pow(p2OldXDist, 2);
                //double p2OldYDistSquare = Math.Pow(p2OldYDist, 2);

                //double newDist = Math.Sqrt(p2NewXDistSquare + p2NewYDistSquare);
                //double oldDist = Math.Sqrt(p2OldXDistSquare + p2OldYDistSquare);

                //double degreeNew = Math.Atan2(p2NewYDist, p2NewXDist);
                //double degreeOld = Math.Atan2(p2OldYDist, p2OldXDist);
                //double degreeDist = degreeNew - degreeOld;
                //double sin = Math.Sin(degreeDist);
                //double cos = Math.Cos(degreeDist);

                //double newCenterX = (p1New.X + p2New.X) / 2;
                //double newCenterY = (p1New.Y + p2New.Y) / 2;
                //double newCenterZ = (p1New.Z + p2New.Z) / 2;
                //double oldCenterX = (p1Old.X + p2Old.X) / 2;
                //double oldCenterY = (p1Old.Y + p2Old.Y) / 2;

                //double convertX;
                //double convertY;
                //double x;
                //double y;
                //for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                //{
                //    x = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X - oldCenterX;
                //    y = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y - oldCenterY;

                //    convertX = cos * x - sin * y;
                //    convertY = sin * x + cos * y;

                //    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X = convertX + newCenterX;
                //    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y = convertY + newCenterY;
                //    InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Z = newCenterZ;
                //}
                */


                ///////////////////////Nội Suy///////////////////////////////////////
                // 기준점 offset
                double offsetX1 = p1New.X - p1Old.X;
                double offsetY1 = p1New.Y - p1Old.Y;

                double offsetX2 = p2New.X - p2Old.X;
                double offsetY2 = p2New.Y - p2Old.Y;

                // Old 기준선 벡터
                double baseDx = p2Old.X - p1Old.X;
                double baseDy = p2Old.Y - p1Old.Y;
                double baseLen2 = baseDx * baseDx + baseDy * baseDy;

                for (int i = 0; i < InforTeaching.Instance.P_Screw[m_idx].Count; i++)
                {
                    // 현재 점을 first 기준으로 좌표계 변환
                    double relX = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X - p1Old.X;
                    double relY = InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y - p1Old.Y;

                    // first→last 방향으로 얼마나 떨어져 있는지 비율 계산 (투영)
                    double t = 0;
                    if (baseLen2 > 0)
                        t = (relX * baseDx + relY * baseDy) / baseLen2;

                    // 보정량 선형 보간
                    double offsetX = offsetX1 + (offsetX2 - offsetX1) * t;
                    double offsetY = offsetY1 + (offsetY2 - offsetY1) * t;

                    m_dVisionOffsetX[i] = offsetX;
                    m_pTrsJig[m_idx].m_dVisionOffsetY[i] = offsetY;
                    if (MSystem.m_bManual_Align == true)
                    {
                        InforTeaching.Instance.P_Screw[m_idx][i].PScrew.X += offsetX;
                        InforTeaching.Instance.P_Screw[m_idx][i].PScrew.Y += offsetY;
                    }
                }
            }
            catch { }
        }
        // Log Screw
        public void LogStep(int step)
        {
            string Log = $"LogScrewStep_{DateTime.Now:yyyyMMdd}- {DateTime.Now:HH:mm:ss fff} Step: {step}";

            m_pLogSave.AddTail(LogIndex.eLogScrew1+ m_idx, Log);
        }
        private void LogScrew(bool type, string eventType = "")//true =OK, False=NG
        {

            Thread.Sleep(50); // Ensure Hantas is ready to read data
            try
            {
                SingleScrewTime.Stop();
                double singleScrewTimeSeconds = SingleScrewTime.ElapsedMilliseconds / 1000.0;

                if (type)
                {
                    eventData = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3200, 14);//event data
                    string screwLog = string.Empty;
                    if (eventData != null)
                    {
                        screwLog = $"LogScrew_{DateTime.Now:yyyyMMdd}- SCREW_DETAIL | Screw: {(m_pTrsJig[m_idx].m_iIndexPoint + 1):D2}/{InforTeaching.Instance.P_Screw[m_idx].Count:D2} | " +
                                                                     $"Torque: {eventData[4] / 100.0:F2}kgf.cm | Result: {eventType} | Time: {singleScrewTimeSeconds}s";


                        if (m_idx == Constants.left)
                            m_pLogSave.AddTail(LogIndex.eLogScrew1, screwLog);
                        else
                            m_pLogSave.AddTail(LogIndex.eLogScrew2, screwLog);
                    }
                }
                else
                {
                    realtimeData = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3300, 13);
                    string screwLog = string.Empty;
                    if (realtimeData != null)
                    {
                        screwLog = $"LogScrew_{DateTime.Now:yyyyMMdd}- SCREW_DETAIL | Screw: {(m_pTrsJig[m_idx].m_iIndexPoint + 1):D2}/{InforTeaching.Instance.P_Screw[m_idx].Count:D2} | " +
                                       $"Torque: {realtimeData[0] / 100.0:F2}kgf.cm | Result: {eventType} | Time: {singleScrewTimeSeconds:F1}s";
                        if (m_idx == Constants.left)
                            m_pLogSave.AddTail(LogIndex.eLogScrew1, screwLog);
                        else
                            m_pLogSave.AddTail(LogIndex.eLogScrew2, screwLog);
                    }
                }
            }
            catch (Exception ex)
            {
                MyMessagerBottom($"Log Screw Error, Hantas data reading fail: {ex.Message}");
                MSystem.m_pLogSave.DevLogSave($" Log Screw Error, Hantas data reading fail: eventData{eventData.Count()} {ex.Message}");
            }
        }
        //Log Hantas
        private void LogAllHantasData(bool type, string eventType = "")//true=OK, false=NG
        {
            try
            {
                //  Event Data (3200-3213)
                Thread.Sleep(50); // Ensure Hantas is ready to read data
                                  //  ushort[] eventData = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3200, 14);

                // Realtime Data(3300 - 3312)  
                // ushort[] realtimeData = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3300, 13);


                string hantasLog = CreateFullHantasLog(/*eventData, realtimeData, */eventType, type);


                if (m_idx == Constants.left)
                    m_pLogSave.AddTail(LogIndex.eLogHantasLeft, hantasLog);
                else
                    m_pLogSave.AddTail(LogIndex.eLogHantasRight, hantasLog);
            }
            catch (Exception ex)
            {
                MyMessagerBottom($"Error logging Hantas data: {ex.Message}");
            }
        }
        private string CreateFullHantasLog(/*ushort[] eventData, ushort[] realtimeData, */string eventType, bool type)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss.fff");
            int screwIndex = m_pTrsJig[m_idx].m_iIndexPoint + 1;

            var logBuilder = new System.Text.StringBuilder();

            // Header
            logBuilder.AppendLine();
            logBuilder.AppendLine();
            logBuilder.AppendLine($"=== HANTAS FULL DATA LOG ===");
            logBuilder.AppendLine($"Event Type: {eventType}");
            logBuilder.AppendLine($"Screw Index: {screwIndex:D2}");
            logBuilder.AppendLine($"Timestamp: {timestamp}");
            logBuilder.AppendLine($"Unit: {(m_idx == Constants.left ? "LEFT" : "RIGHT")}");
            logBuilder.AppendLine();

            // Event Data (3200-3213)
            if (type == true/* &&eventData != null && eventData.Length >= 14*/)
            {
                eventData = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3200, 14);
                logBuilder.AppendLine("=== EVENT DATA (Registers 3200-3213) ===");
                logBuilder.AppendLine($"Event Count (3200): {eventData[0]}");
                logBuilder.AppendLine($"Fastening Time (3201): {eventData[1]} ms");
                logBuilder.AppendLine($"Preset Number (3202): {eventData[2]}");
                logBuilder.AppendLine($"Target Torque (3203): {eventData[3] / 100.0:F2} kgf.cm");
                logBuilder.AppendLine($"Converted Torque (3204): {eventData[4] / 100.0:F2} kgf.cm");
                logBuilder.AppendLine($"Target Speed (3205): {eventData[5]} rpm");
                logBuilder.AppendLine($"A1 Angle (3206): {eventData[6]}°");
                logBuilder.AppendLine($"A2 Angle (3207): {eventData[7]}°");
                logBuilder.AppendLine($"A3 Angle (3208): {eventData[8]}°");
                logBuilder.AppendLine($"Screw Count (3209): {eventData[9]}");
                logBuilder.AppendLine($"Error Code (3210): {eventData[10]}");
                logBuilder.AppendLine($"Forward/Loosening (3211): {eventData[11]}");
                logBuilder.AppendLine($"Status Complete (3212): {eventData[12]}");
                logBuilder.AppendLine($"Snug Torque Angle (3213): {eventData[13]}°");
                //  logBuilder.AppendLine();
            }
            else if (type == false)
            {
                realtimeData = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3300, 13);
                logBuilder.AppendLine($"RT Converted Torque (3300): {realtimeData[0] / 100.0:F2} kgf.cm");
                logBuilder.AppendLine($"RT Speed (3301): {realtimeData[1]} rpm");
                logBuilder.AppendLine($"RT Motor Current (3302): {realtimeData[2]} mA");
                logBuilder.AppendLine($"RT Current Preset (3303): {realtimeData[3]}");
                logBuilder.AppendLine($"RT Torque Up (3304): {realtimeData[4]}");
                logBuilder.AppendLine($"RT Fastening OK (3305): {realtimeData[5]}");

            }


            // Realtime Data (3300-3312)
            /*     if (realtimeData != null && realtimeData.Length >= 13 && eventData != null && eventData.Length >= 14)
                 {

                     *//*logBuilder.AppendLine("=== REALTIME DATA (Registers 3300-3312) ===");
                     logBuilder.AppendLine($"RT Converted Torque (3300): {realtimeData[0] / 100.0:F2} kgf.cm");
                     logBuilder.AppendLine($"RT Speed (3301): {realtimeData[1]} rpm");
                     logBuilder.AppendLine($"RT Motor Current (3302): {realtimeData[2]} mA");
                     logBuilder.AppendLine($"RT Current Preset (3303): {realtimeData[3]}");
                     logBuilder.AppendLine($"RT Torque Up (3304): {realtimeData[4]}");
                     logBuilder.AppendLine($"RT Fastening OK (3305): {realtimeData[5]}");
                     logBuilder.AppendLine($"RT Ready (3306): {realtimeData[6]}");
                     logBuilder.AppendLine($"RT Motor RUN (3307): {realtimeData[7]}");
                     logBuilder.AppendLine($"RT Alarm Number (3308): {realtimeData[8]}");
                     logBuilder.AppendLine($"RT Forward/Loosening (3309): {realtimeData[9]}");
                     logBuilder.AppendLine($"RT Screw Count (3310): {realtimeData[10]}");
                     logBuilder.AppendLine($"RT Input Status (3311): {realtimeData[11]}");
                     logBuilder.AppendLine($"RT Output Status (3312): {realtimeData[12]}");
                     logBuilder.AppendLine();*//*
                 }*/

            logBuilder.AppendLine("=== END OF HANTAS DATA ===");
            logBuilder.AppendLine("=================================================================================");
            logBuilder.AppendLine();

            return logBuilder.ToString();
        }


        public bool OverAngleA2FirstCheck()
        {
            try
            {
                //Thread.Sleep(50);
                eventData = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3200, 14);
                if (lastA2Angle == eventData[7])
                    return false;
                if (eventData != null && eventData.Length >= 14)
                {
                    MyMessagerBottom($"A2 First{eventData[7]}");
                    lastA2Angle = eventData[7];
                    return (eventData[7] > InforManager.Instance.m_iHantasA2AngleControl);
                }
            }
            catch (Exception ex)
            {
                MyMessagerBottom($"Error Read A2 Angle First Check data: {ex.Message}");
            }
            return false;
        }

        public bool OverAngleA2CheckRetry()
        {
            try
            {
                // Thread.Sleep(70);
                eventData = MSystem.m_pHantas[m_idx].MbReadInputRegister(1, 3200, 14);
                if (lastA2Angle == eventData[7])
                    return false;
                if (eventData != null && eventData.Length >= 14/* && lasteventData!=eventData*/)
                {
                    MyMessagerBottom($"A2 Retry{eventData[7]}");
                    lastA2Angle = eventData[7];
                    return (eventData[7] > InforManager.Instance.m_iHantasA2AngleRetryControl);
                }
            }
            catch (Exception ex)
            {
                MyMessagerBottom($"Error Read A2 Angle Retry data: {ex.Message}");
            }

            return false;


        }
    }
}

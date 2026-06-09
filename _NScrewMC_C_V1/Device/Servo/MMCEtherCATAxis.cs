using System;
using System.Threading;
using NMCMotionSDK;

namespace _NScrewMC_C_V1
{
    public partial class MMCEtherCATAxis : MSystem
    {
        TimerDelay m_ttOriginTimer = new TimerDelay();

        public sAxisInfo m_sAxisInfo = new sAxisInfo();

        public int m_iOriginStep = 100;
        public int m_iOriginPrevStep;
        public bool m_bOriginFlag;
        public string OriginErrorName { get; set; } = string.Empty;

        #region // Variable
        public string m_strAxisName;
        private ushort m_iAxisID = 0;
        private byte m_iPort;
        public double m_dScale;
        public int m_iPrior;
        public bool m_bFlagOrigin = false;
        #endregion
        
        #region //Axis Initial
        public MMCEtherCATAxis(string strName, ushort id, double scale, int prior)
        {
            m_strAxisName = strName;
            m_iAxisID = (ushort)(2 + id);
            m_dScale = scale;
            m_iPrior = prior;
            m_bOriginFlag = false;
            LoadData();
        }

        ~MMCEtherCATAxis() {

        }
        #endregion

        #region //Parameter and data
        public void LoadData() {
            ServoParam.Instance.LoadSetting();
            m_sAxisInfo = ServoParam.Instance.m_sAxisInfo[m_iAxisID - 2];
            SaveData();
        }
        public void SaveData() {
            //ServoParam.Instance.SaveSettings();
            SetParameter(3, (int)m_sAxisInfo.dAxisAcc);
            SetParameter(4, (int)m_sAxisInfo.dAxisAcc);
            SetParameter(8, (int)m_sAxisInfo.dAxisAcc);
            SetParameter(9, (int)m_sAxisInfo.dLimitPluseValue);
            SetParameter(10, (int)m_sAxisInfo.dLimitMinusValue);
           // SetParameter(13, 0);
        }
        #endregion

        #region //Tool function
        public int OriginReturn(bool bMove)
        {
            int iResult = 0;
            bool bStatus = false;
            bool bResult = false;

            switch (m_iOriginStep)
            {
                case 100:
                    m_bOriginFlag = false;
                    SetOriginStep(110);
                    break;

                case 110:
                    if (GetAmpFault() == true)
                    {
                        bResult = ClearAxis();
                        if (bResult == false)
                        {
                            OriginErrorName = "Servo Alarm Reset Fail";
                            SetOriginStep(1000);
                            break;
                        }

                        SetOriginStep(111);
                        break;
                    }

                    SetOriginStep(115);
                    break;

                case 111:
                    if (GetAmpFault() == false)
                        SetOriginStep(115);

                    if (m_ttOriginTimer.MoreThan(10) == true)
                    {
                        OriginErrorName = "Servo Alarm Reset Fail";
                        SetOriginStep(1000);
                        break;
                    }
                    break;

                case 115:
                    if (GetAmpEnable() == false)
                    {
                        bResult = PowerOn();
                        if (bResult == false)
                        {
                            OriginErrorName = "Servo On Fail";
                            SetOriginStep(1000);
                            break;
                        }

                        SetOriginStep(116);
                        break;
                    }

                    SetOriginStep(200);
                    break;

                case 116:
                    if (GetAmpEnable() == true)
                        SetOriginStep(200);

                    if (m_ttOriginTimer.MoreThan(10) == true)
                    {
                        OriginErrorName = "Servo On Fail";
                        SetOriginStep(1000);
                        break;
                    }
                    break;

                case 200:
                    SetOriginStep(210);
                    break;

                case 210:
                    SetOriginStep(220);
                    break;

                case 220:
                    if (NMCSDKLib.MC_Home(0, m_iAxisID, 0, NMCSDKLib.MC_BUFFER_MODE.mcAborting) != NMCSDKLib.MC_STATUS.MC_OK)
                    {
                        OriginErrorName = "Origin Fail";
                        SetOriginStep(1000);
                        break;
                    }

                    SetOriginStep(305);
                    break;

                case 305:
                    if (m_ttOriginTimer.MoreThan(0.2) == true)
                    {  
                        SetOriginStep(310);
                        m_ttOriginTimer.StartTimer();
                    }
                    break;

                case 310:
                    if ((IsOriginOK()&&IsAxisDone()) || (m_ttOriginTimer.MoreThan(7.0)&& MSystem.SIMULATION))
                    {
                        SetOrigin(true);
                        SetOriginStep(2000);
                        break;
                    }
                    else if (m_ttOriginTimer.MoreThan(m_sAxisInfo.dOriginLimitTime))
                    {
                        OriginErrorName = "Origin TimeOut";
                        SetOriginStep(1000);
                        break;
                    }
                    break;

                case 315:
                    if (m_ttOriginTimer.MoreThan(0.2))
                        SetOriginStep(2000);
                    break;

                case 1000:
                    ResetOrigin();
                    EStop();
                    Thread.Sleep(50);
                    SetOriginStep(999);
                    break;

                case 2000:
                    SetOrigin(true);
                    break;

                default:
                    OriginErrorName = "Step Error";
                    SetOriginStep(1000);
                    break;
            }
            return m_iOriginStep;
        }

        public bool PowerOn()
        {
            NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_Power(0, m_iAxisID, true);
            return mc == NMCSDKLib.MC_STATUS.MC_OK;
        }

        public bool PowerOff()
        {
            NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_Power(0, m_iAxisID, false);
            return mc == NMCSDKLib.MC_STATUS.MC_OK;
        }

        public bool GetAmpEnable()
        {
            bool flagServOn = false;
            lock (LockAxisManager) {

                uint status = 0;
                NMCSDKLib.MC_ReadAxisStatus(0, m_iAxisID, ref status);
                flagServOn = (status & 0x00800000) != 0;
            }

            return flagServOn;
        }
        #endregion

        public bool ClearAxis() {

            return NMCSDKLib.MC_Reset(0, m_iAxisID) == NMCSDKLib.MC_STATUS.MC_OK;
        }

        public void SetOriginStep(int iStep) {
            m_iOriginPrevStep = m_iOriginStep;
            m_iOriginStep = iStep;
            m_ttOriginTimer.StartTimer();
        }

        public bool GetAmpFault()
        {
            NMCSDKLib.MC_STATUS mc;
            bool bStatus = false;
            lock (LockAxisManager)
            {

                uint status = 0;
                mc = NMCSDKLib.MC_ReadAxisStatus(0, m_iAxisID, ref status);
                bStatus = (status & (uint)NMCSDKLib.MC_AXISSTATUS.mcDriveFault) != 0;
            }

            return bStatus;
            //if ((AxisStatus & (uint)NMCSDKLib.MC_AXISSTATUS.mcErrorStop) != 0)
            //    return true; //mcErrorStop
            //else
            //    return false;
        }

        public bool IsAxisDone()
        {
            uint status = 0;
            if (NMCSDKLib.MC_STATUS.MC_OK != NMCSDKLib.MC_ReadAxisStatus(0, m_iAxisID, ref status))
                return false;

            bool flag1, flag2;
            flag1 = (status & (uint)NMCSDKLib.MC_AXISSTATUS.mcMotionComplete) != 0;
            flag2 = (status & (uint)NMCSDKLib.MC_AXISSTATUS.mcContinuousMotion) != 0;

            if (flag1 == true && flag2 == false) 
                return true;
            else 
                return false;
        }

        public bool IsReady()
        {
            uint status = 0;
            if (NMCSDKLib.MC_STATUS.MC_OK != NMCSDKLib.MC_ReadAxisStatus(0, m_iAxisID, ref status))
                return false;

            bool flag1, flag2;
            flag1 = (status & (uint)NMCSDKLib.MC_AXISSTATUS.mcMotionComplete) != 0;
            flag2 = (status & (uint)NMCSDKLib.MC_AXISSTATUS.mcContinuousMotion) != 0;

            if (flag1 == true && flag2 == false)
                return true;
            else
                return false;
        }

        public bool IsMoveDone(double targetPos)
        {
            if (Math.Abs(GetActualPos() - targetPos) < 0.1)
                return true;

            return false;
        }

        public double GetActualPos()
        {
            double currentPos = 0;
            try
            {
                NMCSDKLib.MC_ReadActualPosition(0, m_iAxisID, ref currentPos);
            }
            catch (Exception) { }
            return currentPos / m_dScale;
        }

        public int EStop()
        {
            double decel = m_sAxisInfo.dAxisMaxSpeed * m_dScale * 1000.0 / m_sAxisInfo.dAxisAcc;
            NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_Halt(0, m_iAxisID, decel, 0, NMCSDKLib.MC_BUFFER_MODE.mcAborting);
            return (int)mc;
        }

        public int VStop()
        {
            double decel = m_sAxisInfo.dAxisMaxSpeed * m_dScale * 1000.0 / m_sAxisInfo.dAxisAcc;
            NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_Halt(0, m_iAxisID, decel, 0, NMCSDKLib.MC_BUFFER_MODE.mcAborting);
            return (int)mc;
        }

        public int Stop()
        {
            double decel = m_sAxisInfo.dAxisMaxSpeed * m_dScale * 1000.0 / m_sAxisInfo.dAxisAcc;
            NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_Halt(0, m_iAxisID, decel, 0, NMCSDKLib.MC_BUFFER_MODE.mcAborting);
            return (int)mc;
        }

        public void SetOrigin(bool flag)
        {
            m_bOriginFlag = flag;
        }

        public void ResetOrigin()
        {
            m_bOriginFlag = false;
            m_iOriginStep = 100;
            m_iOriginPrevStep = 100;
        }

        public int StartMove(double position)
        {
            double pos = position * m_dScale;
            double vel = m_sAxisInfo.dAxisMaxSpeed * m_dScale;
            double acc = m_sAxisInfo.dAxisMaxSpeed * m_dScale * 1000.0 / m_sAxisInfo.dAxisAcc;
            NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_MoveAbsolute(0, m_iAxisID, pos, vel, acc, acc, 0, NMCSDKLib.MC_DIRECTION.mcPositiveDirection, NMCSDKLib.MC_BUFFER_MODE.mcAborting);

            if (mc != NMCSDKLib.MC_STATUS.MC_OK)
            {
                return 100001 + (m_iAxisID * 1000);
            }

            return 0;
        }

        public int StartMove(double position, double speed = 0.0)
        {
            double pos = position * m_dScale;
            double vel;
            if (speed == 0.0)
                vel = m_sAxisInfo.dAxisMaxSpeed * m_dScale;
            else
                vel = speed * m_dScale;
            double acc = vel * m_dScale * 1000.0 / m_sAxisInfo.dAxisAcc;
            NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_MoveAbsolute(0, m_iAxisID, pos, vel, acc, acc, 0, NMCSDKLib.MC_DIRECTION.mcPositiveDirection, NMCSDKLib.MC_BUFFER_MODE.mcAborting);

            if (mc != NMCSDKLib.MC_STATUS.MC_OK)
            {
                return 100001 + (m_iAxisID * 1000);
            }
            return 0;
        }

        public int GetOriginPriority()
        {
            return m_iPrior;
        }

        public int GetOriginStep()
        {
            return m_iOriginStep;
        }

        public double GetCurrentPos()
        {
            double currentPos = 0;

            try
            {
                NMCSDKLib.MC_ReadActualPosition(0, m_iAxisID, ref currentPos);
            }
            catch
            {
                
            }
            
            return currentPos / m_dScale;
        }
        public bool IsOrigin()
        {
            return (IsOriginOK());
            // return (m_bOriginFlag && IsOriginOK());
        }

        public bool IsOriginOK()
        {
            if (MSystem.SIMULATION) 
                return true;

            bool flagHomeComp = false;
            lock (LockAxisManager)
            {

                uint status = 0;
                NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_ReadAxisStatus(0, m_iAxisID, ref status);
                if (mc != NMCSDKLib.MC_STATUS.MC_OK)
                    return false;
                
                flagHomeComp = (status & (uint)NMCSDKLib.MC_AXISSTATUS.mcIsHomed) != 0;
                //flagHomeComp = (status & 0x01000000) != 0;
            }

            return flagHomeComp;
        }
        public int JogMoveSlow(bool dir)
        {
            NMCSDKLib.MC_DIRECTION md = dir == true ? NMCSDKLib.MC_DIRECTION.mcPositiveDirection : NMCSDKLib.MC_DIRECTION.mcNegativeDirection;
            double vel = m_sAxisInfo.dJogSlowSpeed * m_dScale;
            double acc = vel * 1000.0 / m_sAxisInfo.dAxisAcc;
            return (int)NMCSDKLib.MC_MoveVelocity(0, m_iAxisID, vel, acc, acc, 0, md, NMCSDKLib.MC_BUFFER_MODE.mcAborting);
        }

        public int JogMoveMid(bool dir)
        {
            NMCSDKLib.MC_DIRECTION md = dir == true ? NMCSDKLib.MC_DIRECTION.mcPositiveDirection : NMCSDKLib.MC_DIRECTION.mcNegativeDirection;
            double vel = m_sAxisInfo.dJogMidSpeed * m_dScale;
            double acc = vel * 1000.0 / m_sAxisInfo.dAxisAcc;
            return (int)NMCSDKLib.MC_MoveVelocity(0, m_iAxisID, vel, acc, acc, 0, md, NMCSDKLib.MC_BUFFER_MODE.mcAborting);
        }

        public int JogMoveFast(bool dir)
        {
            NMCSDKLib.MC_DIRECTION md = dir == true ? NMCSDKLib.MC_DIRECTION.mcPositiveDirection : NMCSDKLib.MC_DIRECTION.mcNegativeDirection;
            double vel = m_sAxisInfo.dJogFastSpeed * m_dScale;
            double acc = vel * 1000.0 / m_sAxisInfo.dAxisAcc;
            return (int)NMCSDKLib.MC_MoveVelocity(0, m_iAxisID, vel, acc, acc, 0, md, NMCSDKLib.MC_BUFFER_MODE.mcAborting);
        }

        public double GetTargetPos()
        {
            double currentPos = 0;
            NMCSDKLib.MC_ReadCommandedPosition(0, m_iAxisID, ref currentPos);
            return currentPos / m_dScale;
        }

        public int SetParameter(byte iParamNo, int lParamValue)
        {
            // todo : have to save parameter later..
            return 0;
            //int iResult = 0;

            //if (EziMOTIONPlusELib.FMM_OK != (iResult = EziMOTIONPlusELib.FAS_SetParameter(m_iAxisID, iParamNo, lParamValue)))
            //{
            //    return iResult;
            //}

            //if (EziMOTIONPlusELib.FMM_OK != (iResult = EziMOTIONPlusELib.FAS_SaveAllParameters( m_iAxisID)))
            //{
            //    return iResult;
            //}

            //return 0;
        }

        public void GetAxisInfo(ref sAxisInfo pax1info)
        {
            pax1info = m_sAxisInfo;
        }

        public int SetAxisInfo(sAxisInfo pax1info)
        {
            m_sAxisInfo = pax1info;
            return 0;
        }

        public bool IsPlusLimit()
        {
            bool result = false;
            lock (LockAxisManager)
            {
                uint status = 0;
                NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_ReadAxisStatus(0, m_iAxisID, ref status);
                if (mc != NMCSDKLib.MC_STATUS.MC_OK)
                    return false;

                result = (status & (uint)NMCSDKLib.MC_AXISSTATUS.mcLimitSwitchPosEvent) != 0;
            }

            return result;
        }

        public bool IsMinusLimit()
        {
            bool result = false;
            lock (LockAxisManager)
            {
                uint status = 0;
                NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_ReadAxisStatus(0, m_iAxisID, ref status);
                if (mc != NMCSDKLib.MC_STATUS.MC_OK)
                    return false;

                result = (status & (uint)NMCSDKLib.MC_AXISSTATUS.mcLimitSwitchNegEvent) != 0;
            }

            return result;
        }
        public bool IsGetOriginStatus()
        {
            bool result = true;
            lock (LockAxisManager)
            {
                uint status = 0;
                NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_ReadAxisStatus(0, m_iAxisID, ref status);
                if (mc != NMCSDKLib.MC_STATUS.MC_OK)
                    return false;
            }
            return result;
        }
    }
}

using System;
using System.Threading;

namespace _NScrewMC_C_V1
{
    public class MTrsAutoManager : MSystem
    {
        TimerDelay TimDetectEMC = new TimerDelay();
        string _strDoormessenger = "";
        public MTrsAutoManager()
        {
            Thread t = new Thread(new ThreadStart(MessengerPoll));
            t.IsBackground = true;
            t.Start();
        }

        ~MTrsAutoManager()
        {

        }

        void MessengerPoll()
        {
            while (true)
            {
                try
                {
                    if (SysStatus == StatusRun.RUN)
                    {

                        if (!IsDry() && MSystem.m_pTrsScrew[MSystem.eLEFT].m_bOnPickupError)
                        {
                            m_pTrsBuzzer.SetBuzzerPattern(0);
                            if (MyMsgMemo("Left Screw Pickup Fail...!", "Retry Pickup", msgButton.OK, msgIcon.Infor) == System.Windows.Forms.DialogResult.OK)
                            {
                                MSystem.m_pTrsScrew[MSystem.eLEFT].m_bOnPickupError = false;
                                m_pTrsBuzzer.StopAllBuzzers();
                            }
                        }
                        if (!IsDry() && MSystem.m_pTrsScrew[MSystem.eRIGHT].m_bOnPickupError)
                        {
                            m_pTrsBuzzer.SetBuzzerPattern(0);
                            if (MyMsgMemo("Right Screw Pickup Fail...!", "Retry Pickup", msgButton.OK, msgIcon.Infor) == System.Windows.Forms.DialogResult.OK)
                            {
                                MSystem.m_pTrsScrew[MSystem.eRIGHT].m_bOnPickupError = false;
                                m_pTrsBuzzer.StopAllBuzzers();
                            }
                        }
                        //if(!IsDry() && MSystem.m_pTrsScrew[MSystem.eLEFT].m_bOnAlignFail)
                        //{
                        //    m_pTrsBuzzer.SetBuzzerPattern(0);
                        //    if (MyMsgMemo("Left Screw Align Fail...!", "Retry Pickup", msgButton.OK, msgIcon.Infor) == System.Windows.Forms.DialogResult.OK)
                        //    {
                        //        MSystem.m_pTrsScrew[MSystem.eLEFT].m_bOnAlignFail = false;
                        //        m_pTrsBuzzer.StopAllBuzzers();
                        //    }
                        //}
                        //if (!IsDry() && MSystem.m_pTrsScrew[MSystem.eRIGHT].m_bOnAlignFail)
                        //{
                        //    m_pTrsBuzzer.SetBuzzerPattern(0);
                        //    if (MyMsgMemo("Right Screw Align Fail...!", "Retry Pickup", msgButton.OK, msgIcon.Infor) == System.Windows.Forms.DialogResult.OK)
                        //    {
                        //        MSystem.m_pTrsScrew[MSystem.eRIGHT].m_bOnAlignFail = false;
                        //        m_pTrsBuzzer.StopAllBuzzers();
                        //    }
                        //}
                    }
                }
                finally
                {
                    Thread.Sleep(15);
                }
                if (!MSystem.m_bLife) break;
            }
        }
        public void dorunStep()
        {
            while (true)
            {
                switch (SysStatus)
                {
                    case StatusRun.ERROR:
                        MSystem.SysStatus = StatusRun.ERROR;
                        break;

                    case StatusRun.STOP:
                        if (IsDetectEmergency())
                        {
                            OnEmergency();
                            break;
                        }

                        break;

                    case StatusRun.RUN:
                        if (IsDetectEmergency())
                        {
                            m_pLogSave.AddTail(LogIndex.eDeviceLog, "EMC Press Detect");
                            m_pLogSave.ErrorStopTimeLogSave($"Error Stop,EMERGENCY_DETECT,");
                            OnEmergency();
                            break;
                        }

                        if (IsDetectDoorOpen(out _strDoormessenger) == true)
                        {
                            MSystem.SysStatus = StatusRun.ERROR;
                            isStart[Constants.left] = false;
                            isStart[Constants.right] = false;

                            StopAllServo();
                            m_pLogSave.ErrorStopTimeLogSave($"Error Stop,DOOR_OPEN,");
                            MSystem.m_pLogSave.AddTail(LogIndex.eDeviceLog, _strDoormessenger);
                            m_pLogSave.AddTail(LogIndex.eLogGEIM, $"{1}"); //GEIM DOOR OPEN
                            MSystem.SysStatus = StatusRun.STOP;
                            //m_pTrsBuzzer.SetBuzzerPattern(0);
                            //MSystem.MyMsgMemo(_strDoormessenger, "Error");
                            MSystem.MySafetyAlarm();
                            MSystem.m_bIsDoorOpen = true;
                            break;
                        }

                        if (!SIMULATION && m_pDIO.IsOff(IOMap.IN["IN_FRONT_OP_BOX_E_STOP_SW"]))
                        {
                            MSystem.SysStatus = StatusRun.STOP;
                            isStart[Constants.left] = false;
                            isStart[Constants.right] = false;
                            StopAllServo();
                            break;
                        }
                        break;

                    default:
                        break;
                }

                Thread.Sleep(15);
            }
        }

        public void OnEmergency()
        {
            if (MSystem.SIMULATION) return;
            if (IsDetectEmergency())
            {
                if (SysStatus == StatusRun.RUN)
                    m_pLogSave.AddTail(LogIndex.eDeviceLog, $"{MSystem.SysStatus} --> SW EMC Press");

                SysStatus = StatusRun.ERROR;
                MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
                if (TimDetectEMC.MoreThan(2))
                {
                    //MSystem.MyMsgMemo("E-STOP BUTTON PRESSED!!\r\nPUSH RESET BUTTON 2sec", "Emergency Stop", msgButton.OK, msgIcon.Error);
                    //MSystem.m_pTrsBuzzer.BuzzerOff();
                    MSystem.MySafetyAlarm();
                }
                StopAllServo();
                TimDetectEMC.StartTimer();
                SysStatus = StatusRun.STOP;
                MSystem.m_pTrsBuzzer.StopAllBuzzers();
            }
        }

        public void StopAllServo()
        {
            MSystem.SetAllStop();
        }

        public void AllServoOff()
        {
            try
            {
                for (int i = 0; i < eAXIS_MAX; i++)
                {
                    if (MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i] == null)
                        continue;

                    MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].ResetOrigin();
                    MSystem.m_pAxisManager.m_pMmcEtherCatAxis[i].PowerOff();
                }
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("AllServoOff error");
                Thread.Sleep(100);
            }
        }
        public void DoorLock()
        {
        }

        public void DoorUnlock()
        {

        }
    }
}

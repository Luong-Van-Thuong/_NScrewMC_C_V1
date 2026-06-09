using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _NScrewMC_C_V1
{
    public class MTrsOP : MSystem
    {
        public bool m_bStopButton;
        public bool m_bStartButton;
        //************************
        public bool m_bLeftStopButton;
        public bool m_bRightStopButton;
        public MTrsOP() {

        }
        ~MTrsOP() {

        }

        public void dorunStep()
        {
            while (true)
            {
                OPSW();
                OPLamp();
                switch (MSystem.SysStatus) {
                    case StatusRun.ERROR:
                        break;
                    case StatusRun.STOP:
                        break;
                    case StatusRun.RUN:
                        break;
                    default:break;
                }
                Thread.Sleep(15);
            }
        }
        void OPLamp()
        {
            if (MSystem.SysStatus == StatusRun.RUN)
            {
                m_pDIO.OutPutOn(IOMap.OUT["OUT_FRONT_OP_START_SW_LAMP"]);
                m_pDIO.OutPutOff(IOMap.OUT["OUT_FRONT_OP_STOP_SW_LAMP"]);
            }
            else
            {
                m_pDIO.OutPutOff(IOMap.OUT["OUT_FRONT_OP_START_SW_LAMP"]);
                m_pDIO.OutPutOn(IOMap.OUT["OUT_FRONT_OP_STOP_SW_LAMP"]);
            }
            if (MSystem.SysStatus == StatusRun.ERROR)
            {
                m_pDIO.OutPutOff(IOMap.OUT["OUT_FRONT_OP_START_SW_LAMP"]);
                m_pDIO.OutPutOn(IOMap.OUT["OUT_FRONT_OP_STOP_SW_LAMP"]);
            }
        }
        public void OPSW() {
            /**********Stop button********/
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_FRONT_OP_BOX_STOP_SW"]))
            {
                m_pTrsBuzzer.StopAllBuzzers();
                if (!m_bStopButton)
                {
                    m_bStopButton = true;
                    m_bStartButton = false;
                    OnStopButton();
                }
            }
            else
            {
                m_bStopButton = false;
            }

            /********Start Button*************/
            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_FRONT_OP_BOX_START_SW"]))
            {
                if (!m_bStartButton && !m_bIsTeachDlg) 
                {
                    m_bStartButton = true;
                    m_bStopButton = false;
                    OnStartButton();
                }
            }
            else
            {
                m_bStartButton = false;
            }


            if (MSystem.m_pDIO.IsOn(IOMap.IN["IN_FRONT_OP_BOX_RESET_SW"]))
                MSystem.m_pDIO.OutPutOn(IOMap.OUT["OUT_FRONT_OP_RESET_SW_LAMP"]);
            else
                MSystem.m_pDIO.OutPutOff(IOMap.OUT["OUT_FRONT_OP_RESET_SW_LAMP"]);

            if (isStart[Constants.left] == true)
            {
                m_pDIO.OutPutOn(IOMap.OUT["OUT_LEFT_JIG_START_SW_L"]);
                m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_JIG_STOP_SW_L"]);
            }
            else
            {
                m_pDIO.OutPutOff(IOMap.OUT["OUT_LEFT_JIG_START_SW_L"]);
                m_pDIO.OutPutOn(IOMap.OUT["OUT_LEFT_JIG_STOP_SW_L"]);
            }

            if (isStart[Constants.right] == true)
            {
                m_pDIO.OutPutOn(IOMap.OUT["OUT_RIGHT_JIG_START_SW_R"]);
                m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_JIG_STOP_SW_R"]);
            }
            else
            {
                m_pDIO.OutPutOff(IOMap.OUT["OUT_RIGHT_JIG_START_SW_R"]);
                m_pDIO.OutPutOn(IOMap.OUT["OUT_RIGHT_JIG_STOP_SW_R"]);
            }

            if (MSystem.SysStatus == StatusRun.RUN)
            {
                if (m_pDIO.IsOn(IOMap.IN["IN_LEFT_JIG_START_SW_L"]) == true && InforManager.Instance.IsLeftUnitUse == true && (InforManager.Instance.IsDetectSetUse && m_pDIO.IsOn(IOMap.IN["IN_LEFT_JIG_DETECT_SET_SENS"]) == true|| InforManager.Instance.IsDetectSetUse==false) )
                {
                    isStart[Constants.left] = true;
                    isStartForMemo = true; // add 250705 jlyoon
                }
                else if(m_pDIO.IsOn(IOMap.IN["IN_LEFT_JIG_START_SW_L"]) == true && InforManager.Instance.IsLeftUnitUse == true && InforManager.Instance.IsDetectSetUse && m_pDIO.IsOff(IOMap.IN["IN_LEFT_JIG_DETECT_SET_SENS"]) == true)
                    {
                    MSystem.MyMsgMemo("NOT DETECT SET IN LEFT JIG", "Error", msgButton.OK, msgIcon.Error);
                    return;
                }
                if (m_pDIO.IsOn(IOMap.IN["IN_LEFT_JIG_STOP_SW_L"]) == true)
                    isStart[Constants.left] = false;

                if (m_pDIO.IsOn(IOMap.IN["IN_RIGHT_JIG_START_SW_R"]) == true && InforManager.Instance.IsRightUnitUse == true && (InforManager.Instance.IsDetectSetUse && m_pDIO.IsOn(IOMap.IN["IN_RIGHT_JIG_DETECT_SET_SENS"]) == true|| InforManager.Instance.IsDetectSetUse == false))
                {
                    isStart[Constants.right] = true;
                    isStartForMemo = true; // add 250705 jlyoon
                }
                else if (m_pDIO.IsOn(IOMap.IN["IN_RIGHT_JIG_START_SW_R"]) == true && InforManager.Instance.IsRightUnitUse == true && InforManager.Instance.IsDetectSetUse && m_pDIO.IsOff(IOMap.IN["IN_RIGHT_JIG_DETECT_SET_SENS"]) == true)
                {
                    MSystem.MyMsgMemo("NOT DETECT SET IN RIGHT JIG", "Error", msgButton.OK, msgIcon.Error);
                    return;
                }
                if (m_pDIO.IsOn(IOMap.IN["IN_RIGHT_JIG_STOP_SW_R"]) == true)
                    isStart[Constants.right] = false;
            }
        }
        
        public void OnStartButton()
        {
            m_pLogSave.ErrorStopTimeLogSave("Start,,");
            if (IsDetectDoorOpen(out string m) == true)
            {
                //MSystem.MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                MSystem.MySafetyAlarm();
                return;
            }

            if (MSystem.IsOriginAll() == false)
            {
                MSystem.MyMsgMemo("NO ORIGIN!!", "Error", msgButton.OK, msgIcon.Error);
                return;
            }

            if (MSystem.SysStatus == StatusRun.RUN)
                return;

            //Reset All Timer for all other unit
            UnitTimerRestart();
          
            //DOOR OPEN CHECK
            
            /******************************************************************/
            if (m_bIsTeachDlg) {
                return;
            }
            /******************************************************************/

            m_pLogSave.DevLogSave($"{MSystem.SysStatus} --> {StatusRun.RUN}!");

            /******************************************************************/
            MSystem.m_bInitWorkFlag = false;
            m_pLogSave.AddTail(LogIndex.eLogGEIM, $"{9000}");

            UpdateStatusJig9020();

            MSystem.SysStatus = StatusRun.RUN;
            Tacttime.RunTimer();
            //for(int i=0; i< 2; i++)
            //{
            //    ushort[] temp = MSystem.m_pHantas[i].MbReadHoldingRegister(0x01, 0x02, 7);
            //    if (temp != null && temp.Length == 7)
            //    {
            //        double read = (double)temp[0] / 100.0;
            //        if (read != InforTeaching.Instance.Handtas_Touque[i])
            //        {
            //            ushort[] data = new ushort[1];
            //            data[0] = (ushort)Convert.ToUInt16(InforTeaching.Instance.Handtas_Touque[i] * 100);
            //            //if (MSystem.m_pHantas[i].MbWriteRegister(0x01, 0x02, data))
            //            int _data = (int)Convert.ToInt32(InforTeaching.Instance.Handtas_Touque[i] * 100);
            //            if (MSystem.m_pHantas[i].setTorpueData(_data))
            //            {
            //                MSystem.m_pTrsScrew[i].DriverRun(1);
            //                Thread.Sleep(100);
            //                MSystem.m_pTrsScrew[i].DriverStop();
            //            }
            //            else
            //            { 
            //                MSystem.SysStatus = StatusRun.STOP;
            //                MSystem.m_pTrsBuzzer.SetBuzzerPattern(0);
            //                MSystem.MyMsgMemo($"Hantas {i} Set Touque Fail","Error", msgButton.OK, msgIcon.Error);
                            
            //                break;
            //            }
            //        }
            //    }
            //}
        }
        public void OnStopButton()
        {
            m_pLogSave.ErrorStopTimeLogSave("Stop,,");
            if (MSystem.SysStatus == StatusRun.STOP) return;

            UpdateStatusJig9020();

            m_pLogSave.AddTail(LogIndex.eLogGEIM, $"{9001}");
            MSystem.SysStatus = StatusRun.STOP;
            isStart[Constants.left] = false;
            isStart[Constants.right] = false;
            
            m_pLogSave.DevLogSave($"{MSystem.SysStatus} --> {StatusRun.STOP}");

            Tacttime.PauseTimer();
            m_pTrsJig[0].m_MTestTimer.PauseTimer();
            m_pTrsJig[1].m_MTestTimer.PauseTimer();

            m_pTrsJig[0].m_MWaitTimer.PauseTimer();
            m_pTrsJig[1].m_MWaitTimer.PauseTimer();
        }
        public void UpdateStatusJig9020()
        {
            // Screw Setup
            if (!InforManager.Instance.IsLeftUnitUse)
                m_pLogSave.AddTail(LogIndex.eLogGEIM9020, $"{PortStatus.STATUS}*{_PStatusDetail.NOTUSE}*{_PResult.MAX}*1");
            else
                m_pLogSave.AddTail(LogIndex.eLogGEIM9020, $"{PortStatus.STATUS}*{_PStatusDetail.NORMAL}*{_PResult.MAX}*1");
            if (!InforManager.Instance.IsRightUnitUse)
                m_pLogSave.AddTail(LogIndex.eLogGEIM9020, $"{PortStatus.STATUS}*{_PStatusDetail.NOTUSE}*{_PResult.MAX}*2");
            else
                m_pLogSave.AddTail(LogIndex.eLogGEIM9020, $"{PortStatus.STATUS}*{_PStatusDetail.NORMAL}*{_PResult.MAX}*2");

        }
        public void OnResetButton()
        {
            MSystem.m_pTrsBuzzer.StopAllBuzzers();
            
            if ( MSystem.SysStatus != StatusRun.RUN )
            {
                MSystem.m_pTrsAutoManager.DoorUnlock();
                m_pLogSave.AddTail(LogIndex.eDeviceLog, $"{MSystem.SysStatus} --> Reset Press");
            }
        }
        public void UnitTimerRestart()
        {
            for (int i = 0; i < 2; i++) 
            {
                m_pTrsJig[i].m_iTimer.StartTimer();
                m_pTrsScrew[i].m_iTimer.StartTimer();

                m_pTrsJig[i].m_MTestTimer.RunTimer();
            }
        }
    }
}

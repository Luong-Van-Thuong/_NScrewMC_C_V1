using System.Threading;

namespace _NScrewMC_C_V1
{
    public class MTrLamp : MSystem
    {
        public TimerDelay _Timcheck9200 = new TimerDelay();
        int[] m_iPrev_TowerLamp = { 0, 0, 0 };
        private void TowerLamp_GEIM()
        {
            if (InforManager.Instance.GeimUse)
            {
                //GREEN---------------------------------------
                if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_TOWER_LAMP_GREEN"]))
                {
                    m_iPrev_TowerLamp[0] = 1;
                }
                else
                {
                    m_iPrev_TowerLamp[0] = 0;
                }
                //YELLOW---------------------------------------
                if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_TOWER_LAMP_YELLOW"]))
                {
                    m_iPrev_TowerLamp[1] = 1;
                }
                else
                {
                    m_iPrev_TowerLamp[1] = 0;

                }
                //RED----------------------------------------
                if (MSystem.m_pDIO.IsOn(IOMap.OUT["OUT_TOWER_LAMP_RED"]))
                {
                    m_iPrev_TowerLamp[2] = 1;
                }
                else
                {
                    m_iPrev_TowerLamp[2] = 0;
                }
                if (m_iPrev_TowerLamp[0] != MSystem.m_iTowerLampGeim[0] ||
                    m_iPrev_TowerLamp[1] != MSystem.m_iTowerLampGeim[1] ||
                    m_iPrev_TowerLamp[2] != MSystem.m_iTowerLampGeim[2])
                {
                    MSystem.m_iTowerLampGeim[0] = m_iPrev_TowerLamp[0];
                    MSystem.m_iTowerLampGeim[1] = m_iPrev_TowerLamp[1];
                    MSystem.m_iTowerLampGeim[2] = m_iPrev_TowerLamp[2];
                    MSystem._LampState[0] = $"{MSystem.m_iTowerLampGeim[0]}{MSystem.m_iTowerLampGeim[1]}{MSystem.m_iTowerLampGeim[2]}";

                    if (MSystem._LampState[0] != MSystem._LampState[1])
                    {
                        MSystem._LampState[1] = MSystem._LampState[0];
                        MSystem.m_pLogSave.AddTail(LogIndex.eLogGEIM, $"{9009}");
                        //Thread.Sleep(1000);
                        //m_pTrsLogGEIM.MMMSLogSave(0, 9009, "010");
                    }
                }
            }
        }
        public void Lamp_Poll()
        {
            while (true)
            {
                if (_Timcheck9200.MoreThan(60))
                {
                    _Timcheck9200.StartTimer();
                    m_pLogSave.AddTail(LogIndex.eLogGEIM, "9200");
                }
                TowerLamp_GEIM();
                switch (SysStatus)
                {
                    case StatusRun.RUN:
                        if (IsWaitInput() && SysMode != ModeRun.DryRun)
                        {
                            LoadlampStop();
                            break;
                        }

                        LampRun();
                        break;

                    case StatusRun.STOP:
                        if (IsDetectDoorOpen(out _) == true)
                            LoadlampError();
                        else
                            LoadlampStop();
                        break;

                    case StatusRun.ERROR:
                        LoadlampError();
                        break;
                    case StatusRun.OPRATOR_CALL:
                        LoadlampOperator();
                        break;
                    default: break;
                }
                Thread.Sleep(50);
            }
        }
        public void LampRun()
        {
            m_pDIO.OutPutOff(IOMap.OUT["OUT_TOWER_LAMP_RED"]);
            m_pDIO.OutPutOff(IOMap.OUT["OUT_TOWER_LAMP_YELLOW"]);
            m_pDIO.OutPutOn(IOMap.OUT["OUT_TOWER_LAMP_GREEN"]);
        }

        public void LoadlampStop()
        {
            m_pDIO.OutPutOff(IOMap.OUT["OUT_TOWER_LAMP_RED"]);
            m_pDIO.OutPutOn(IOMap.OUT["OUT_TOWER_LAMP_YELLOW"]);
            m_pDIO.OutPutOff(IOMap.OUT["OUT_TOWER_LAMP_GREEN"]);
        }
        public void LoadlampError()
        {
            m_pDIO.OutPutOn(IOMap.OUT["OUT_TOWER_LAMP_RED"]);
            m_pDIO.OutPutOff(IOMap.OUT["OUT_TOWER_LAMP_YELLOW"]);
            m_pDIO.OutPutOff(IOMap.OUT["OUT_TOWER_LAMP_GREEN"]);
        }
        public void LoadlampOperator()
        {
            m_pDIO.OutPutOn(IOMap.OUT["OUT_TOWER_LAMP_RED"]);
            m_pDIO.OutPutOn(IOMap.OUT["OUT_TOWER_LAMP_YELLOW"]);
            m_pDIO.OutPutOff(IOMap.OUT["OUT_TOWER_LAMP_GREEN"]);
        }
        public bool IsWaitInput()
        {
            return m_pTrsJig[0].IsOverTime(60.0) && m_pTrsJig[1].IsOverTime(60.0) &&
                   m_pTrsScrew[0].IsOverTime(60.0) && m_pTrsScrew[1].IsOverTime(60.0);
        }
    }
}

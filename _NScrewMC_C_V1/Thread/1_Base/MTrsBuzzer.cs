using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;

namespace _NScrewMC_C_V1
{
    struct BuzzerData
    {
        public long dwOnTime;
        public long dwOffTime;
    };
    public class MTrsBuzzer : MSystem
    {
        public int m_iCurrentStep = 0;
        TimerDelay m_iTimer = new TimerDelay();
        List<BuzzerData> m_listBuzzer = new List<BuzzerData>();
        BuzzerData m_bzCurrent = new BuzzerData();

        #region //Class Init
        public MTrsBuzzer()
        {

        }
        ~MTrsBuzzer()
        {

        }
        #endregion
       
        public async void ProcessBuzzer()
        {
            while (true)
            {
                switch (m_iCurrentStep)
                {
                    case 0:
                        //BuzzerOff(); Delete 20251121 cnz
                        lock (LockBuzzer)
                        {
                            if (m_listBuzzer.Count > 0)
                            {
                                m_bzCurrent = m_listBuzzer[0];
                                m_listBuzzer.RemoveAt(0);
                                SetStep(100);
                                break;
                            }
                        }
                        break;
                    case 100: // Buzzer On
                        BuzzerOn();
                        if (m_iTimer.MoreThan(m_bzCurrent.dwOnTime / 1000.0))
                        {
                            SetStep(200);
                            break;
                        }
                        break;
                    case 200: // Buzzer Off
                        BuzzerOff();
                        if (m_iTimer.MoreThan(m_bzCurrent.dwOffTime / 1000.0))
                        {
                            SetStep(300);
                            break;
                        }
                        break;
                    case 300:
                        SetStep(0);
                        break;
                }
                await Task.Delay(25);
                Thread.Sleep(15);
            }
        }
        public void StopAllBuzzers()
        {
            lock (LockBuzzer) {
                SetStep(0);
                m_listBuzzer.Clear();
                BuzzerOff();
            }
        }
        public void BuzzerOn()
        {
            MSystem.m_pDIO.OutPutOn(IOMap.OUT["OUT_BUZZER"]);
        }
        public void BuzzerOff()
        {
            MSystem.m_pDIO.OutPutOff(IOMap.OUT["OUT_BUZZER"]);
        }
        public void SetStep(int _step)
        {
            m_iTimer.StartTimer();
            m_iCurrentStep = _step;
        }
        public void SetBuzzer(long dwOnTime, long dwOffTime)
        {
            lock (LockBuzzer)
            {
                BuzzerData data;
                data.dwOnTime = dwOnTime;
                data.dwOffTime = dwOffTime;
                m_listBuzzer.Add(data);
            }
        }
        public void SetBuzzerPattern(int iPattern)
        {
            switch (iPattern)
            {
                case 0: // 1회.
                    SetBuzzer((long)(InforManager.Instance.TimeBuzze*1000), 1000); // 3s on, 1s off.
                    break;
                case 1:
                    SetBuzzer(10000, 1000);//On Allway time until Reset
                    break;
            }
        }
    }
}

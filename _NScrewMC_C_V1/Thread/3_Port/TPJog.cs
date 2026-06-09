using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace _NScrewMC_C_V1
{
    public class TPJog : MSystem
    {
        TimerDelay Timcheck = new TimerDelay();
        public const int Minus = 0;
        public const int Plus = 1;

        public int _JogMode = 0;
        public int _idtmp = 0;
        public int _Dir = 0;

        //Rear
        public bool FlagX1 = false;
        public bool FlagX11 = false;
        public bool FlagY1 = false ;
        public bool FlagY11 = false;
        public bool FlagZ1 = false;
        public bool FlagZ11 = false;
        public bool FlagJ1 = false;
        public bool FlagJ11 = false;
        //Front
        public bool FlagX2 = false;
        public bool FlagX22 = false;
        public bool FlagY2 = false;
        public bool FlagY22 = false;
        public bool FlagZ2 = false;
        public bool FlagZ22 = false;
        public bool FlagJ2 = false;
        public bool FlagJ22 = false;

        //Flag Stop
        public bool FlagStop = false;
        public bool FlagInterface = false;
        //Flag Run
        public bool[] FlagRunR = new bool[(int)Axis.eAXIS_MAX];
        public bool[] FlagRunF = new bool[(int)Axis.eAXIS_MAX];
        //flag Dir 
        public bool[] _indexDir = new bool[(int)Axis.eAXIS_MAX];
        public bool[] _CMDRun = new bool[(int)Axis.eAXIS_MAX];

        public SerialHelper Serial { get; set; } = null;
        public string NameMC = "";
        public TPJog(string PortName) {
            NameMC = PortName;
            InforManager.Instance.LoadSetting();
            Serial = new SerialHelper(InforManager.Instance.JogPort, 115200, 1, PortName);
            var TP_Port = Task.Factory.StartNew(() => dorunStep());
            var JOG_POLL = Task.Factory.StartNew(() => JogPoll());
        }

        async void dorunStep()
        {
            while (true)
            {
                try 
                {
                    if (Serial.IsOpen && Serial.BytesToRead != 0)
                    {
                        Serial._index = 0;
                        int ReciveCount = 0;
                        if (Serial.BytesToRead != 0)
                        {
                            ReciveCount = await Serial.BaseStream.ReadAsync(Serial.buffer, Serial._index, Serial.buffer.Length);
                            Serial._index += ReciveCount;
                            if (Serial.buffer[0] == 0x02 && Serial.buffer[Serial._index - 1] == 0x03)
                            {
                                Serial.ReceivedByte = new byte[Serial._index];
                                Array.Copy(Serial.buffer, 0, Serial.ReceivedByte, 0, Serial._index);
                                AnalyzeCommData(Serial.ReceivedByte);
                                Serial._index = 0;
                            }
                        }
                        else //Check Time out
                        {
                            Thread.Sleep(1);
                            Serial.TimeRetry++;
                            if (Serial.TimeRetry > 10)
                            {
                                Serial._index = 0;
                            }
                        }
                    }
                }
                catch (IOException ex1)
                {
                    ex1.ToString();
                }
                catch (Exception ex) 
                {
                    ex.ToString();
                }
                
                Thread.Sleep(5);
            }
        }
        void JogPoll() 
        {
            while (true)
            {
                try 
                {
                    if (MSystem.SysStatus != StatusRun.RUN)
                    {
                        if (GetFlagJog())
                        {
                            for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
                            {
                                if (_CMDRun[i])
                                {
                                    if (!FlagRunR[i])
                                    {
                                        FlagRunR[i] = true;
                                        if (!MSystem.SIMULATION)
                                        {
                                            JogAxis((Axis)i, _indexDir[i]);
                                        }
                                    }
                                }
                            }
                            if (FlagStop)
                            {
                                FlagStop = false;
                                for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
                                {
                                    _CMDRun[i] = false;
                                    _indexDir[i] = false;
                                    FlagRunR[i] = false;
                                }
                                m_pTrsJog.FlagX1 = m_pTrsJog.FlagY1 = m_pTrsJog.FlagZ1 = m_pTrsJog.FlagJ1 = false;
                                m_pTrsJog.FlagX2 = m_pTrsJog.FlagY2 = m_pTrsJog.FlagZ2 = m_pTrsJog.FlagJ2 = false;
                                m_pTrsJog.FlagX11 = m_pTrsJog.FlagY11 = m_pTrsJog.FlagZ11 = m_pTrsJog.FlagJ11 = false;
                                m_pTrsJog.FlagX22 = m_pTrsJog.FlagY22 = m_pTrsJog.FlagZ22 = m_pTrsJog.FlagJ22 = false;
                            }

                            if (FlagInterface && Timcheck.MoreThan(0.2) && !MSystem.m_bOrigin)
                            {
                                FlagInterface = false;
                                FlagStop = true;
                                for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
                                {
                                    _CMDRun[i] = false;
                                    _indexDir[i] = false;
                                    FlagRunR[i] = false;
                                }
                                MSystem.SetAllStop();
                            }
                        }
                        else
                        {
                            if (FlagInterface && Timcheck.MoreThan(0.2) && !MSystem.m_bOrigin)
                            {
                                FlagInterface = false;
                                FlagStop = true;
                                for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
                                {
                                    _CMDRun[i] = false;
                                    _indexDir[i] = false;
                                    FlagRunR[i] = false;
                                }
                                MSystem.SetAllStop();
                            }
                            /*************************************************/
                            if (FlagStop)
                            {
                                FlagStop = false;
                                for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
                                {
                                    _CMDRun[i] = false;
                                    _indexDir[i] = false;
                                    FlagRunR[i] = false;
                                }
                                MSystem.SetAllStop();
                            }
                            FlagStop = false;
                        }
                    }
                }
                catch(Exception ex) 
                {
                    ex.ToString();
                }
                
                Thread.Sleep(15);
            }
        }
        void AnalyzeCommData(byte[] _DataAnalyzeCom) {
            if (MSystem.SysStatus == StatusRun.RUN) return;
            FlagInterface = true;
            Timcheck.StartTimer();
            int _data = (_DataAnalyzeCom[2] << 8)&0xFF00 | _DataAnalyzeCom[3]&0x00FF;
            switch (_DataAnalyzeCom[1]) {
                case 0x00:
                    if(!MSystem.m_bOrigin)
                        SelectAxisJog(_data);
                    break;
                case 0x0F:
                    if (_data == 0x00)
                        MSystem.m_pTrsJog._JogMode = 0;
                    else if (_data == 0x01)
                        MSystem.m_pTrsJog._JogMode = 1;
                    else if (_data == 0x02)
                        MSystem.m_pTrsJog._JogMode = 2;
                    break;
                default: break;
            }
        }
        void SelectAxisJog(int _Data) {
            switch (_Data) {
                case 0x00://Stop All
                    FlagInterface = false;
                    if (MSystem.SysStatus != StatusRun.RUN && !MSystem.m_bOrigin)
                        FlagStop = true;

                    m_pTrsJog.FlagX1 = m_pTrsJog.FlagY1 = m_pTrsJog.FlagZ1 = m_pTrsJog.FlagJ1 = false;
                    m_pTrsJog.FlagX2 = m_pTrsJog.FlagY2 = m_pTrsJog.FlagZ2 = m_pTrsJog.FlagJ2 = false;
                    m_pTrsJog.FlagX11 = m_pTrsJog.FlagY11 = m_pTrsJog.FlagZ11 = m_pTrsJog.FlagJ11 = false;
                    m_pTrsJog.FlagX22 = m_pTrsJog.FlagY22 = m_pTrsJog.FlagZ22 = m_pTrsJog.FlagJ22 = false;
                    break;

                //Rear
                case 0x01: // X1-Left
                    FlagX11 = true;
                   // FlagStop = false;
                    break;
                case 0x02: //X1-Right
                    FlagX1 = true;
                   // FlagStop = false;
                    break;
                case 0x08: //Y1-Down
                    //FlagY1 = true;
                   // FlagStop = false;
                    break;
                case 0x04://Y1-Up
                    //FlagY11 = true;
                   // FlagStop = false;
                    break;
                case 0x20: //Z1-Down
                    FlagZ11 = true;
                 //   FlagStop = false;
                    break;
                case 0x10://Z1-Up
                    FlagZ1 = true;
                 //   FlagStop = false;
                    break;
                case 0x80://J1-Down
                    FlagY1 = true;
                    //FlagJ11 = true;
                   // FlagStop = false;
                    break;
                case 0x40://J1-Up
                    FlagY11 = true;
                    //FlagJ1 = true;
                  //  FlagStop = false;
                    break;

                //Front
                case 0x0100: // X2-Left
                    FlagX22 = true;
                //    FlagStop = false;
                    break;
                case 0x0200: //X2-Right
                    FlagX2 = true;
                 //   FlagStop = false;
                    break;
                case 0x0800: //Y2-Down
                    //FlagY2 = true;
                //    FlagStop = false;
                    break;
                case 0x0400://Y2-Up
                    //FlagY22 = true;
                //   FlagStop = false;
                    break;
                case 0x2000: //Z2-Down
                    FlagZ22 = true;
                 //   FlagStop = false;
                    break;
                case 0x1000://Z2-Up
                    FlagZ2 = true;
                //    FlagStop = false;
                    break;
                case 0x8000://J2-Down
                    FlagY2 = true;
                     //   FlagJ22 = true;
                    //    FlagStop = false;
                    break;
                case 0x4000://J2-Up
                    FlagY22 = true;
                    //FlagJ2 = true;
                    //    FlagStop = false;
                    break;
                default:break;
            }
        }
        public void JogAxis(Axis _Axis,bool _Dir)
        {
            switch (MSystem.m_pTrsJog._JogMode)
            {
                case 0:
                    MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)_Axis].JogMoveSlow(_Dir);
                    break;
                case 1:
                    MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)_Axis].JogMoveMid(_Dir);
                    break;
                case 2:
                    MSystem.m_pAxisManager.m_pMmcEtherCatAxis[(int)_Axis].JogMoveFast(_Dir);
                    break;
                default: break;
            }
        }
        public void ResetAllFlag()
        {
            for (int i = 0; i < 2; i++)
            {
                //Rear
                if (FlagX1) FlagX1 = false;
                if (FlagY1) FlagY1 = false;
                if (FlagZ1) FlagZ1 = false;
                if (FlagJ1) FlagJ1 = false;

                //Front
                if (FlagX2) FlagX2 = false;
                if (FlagY2) FlagY2 = false;
                if (FlagZ2) FlagZ2 = false;
                if (FlagJ2) FlagJ2 = false;
            }
        }
        public bool GetFlagJog() 
        {
            return (FlagX1 || FlagY1 || FlagZ1 || FlagJ1 || FlagX2 || FlagY2 || FlagZ2 || FlagJ2 ||
                    FlagX11 || FlagY11 || FlagZ11 || FlagJ11 || FlagX22 || FlagY22 || FlagZ22 || FlagJ22);
        }
    }
}

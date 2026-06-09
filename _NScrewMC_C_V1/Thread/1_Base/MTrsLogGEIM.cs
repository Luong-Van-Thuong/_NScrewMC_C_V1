using System;
using System.IO;
using System.Threading;

namespace _NScrewMC_C_V1
{
    public enum PortStatus
    {
        STATUS = 1,
        NONE,
        START,
        RESULT,
        MAX
    };
    public enum _PStatusDetail
    {
        NORMAL,
        NOTUSE,
        BLOCK,
        MAX

    };
    public enum _PResult
    {
        PASS,
        ERROR,
        FAIL,
        USER_RESET,
        MAX
    };
    public class MTrsLogGEIM : MSystem
    {
        #region //CLASS INITIAL
        public static readonly object LockEDMSave = new object();
        string _TOPID = "TOPMxx";
        private static TextWriter GEIMLog;
        private static TextWriter GEIMLog_D1;
        private static TextWriter GEIMLog_C1;

        public MTrsLogGEIM()
        {

        }

        ~MTrsLogGEIM()
        {

        }
        #endregion

        #region //Function GEIM LOG
        public void MMMSLogSave(int index, int iErrCode, string _StateLam = "")
        {
            lock (LockEDMSave)
            {
                if (!InforManager.Instance.GeimUse) return;
                #region //Variable
                string Errcode, Time, JigUseNumber, JigNotUseNumber, JigTotalNumber, RobotVer;
                string JigBlockNumber, JigPackNGNumber;
                string TotalCount, OKCount, NGCount, TimeOutPut;

                int iTotalCount = 0, iOKCount = 0, iNGCount = 0;
                int iJigMax = 2;
                int iSetExist = 0;
                #endregion

                iTotalCount = (int)(InforProduct.Instance.Total_product);

                Errcode = $",{iErrCode.ToString("d4")}";
                Time = $",{DateTime.Now.ToString("yyyyMMddHHmmss")}{DateTime.Now.Millisecond / 100}";

                int sumUse = 0, sumNotUse = 0, sumBlock = 0, sumPackNG = 0;

                iSetExist++;
                JigBlockNumber = $"{sumBlock.ToString("d2")}"; ;
                JigPackNGNumber = $"{sumPackNG.ToString("d2")}"; ;
                JigNotUseNumber = $"{sumNotUse.ToString("d2")}"; ;
                JigUseNumber = $"{sumUse.ToString("d2")}"; ;
                JigTotalNumber = $"{iJigMax.ToString("d2")}"; ;

                RobotVer = Program.Version;
                RobotVer = RobotVer.Replace(" ", "_");

                TotalCount = $",{iTotalCount.ToString()}";
                OKCount = $",{InforProduct.Instance.ProductPass}";
                NGCount = $",{InforProduct.Instance.ProductNG}";

                string pathname2 = $"{Config.PathLogGEIM}{Time}{Errcode},{JigPackNGNumber}{JigBlockNumber}{JigNotUseNumber}{JigTotalNumber}";
                if (iErrCode == 9009)
                {
                    if (_StateLam != "" && _StateLam == "100")
                    {
                        if (MSystem._LampState[1] == "100" || InforManager.Instance._LastLampState == "100") return;
                        MSystem._LampState[1] = "100";
                        InforManager.Instance._LastLampState = "100";
                        pathname2 += $",{RobotVer},0,100,,,.txt";

                    }
                    else if (_StateLam != "" && _StateLam == "010")
                    {
                        if (MSystem._LampState[1] == "010" || InforManager.Instance._LastLampState == "010") return;
                        MSystem._LampState[1] = "010";
                        InforManager.Instance._LastLampState = "010";
                        pathname2 += $",{RobotVer},0,010,,,.txt";
                    }
                    else
                    {
                        string TowerLamp = $"{MSystem._LampState[1]}";

                        if (TowerLamp == InforManager.Instance._LastLampState) return;
                        InforManager.Instance._LastLampState = TowerLamp;
                        pathname2 += $",{RobotVer},0,{InforManager.Instance._LastLampState},,,.txt";
                    }
                    InforManager.Instance.SaveData();
                }
                else if (iErrCode == 9003)
                {
                    pathname2 += $",{RobotVer},00,0,{_TOPID},,.txt";
                }
                else if (iErrCode == 9200)
                {
                    pathname2 += $",{RobotVer},,{iSetExist},,,.txt";
                }
                else
                {
                    pathname2 += $",{RobotVer},0,0,{Program.ModelName},,.txt";
                }
                if (!Directory.Exists(Config.PathLogGEIM))
                {
                    Directory.CreateDirectory(Config.PathLogGEIM);
                }

                using (GEIMLog = new StreamWriter(new FileStream(pathname2, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    GEIMLog.Close();
                }
                /*IP Mode Wait*/
            }

        }
        public void MMMSLogSave9020(string _strData)
        {
            lock (LockEDMSave)
            {
                PortStatus _Status = PortStatus.MAX;
                _PStatusDetail _STDeTail = _PStatusDetail.MAX;
                _PResult _RS = _PResult.MAX;
                int _JigNum = 0;

                try
                {
                    string[] _Data = _strData.Split('*');
                    /************************************/
                    if (_Data[3] == "1")
                        _JigNum = 1;
                    else if (_Data[3] == "2")
                        _JigNum = 2;
                    else
                        return;
                    /************************************/
                    if (_Data[0] == "STATUS")
                        _Status = PortStatus.STATUS;
                    else if (_Data[0] == "START")
                        _Status = PortStatus.START;
                    else if (_Data[0] == "RESULT")
                        _Status = PortStatus.RESULT;
                    /************************************/
                    if (_Data[1] == "NORMAL")
                        _STDeTail = _PStatusDetail.NORMAL;
                    else if (_Data[1] == "NOTUSE")
                        _STDeTail = _PStatusDetail.NOTUSE;
                    else if (_Data[1] == "BLOCK")
                        _STDeTail = _PStatusDetail.BLOCK;
                    /************************************/
                    if (_Data[2] == "PASS")
                        _RS = _PResult.PASS;
                    else if (_Data[2] == "FAIL")
                        _RS = _PResult.FAIL;
                    else if (_Data[2] == "ERROR")
                        _RS = _PResult.ERROR;
                    else if (_Data[2] == "USER_RESET" || _Data[2] == "MAX")
                        _RS = _PResult.USER_RESET;

                    /************************************/

                    if (!InforManager.Instance.GeimUse) return;

                    #region //Variable
                    string Errcode, Time, JigUseNumber, JigNotUseNumber, JigTotalNumber, RobotVer;
                    string JigBlockNumber, JigPackNGNumber;
                    string TotalCount, OKCount, NGCount, TimeOutPut;

                    int iTotalCount = 0, iOKCount = 0, iNGCount = 0;
                    int iJigMax = 2;
                    #endregion

                    iTotalCount = (int)(InforProduct.Instance.Total_product);

                    Errcode = $",{"9020"}";
                    Time = $",{DateTime.Now.ToString("yyyyMMddHHmmss")}{DateTime.Now.Millisecond / 100}";

                    int sumUse = 0, sumNotUse = 0, sumBlock = 0, sumPackNG = 0;

                    JigBlockNumber = $"{sumBlock.ToString("d2")}"; ;
                    JigPackNGNumber = $"{sumPackNG.ToString("d2")}"; ;
                    JigNotUseNumber = $"{sumNotUse.ToString("d2")}"; ;
                    JigUseNumber = $"{sumUse.ToString("d2")}"; ;
                    JigTotalNumber = $"{iJigMax.ToString("d2")}"; ;

                    RobotVer = Program.Version;
                    RobotVer = RobotVer.Replace(" ", "_");

                    TotalCount = $",{iTotalCount.ToString()}";
                    OKCount = $",{InforProduct.Instance.ProductPass}";
                    NGCount = $",{InforProduct.Instance.ProductNG}";

                    string pathname2 = $"{Config.PathLogGEIM}{Time}{Errcode},{JigPackNGNumber}{JigBlockNumber}{JigNotUseNumber}{JigTotalNumber},{(int)_Status},{_JigNum.ToString("d2")}";
                    switch (_Status)
                    {
                        case PortStatus.STATUS:
                            pathname2 += $",{(int)_STDeTail},{_TOPID},,";
                            break;
                        case PortStatus.START:
                            pathname2 += $",,{_TOPID},,";
                            break;
                        case PortStatus.RESULT:
                            pathname2 += $",{(int)_RS},{_TOPID},{InforManager.Instance.ModelName},";
                            break;
                    }
                    if (!Directory.Exists(Config.PathLogGEIM))
                    {
                        Directory.CreateDirectory(Config.PathLogGEIM);
                    }
                    pathname2 += ".txt";
                    using (GEIMLog = new StreamWriter(new FileStream(pathname2, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                    {
                        GEIMLog.Close();
                    }
                }
                catch (Exception)
                {
                    MyMessagerBottom("MMMSLogSave Unit Fail Logic");
                    Thread.Sleep(1000);
                }

                /*IP Mode Wait*/
            }

        }
        public void MMMSLogSaveModelChange(int code)
        {
            lock (LockEDMSave)
            {
                if (!InforManager.Instance.GeimUse) return;
                /*
                 - 9005 : Start change model 
                 - 9006 : Model end change
                 */
                string Time = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{DateTime.Now.Millisecond / 100}";
                string RobotVer = Program.Version.Replace(" ", "_");
                string CodeMsg = code.ToString("d4");

                string pathname2 = $"{Config.PathLogGEIM_C1},{Time},{CodeMsg},00000001,{RobotVer},,,,,,.txt";

                if (!Directory.Exists(Config.PathLogGEIM_C1))
                {
                    Directory.CreateDirectory(Config.PathLogGEIM_C1);
                }

                using (GEIMLog_C1 = new StreamWriter(new FileStream(pathname2, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    GEIMLog_C1.Close();
                }
            }

        }
        public void MMMSLogSave(int iErrCode)
        {
            MMMSLogSave(0, iErrCode);
        }
        public void MMMSLogSaveD1(int count)
        {
            lock (LockEDMSave)
            {
                if (!InforManager.Instance.GeimUse) return;
                string Time = $"{DateTime.Now.ToString("yyyyMMddHHmmss")}{DateTime.Now.Millisecond / 100}";
                string RobotVer = Program.Version.Replace(" ", "_");

                long ok = InforProduct.Instance.ProductGMES_Pass;
                long ng = InforProduct.Instance.ProductNG;
                long tact = (int)MSystem.GetLassTactime();
                long input = count + ok + ng;

                string pathname2 = $"{Config.PathLogGEIM_D1},{Time},9200,00000000,{RobotVer},{input},{ok},{ng},{tact},,,,,,,.txt";
                lock (LockLog)
                {
                    if (!Directory.Exists(Config.PathLogGEIM_D1))
                    {
                        Directory.CreateDirectory(Config.PathLogGEIM_D1);
                    }

                    using (GEIMLog_D1 = new StreamWriter(new FileStream(pathname2, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                    {
                        GEIMLog_D1.Close();
                    }
                }
            }

        }
        #endregion
    }
}

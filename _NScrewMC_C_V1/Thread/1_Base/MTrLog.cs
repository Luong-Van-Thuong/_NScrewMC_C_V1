using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace _NScrewMC_C_V1
{
    public enum LogIndex { 
        eErrorLog,
        eDeviceLog,
        eDataSave,
        eDGSLogStart,
        eDGSLogEnd,
        eLogGEIM,
        eLogGEIM_D1,
        eLogGMESSave,
        eLogGEIM9020,
        eLogScrew1,
        eLogScrew2,
        eLogScrew3,
        eLogScrewNetwork,
        eLogErrStopTime,
        eLogHantasLeft,
        eLogHantasRight,
        eLogBarcodeResultL,
        eLogBarcodeResultR,
        eMax
    };
    public class MTrLog : MSystem
    {
        public const string Error    = "0";
        public const string Device  = "1";
        public const string DataSave    = "2";
        public const string LogDGSSaveStart = "3";
        public const string LogDGSSaveEnd = "4";
        public const string LogGEIM = "5";
        public const string LogGEIM_D1 = "6";
        public const string LogGMES = "7";
        public const string LogGEIM9020 = "8";
        public const string LogScrew1 = "9";
        public const string LogScrew2 = "10";
        public const string LogScrew3 = "11";
        public const string LogScrewNetwork = "12";
        public const string LogErrorStopTime = "13";
        public const string LogHantasLeft = "14";
        public const string LogHantasRight = "15";
        public const string LogBarcodeResultL = "16";
        public const string LogBarcodeResultR = "17";

        public Queue<string> strLog = new Queue<string>();
        string[] strLogSave = new string[2];

        private static TextWriter DeviceLog;
        private static TextWriter ErrorLog;
        private static TextWriter DataLog;
        private static TextWriter GMESLog;
        private static TextWriter DataLogScrew1;
        private static TextWriter DataLogScrew2;
        private static TextWriter DataLogScrew3;
        private static TextWriter DataLogScrewNetwork;
        private static TextWriter LogErrStopTime;
        private static TextWriter DataLogHantasLeft;
        private static TextWriter DataLogHantasRight;
        private static TextWriter DataLogBarcodeResultL;
        private static TextWriter DataLogBarcodeResultR;

        /*****************************************/
        public  string FilePathLogDevice = "";
        public  string FilePathErrorLog = "";
        public  string FilePathDataLog = "";
        public string FilePathGMESLog = "";
        public string FilePathLogScrew1 = "";
        public string FilePathLogScrew2 = "";
        public string FilePathLogScrew3 = "";
        public string FilePathLogScrewNetwork = "";
        public string FilePathLogErrStopTime = "";
        public string FilePathLogHantasLeft = "";
        public string FilePathLogHantasRight = "";

        #region //Initial Class 
        public MTrLog() {

            lock (MSystem.LockLog) {

                //Check Folder exits
                if (!Directory.Exists(Config.LogDevice))   Directory.CreateDirectory(Config.LogDevice);
                if (!Directory.Exists(Config.LogError))    Directory.CreateDirectory(Config.LogError);
                if (!Directory.Exists(Config.LogDataSave)) Directory.CreateDirectory(Config.LogDataSave);
                if (!Directory.Exists(Config.LogGMES)) Directory.CreateDirectory(Config.LogGMES);
                // Log Screw
                if (!Directory.Exists(Config.LogScrew1)) Directory.CreateDirectory(Config.LogScrew1);
                if (!Directory.Exists(Config.LogScrew2)) Directory.CreateDirectory(Config.LogScrew2);
                if (!Directory.Exists(Config.LogHantasLeft)) Directory.CreateDirectory(Config.LogHantasLeft);
                if (!Directory.Exists(Config.LogHantasRight)) Directory.CreateDirectory(Config.LogHantasRight);

                //if (!Directory.Exists(Config.LogScrew3)) Directory.CreateDirectory(Config.LogScrew3);
                try
                {
                    if (!Directory.Exists(Config.LogScrewPathNetwork))
                        Directory.CreateDirectory(Config.LogScrewPathNetwork);
                }
                catch {
                    MyMessagerBottom("Network: Created Log File Screw Fail");
                }
                //create file name
                FilePathLogDevice = Config.LogDevice + DateTime.Now.ToString("yyyyMMdd") + "_" + "Devlog.txt";
                FilePathErrorLog = Config.LogError + DateTime.Now.ToString("yyyyMMdd") + "_" + "Error.txt";
                FilePathDataLog = Config.LogDataSave + DateTime.Now.ToString("yyyyMMdd") + "_" + "DataSave.txt";
                FilePathGMESLog = Config.LogGMES + DateTime.Now.ToString("yyyyMMdd") + "_" + "GMES.txt";

                //Create file 
                using (DeviceLog = new StreamWriter(new FileStream(FilePathLogDevice, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))) {
                    DeviceLog.Close();
                }
                using (ErrorLog = new StreamWriter(new FileStream(FilePathErrorLog, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))) {
                    DeviceLog.Close();
                }
                using (DataLog = new StreamWriter(new FileStream(FilePathDataLog, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))) {
                    DeviceLog.Close();
                }
                using (GMESLog = new StreamWriter(new FileStream(FilePathGMESLog, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    GMESLog.Close();
                }
            }
        }
#endregion
        public void LogprocessData() {
            while (true) {
                try
                {
                    lock (LockMessagerError)
                    {
                        if (strLog.Count > 0)
                        {
                            strLogSave = strLog.Dequeue().Trim().Split(',');
                            switch (strLogSave[0])
                            {
                                case Error:
                                    ErrorLogSave(strLogSave);
                                    break;
                                case Device:
                                    DevLogSave(strLogSave[1]);
                                    break;
                                case DataSave:
                                    DatalogSave(strLogSave[1]);
                                    break;
                                case LogDGSSaveStart:
                                    //m_pTrsLoader.AddStartDGSLog();
                                    break;
                                case LogDGSSaveEnd:
                                    //m_pTrsLoader.AddLogDGSResult();
                                    break;

                                case LogGEIM:
                                    m_pTrsLogGEIM.MMMSLogSave(int.Parse(strLogSave[1]));
                                    break;
                                case LogGEIM9020:
                                    m_pTrsLogGEIM.MMMSLogSave9020(strLogSave[1]);
                                    break;
                                case LogGEIM_D1:
                                    m_pTrsLogGEIM.MMMSLogSaveD1(0);
                                    break;
                                case LogGMES:
                                    GMESLogSave(strLogSave[1]);
                                    break;

                                // Log Screw
                                case LogScrew1:
                                    Srew1_LogSave(strLogSave[1]);
                                    break;
                                case LogScrew2:
                                    Srew2_LogSave(strLogSave[1]);
                                    break;
                                case LogScrewNetwork:
                                    NetworkScrewLog(strLogSave[1]);
                                    break;
                                case LogErrorStopTime:
                                    break;
                                case LogHantasLeft:
                                    HantasLeft_LogSave(strLogSave[1]);
                                    break;
                                case LogHantasRight:
                                    HantasRight_LogSave(strLogSave[1]);
                                    break;
                                case LogBarcodeResultL:
                                    BarcodeResultL_LogSave(strLogSave[1]);
                                    break;
                                case LogBarcodeResultR:
                                    BarcodeResultR_LogSave(strLogSave[1]);

                                    break;

                                default: break;
                            }
                        }
                    }
                }
                finally {
                    Thread.Sleep(200);
                }
                
            }
        }
        public void GMESLogSave(string _strlog)
        {
            string _strtmp = $"{DateTime.Now.ToString("yyyy_HH:mm:ss")} {_strlog}";

            lock (LockLog)
            {
                using (GMESLog = new StreamWriter(new FileStream(FilePathGMESLog, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    GMESLog.WriteLine(_strtmp);
                    GMESLog.Flush();
                    GMESLog.Close();
                }
            }
            MSystem.MyMessagerBottom(_strlog);
        }
        public void AddTail(LogIndex? _LogType, string _strlog) {
            lock (LockMessagerError) {
                switch (_LogType) {
                    case LogIndex.eErrorLog:
                        _strlog = "0," + _strlog;
                        break;
                    case LogIndex.eDeviceLog:
                        _strlog = "1," + _strlog;
                        break;
                    case LogIndex.eDataSave:
                        _strlog = "2," + _strlog;
                        break;
                    case LogIndex.eDGSLogStart:
                        _strlog = "3," + _strlog;
                        break;
                    case LogIndex.eDGSLogEnd:
                        _strlog = "4," + _strlog;
                        break;
                    case LogIndex.eLogGEIM:
                        _strlog = "5," + _strlog;
                        break;
                    case LogIndex.eLogGEIM_D1:
                        _strlog = "6," + _strlog;
                        break;
                    case LogIndex.eLogGMESSave:
                        _strlog = "7," + _strlog;
                        break;
                    case LogIndex.eLogGEIM9020:
                        _strlog = "8," + _strlog;
                        break;
                    // Log Screw
                    case LogIndex.eLogScrew1:
                        _strlog = "9," + _strlog;
                        break;
                    case LogIndex.eLogScrew2:
                        _strlog = "10," + _strlog;
                        break;
                    case LogIndex.eLogScrew3:
                        _strlog = "11," + _strlog;
                        break;
                    case LogIndex.eLogScrewNetwork:
                        _strlog = "12," + _strlog;
                        break;
                    case LogIndex.eLogHantasLeft:
                        _strlog = "14," + _strlog;
                        break;
                    case LogIndex.eLogHantasRight:
                        _strlog = "15," + _strlog;
                        break;
                    case LogIndex.eLogBarcodeResultL:
                        _strlog = "16," + _strlog;
                        break;
                    case LogIndex.eLogBarcodeResultR:
                        _strlog = "17," + _strlog;
                        break;


                    default: break;
                }
                strLog.Enqueue(_strlog); 
            }
        }
        public void ErrorStopTimeLogSave(string _strlog)
        {
            string _strtmp = $" {_strlog}{DateTime.Now.ToString("HH:mm:ss")}";
            FilePathLogErrStopTime = Config.LogErrStopTime + "Error Stop Time_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (!Directory.Exists(Config.LogErrStopTime))
                Directory.CreateDirectory(Config.LogErrStopTime);
            if (!File.Exists(FilePathLogErrStopTime))
            {
                using (LogErrStopTime = new StreamWriter(new FileStream(FilePathLogErrStopTime, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    LogErrStopTime.Close();
                }
            }
            lock (LockLog)
            {
                try
                {
                    using (LogErrStopTime = new StreamWriter(new FileStream(FilePathLogErrStopTime, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                    {
                        LogErrStopTime.WriteLine(_strtmp);
                        LogErrStopTime.Flush();
                        LogErrStopTime.Close();
                    }
                }
                catch (Exception)
                {
                    MyMsgMemo("Please close file Error Log before run Program", "Error Log Save");
                }
            }
            MSystem.MyMessagerBottom($"[{DateTime.Now.ToString("yyyy-HH:mm:ss")}] : {_strlog[1]}");
        }
        public void GMESlogSave(string _strlog)
        {
            try
            {
                string _strtmp = $"{DateTime.Now.ToString("yyyy_HH:mm:ss")} {_strlog}";
                lock (LockLog)
                {
                    if (!Directory.Exists(Config.LogGMESSave)) Directory.CreateDirectory(Config.LogGMESSave);
                    FilePathGMESLog = Config.LogGMESSave + DateTime.Now.ToString("yyyyMMdd") + "_" + "GMES.txt";
                    using (DataLog = new StreamWriter(new FileStream(FilePathGMESLog, FileMode.Append, FileAccess.Write, FileShare.Read)))
                    {
                        DataLog.WriteLine(_strtmp);
                        DataLog.Flush();
                        DataLog.Close();
                    }
                }
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("DMC Middle Save Log Fail");
            }

            MSystem.MyMessagerBottom(_strlog);
        }
        public void DevLogSave(string _strlog)
        {
            string _strtmp = $"{DateTime.Now.ToString("yyyy_HH:mm:ss")} {_strlog}";
            
            lock(LockLog)
            {
                using (DeviceLog = new StreamWriter(new FileStream(FilePathLogDevice, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    DeviceLog.WriteLine(_strtmp);
                    DeviceLog.Flush();
                    DeviceLog.Close();
                }
            }
            MSystem.MyMessagerBottom(_strlog);
        }
        public void ErrorLogSave(string[] _strlog)
        {
            lock (LockLog)
            {
                string[] _strtmp = new string[3];
                _strtmp[0] = _strlog[2];
                _strtmp[1] = $"{DateTime.Now.ToString("yyyyMMdd_HH:mm:ss")}";
                _strtmp[2] = _strlog[1];
               
                MSystem.m_pDatabaseLog.InsertData(_strtmp);

                using (ErrorLog = new StreamWriter(new FileStream(FilePathErrorLog, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    ErrorLog.WriteLine($"{_strtmp[0]} {_strtmp[1]} - {_strtmp[2]}");
                    ErrorLog.Flush();
                    ErrorLog.Close();
                }
            }
            MSystem.MyMessagerBottom($"[{DateTime.Now.ToString("yyyy-HH:mm:ss")}] : {_strlog[1]}");
        }
        public void DatalogSave(string _strlog)
        {
            string _strtmp = $"{DateTime.Now.ToString("yyyy_HH:mm:ss")} {_strlog}";
            lock (LockLog)
            {
                using (DataLog = new StreamWriter(new FileStream(FilePathDataLog, FileMode.Append, FileAccess.Write, FileShare.ReadWrite)))
                {
                    DataLog.WriteLine(_strtmp);
                    DataLog.Flush();
                    DataLog.Close();
                }
            }
            MSystem.MyMessagerBottom(_strlog);
        }

        public void NetworkScrewLog(string _strlog)
        {
            try
            {
                //string _strtmp = $"{DateTime.Now.ToString("HH:mm:ss.fff")} {_strlog}";
                //string[] str_anlize = _strlog.Trim().Split(':');
                lock (LockLog)
                {
                    if (!Directory.Exists(Config.LogScrewPathNetwork)) Directory.CreateDirectory(Config.LogScrewPathNetwork);
                    FilePathLogScrewNetwork = Config.LogScrewPathNetwork + DateTime.Now.ToString("yyyyMMdd") + "_" + "ScrewLog.txt";

                    using (DataLogScrewNetwork = new StreamWriter(new FileStream(FilePathLogScrewNetwork, FileMode.Append, FileAccess.Write, FileShare.Read)))
                    {
                        DataLogScrewNetwork.WriteLine(_strlog);
                        DataLogScrewNetwork.Flush();
                        DataLogScrewNetwork.Close();
                    }
                }
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("Screw Network Save Log Fail");
            }
        }


        public void Srew1_LogSave(string _strlog)
        {
            try
            {
                // string _strtmp = $"{DateTime.Now.ToString("HH:mm:ss.fff")} {_strlog}";
                string[] str_anlize = _strlog.Trim().Split('-');
                lock (LockLog)
                {
                    if (!Directory.Exists(Config.LogScrew1)) Directory.CreateDirectory(Config.LogScrew1);
                    FilePathLogScrew1 = Config.LogScrew1 + str_anlize[0] + ".txt";
                    using (DataLogScrew1 = new StreamWriter(new FileStream(FilePathLogScrew1, FileMode.Append, FileAccess.Write, FileShare.Read)))
                    {
                        DataLogScrew1.WriteLine(str_anlize[1]);
                        DataLogScrew1.Flush();
                        DataLogScrew1.Close();
                    }
                }
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("Screw 1 Save Log Fail");
            }
        }
        public void Srew2_LogSave(string _strlog)
        {
            try
            {
                //string _strtmp = $"{DateTime.Now.ToString("HH:mm:ss.fff")} {_strlog}";
                string[] str_anlize = _strlog.Trim().Split('-');
                lock (LockLog)
                {
                    if (!Directory.Exists(Config.LogScrew2)) Directory.CreateDirectory(Config.LogScrew2);
                    FilePathLogScrew2 = Config.LogScrew2 + str_anlize[0] + ".txt";
                    using (DataLogScrew2 = new StreamWriter(new FileStream(FilePathLogScrew2, FileMode.Append, FileAccess.Write, FileShare.Read)))
                    {
                        DataLogScrew2.WriteLine(str_anlize[1]);
                        DataLogScrew2.Flush();
                        DataLogScrew2.Close();
                    }
                }
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("Screw 2 Save Log Fail");
            }
        }
        //public void HantasLeft_LogSave(string _strlog)
        //{
        //    try
        //    {
        //        // string _strtmp = $"{DateTime.Now.ToString("HH:mm:ss.fff")} {_strlog}";
        //        string[] str_anlize = _strlog.Trim().Split('-');
        //        lock (LockLog)
        //        {
        //            if (!Directory.Exists(Config.LogHantasLeft)) Directory.CreateDirectory(Config.LogHantasLeft);
        //            FilePathLogHantasLeft = Config.LogHantasLeft + str_anlize[0] + ".txt";
        //            using (DataLogHantasLeft = new StreamWriter(new FileStream(FilePathLogHantasLeft, FileMode.Append, FileAccess.Write, FileShare.Read)))
        //            {
        //                DataLogHantasLeft.WriteLine(str_anlize[1]);

        //                DataLogHantasLeft.Flush();
        //                DataLogHantasLeft.Close();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MSystem.MyMessagerBottom("Hantas Left Save Log Fail");
        //    }
        //}

        public void HantasLeft_LogSave(string _strlog)
        {
            try
            {
                lock (LockLog)
                {
                    if (!Directory.Exists(Config.LogHantasLeft))
                        Directory.CreateDirectory(Config.LogHantasLeft);

                    
                    string fileName = $"HantasLeft_{DateTime.Now:yyyyMMdd}.txt";
                    FilePathLogHantasLeft = Config.LogHantasLeft + fileName;

                    using (DataLogHantasLeft = new StreamWriter(new FileStream(FilePathLogHantasLeft, FileMode.Append, FileAccess.Write, FileShare.Read)))
                    {
                        
                        DataLogHantasLeft.Write(_strlog);
                        DataLogHantasLeft.Flush();
                        DataLogHantasLeft.Close();
                    }
                }
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("Hantas Left Save Log Fail");
            }
        }

        public void HantasRight_LogSave(string _strlog)
        {
            try
            {
                lock (LockLog)
                {
                    if (!Directory.Exists(Config.LogHantasRight))
                        Directory.CreateDirectory(Config.LogHantasRight);

                    
                    string fileName = $"HantasRight_{DateTime.Now:yyyyMMdd}.txt";
                    FilePathLogHantasRight = Config.LogHantasRight + fileName;

                    using (DataLogHantasRight = new StreamWriter(new FileStream(FilePathLogHantasRight, FileMode.Append, FileAccess.Write, FileShare.Read)))
                    {
                        
                        DataLogHantasRight.Write(_strlog);
                        DataLogHantasRight.Flush();
                        DataLogHantasRight.Close();
                    }
                }
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("Hantas Right Save Log Fail");
            }
        }
        public void BarcodeResultL_LogSave(string _strlog)
        {
            try
            {
                lock (LockLog)
                {
                    if (!Directory.Exists(Config.LogBarcodeResult))
                        Directory.CreateDirectory(Config.LogBarcodeResult);

                    string fileName = $"Left_{DateTime.Now:yyyyMMdd}.txt";
                    string Path = Config.LogBarcodeResult + fileName;
                    _strlog += "\r\n";
                    using (DataLogBarcodeResultL = new StreamWriter(new FileStream(Path, FileMode.Append, FileAccess.Write, FileShare.Read)))
                    {
                        DataLogBarcodeResultL.Write(_strlog);
                        DataLogBarcodeResultL.Flush();
                        DataLogBarcodeResultL.Close();
                    }
                }
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("Barcode Result Left Save Log Fail");
            }
        }
        public void BarcodeResultR_LogSave(string _strlog)
        {
            try
            {
                lock (LockLog)
                {
                    if (!Directory.Exists(Config.LogBarcodeResult))
                        Directory.CreateDirectory(Config.LogBarcodeResult);

                    string fileName = $"Right_{DateTime.Now:yyyyMMdd}.txt";
                    string Path = Config.LogBarcodeResult + fileName;

                    using (DataLogBarcodeResultR = new StreamWriter(new FileStream(Path, FileMode.Append, FileAccess.Write, FileShare.Read)))
                    {
                        DataLogBarcodeResultR.Write(_strlog);
                        DataLogBarcodeResultR.Flush();
                        DataLogBarcodeResultR.Close();
                    }
                }
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("Barcode Result Right Save Log Fail");
            }
        }
        //public void HantasRight_LogSave(string _strlog)
        //{
        //    try
        //    {
        //        // string _strtmp = $"{DateTime.Now.ToString("HH:mm:ss.fff")} {_strlog}";
        //        string[] str_anlize = _strlog.Trim().Split('-');
        //        lock (LockLog)
        //        {
        //            if (!Directory.Exists(Config.LogHantasRight)) Directory.CreateDirectory(Config.LogHantasRight);
        //            FilePathLogHantasRight = Config.LogHantasRight + str_anlize[0] + ".txt";
        //            using (DataLogHantasRight = new StreamWriter(new FileStream(FilePathLogHantasRight, FileMode.Append, FileAccess.Write, FileShare.Read)))
        //            {
        //                DataLogHantasRight.WriteLine(str_anlize[1]);

        //                DataLogHantasRight.Flush();
        //                DataLogHantasRight.Close();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MSystem.MyMessagerBottom("Hantas Right  Save Log Fail");
        //    }
        //}
    }
}

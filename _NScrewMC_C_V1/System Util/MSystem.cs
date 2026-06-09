using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Runtime.InteropServices;
using _NScrewMC_C_V1.System_Util;


namespace _NScrewMC_C_V1
{
    public class MSystem
    {
        public static Thread _mRun;

        public const int eLEFT = 0;
        //public const int eMID = 1;
        public const int eRIGHT = 1;

        public const int eREAR = 0;
        public const int eFRONT = 1;
        
        public const bool ON = true;
        public const bool OFF = false;
        public const bool START = true;
        public const bool STOP = false;

        public const int eJIG_LEFT = 0;
        public const int eJIG_RIGHT = 1;

        public const int NONE = -1;
        public const int NO_MC = -1;
        public const int NG = 0;
        public const int PASS = 1;

        public const int MMC_OK = 0;

        public const bool MLine = false;
        public const bool MCircle = true;

        public const bool MPoint = false;
        public const bool MContinue = true;

        public static int m_iJigNG = 0;

        public static bool IsDoorSkip { get; set; } = false;
        public static DateTime DoorSkipStartTime { get; set; } = new DateTime();
        public static bool IsVisionCheckDoor { get; set; } = false;
        public static bool IsVisionCheckSSSafety { get; set; } = false;

        public static bool IsConnectScanner { get; set; } = true;

        public static int[] m_iTowerLampGeim = { 0, 0, 0 };
        public static string[] _LampState = { "100", "100" };

        public static bool[] _Rework = {false, false};
        public static int[] m_ScrewTestResult = new int[100]; 
        #region Var for Vision
        public static bool m_bLiveCam = false;

        public static bool m_bManual_Align = false;
        public static bool m_bVisionTestDemo = false;

        public static VisionInterface vinterface = new VisionInterface();
        #endregion

        #region // ini file read
        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(string section,
                                                           string key,
                                                           string defaultValue, //키값이 없을 때의 기본 값
                                                           StringBuilder returnedString,
                                                           uint size,
                                                           string filePath);
        #endregion

        ~MSystem()
        {

        }

        #region //TP Jog 

        /*Mtrs Jog*/
        public static TPJog m_pTrsJog = new TPJog("TPJog");
        public static ModbusIO[] m_pHantas = { new ModbusIO(0), new ModbusIO(1) };
        public static Barcode[] m_pBcr = { new Barcode(0), new Barcode(1) };
        public static HttpRestClient[] HttpRestClient = {new HttpRestClient(), new HttpRestClient() };
        #endregion

        #region IO Manager
        public static SystemIO m_pDIO = new SystemIO();
        #endregion

        #region Status PGM
        public static ModeRun SysMode = new ModeRun();
        public static StatusRun SysStatus = new StatusRun();
        public static bool[] isStart = new bool[2] { false, false };
        public static bool isStartForMemo = false;

        public static void SetOPStatus(StatusRun _Status)
        {
            SysStatus = _Status;
        }

        #endregion

        #region //Variable for run
        public static bool SIMULATION = false;
        public static bool TEST_MC = true;
        public static bool SCAN_LOCATION_CV = false;

        public static bool m_bInitWorkFlag = true;
        public static bool m_bErrorOccur = false;
        public static bool m_bLife = true;
        public static bool m_bOrigin = false;

        public static bool m_bInputProduct = false;
        public static bool m_bOutputProduct = false;

        public static bool m_bButtonStop = false;
        public static bool m_bIsDoorOpen = false;
        public static bool m_bIsDockingFail = false;

        public static bool m_bIsUnitInitDone = false;

        // Add more
        public static readonly object LockReqMoving = new object();
        public static bool m_bIsInputAutoMode = true;

        public static bool m_IsSite = false; // VH = true , KR = false
        #endregion

        #region //Data SystemManager
        //public static DataManager m_pInforManager = new DataManager();
        #endregion

        #region //Variable Lock Thread and Task
        //Thread
        public static bool m_bInitManual = false;
        public static readonly object LockGEIM = new object();

        public readonly object LockSendLogScrewData = new object();
        public static readonly object LockTactime = new object();
        public static readonly object LockMessagerError = new object();
        public static readonly object LockLog = new object();
        public static readonly object Lock_DisplayLog = new object();
        public static readonly object LockBuzzer = new object();

        public static readonly object LockButtonMessenger = new object();
        public static readonly object LockInsertAlarm = new object();
        public static readonly object LockAxisManager = new object();
        public static readonly object LockDisplayNGCV = new object();

        public readonly object LockLoaderCheckCBAS = new object();
        public readonly object LockInforNGCV = new object();

        public static readonly object LockMoveContinue = new object();
        public static readonly object LockServo = new object();

        public static long testData = 0;
        #endregion

        #region // Variable List of tact time
        public static List<double> _tactime = new List<double>();
        public static TimerDelay Tacttime = new TimerDelay();

        public static List<double> _TactimeSC1 = new List<double>();
        public static List<double> _TactimeSC2 = new List<double>();
        public static List<double> _TactimeSC3 = new List<double>();
        public static TimerDelay TacttimeSC1 = new TimerDelay();
        public static TimerDelay TacttimeSC2 = new TimerDelay();
        public static TimerDelay TacttimeSC3 = new TimerDelay();
        public static void LoadParameter()
        {
            MSystem.SysStatus = StatusRun.STOP;
            ServoParam.Instance.LoadSetting();
            InforManager.Instance.LoadSetting();
            InforTeaching.Instance.LoadSetting();
            InforProduct.Instance.LoadSetting();
        }

        #endregion

        #region //PRODUCT INFOR
        public static double GetLassTactime()
        {
            int count = _tactime.Count;
            double LasTactime = 0;
            lock (LockTactime)
            {
                count = _tactime.Count;

                if (count > 0)
                {
                    LasTactime = _tactime[count - 1];
                }

            }
            return LasTactime;
        }
        public static void UpdateTactime()
        {
            lock (LockTactime)
            {
                if (!Tacttime._TimStop && Tacttime.GetTime < 100)
                {
                    _tactime.Add(Tacttime.GetTime);
                }
                Tacttime.ResetTimer();
            }
        }


        public static double GetLassTactimeScrew(int _index)
        {
            int count = _tactime.Count;
            double LasTactimeSC = 0;
            lock (LockTactime)
            {
                if (_index == 0)
                {
                    count = _TactimeSC1.Count;

                    if (count > 0)
                    {
                        LasTactimeSC = _TactimeSC1[count - 1];
                    }
                }
                else if (_index == 1)
                {
                    count = _TactimeSC2.Count;

                    if (count > 0)
                    {
                        LasTactimeSC = _TactimeSC2[count - 1];
                    }
                }
                else if (_index == 2)
                {
                    count = _TactimeSC3.Count;

                    if (count > 0)
                    {
                        LasTactimeSC = _TactimeSC3[count - 1];
                    }
                }

            }
            return LasTactimeSC;
        }

        public static void UpdateTactimeSC(int _index)
        {
            lock (LockTactime)
            {
                if (_index == 0)
                {
                    if (!TacttimeSC1._TimStop && TacttimeSC1.GetTime < 100)
                    {
                        _TactimeSC1.Add(TacttimeSC1.GetTime);
                    }

                    //TacttimeSC1.ResetTimer();
                    TacttimeSC1.PauseTimer();
                }
                else if (_index == 1)
                {
                    if (!TacttimeSC2._TimStop && TacttimeSC2.GetTime < 100)
                    {
                        _TactimeSC2.Add(TacttimeSC2.GetTime);
                    }
                    //TacttimeSC2.ResetTimer();
                    TacttimeSC2.PauseTimer();
                }
                else if (_index == 2)
                {
                    if (!TacttimeSC3._TimStop && TacttimeSC3.GetTime < 100)
                    {
                        _TactimeSC3.Add(TacttimeSC3.GetTime);
                    }
                    //TacttimeSC3.ResetTimer();
                    TacttimeSC3.PauseTimer();
                }
            }
        }

        public void ResetTactimeSC(int _index)
        {
            lock (LockTactime)
            {
                if (_index == 0)
                {
                    TacttimeSC1.StartTimer();
                }
                else if (_index == 1)
                {
                    TacttimeSC2.StartTimer();
                }
                else if (_index == 2)
                {
                    TacttimeSC3.StartTimer();
                }
            }
        }

        public static void INC_ProductPass()
        {
            lock (LockTactime)
            {
                UpdateTactime();
                InforProduct.Instance.ProductPass++;
                if (SysMode != ModeRun.DryRun)
                {
                    m_pLogSave.AddTail(LogIndex.eLogGEIM, $"9002");
                    InforProduct.Instance.SaveSettings();
                }
            }
        }
        public static void INC_ProductInput()
        {
            lock (LockTactime)
            {
                InforProduct.Instance.Total_product++;
                if (SysMode != ModeRun.DryRun)
                {
                    m_pLogSave.AddTail(LogIndex.eLogGEIM, $"9004");
                    InforProduct.Instance.SaveSettings();
                }
            }
        }
        public static void INC_ProductNG()
        {
            lock (LockTactime)
            {
                InforProduct.Instance.ProductNG++;
                if (SysMode != ModeRun.DryRun)
                {
                    m_pLogSave.AddTail(LogIndex.eLogGEIM, $"9003");
                    InforProduct.Instance.SaveSettings();
                }
            }
        }
        public static void INC_JigInOutStatus9020(int index, bool isIn, bool result = true)
        {
            if (isIn == true)
            {
                m_pLogSave.AddTail(LogIndex.eLogGEIM9020, $"{PortStatus.START}*{_PStatusDetail.NOTUSE}*{_PResult.MAX}*{index+1}");
            }
            else
            {
                if (result == true)
                    m_pLogSave.AddTail(LogIndex.eLogGEIM9020, $"{PortStatus.RESULT}*{_PStatusDetail.NOTUSE}*{_PResult.PASS}*{index + 1}");
                else
                    m_pLogSave.AddTail(LogIndex.eLogGEIM9020, $"{PortStatus.RESULT}*{_PStatusDetail.NOTUSE}*{_PResult.FAIL}*{index + 1}");
            }
        }
        #region DETAIL PRODUCT INFORM
        public static void INC_ProductGMES_Pass()
        {
            lock (LockTactime)
            {
                InforProduct.Instance.ProductGMES_Pass++;
                if (SysMode != ModeRun.DryRun)
                {
                    InforProduct.Instance.SaveSettings();
                }
            }
        }
        public static void INC_ProductGMES_Fail()
        {
            lock (LockTactime)
            {
                InforProduct.Instance.ProductGMES_Fail++;
                if (SysMode != ModeRun.DryRun)
                {
                    InforProduct.Instance.SaveSettings();
                }
            }
        }
        public static void INC_ProductGMES_Input()
        {
            lock (LockTactime)
            {
                InforProduct.Instance.ProductGMES_Total++;
                if (SysMode != ModeRun.DryRun)
                {
                    InforProduct.Instance.SaveSettings();
                }
            }
        }
        //public static void INC_ProductScrew_Pass(int m_idx)
        //{
        //    lock (LockTactime)
        //    {
        //        InforProduct.Instance.ProductScrew_Pass[m_idx]++;
        //        if (InforManager.Instance.IsAutoAlign)
        //            InforProduct.Instance.ProductScrew_Pass_Vision[m_idx]++;
        //        if (SysMode != ModeRun.DryRun)
        //        {
        //            InforProduct.Instance.SaveSettings();
        //        }
        //    }
        //}
        //public static void INC_ProductScrew_Fail(int m_idx)
        //{
        //    lock (LockTactime)
        //    {
        //        InforProduct.Instance.ProductScrew_Fail[m_idx]++;
        //        if (InforManager.Instance.IsAutoAlign)
        //            InforProduct.Instance.ProductScrew_Fail_Vision[m_idx]++;
        //        if (SysMode != ModeRun.DryRun)
        //        {
        //            InforProduct.Instance.SaveSettings();
        //        }
        //    }
        //}
        //public static void INC_ProductScrew_Input(int m_idx)
        //{
        //    lock (LockTactime)
        //    {
        //        InforProduct.Instance.ProductScrew_Total[m_idx]++;
        //        if (InforManager.Instance.IsAutoAlign)
        //            InforProduct.Instance.ProductScrew_Total_Vision[m_idx]++;
        //        if (SysMode != ModeRun.DryRun)
        //        {
        //            InforProduct.Instance.SaveSettings();
        //        }
        //    }
        //}
        #endregion

        public static void ResetAllProductInfor()
        {
            lock (LockTactime)
            {
                InforProduct.Instance.Total_product = 0;
                InforProduct.Instance.ProductPass = 0;
                InforProduct.Instance.ProductNG = 0;

                InforProduct.Instance.ProductGMES_Total = 0;
                InforProduct.Instance.ProductGMES_Pass = 0;
                InforProduct.Instance.ProductGMES_Fail = 0;
                InforProduct.Instance.ProductBCR_Fail_L = 0;
                InforProduct.Instance.ProductBCR_Fail_R = 0;

                for (int i = 0; i < 2; i++)
                {
                    InforProduct.Instance.ProductScrew_Total[i] = 0;
                    InforProduct.Instance.ProductScrew_Pass[i] = 0;
                    InforProduct.Instance.ProductScrew_Fail[i] = 0;
               
                    InforProduct.Instance.ProductScrew_Total_Vision[i] = 0;
                    InforProduct.Instance.ProductScrew_Pass_Vision[i] = 0;
                    InforProduct.Instance.ProductScrew_Fail_Vision[i] = 0;
                }



                for (int i = 0; i < InforTeaching.Instance.P_Screw[Constants.left].Count; i++)
                {
                    InforProduct.Instance.ProductDetail[MSystem.eLEFT][i].m_dCountFail = 0;
                    InforProduct.Instance.ProductDetail[MSystem.eLEFT][i].m_dCountFailVision = 0;
                    InforProduct.Instance.ProductDetail[MSystem.eLEFT][i].DetailStack.Clear();
                }

                for (int i = 0; i < InforTeaching.Instance.P_Screw[Constants.right].Count; i++)
                {
                    InforProduct.Instance.ProductDetail[MSystem.eRIGHT][i].m_dCountFail = 0;
                    InforProduct.Instance.ProductDetail[MSystem.eRIGHT][i].m_dCountFailVision = 0;
                    InforProduct.Instance.ProductDetail[MSystem.eRIGHT][i].DetailStack.Clear();
                }

                InforTeaching.Instance.SaveSettings();
                InforProduct.Instance.SaveSettings();
            }
        }
        public static void Reset_DetailInForm()
        {
            lock (LockTactime)
            {
                //InforProduct.Instance.BCR_Pass = 0;
                //InforProduct.Instance.BCR_Fail = 0;
                //InforProduct.Instance.BCR_Rate = 0;

                //InforProduct.Instance.JSON_Pass = 0;
                //InforProduct.Instance.JSON_Fail = 0;
                //InforProduct.Instance.JSON_Rate = 0;

                //InforProduct.Instance.DMCTEST_Pass = 0;
                //InforProduct.Instance.DMCTEST_Fail = 0;
                //InforProduct.Instance.DMCTEST_Rate = 0;

                InforProduct.Instance.SaveSettings();
            }
        }
        public static void ResetInforJig(int _idx)
        {
            lock (LockTactime)
            {
                InforProduct.Instance.ProductScrew_Total[_idx] = 0;
                InforProduct.Instance.ProductScrew_Pass[_idx] = 0;
                InforProduct.Instance.ProductScrew_Fail[_idx] = 0;
                InforProduct.Instance.ProductScrew_Total_Vision[_idx] = 0;
                InforProduct.Instance.ProductScrew_Pass_Vision[_idx] = 0;
                InforProduct.Instance.ProductScrew_Fail_Vision[_idx] = 0;
                InforProduct.Instance.ProductBCR_Fail_L = 0;
                InforProduct.Instance.ProductBCR_Fail_R = 0;
                InforProduct.Instance.SaveSettings();

                for (int i = 0; i < InforTeaching.Instance.P_Screw[_idx].Count; i++)
                {
                    InforProduct.Instance.ProductDetail[_idx][i].m_dCountFail = 0;
                        InforProduct.Instance.ProductDetail[_idx][i].m_dCountFailVision = 0;
                    InforProduct.Instance.ProductDetail[_idx][i].DetailStack.Clear();
                }
                InforTeaching.Instance.SaveSettings();//test delete saving
                InforProduct.Instance.SaveSettings();
            }
        }

        public static void ResetInforGMES()
        {
            lock (LockTactime)
            {
                InforProduct.Instance.ProductGMES_Total = 0;
                InforProduct.Instance.ProductGMES_Pass = 0;
                InforProduct.Instance.ProductGMES_Fail = 0;
                InforProduct.Instance.SaveSettings();
            }
        }
        #endregion

        #region //Display LOG
        public static CtrListView m_plisView = new CtrListView();
        public static CtrListViewScrew m_pListScrew = new CtrListViewScrew();
        public static MTrLog m_pLogSave = new MTrLog();
        #endregion

        #region Variable dlg display
        public static bool m_bIsTeachDlg = false;
        #endregion

        #region Flag Unit Working

        public static bool[] m_bIsWorkingShuttle = { false, false };
        public static bool m_bIsWorkingLoader = false;
        public static bool m_bIsWorkingUnLoader = false;

        public static bool[] m_bIsWorkingJigDMC = { false, false, false };
        public static bool m_bIsWorkingOutOK_CV = false;
        public static bool m_bIsWorkingOutNG_CV = false;

        public static bool IsOriginInitFlag = false;
        public static bool IsAutoInitillize = false;
        public static bool IsInitillizeStatus = false;
        public static bool IsOriginStatus = false;

        #endregion

        #region INITIAL ALL FORM
        /***********************************************/
        public static int NumDisplay = 1, NumDisplayOld = 0;
        public static FormTitle TitleForm = new FormTitle();
        public static FormBottom BottomForm = new FormBottom();
        public static FormAuto AutoForm = new FormAuto();
        public static FormLog LogForm = new FormLog();
        public static FormTeach TeachForm = new FormTeach();
        public static FormData DataForm = new FormData();
        /***********************************************/
        #endregion

        // Form Manual 1
        #region //UNIT variable
        public static MTrsScrew[] m_pTrsScrew = { new MTrsScrew(Constants.left), new MTrsScrew(Constants.right) };
        public static MTrsJig[] m_pTrsJig = { new MTrsJig(Constants.left), new MTrsJig(Constants.right) };
        #endregion

        #region //Main thread Base Thread 
        public static MTrsBuzzer m_pTrsBuzzer = new MTrsBuzzer();

        public static MTrsAutoManager m_pTrsAutoManager = new MTrsAutoManager();
        public static MTrsLogGEIM m_pTrsLogGEIM = new MTrsLogGEIM();
        public static MTrLamp m_pTrsLamp = new MTrLamp();
        public static MTrsOP m_pTrsOP = new MTrsOP();
        public static Alarm m_pAlarm = new Alarm();

        #endregion

        public static void RunAllThread()
        {
            /**********************************************************************************/
            var MtrLog = Task.Factory.StartNew(() => m_pLogSave.LogprocessData());
            var Buzzer = Task.Factory.StartNew(() => m_pTrsBuzzer.ProcessBuzzer());
            var Mtrsauto = Task.Factory.StartNew(() => m_pTrsAutoManager.dorunStep());
            var MtrOP = Task.Factory.StartNew(() => m_pTrsOP.dorunStep());
            var _MTrslamp = Task.Factory.StartNew(() => m_pTrsLamp.Lamp_Poll());
            /**********************************************************************************/

        }

        #region //DataTable for Log
        public static string TableLog = "_NscrewMC_C_V1_LogTB";
        public static SQLiteYJ m_pDatabaseLog = new SQLiteYJ(TableLog, Config.DataLogError);
        #endregion

        #region // Initial Main Form
        public static void DisplayMain(System.Windows.Forms.Panel panel, int num)
        {
            switch (num)
            {
                //Auto form
                case (int)NumViewMain.eViewAuto:
                    {
                        if (AutoForm.InvokeRequired)
                        {
                            AutoForm.Invoke(new Action(() =>
                            {
                                NumDisplay = 0;
                                MSystem.HideAllForm(panel);
                                MSystem.AutoForm.Visible = true;
                                MSystem.checkBorderBottomForm(BottomForm, (int)NumViewMain.eViewAuto);
                            }));
                        }
                        else
                        {
                            NumDisplay = 0;
                            MSystem.HideAllForm(panel);
                            MSystem.AutoForm.Visible = true;
                            MSystem.checkBorderBottomForm(BottomForm, (int)NumViewMain.eViewAuto);
                        }

                    }
                    break;
                //Teach Form
                case (int)NumViewMain.eViewTeach:
                    {
                        if (TeachForm.InvokeRequired)
                        {
                            TeachForm.Invoke(new Action(() =>
                            {
                                NumDisplay = 0;
                                MSystem.HideAllForm(panel);
                                MSystem.TeachForm.Visible = true;
                                MSystem.checkBorderBottomForm(BottomForm, (int)NumViewMain.eViewTeach);
                            }));
                        }
                        else
                        {
                            NumDisplay = 0;
                            MSystem.HideAllForm(panel);
                            MSystem.TeachForm.Visible = true;
                            MSystem.checkBorderBottomForm(BottomForm, (int)NumViewMain.eViewTeach);
                        }
                    }
                    break;
                //Data Form
                case (int)NumViewMain.eViewData:
                    {
                        if (DataForm.InvokeRequired)
                        {
                            DataForm.Invoke(new Action(() =>
                            {
                                NumDisplay = 0;
                                MSystem.HideAllForm(panel);
                                MSystem.DataForm.Visible = true;
                                MSystem.checkBorderBottomForm(BottomForm, (int)NumViewMain.eViewData);
                            }));
                        }
                        else
                        {
                            NumDisplay = 0;
                            MSystem.HideAllForm(panel);
                            MSystem.DataForm.Visible = true;
                            MSystem.checkBorderBottomForm(BottomForm, (int)NumViewMain.eViewData);
                        }
                    }
                    break;
                //Log Form
                case (int)NumViewMain.eViewLog:
                    {
                        if (LogForm.InvokeRequired)
                        {
                            LogForm.Invoke(new Action(() =>
                            {
                                NumDisplay = 0;
                                MSystem.HideAllForm(panel);
                                MSystem.LogForm.Visible = true;
                                MSystem.checkBorderBottomForm(BottomForm, (int)NumViewMain.eViewLog);
                            }));
                        }
                        else
                        {
                            NumDisplay = 0;
                            MSystem.HideAllForm(panel);
                            MSystem.LogForm.Visible = true;
                            MSystem.checkBorderBottomForm(BottomForm, (int)NumViewMain.eViewLog);
                        }

                    }
                    break;

                default:
                    break;
            }
            GC.Collect();
        }
        public static void InitialMainForm(System.Windows.Forms.Panel panel)
        {
            MSystem.AddAllForm(panel);
            MSystem.HideAllForm(panel);
        }
        public static void HideAllForm(System.Windows.Forms.Panel panel)
        {
            foreach (System.Windows.Forms.Form p in panel.Controls)
            {
                p.Visible = false;
            }
        }
        public static void AddAllForm(System.Windows.Forms.Panel panel)
        {
            //Auto Form
            MSystem.AutoForm.TopLevel = false;
            panel.Controls.Add(MSystem.AutoForm);
            //MSystem.AutoForm.Show();
            //LogForm
            MSystem.LogForm.TopLevel = false;
            panel.Controls.Add(MSystem.LogForm);
            //MSystem.LogForm.Show();
            //Teach Form
            MSystem.TeachForm.TopLevel = false;
            panel.Controls.Add(MSystem.TeachForm);
            //MSystem.TeachForm.Show();
            //Data Form
            MSystem.DataForm.TopLevel = false;
            panel.Controls.Add(MSystem.DataForm);
            //MSystem.DataForm.Show();

            GC.Collect();
        }
        #endregion

        #region//Initial Sub form
        public static void DisplayForm(System.Windows.Forms.Panel panel, System.Windows.Forms.Form form)
        {
            if (form == null)
            {
                form = new System.Windows.Forms.Form();
            }
            form.TopLevel = false;
            panel.Controls.Add(form);
            form.Show();
        }

        public static void checkBorderBottomForm(System.Windows.Forms.Form form, int index)
        {
            foreach (var pb in form.Controls.OfType<SUserControls.ColorButton>())
            {
                pb.Checked = false;
            }
            switch (index)
            {
                case 1:
                    {
                        MSystem.BottomForm.BtAuto.Checked = true;
                    }
                    break;
                case 2:
                    {
                        MSystem.BottomForm.BtTeach.Checked = true;
                    }
                    break;
                case 3:
                    {
                        MSystem.BottomForm.BtData.Checked = true;
                    }
                    break;
                case 4:
                    {
                        MSystem.BottomForm.BtExit.Checked = true;
                    }
                    break;

                default: break;
            }
        }
        #endregion

        #region // Messenger Display
        public static void SetError(int _IDERROR, string strName)
        {
            if (MSystem.SysStatus == StatusRun.ERROR) return;
            m_pAlarm.SetErrorMsg(_IDERROR, strName);
        }
        public static void MySafetyAlarm()
        {
            FormSafety dlg = new FormSafety();
            dlg.ShowDialog();
        }
        public static System.Windows.Forms.DialogResult MyMsgMemo(string strmsg, string strTitle, msgButton _bt = msgButton.OK, msgIcon _icon = msgIcon.Infor)
        {
            return MSystem.BottomForm.msgMemo(strmsg, strTitle, _bt, _icon);
        }
        public static void MyMsgDisplay(string strcontent)
        {
            MSystem.BottomForm.msgDisplay(strcontent);
        }
        public static void SetMsgDisplay(string strcontent)
        {
            FMsgDisplay.strMsg = strcontent;
        }
        public static void KillMsgDisplay()
        {
            FMsgDisplay.KillLife = true;
        }

        public static void MyMessagerBottom(string messager)
        {
            messager = DateTime.Now.ToString("yyyy-MM-dd : HH:mm:ss : ") + messager;
            lock (LockButtonMessenger)
            {
                BottomForm.LbHistory.Items.Insert(0, messager);
                if (BottomForm.LbHistory.Items.Count > 200)
                {
                    BottomForm.LbHistory.Items.RemoveAt(200);
                }
            }
        }
        #endregion

        #region // Servo Data
        public static int eAXIS_MAX = (int)Axis.eAXIS_MAX;
        public static sAxisInfo[] sAxisInfo = new sAxisInfo[eAXIS_MAX];
        public static AxisManager m_pAxisManager = new AxisManager();

        #region Function For Servo
        public static bool IsAxisError(Axis _iaxis)
        {
            return m_pAxisManager.m_pMmcEtherCatAxis[(int)_iaxis] != null && m_pAxisManager.m_pMmcEtherCatAxis[(int)_iaxis].GetAmpFault();
        }

        public static bool IsAxisMoveDone(int _iaxis)
        {
            if (m_pAxisManager.m_pMmcEtherCatAxis[_iaxis] != null && m_pAxisManager.m_pMmcEtherCatAxis[_iaxis].IsAxisDone())
                return true;
            else
                return false;
        }

        public static bool IsOriginAll()
        {
            int _MaxAxis = (int)Axis.eAXIS_MAX;
            for (int i = 0; i < _MaxAxis; i++)
            {
                // add 260226 cnz
                if (InforManager.Instance.IsLeftUnitUse == false)
                {
                    if (i == (int)Axis.AXIS_X1 || i == (int)Axis.AXIS_Y1 || i == (int)Axis.AXIS_Z1)
                        continue;
                }
                if (InforManager.Instance.IsRightUnitUse == false)
                {
                    if (i == (int)Axis.AXIS_X2 || i == (int)Axis.AXIS_Y2 || i == (int)Axis.AXIS_Z2)
                        continue;
                }
                if (m_pAxisManager.m_pMmcEtherCatAxis[i] == null || !m_pAxisManager.m_pMmcEtherCatAxis[i].IsOrigin())
                    return false;
            }
            return true;
        }

        public static bool IsAxisOrigin(Axis _iaxis)
        {
            if (m_pAxisManager.m_pMmcEtherCatAxis[(int)_iaxis] == null || !m_pAxisManager.m_pMmcEtherCatAxis[(int)_iaxis].IsOrigin())
                return false;
            else
                return true;
        }
        public static bool IsServoOn(int _Axis)
        {
            return m_pAxisManager.m_pMmcEtherCatAxis[_Axis].GetAmpEnable();
        }

        public static int SetMove(int iAxis, double dPosition)
        {
            int iRtn=0;
            try
            {
                sAxisInfo info = new sAxisInfo();
                MSystem.m_pAxisManager.m_pMmcEtherCatAxis[iAxis].GetAxisInfo(ref info);

                iRtn = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[iAxis].StartMove(dPosition);
            }
            catch (Exception)
            {
                MSystem.MyMessagerBottom("Debug 1");
            }
            return iRtn;
        }

        public static int SetMove(int iAxis, double dPosition, double _Speed)
        {
            int iRtn;
            sAxisInfo info = new _NScrewMC_C_V1.sAxisInfo();
            MSystem.m_pAxisManager.m_pMmcEtherCatAxis[iAxis].GetAxisInfo(ref info);

            // 이동.
            iRtn = MSystem.m_pAxisManager.m_pMmcEtherCatAxis[iAxis].StartMove(dPosition, _Speed); //, info.dAxisAcc, info.dAxisDec);
            return 0;
        }

        public static bool IsAxisPositionCheck(int iAxis, double dPosition)
        {

            if (MSystem.SIMULATION || m_pAxisManager.m_pMmcEtherCatAxis[iAxis] == null)
                return false;

            double dCurPos = m_pAxisManager.m_pMmcEtherCatAxis[iAxis].GetCurrentPos();
            double tar_pos = dPosition;

            if (Math.Abs(dCurPos - tar_pos) < 0.5 && IsAxisMoveDone(iAxis))
                return true;// OK

            return false; // No
        }
        public static bool IsAxisPositionCheck(int iAxis, double dPosition, double _tolerance)
        {

            if (MSystem.SIMULATION)
                return false;

            double dCurPos = m_pAxisManager.m_pMmcEtherCatAxis[iAxis].GetCurrentPos();
            double tar_pos = dPosition;

            if (Math.Abs(dCurPos - tar_pos) < _tolerance && IsAxisMoveDone(iAxis))
                return true;// OK

            return false; // No
        }

        public static int SetMoveLinear(int iAxis1, double dPos1, int iAxis2, double dPos2)
        {
            MSystem.m_pAxisManager.m_pMmcEtherCatAxis[iAxis1].StartMove(dPos1);
            MSystem.m_pAxisManager.m_pMmcEtherCatAxis[iAxis2].StartMove(dPos2);
            return 0;
        }

        public static bool waitServoInitOk()
        {
            int delayTime = 0;
            bool KillServo = true;
            bool _Result = true;
            Task.Run(async () =>
            {
                await Task.Delay(10);
                while (delayTime <= 50)
                {
                    SetMsgDisplay($"Start Init Servor for Origin {delayTime * 100}");
                    delayTime++;
                    KillServo = true;
                    for (int i = 0; i < (int)Axis.eAXIS_MAX; i++)
                    {
                        if (m_pAxisManager.m_pMmcEtherCatAxis[0] == null)
                        {
                            KillServo = false;
                            _Result = false;
                        }
                    }
                    if (KillServo)
                    {
                        KillMsgDisplay();
                        break;
                    }
                    Thread.Sleep(100);
                }
                KillMsgDisplay();
            });
            MyMsgDisplay("Start Init Servor for Origin ......");
            KillMsgDisplay();
            return _Result;
        }
        public static double GetCurrentPos(int iAxis)
        {

            if (m_pAxisManager.m_pMmcEtherCatAxis[iAxis] == null) return -1;
            double dCurPos = m_pAxisManager.m_pMmcEtherCatAxis[iAxis].GetCurrentPos();
            return dCurPos;
        }

        public static double GetCurrentPos(Axis iAxis)
        {
            if (m_pAxisManager.m_pMmcEtherCatAxis[(int)iAxis] == null) return -1.123;
            double dCurPos = m_pAxisManager.m_pMmcEtherCatAxis[(int)iAxis].GetCurrentPos();
            return dCurPos;
        }
        public static void MoveStop(Axis _axis)
        {
            try
            {
                if (m_pAxisManager.m_pMmcEtherCatAxis[(int)_axis] == null) return;
                m_pAxisManager.m_pMmcEtherCatAxis[(int)_axis].Stop();
            }
            catch (Exception) { }
        }

        public static void SetAllStop()
        {
            try
            {
                for (int i = 0; i < eAXIS_MAX; i++)
                {
                    if (m_pAxisManager.m_pMmcEtherCatAxis[i] == null)
                        continue;

                    m_pAxisManager.m_pMmcEtherCatAxis[i].Stop();
                }
            }
            catch (Exception ex)
            {
                string temp = ex.ToString();
            }
        }
        #endregion

        #endregion

        #region //CHECK SAFETY
        public static bool CheckSafety()
        {
            if (IsDetectEmergency())
            {
                MyMsgMemo("E-STOP BUTTON PRESSED!!\r\nPleass Check E-STOP Button \r\nand Push Reste Button 2sec", "Error", msgButton.OK, msgIcon.Error);
                return true;
            }

            if (IsDetectDoorOpen(out string m) == true)
            {
                MyMsgMemo(m, "Error", msgButton.OK, msgIcon.Error);
                return true;
            }

            return false;
        }

        public static bool IsDetectEmergency()
        {
            if (m_pDIO.IsOff(IOMap.IN["IN_FRONT_OP_BOX_E_STOP_SW"]) == true)
                return true;

            return false;
        }

        public static bool IsDetectDoorOpen(out string message)
        {
            message = string.Empty;

            if (IsDoorSkip == true)
                return false;

            if (m_pDIO.IsOff(IOMap.IN["IN_DOOR_SENSOR"]) == true)
            {
                if (m_pDIO.IsOff(IOMap.IN["IN_FRONT_LEFT_DOOR_DETECT"]) == true)
                    message = "FRONT DOOR OPEN!!";
                else if (m_pDIO.IsOff(IOMap.IN["IN_FRONT_RIGHT_DOOR_DETECT"]) == true)
                    message = "FRONT DOOR OPEN!!";
                else if (m_pDIO.IsOff(IOMap.IN["IN_REAR_LEFT_DOOR_DETECT"]) == true)
                    message = "REAR DOOR OPEN!!";
                else if (m_pDIO.IsOff(IOMap.IN["IN_REAR_RIGHT_DOOR_DETECT"]) == true)
                    message = "REAR DOOR OPEN!!";
                else if (m_pDIO.IsOff(IOMap.IN["IN_LEFT_DOOR_1_DETECT"]) == true)
                    message = "LEFT DOOR OPEN!!";
                //else if (m_pDIO.IsOff(IO.IN_LEFT_DOOR_2_DETECT) == true)
                //    message = "LEFT DOOR OPEN!!";
                //else if (m_pDIO.IsOff(IO.IN_RIGHT_DOOR_1_DETECT) == true)
                //    message = "RIGHT DOOR OPEN!!";
                else if (m_pDIO.IsOff(IOMap.IN["IN_RIGHT_DOOR_2_DETECT"]) == true)
                    message = "RIGHT DOOR OPEN!!";
                else
                    message = "RESET BUTTON PUSH!!";

                return true;
            }

            return false;
        }

        public static bool IsLightCurtainDetected()
        {
            if (InforManager.Instance.InspectionType == "VST" || InforManager.Instance.InspectionType == "H8")
            {
                if (m_pDIO.IsOff(IOMap.IN["IN_LEFT_LIGHT_CURTAIN_DETECT"]) || m_pDIO.IsOff(IOMap.IN["IN_LEFT_CURTAIN_DETECT_2"])
                    || m_pDIO.IsOff(IOMap.IN["IN_RIGHT_LIGHT_CURTAIN_DETECT"]) || m_pDIO.IsOff(IOMap.IN["IN_RIGHT_CURTAIN_DETECT_2"]))
                    return true;
                else
                    return false;
            }
            else
            {
                if (m_pDIO.IsOff(IOMap.IN["IN_RIGHT_LIGHT_CURTAIN_DETECT"]) || m_pDIO.IsOff(IOMap.IN["IN_LEFT_LIGHT_CURTAIN_DETECT"]))
                    return true;
            }

            return false;
        }

#endregion

        #region //INIT UNIT
        public static int UnitInitAll()
        {
            // INIT
            int _Result = 100;
            var TaskRun = Task.Run(async () =>
            {
                MSystem.SetMsgDisplay($"Init Unit {(Unit.eUNIT_SCREW_1).ToString()} ......");
                if (_Result == 100 && MSystem.m_pTrsScrew[(int)_NSC.eLEFT].SetUnitInitialize() != (int)InitResult.UNIT_INIT_SUCCESS)
                    _Result = 0;

                if (_Result == 100 && MSystem.m_pTrsJig[(int)_NSC.eLEFT].SetUnitInitialize() != (int)InitResult.UNIT_INIT_SUCCESS)
                    _Result = 0;

                MSystem.SetMsgDisplay($"Init Unit {(Unit.eUNIT_SCREW_2).ToString()} ......");
                if (_Result == 100 && MSystem.m_pTrsScrew[(int)_NSC.eRIGHT].SetUnitInitialize() != (int)InitResult.UNIT_INIT_SUCCESS)
                    _Result = 1;

                if (_Result == 100 && MSystem.m_pTrsJig[(int)_NSC.eRIGHT].SetUnitInitialize() != (int)InitResult.UNIT_INIT_SUCCESS)
                    _Result = 1;

                MSystem.KillMsgDisplay();
                await Task.Delay(100);

                if (_Result != 100)
                    MSystem.MyMsgMemo($"Init {(Unit)_Result} Fail!", "Unit Init", msgButton.OK, msgIcon.Infor);
            });
            MSystem.MyMsgDisplay("Start Init Unit ........\r\nWait a moment ...");

            MSystem.KillMsgDisplay();
            return _Result;
        }
        #endregion

        #region //Keyboard get num
        public static bool GetIn(object sender, ref int _data)
        {
            KeyboardNum getValue = new KeyboardNum();
            getValue.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
            DialogResult Result = getValue.ShowDialog();
            if (Result == DialogResult.OK)
            {
                int TEMP = 0;
                if (int.TryParse(getValue.TxValue.Text, out TEMP))
                {
                    (sender as SUserControls.ColorButton).Text = getValue.TxValue.Text;
                    _data = TEMP;
                    return true;
                }
            }
            return false;
        }
        public static bool GetIn2(object sender, ref int _data)
        {
            KeyboardNum getValue = new KeyboardNum();
            getValue.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
            DialogResult Result = getValue.ShowDialog();
            if (Result == DialogResult.OK)
            {
                int TEMP = 0;
                if (int.TryParse(getValue.TxValue.Text, out TEMP))
                {
                    (sender as SUserControls.ColorButton).Text = getValue.TxValue.Text;
                    _data = TEMP;
                    return true;
                }
            }
            return false;
        }

        public static bool GetInLV(int _rows, int _columns, ref int _data, int lmPl = 3)
        {
            KeyboardNum getValue = new KeyboardNum();
            getValue.TxCurrent.Text = MSystem.m_pListScrew.GetOldvalue(_rows, _columns);
            DialogResult Result = getValue.ShowDialog();
            if (Result == DialogResult.OK)
            {
                int TEMP = 0;
                if (int.TryParse(getValue.TxValue.Text, out TEMP))
                {
                    if (TEMP > lmPl || TEMP <= 0) return false;
                    _data = TEMP;
                    return true;
                }
            }
            return false;
        }
        public static bool GetFloat(object sender, ref float _data)
        {
            KeyboardNum getValue = new KeyboardNum();
            getValue.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
            DialogResult Result = getValue.ShowDialog();
            if (Result == DialogResult.OK)
            {
                float TEMP = 0;
                if (float.TryParse(getValue.TxValue.Text, out TEMP))
                {
                    (sender as SUserControls.ColorButton).Text = getValue.TxValue.Text;
                    _data = TEMP;
                    return true;
                }
            }
            return false;
        }

        public static bool GetDoubleLV(int _rows, int _columns, ref double _data, double lmPl = 99999999999999.0)
        {
            KeyboardNum getValue = new KeyboardNum();
            getValue.TxCurrent.Text = MSystem.m_pListScrew.GetOldvalue(_rows, _columns);
            DialogResult Result = getValue.ShowDialog();
            if (Result == DialogResult.OK)
            {
                double TEMP = 0;
                if (double.TryParse(getValue.TxValue.Text, out TEMP))
                {
                    if (TEMP > lmPl) return false;
                    _data = TEMP;
                    return true;
                }
            }
            return false;
        }
        public static bool Getdouble(object sender, ref double _data)
        {
            KeyboardNum getValue = new KeyboardNum();
            getValue.TxCurrent.Text = (sender as SUserControls.ColorButton).Text;
            DialogResult Result = getValue.ShowDialog();
            if (Result == DialogResult.OK)
            {
                double TEMP = 0;
                if (double.TryParse(getValue.TxValue.Text, out TEMP))
                {
                    (sender as SUserControls.ColorButton).Text = TEMP.ToString("f3");
                    _data = TEMP;
                    return true;
                }
            }
            return false;
        }
        public static bool Getdouble2(object sender, ref double _data)
        {
            KeyboardNum getValue = new KeyboardNum();
            getValue.TxCurrent.Text = (sender as AxBTNENHLib4.AxBtnEnh).Caption;
            DialogResult Result = getValue.ShowDialog();
            if (Result == DialogResult.OK)
            {
                double TEMP = 0;
                if (double.TryParse(getValue.TxValue.Text, out TEMP))
                {
                    (sender as AxBTNENHLib4.AxBtnEnh).Caption = TEMP.ToString("f3");
                    _data = TEMP;
                    return true;
                }
            }
            return false;
        }
        public static bool GetCharKeyboard(object sender)
        {
            KeyboardGetValue dlgvalue = new KeyboardGetValue(0);
            dlgvalue.TxValue.Text = (sender as SUserControls.ColorButton).Text;
            DialogResult result = dlgvalue.ShowDialog();
            if (result == DialogResult.OK)
            {
                (sender as SUserControls.ColorButton).Text = dlgvalue.TxValue.Text;
                return true;
            }
            return false;
        }
        public static string GetPass()
        {
            KeyboardGetValue dlgvalue = new KeyboardGetValue(1);
            DialogResult result = dlgvalue.ShowDialog();
            if (result == DialogResult.OK)
            {
                string _tmp = dlgvalue.Keyword.Trim();
                dlgvalue = null;
                return _tmp;
            }
            dlgvalue = null;
            return "";
        }
        #endregion

        #region IO Update Sensor
        static Image IO_ON = Properties.Resources.On;
        static Image IO_OFF = Properties.Resources.Off;
        static Image IO_DIS = Properties.Resources.Dis;

        static public Image B_BGlass = Properties.Resources.UB_img_1;
        static public Image B_Done = Properties.Resources.phone_2;
        static public Image B_Done_Horizon = Properties.Resources.phone_1;
        static public Image B_None = null;
        public static void IsSensorOn(AxBTNENHLib4.AxBtnEnh _bt, bool _SS)
        {
            if (_SS)
            {
                if (_bt.Value != 1)
                {
                    _bt.Value = 1;
                }
            }
            else
            {
                if (_bt.Value != 0)
                {
                    _bt.Value = 0;
                }
            }
        }
        public static void IsSensorOn(AxBTNENHLib4.AxBtnEnh _bt, bool _SS, string _strON, string _strOff)
        {
            if (_SS)
            {
                if (_bt.Value != 1)
                {
                    _bt.Value = 1;
                    _bt.Caption = _strON;
                }
                if (_bt.Caption != _strON)
                {
                    _bt.Caption = _strON;
                }

            }
            else
            {
                if (_bt.Value != 0)
                {
                    _bt.Value = 0;
                    _bt.Caption = _strOff;
                }
                if (_bt.Caption != _strOff)
                {
                    _bt.Caption = _strOff;
                }

            }
        }
        public static void IsSensorOnOut(AxBTNENHLib4.AxBtnEnh _bt, bool _SS1, bool _SS2, string _stateNormal = "", string _stateSS1 = "", string _stateSS2 = "")
        {
            if (_SS1)
            {
                if (_bt.Value != 1)
                {
                    _bt.Value = 1;
                }
                if (_bt.BackColorPressed != Color.FromArgb(32, 255, 32))
                {
                    _bt.Caption = _stateSS1;
                    _bt.BackColorPressed = Color.FromArgb(32, 255, 32);
                }
            }
            else if (_SS2)
            {
                if (_bt.Value != 1)
                {
                    _bt.Value = 1;
                }
                if (_bt.BackColorPressed != Color.FromArgb(255, 108, 108))
                {
                    _bt.Caption = _stateSS2;
                    _bt.BackColorPressed = Color.FromArgb(255, 108, 108);
                }
            }
            else
            {
                if (_bt.Value != 0)
                {
                    _bt.BackColorPressed = _bt.BackColorMouseOver;
                    _bt.Caption = _stateNormal;
                    _bt.Value = 0;
                }
            }

        }
        public static void IsSensorOnOut(AxBTNENHLib4.AxBtnEnh _bt, bool _SS1, string _stateOn = "", string _stateOff = "")
        {
            if (_SS1)
            {
                if (_bt.Value != 1)
                {
                    _bt.Value = 1;
                }
                if (_bt.Caption != _stateOn)
                    _bt.Caption = _stateOn;
            }

            else
            {
                if (_bt.Value != 0)
                {
                    _bt.Value = 0;
                }
                if (_bt.Caption != _stateOff)
                    _bt.Caption = _stateOff;
            }

        }
        public static void IsSensorOn(MyButton.ButtonPress _bt, bool _SS)
        {
            if (_SS)
            {
                if (_bt.Image != IO_ON || _bt.Press != true)
                {
                    _bt.Press = true;
                    _bt.Image = IO_ON;
                }
            }
            else
            {
                if (_bt.Image != IO_DIS || _bt.Press == true)
                {
                    _bt.Press = false;
                    _bt.Image = IO_DIS;
                }
            }
        }
        public static void IsSensorOn(MyButton.ButtonPress _bt, bool _SS, int _Image)
        {
            if (_SS)
            {
                if (_bt.Press != true)
                {
                    _bt.SetPress = true;
                }
            }
            else
            {
                if (_bt.Press == true)
                {
                    _bt.SetPress = false;
                }
            }
        }
        public static void IsSensorOn(MyButton.ButtonPress _bt, bool _SS1, bool _SS2)
        {
            if (_SS1)
            {
                if (_bt.Image != IO_ON || _bt.Press != true)
                {
                    _bt.Press = true;
                    _bt.Image = IO_ON;
                }
            }
            else if (_SS2)
            {
                if (_bt.Image != IO_ON || _bt.Press == true)
                {
                    _bt.Press = false;
                    _bt.Image = IO_ON;
                }
            }
            else
            {
                if (_bt.Image != IO_DIS || _bt.Press == true)
                {
                    _bt.Image = IO_DIS;
                    _bt.Press = false;
                }
            }
        }

        public static void ProductDisPlay(PictureBox _Image, Image _ImgDisplay)
        {
            if (_Image.Image != _ImgDisplay)
            {
                _Image.Image = _ImgDisplay;
            }
        }
        public static void ProductDisPlay(PictureBox _Image, bool _SS1, bool _SS2)
        {
            //if (_SS1)
            //    _Image.Image = B_Front;
            //else if (_SS2)
            //    _Image.Image = B_Done;
            //else
            //    _Image = null;
        }
        public static void ProductDisPlay(AxBTNENHLib4.AxBtnEnh _BOX, bool _SS1, string _Image)
        {
            if (_SS1)
            {
                if (!_BOX.Visible)
                {
                    _BOX.Visible = true;
                    //_BOX.Picture = _Image;
                }
            }
            else
            {
                if (_BOX.Visible)
                {
                    _BOX.Visible = false;
                }

            }
        }
        public static void ProductDisPlay(AxBTNENHLib4.AxBtnEnh _BOX, bool _SS1, bool _SS2, string _Image1, string _Image2)
        {
            if (_SS1)
            {
                if (!_BOX.Visible || _BOX.Picture != _Image1)
                {
                    _BOX.Visible = true;
                    _BOX.Picture = _Image1;
                }
            }
            else if (_SS2)
            {
                if (!_BOX.Visible || _BOX.Picture != _Image2)
                {
                    _BOX.Visible = true;
                    _BOX.Picture = _Image2;
                }
            }
            else
            {
                if (_BOX.Visible)
                {
                    _BOX.Visible = false;
                }

            }
        }
        public static void DisPlayString(SUserControls.ColorButton _Tb, string _StringPrint)
        {
            if (_Tb.Text != _StringPrint)
            {
                _Tb.Text = _StringPrint;
            }
        }
        public static void DisPlayString2(AxBTNENHLib4.AxBtnEnh _Tb, string _StringPrint)
        {
            if (_Tb.Caption != _StringPrint)
            {
                _Tb.Caption = _StringPrint;
            }
        }
        public static void DisPlayString(MyButton.ButtonPress _Tb, string _StringPrint)
        {
            if (_Tb.Text != _StringPrint)
            {
                _Tb.Text = _StringPrint;
            }
        }
        public static void DisPlayString(System.Windows.Forms.Label _Tb, string _StringPrint)
        {
            if (_Tb.Text != _StringPrint)
            {
                _Tb.Text = _StringPrint;
            }
        }
        #endregion

        #region GET MODE RUN
        public static bool IsDry()
        {
            if (SysMode == ModeRun.DryRun)
                return true;
            else
                return false;
        }

        public static bool IsAuto()
        {
            return (SysMode == ModeRun.Auto);
        }

        public static bool IsRun()
        {
            return (SysStatus == StatusRun.RUN) ? true : false;
        }

        public static bool IsStop()
        {
            return (SysStatus == StatusRun.STOP) ? true : false;
        }
        #endregion

        public static void CheckVisionProgramRun()
        {
            #region
            // "ScrewVision" 이름의 프로세스를 검색
            if (InforManager.Instance.AutoVisionAlignMode == 0)
                return;
            try
            {
                Process[] processes = Process.GetProcessesByName("ScrewAlign_VPRO");
                if (processes.Length == 0)
                {
                    // 프로세스가 실행 중이지 않으면 실행
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = @"C:\ScrewVision\ScrewAlign_VPRO.exe",
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Minimized // 최소화 상태로 실행
                    };

                    Process process = Process.Start(startInfo);
                    if (process == null)
                    {
                        MessageBox.Show("Vision Program Open Fail.\r\n C:\\ScrewVision\\ScrewAlign_VPRO.exe");
                    }
                }
            }
            catch(Exception ex)
            {
                string err = ex.ToString();
                MessageBox.Show("Vision Program Open Fail");
            }
            #endregion
        }
    }
}

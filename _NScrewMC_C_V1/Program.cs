using NMCMotionSDK;
using System;
using System.Data.SQLite;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace _NScrewMC_C_V1
{
    static class Program
    {
        public static string ModelName { get; set; } = "_NScrewMC_C_V1";


        //public static string Version { get; } = $"1.{DateTime.Now.ToString("yyMMdd")}.1T";

        // 1.250624.2T
        // 1.250702.1T - jlyoon
        //    - add Display Align Screw Image
        //    - add NG Screw Point to Vision
        // 1.250703.1T - jlyoon
        //    - LightCurtain Msg Bug Fix.
        //    - Display Align Screw Image Bug Fix.
        // 1.250704.1T - jlyoon
        //    - Second STOP button: move to ReadyY
        //    - MoveScrewPosY() check SafetyZ()
        //      ref: add 250704 jlyoon
        // 1.250705.3T - jlyoon
        //    - continus vision fail -> vision align not use
        //    - MSystem._Rework[m_idx] : vision skip
        //    - MemoWindow Close for LightCurtain // add 250705 jlyoon
        //    - add JigStatusOrg for Except JigStatus==8 Count
        // 1.250707.1T - jlyoon
        //    - update 250707 jlyoon
        // 1.250709.1T - jlyoon
        //    - STEP_TRASH_SCREW + 100:
        //      ref: add 250705 jlyoon
        // 1.250711.1T - jlyoon
        //    - MTrsJig step 50 update: ref: add 250705 jlyoon
        // 1.250717.1T - jlyoon
        //   - add 0716 jlyoon : jig stop two touch, button MtrsScrew init
        // 1.250719.1T - jlyoon
        //   - add 0719 jlyoon : jig stop button bug fix
        // 1.250722.1T - jlyoon
        //   - add 250722 jlyoon : rework vision not align

        public static string Version { get; set; } = $"1.260602.1T";
        public static string Directory = string.Empty;
        [STAThread]
        static void Main()
        {
            //////////////////////////////////////////////////////////////
            // Defult Deirctory
            string Path = System.IO.Directory.GetCurrentDirectory();
            Program.Directory = Path.Substring(0, Path.LastIndexOf('\\'));
            //////////////////////////////////////////////////////////////
            if (Connect_MMC() == false)
            {
                MessageBox.Show("Check mmce manager.");
                //return;
            }
            Mutex mutex = new Mutex(false, "SCREW_MC");
            try
            {
                if (mutex.WaitOne(0, false))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMain());
                }
                else
                {
                    MessageBox.Show("The program is running.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("PGM LOGIC FAIL " + e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (mutex != null)
                {
                    mutex.Close();
                    mutex = null;
                }
            }
            
        }

        static bool Connect_MMC()
        {
            try
            {
                NMCSDKLib.MC_STATUS mc = NMCSDKLib.MC_Init();
                if (mc != NMCSDKLib.MC_STATUS.MC_OK)
                    return false;

                if (NMCSDKLib.MC_MasterRUN(0) != NMCSDKLib.MC_STATUS.MC_OK)
                    return false;

                Stopwatch sw = Stopwatch.StartNew();
                while (true)
                {
                    byte mode = 0;
                    mc = NMCSDKLib.MasterGetCurMode(0, ref mode);
                    if (mc != NMCSDKLib.MC_STATUS.MC_OK)
                        return false;

                    if (mode == (byte)NMCSDKLib.EcMstMode.eMM_RUN)
                        break;

                    if (mode == (byte)NMCSDKLib.EcMstMode.eMM_ERR || mode == (byte)NMCSDKLib.EcMstMode.eMM_LINKBROKEN)
                        return false;

                    if (sw.ElapsedMilliseconds > 30000)
                        return false;

                    Thread.Sleep(500);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}

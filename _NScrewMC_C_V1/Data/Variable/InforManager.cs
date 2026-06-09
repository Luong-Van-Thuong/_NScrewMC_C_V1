using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace _NScrewMC_C_V1
{
    public class InforManager : MSystem
    {
        private static InforManager instance;
        public static InforManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InforManager();
                }
                return instance;
            }
        }

        /* system */
        public bool m_bVisionType = false; // true = Internal; false = External
        public bool IsSmartKitUse = false;
        public bool IsScrewPickupReady = false;
        public int AutoVisionAlignMode = 0;
        public bool IsVisionAlignZUpUse = false;
        public bool IsVisionScanNgOut = false;
        public bool IsLeftUnitUse = false;
        public bool IsRightUnitUse = false;
        public bool GeimUse = false;
        public bool IsDetectSetUse = true;
        public string _LastLampState = "";
        public bool ContinuesScrew = false;
        public bool RunScrewAlign = false;
        public bool IsBarcodeUse = false;
        public string InspectionType = "Tablet"; // false = Mobile; true = Tablet
		public bool IsUseRework=false;
        public bool IsAngleControl = false;
        public bool IsAlignType = true;//True= 2point, false=multipoint
        public bool IsPickupVaccumMode = true;
        public bool IsScrewNgVaccumOff = false;
        public bool IsYAxisReverce = false;

        /* screw move */
        public double m_dSpeedScrew = 45.0;
        public double m_dScrewOffset = 30.0;
        public double m_dPickupOffset = 0.0;
        public double m_dLimitZPlus = 100.0;
        public double m_dScrewVaccumOffLevelZ = 0.0;

        /* delay time */
        public double BlowTime = 0.5;
        public double ScrewOverTime = 5.0;
        public double TimeOutFeederReady = 5.0;
        public double TimeBuzze = 3;
        public double PickupDelayTime = 0.3; //  add 260226 cnz

        public double DelayFastening = 0;

        /*Hantas*/
        public double m_iHantasA2AngleControl = 30000;
        public double m_iHantasA2AngleRetryControl = 30000;
        /* model data */
        public string ModelName = "MODEL_DEFUALT";
        public string ModelNameVision = "MODEL_DEFUALT VISION";

        /* comport */
        public string JogPort = "COM1";
        public string[] HantasPort = { "COM3", "COM4" };
        public string[] BarcodePort = { "COM5", "COM6" };

        /* etc */
        public double m_dVacuumOn = 0.3;
        public bool ProductInfor = false;
        public bool IsVisionPoint = false;

        private static readonly object _fileLock = new object();
        public void LoadSetting()
        {
            if (File.Exists(Config.SystemFilePath))
            {
                string _s = File.ReadAllText(Config.SystemFilePath, Encoding.UTF8);
                try
                {
                    instance = JsonConvert.DeserializeObject<InforManager>(_s);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SystemFile need to check" + ex.ToString());
                    SaveData();
                }
            }
            else
            {
                Directory.CreateDirectory(Config.SystemSavePath);
                SaveData();
            }
        }
        //public void SaveData()
        //{
        //    if (!Directory.Exists(Config.SystemSavePath))
        //        Directory.CreateDirectory(Config.SystemSavePath);
        //    if (!File.Exists(Config.SystemFilePath))
        //        using (var myfile = File.Create(Config.SystemFilePath)) { }
        //    try
        //    {
        //        string _sTestSpecString = JsonConvert.SerializeObject(Instance);
        //        string _sTestSpecStringIndeneted = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
        //        File.WriteAllText(Config.SystemFilePath, _sTestSpecStringIndeneted);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        public void SaveData()
        {
            lock (_fileLock) 
            {
                if (!Directory.Exists(Config.SystemSavePath))
                    Directory.CreateDirectory(Config.SystemSavePath);
                if (!File.Exists(Config.SystemFilePath))
                    using (var myfile = File.Create(Config.SystemFilePath)) { }
                try
                {
                    string _sTestSpecString = JsonConvert.SerializeObject(Instance);
                    string _sTestSpecStringIndeneted = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
                    File.WriteAllText(Config.SystemFilePath, _sTestSpecStringIndeneted);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"SaveData Error: {ex.Message}");
                    ex.ToString();
                }
            }  
        }
    }
    public class Config
    {
        //Infor manager
        public static string FolderPath = $@"{Program.Directory}\";
        public static string SystemSavePath = FolderPath + @"System\Manager\";
        public static string SystemFilePath = SystemSavePath + "SystemManager.json";

        //servo parameter
        public static string ServoSavePath = FolderPath + @"Infor\Axis\";
        public static string ServoFilePath = ServoSavePath + "axis.json";

        //Model position
        public static string ModelSavePath = FolderPath + @"Infor\Model\";
        public static string ModelVisionSavePath = FolderPath + @"Infor\Model\ModelVision\";
        //public static string ModelFilePath = ModelSavePath + InforManager.Instance.ModelName + ".json";

        // GMES
        public static string LogGMESSave = @"C:\FA\MagnetAuto\Log\DataGMESRun\";

        //Log Data
        public static string LogDevice = $@"{FolderPath}Log\DevLog\";
        public static string LogError = $@"{FolderPath}Log\Error\";
        public static string LogDataSave = $@"{FolderPath}Log\DataSave\";
        public static string LogScrew1 = @"D:LogScrew\DataScrew1\";
        public static string LogScrew2 = @"D:LogScrew\DataScrew2\";
        public static string LogHantasLeft = @"D:LogScrew\DataHantasLeft\";
        public static string LogHantasRight = @"D:LogScrew\DataHantasRight\";
        public static string LogBarcodeResult = @"D:LogScrew\InspectionResult\";


        public static string LogGMES = $@"{FolderPath}Log\GMES_Log\";
        public static string LogScrewPathNetwork = @"C:\FA\ScrewV1Log\";

        //Data Running
        public static string SysTemRunning = $@"{FolderPath}System\DataRunning\";

        //DataBase 
        public static string DataLogError = $@"{FolderPath}Log\Error\";
        //
        public static string PathLogGEIM = @"C:\FA\LOG\";
        public static string PathLogGEIM_D1 = @"C:\FA\LOG_D1\";
        public static string PathLogGEIM_C1 = @"C:\FA\LOG_C1\";
        // Log Error Stop Time
        public static string LogErrStopTime = @"C:\FA\ErrorStopTime\";
    }
}

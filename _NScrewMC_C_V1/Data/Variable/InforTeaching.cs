using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _NScrewMC_C_V1
{
    public class InforTeaching
    {
        private static InforTeaching instance;
        public static InforTeaching Instance
        {
            get {
                if (instance == null)
                {
                    instance = new InforTeaching();
                }
                return instance;
            }
        }
        [JsonProperty]

        public PointAxis[,] P_MainScrew = new PointAxis[3, (int)PScrewMain.ePOS_MAX];

        public List<PointScrew>[] P_Screw = {new List<PointScrew>(),
                                               new List<PointScrew>(),
                                               new List<PointScrew>() };
        //public List<PointAxis> P_ScrewMaster = new List<PointAxis>();
        public PointAxis[] P_ScrewMaster = new PointAxis[50];
        public double[] Handtas_Touque = new double[2];
        
        private static readonly object _fileLock = new object();
        public int LRW500Value = 100;
        public int[] LRW500RGB = {0,0,0};
        public void LoadSettingPath(string path)
        {

            if (File.Exists(path))
            {
                string _s = File.ReadAllText(path, Encoding.UTF8);
                try
                {
                    instance = JsonConvert.DeserializeObject<InforTeaching>(_s);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("SystemFile need to check" + ex.ToString());
                }
            }
            else
            {
                MSystem.MyMsgMemo($"{path} File Load Fail", "Error");
            }
        }
        public void LoadSetting()
        {
           
           if (File.Exists(Config.ModelSavePath+InforManager.Instance.ModelName+".json"))
            {
                string _s = File.ReadAllText(Config.ModelSavePath + InforManager.Instance.ModelName + ".json", Encoding.UTF8);
                try
                {
                    instance = JsonConvert.DeserializeObject<InforTeaching>(_s);

                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SystemFile need to check" + ex.ToString());
                }
            }
            else
            {
                Directory.CreateDirectory(Config.ModelSavePath);
                SaveSettings();
            }
        }

        public void LoadSettingVision()
        {

            if (File.Exists(Config.ModelVisionSavePath + InforManager.Instance.ModelNameVision + ".json"))
            {
                string _s1 = File.ReadAllText(Config.ModelVisionSavePath + InforManager.Instance.ModelNameVision + ".json", Encoding.UTF8);
                try
                {
                    instance = JsonConvert.DeserializeObject<InforTeaching>(_s1);


                }
                catch (Exception ex)
                {
                    MessageBox.Show("SystemFile need to check" + ex.ToString());
                }
            }
            else
            {
                Directory.CreateDirectory(Config.ModelVisionSavePath);
                SaveSettingsVision();
            }
        }
        //public void SaveSettings()
        //{
        //    if (!File.Exists(Config.ModelSavePath))
        //    {
        //        InforManager.Instance.SaveData();
        //        using (var myfile = File.Create(Config.ModelSavePath + InforManager.Instance.ModelName + ".json")) { }
        //    }
        //    try
        //    {
        //        string _sTestSpecString = JsonConvert.SerializeObject(Instance);
        //        string _sTestSpecStringIndeneted = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
        //        File.WriteAllText(Config.ModelSavePath + InforManager.Instance.ModelName + ".json", _sTestSpecStringIndeneted);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        public void SaveSettings()
        {
            lock (_fileLock)
            {

                if (!File.Exists(Config.ModelSavePath))
                {
                    InforManager.Instance.SaveData();
                    using (var myfile = File.Create(Config.ModelSavePath + InforManager.Instance.ModelName + ".json")) { }
                }
                try
                {
                    string _sTestSpecString = JsonConvert.SerializeObject(Instance);
                    string _sTestSpecStringIndeneted = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
                    File.WriteAllText(Config.ModelSavePath + InforManager.Instance.ModelName + ".json", _sTestSpecStringIndeneted);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"SaveSettings Error: {ex.Message}");
                    ex.ToString();
                }
            }
        }

        public void SaveSettingsVision()
        {
            lock (_fileLock)  
            {
                if (!File.Exists(Config.ModelVisionSavePath))
                {
                    InforManager.Instance.SaveData();
                    using (var myfile = File.Create(
                        Config.ModelVisionSavePath + InforManager.Instance.ModelNameVision + ".json"
                    )) { }
                }

                try
                {
                    string _sTestSpecString = JsonConvert.SerializeObject(Instance);
                    string _sTestSpecStringIndeneted = JToken.Parse(_sTestSpecString)
                        .ToString(Formatting.Indented);
                    File.WriteAllText(
                        Config.ModelVisionSavePath + InforManager.Instance.ModelNameVision + ".json",
                        _sTestSpecStringIndeneted
                    );
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"SaveSettingsVision Error: {ex.Message}");
                    ex.ToString();
                }
            }
        }

        //public void SaveSettings(string filepath)
        //{
        //    if (!Directory.Exists(Config.ModelSavePath))
        //        Directory.CreateDirectory(Config.ModelSavePath);
        //    try
        //    {
        //        string _sTestSpecString = JsonConvert.SerializeObject(Instance);
        //        string _sTestSpecStringIndeneted = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
        //        File.WriteAllText($"{Config.ModelSavePath + filepath + ".json"}", _sTestSpecStringIndeneted);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }

        //}

        public void SaveSettings(string filepath)
        {
            lock (_fileLock)  
            {
                
                if (!Directory.Exists(Config.ModelSavePath))
                    Directory.CreateDirectory(Config.ModelSavePath);
                try
                {
                    string _sTestSpecString = JsonConvert.SerializeObject(Instance);
                    string _sTestSpecStringIndeneted = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
                    File.WriteAllText($"{Config.ModelSavePath + filepath + ".json"}", _sTestSpecStringIndeneted);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"SaveSettings Error: {ex.Message}");
                    ex.ToString();
                }
            } 
        }
        public string[] LoadAllModell() {
            DirectoryInfo d = new DirectoryInfo(Config.ModelSavePath);
            try
            {
                FileInfo[] Files = d.GetFiles("*.json");
                string str = "";
                if (Files.Length > 0)
                {
                    foreach (FileInfo file in Files)
                    {
                        str = str + "," + file.Name;
                    }
                    string[] strModel = str.Split(',');
                    return strModel;
                }
                else
                    return null;
            }
            catch 
            {
                return null;
            }
        }

    }
    public class AoiParam
    {
        private static AoiParam instance;

        public static AoiParam Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AoiParam();
                }
                return instance;
            }
        }

        public string CameraName { get; set; }
        public long Exposure { get; set; }
        public long SizeW { get; set; }
        public long SizeH { get; set; }
        public long OffsetX { get; set; }
        public long OffsetY { get; set; }

        //Camera
        public string SET1_LEFT = "CAML";
        public string SET1_RIGHT = "CAMR";


        public List<AoiParam> AoiParams = new List<AoiParam>();
        [JsonProperty]
        public List<AoiParam> Aoi = new List<AoiParam>();
        [JsonProperty]
        public long NumberOfCamera = 2;
    }
}

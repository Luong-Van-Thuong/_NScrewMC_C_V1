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
    public class InforModelImport
    {
        private static InforModelImport instance;
        public static InforModelImport Instance
        {
            get {
                if (instance == null)
                {
                    instance = new InforModelImport();
                }
                return instance;
            }
        }
        [JsonProperty]
        public PointAxis[,] P_MainScrew     = new PointAxis[3, (int)PScrewMain.ePOS_MAX];

        public List<PointScrew>[] P_Screw   = {new List<PointScrew>(),
                                               new List<PointScrew>(),
                                               new List<PointScrew>() };

        public void LoadSetting(string _Modelname)
        {
            if (File.Exists(Config.ModelSavePath+_Modelname+".json"))
            {
                string _s = File.ReadAllText(Config.ModelSavePath + _Modelname + ".json", Encoding.UTF8);
                try
                {
                    instance = JsonConvert.DeserializeObject<InforModelImport>(_s);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SystemFile need to check" + ex.ToString());
                }
            }
            else
            {
                Directory.CreateDirectory(Config.ModelSavePath);
                SaveSettings(_Modelname);
            }
        }
        public void SaveSettings(string _Modelname)
        {
            try
            {
                string _sTestSpecString = JsonConvert.SerializeObject(Instance);
                string _sTestSpecStringIndeneted = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
                File.WriteAllText(Config.ModelSavePath + _Modelname + ".json", _sTestSpecStringIndeneted);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

    }
}

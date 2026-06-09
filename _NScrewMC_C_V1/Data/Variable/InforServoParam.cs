using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace _NScrewMC_C_V1
{
    public class ServoParam
    {
        private static ServoParam instance;
        public static ServoParam Instance
        {
            get {
                if (instance == null) {
                    instance = new ServoParam();
                    for (int _index = 0; _index < (int)Axis.eAXIS_MAX; _index++) {
                        instance.m_sAxisInfo.Add(new sAxisInfo());
                    }
                }
                return instance;
            }
        }

        [JsonProperty]
        public List<sAxisInfo> m_sAxisInfo = new List<sAxisInfo>();
        protected void LoadDefaultParam(int _index)
        {
            m_sAxisInfo[_index].AxisName           = ((Axis)_index).ToString();
            m_sAxisInfo[_index].dAxisDec           = 100;
            m_sAxisInfo[_index].dJogSlowSpeed      = 10.0;
            m_sAxisInfo[_index].dJogSlowAccDec     = 300;
            m_sAxisInfo[_index].dJogMidSpeed       = 30;
            m_sAxisInfo[_index].dJogMidAccDec      = 300;
            m_sAxisInfo[_index].dJogFastSpeed      = 50.0;
            m_sAxisInfo[_index].dJogFastAccDec     = 300;
            m_sAxisInfo[_index].dLimitPluseValue   = 134217727;
            m_sAxisInfo[_index].dLimitMinusValue   = -13421772;
            m_sAxisInfo[_index].dOriginSpeed       = 500;
            m_sAxisInfo[_index].dOriginSearchSpeed = 10;
            m_sAxisInfo[_index].dOriginLimitTime   = 100;
            m_sAxisInfo[_index].dOriginOffset      = 0;
            m_sAxisInfo[_index].iGain              = 4;
            switch (_index) 
            {
                case 0:
                    m_sAxisInfo[0].OrgDir        = 1;
                    m_sAxisInfo[0].MotionDir     = 1;
                    m_sAxisInfo[0].dAxisMaxSpeed = 750;
                    m_sAxisInfo[0].dAxisAcc      = 300;
                    break;
                case 1:
                    m_sAxisInfo[1].OrgDir        = 1;
                    m_sAxisInfo[1].MotionDir     = 1;
                    m_sAxisInfo[1].dAxisMaxSpeed = 750;
                    m_sAxisInfo[1].dAxisAcc      = 300;
                    break;
                case 2:
                    m_sAxisInfo[2].OrgDir        = 1;
                    m_sAxisInfo[2].MotionDir     = 1;
                    m_sAxisInfo[2].dAxisMaxSpeed = 750;
                    m_sAxisInfo[2].dAxisAcc      = 300;
                    break;
                case 3:
                    m_sAxisInfo[3].OrgDir        = 1;
                    m_sAxisInfo[3].MotionDir     = 1;
                    m_sAxisInfo[3].dAxisMaxSpeed = 750;
                    m_sAxisInfo[3].dAxisAcc      = 300;
                    break;
                case 4:
                    m_sAxisInfo[4].OrgDir        = 1;
                    m_sAxisInfo[4].MotionDir     = 1;
                    m_sAxisInfo[4].dAxisMaxSpeed = 750;
                    m_sAxisInfo[4].dAxisAcc      = 300;
                    break;
                case 5:
                    m_sAxisInfo[5].dAxisMaxSpeed = 750;
                    m_sAxisInfo[5].dAxisAcc      = 300;
                    m_sAxisInfo[5].OrgDir        = 1;
                    m_sAxisInfo[5].MotionDir     = 1;
                    break;
                case 6:
                    m_sAxisInfo[6].dAxisMaxSpeed = 750;
                    m_sAxisInfo[6].dAxisAcc      = 300;
                    m_sAxisInfo[6].OrgDir        = 1;
                    m_sAxisInfo[6].MotionDir     = 1;
                    break;
                case 7:
                    m_sAxisInfo[7].dAxisMaxSpeed = 750;
                    m_sAxisInfo[7].dAxisAcc      = 300;
                    m_sAxisInfo[7].OrgDir        = 1;
                    m_sAxisInfo[7].MotionDir     = 1;
                    break;
                case 8:
                    m_sAxisInfo[8].dAxisMaxSpeed = 750;
                    m_sAxisInfo[8].dAxisAcc      = 300;
                    m_sAxisInfo[8].OrgDir        = 1;
                    m_sAxisInfo[8].MotionDir     = 1;
                    break;
                case 9:
                    m_sAxisInfo[9].dAxisMaxSpeed = 750;
                    m_sAxisInfo[9].dAxisAcc      = 300;
                    m_sAxisInfo[9].OrgDir        = 1;
                    m_sAxisInfo[9].MotionDir     = 1;
                    break;
                case 10:
                    m_sAxisInfo[10].dAxisMaxSpeed = 750;
                    m_sAxisInfo[10].dAxisAcc      = 300;
                    m_sAxisInfo[10].OrgDir        = 1;
                    m_sAxisInfo[10].MotionDir     = 1;
                    break;
                case 11:
                    m_sAxisInfo[11].dAxisMaxSpeed = 750;
                    m_sAxisInfo[11].dAxisAcc      = 300;
                    m_sAxisInfo[11].OrgDir        = 1;
                    m_sAxisInfo[11].MotionDir     = 1;
                    break;
                case 12:
                    m_sAxisInfo[12].dAxisMaxSpeed = 750;
                    m_sAxisInfo[12].dAxisAcc      = 300;
                    m_sAxisInfo[12].OrgDir        = 1;
                    m_sAxisInfo[12].MotionDir     = 1;
                    break;
                case 13:
                    m_sAxisInfo[13].dAxisMaxSpeed = 750;
                    m_sAxisInfo[13].dAxisAcc      = 300;
                    m_sAxisInfo[13].OrgDir        = 1;
                    m_sAxisInfo[13].MotionDir     = 1;
                    break;
                default: break;
            }
        }
        public void LoadSetting()
        {
            start:
            if (File.Exists(Config.ServoFilePath))
            {
                string _s = File.ReadAllText(Config.ServoFilePath, Encoding.UTF8);
                try
                {
                    instance = JsonConvert.DeserializeObject<ServoParam>(_s);
                    for (int i = 0; i < (int)Axis.eAXIS_MAX; i++) {
                        if (i > instance.m_sAxisInfo.Count - 1 
                            || (instance.m_sAxisInfo[i].dAxisMaxSpeed == 0 && instance.m_sAxisInfo[i].dAxisAcc == 0))
                        {
                            instance.LoadDefaultParam(i);  
                        }
                    }
                    instance.SaveSettings();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SystemFile need to check" + ex.ToString());
                }
            }
            else
            {
                Directory.CreateDirectory(Config.ServoSavePath);
                SaveSettings();
                goto start;
            }
        }
        public void SaveSettings()
        {
            if (!File.Exists(Config.ServoFilePath))
                using (var myFile = File.Create(Config.ServoFilePath)) { }
            try
            {
                string _sTestSpecString = JsonConvert.SerializeObject(Instance);
                string _sTestSpecStringIndeneted = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
                File.WriteAllText(Config.ServoFilePath, _sTestSpecStringIndeneted);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
    }
}

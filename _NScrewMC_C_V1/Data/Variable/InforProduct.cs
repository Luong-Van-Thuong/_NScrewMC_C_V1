using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace _NScrewMC_C_V1
{
    public class InforProduct : MSystem
    {
        private static InforProduct instance;
        public static InforProduct Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InforProduct();
                }
                return instance;
            }
        }
      
        [JsonProperty]
        public long Total_product = 0;
        [JsonProperty]
        public long ProductPass = 0;
        [JsonProperty]
        public long ProductNG = 0;

        [JsonProperty]
        public long ProductGMES_Total = 0;
        public long ProductGMES_Pass = 0;
        public long ProductGMES_Fail = 0;

        public long ProductBCR_Fail_L =  0;
        public long ProductBCR_Fail_R = 0;

        public long[] ProductScrew_Total = { 0, 0 };
        public long[] ProductScrew_Pass = { 0, 0 };
        public long[] ProductScrew_Fail = { 0, 0 };
        public long[] ProductScrew_Total_Vision = { 0, 0 };
        public long[] ProductScrew_Pass_Vision = { 0, 0 };
        public long[] ProductScrew_Fail_Vision = { 0, 0 };

        [JsonProperty]
        public bool ProductInforShow = true;

        public List<ProductDetail>[] ProductDetail = {new List<ProductDetail>(),
                                               new List<ProductDetail>()};
        #region ini file Read Write
        [DllImport("kernel32.dll")]
        private static extern uint GetPrivateProfileString(string section,
                                                           string key,
                                                           string defaultValue, //키값이 없을 때의 기본 값
                                                           StringBuilder returnedString,
                                                           uint size,
                                                           string filePath);

        [DllImport("kernel32.dll")]
        private static extern bool WritePrivateProfileString(string section,
                                                             string key,
                                                             string value,
                                                             string filePath);
        #endregion
        private readonly object saveTorqueLock = new object(); // 동기화를 위한 객체
        private static readonly object _fileLock = new object();
        public void LoadSetting()
        {
            if (File.Exists(Config.SysTemRunning + "DataRunning.json"))
            {
                string _s = File.ReadAllText(Config.SysTemRunning + "DataRunning.json", Encoding.UTF8);
                try
                {
                    instance = JsonConvert.DeserializeObject<InforProduct>(_s);

                    // Ensure P_Screw is initialized
                    if (InforTeaching.Instance.P_Screw[MSystem.eLEFT] != null && InforTeaching.Instance.P_Screw[MSystem.eRIGHT] != null)
                    {
                        // Initialize ProductDetail for LEFT
                        while (InforProduct.instance.ProductDetail[MSystem.eLEFT].Count < InforTeaching.Instance.P_Screw[MSystem.eLEFT].Count)
                        {
                            InforProduct.instance.ProductDetail[MSystem.eLEFT].Add(new ProductDetail());
                        }

                        // Initialize ProductDetail for RIGHT
                        while (InforProduct.instance.ProductDetail[MSystem.eRIGHT].Count < InforTeaching.Instance.P_Screw[MSystem.eRIGHT].Count)
                        {
                            InforProduct.instance.ProductDetail[MSystem.eRIGHT].Add(new ProductDetail());
                        }
                        Directory.CreateDirectory(Config.SysTemRunning);
                        SaveSettings();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("SystemFile need to check" + ex.ToString());
                    SaveSettings();
                }
            }
            else
            {
                // Ensure P_Screw is initialized
                if (InforTeaching.Instance.P_Screw[MSystem.eLEFT] != null && InforTeaching.Instance.P_Screw[MSystem.eRIGHT] != null)
                {
                    // Initialize ProductDetail for LEFT
                    while (InforProduct.instance.ProductDetail[MSystem.eLEFT].Count < InforTeaching.Instance.P_Screw[MSystem.eLEFT].Count)
                    {
                        InforProduct.instance.ProductDetail[MSystem.eLEFT].Add(new ProductDetail());
                    }

                    // Initialize ProductDetail for RIGHT
                    while (InforProduct.instance.ProductDetail[MSystem.eRIGHT].Count < InforTeaching.Instance.P_Screw[MSystem.eRIGHT].Count)
                    {
                        InforProduct.instance.ProductDetail[MSystem.eRIGHT].Add(new ProductDetail());
                    }
                    Directory.CreateDirectory(Config.SysTemRunning);
                    SaveSettings();
                }
            }
        }

        //public void SaveSettings()
        //{
        //    try
        //    {
        //        string _sTestSpecString = JsonConvert.SerializeObject(Instance);
        //        string _sTestSpecStringIndeneted = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
        //        File.WriteAllText(Config.SysTemRunning + "DataRunning.json", _sTestSpecStringIndeneted);
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
                try
                {
                    string _sTestSpecString = JsonConvert.SerializeObject(Instance);
                    string _sTestSpecStringIndeneted = JToken.Parse(_sTestSpecString).ToString(Formatting.Indented);
                    File.WriteAllText(Config.SysTemRunning + "DataRunning.json", _sTestSpecStringIndeneted);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"SaveSettings Error: {ex.Message}");
                    ex.ToString();
                }
            }  
        }

        public void ResetTactime()
        {
            lock (LockTactime)
            {
                _tactime.Clear();
                //AutoForm.TXT_TACTIME.Text = "0.00";
                //AutoForm.TXT_TACTIME_AVG.Text = "0.00";
            }

        }

        public void UpdateTactimeDisplay()
        {
            lock (LockTactime)
            {
                if (_tactime.Count > 50) _tactime.RemoveAt(0);
                if (_tactime.Count > 0)
                {
                    double total = 0;
                    foreach (double _tmp in _tactime)
                    {
                        total += _tmp;
                    }
                    total = total / _tactime.Count;
                    //DisPlayString2(AutoForm.TXT_TACTIME, _tactime[_tactime.Count - 1].ToString("f2"));
                    //DisPlayString2(AutoForm.TXT_TACTIME_AVG, total.ToString("f2"));
                }
                else
                {
                    //DisPlayString2(AutoForm.TXT_TACTIME, "0.00");
                    //DisPlayString2(AutoForm.TXT_TACTIME_AVG,"0.00");
                }
                //DisPlayString2(AutoForm.TXT_TOTAL_PRODUCT, InforProduct.Instance.Total_product.ToString());
                //DisPlayString2(AutoForm.TXT_PASS_PRODUCT, InforProduct.Instance.ProductPass.ToString());
                //DisPlayString2(AutoForm.TXT_NG_PRODUCT, InforProduct.Instance.ProductNG.ToString());
            }
        }
        public void DetailDataAdd(int LeftRight, int pos)
        {
            if (InforTeaching.Instance.P_Screw[LeftRight].Count <= pos)
                return;

            if (LeftRight == MSystem.eLEFT)
            {
                instance.ProductDetail[LeftRight][pos].posName = $@"LEFT - P{pos + 1}";
            }
            else
            {
                instance.ProductDetail[LeftRight][pos].posName = $@"RIGHT - P{pos + 1}";
            }

            string detail = DateTime.Now.ToString("HH:mm:ss fff") + "," + $"{MSystem.m_pTrsScrew[LeftRight]._LastTorque:F2}";

            Instance.ProductDetail[LeftRight][pos].DetailStack.Push(detail);
            if (Instance.ProductDetail[LeftRight][pos].DetailStack.Count > 1000)
            {
                // 1. Stack 데이터를 배열로 변환
                string[] allItems = Instance.ProductDetail[LeftRight][pos].DetailStack.ToArray();

                // 2. 마지막 1000개 값 추출
                string[] lastFiveItems = allItems.Take(1000).Reverse().ToArray();

                // 3. Stack 초기화 및 마지막 1000개 값 다시 추가
                Instance.ProductDetail[LeftRight][pos].DetailStack.Clear();
                foreach (string item in lastFiveItems)
                {
                    Instance.ProductDetail[LeftRight][pos].DetailStack.Push(item);
                }
            }
        }
        public void SaveTorqueValue(int UnitIndex, int pos, double newTorque)
        {
            lock (saveTorqueLock)
            {
                string IniFilePath = Config.FolderPath + @"System\DataRunning\TorqueGraphData.ini";
                string SectionName;
                SectionName = UnitIndex == MSystem.eLEFT ? "LEFT" : "RIGHT";
                string key = $"P{pos + 1}";

                // 기존 Torque 값 읽기
                StringBuilder existingValues = new StringBuilder(1024);
                GetPrivateProfileString(SectionName, key, "", existingValues, (uint)existingValues.Capacity, IniFilePath);

                // 기존 값을 ','로 분리하여 리스트로 변환
                List<string> torqueList = existingValues.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                // 새 Torque 값 추가
                torqueList.Add(newTorque.ToString("F2")); // 소수점 2자리까지 저장

                // 200개 초과 시 오래된 값 제거
                if (torqueList.Count > 200)
                {
                    torqueList.RemoveAt(0); // 첫 번째 데이터 제거
                }

                // 리스트를 ','로 연결하여 다시 저장
                string updatedValues = string.Join(",", torqueList);
                WritePrivateProfileString(SectionName, key, updatedValues, IniFilePath);
            }
        }

        /// <summary>
        /// .ini 파일에서 Torque 값을 읽어오기
        /// </summary>
        /// <returns>Torque 값 리스트</returns>
        public List<double> LoadTorqueValues(int UnitIndex, int pos)
        {
            string IniFilePath = Config.FolderPath +@"System\DataRunning\TorqueGraphData.ini";
            string SectionName;
            SectionName = UnitIndex == MSystem.eLEFT ? "LEFT" : "RIGHT";
            string key = $"P{pos + 1}";
            StringBuilder existingValues = new StringBuilder(1024);
            GetPrivateProfileString(SectionName, key, "", existingValues, (uint)existingValues.Capacity, IniFilePath);

            return existingValues.ToString()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(value => double.Parse(value))
                .ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _NScrewMC_C_V1
{
    public class Alarm : MSystem
    {
        public string path = $@"{Program.Directory}\Data\";
        public string fileName = "ErrorMessage.ini";
        public string PathFile = "";

        public List<string[]> AlarmList = new List<string[]>();

        public Alarm() 
        {
            PathFile = $"{path}{fileName}";
            LoadAlarmList(PathFile);
        }

        public void LoadAlarmList(string Path)
        {
            bool tmp;
            if (File.Exists(Path) == false)
                return;

            //Load Alarm List
            string[] line = File.ReadAllLines(Path, Encoding.Unicode);
            for (int i = 0; i < line.Length; i++)
            {
                foreach (char ckeckstr in line[i])
                {
                    if (line[i] == "") break;
                    if (ckeckstr.ToString() == "[")
                    {
                        tmp = true;
                        string[] strListAlarm = new string[5];
                        for (int j = 0; j < 5; j++)
                        {
                            if ((i + j) < line.Length)
                            {
                                if (line[i + j] == "") break;
                                if (line[i + j][0] == '[' && !tmp) break;
                                strListAlarm[j] = line[i + j].Trim();
                                tmp = false;
                            }
                            //////////////////////////////////////
                        }
                        AlarmList.Add(strListAlarm);
                    }
                    else
                    {
                        continue;
                    }
                    break;
                }
            }
        }

        public void SetErrorMsg(long number, string strunit)
        {
            lock (LockButtonMessenger)
            {
                if (SysStatus == StatusRun.ERROR || (SysStatus == StatusRun.STOP))
                    return;

                SysStatus = StatusRun.ERROR;

                if(strunit.Contains("Left"))
                    isStart[Constants.left] = false;
                else
                    isStart[Constants.right] = false;

                m_pTrsBuzzer.SetBuzzerPattern(0);

                string strError = "";
                string strMessager = "[" + number.ToString() + "]";
                bool ErrorcodeFound = false;
                for (int i = 0; i < AlarmList.Count; i++)
                {
                    if (AlarmList[i][0] == strMessager)
                    {
                        ErrorcodeFound = true;
                        string reson = "";
                        int _index = 0;
                        string action = "";
                        for (int j = 1; j < AlarmList[i].Length; j++)
                        {
                            if (AlarmList[i][j] != "")
                            {
                                string[] str1 = AlarmList[i][j].Split('=');
                                if (str1[0].Trim().Contains("message") || str1[0].Trim() == "messager" || str1[0].Trim() == "messager2")
                                {
                                    strError += $"{AlarmList[i][j].Split('=')[1].Trim()}\n";
                                    _index++;

                                }
                                if (AlarmList[i][j].Contains("recovery"))
                                {
                                    action += $"{AlarmList[i][j].Split('=')[1].Trim()} \r\n";
                                    _index++;
                                }

                            }
                        }
                        int _index2 = 0;
                        for (int j = _index; j < AlarmList[i].Length; j++)
                        {
                            if (AlarmList[i][j] != "")
                            {
                                string[] str1 = AlarmList[i][j].Split('=');
                                if (str1[0].Trim() == "recovery" || str1[0].Trim() == "recovery2")
                                {
                                    _index2++;
                                }
                                else break;
                            }
                        }
                        if (strError.Trim() == "") break;
                        string temp_error = strError;
                        if (temp_error.Contains("\n"))
                            temp_error = temp_error.Replace("\n", " ");
                        MSystem.BottomForm.ShowTime(strError, action, $"{strunit}[{number}]");
                        m_pLogSave.AddTail(LogIndex.eLogGEIM, $"{number.ToString()}");
                        m_pLogSave.AddTail(LogIndex.eErrorLog, $"{temp_error.Split('\r')[0]} ,[{number}]");
                        // Log ErrorStopTime
                        string _errorstopstr = temp_error.Split('\r')[0].Split('.')[0];
                        m_pLogSave.ErrorStopTimeLogSave($"Error Stop,{_errorstopstr},");
                        SysStatus = StatusRun.STOP;
                        m_pTrsBuzzer.StopAllBuzzers();
                        break;
                    }
                }
                if (!ErrorcodeFound)
                {
                    // Log ErrorStopTime
                    m_pLogSave.ErrorStopTimeLogSave($"Error Stop,UNKNOW,");
                    MSystem.BottomForm.ShowTime("UNKNOW", "Please Contac with Engineer EQM", $"{strunit} [{number}]");
                    SysStatus = StatusRun.STOP;
                    m_pTrsBuzzer.StopAllBuzzers();
                }
            }
        }
    }
}

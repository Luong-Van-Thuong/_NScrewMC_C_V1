using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _NScrewMC_C_V1
{
    public class DataManager
    {
        static string pathSave = $@"{Program.Directory}\System\";
        static string fileName = $"RunningData.ini";
        public string LineName = "Default";
        public string Companny = "SEVT";
        public string ModelName = "DEFAULT";

        public string ServoPort          = "COM1";
        public string IOPort             = "COM2";
        public string BCR_Shuttle_Port   = "COM3";
        public string BCR_BG_Port        = "COM4";
        public string GMESPort           = "COM5";

        public double VaccumOnTime  = 0.05;
        public double VaccumOffTime = 0.0;
        public double GripOnTime    = 0.05;
        public double GripOffTime   = 0.0;

        DataReadWrite m_pDataSys = new DataReadWrite(pathSave, fileName);

        public DataManager() 
        {
            ReadData();
            SaveData();
        }

        public void SaveData() 
        {
            m_pDataSys.SaveString("GENERAL","Line Name", "Line 01");
            m_pDataSys.SaveString("GENERAL", "Factory", "SEV");
            m_pDataSys.SaveString("GENERAL", "ModelName", "SEV");

            m_pDataSys.SaveString("GENERAL", "SERVO PORT", ServoPort);
            m_pDataSys.SaveString("GENERAL", "IO PORT", IOPort);
            m_pDataSys.SaveString("GENERAL", "BCR SHUTTER PORT", BCR_Shuttle_Port);
            m_pDataSys.SaveString("GENERAL", "BCR BGLASS PORT", BCR_BG_Port);
            m_pDataSys.SaveString("GENERAL", "GMESS PORT PORT", GMESPort);

            m_pDataSys.SaveDouble("GENERAL", "Vaccum On Time", VaccumOnTime);
            m_pDataSys.SaveDouble("GENERAL", "Vaccum Off Time", VaccumOffTime);
            m_pDataSys.SaveDouble("GENERAL", "Grip On Time", GripOnTime);
            m_pDataSys.SaveDouble("GENERAL", "Grip Off Time", GripOffTime);

        }
        public void ReadData() 
        {
            m_pDataSys.ReadString("GENERAL", "Line Name", ref LineName,"Default");
            m_pDataSys.ReadString("GENERAL", "Factory", ref Companny,"SEVT");
            m_pDataSys.ReadString("GENERAL", "ModelName", ref ModelName, "DEFAULT");

            m_pDataSys.ReadString("GENERAL", "SERVO PORT", ref ServoPort, "COM1");
            m_pDataSys.ReadString("GENERAL", "IO PORT", ref IOPort, "COM2");
            m_pDataSys.ReadString("GENERAL", "BCR SHUTTER PORT", ref BCR_Shuttle_Port, "COM3");
            m_pDataSys.ReadString("GENERAL", "BCR BGLASS PORT", ref BCR_BG_Port, "COM4");
            m_pDataSys.ReadString("GENERAL", "GMESS PORT PORT", ref GMESPort, "COM5");

            m_pDataSys.GetDouble("GENERAL", "Vaccum On Time", ref VaccumOnTime);
            m_pDataSys.GetDouble("GENERAL", "Vaccum Off Time", ref VaccumOffTime);
            m_pDataSys.GetDouble("GENERAL", "Grip On Time", ref GripOnTime);
            m_pDataSys.GetDouble("GENERAL", "Grip Off Time", ref GripOffTime);
        }
    }
}

namespace _NScrewMC_C_V1
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class IniFile
    {
        private string filePath;

        public IniFile(string path)
        {
            filePath = path;
        }

        // INI 쓰기
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(
            string section,
            string key,
            string value,
            string filePath);

        // INI 읽기
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
            string section,
            string key,
            string defaultValue,
            StringBuilder returnValue,
            int size,
            string filePath);

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, filePath);
        }

        public string Read(string section, string key, string defaultValue = "")
        {
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString(section, key, defaultValue, sb, 255, filePath);
            return sb.ToString();
        }
    }
}

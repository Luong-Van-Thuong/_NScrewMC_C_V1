using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _NScrewMC_C_V1
{
    public class LogCleaner
    {
        // Định nghĩa các đường dẫn log
        public static string LogScrew1 = @"D:\Log\DataScrew1\";
        public static string LogScrew2 = @"D:\Log\DataScrew2\";
        public static string LogHantasLeft = @"D:\Log\DataHantasLeft\";
        public static string LogHantasRight = @"D:\Log\DataHantasRight\";

        private static Timer cleanupTimer;

        // Cấu hình: xóa file log cũ hơn bao nhiêu ngày
        private static int daysToKeep = 7; // Giữ lại 7 ngày

        // Cấu hình: chạy cleanup mỗi bao nhiêu giờ
        private static int intervalHours = 24; // Chạy mỗi 24 giờ

        public static void StartAutoCleanup()
        {
            try
            {
                // Tạo timer để chạy định kỳ
                cleanupTimer = new Timer(intervalHours * 60 * 60 * 1000); // Convert to milliseconds
                cleanupTimer.Elapsed += OnTimerElapsed;
                cleanupTimer.AutoReset = true;
                cleanupTimer.Enabled = true;


                // Chạy cleanup ngay lần đầu
                CleanupLogs();
            }
            catch (Exception ex)
            {
              
            }
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            CleanupLogs();
        }

        public static void CleanupLogs()
        {
            string[] logPaths = { LogScrew1, LogScrew2, LogHantasLeft, LogHantasRight };

           

            foreach (string logPath in logPaths)
            {
                try
                {
                    CleanupDirectory(logPath);
                }
                catch (Exception ex)
                {
                   
                }
            }

         
        }

        private static void CleanupDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
               
                return;
            }

            try
            {
                DirectoryInfo dir = new DirectoryInfo(directoryPath);
                FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);

                DateTime cutoffDate = DateTime.Now.AddDays(-daysToKeep);
                int deletedCount = 0;
                long deletedSize = 0;

                foreach (FileInfo file in files)
                {
                    try
                    {
                        if (file.CreationTime < cutoffDate && file.LastWriteTime < cutoffDate)
                        {
                            deletedSize += file.Length;
                            file.Delete();
                            deletedCount++;
                            
                        }
                    }
                    catch (Exception ex)
                    {
                       
                    }
                }

                // Xóa các thư mục rỗng
                DeleteEmptyDirectories(dir);

               
            }
            catch (Exception ex)
            {
               
            }
        }

        private static void DeleteEmptyDirectories(DirectoryInfo directory)
        {
            try
            {
                foreach (DirectoryInfo subDirectory in directory.GetDirectories())
                {
                    DeleteEmptyDirectories(subDirectory);

                    if (subDirectory.GetFiles().Length == 0 && subDirectory.GetDirectories().Length == 0)
                    {
                        subDirectory.Delete();
                     
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private static string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }

        public static void StopAutoCleanup()
        {
            cleanupTimer?.Stop();
            cleanupTimer?.Dispose();
           
        }

       
        public static void SetCleanupConfig(int keepDays, int intervalHours)
        {
            daysToKeep = keepDays;
            LogCleaner.intervalHours = intervalHours;

           
            if (cleanupTimer != null)
            {
                StopAutoCleanup();
                StartAutoCleanup();
            }
        }
    }
}

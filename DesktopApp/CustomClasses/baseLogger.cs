using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatnedTestMatic
{
    public static class Logger
    {
        private static readonly string logFilePath;
        private static int logLineNumber = 0;

        static Logger()
        {
            // Determine the application-specific temporary folder
            string tempFolderPath = Path.Combine(Path.GetTempPath(), "PL-TestMatic"); 

            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            logFilePath = Path.Combine(tempFolderPath, "pl-application_log.log");

            if (File.Exists(logFilePath))
            {
                logLineNumber = File.ReadAllLines(logFilePath).Length;
            }
        }

        public static void Log(string message, string logType = "Info")
        {
            logLineNumber++; 

            string logEntry = $"{logLineNumber}, {DateTime.Now}, {logType}, {message}";

            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write log: {ex.Message}");
            }
        }
    }
}

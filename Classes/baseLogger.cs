using DocumentFormat.OpenXml.Office.CustomXsn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PlatnedMahara.Classes
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
            if (LoadConfigData())
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

        private static bool LoadConfigData()
        {
            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pl-application_config.xml");
            bool LoggingEnabled = false;

            if (!string.IsNullOrEmpty(configFilePath))
            {
                if (File.Exists(configFilePath))
                {
                    try
                    {
                        var configXml = XDocument.Load(configFilePath);

                        LoggingEnabled = Convert.ToBoolean(configXml.Root.Element("LoggingEnabled")?.Value ?? bool.FalseString);
                        return LoggingEnabled;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"Error loading configuration!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return LoggingEnabled;
                    }
                }
                else
                {
                    var configXml = new XDocument(
                        new XElement("Configuration",
                            new XElement("AccessTokenUrl", ""),
                            new XElement("ClientId", ""),
                    new XElement("ClientSecret", ""),
                            new XElement("Scope", ""),
                            new XElement("LoggingEnabled", false)
                        )
                    );

                    configXml.Save(configFilePath);
                    Log($"Blank configurations saved to location: {configFilePath}");

                    return LoggingEnabled;
                }
            }
            else
            {
                Log("Config file path is null or empty.");
                return LoggingEnabled;
            }
        }

    }
}

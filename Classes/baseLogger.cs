using ABI.System;
using DocumentFormat.OpenXml.Office.CustomXsn;
using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing;
using Newtonsoft.Json.Linq;
using PlatnedMahara.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.System;
using static PlatnedMahara.Classes.ApiExecution;

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
                    //SendLogsToPlatned(logLineNumber, logType, message);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"Failed to write log: {ex.Message}");
                }
            }
        }

        private static Boolean LoadConfigData()
        {
            var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pl-application_config.xml");
            Boolean LoggingEnabled = false;

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
                    catch (System.Exception ex)
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
                    Logger.Log($"Blank configurations saved to location: {configFilePath}");

                    return LoggingEnabled;
                }
            }
            else
            {
                Logger.Log("Config file path is null or empty.");
                return LoggingEnabled;
            }
        }

        private static async void SendLogsToPlatned(int logLineNumber, string logType, string message) {
            var requestBody = $@"
                                        {{
                                            ""CompanyId"": ""{GlobalData.CompanyId}"",
                                            ""UserId"": ""{GlobalData.UserId}"",
                                            ""LogLineNumber"": ""{logLineNumber}"",
                                            ""LogDate"": ""{DateTime.Now}"",
                                            ""LogType"": ""{logType}"",
                                            ""LogDescription"": ""{message}""
                                        }}";
            
            var url = "https://ifscloud-demo.platnedcloud.com/main/ifsapplications/projection/v1/PassAppLogsHandling.svc/AppLogSet";
            PageConfig basicDataForm = new PageConfig();
            var token = await basicDataForm.RefreshToken();

            if (token != null)
            {
                ApiExecution api = new ApiExecution();
                var apiResponse = await api.Post(url, "", requestBody, token);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage;
using Microsoft.UI.Text;
using Microsoft.UI;
using Windows.UI;
using ClosedXML.Excel;
using System.Xml.Linq;
using Windows.Media.Protection.PlayReady;
using ABI.System;
using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing;
using static PlatnedMahara.Classes.ApiExecution;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using ClosedXML.Graphics;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using PlatnedMahara.Classes;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatnedMahara.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageLicense : Microsoft.UI.Xaml.Controls.Page
    {



        protected Boolean appLoggingEnabled = false; 
        private readonly string configFilePath = GlobalData.configFilePath;
        protected string accessTokenUrl = "";
        protected string clientId = "";
        protected string clientSecret = "";
        protected string scope = "";
        protected Boolean validLicense = false;
        protected string licenseKey = "";
        private string token = "";

        protected static string accessTokenUrlPl = GlobalData.AccessTokenUrlPl;
        protected static string clientIdPl = GlobalData.ClientIdPl;
        protected static string clientSecretPl = GlobalData.ClientSecretPl;
        protected static string scopePl = GlobalData.ScopePl;
        protected static string userId = GlobalData.UserId;
        protected static string companyId = GlobalData.CompanyId;

        private static readonly HttpClient client = new HttpClient();

        public PageLicense()
        {
            this.InitializeComponent();
            LoadConfigData();
        }

        private async void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            validateLicenseAsync();
        }

        private async Task<bool> validateLicenseAsync()
        {
            try
            {
                if (File.Exists(configFilePath))
                {
                    Logger.Log("Saved basic data configurations found!");

                    try
                    {
                        Logger.Log("Reading saved configuration started...");
                        Logger.Log($"Configuration path: {configFilePath}");
                        var configXml = XDocument.Load(configFilePath);
                        Logger.Log($"Configuration reading completed: {configXml}");

                        accessTokenUrl = configXml.Root.Element("AccessTokenUrl")?.Value ?? string.Empty;
                        clientId = configXml.Root.Element("ClientId")?.Value ?? string.Empty;
                        clientSecret = configXml.Root.Element("ClientSecret")?.Value ?? string.Empty;
                        scope = configXml.Root.Element("Scope")?.Value ?? string.Empty;
                        appLoggingEnabled = Convert.ToBoolean(configXml.Root.Element("LoggingEnabled")?.Value ?? bool.FalseString);
                        Logger.Log("Configuration retrieval completed!");


                        Logger.Log("Changing application registration state...");

                        licenseKey = txtLicenseCode.Text;


                        var jsonBody = $@"
                                        {{
                                            ""Objstate"": ""IfsApp.PassUsersHandling.PassUsersPerCompanyState'Active'"",
                                            ""LicenseKey"": ""{licenseKey}"",
                                            ""UserId"": ""{userId}"",
                                            ""CompanyId"": ""{companyId}""
                                        }}";

                        var jsonDoc    = JsonDocument.Parse(jsonBody);
                        var Objstate   = jsonDoc.RootElement.GetProperty("Objstate").GetString();
                        var LicenseKey = jsonDoc.RootElement.GetProperty("LicenseKey").GetString();
                        var UserId     = jsonDoc.RootElement.GetProperty("UserId").GetString();
                        var CompanyId  = jsonDoc.RootElement.GetProperty("CompanyId").GetString();

                        var baseUrl = $"{GlobalData.BaseUrlPl}/main/ifsapplications/projection/v1/PassUsersHandling.svc/UsersSet";

                        var filter = $"Objstate eq {Objstate} and LicenseKey eq '{LicenseKey}' and UserId eq '{UserId}' and CompanyId eq '{CompanyId}'";

                        var uriBuilder = new UriBuilder(baseUrl)
                        {
                            //Query = $"$filter={System.Uri.EscapeDataString(filter)}"
                            Query = $"$filter={filter}"
                        };

                        token = await AuthPlatnedPass.GetAccessTokenPlatndPass(accessTokenUrlPl, clientIdPl, clientSecretPl, scopePl);


                        string method = "GET";
                        string url = uriBuilder.ToString();
                        string headers = "";
                        string requestBody = jsonBody;

                        ApiExecution api = new ApiExecution();
                        ApiExecution.ApiResponse apiResponse = null;

                        Logger.Log("GET - Request body: " + requestBody);
                        apiResponse = await api.Get(url, headers, requestBody, token);


                        Logger.Log($"Response StatusCode={apiResponse.StatusCode}, ResponseBody={apiResponse.ResponseBody}");

                        if (apiResponse != null && (apiResponse.StatusCode == 200 || apiResponse.StatusCode == 201))
                        {
                            var content = apiResponse?.ResponseBody;

                            // Parse the API response content
                            var apiResponseParsed = JsonDocument.Parse(content);

                            // Check if the response contains the License_Key
                            if (apiResponseParsed.RootElement.TryGetProperty("value", out JsonElement valueElement))
                            {
                                foreach (var item in valueElement.EnumerateArray())
                                {
                                    var responseLicenseKey = item.GetProperty("LicenseKey").GetString();
                                    var clientIdValue = item.GetProperty("CompanyId").GetString();

                                    // Check if the license key matches the one from the request
                                    if (responseLicenseKey == LicenseKey)
                                    {
                                        Logger.Log("License Key validated with Platned Pass");

                                        Logger.Log("Saving configuration started...");
                                        PageConfig pageConfig = new PageConfig();
                                        pageConfig.SaveConfigData(accessTokenUrl, clientId, clientSecret, scope, appLoggingEnabled, licenseKey);
                                        Logger.Log("Saving configuration completed!");

                                        if (App.MainWindow is MainWindow mainWindow)
                                        {
                                            mainWindow.ShowInfoBar("Success!", "License Key validated with Platned Pass.", InfoBarSeverity.Success);
                                        }

                                        return validLicense;
                                    }
                                    else
                                    {
                                        Logger.Log("License key does not match.");
                                        if (App.MainWindow is MainWindow mainWindow)
                                        {
                                            mainWindow.ShowInfoBar("Error!", "License Key rejected by Platned Pass.", InfoBarSeverity.Error);
                                        }

                                        return validLicense;
                                    }
                                }
                            }
                            else
                            {
                                Logger.Log("License Key not found in the response.");
                                if (App.MainWindow is MainWindow mainWindow)
                                {
                                    mainWindow.ShowInfoBar("Attention!", "Please enter a valid License Key.", InfoBarSeverity.Warning);
                                }

                                return validLicense;
                            }
                        }
                        else
                        {
                            Logger.Log($"Error: {apiResponse.StatusCode}");
                            if (App.MainWindow is MainWindow mainWindow)
                            {
                                mainWindow.ShowInfoBar("Attention!", "Please enter a valid License Key.", InfoBarSeverity.Warning);
                            }
                            return validLicense;
                        }



                    }
                    catch (System.Exception ex)
                    {
                        Logger.Log($"Error loading configuration: {ex.Message}", "Error");
                        return validLicense;
                    }
                }
                else
                {
                    Logger.Log("No saved basic data configurations found!");
                    return validLicense;
                }

            }
            catch (System.Exception ex)
            {
                Logger.Log($"Authentication failed: {ex.Message}", "Error");
            }

            if (!validLicense)
            {
                Logger.Log("License Key not found in the response.");
                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Attention!", "Please enter a valid License Key.", InfoBarSeverity.Warning);
                }
            }

            return validLicense;
        }


        private void LoadConfigData()
        {
            if (File.Exists(configFilePath))
            {
                Logger.Log("Saved basic data configurations found!");

                try
                {
                    Logger.Log("Reading saved configuration started...");
                    var configXml = XDocument.Load(configFilePath);
                    Logger.Log($"Configuration reading completed: {configXml}");

                    txtLicenseCode.Text = (configXml.Root.Element("licenseKey")?.Value ?? String.Empty);

                    Logger.Log("Configuration retrieval completed!");
                }
                catch (System.Exception ex)
                {
                    Logger.Log($"Error loading configuration: {ex.Message}", "Error");
                    //MessageBox.Show($"Error loading configuration! Refer to application logs for more info.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Logger.Log("No saved basic data configurations found!");
            }
        }

        





    }
}

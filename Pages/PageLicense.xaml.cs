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
using PlatnedMahara.Classes;
using System.Xml.Linq;
using Windows.Media.Protection.PlayReady;
using ABI.System;
using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using ClosedXML.Graphics;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using System.Net.NetworkInformation;
using Newtonsoft.Json;


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
        private readonly string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pl-application_config.xml");
        protected string accessTokenUrl = "";
        protected string clientId = "";
        protected string clientSecret = "";
        protected string scope = "";
        protected Boolean validLicense = false;
        protected string licenseKey = "";
        private string token = "";

        protected static string accessTokenUrlPl = "https://cloud.platnedcloud.com/auth/realms/platprd/protocol/openid-connect/token";
        protected static string clientIdPl = "Plat_APT_Service";
        protected static string clientSecretPl = "JawpIl0UXtCABshpP8TlWHFL9ghtzGue";
        protected static string scopePl = "openid microprofile-jwt";
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

                        var userMac = GetMacAddress();

                        Logger.Log("Changing application registration state...");

                        licenseKey = txtLicenseCode.Text;

                        var jsonBody = $@"
                                        {{
                                            ""Cf_Active"": ""true"",
                                            ""Cf_License_Key"": ""{licenseKey}""
                                        }}";

                        var jsonDoc = JsonDocument.Parse(jsonBody);
                        var cfActive = jsonDoc.RootElement.GetProperty("Cf_Active").GetString();
                        var cfLicenseKey = jsonDoc.RootElement.GetProperty("Cf_License_Key").GetString();

                        var baseUrl = "https://cloud.platnedcloud.com/main/ifsapplications/projection/v1/AptLicensing.svc/AptLicensingSet";

                        var filter = $"Cf_Active eq {cfActive.ToLower()} and Cf_License_Key eq '{cfLicenseKey}'";

                        var uriBuilder = new UriBuilder(baseUrl)
                        {
                            Query = $"$filter={System.Uri.EscapeDataString(filter)}"
                        };

                        token = await GetAccessTokenPlatndPass(accessTokenUrlPl, clientIdPl, clientSecretPl, scopePl);


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

                            // Check if the response contains the Cf_License_Key
                            if (apiResponseParsed.RootElement.TryGetProperty("value", out JsonElement valueElement))
                            {
                                foreach (var item in valueElement.EnumerateArray())
                                {
                                    var responseLicenseKey = item.GetProperty("Cf_License_Key").GetString();
                                    var responseMacAddress = item.GetProperty("Cf_Mac_Address").GetString();
                                    var objkey = item.GetProperty("Objkey").GetString();
                                    var clientIdValue = item.GetProperty("Cf_Client_Id").GetString();

                                    // Check if the license key matches the one from the request
                                    if (responseLicenseKey == cfLicenseKey)
                                    {
                                        Logger.Log("License Key validated with Platned Pass");

                                        if (string.IsNullOrEmpty(responseMacAddress))
                                        {
                                            bool updateSuccessful = await UpdateMacAddressAsync(clientIdValue, objkey, userMac, token);

                                            if (updateSuccessful)
                                            {
                                                Logger.Log("Saving configuration started...");
                                                PageConfig pageConfig = new PageConfig();
                                                pageConfig.SaveConfigData(accessTokenUrl, clientId, clientSecret, scope, appLoggingEnabled, licenseKey);
                                                Logger.Log("Saving configuration completed!");

                                                if (App.MainWindow is MainWindow mainWindow)
                                                {
                                                    mainWindow.ShowInfoBar("Success!", "License Key validated with Platned Pass.", InfoBarSeverity.Success);
                                                }

                                                return true;
                                            }
                                            else
                                            {
                                                if (App.MainWindow is MainWindow mainWindow)
                                                {
                                                    mainWindow.ShowInfoBar("Attention!", "MAC Address registration issue found! Please contact Platned.", InfoBarSeverity.Warning);
                                                }
                                            }
                                        }
                                        else if (userMac != responseMacAddress)
                                        {
                                            if (App.MainWindow is MainWindow mainWindow)
                                            {
                                                mainWindow.ShowInfoBar("Attention!", "Registered MAC Address mismatch found! Please contact Platned.", InfoBarSeverity.Warning);
                                            }
                                        }else if (userMac == responseMacAddress)
                                        {
                                            Logger.Log("Saving configuration started...");
                                            PageConfig pageConfig = new PageConfig();
                                            pageConfig.SaveConfigData(accessTokenUrl, clientId, clientSecret, scope, appLoggingEnabled, licenseKey);
                                            Logger.Log("Saving configuration completed!");

                                            if (App.MainWindow is MainWindow mainWindow)
                                            {
                                                mainWindow.ShowInfoBar("Success!", "License Key validated with Platned Pass.", InfoBarSeverity.Success);
                                            }

                                            return true;
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
                                Logger.Log("Cf_License_Key not found in the response.");
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
                //MessageBox.Show($"Authentication failed! Refer to application logs for more info.");
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

        private async Task<string> GetAccessTokenPlatndPass(string accessTokenUrl, string clientId, string clientSecret, string scope)
        {
            Logger.Log("Encoding the client ID and secret started...");
            var clientCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            Logger.Log("Encoding the client ID and secret completed.");

            Logger.Log("Creating the request message started...");
            var request = new HttpRequestMessage(HttpMethod.Post, accessTokenUrl);
            Logger.Log($"Created request: {request}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", clientCredentials);
            Logger.Log("Creating the request message completed.");

            Logger.Log("Preparing the content (form-urlencoded) started...");
            var content = new StringContent($"grant_type=client_credentials&scope={scope}", Encoding.UTF8, "application/x-www-form-urlencoded");
            Logger.Log($"Created content: {content}");
            request.Content = content;
            Logger.Log("Preparing the content (form-urlencoded) completed.");

            Logger.Log("Send the request started...");
            var response = await client.SendAsync(request);
            Logger.Log($"Received response: {response}");
            response.EnsureSuccessStatusCode();
            Logger.Log("Send the request completed.");

            Logger.Log("Parsing the response and extracting the access token started...");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonResponse);
            Logger.Log("Parsing the response and extracting the access token completed.");

            return tokenResponse.access_token;
        }

        private static string GetMacAddress()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            // Prioritize Ethernet (LAN) interfaces, which are more likely to have permanent MAC addresses
            var macAddress = networkInterfaces
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up &&
                              nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                              !nic.Description.ToLower().Contains("virtual") && // Skip virtual network interfaces
                              !nic.Description.ToLower().Contains("pseudo"))   // Skip pseudo or loopback interfaces
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();

            // If no Ethernet, fall back to any other available interface
            if (string.IsNullOrEmpty(macAddress))
            {
                macAddress = networkInterfaces
                    .Where(nic => nic.OperationalStatus == OperationalStatus.Up &&
                                  nic.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                                  !nic.Description.ToLower().Contains("virtual"))
                    .Select(nic => nic.GetPhysicalAddress().ToString())
                    .FirstOrDefault();
            }

            // Insert colons between every two characters
            string formattedMac = string.Join(":", Enumerable.Range(0, macAddress.Length / 2).Select(i => macAddress.Substring(i * 2, 2)));

            Logger.Log($"User MAC Address Retrieved: {formattedMac}");
            return formattedMac;
        }

        // Method to POST MAC Address
        private async Task<bool> UpdateMacAddressAsync(string clientIdValue, string secondObjKey, string userMac, string token)
        {
            try
            {
                // Base URL for the GET request
                var baseUrl = "https://cloud.platnedcloud.com/main/ifsapplications/projection/v1/AptLicensing.svc/AptLicensingOverviewSet";

                // JSON body containing Cf_Client_Id
                var jsonBody = $@"
                        {{
                            ""Cf_Client_Id"": ""{clientIdValue}""
                        }}";

                // Filter based on Cf_Client_Id
                var filter = $"Cf_Client_Id eq IfsApp.AptLicensing.CfEnum_AptLicensingClient'{clientIdValue}'";

                // Build the GET request URL with the filter applied
                var uriBuilder = new UriBuilder(baseUrl)
                {
                    Query = $"$filter={System.Uri.EscapeDataString(filter)}&$select=Cf_Client_Id,Cf_Active,Cf_License_Purchased,Cf_License_Consumed,Objgrants,luname,keyref&$skip=0&$top=25"
                };

                string url = uriBuilder.ToString();

                // Sending GET request with Cf_Client_Id in the body
                ApiExecution api = new ApiExecution();
                ApiExecution.ApiResponse apiResponse = await api.Get(url, "", jsonBody, token);

                if (apiResponse != null && (apiResponse.StatusCode == 200 || apiResponse.StatusCode == 201))
                {
                    var content = apiResponse?.ResponseBody;
                    var apiResponseParsed = JsonDocument.Parse(content);

                    if (apiResponseParsed.RootElement.TryGetProperty("value", out JsonElement valueElement))
                    {
                        foreach (var item in valueElement.EnumerateArray())
                        {
                            var firstObjKey = item.GetProperty("Objkey").GetString();

                            var patchJsonBody = $@"
                                    {{
                                        ""Cf_Mac_Address"": ""{userMac}""
                                    }}";

                            // Create PATCH URL with the retrieved Objkeys
                            string patchUrl = $"https://cloud.platnedcloud.com/main/ifsapplications/projection/v1/AptLicensing.svc/AptLicensingOverviewSet(Objkey='{firstObjKey}')/ClientId_to_ClientId(Objkey='{secondObjKey}')?select-fields=Cf_Mac_Address";

                            // Sending PATCH request
                            apiResponse = await api.Patch(patchUrl, "", patchJsonBody, token);

                            if (apiResponse != null && (apiResponse.StatusCode == 200 || apiResponse.StatusCode == 204))
                            {
                                Logger.Log("PATCH - MAC address updated successfully!");
                                return true; // Return true if update is successful
                            }
                            else
                            {
                                Logger.Log($"Error PATCHing MAC address: {apiResponse.StatusCode}");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        Logger.Log("No valid entries found in the GET response.");
                    }

                }
                else
                {
                    Logger.Log($"Error during GET request: {apiResponse.StatusCode}");
                }
            }
            catch (System.Exception ex)
            {
                Logger.Log($"Error during MAC address update: {ex.Message}", "Error");
            }
            return false; // Return false if any errors occur
        }






    }
}

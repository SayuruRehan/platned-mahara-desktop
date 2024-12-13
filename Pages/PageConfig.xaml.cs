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
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using PlatnedMahara.Classes;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PlatnedMahara.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageConfig : Page
    {
        private static readonly HttpClient client = new HttpClient();
        //private readonly string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pl-application_config.xml");
        private readonly string configFilePath = GlobalData.configFilePath;
        protected string accessTokenUrl = "";
        protected string clientId = "";
        protected string clientSecret = "";
        protected string scope = "";
        protected string accessToken = "";
        protected Boolean appLoggingEnabled = false;
        protected string licenseKey = "";

        public PageConfig()
        {
            this.InitializeComponent();
            Logger.Log("Retrieving basic data if exists...");
            LoadConfigData();
        }

        private async void btnAuthenticate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Log("Trying to retrieve Access Token...");

                accessTokenUrl = txtAccessTokenUrl.Text;
                clientId = txtClientId.Text;
                clientSecret = txtClientSecret.Password;
                scope = txtScope.Text;

                var token = await GetAccessToken();
                Logger.Log($"Access Token retrieved: {token}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                accessToken = token;
                Logger.Log("Authenticated successfully!");
                btnAuthenticate.IsEnabled = false;
                btnResetAuth.IsEnabled = true;
                Logger.Log("Saving configuration started...");
                SaveConfigData(accessTokenUrl, clientId, clientSecret, scope, appLoggingEnabled, licenseKey);
                Logger.Log("Saving configuration completed!");                

                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Success!", "Authenticated successfully!", InfoBarSeverity.Success);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Authentication failed: {ex.Message}", "Error");

                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Error!", "Authentication failed! Refer to application logs for more info!", InfoBarSeverity.Error);
                }
            }
        }

        private async Task<string> GetAccessToken()
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

        private void btnResetAuthBasicData_Click(object sender, RoutedEventArgs e)
        {
            txtClientId.Text = "";
            txtClientSecret.Password = "";
            txtAccessTokenUrl.Text = "";
            txtScope.Text = "";

            btnAuthenticate.IsEnabled = true;
            btnResetAuth.IsEnabled = false;
            Logger.Log("Basic Data has been reset.");

            if (App.MainWindow is MainWindow mainWindow)
            {
                mainWindow.ShowInfoBar("Success!", "Basic Data has been reset.", InfoBarSeverity.Informational);
            }
        }

        public void SaveConfigData(string accessTokenUrl, string clientId, string clientSecret, string scope, Boolean appLoggingEnabled = false, string licenseKey = "")
        {
            Logger.Log("Saving configurations...");
            var configXml = new XDocument(
                new XElement("Configuration",
                    new XElement("AccessTokenUrl", accessTokenUrl),
                    new XElement("ClientId", clientId),
                    new XElement("ClientSecret", clientSecret),
                    new XElement("Scope", scope),
                    new XElement("LoggingEnabled", appLoggingEnabled),
                    new XElement("licenseKey", licenseKey)
                )
            );

            configXml.Save(configFilePath);
            Logger.Log($"Configurations saved to location: {configFilePath}");
        }

        private void LoadConfigData()
        {
            if (File.Exists(configFilePath))
            {
                Logger.Log("Saved basic data configurations found!");

                btnAuthenticate.IsEnabled = false;
                btnResetAuth.IsEnabled = true;

                try
                {
                    Logger.Log("Reading saved configuration started...");
                    var configXml = XDocument.Load(configFilePath);
                    Logger.Log($"Configuration reading completed: {configXml}");

                    txtAccessTokenUrl.Text = configXml.Root.Element("AccessTokenUrl")?.Value ?? string.Empty;
                    txtClientId.Text = configXml.Root.Element("ClientId")?.Value ?? string.Empty;
                    txtClientSecret.Password = configXml.Root.Element("ClientSecret")?.Value ?? string.Empty;
                    txtScope.Text = configXml.Root.Element("Scope")?.Value ?? string.Empty;
                    appLoggingEnabled = Convert.ToBoolean(configXml.Root.Element("LoggingEnabled")?.Value ?? bool.FalseString);
                    licenseKey = (configXml.Root.Element("licenseKey")?.Value ?? String.Empty);

                    if (txtAccessTokenUrl.Text == string.Empty && txtClientId.Text == string.Empty && txtClientSecret.Password == string.Empty && txtScope.Text == string.Empty)
                    {
                        btnAuthenticate.IsEnabled = true;
                        btnResetAuth.IsEnabled = false;
                    }

                    Logger.Log("Configuration retrieval completed!");
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error loading configuration: {ex.Message}", "Error");
                    //MessageBox.Show($"Error loading configuration! Refer to application logs for more info.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                btnAuthenticate.IsEnabled = true;
                btnResetAuth.IsEnabled = false;

                Logger.Log("No saved basic data configurations found!");
            }
        }

        public async Task<string> RefreshToken()
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
                    Logger.Log("Configuration retrieval completed!");

                    Logger.Log("Trying to retrieve Access Token...");
                    accessToken = await GetAccessToken();
                    Logger.Log($"Access Token retrieved: {accessToken}");

                    return accessToken;
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error loading configuration: {ex.Message}", "Error");
                    return "Error loading configuration! Refer to application logs for more info.";
                }
            }
            else
            {
                Logger.Log("No saved basic data configurations found!");
                return "No saved basic data configurations found!";
            }
        }

        private void RevealModeCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            if (revealModeCheckBox.IsChecked == true)
            {
                txtClientSecret.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                txtClientSecret.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }

    }
}

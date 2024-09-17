using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PlatnedTestMatic
{
    public partial class frmBasicData : frmBaseForm
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pl-application_config.xml");       
        protected string accessTokenUrl = "";
        protected string clientId = "";
        protected string clientSecret = "";
        protected string scope = "";
        protected string accessToken = "";
        protected Boolean appLoggingEnabled = false;

        public frmBasicData()
        {
            InitializeComponent();
            Logger.Log("Retrieving basic data if exists...");
            LoadConfigData();
        }

        private async void btnAuthenticate_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Log("Trying to retrieve Access Token...");

                accessTokenUrl   = txtAccessTokenUrl.Text;
                clientId         = txtClientId.Text;
                clientSecret     = txtClientSecret.Text;
                scope            = txtScope.Text;

                var token = await GetAccessToken();
                Logger.Log($"Access Token retrieved: {token}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                accessToken = token;
                Logger.Log("Authenticated successfully!");
                btnAuthenticate.Enabled = false;
                chkEnableLogging.Enabled = true;

                Logger.Log("Saving configuration started...");
                SaveConfigData(accessTokenUrl, clientId, clientSecret, scope);
                Logger.Log("Saving configuration completed!");

                MessageBox.Show("Authenticated successfully!");
            }
            catch (Exception ex)
            {
                Logger.Log($"Authentication failed: {ex.Message}", "Error");
                MessageBox.Show($"Authentication failed! Refer to application logs for more info.");
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

        private void btnResetAuthBasicData_Click(object sender, EventArgs e)
        {
            txtClientId.Text        = "";
            txtClientSecret.Text    = "";
            txtAccessTokenUrl.Text  = "";
            txtScope.Text           = "";

            btnAuthenticate.Enabled         = true;
            btnResetAuthBasicData.Enabled   = false;
            Logger.Log("Basic Data has been reset.");
        }

        private void SaveConfigData(string accessTokenUrl, string clientId, string clientSecret, string scope, Boolean appLoggingEnabled = false)
        {
            Logger.Log("Saving configurations...");
            var configXml = new XDocument(
                new XElement("Configuration",
                    new XElement("AccessTokenUrl", accessTokenUrl),
                    new XElement("ClientId", clientId),
                    new XElement("ClientSecret", clientSecret),
                    new XElement("Scope", scope),
                    new XElement("LoggingEnabled", appLoggingEnabled)
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

                btnAuthenticate.Enabled         = false;
                btnResetAuthBasicData.Enabled   = true;

                try
                {
                    Logger.Log("Reading saved configuration started...");
                    var configXml = XDocument.Load(configFilePath);
                    Logger.Log($"Configuration reading completed: {configXml}");

                    txtAccessTokenUrl.Text = configXml.Root.Element("AccessTokenUrl")?.Value ?? string.Empty;
                    txtClientId.Text = configXml.Root.Element("ClientId")?.Value ?? string.Empty;
                    txtClientSecret.Text = configXml.Root.Element("ClientSecret")?.Value ?? string.Empty;
                    txtScope.Text = configXml.Root.Element("Scope")?.Value ?? string.Empty;
                    chkEnableLogging.Checked = Convert.ToBoolean(configXml.Root.Element("LoggingEnabled")?.Value ?? bool.FalseString);
                    Logger.Log("Configuration retrieval completed!");
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error loading configuration: {ex.Message}", "Error");
                    MessageBox.Show($"Error loading configuration! Refer to application logs for more info.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else {
                btnAuthenticate.Enabled         = true;
                btnResetAuthBasicData.Enabled   = false;

                Logger.Log("No saved basic data configurations found!");
            }
        }

        private void btnAuthenticate_MouseEnter(object sender, EventArgs e)
        {
            btnAuthenticate.Image = Properties.Resources.Auth;
            btnAuthenticate.Text = "";
        }

        private void btnAuthenticate_MouseLeave(object sender, EventArgs e)
        {
            btnAuthenticate.Image = null;
            btnAuthenticate.Text = "Authenticate";
        }

        private void btnResetAuthBasicData_MouseEnter(object sender, EventArgs e)
        {
            btnResetAuthBasicData.Image = Properties.Resources.Reset;
            btnResetAuthBasicData.Text = "";
        }

        private void btnResetAuthBasicData_MouseLeave(object sender, EventArgs e)
        {
            btnResetAuthBasicData.Image = null;
            btnResetAuthBasicData.Text = "Reset";
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

                    accessTokenUrl  = configXml.Root.Element("AccessTokenUrl")?.Value ?? string.Empty;
                    clientId        = configXml.Root.Element("ClientId")?.Value ?? string.Empty;
                    clientSecret    = configXml.Root.Element("ClientSecret")?.Value ?? string.Empty;
                    scope           = configXml.Root.Element("Scope")?.Value ?? string.Empty;
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

        private void chkEnableLogging_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.Log("Changing application logging state...");

                accessTokenUrl = txtAccessTokenUrl.Text;
                clientId = txtClientId.Text;
                clientSecret = txtClientSecret.Text;
                scope = txtScope.Text;
                appLoggingEnabled = chkEnableLogging.Checked;

                Logger.Log("Saving configuration started...");
                SaveConfigData(accessTokenUrl, clientId, clientSecret, scope, appLoggingEnabled);
                Logger.Log("Saving configuration completed!");

                if (appLoggingEnabled)
                {
                    Logger.Log("Application Logging Enabled!");
                    MessageBox.Show("Application Logging Enabled!");
                }
                else
                {
                    Logger.Log("Application Logging Disabled!");
                    MessageBox.Show("Application Logging Disabled!");
                }
                Logger.Log("Changed application logging state.");
            }
            catch (Exception ex)
            {
                Logger.Log($"Authentication failed: {ex.Message}", "Error");
                MessageBox.Show($"Authentication failed! Refer to application logs for more info.");
            }
        }
    }
}

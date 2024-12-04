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

                        if (licenseKey != "")
                        {
                            Pass_Users_Company pass_User_det = new Pass_Users_Company
                            {
                                CompanyID = GlobalData.CompanyId,
                                UserID = GlobalData.UserId
                            };

                            Pass_Users_Company pass_User = new Pass_Users_Company();
                            pass_User = new AuthPlatnedPass().GetPass_User_Per_Company(pass_User_det);

                            if (pass_User != null)
                            {
                                if (pass_User.LicenseKey == licenseKey)
                                {
                                    if (pass_User.ValidTo >= DateTime.Now)
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

                                        validLicense = true;
                                        return validLicense;
                                    }
                                    else
                                    {
                                        Logger.Log($"License key is expired for user: {GlobalData.UserId}.");
                                        if (App.MainWindow is MainWindow mainWindow)
                                        {
                                            mainWindow.ShowInfoBar("Attention!", "License Key is expired!", InfoBarSeverity.Warning);
                                        }

                                        return validLicense;
                                    }

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
                            else
                            {
                                if (App.MainWindow is MainWindow mainWindow)
                                {
                                    mainWindow.ShowInfoBar("Attention!", "Please enter a valid License Key.", InfoBarSeverity.Warning);
                                }

                                return validLicense;
                            }
                        }
                        else
                        {
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

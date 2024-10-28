using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage.Pickers;
using CommunityToolkit.WinUI.UI.Controls;
using System.Data;
using System.Collections.ObjectModel;
using PL_PlatnedTestMatic.Classes;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Threading;
using ClosedXML.Excel;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using System.Net.Http.Json;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PL_PlatnedTestMatic.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageHome : Microsoft.UI.Xaml.Controls.Page
    {
        public ObservableCollection<GridItem> GridItems { get; set; }
        private string uploadedJSONFilePath;
        private string uploadedCSVFilePath;
        //private DataGridView dgvTestResults;
        private int totalIterations;
        private string token = "";
        private List<JObject> apiCalls;
        private DataTable csvData;
        private string tempFolderPath;
        private bool testingStausFailed = false;
        string jsonFilePath;
        string csvFilePath;
        bool errorFound = false;
        private CancellationTokenSource cancellationTokenSource;
        private readonly string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pl-application_config.xml");
        private string entitySet = "";
        private string entitySetParam = "";
        private string entitySetArray = "";

        public PageHome()
        {
            this.InitializeComponent();
            LoadData();
            dataGrid.ItemsSource = GridItems;
        }

        public void PrepareConnectedAnimation(ConnectedAnimationConfiguration config)
        {
            var animJson = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", SourceJsonElement);
            var animCsv = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", SourceJsonElement);
            var animTest = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", SourceJsonElement);

            if (config != null && ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                animJson.Configuration = config;
                animCsv.Configuration = config;
                animTest.Configuration = config;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var animJson = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackwardConnectedAnimation");
            var animCsv = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackwardConnectedAnimation");
            var animTest = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackwardConnectedAnimation");

            if (animJson != null)
            {
                animJson.TryStart(SourceJsonElement);
            }
            if (animCsv != null)
            {
                animCsv.TryStart(SourceJsonElement);
            }
            if (animTest != null)
            {
                animTest.TryStart(SourceJsonElement);
            }
        }

        private async void PickJsonFileButton_Click(object sender, RoutedEventArgs e)
        {
            PickJsonFileOutputTextBlock.Text = "";

            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            var window = App.MainWindow;

            if (window != null)
            {
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

                WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.FileTypeFilter.Add(".json");

                var file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    try
                    {
                        string sourceFilePath = file.Path;
                        string tempFolderPath = Path.Combine(Path.GetTempPath(), "PL-TestMatic");

                        if (!Directory.Exists(tempFolderPath))
                        {
                            Directory.CreateDirectory(tempFolderPath);
                        }

                        string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(sourceFilePath));

                        File.Copy(sourceFilePath, tempFilePath, true);

                        uploadedJSONFilePath = tempFilePath;

                        PickJsonFileOutputTextBlock.Text = file.Name;
                        PickCsvFileButton.IsEnabled = true;

                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Success!", "JSON file successfully uploaded.", InfoBarSeverity.Success);
                        }
                        Logger.Log($"JSON file uploaded to location: {tempFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"An error occurred: {ex.Message}", "Error");
                        //MessageBox.Show($"An error occurred. Refer to application logs for more info.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    PickJsonFileOutputTextBlock.Text = "Operation cancelled.";
                }
            }
            else
            {
                PickJsonFileOutputTextBlock.Text = "Could not retrieve the window handle.";
                Logger.Log("JSON file upload error! Could not retrieve the window handle.", "Error");
            }

        }

        private async void PickCsvFileButton_Click(object sender, RoutedEventArgs e)
        {
            PickCsvFileOutputTextBlock.Text = "";

            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            var window = App.MainWindow;

            if (window != null)
            {
                var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

                WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

                openPicker.ViewMode = PickerViewMode.Thumbnail;
                openPicker.FileTypeFilter.Add(".csv");

                var file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    try
                    {
                        string sourceFilePath = file.Path;
                        string tempFolderPath = Path.Combine(Path.GetTempPath(), "PL-TestMatic");

                        if (!Directory.Exists(tempFolderPath))
                        {
                            Directory.CreateDirectory(tempFolderPath);
                        }

                        string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(sourceFilePath));

                        File.Copy(sourceFilePath, tempFilePath, true);

                        uploadedCSVFilePath = tempFilePath;

                        PickCsvFileOutputTextBlock.Text = file.Name;
                        btnStart.IsEnabled = true;
                        btnRerun.IsEnabled = false;
                        btnStop.IsEnabled = false;

                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Success!", "CSV file successfully uploaded.", InfoBarSeverity.Success);
                        }
                        Logger.Log($"CSV file uploaded to location: {tempFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"An error occurred: {ex.Message}", "Error");
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Error!", "An error occurred. Refer to application logs for more info.", InfoBarSeverity.Error);
                        }
                    }
                }
                else
                {
                    PickCsvFileOutputTextBlock.Text = "Operation cancelled.";
                }
            }
            else
            {
                PickCsvFileOutputTextBlock.Text = "Could not retrieve the window handle.";
                Logger.Log("CSV file upload error! Could not retrieve the window handle.", "Error");
            }
        }

        // DataGrid Code - Start
        private void LoadData()
        {
            GridItems = new ObservableCollection<GridItem>();
            for (int gi = 1; gi <= 1; gi++)
            {
                var item = new GridItem($"Item {gi}")
                {
                    // Initialize with default values
                    TreeNodesItemName = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesApi = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesDesc = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesStatC = new ObservableCollection<TreeNode> { new TreeNode("") },
                    TreeNodesRes = new ObservableCollection<TreeNode> { new TreeNode("") }
                };

                GridItems.Add(item);
            }
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {

            if (LoadConfigData())
            {
                progExec.ShowPaused = false;
                progExec.ShowError = false;
                progExec.IsIndeterminate = true;
                progExec.Visibility = Visibility.Visible;
                btnStart.IsEnabled = false;
                btnRerun.IsEnabled = false;
                btnStop.IsEnabled = true;

                await RunTestIterationsAsync(uploadedJSONFilePath, uploadedCSVFilePath);
            }
            else
            {
                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Error!", "License Key required to proceed. Please register with Platned Pass!", InfoBarSeverity.Error);
                }
            }

        }

        private System.Boolean LoadConfigData()
        {
            string licenseCode = "";

            if (File.Exists(configFilePath))
            {
                Logger.Log("Saved basic data configurations found!");

                try
                {
                    Logger.Log("Reading saved configuration started...");
                    var configXml = XDocument.Load(configFilePath);
                    Logger.Log($"Configuration reading completed: {configXml}");

                    licenseCode = (configXml.Root.Element("licenseKey")?.Value ?? System.String.Empty);

                    Logger.Log("Configuration retrieval completed!");
                }
                catch (System.Exception ex)
                {
                    Logger.Log($"Error loading configuration: {ex.Message}", "Error");
                }

                if (licenseCode != null && licenseCode != "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Logger.Log("No saved basic data configurations found!");
                return false;
            }
        }

        private async Task InitializeAsync()
        {
            tempFolderPath = Path.Combine(Path.GetTempPath(), "PL-TestMatic");

            string jsonContent = File.ReadAllText(jsonFilePath);
            JObject jsonObject = JObject.Parse(jsonContent);
            apiCalls = jsonObject["item"].Select(item => (JObject)item).ToList();
            Logger.Log("API calls loaded from JSON.");

            csvData = new DataTable();
            using (StreamReader reader = new StreamReader(csvFilePath))
            {
                string[] headers = reader.ReadLine().Split(',');
                foreach (string header in headers)
                {
                        csvData.Columns.Add(header);
                }
                Logger.Log("CSV headers loaded.");

                while (!reader.EndOfStream)
                {
                    string[] rows = reader.ReadLine().Split(',');
                    csvData.Rows.Add(rows);
                }
                totalIterations = csvData.Rows.Count;
                Logger.Log($"{totalIterations} data items retrieved from CSV.");
                Logger.Log("CSV data loaded.");
            }
        }


        public async Task RunTestIterationsAsync(string uploadedJSONFilePath, string uploadedCSVFilePath)
        {
            progExec.Visibility = Visibility.Visible;
            lblExecStarted.Text = DateTime.Now.ToString();
            lblExecFinished.Text = "~";
            drpShareResults.IsEnabled = true;

            jsonFilePath = uploadedJSONFilePath;
            csvFilePath = uploadedCSVFilePath;
            Logger.Log("jsonFilePath, csvFilePath received for execution!");
            InitializeAsync();

            GridItems.Clear();
            for (int i = 0; i < totalIterations; i++)
            {
                var item = new GridItem($"Item {GridItems.Count + 1}");

                item.TreeNodesItemName.Add(new TreeNode($"Item {i}"));
                item.TreeNodesApi.Add(new TreeNode("-"));
                item.TreeNodesDesc.Add(new TreeNode("-"));
                item.TreeNodesStatC.Add(new TreeNode("-"));
                item.TreeNodesRes.Add(new TreeNode("Pending..."));

                GridItems.Add(item);
            }

            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            if (string.IsNullOrEmpty(token))
            {
                Logger.Log("No saved token found. Refreshing the token...");
                PageConfig basicDataForm = new PageConfig();
                token = await basicDataForm.RefreshToken();
                Logger.Log("Token refreshed!");
            }

            for (int iteration = 1; iteration <= totalIterations; iteration++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Logger.Log("Test execution cancelled.");
                    progExec.ShowPaused = true;
                    progExec.ShowError = false;
                    progExec.Visibility = Visibility.Visible;
                    if (App.MainWindow is MainWindow mainWindow)
                    {
                        mainWindow.ShowInfoBar("Cancelled!", "Execution successfully cancelled.", InfoBarSeverity.Warning);
                    }
                    return;
                }

                Logger.Log($"Starting iteration {iteration} =============================================> ");
                errorFound = false;
                var apiLoop = 0;
                UpdateIterationStatus(iteration, $"{apiLoop}/{apiCalls.Count}", "", "", "In Progress...");


                foreach (var apiCall in apiCalls)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        Logger.Log("Test execution cancelled.");
                        progExec.ShowPaused = true;
                        progExec.ShowError = false;
                        progExec.Visibility = Visibility.Visible;
                        if (App.MainWindow is MainWindow mainWindow)
                        {
                            mainWindow.ShowInfoBar("Cancelled!", "Execution successfully cancelled.", InfoBarSeverity.Warning);
                        }
                        return;
                    }

                    apiLoop += 1;
                    if (!errorFound)
                    {
                        UpdateIterationStatus(iteration, $"{apiLoop}/{apiCalls.Count}", "", "", "In Progress...");
                        Logger.Log("API Call: " + apiCall);
                        await RunTestIteration(iteration, apiLoop, apiCalls.Count, apiCall);
                    }
                    else
                    {
                        Logger.Log($"Skipped the remaining API calls due to previous error in iteration {iteration}.");
                        break;
                    }
                }
            }

            if (!testingStausFailed)
            {
                progExec.ShowPaused = false;
                progExec.ShowError = false;
                progExec.IsIndeterminate = false;
                progExec.Value = 100;
                progExec.Visibility = Visibility.Visible;
                btnStart.IsEnabled = false;
                btnStop.IsEnabled = false;
                btnRerun.IsEnabled = false;
                lblExecFinished.Text = DateTime.Now.ToString();

                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Success!", "Execution successfully completed.", InfoBarSeverity.Success);
                }
            }
            else
            {
                progExec.ShowPaused = false;
                progExec.ShowError = true;
                progExec.Visibility = Visibility.Visible;
                btnStart.IsEnabled = false;
                btnStop.IsEnabled = false;
                btnRerun.IsEnabled = true;
                lblExecFinished.Text = DateTime.Now.ToString();

                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Error!", "Execution completed with errors.", InfoBarSeverity.Error);
                }
            }
        }

        private async Task RunTestIteration(int iterationNumber, int apiLoop, int apiCount, JObject apiCall)
        {
            string method = apiCall["request"]["method"].ToString();
            string url = apiCall["request"]["url"]["raw"].ToString();

            string bodyRawJson = "";
            if (method == "POST")
            {
                try
                {
                    bodyRawJson = apiCall["request"]["body"]["raw"].ToString();
                }
                catch (Exception e)
                {
                    Logger.Log($"Exception occurred. Possible low priority exception: {e}");
                }
                
            }

            string headers = "";
            string requestBody = "";
            System.Boolean multipartErrorSkip = false;

            ApiExecution api = new ApiExecution();
            ApiExecution.ApiResponse apiResponse = null;

            Logger.Log("Extracting data from CSV for API request");

            InitializeAsync();

            DataRow csvRow = csvData.Rows[iterationNumber - 1];
            Logger.Log($"Extracting data from CSV for API request completed: {csvRow}");
            Dictionary<string, string> csvParameters = new Dictionary<string, string>();

            foreach (DataColumn column in csvData.Columns)
            {
                csvParameters[column.ColumnName] = csvRow[column.ColumnName]?.ToString();
            }

            switch (method)
            {
                case "GET":
                    requestBody = BuildRequestBody(csvParameters);
                    Logger.Log("GET - Request body: " + requestBody);

                    Logger.Log("GET - Constructing URL: " + url);
                    try
                    {
                        url = UrlReconstructor.ReconstructUrl(url, requestBody);
                        Logger.Log("GET - Constructed URL: " + url);
                    }
                    catch (Exception e)
                    {
                        Logger.Log("No parameters found in the URL. Skipping URL Construction.");
                    }

                    apiResponse = await api.Get(url, headers, requestBody, token);
                    break;

                case "POST":
                    requestBody = BuildRequestBody(csvParameters);
                    Logger.Log("POST - Request body: " + requestBody);

                    Logger.Log("POST - Constructing URL: " + url);
                    try
                    {
                        url = UrlReconstructor.ReconstructUrl(url, requestBody);
                        Logger.Log("POST - Constructed URL: " + url);
                    }
                    catch(Exception e)
                    {
                        Logger.Log("No parameters found in the URL. Skipping URL Construction.");
                    }
                    
                    var tempRequestBody = "";

                    if (bodyRawJson != "")
                    {
                        tempRequestBody = FilterRequestBody(bodyRawJson, requestBody);
                        Logger.Log("POST - Filtered Request body: " + requestBody);
                    }
                    if(tempRequestBody != "IGNORE" && bodyRawJson != "")
                    {
                        requestBody = tempRequestBody;
                    }

                    apiResponse = await api.Post(url, headers, requestBody, token);

                    if (apiResponse.ResponseBody.Contains("DATABASE_ERROR") && (apiResponse.ResponseBody.Contains("ACCESS_FAILURE") || apiResponse.ResponseBody.Contains("CASE not found")))
                    {
                        requestBody = BuildRequestBody(csvParameters);
                        Logger.Log("POST - Request body for ValidationRec: " + requestBody);
                        apiResponse = await api.ValidationRec(url, headers, requestBody, token, method, "ValidationRec");
                        break;
                    }
                    if (apiResponse.ResponseBody.Contains("REQUEST_ERROR") && apiResponse.ResponseBody.Contains("UNEXPECTED_CONTENT_TYPE") && apiResponse.ResponseBody.Contains("multipart/mixed"))
                    {
                        if(bodyRawJson != "")
                        {
                            ParseRawContent(bodyRawJson);
                        }
                        else
                        {
                            string jsonContent = File.ReadAllText(jsonFilePath);
                            ParseJson(jsonContent);
                        }
                        
                        requestBody = BuildRequestBody(csvParameters);
                        Logger.Log("POST - Request body for SendMultipartRequest: " + requestBody);
                        apiResponse = await api.SendMultipartRequest(url, headers, requestBody, token, method, "SendMultipartRequest", entitySet, entitySetParam, entitySetArray);
                        break;
                    }

                    break;

                case "PATCH":
                    requestBody = BuildRequestBody(csvParameters);
                    Logger.Log("PATCH - Request body: " + requestBody);

                    Logger.Log("PATCH - Constructing URL: " + url);
                    try
                    {
                        url = UrlReconstructor.ReconstructUrl(url, requestBody);
                        Logger.Log("PATCH - Constructed URL: " + url);
                    }
                    catch (Exception e)
                    {
                        Logger.Log("No parameters found in the URL. Skipping URL Construction.");
                    }

                    apiResponse = await api.Patch(url, headers, requestBody, token);
                    break;

                case "DELETE":
                    Logger.Log("DELETE - Request body: " + requestBody);

                    Logger.Log("DELETE - Constructing URL: " + url);
                    try
                    {
                        url = UrlReconstructor.ReconstructUrl(url, requestBody);
                        Logger.Log("DELETE - Constructed URL: " + url);
                    }
                    catch (Exception e)
                    {
                        Logger.Log("No parameters found in the URL. Skipping URL Construction.");
                    }

                    apiResponse = await api.Delete(url, headers, token);
                    break;

                default:
                    Logger.Log($"Unsupported HTTP method: {method}", "Error");
                    return;
            }

            Logger.Log($"Iteration {iterationNumber}: Response StatusCode={apiResponse.StatusCode}, ResponseBody={apiResponse.ResponseBody}");

            if (apiResponse != null && (apiResponse.StatusCode == 200 || apiResponse.StatusCode == 201) && errorFound != true && !apiResponse.ResponseBody.Contains("error"))
            {
                UpdateIterationStatus(iterationNumber, $"{apiLoop}/{apiCount}", apiResponse.StatusCode.ToString(), "Successful", "OK");
            }
            else if (apiResponse != null && errorFound != true && apiResponse.ResponseBody.Contains("DB_OBJECT_EXIST"))
            {
                UpdateIterationStatus(iterationNumber, $"{apiLoop}/{apiCount}", apiResponse.StatusCode.ToString(), "Successful", "OK");
                multipartErrorSkip = true;

            }
            else if (apiResponse != null && errorFound != true)
            {
                try
                {
                    // Log the raw response for debugging
                    Logger.Log($"Raw Response Body: {apiResponse.ResponseBody}");

                    // Extract the JSON part of the response by searching for the first occurrence of '{' and last occurrence of '}'
                    int jsonStartIndex = apiResponse.ResponseBody.IndexOf('{');
                    int jsonEndIndex = apiResponse.ResponseBody.LastIndexOf('}');

                    if (jsonStartIndex >= 0 && jsonEndIndex >= 0 && jsonEndIndex > jsonStartIndex)
                    {
                        // Extract the substring that contains the JSON
                        string jsonString = apiResponse.ResponseBody.Substring(jsonStartIndex, jsonEndIndex - jsonStartIndex + 1);

                        // Parse the extracted JSON string
                        JObject responseJson = JObject.Parse(jsonString);

                        // Check if there's an error with code "DATABASE_ERROR"
                        var errorCode = responseJson["error"]?["code"]?.ToString();
                        var errorMessage = responseJson["error"]?["message"]?.ToString();
                        var errorDetails = responseJson["error"]?["details"]?.ToObject<JArray>();

                        if (errorCode == "DATABASE_ERROR" && errorDetails != null)
                        {
                            // Loop through error details to check for "LINE_ALREADY_EXISTS" or similar error codes
                            foreach (var detail in errorDetails)
                            {
                                var detailMessage = detail["message"]?.ToString();

                                if (detailMessage != null && detailMessage.Contains("ALREADY_EXISTS"))
                                {
                                    // Handle the specific duplicate line case
                                    UpdateIterationStatus(iterationNumber, $"{apiLoop}/{apiCount}", apiResponse.StatusCode.ToString(), "Successful", "OK");
                                    Logger.Log($"Error _ALREADY_EXISTS found for Multipart Request. Skipping to next API due to duplicate API call. Error Message: {detailMessage}");
                                    multipartErrorSkip = true;
                                    //break; // Skip further iteration as error is found
                                }
                            }
                        }
                    }
                    else
                    {
                        // Log that no valid JSON was found in the response
                        Logger.Log("No valid JSON found in the response body.");
                    }
                    if (apiResponse.StatusCode.ToString() == "401")
                    {
                        errorFound = true;
                        testingStausFailed = true;
                        progExec.ShowError = true;
                        UpdateIterationStatus(iterationNumber, $"{apiLoop}/{apiCount}", apiResponse.StatusCode.ToString(), "Authorization Required!", "Aborted");
                    }
                    else
                    {
                        if (apiResponse.ResponseBody.Contains("error"))
                        {
                            errorFound = true;
                            testingStausFailed = true;
                            UpdateIterationStatus(iterationNumber, $"{apiLoop}/{apiCount}", apiResponse?.StatusCode.ToString() ?? "N/A", apiResponse?.ResponseBody, "Error");
                        }
                        else
                        {
                            UpdateIterationStatus(iterationNumber, $"{apiLoop}/{apiCount}", apiResponse.StatusCode.ToString(), "Successful", "OK");
                        }
                        
                    }

                }
                catch (JsonReaderException ex)
                {
                    // If JSON parsing fails, handle it here
                    Logger.Log($"Failed to parse response body for error handling. Error: {ex.Message}");
                }
            }
            else
            {
                errorFound = true;
                testingStausFailed = true;
                UpdateIterationStatus(iterationNumber, $"{apiLoop}/{apiCount}", apiResponse?.StatusCode.ToString() ?? "N/A", apiResponse?.ResponseBody, "Error");
            }

            WriteDataTableToCsvWithStatus(csvData, csvFilePath, iterationNumber, errorFound, apiResponse.ResponseBody);

            if (!errorFound && apiResponse.ResponseBody.ToString() != "" && multipartErrorSkip != true)
            {
                Logger.Log("Processing CSV for new data...");
                JObject responseJson = new Newtonsoft.Json.Linq.JObject();
                try
                {
                    responseJson = JObject.Parse(apiResponse.ResponseBody);

                    DataRow row = csvData.Rows[iterationNumber - 1];

                    foreach (var property in responseJson.Properties())
                    {
                        try
                        {
                            Logger.Log($"Processing property: '{property.Name}' with value: '{property.Value}'");

                            // Check if the property is 'value', which contains an array of objects
                            if (property.Name == "value" && property.Value.Type == JTokenType.Array)
                            {
                                // Loop through each object in the 'value' array
                                foreach (var item in property.Value)
                                {
                                    if (item.Type == JTokenType.Object)
                                    {
                                        // Process each object in the array as if it were a set of key-value pairs
                                        foreach (var subProperty in item.Children<JProperty>())
                                        {
                                            if (subProperty.Name != "Objstate")
                                            {
                                                ProcessProperty(subProperty, csvData, row, iterationNumber);
                                            }
                                            else
                                            {
                                                var abc = "";
                                            }

                                        }
                                    }
                                }
                            }
                            else if (!new[] { "error", "@odata.context" }.Contains(property.Name) && !property.Name.Contains("Ref") && !property.Name.Contains("Objstate"))
                            {
                                ProcessProperty(property, csvData, row, iterationNumber);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"Error processing property '{property.Name}': {ex.Message}");
                        }
                    }

                }
                catch (JsonReaderException ex)
                {
                    if (ex.Message.Contains("Unexpected character encountered while parsing number: t"))
                    {
                        Console.WriteLine("Ignoring error: " + ex.Message);
                        Logger.Log($"Possible OK 200 Response! Ignoring error: {ex.Message}");
                    }
                    else
                    {
                        Logger.Log($"Recommended to investigate. Ignoring error: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Recommended to investigate. Ignoring error: {ex.Message}");
                }


            }
            multipartErrorSkip = false;

        }

        private void ProcessProperty(JProperty property, DataTable csvData, DataRow row, int iterationNumber)
        {
            try
            {
                // Skip properties we don't want to include
                if (!csvData.Columns.Contains(property.Name))
                {
                    string sanitizedColumnName = property.Name.Replace("@", "").Replace(".", "_");

                    JTokenType propertyType = property.Value.Type;
                    Type columnType = typeof(string);

                    switch (propertyType)
                    {
                        case JTokenType.Integer:
                            columnType = typeof(double);
                            break;
                        case JTokenType.Float:
                            columnType = typeof(double);
                            break;
                        case JTokenType.Boolean:
                            columnType = typeof(bool);
                            break;
                        case JTokenType.Date:
                            columnType = typeof(DateTime);
                            break;
                        case JTokenType.Null:
                            columnType = typeof(DBNull);
                            break;
                        default:
                            columnType = typeof(string);
                            break;
                    }

                    csvData.Columns.Add(sanitizedColumnName, columnType);
                    Logger.Log($"Iteration {iterationNumber}: Added new column '{sanitizedColumnName}' of type '{columnType.Name}' to CSV.");
                }

                string propertyName = property.Name.Replace("@", "").Replace(".", "_");
                if (row[propertyName] == DBNull.Value || string.IsNullOrEmpty(row[propertyName]?.ToString()))
                {
                    if (property.Value.Type != JTokenType.Null)
                    {
                        switch (property.Value.Type)
                        {
                            case JTokenType.Integer:
                                row[propertyName] = property.Value.ToObject<int>();
                                break;
                            case JTokenType.Float:
                                row[propertyName] = property.Value.ToObject<double>();
                                break;
                            case JTokenType.Boolean:
                                row[propertyName] = property.Value.ToObject<bool>();
                                break;
                            case JTokenType.Date:
                                row[propertyName] = property.Value.ToObject<DateTime>();
                                break;
                            default:
                                row[propertyName] = property.Value.ToString();
                                break;
                        }

                        Logger.Log($"Iteration {iterationNumber}: Updated column '{propertyName}' with value '{row[propertyName]}' of type '{row[propertyName].GetType().Name}'.");
                    }
                }

                WriteDataTableToCsv(csvData, csvFilePath);

                Logger.Log($"Iteration {iterationNumber}: CSV file updated.");

            }
            catch (Exception ex)
            {
                Logger.Log($"Error processing property '{property.Name}': {ex.Message}");
            }
        }

        private string BuildRequestBody(Dictionary<string, string> parameters)
        {
            JObject requestBodyJson = new JObject();

            foreach (var param in parameters)
            {
                if (param.Key == "TestStatus" || param.Key == "TestStatusDescription")
                {
                    Logger.Log($"Skipped '{param.Key}' column");
                    continue;
                }

                if (param.Value != null && param.Value != "")
                {
                    string paramValue = param.Value.ToString();

                    if (Regex.IsMatch(paramValue, @"^0[0-9]+$"))
                    {
                        requestBodyJson[param.Key] = paramValue;
                        //Logger.Log($"Preserved leading zero value for '{param.Key}' as string: {paramValue}");
                    }
                    else if (double.TryParse(paramValue, out double doubleValue))
                    {
                        if (paramValue.Contains("."))
                        {
                            requestBodyJson[param.Key] = doubleValue;
                            //Logger.Log($"Formatted double value for '{param.Key}': {doubleValue}");
                        }
                        else
                        {
                            requestBodyJson[param.Key] = Convert.ToInt32(doubleValue);
                            //Logger.Log($"Formatted integer value for '{param.Key}': {Convert.ToInt32(doubleValue)}");
                        }
                    }
                    else if (DateTime.TryParse(paramValue, out DateTime dateValue))
                    {
                        requestBodyJson[param.Key] = dateValue.ToString("yyyy-MM-ddTHH:mm:ssZ");
                        //Logger.Log($"Formatted date value for '{param.Key}': {dateValue.ToString("yyyy-MM-ddTHH:mm:ssZ")}");
                    }
                    else if (bool.TryParse(paramValue, out bool boolValue))
                    {
                        requestBodyJson[param.Key] = boolValue;
                        //Logger.Log($"Formatted boolean value for '{param.Key}': {boolValue}");
                    }
                    else
                    {
                        requestBodyJson[param.Key] = paramValue;
                        //Logger.Log($"Added string value for '{param.Key}': {paramValue}");
                    }
                }
                else
                {
                    Logger.Log($"Skipped null or empty value for '{param.Key}'");
                }
            }

            string requestBody = requestBodyJson.ToString();
            Logger.Log($"Built request body: {requestBody}");
            return requestBody;
        }

        private void UpdateIterationStatus(int iterationNumber, string currentApiLoop, string statusCode, string description, string result)
        {
            if (iterationNumber < 1 || iterationNumber > GridItems.Count) return;

            var item = GridItems[iterationNumber - 1];

            item.TreeNodesItemName[0].Name = $"Item {iterationNumber}";
            item.TreeNodesApi[0].Name = currentApiLoop;
            item.TreeNodesStatC[0].Name = statusCode;
            item.TreeNodesDesc[0].Name = description;
            item.TreeNodesRes[0].Name = result;

            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = GridItems;
        }

        private void WriteDataTableToCsv(DataTable dataTable, string filePath)
        {
            Logger.Log("Starting to write data to CSV...");

            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                try
                {
                    Logger.Log("Writing column headers...");
                    // Write the column headers
                    string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                            .Select(column => column.ColumnName)
                                            .ToArray();
                    writer.WriteLine(string.Join(",", columnNames));
                    Logger.Log($"Column headers written: {string.Join(", ", columnNames)}");

                    // Write the data rows
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string[] fields = row.ItemArray.Select(field =>
                        {
                            if (field is decimal decimalValue)
                            {
                                //Logger.Log($"Formatting decimal value: {decimalValue}");
                                return decimalValue.ToString("F2");
                            }
                            else if (field is double doubleValue)
                            {
                                //Logger.Log($"Formatting double value: {doubleValue}");
                                return doubleValue.ToString("F2");
                            }
                            else if (field is int || field is long)
                            {
                                //Logger.Log($"Preserving integer value: {field}");
                                return field.ToString();
                            }
                            else
                            {
                                //Logger.Log($"Handling non-numeric value: {field}");
                                return field.ToString();
                            }
                        }).ToArray();

                        writer.WriteLine(string.Join(",", fields));
                        Logger.Log($"Written row to CSV: {string.Join(", ", fields)}");
                    }

                    Logger.Log("CSV writing completed successfully.");
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error writing to CSV: {ex.Message}");
                    throw;
                }
            }
        }

        private void btnExportResults_Click(object sender, RoutedEventArgs e)
        {
            ConvertCsvToExcelAndPromptSave(csvFilePath);
        }

        public async Task ConvertCsvToExcelAndPromptSave(string csvFilePath)
        {
            if (!File.Exists(csvFilePath))
            {
                var dialog = new ContentDialog
                {
                    Title = "File Not Found",
                    Content = "CSV file not found.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
                return;
            }

            // Open a save file picker
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                FileTypeChoices = { { "Excel Files", new List<string> { ".xlsx" } } },
                SuggestedFileName = "Platned-TestMatic_Report"
            };

            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);

            var file = await savePicker.PickSaveFileAsync();
            if (file == null) return;

            using (var stream = await file.OpenStreamForWriteAsync())
            {
                using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
                {
                    // Add a WorkbookPart to the document.
                    var workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    // Add a WorksheetPart to the WorkbookPart.
                    var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    worksheetPart.Worksheet = new Worksheet(new SheetData());

                    // Append the SheetData to the Worksheet.
                    var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    // Read CSV and add to sheet
                    using (var reader = new StreamReader(csvFilePath))
                    {
                        string headerLine = await reader.ReadLineAsync();
                        if (headerLine != null)
                        {
                            var headerValues = headerLine.Split(',');
                            var headerRow = new Row();
                            foreach (var header in headerValues)
                            {
                                headerRow.Append(new Cell { CellValue = new CellValue(header), DataType = CellValues.String });
                            }
                            sheetData.AppendChild(headerRow); // Add header row
                        }

                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            if (line != null)
                            {
                                var values = line.Split(',');
                                var newRow = new Row();
                                foreach (var value in values)
                                {
                                    newRow.Append(new Cell { CellValue = new CellValue(value), DataType = CellValues.String });
                                }
                                sheetData.AppendChild(newRow); // Add data row
                            }
                        }
                    }

                    // Create Sheets and append to Workbook
                    var sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
                    var sheet = new Sheet
                    {
                        Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = "Sheet1"
                    };
                    sheets.Append(sheet);

                    // Save the workbook
                    workbookPart.Workbook.Save();
                }

                var successDialog = new ContentDialog
                {
                    Title = "File Saved",
                    Content = $"Excel file saved successfully!",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await successDialog.ShowAsync();
            }
        }

        private void btnStopExecution_Click(object sender, RoutedEventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                Logger.Log("Cancellation requested.");
            }
            btnRerun.IsEnabled = true;
            btnStop.IsEnabled = false;
            btnStart.IsEnabled = false;
        }

        private async void btnRunAgain_Click(object sender, RoutedEventArgs e)
        {
            if (LoadConfigData())
            {
                var csvFile = csvFilePath;
                var jsonFile = jsonFilePath;

                btnRerun.IsEnabled = false;
                btnStop.IsEnabled = true;
                btnStart.IsEnabled = false;
                progExec.ShowPaused = false;
                progExec.IsIndeterminate = true;

                await RunTestIterationsAsync(jsonFile, csvFile);
            }
            else
            {
                if (App.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ShowInfoBar("Error!", "License Key required to proceed. Please register with Platned Pass!", InfoBarSeverity.Error);
                }
            }

        }

        private void ParseJson(string jsonContent)
        {
            try
            {
                // Parse the JSON content
                JObject jsonObject = JObject.Parse(jsonContent);

                // Check if 'item' array exists
                JArray items = (JArray)jsonObject["item"];
                if (items == null || items.Count == 0)
                {
                    Logger.Log("Error: 'item' array not found or is empty.");
                    return;
                }

                // Get the first item in the array (you can loop if necessary for multiple items)
                JObject firstItem = (JObject)items[0];

                // Check if 'request' exists in the first item
                JObject requestObject = firstItem["request"] as JObject;
                if (requestObject == null)
                {
                    Logger.Log("Error: 'request' object not found in the first item.");
                    return;
                }

                // Check if 'body' exists in the 'request' object
                JObject bodyObject = requestObject["body"] as JObject;
                if (bodyObject == null)
                {
                    Logger.Log("Error: 'body' object not found in the 'request'. Exiting.");
                    return; // Exit if body is null
                }

                // Check if 'raw' exists in the 'body' object
                string rawContent = bodyObject["raw"]?.ToString();
                if (string.IsNullOrEmpty(rawContent))
                {
                    Logger.Log("Error: 'raw' content is null or empty.");
                    return;
                }

                // Output the extracted raw content
                Logger.Log($"Extracted Raw Content: {rawContent}");

                // You can call your method to parse the raw content here
                ParseRawContent(rawContent);
            }
            catch (Exception ex)
            {
                Logger.Log($"An error occurred while parsing JSON: {ex.Message}");
            }
        }

        private void ParseRawContent(string rawContent)
        {
            // 1. Extract EntitySet (the word ending with 'Set')
            string entitySetPattern = @"POST\s(\w+Set)\(";
            Match entitySetMatch = Regex.Match(rawContent, entitySetPattern);
            if (entitySetMatch.Success)
            {
                entitySet = entitySetMatch.Groups[1].Value;
                Logger.Log($"EntitySet: {entitySet}");
            }

            // 2. Extract the parameter name inside CustomerOrderSet()
            string entitySetParamsPattern = @"\(([^)]+)\)";  // Matches the content inside parentheses
            Match entitySetParamsMatch = Regex.Match(rawContent, entitySetParamsPattern);

            if (entitySetParamsMatch.Success)
            {
                entitySetParam = entitySetParamsMatch.Groups[1].Value;

                // Extract the specific parameter name (e.g., OrderNo) inside the parentheses
                string paramNamePattern = @"(\w+)=['""]([^'""]+)['""]";  // Pattern to match the parameter name and value
                Match paramNameMatch = Regex.Match(entitySetParam, paramNamePattern);

                if (paramNameMatch.Success)
                {
                    entitySetParam = paramNameMatch.Groups[1].Value;  // The parameter name, e.g., "OrderNo"
                    Logger.Log($"Parameter Name: {entitySetParam}");
                }
                else
                {
                    Logger.Log("Parameter name not found.");
                }
            }
            else
            {
                Logger.Log("No parameters found inside parentheses.");
            }

            // 3. Extract the word after ')' and before '?' (e.g., OrderLinesArray)
            string afterSetPattern = @"\)\s*/\s*(\w+)\?";
            Match afterSetMatch = Regex.Match(rawContent, afterSetPattern);
            if (afterSetMatch.Success)
            {
                entitySetArray = afterSetMatch.Groups[1].Value;
                Logger.Log($"EntitySet Array: {entitySetArray}");
            }
        }

        public string FilterRequestBody(string bodyRawJson, string requestBodyIn)
        {
            try
            {
                // Parse bodyRawJson to extract all property names
                JObject bodyRawJObject = JObject.Parse(bodyRawJson);
                var propertiesToRetain = bodyRawJObject.Properties().Select(p => p.Name).ToList();

                // Parse the request body raw JSON into a JObject for manipulation
                JObject requestBody = JObject.Parse(requestBodyIn);

                // Iterate through all properties in the request body and remove the unwanted ones
                foreach (var property in requestBody.Properties().ToList())
                {
                    if (!propertiesToRetain.Contains(property.Name))
                    {
                        property.Remove(); // Remove the property if it's not in the list of properties to retain
                    }
                }

                // Serialize the filtered requestBody back to JSON
                string filteredJson = JsonConvert.SerializeObject(requestBody, Formatting.Indented);

                return filteredJson;
            }catch (Exception e)
            {
                return "IGNORE";
            }
            
        }


        private void WriteDataTableToCsvWithStatus(DataTable dataTable, string filePath, int iterationNumber, bool errorFound, string apiResponseBody)
        {
            Logger.Log("Starting to write data to CSV with status columns...");

            // Add the "Status" and "Status Description" columns if they don't exist
            if (!dataTable.Columns.Contains("TestStatus"))
            {
                dataTable.Columns.Add("TestStatus", typeof(string));
            }

            if (!dataTable.Columns.Contains("TestStatusDescription"))
            {
                dataTable.Columns.Add("TestStatusDescription", typeof(string));
            }

            // Get the specific row based on the iteration number
            if (iterationNumber > 0 && iterationNumber <= dataTable.Rows.Count)
            {
                DataRow row = dataTable.Rows[iterationNumber - 1];

                // Update the "Status" and "Status Description" columns based on the error condition
                if (errorFound)
                {
                    row["TestStatus"] = "Error";
                    // Replace commas with underscores and escape quotes
                    string sanitizedMessage = apiResponseBody.Replace(",", "_").Replace("\"", "\"\"");
                    // Enclose the error message in double quotes to escape any special characters
                    row["TestStatusDescription"] = $"\"{sanitizedMessage}\"";
                    Logger.Log($"Error message added to 'TestStatusDescription': {row["TestStatusDescription"]}");
                }
                else
                {
                    row["TestStatus"] = "OK";
                    row["TestStatusDescription"] = "Successfully Completed";
                }
            }
            else
            {
                Logger.Log($"Invalid iteration number: {iterationNumber}");
                return; // Exit if the iteration number is invalid
            }

            // Write the DataTable to the CSV file
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                try
                {
                    Logger.Log("Writing column headers...");
                    // Write the column headers
                    string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                            .Select(column => column.ColumnName)
                                            .ToArray();
                    writer.WriteLine(string.Join(",", columnNames));
                    Logger.Log($"Column headers written: {string.Join(", ", columnNames)}");

                    // Write the data rows
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string[] fields = row.ItemArray.Select(field =>
                        {
                            if (field is decimal decimalValue)
                            {
                                return decimalValue.ToString("F2");
                            }
                            else if (field is double doubleValue)
                            {
                                return doubleValue.ToString("F2");
                            }
                            else if (field is int || field is long)
                            {
                                return field.ToString();
                            }
                            else
                            {
                                return field.ToString();
                            }
                        }).ToArray();

                        writer.WriteLine(string.Join(",", fields));
                        Logger.Log($"Written row to CSV: {string.Join(", ", fields)}");
                    }

                    Logger.Log("CSV writing with status completed successfully.");
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error writing to CSV: {ex.Message}");
                    throw;
                }
            }
        }

    }


    /// <summary>
    /// Other Classes
    /// </summary>

    public class TreeNode
    {
        public string Name { get; set; }
        public ObservableCollection<TreeNode> Children { get; set; }

        public TreeNode(string name)
        {
            Name = name;
            Children = new ObservableCollection<TreeNode>();
        }
    }

    public class GridItem
    {
        public ObservableCollection<TreeNode> TreeNodesItemName { get; set; }
        public ObservableCollection<TreeNode> TreeNodesApi { get; set; }
        public ObservableCollection<TreeNode> TreeNodesDesc { get; set; }
        public ObservableCollection<TreeNode> TreeNodesStatC { get; set; }
        public ObservableCollection<TreeNode> TreeNodesRes { get; set; }

        public GridItem(string itemName)
        {
            TreeNodesItemName = new ObservableCollection<TreeNode>();
            TreeNodesApi = new ObservableCollection<TreeNode>();
            TreeNodesDesc = new ObservableCollection<TreeNode>();
            TreeNodesStatC = new ObservableCollection<TreeNode>();
            TreeNodesRes = new ObservableCollection<TreeNode>();

            TreeNodesItemName.Add(new TreeNode(itemName));
        }
    }
    // DataGrid Code - End


}
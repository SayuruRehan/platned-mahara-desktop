using Newtonsoft.Json.Linq;
using PlatnedTestMatic.CustomClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PlatnedTestMatic.CustomClasses.ApiExecution;
using ClosedXML.Excel;
using System.Threading;

namespace PlatnedTestMatic
{
    public partial class frmTestRun : frmBaseForm
    {
        private DataGridView dgvTestResults;
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


        public frmTestRun()
        {
            InitializeComponent();
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
            lblTestStatus.Text = "In Progress";
            btnRunAgain.Visible = false;
            btnStopExecution.Visible = true;
            lblExecStarted.Text = DateTime.Now.ToString();

            jsonFilePath = uploadedJSONFilePath;
            csvFilePath = uploadedCSVFilePath;
            Logger.Log("jsonFilePath, csvFilePath received for execution!");
            InitializeAsync();
            InitializeTestResultsGrid();

            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            if (string.IsNullOrEmpty(token))
            {
                Logger.Log("No saved token found. Refreshing the token...");
                frmBasicData basicDataForm = new frmBasicData();
                token = await basicDataForm.RefreshToken();
                Logger.Log("Token refreshed!");
            }

            for (int iteration = 1; iteration <= totalIterations; iteration++)
            {
                if (cancellationToken.IsCancellationRequested) 
                {
                    Logger.Log("Test execution cancelled.");
                    lblTestStatus.Text = "Cancelled";
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
                        lblTestStatus.Text = "Cancelled";
                        return;
                    }

                    apiLoop += 1;
                    if (!errorFound)
                    {
                        UpdateIterationStatus(iteration, $"{apiLoop}/{apiCalls.Count}", "", "", "In Progress...");
                        Logger.Log("API Call: " + apiCall);
                        await RunTestIteration(iteration, apiLoop, apiCalls.Count, apiCall);
                    }
                    else {
                        Logger.Log($"Skipped the remaining API calls due to previous error in iteration {iteration}.");
                        break;
                    }
                }
            }

            if (!testingStausFailed)
            {
                lblTestStatus.Text = "Completed";
                lblExecFinished.Text = DateTime.Now.ToString();
            }
            else
            {
                lblTestStatus.Text = "Failed";
                lblTestStatus.BackColor = Color.Red;
                lblTestStatus.ForeColor = Color.White;
                lblExecFinished.Text = DateTime.Now.ToString();
            }
        }

        private async Task RunTestIteration(int iterationNumber, int apiLoop, int apiCount, JObject apiCall)
        {
            string method = apiCall["request"]["method"].ToString();
            string url = apiCall["request"]["url"]["raw"].ToString();
            string headers = "";
            string requestBody = "";

            ApiExecution api = new ApiExecution();
            ApiExecution.ApiResponse apiResponse = null;

            Logger.Log("Extracting data from CSV for API request");
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
                    apiResponse = await api.Get(url, headers, requestBody, token);
                    break;

                case "POST":
                    requestBody = BuildRequestBody(csvParameters);
                    Logger.Log("POST - Request body: " + requestBody);
                    apiResponse = await api.Post(url, headers, requestBody, token);
                    break;

                case "PATCH":
                    requestBody = BuildRequestBody(csvParameters); 
                    Logger.Log("PATCH - Request body: " + requestBody);
                    apiResponse = await api.Patch(url, headers, requestBody, token);
                    break;

                case "DELETE":
                    Logger.Log("DELETE - Request body: " + requestBody);
                    apiResponse = await api.Delete(url, headers, token); 
                    break;

                default:
                    Logger.Log($"Unsupported HTTP method: {method}", "Error");
                    return;
            }


            Logger.Log($"Iteration {iterationNumber}: Response StatusCode={apiResponse.StatusCode}, ResponseBody={apiResponse.ResponseBody}");
            
            if (apiResponse != null && (apiResponse.StatusCode == 200 || apiResponse.StatusCode == 201) && errorFound != true)
            {
                UpdateIterationStatus(iterationNumber, $"{apiLoop}/{apiCount}", apiResponse.StatusCode.ToString(), "Successful", "OK");
            }
            else
            {
                errorFound = true;
                testingStausFailed = true;
                UpdateIterationStatus(iterationNumber, $"{apiLoop}/{apiCount}", apiResponse?.StatusCode.ToString() ?? "N/A", apiResponse?.ResponseBody, "Error");
            }

            if (!errorFound)
            {
                Logger.Log("Processing CSV for new data...");
                JObject responseJson = JObject.Parse(apiResponse.ResponseBody);
                DataRow row = csvData.Rows[iterationNumber - 1];

                foreach (var property in responseJson.Properties())
                {
                    try
                    {
                        Logger.Log($"Processing property: '{property.Name}' with value: '{property.Value}'");

                        if (!csvData.Columns.Contains(property.Name) && !new[] { "error", "@odata.context", "value" }.Contains(property.Name) && !property.Name.Contains("Ref"))
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
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Error processing property '{property.Name}': {ex.Message}");
                    }
                }

                Logger.Log("Starting to write data to CSV...");

                WriteDataTableToCsv(csvData, csvFilePath);

                Logger.Log($"Iteration {iterationNumber}: CSV file updated.");
            }
            
        }

        private string BuildRequestBody(Dictionary<string, string> parameters)
        {
            JObject requestBodyJson = new JObject();

            foreach (var param in parameters)
            {
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
            var row = dgvTestResults.Rows[iterationNumber - 1];
            row.Cells["API Calls"].Value = currentApiLoop;
            row.Cells["Description"].Value = description;
            row.Cells["Status Code"].Value = statusCode; 
            row.Cells["Result"].Value = result; 
            dgvTestResults.Refresh();
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

        private void InitializeTestResultsGrid()
        {
            dgvTestResults = new DataGridView();
            dgvTestResults.Location = new Point(20, 160);
            dgvTestResults.Size = new Size(600, 300);

            dgvTestResults.ColumnCount = 5;
            dgvTestResults.Columns[0].Name = "Iteration";
            dgvTestResults.Columns[1].Name = "API Calls";
            dgvTestResults.Columns[2].Name = "Description";
            dgvTestResults.Columns[3].Name = "Status Code";
            dgvTestResults.Columns[4].Name = "Result";

            for (int i = 1; i <= totalIterations; i++)
            {
                dgvTestResults.Rows.Add($"Iteration {i}", "", "Pending...", "", "Queued...");
            }

            dgvTestResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.Controls.Add(dgvTestResults);
        }

        private void btnExportResults_Click(object sender, EventArgs e)
        {
            ConvertCsvToExcelAndPromptSave(csvFilePath);
        }

        public void ConvertCsvToExcelAndPromptSave(string csvFilePath)
        {
            if (!File.Exists(csvFilePath))
            {
                MessageBox.Show("CSV file not found.");
                Logger.Log("CSV file not found.");
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.DefaultExt = "xlsx";
                saveFileDialog.FileName = "Platned-TestMatic_Report.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedExcelFilePath = saveFileDialog.FileName;

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Sheet1");

                        using (var reader = new StreamReader(csvFilePath))
                        {
                            int currentRow = 1;

                            while (!reader.EndOfStream)
                            {
                                var line = reader.ReadLine();
                                var values = line.Split(',');

                                for (int i = 0; i < values.Length; i++)
                                {
                                    worksheet.Cell(currentRow, i + 1).Value = values[i];
                                }

                                currentRow++;
                            }
                        }

                        workbook.SaveAs(selectedExcelFilePath);
                    }

                    MessageBox.Show($"Excel file saved at: {selectedExcelFilePath}");
                    Logger.Log($"Excel file saved at: {selectedExcelFilePath}");
                }
            }
        }

        private void btnStopExecution_Click(object sender, EventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                Logger.Log("Cancellation requested.");
            }
            btnRunAgain.Visible = true;
            btnStopExecution.Visible = false;
        }

        private async void btnRunAgain_Click(object sender, EventArgs e)
        {
            var csvFile = csvFilePath;
            var jsonFile = jsonFilePath;

            btnRunAgain.Visible = false;
            btnStopExecution.Visible = true;

            frmTestRun frmTestRunNew = new frmTestRun();
            this.Close();
            frmTestRunNew.Show(); 

            await frmTestRunNew.RunTestIterationsAsync(jsonFile, csvFile); 
        }

    }
}

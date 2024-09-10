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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatnedTestMatic
{
    public partial class frmTestRun : frmBaseForm
    {
        private DataGridView dgvTestResults;
        private int totalIterations = 2;
        private string token = "";
        private List<JObject> apiCalls;
        private DataTable csvData;
        private string tempFolderPath;
        private bool testingStausFailed = false;

        public frmTestRun()
        {
            InitializeComponent();
            InitializeAsync();
            InitializeTestResultsGrid();
        }

        private async Task InitializeAsync()
        {
            tempFolderPath = Path.Combine(Path.GetTempPath(), "PL-TestMatic");

            string jsonFilePath = Path.Combine(tempFolderPath, "postman_collection_create_usergroups.json");
            string csvFilePath = Path.Combine(tempFolderPath, "data.csv");

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
                Logger.Log("CSV data loaded.");
            }
        }

        public async Task RunTestIterationsAsync()
        {
            if (string.IsNullOrEmpty(token))
            {
                Logger.Log("No saved token found. Refreshing the token...");
                frmBasicData basicDataForm = new frmBasicData();
                token = await basicDataForm.RefreshToken();
                Logger.Log("Token refreshed!");
            }

            for (int iteration = 1; iteration <= totalIterations; iteration++)
            {
                Logger.Log($"Starting iteration {iteration} =============================================> ");

                foreach (var apiCall in apiCalls)
                {
                    Logger.Log("API Call: " + apiCall);
                    await RunTestIteration(iteration, apiCall);
                }
            }

            if (!testingStausFailed)
            {
                lblTestStatus.Text = "Completed";
            }
            else
            {
                lblTestStatus.Text = "Failed";
                lblTestStatus.BackColor = Color.Red;
                lblTestStatus.ForeColor = Color.White;
            }
        }

        private async Task RunTestIteration(int iterationNumber, JObject apiCall)
        {
            InitializeAsync();

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

            if (method == "GET")
            {
                apiResponse = await api.Get(url, headers, requestBody, token);
            }
            else if (method == "POST")
            {
                requestBody = BuildRequestBody(csvParameters);
                Logger.Log("Request body: " + requestBody);
                apiResponse = await api.Post(url, headers, requestBody, token);
            }
            // PATCH, DELETE to be added


            Logger.Log($"Iteration {iterationNumber}: Response StatusCode={apiResponse.StatusCode}, ResponseBody={apiResponse.ResponseBody}");
            bool errorFound = false;
            if (apiResponse != null && (apiResponse.StatusCode == 200 || apiResponse.StatusCode == 201) && errorFound != true)
            {
                UpdateIterationStatus(iterationNumber, apiResponse.StatusCode.ToString(), "Successful", "OK");
            }
            else
            {
                errorFound = true;
                testingStausFailed = true;
                UpdateIterationStatus(iterationNumber, apiResponse?.StatusCode.ToString() ?? "N/A", apiResponse?.ResponseBody, "Error");
            }

            if (!errorFound)
            {
                Logger.Log("Processing CSV for new data...");
                JObject responseJson = JObject.Parse(apiResponse.ResponseBody);
                DataRow row = csvData.Rows[iterationNumber - 1];

                foreach (var property in responseJson.Properties())
                {
                    if (!csvData.Columns.Contains(property.Name) && property.Name != "error")
                    {
                        csvData.Columns.Add(property.Name);
                        Logger.Log($"Iteration {iterationNumber}: Added new column '{property.Name}' to CSV.");
                    }
                    if (row[property.Name] == DBNull.Value || string.IsNullOrEmpty(row[property.Name]?.ToString()))
                    {
                        if (!string.IsNullOrEmpty(property.Value.ToString()))
                        {
                            row[property.Name] = property.Value.ToString();
                        }
                    }
                }
                string csvFilePath = Path.Combine(tempFolderPath, "data.csv");
                WriteDataTableToCsv(csvData, csvFilePath);

                Logger.Log($"Iteration {iterationNumber}: CSV file updated.");
            }
            
        }

        private string BuildRequestBody(Dictionary<string, string> parameters)
        {
            JObject requestBodyJson = new JObject();

            foreach (var param in parameters)
            {
                if (!string.IsNullOrEmpty(param.Value))
                {
                    requestBodyJson[param.Key] = param.Value;
                }
            }

            return requestBodyJson.ToString();
        }

        private void UpdateIterationStatus(int iterationNumber, string statusCode, string description, string result)
        {
            var row = dgvTestResults.Rows[iterationNumber - 1];
            row.Cells["Description"].Value = description;
            row.Cells["Status Code"].Value = statusCode; 
            row.Cells["Result"].Value = result; 
            dgvTestResults.Refresh();
        }

        private void WriteDataTableToCsv(DataTable dataTable, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                string[] columnNames = dataTable.Columns.Cast<DataColumn>()
                                        .Select(column => column.ColumnName)
                                        .ToArray();
                writer.WriteLine(string.Join(",", columnNames));

                foreach (DataRow row in dataTable.Rows)
                {
                    string[] fields = row.ItemArray.Select(field => field.ToString()).ToArray();
                    writer.WriteLine(string.Join(",", fields));
                }
            }
        }

        private void InitializeTestResultsGrid()
        {
            dgvTestResults = new DataGridView();
            dgvTestResults.Location = new Point(20, 160);
            dgvTestResults.Size = new Size(600, 300);

            dgvTestResults.ColumnCount = 4;
            dgvTestResults.Columns[0].Name = "Iteration";
            dgvTestResults.Columns[1].Name = "Description";
            dgvTestResults.Columns[2].Name = "Status Code";
            dgvTestResults.Columns[3].Name = "Result";

            for (int i = 1; i <= totalIterations; i++)
            {
                dgvTestResults.Rows.Add($"Iteration {i}", "Pending...", "", "");
            }

            dgvTestResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.Controls.Add(dgvTestResults);
        }

    }
}

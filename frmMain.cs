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
    public partial class frmMain : frmBaseForm
    {
        private string uploadedJSONFilePath;
        private string uploadedCSVFilePath;
        public frmMain()
        {
            InitializeComponent();
            Logger.Log("Application started.");
        }

        private void btnRunTests_MouseEnter(object sender, EventArgs e)
        {
            btnRunTests.Image = Properties.Resources.Start;
            btnRunTests.Text = "";
        }

        private void btnRunTests_MouseLeave(object sender, EventArgs e)
        {
            btnRunTests.Image = null;
            btnRunTests.Text = "Run Tests";
        }

        private void btnUploadJson_MouseEnter(object sender, EventArgs e)
        {
            btnUploadJson.Image = Properties.Resources.Upload;
            btnUploadJson.Text = "";
        }

        private void btnUploadJson_MouseLeave(object sender, EventArgs e)
        {
            btnUploadJson.Image = null;
            btnUploadJson.Text = "Upload Json";
        }

        private void btnUploadDataSource_MouseEnter(object sender, EventArgs e)
        {
            btnUploadDataSource.Image = Properties.Resources.Upload;
            btnUploadDataSource.Text = "";
        }

        private void btnUploadDataSource_MouseLeave(object sender, EventArgs e)
        {
            btnUploadDataSource.Image = null;
            btnUploadDataSource.Text = "Upload Data Source";
        }

        private void btnUploadJson_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.Title = "Select a JSON file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string sourceFilePath = openFileDialog.FileName;

                        string tempFolderPath = Path.Combine(Path.GetTempPath(), "PL-TestMatic"); 

                        if (!Directory.Exists(tempFolderPath))
                        {
                            Directory.CreateDirectory(tempFolderPath);
                        }

                        string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(sourceFilePath));

                        File.Copy(sourceFilePath, tempFilePath, true);

                        uploadedJSONFilePath = tempFilePath;

                        lblUploadJsonStatus.Text = $"Upload Completed!";
                        lblUploadJsonStatus.Visible = true ;
                        Logger.Log($"JSON file uploaded to location: {tempFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"An error occurred: {ex.Message}", "Error");
                        MessageBox.Show($"An error occurred. Refer to application logs for more info.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnProcessJson_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(uploadedJSONFilePath) && File.Exists(uploadedJSONFilePath))
            {
                MessageBox.Show($"Processing file: {uploadedJSONFilePath}");

                string jsonContent = File.ReadAllText(uploadedJSONFilePath);
                
                // Process the JSON content below
            }
            else
            {
                MessageBox.Show("No file uploaded or file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnUploadDataSource_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.Title = "Select a CSV file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string sourceFilePath = openFileDialog.FileName;

                        string tempFolderPath = Path.Combine(Path.GetTempPath(), "PL-TestMatic");

                        if (!Directory.Exists(tempFolderPath))
                        {
                            Directory.CreateDirectory(tempFolderPath);
                        }

                        string tempFilePath = Path.Combine(tempFolderPath, Path.GetFileName(sourceFilePath));

                        File.Copy(sourceFilePath, tempFilePath, true);

                        uploadedCSVFilePath = tempFilePath;

                        lblUploadDataSourceStatus.Text = $"Upload Completed!";
                        lblUploadDataSourceStatus.Visible = true;
                        Logger.Log($"CSV file uploaded to location: {tempFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"An error occurred: {ex.Message}", "Error");
                        MessageBox.Show($"An error occurred. Refer to application logs for more info.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            frmBasicData basicData = new frmBasicData();
            basicData.Show();
        }

        private void btnRunTests_Click(object sender, EventArgs e)
        {
            frmTestRun frmTestRun = new frmTestRun();
            frmTestRun.Show();
            frmTestRun.RunTestIterationsAsync(uploadedJSONFilePath, uploadedCSVFilePath);
        }
    }
}

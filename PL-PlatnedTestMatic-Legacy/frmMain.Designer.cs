namespace PlatnedTestMatic
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnUploadJson = new System.Windows.Forms.Button();
            this.btnUploadDataSource = new System.Windows.Forms.Button();
            this.btnRunTests = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnSetting = new System.Windows.Forms.Button();
            this.lblUploadJsonStatus = new System.Windows.Forms.Label();
            this.lblUploadDataSourceStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PlatnedTestMatic.Properties.Resources.Platned;
            this.pictureBox1.Location = new System.Drawing.Point(237, 34);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(188, 158);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // btnUploadJson
            // 
            this.btnUploadJson.BackColor = System.Drawing.Color.Lavender;
            this.btnUploadJson.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadJson.Location = new System.Drawing.Point(69, 288);
            this.btnUploadJson.Margin = new System.Windows.Forms.Padding(2);
            this.btnUploadJson.Name = "btnUploadJson";
            this.btnUploadJson.Size = new System.Drawing.Size(154, 58);
            this.btnUploadJson.TabIndex = 22;
            this.btnUploadJson.Text = "Upload JSON";
            this.btnUploadJson.UseVisualStyleBackColor = false;
            this.btnUploadJson.Click += new System.EventHandler(this.btnUploadJson_Click);
            this.btnUploadJson.MouseEnter += new System.EventHandler(this.btnUploadJson_MouseEnter);
            this.btnUploadJson.MouseLeave += new System.EventHandler(this.btnUploadJson_MouseLeave);
            // 
            // btnUploadDataSource
            // 
            this.btnUploadDataSource.BackColor = System.Drawing.Color.Lavender;
            this.btnUploadDataSource.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadDataSource.Location = new System.Drawing.Point(259, 288);
            this.btnUploadDataSource.Margin = new System.Windows.Forms.Padding(2);
            this.btnUploadDataSource.Name = "btnUploadDataSource";
            this.btnUploadDataSource.Size = new System.Drawing.Size(154, 58);
            this.btnUploadDataSource.TabIndex = 23;
            this.btnUploadDataSource.Text = "Upload Data Source";
            this.btnUploadDataSource.UseVisualStyleBackColor = false;
            this.btnUploadDataSource.Click += new System.EventHandler(this.btnUploadDataSource_Click);
            this.btnUploadDataSource.MouseEnter += new System.EventHandler(this.btnUploadDataSource_MouseEnter);
            this.btnUploadDataSource.MouseLeave += new System.EventHandler(this.btnUploadDataSource_MouseLeave);
            // 
            // btnRunTests
            // 
            this.btnRunTests.BackColor = System.Drawing.Color.ForestGreen;
            this.btnRunTests.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunTests.ForeColor = System.Drawing.Color.White;
            this.btnRunTests.Location = new System.Drawing.Point(449, 288);
            this.btnRunTests.Margin = new System.Windows.Forms.Padding(2);
            this.btnRunTests.Name = "btnRunTests";
            this.btnRunTests.Size = new System.Drawing.Size(154, 58);
            this.btnRunTests.TabIndex = 24;
            this.btnRunTests.Text = "Run Tests";
            this.btnRunTests.UseVisualStyleBackColor = false;
            this.btnRunTests.Click += new System.EventHandler(this.btnRunTests_Click);
            this.btnRunTests.MouseEnter += new System.EventHandler(this.btnRunTests_MouseEnter);
            this.btnRunTests.MouseLeave += new System.EventHandler(this.btnRunTests_MouseLeave);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(164, 197);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(342, 57);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 25;
            this.pictureBox2.TabStop = false;
            // 
            // btnSetting
            // 
            this.btnSetting.Image = global::PlatnedTestMatic.Properties.Resources.Setting;
            this.btnSetting.Location = new System.Drawing.Point(620, 379);
            this.btnSetting.Margin = new System.Windows.Forms.Padding(2);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(51, 51);
            this.btnSetting.TabIndex = 26;
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // lblUploadJsonStatus
            // 
            this.lblUploadJsonStatus.AutoSize = true;
            this.lblUploadJsonStatus.Location = new System.Drawing.Point(69, 352);
            this.lblUploadJsonStatus.Name = "lblUploadJsonStatus";
            this.lblUploadJsonStatus.Size = new System.Drawing.Size(117, 13);
            this.lblUploadJsonStatus.TabIndex = 27;
            this.lblUploadJsonStatus.Text = "<JSON Upload Status>";
            this.lblUploadJsonStatus.Visible = false;
            // 
            // lblUploadDataSourceStatus
            // 
            this.lblUploadDataSourceStatus.AutoSize = true;
            this.lblUploadDataSourceStatus.Location = new System.Drawing.Point(256, 352);
            this.lblUploadDataSourceStatus.Name = "lblUploadDataSourceStatus";
            this.lblUploadDataSourceStatus.Size = new System.Drawing.Size(117, 13);
            this.lblUploadDataSourceStatus.TabIndex = 28;
            this.lblUploadDataSourceStatus.Text = "<JSON Upload Status>";
            this.lblUploadDataSourceStatus.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 453);
            this.Controls.Add(this.lblUploadDataSourceStatus);
            this.Controls.Add(this.lblUploadJsonStatus);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnRunTests);
            this.Controls.Add(this.btnUploadDataSource);
            this.Controls.Add(this.btnUploadJson);
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Home";
            this.Controls.SetChildIndex(this.pictureBox1, 0);
            this.Controls.SetChildIndex(this.btnUploadJson, 0);
            this.Controls.SetChildIndex(this.btnUploadDataSource, 0);
            this.Controls.SetChildIndex(this.btnRunTests, 0);
            this.Controls.SetChildIndex(this.pictureBox2, 0);
            this.Controls.SetChildIndex(this.btnSetting, 0);
            this.Controls.SetChildIndex(this.lblUploadJsonStatus, 0);
            this.Controls.SetChildIndex(this.lblUploadDataSourceStatus, 0);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnUploadJson;
        private System.Windows.Forms.Button btnUploadDataSource;
        private System.Windows.Forms.Button btnRunTests;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Label lblUploadJsonStatus;
        private System.Windows.Forms.Label lblUploadDataSourceStatus;
    }
}
namespace PlatnedTestMatic
{
    partial class frmTestRun
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTestStatus = new PlatnedTestMatic.CustomClasses.RoundedLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblExecStarted = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblExecFinished = new System.Windows.Forms.Label();
            this.btnRunAgain = new System.Windows.Forms.Button();
            this.btnExportResults = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnStopExecution = new System.Windows.Forms.Button();
            this.dgvTestResults = new System.Windows.Forms.DataGridView();
            this.clmIteration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAPICalls = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStatusCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestResults)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Test Runs";
            // 
            // lblTestStatus
            // 
            this.lblTestStatus.BackColor = System.Drawing.Color.White;
            this.lblTestStatus.CornerRadius = 1000;
            this.lblTestStatus.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestStatus.ForeColor = System.Drawing.Color.Black;
            this.lblTestStatus.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTestStatus.Location = new System.Drawing.Point(104, 37);
            this.lblTestStatus.Name = "lblTestStatus";
            this.lblTestStatus.Padding = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.lblTestStatus.Size = new System.Drawing.Size(88, 33);
            this.lblTestStatus.TabIndex = 23;
            this.lblTestStatus.Text = "Pending";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 19);
            this.label2.TabIndex = 24;
            this.label2.Text = "Execution started at ";
            // 
            // lblExecStarted
            // 
            this.lblExecStarted.AutoSize = true;
            this.lblExecStarted.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExecStarted.Location = new System.Drawing.Point(147, 80);
            this.lblExecStarted.Name = "lblExecStarted";
            this.lblExecStarted.Size = new System.Drawing.Size(89, 19);
            this.lblExecStarted.TabIndex = 25;
            this.lblExecStarted.Text = "<date_time>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 19);
            this.label3.TabIndex = 24;
            this.label3.Text = "Execution finished at ";
            // 
            // lblExecFinished
            // 
            this.lblExecFinished.AutoSize = true;
            this.lblExecFinished.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExecFinished.Location = new System.Drawing.Point(152, 106);
            this.lblExecFinished.Name = "lblExecFinished";
            this.lblExecFinished.Size = new System.Drawing.Size(89, 19);
            this.lblExecFinished.TabIndex = 25;
            this.lblExecFinished.Text = "<date_time>";
            // 
            // btnRunAgain
            // 
            this.btnRunAgain.BackColor = System.Drawing.Color.White;
            this.btnRunAgain.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunAgain.ForeColor = System.Drawing.Color.Black;
            this.btnRunAgain.Image = global::PlatnedTestMatic.Properties.Resources.restart;
            this.btnRunAgain.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRunAgain.Location = new System.Drawing.Point(121, 13);
            this.btnRunAgain.Name = "btnRunAgain";
            this.btnRunAgain.Size = new System.Drawing.Size(111, 48);
            this.btnRunAgain.TabIndex = 26;
            this.btnRunAgain.Text = "Run Again";
            this.btnRunAgain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRunAgain.UseVisualStyleBackColor = false;
            this.btnRunAgain.Click += new System.EventHandler(this.btnRunAgain_Click);
            // 
            // btnExportResults
            // 
            this.btnExportResults.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportResults.Image = global::PlatnedTestMatic.Properties.Resources.Export;
            this.btnExportResults.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportResults.Location = new System.Drawing.Point(238, 14);
            this.btnExportResults.Name = "btnExportResults";
            this.btnExportResults.Size = new System.Drawing.Size(133, 46);
            this.btnExportResults.TabIndex = 27;
            this.btnExportResults.Text = "Export Results";
            this.btnExportResults.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportResults.UseVisualStyleBackColor = true;
            this.btnExportResults.Click += new System.EventHandler(this.btnExportResults_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStopExecution);
            this.groupBox1.Controls.Add(this.btnRunAgain);
            this.groupBox1.Controls.Add(this.btnExportResults);
            this.groupBox1.Location = new System.Drawing.Point(296, 25);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(374, 67);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            // 
            // btnStopExecution
            // 
            this.btnStopExecution.BackColor = System.Drawing.Color.White;
            this.btnStopExecution.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopExecution.ForeColor = System.Drawing.Color.Black;
            this.btnStopExecution.Image = global::PlatnedTestMatic.Properties.Resources.restart;
            this.btnStopExecution.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStopExecution.Location = new System.Drawing.Point(6, 12);
            this.btnStopExecution.Name = "btnStopExecution";
            this.btnStopExecution.Size = new System.Drawing.Size(111, 48);
            this.btnStopExecution.TabIndex = 28;
            this.btnStopExecution.Text = "Stop";
            this.btnStopExecution.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStopExecution.UseVisualStyleBackColor = false;
            this.btnStopExecution.Click += new System.EventHandler(this.btnStopExecution_Click);
            // 
            // dgvTestResults
            // 
            this.dgvTestResults.AllowUserToAddRows = false;
            this.dgvTestResults.AllowUserToDeleteRows = false;
            this.dgvTestResults.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTestResults.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTestResults.BackgroundColor = System.Drawing.Color.White;
            this.dgvTestResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTestResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvTestResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(130)))), ((int)(((byte)(233)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTestResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvTestResults.ColumnHeadersHeight = 35;
            this.dgvTestResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTestResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmIteration,
            this.clmAPICalls,
            this.clmDescription,
            this.clmStatusCode,
            this.clmResult});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.LightGray;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTestResults.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvTestResults.EnableHeadersVisualStyles = false;
            this.dgvTestResults.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgvTestResults.Location = new System.Drawing.Point(9, 137);
            this.dgvTestResults.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvTestResults.Name = "dgvTestResults";
            this.dgvTestResults.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTestResults.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvTestResults.RowHeadersVisible = false;
            this.dgvTestResults.RowHeadersWidth = 15;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTestResults.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvTestResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTestResults.Size = new System.Drawing.Size(662, 300);
            this.dgvTestResults.TabIndex = 29;
            // 
            // clmIteration
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.clmIteration.DefaultCellStyle = dataGridViewCellStyle3;
            this.clmIteration.HeaderText = "Iteration";
            this.clmIteration.MinimumWidth = 6;
            this.clmIteration.Name = "clmIteration";
            this.clmIteration.ReadOnly = true;
            this.clmIteration.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.clmIteration.Width = 125;
            // 
            // clmAPICalls
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.clmAPICalls.DefaultCellStyle = dataGridViewCellStyle4;
            this.clmAPICalls.HeaderText = "API Calls";
            this.clmAPICalls.MinimumWidth = 6;
            this.clmAPICalls.Name = "clmAPICalls";
            this.clmAPICalls.ReadOnly = true;
            this.clmAPICalls.Width = 80;
            // 
            // clmDescription
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.clmDescription.DefaultCellStyle = dataGridViewCellStyle5;
            this.clmDescription.HeaderText = "Description";
            this.clmDescription.MinimumWidth = 6;
            this.clmDescription.Name = "clmDescription";
            this.clmDescription.ReadOnly = true;
            this.clmDescription.Width = 200;
            // 
            // clmStatusCode
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.clmStatusCode.DefaultCellStyle = dataGridViewCellStyle6;
            this.clmStatusCode.HeaderText = "Status Code";
            this.clmStatusCode.MinimumWidth = 6;
            this.clmStatusCode.Name = "clmStatusCode";
            this.clmStatusCode.ReadOnly = true;
            // 
            // clmResult
            // 
            this.clmResult.HeaderText = "Result";
            this.clmResult.MinimumWidth = 6;
            this.clmResult.Name = "clmResult";
            this.clmResult.ReadOnly = true;
            this.clmResult.Width = 200;
            // 
            // frmTestRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 453);
            this.Controls.Add(this.dgvTestResults);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblExecFinished);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblExecStarted);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTestStatus);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmTestRun";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTestRun";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lblTestStatus, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.lblExecStarted, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lblExecFinished, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.dgvTestResults, 0);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTestResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private CustomClasses.RoundedLabel lblTestStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblExecStarted;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblExecFinished;
        private System.Windows.Forms.Button btnRunAgain;
        private System.Windows.Forms.Button btnExportResults;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStopExecution;
        private System.Windows.Forms.DataGridView dgvTestResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmIteration;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAPICalls;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStatusCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmResult;
    }
}
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblTestStatus = new PlatnedTestMatic.CustomClasses.RoundedLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblExecStarted = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblExecFinished = new System.Windows.Forms.Label();
            this.btnRunAgain = new System.Windows.Forms.Button();
            this.btnExportResults = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 20);
            this.label1.TabIndex = 21;
            this.label1.Text = "Test Runs";
            // 
            // lblTestStatus
            // 
            this.lblTestStatus.AutoSize = true;
            this.lblTestStatus.BackColor = System.Drawing.Color.Blue;
            this.lblTestStatus.CornerRadius = 1000;
            this.lblTestStatus.ForeColor = System.Drawing.Color.White;
            this.lblTestStatus.Location = new System.Drawing.Point(132, 41);
            this.lblTestStatus.Name = "lblTestStatus";
            this.lblTestStatus.Padding = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.lblTestStatus.Size = new System.Drawing.Size(86, 19);
            this.lblTestStatus.TabIndex = 23;
            this.lblTestStatus.Text = "Pending";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Execution started at ";
            // 
            // lblExecStarted
            // 
            this.lblExecStarted.AutoSize = true;
            this.lblExecStarted.Location = new System.Drawing.Point(135, 76);
            this.lblExecStarted.Name = "lblExecStarted";
            this.lblExecStarted.Size = new System.Drawing.Size(65, 13);
            this.lblExecStarted.TabIndex = 25;
            this.lblExecStarted.Text = "<date_time>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(418, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(108, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Execution finished at ";
            // 
            // lblExecFinished
            // 
            this.lblExecFinished.AutoSize = true;
            this.lblExecFinished.Location = new System.Drawing.Point(527, 76);
            this.lblExecFinished.Name = "lblExecFinished";
            this.lblExecFinished.Size = new System.Drawing.Size(65, 13);
            this.lblExecFinished.TabIndex = 25;
            this.lblExecFinished.Text = "<date_time>";
            // 
            // btnRunAgain
            // 
            this.btnRunAgain.BackColor = System.Drawing.Color.Blue;
            this.btnRunAgain.ForeColor = System.Drawing.Color.White;
            this.btnRunAgain.Location = new System.Drawing.Point(421, 36);
            this.btnRunAgain.Name = "btnRunAgain";
            this.btnRunAgain.Size = new System.Drawing.Size(75, 23);
            this.btnRunAgain.TabIndex = 26;
            this.btnRunAgain.Text = "Run Again";
            this.btnRunAgain.UseVisualStyleBackColor = false;
            // 
            // btnExportResults
            // 
            this.btnExportResults.Location = new System.Drawing.Point(517, 37);
            this.btnExportResults.Name = "btnExportResults";
            this.btnExportResults.Size = new System.Drawing.Size(106, 23);
            this.btnExportResults.TabIndex = 27;
            this.btnExportResults.Text = "Export Results";
            this.btnExportResults.UseVisualStyleBackColor = true;
            // 
            // frmTestRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 453);
            this.Controls.Add(this.btnExportResults);
            this.Controls.Add(this.btnRunAgain);
            this.Controls.Add(this.lblExecFinished);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblExecStarted);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTestStatus);
            this.Controls.Add(this.label1);
            this.Name = "frmTestRun";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTestRun";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lblTestStatus, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.lblExecStarted, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.lblExecFinished, 0);
            this.Controls.SetChildIndex(this.btnRunAgain, 0);
            this.Controls.SetChildIndex(this.btnExportResults, 0);
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
    }
}
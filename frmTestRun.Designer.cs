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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(25, 40);
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
            this.lblTestStatus.Location = new System.Drawing.Point(113, 37);
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
            this.label2.Location = new System.Drawing.Point(26, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 19);
            this.label2.TabIndex = 24;
            this.label2.Text = "Execution started at ";
            // 
            // lblExecStarted
            // 
            this.lblExecStarted.AutoSize = true;
            this.lblExecStarted.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExecStarted.Location = new System.Drawing.Point(157, 80);
            this.lblExecStarted.Name = "lblExecStarted";
            this.lblExecStarted.Size = new System.Drawing.Size(89, 19);
            this.lblExecStarted.TabIndex = 25;
            this.lblExecStarted.Text = "<date_time>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(26, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 19);
            this.label3.TabIndex = 24;
            this.label3.Text = "Execution finished at ";
            // 
            // lblExecFinished
            // 
            this.lblExecFinished.AutoSize = true;
            this.lblExecFinished.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExecFinished.Location = new System.Drawing.Point(161, 106);
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
            this.btnRunAgain.Location = new System.Drawing.Point(5, 15);
            this.btnRunAgain.Name = "btnRunAgain";
            this.btnRunAgain.Size = new System.Drawing.Size(111, 48);
            this.btnRunAgain.TabIndex = 26;
            this.btnRunAgain.Text = "Run Again";
            this.btnRunAgain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRunAgain.UseVisualStyleBackColor = false;
            // 
            // btnExportResults
            // 
            this.btnExportResults.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportResults.Image = global::PlatnedTestMatic.Properties.Resources.Export;
            this.btnExportResults.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportResults.Location = new System.Drawing.Point(122, 15);
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
            this.groupBox1.Controls.Add(this.btnRunAgain);
            this.groupBox1.Controls.Add(this.btnExportResults);
            this.groupBox1.Location = new System.Drawing.Point(408, 28);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(263, 69);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            // 
            // frmTestRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 453);
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
            this.groupBox1.ResumeLayout(false);
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
    }
}
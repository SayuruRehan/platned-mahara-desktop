namespace PlatnedTestMatic
{
    partial class frmBaseForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBaseForm));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblFooter = new System.Windows.Forms.Label();
            this.btnTopClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "cancel.png");
            this.imageList1.Images.SetKeyName(1, "Deep_Close.png");
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.SystemColors.HotTrack;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(907, 30);
            this.lblHeader.TabIndex = 17;
            this.lblHeader.Text = "Platned TestMatic";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseDown);
            this.lblHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseMove);
            this.lblHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
            // 
            // lblFooter
            // 
            this.lblFooter.BackColor = System.Drawing.SystemColors.HotTrack;
            this.lblFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblFooter.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFooter.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblFooter.Location = new System.Drawing.Point(0, 541);
            this.lblFooter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(907, 17);
            this.lblFooter.TabIndex = 20;
            this.lblFooter.Text = "Platned | Your Trusted Partner For Complete ERP Service";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnTopClose
            // 
            this.btnTopClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTopClose.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnTopClose.FlatAppearance.BorderColor = System.Drawing.SystemColors.Highlight;
            this.btnTopClose.FlatAppearance.BorderSize = 0;
            this.btnTopClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DarkBlue;
            this.btnTopClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkBlue;
            this.btnTopClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTopClose.ImageIndex = 1;
            this.btnTopClose.ImageList = this.imageList1;
            this.btnTopClose.Location = new System.Drawing.Point(874, 2);
            this.btnTopClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnTopClose.Name = "btnTopClose";
            this.btnTopClose.Size = new System.Drawing.Size(31, 26);
            this.btnTopClose.TabIndex = 19;
            this.btnTopClose.UseVisualStyleBackColor = false;
            this.btnTopClose.Click += new System.EventHandler(this.btnTopClose_Click);
            this.btnTopClose.MouseLeave += new System.EventHandler(this.btnTopClose_MouseLeave);
            this.btnTopClose.MouseHover += new System.EventHandler(this.btnTopClose_MouseHover);
            // 
            // frmBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(907, 558);
            this.Controls.Add(this.lblFooter);
            this.Controls.Add(this.btnTopClose);
            this.Controls.Add(this.lblHeader);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmBaseForm";
            this.Text = "frmBaseForm";
            this.Load += new System.EventHandler(this.frmBaseForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmBaseForm_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Button btnTopClose;
        private System.Windows.Forms.Label lblFooter;
    }
}
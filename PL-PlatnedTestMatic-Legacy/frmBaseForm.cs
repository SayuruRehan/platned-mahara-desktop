using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PlatnedTestMatic
{
    public partial class frmBaseForm : Form
    {
        private bool dragging = false;
        private Point draggingCursorPoint;
        private Point dragFormPoint;

        public frmBaseForm()
        {
            InitializeComponent();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                                         Color.DarkBlue, 2, ButtonBorderStyle.Solid,
                                         Color.DarkBlue, 2, ButtonBorderStyle.Solid,
                                         Color.DarkBlue, 2, ButtonBorderStyle.Solid,
                                         Color.DarkBlue, 2, ButtonBorderStyle.Solid);
        } 
        public void SetAppearance()
        {
            btnTopClose.Location = new Point(this.Width - btnTopClose.Size.Width, 1);
            this.lblHeader.Text = "LPS|| " + this.Text;
            btnTopClose.Refresh();
        }

        private void frmBaseForm_Load(object sender, EventArgs e)
        {
            this.ActiveControl = lblHeader;
            BringToFront();
        }

        private void lblHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                this.Cursor = Cursors.SizeAll;
                Point diff = Point.Subtract(Cursor.Position, new Size(draggingCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void lblHeader_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
            this.Cursor = Cursors.Arrow;
        }

        private void btnTopClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTopClose_MouseHover(object sender, EventArgs e)
        {
            btnTopClose.ImageIndex = 0;
        }

        private void btnTopClose_MouseLeave(object sender, EventArgs e)
        {
            btnTopClose.ImageIndex = 1;
        }

        private void frmBaseForm_Paint(object sender, PaintEventArgs e)
        {
            //Rectangle borderRectangle = this.ClientRectangle;
            //borderRectangle.Inflate(-1, -1);
            //ControlPaint.DrawBorder3D(e.Graphics, borderRectangle,
            //    Border3DStyle.Raised);
        }

        private void lblHeader_MouseDown(object sender, MouseEventArgs e)
        {
            this.Focus();
            this.BringToFront();
            dragging = true;
            this.Cursor = Cursors.SizeAll;
            draggingCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }


       
    }
}

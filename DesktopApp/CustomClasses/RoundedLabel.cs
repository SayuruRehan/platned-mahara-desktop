using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlatnedTestMatic.CustomClasses
{
    public class RoundedLabel : Label
    {
        private int cornerRadius = 20; // Default corner radius

        [Category("Appearance")]
        [Description("The radius of the label's corners.")]
        public int CornerRadius
        {
            get { return cornerRadius; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("CornerRadius must be greater than or equal to 0.");
                cornerRadius = value;
                this.Invalidate(); // Trigger control to be redrawn
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Create the GraphicsPath for rounded corners
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            int radius = cornerRadius * 2;

            // Define arcs for rounded corners
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90); // Top-left corner
            path.AddArc(rect.X + rect.Width - radius, rect.Y, radius, radius, 270, 90); // Top-right corner
            path.AddArc(rect.X + rect.Width - radius, rect.Y + rect.Height - radius, radius, radius, 0, 90); // Bottom-right corner
            path.AddArc(rect.X, rect.Y + rect.Height - radius, radius, radius, 90, 90); // Bottom-left corner
            path.CloseFigure();

            // Smooth drawing
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Fill the label background
            using (SolidBrush brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillPath(brush, path);
            }

            // Draw the text centered
            TextRenderer.DrawText(e.Graphics, this.Text, this.Font, rect, this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }
}

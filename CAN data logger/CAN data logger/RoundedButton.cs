using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CAN_data_logger
{
    internal class RoundedButton : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            // Create a rounded rectangle path
            GraphicsPath path = new GraphicsPath();
            int radius = 20; // Adjust the radius as needed
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(this.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(this.Width - radius, this.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, this.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            this.Region = new Region(path);

            // Remove the blue border line
            pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            pevent.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            pevent.Graphics.Clear(this.BackColor);
            pevent.Graphics.FillPath(new SolidBrush(this.BackColor), path);

            // Center the text
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            pevent.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), this.ClientRectangle, sf);
        }
    }
}

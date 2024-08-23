using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D; // Add this line

namespace CAN_data_logger
{
    internal class shadowPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var graphics = e.Graphics;
            var rectangle = this.ClientRectangle;
            rectangle.Inflate(-1, -1); // reduce the size of the rectangle

            // Create a series of rectangles with decreasing size
            for (int i = 0; i < 255; i++)
            {
                // Create a color with increasing transparency
                Color color = Color.FromArgb(i, Color.White);

                // Create a new rectangle that is slightly smaller than the last
                Rectangle rect = new Rectangle(rectangle.X + i, rectangle.Y + i, rectangle.Width - 2 * i, rectangle.Height - 2 * i);

                // Draw the rectangle with the transparent color
                using (SolidBrush brush = new SolidBrush(color))
                {
                    graphics.FillRectangle(brush, rect);
                }
            }
        }
    }
}
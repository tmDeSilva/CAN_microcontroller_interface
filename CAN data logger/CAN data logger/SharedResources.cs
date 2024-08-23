using CAN_data_logger.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CAN_data_logger
{
    public static class SharedResources
    {
        public static FontFamily DSEG14Font { get; private set; }
        public static FontFamily KardustFont { get; private set; }
        static SharedResources()
        {
            var fontCollection = new System.Drawing.Text.PrivateFontCollection();

            string fontPath = "";
            fontPath = Path.Combine(Application.StartupPath, "Fonts", "DSEG14Classic-Regular.ttf");
            Console.WriteLine(fontPath);
            if (File.Exists(fontPath))
            {
                byte[] fontData = File.ReadAllBytes(fontPath);
                IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
                Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
                fontCollection.AddMemoryFont(fontPtr, fontData.Length);
                Marshal.FreeCoTaskMem(fontPtr);
            }
            else
            {
                MessageBox.Show("Font file not found: " + fontPath);
            }
            fontPath = Path.Combine(Application.StartupPath, "Fonts", "FontsFree-Net-Kardust-Bold.ttf");
            Console.WriteLine(fontPath);
            if (File.Exists(fontPath))
            {
                byte[] fontData = File.ReadAllBytes(fontPath);
                IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
                Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
                fontCollection.AddMemoryFont(fontPtr, fontData.Length);
                Marshal.FreeCoTaskMem(fontPtr);
            }
            else
            {
                MessageBox.Show("Font file not found: " + fontPath);
            }
            DSEG14Font = fontCollection.Families[0];
            KardustFont = fontCollection.Families[1];
        }
    }
}

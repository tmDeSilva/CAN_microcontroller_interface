#region Libraries
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using static System.Windows.Forms.AxHost;
using CAN_data_logger;
using System.Drawing.Text;
using System.CodeDom.Compiler;
using System.Diagnostics.PerformanceData;
#endregion
abstract public class DrawingArea : Panel
{
    protected Graphics graphics;
    protected CANdataPlotter form; 

    public DrawingArea(CANdataPlotter form) 
    {
        this.form = form;

    }
    abstract public void OnDraw();
    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams cp = base.CreateParams;
            cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
            return cp;
        }
    }
    protected override void OnPaintBackground(PaintEventArgs pevent)
    {
        // Don't paint background
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        this.graphics = e.Graphics;
        this.graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        this.graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
        this.graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        this.graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        OnDraw();
    }
}


public class IconDrawing : DrawingArea
{
    #region Field Variables
    //Battery Parameters
    public double SoC = 0; //%
    public double SoH = 0; //%
    public double voltage = 0.0; //(V)
    public double current = 0.0; //A
    public double maxTemperature = 0; //°C
    public double minTemperature = 0;
    
    //Misc
    public int ccount = 0;
    private string SoCStringPrev = "";
    private string SoHStringPrev = "";
    public bool drawAnyway = false;

    //Labels
    private Label voltageLabel = new Label();
    private Label currentLabel = new Label();
    private Label tempLabel = new Label();
    private Label powerLabel = new Label();
    #endregion
    public IconDrawing(CANdataPlotter form1) : base(form1) { }
    #region Random Functions
    public string ToZeroOrNot(string x)
    {
        if (x == "0")
        {
            return "0.0";
        }
        else
        {
            return x;
        }
    }//0 => 0.0 (For formatting)
    public void DrawRoundedRectangle(Graphics g, Rectangle r, int d, Brush brush)
    {
        System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();

        gp.AddArc(r.X, r.Y, d, d, 180, 90);
        gp.AddArc(r.X + r.Width - d, r.Y, d, d, 270, 90);
        gp.AddArc(r.X + r.Width - d, r.Y + r.Height - d, d, d, 0, 90);
        gp.AddArc(r.X, r.Y + r.Height - d, d, d, 90, 90);

        g.FillPath(brush, gp);
    }
    public Point pointFToPoint(PointF pPoint)
    {
        return new Point((int)pPoint.X, (int)pPoint.Y);
    } //Convert a point with float coords to it coord (suitable for System.Windows.Forms.Labels)
    #endregion
    public override void OnDraw() //Draw the icons

    {
        using (this.graphics = this.CreateGraphics())
        {

            //Misc. Variables
            int SoCBarHeight = (int)(118 * (1 - SoC));
            int SoCBarShift = (int)(SoCBarHeight / (2 * Math.Sqrt(2)));

            //Circle Variales
            int width = 205;
            int height = 205;
            int x = 10; int y = 10;
            int width2 = 205;
            int height2 = 205;
            int x2 = 10; int y2 = 225;

            //Output Strings
            String SoCString = "SoC: " + Convert.ToString((int)(Math.Round(SoC * 100, 0))) + "%";
            String SoHString = "SoH: " + Convert.ToString((int)(Math.Round(SoH * 100, 0))) + "%";
            string voltageTitle = "Voltage";
            string currentTitle = "Current";
            //string voltageValue = Convert.ToString((int)(voltage / 1)) + " . " + ToZeroOrNot(Convert.ToString(Math.Round((voltage % 1), 2))).Substring(2) + "V";
            string voltageValue = Convert.ToString(Math.Round(voltage, 2)) + "V";
            string currentValue = "";
            if (Math.Abs(current) < 1)
            {
                currentValue = Convert.ToString(Math.Round(current * 1000, 0)) + " mA";
            }
            else
            {
                currentValue = Convert.ToString((int)(current / 1)) + " . " + ToZeroOrNot(Convert.ToString(Math.Round((current % 1), 2))).Substring(2) + " A";
            }
            //String tempValue = Convert.ToString(Math.Round(minTemperature, 0)) + " °C - " + Math.Round(maxTemperature, 0) + " °C";
            string tempValue = Math.Round(maxTemperature, 0) + " °C"; 
            String tempTitle = "Temperature";
            String powerTitle = "Power";
            String powerValue = "";
            if (Math.Abs(current * voltage) < 1)
            {
                powerValue = Convert.ToString(Math.Round(current * voltage * 1000, 0)) + " mW";
            }
            else if (Math.Abs(current * voltage) > 1000)
            {
                powerValue = Convert.ToString(Math.Round((current * voltage / 1000 % 1), 2)) +" kW";
            }
            else
            {
                powerValue = Convert.ToString(Math.Round((current * voltage % 1), 2)) + " W";
            }

            //Fonts
            Font DSEG14 = new Font(SharedResources.DSEG14Font, 14);
            Font KardustBig = new Font(SharedResources.KardustFont, 20);
            Font Kardust = new Font(SharedResources.KardustFont, 14);

            SizeF SoCStringSize = this.graphics.MeasureString(SoCString, DSEG14);
            SizeF SoHStringSize = this.graphics.MeasureString(SoHString, DSEG14);
            SizeF tempStringSize = this.graphics.MeasureString(tempValue, DSEG14);
            SizeF tempTitleSize = this.graphics.MeasureString(tempTitle, Kardust);
            SizeF voltageStringSize = this.graphics.MeasureString(voltageValue, DSEG14);
            SizeF currentStringSize = this.graphics.MeasureString(currentValue, DSEG14);
            SizeF voltageTitleSize = this.graphics.MeasureString(voltageTitle, Kardust);
            SizeF currentTitleSize = this.graphics.MeasureString(currentTitle, Kardust);
            SizeF powerStringSize = this.graphics.MeasureString(powerValue, DSEG14);
            SizeF powerTitleSize = this.graphics.MeasureString(powerTitle, Kardust);


            //Output String positions
            PointF SoCStringPoint = new PointF(790.0F, 190.0F);
            PointF SoHStringPoint = new PointF(790.0F, 375.0F);
            PointF voltageStringPoint = new PointF(((float)(115) - voltageStringSize.Width / 2), ((float)(80) - voltageStringSize.Height / 2));
            PointF currentStringPoint = new PointF(((float)(115) - currentStringSize.Width / 2), ((float)(160) - currentStringSize.Height / 2));
            PointF voltageTitlePoint = new PointF(((float)(115) - voltageTitleSize.Width / 2), ((float)(50) - voltageTitleSize.Height / 2));
            PointF powerStringPoint = new PointF(((float)(115) - powerStringSize.Width / 2), ((float)(390) - powerStringSize.Height / 2));
            PointF powerTitlePoint = new PointF(((float)(115) - powerTitleSize.Width / 2), ((float)(360) - powerTitleSize.Height / 2));
            PointF currentTitlePoint = new PointF(((float)(115) - currentTitleSize.Width / 2), ((float)(130) - currentTitleSize.Height / 2));
            PointF tempStringPoint = new PointF(115F - tempStringSize.Width / 2, 320F - tempStringSize.Height / 2);
            PointF tempTitlePoint = new PointF(115F - tempTitleSize.Width / 2, 280F - tempTitleSize.Height / 2);

            //Brushes
            Color panelColor = Color.White;
            Brush panelBrush = new SolidBrush(panelColor);
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            SolidBrush redSpecBrush = new SolidBrush(Color.FromArgb(255, 40, 46));
            Pen whitePen = new Pen(Color.White);



            //Images
            Image NyoLogoFront = global::CAN_data_logger.Properties.Resources.NyoLogo;
            Image NyoLogoBack = global::CAN_data_logger.Properties.Resources.NyoLogoBg;
            Image SoHFront = global::CAN_data_logger.Properties.Resources.SoHLogo;
            Image SoHBack = global::CAN_data_logger.Properties.Resources.SoHLogoBg;
            Image pack = global::CAN_data_logger.Properties.Resources.forsetiPack;

            //Rectangles (Image positioning and scaling)
            Rectangle front = new Rectangle(750, 0, 199, 199);
            Rectangle SoCBar = new Rectangle(890 - (int)(75 / 2) - SoCBarShift, 57 - (int)(SoCBarHeight / 2) + SoCBarShift, 75, SoCBarHeight);
            Rectangle SoHfront = new Rectangle(787, 250, 130, 130);
            Rectangle SoHBar = new Rectangle(787, 255, 130, (int)((1 - SoH) * 100));
            Rectangle forsetiPackRect = new Rectangle(1000, 180, 220, 148);
            Rectangle SoCBackPanel = new Rectangle(755, 15, 195, 220);
            Rectangle SoHBackPanel = new Rectangle(755, 250, 195, 175);
            Rectangle voltageCover = new Rectangle(50, 65, 100, 50);
            Rectangle currentCover = new Rectangle(50, 140, 100, 50);
            Rectangle powerCover = new Rectangle(50, 375, 100, 30);
            this.graphics.FillRectangle(whiteBrush, voltageCover);
            this.graphics.FillRectangle(whiteBrush, currentCover);
            this.graphics.FillRectangle(whiteBrush, powerCover);


            if (SoCStringPrev != SoCString || drawAnyway) {
                //SoC Graphics
                DrawRoundedRectangle(this.graphics, SoCBackPanel, 20, whiteBrush);
                this.graphics.DrawImage(NyoLogoBack, front);

                //  Rotation
                GraphicsState state = this.graphics.Save();
                Point rectCenter = new Point(SoCBar.Left + SoCBar.Width / 2, SoCBar.Top + SoCBar.Height / 2);
                this.graphics.TranslateTransform(rectCenter.X, rectCenter.Y);
                this.graphics.RotateTransform(45);
                this.graphics.TranslateTransform(-rectCenter.X, -rectCenter.Y);
                this.graphics.FillRectangle(panelBrush, SoCBar);
                this.graphics.Restore(state);

                this.graphics.DrawImage(NyoLogoFront, front);
                this.graphics.DrawString(SoCString, KardustBig, blackBrush, SoCStringPoint);
                
            }

            if (SoHStringPrev != SoHString || drawAnyway) {
                //SoH graphics
                DrawRoundedRectangle(this.graphics, SoHBackPanel, 20, whiteBrush);
                this.graphics.DrawImage(SoHBack, SoHfront);
                this.graphics.FillRectangle(panelBrush, SoHBar);
                this.graphics.DrawImage(SoHFront, SoHfront);
                this.graphics.DrawString(SoHString, KardustBig, blackBrush, SoHStringPoint);
            }

            if (drawAnyway)
            {
                drawAnyway = false;
            }

            if (ccount == 0)
            {
                
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(x - 30, y - 30, width + 60, height + 60);
                System.Drawing.Drawing2D.PathGradientBrush pgb = new System.Drawing.Drawing2D.PathGradientBrush(path);
                pgb.CenterColor = System.Drawing.Color.White;
                pgb.SurroundColors = new System.Drawing.Color[] { System.Drawing.Color.Transparent };
                Graphics g = this.CreateGraphics();
                g.FillPath(pgb, path);
                this.graphics.FillEllipse(whiteBrush, x, y, width, height);



                using (System.Drawing.Drawing2D.GraphicsPath path2 = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path2.AddEllipse(x2 - 30, y2 - 30, width2 + 60, height2 + 60);

                    using (System.Drawing.Drawing2D.PathGradientBrush pgb2 = new System.Drawing.Drawing2D.PathGradientBrush(path2))
                    {
                        pgb2.CenterColor = System.Drawing.Color.White;
                        pgb2.SurroundColors = new System.Drawing.Color[] { System.Drawing.Color.Transparent };
                        g = this.CreateGraphics();
                        g.FillPath(pgb2, path2);
                    }
                }

                using (System.Drawing.Drawing2D.GraphicsPath path3 = new System.Drawing.Drawing2D.GraphicsPath())
                {
                    path3.AddEllipse(980, 210, 260, 128);

                    using (System.Drawing.Drawing2D.PathGradientBrush pgb3 = new System.Drawing.Drawing2D.PathGradientBrush(path3))
                    {
                        pgb3.CenterColor = System.Drawing.Color.White;
                        pgb3.SurroundColors = new System.Drawing.Color[] { System.Drawing.Color.Transparent };
                        g = this.CreateGraphics();
                        g.FillPath(pgb3, path3);
                    }
                }
                this.graphics.FillEllipse(whiteBrush, x2, y2, width2, height2);
                this.graphics.FillEllipse(whiteBrush, x, y, width, height);

                voltageLabel.Location = pointFToPoint(voltageStringPoint);
                voltageLabel.Font = DSEG14;
                voltageLabel.ForeColor = Color.Black;
                voltageLabel.BackColor = Color.White;
                voltageLabel.Text = "";
                this.Controls.Add(voltageLabel);

                currentLabel.Location = pointFToPoint(currentStringPoint);
                currentLabel.Font = DSEG14;
                currentLabel.ForeColor = Color.Black;
                currentLabel.BackColor = Color.White;
                currentLabel.Text = "";
                this.Controls.Add(currentLabel);

                tempLabel.Location = pointFToPoint(tempStringPoint);
                tempLabel.Font = DSEG14;
                tempLabel.ForeColor = Color.Black;
                tempLabel.BackColor = Color.White;
                tempLabel.Text = "";
                this.Controls.Add(tempLabel);

                powerLabel.Location = pointFToPoint(powerStringPoint);
                powerLabel.Font = DSEG14;
                powerLabel.ForeColor = Color.Black;
                powerLabel.BackColor = Color.White;
                powerLabel.Text = "";
                this.Controls.Add(powerLabel);

                this.graphics.DrawString(voltageTitle, Kardust, blackBrush, voltageTitlePoint);
                this.graphics.DrawString(currentTitle, Kardust, blackBrush, currentTitlePoint);
                this.graphics.DrawString(tempTitle, Kardust, blackBrush, tempTitlePoint);
                this.graphics.DrawString(powerTitle, Kardust, blackBrush, powerTitlePoint);

                this.graphics.DrawImage(pack, forsetiPackRect);
                ccount = 1;
            }


            voltageLabel.Location = pointFToPoint(voltageStringPoint);
            currentLabel.Location = pointFToPoint(currentStringPoint);
            tempLabel.Location = pointFToPoint(tempStringPoint);
            powerLabel.Location = pointFToPoint(powerStringPoint);
            
            voltageLabel.Text = voltageValue;
            currentLabel.Text = currentValue;
            tempLabel.Text = tempValue;
            powerLabel.Text = powerValue;

            SoCStringPrev = SoCString;
            SoHStringPrev = SoHString;
        }
    }
}

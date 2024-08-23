namespace CAN_data_logger
{
    partial class CANdataPlotter
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea9 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea10 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CANdataPlotter));
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.plotter = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.parameterCombo = new System.Windows.Forms.ComboBox();
            this.componentCombo = new System.Windows.Forms.ComboBox();
            this.portCombo = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.versionText = new System.Windows.Forms.Label();
            this.modeButton = new System.Windows.Forms.Button();
            this.componentListbox = new System.Windows.Forms.ListBox();
            this.logo = new System.Windows.Forms.PictureBox();
            this.howToUse = new System.Windows.Forms.Label();
            this.btnUpload = new CAN_data_logger.RoundedButton();
            this.shadowPanel6 = new CAN_data_logger.shadowPanel();
            this.hexData = new System.Windows.Forms.ListBox();
            this.roundedButton3 = new CAN_data_logger.RoundedButton();
            this.refreshButton = new CAN_data_logger.RoundedButton();
            this.startButton = new CAN_data_logger.RoundedButton();
            this.shadowPanel3 = new CAN_data_logger.shadowPanel();
            this.shadowPanel1 = new CAN_data_logger.shadowPanel();
            this.cycleCountLabel = new System.Windows.Forms.Label();
            this.cycleCount = new System.Windows.Forms.Label();
            this.roundedButton1 = new CAN_data_logger.RoundedButton();
            this.shadowPanel2 = new CAN_data_logger.shadowPanel();
            this.shadowPanel7 = new CAN_data_logger.shadowPanel();
            this.legendPanel = new System.Windows.Forms.Panel();
            this.legendTextBox = new System.Windows.Forms.RichTextBox();
            this.roundedButton2 = new CAN_data_logger.RoundedButton();
            this.shadowPanel5 = new CAN_data_logger.shadowPanel();
            this.shadowPanel4 = new CAN_data_logger.shadowPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.roundedButton4 = new CAN_data_logger.RoundedButton();
            this.btnDownload = new CAN_data_logger.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plotter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.shadowPanel6.SuspendLayout();
            this.shadowPanel1.SuspendLayout();
            this.shadowPanel7.SuspendLayout();
            this.legendPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea9.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea9);
            legend5.Name = "Legend1";
            this.chart1.Legends.Add(legend5);
            this.chart1.Location = new System.Drawing.Point(165, 112);
            this.chart1.Name = "chart1";
            series9.ChartArea = "ChartArea1";
            series9.Legend = "Legend1";
            series9.Name = "Series1";
            this.chart1.Series.Add(series9);
            this.chart1.Size = new System.Drawing.Size(0, 0);
            this.chart1.TabIndex = 6;
            this.chart1.Text = "chart1";
            // 
            // plotter
            // 
            chartArea10.AxisX.InterlacedColor = System.Drawing.Color.White;
            chartArea10.AxisX.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea10.AxisX.LineColor = System.Drawing.Color.White;
            chartArea10.AxisX.MajorGrid.Enabled = false;
            chartArea10.AxisX.MajorGrid.LineColor = System.Drawing.Color.White;
            chartArea10.AxisX.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea10.AxisX.TitleForeColor = System.Drawing.Color.White;
            chartArea10.AxisY.LabelStyle.ForeColor = System.Drawing.Color.White;
            chartArea10.AxisY.LineColor = System.Drawing.Color.White;
            chartArea10.AxisY.MajorGrid.Enabled = false;
            chartArea10.AxisY.MajorGrid.LineColor = System.Drawing.Color.White;
            chartArea10.AxisY.MajorTickMark.LineColor = System.Drawing.Color.White;
            chartArea10.BackColor = System.Drawing.Color.Transparent;
            chartArea10.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            chartArea10.Name = "ChartArea1";
            this.plotter.ChartAreas.Add(chartArea10);
            this.plotter.Location = new System.Drawing.Point(229, 112);
            this.plotter.Name = "plotter";
            series10.ChartArea = "ChartArea1";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StepLine;
            series10.Color = System.Drawing.Color.Red;
            series10.LabelBackColor = System.Drawing.Color.Transparent;
            series10.LabelForeColor = System.Drawing.Color.White;
            series10.Name = "Series1";
            this.plotter.Series.Add(series10);
            this.plotter.Size = new System.Drawing.Size(505, 290);
            this.plotter.TabIndex = 7;
            this.plotter.Text = "chart2";
            this.plotter.Click += new System.EventHandler(this.plotter_Click);
            // 
            // parameterCombo
            // 
            this.parameterCombo.BackColor = System.Drawing.Color.White;
            this.parameterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.parameterCombo.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.parameterCombo.FormattingEnabled = true;
            this.parameterCombo.Location = new System.Drawing.Point(231, 490);
            this.parameterCombo.Name = "parameterCombo";
            this.parameterCombo.Size = new System.Drawing.Size(290, 24);
            this.parameterCombo.TabIndex = 9;
            // 
            // componentCombo
            // 
            this.componentCombo.BackColor = System.Drawing.Color.White;
            this.componentCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.componentCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.componentCombo.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.componentCombo.FormattingEnabled = true;
            this.componentCombo.Location = new System.Drawing.Point(531, 490);
            this.componentCombo.Name = "componentCombo";
            this.componentCombo.Size = new System.Drawing.Size(215, 24);
            this.componentCombo.TabIndex = 10;
            // 
            // portCombo
            // 
            this.portCombo.AllowDrop = true;
            this.portCombo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.portCombo.BackColor = System.Drawing.Color.White;
            this.portCombo.DropDownHeight = 400;
            this.portCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.portCombo.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.portCombo.ForeColor = System.Drawing.Color.Black;
            this.portCombo.FormattingEnabled = true;
            this.portCombo.IntegralHeight = false;
            this.portCombo.Items.AddRange(new object[] {
            "COM4",
            "COM8"});
            this.portCombo.Location = new System.Drawing.Point(819, 519);
            this.portCombo.Name = "portCombo";
            this.portCombo.Size = new System.Drawing.Size(141, 24);
            this.portCombo.TabIndex = 11;
            this.portCombo.SelectedIndexChanged += new System.EventHandler(this.portCombo_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(401, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(179, 59);
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(237, 457);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 23);
            this.label1.TabIndex = 21;
            this.label1.Text = "Battery Parameters";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Tai Le", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(527, 457);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 23);
            this.label2.TabIndex = 22;
            this.label2.Text = "Component";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Tai Le", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(883, 488);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 23);
            this.label3.TabIndex = 23;
            this.label3.Text = "Files";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 25;
            this.label4.Text = "label4";
            // 
            // infoLabel
            // 
            this.infoLabel.ForeColor = System.Drawing.Color.White;
            this.infoLabel.Location = new System.Drawing.Point(1004, 343);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(230, 123);
            this.infoLabel.TabIndex = 26;
            this.infoLabel.Text = "Forseti 28-000001 Nominal Voltage: 35.2V Maximum Charge voltage: 46.3V DC Nominal" +
    " Capacity:8.1 Ah";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // versionText
            // 
            this.versionText.AutoSize = true;
            this.versionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.versionText.ForeColor = System.Drawing.Color.White;
            this.versionText.Location = new System.Drawing.Point(1030, 621);
            this.versionText.Name = "versionText";
            this.versionText.Size = new System.Drawing.Size(160, 26);
            this.versionText.TabIndex = 28;
            this.versionText.Text = "Nyobolt CAN data Logger V1.0.0\r\nThehan De Silva";
            this.versionText.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // modeButton
            // 
            this.modeButton.BackColor = System.Drawing.Color.Black;
            this.modeButton.BackgroundImage = global::CAN_data_logger.Properties.Resources.disconnected;
            this.modeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.modeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.modeButton.ForeColor = System.Drawing.Color.Black;
            this.modeButton.Location = new System.Drawing.Point(763, 503);
            this.modeButton.Name = "modeButton";
            this.modeButton.Size = new System.Drawing.Size(50, 50);
            this.modeButton.TabIndex = 31;
            this.modeButton.UseVisualStyleBackColor = false;
            this.modeButton.Click += new System.EventHandler(this.modeButton_Click);
            // 
            // componentListbox
            // 
            this.componentListbox.BackColor = System.Drawing.Color.Black;
            this.componentListbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.componentListbox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.componentListbox.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.componentListbox.FormattingEnabled = true;
            this.componentListbox.Location = new System.Drawing.Point(763, 558);
            this.componentListbox.Name = "componentListbox";
            this.componentListbox.Size = new System.Drawing.Size(185, 91);
            this.componentListbox.TabIndex = 32;
            this.componentListbox.SelectedIndexChanged += new System.EventHandler(this.componentListbox_SelectedIndexChanged);
            // 
            // logo
            // 
            this.logo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("logo.BackgroundImage")));
            this.logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.logo.Location = new System.Drawing.Point(1196, 607);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(40, 40);
            this.logo.TabIndex = 39;
            this.logo.TabStop = false;
            this.logo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.logo_MouseClick);
            this.logo.MouseEnter += new System.EventHandler(this.logo_MouseEnter);
            this.logo.MouseLeave += new System.EventHandler(this.logo_MouseLeave);
            // 
            // howToUse
            // 
            this.howToUse.AutoSize = true;
            this.howToUse.Location = new System.Drawing.Point(649, 453);
            this.howToUse.Name = "howToUse";
            this.howToUse.Size = new System.Drawing.Size(35, 13);
            this.howToUse.TabIndex = 40;
            this.howToUse.Text = "label5";
            // 
            // btnUpload
            // 
            this.btnUpload.BackColor = System.Drawing.Color.SteelBlue;
            this.btnUpload.Font = new System.Drawing.Font("Agency FB", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.ForeColor = System.Drawing.Color.White;
            this.btnUpload.Location = new System.Drawing.Point(1154, 477);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(50, 50);
            this.btnUpload.TabIndex = 42;
            this.btnUpload.Text = "⤒";
            this.btnUpload.UseVisualStyleBackColor = false;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // shadowPanel6
            // 
            this.shadowPanel6.Controls.Add(this.hexData);
            this.shadowPanel6.Controls.Add(this.roundedButton3);
            this.shadowPanel6.Location = new System.Drawing.Point(0, 515);
            this.shadowPanel6.Name = "shadowPanel6";
            this.shadowPanel6.Size = new System.Drawing.Size(225, 150);
            this.shadowPanel6.TabIndex = 38;
            // 
            // hexData
            // 
            this.hexData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hexData.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hexData.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.hexData.FormattingEnabled = true;
            this.hexData.ItemHeight = 14;
            this.hexData.Location = new System.Drawing.Point(29, 20);
            this.hexData.Name = "hexData";
            this.hexData.Size = new System.Drawing.Size(168, 112);
            this.hexData.TabIndex = 36;
            // 
            // roundedButton3
            // 
            this.roundedButton3.BackColor = System.Drawing.Color.White;
            this.roundedButton3.Location = new System.Drawing.Point(17, 14);
            this.roundedButton3.Name = "roundedButton3";
            this.roundedButton3.Size = new System.Drawing.Size(190, 123);
            this.roundedButton3.TabIndex = 37;
            this.roundedButton3.UseVisualStyleBackColor = false;
            // 
            // refreshButton
            // 
            this.refreshButton.BackColor = System.Drawing.Color.SteelBlue;
            this.refreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.refreshButton.ForeColor = System.Drawing.Color.White;
            this.refreshButton.Location = new System.Drawing.Point(900, 445);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(40, 40);
            this.refreshButton.TabIndex = 33;
            this.refreshButton.Text = "⟳";
            this.refreshButton.UseVisualStyleBackColor = false;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // startButton
            // 
            this.startButton.BackColor = System.Drawing.Color.White;
            this.startButton.Location = new System.Drawing.Point(764, 445);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(130, 39);
            this.startButton.TabIndex = 30;
            this.startButton.Text = "START";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // shadowPanel3
            // 
            this.shadowPanel3.Location = new System.Drawing.Point(740, 240);
            this.shadowPanel3.Name = "shadowPanel3";
            this.shadowPanel3.Size = new System.Drawing.Size(225, 200);
            this.shadowPanel3.TabIndex = 29;
            // 
            // shadowPanel1
            // 
            this.shadowPanel1.Controls.Add(this.cycleCountLabel);
            this.shadowPanel1.Controls.Add(this.cycleCount);
            this.shadowPanel1.Controls.Add(this.roundedButton1);
            this.shadowPanel1.Location = new System.Drawing.Point(0, 436);
            this.shadowPanel1.Name = "shadowPanel1";
            this.shadowPanel1.Size = new System.Drawing.Size(225, 88);
            this.shadowPanel1.TabIndex = 12;
            // 
            // cycleCountLabel
            // 
            this.cycleCountLabel.BackColor = System.Drawing.Color.White;
            this.cycleCountLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cycleCountLabel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cycleCountLabel.Location = new System.Drawing.Point(121, 17);
            this.cycleCountLabel.Name = "cycleCountLabel";
            this.cycleCountLabel.Size = new System.Drawing.Size(76, 56);
            this.cycleCountLabel.TabIndex = 21;
            this.cycleCountLabel.Text = "Cycles";
            this.cycleCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cycleCountLabel.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // cycleCount
            // 
            this.cycleCount.BackColor = System.Drawing.Color.White;
            this.cycleCount.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.cycleCount.Location = new System.Drawing.Point(36, 17);
            this.cycleCount.Name = "cycleCount";
            this.cycleCount.Size = new System.Drawing.Size(79, 56);
            this.cycleCount.TabIndex = 18;
            this.cycleCount.Text = "0";
            this.cycleCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cycleCount.Click += new System.EventHandler(this.timeUntilEmptyFull_Click);
            // 
            // roundedButton1
            // 
            this.roundedButton1.BackColor = System.Drawing.Color.White;
            this.roundedButton1.Location = new System.Drawing.Point(17, 17);
            this.roundedButton1.Name = "roundedButton1";
            this.roundedButton1.Size = new System.Drawing.Size(190, 56);
            this.roundedButton1.TabIndex = 31;
            this.roundedButton1.UseVisualStyleBackColor = false;
            // 
            // shadowPanel2
            // 
            this.shadowPanel2.Location = new System.Drawing.Point(740, 0);
            this.shadowPanel2.Name = "shadowPanel2";
            this.shadowPanel2.Size = new System.Drawing.Size(225, 250);
            this.shadowPanel2.TabIndex = 13;
            // 
            // shadowPanel7
            // 
            this.shadowPanel7.Controls.Add(this.legendPanel);
            this.shadowPanel7.Location = new System.Drawing.Point(213, 94);
            this.shadowPanel7.Name = "shadowPanel7";
            this.shadowPanel7.Size = new System.Drawing.Size(531, 360);
            this.shadowPanel7.TabIndex = 18;
            // 
            // legendPanel
            // 
            this.legendPanel.BackColor = System.Drawing.Color.White;
            this.legendPanel.Controls.Add(this.legendTextBox);
            this.legendPanel.Location = new System.Drawing.Point(16, 306);
            this.legendPanel.Name = "legendPanel";
            this.legendPanel.Size = new System.Drawing.Size(505, 40);
            this.legendPanel.TabIndex = 41;
            // 
            // legendTextBox
            // 
            this.legendTextBox.Location = new System.Drawing.Point(3, 2);
            this.legendTextBox.Name = "legendTextBox";
            this.legendTextBox.Size = new System.Drawing.Size(499, 35);
            this.legendTextBox.TabIndex = 41;
            this.legendTextBox.Text = "";
            // 
            // roundedButton2
            // 
            this.roundedButton2.BackColor = System.Drawing.Color.Black;
            this.roundedButton2.Location = new System.Drawing.Point(748, 557);
            this.roundedButton2.Name = "roundedButton2";
            this.roundedButton2.Size = new System.Drawing.Size(211, 96);
            this.roundedButton2.TabIndex = 34;
            this.roundedButton2.UseVisualStyleBackColor = false;
            // 
            // shadowPanel5
            // 
            this.shadowPanel5.Location = new System.Drawing.Point(740, 544);
            this.shadowPanel5.Name = "shadowPanel5";
            this.shadowPanel5.Size = new System.Drawing.Size(227, 121);
            this.shadowPanel5.TabIndex = 35;
            // 
            // shadowPanel4
            // 
            this.shadowPanel4.Location = new System.Drawing.Point(966, -10);
            this.shadowPanel4.Name = "shadowPanel4";
            this.shadowPanel4.Size = new System.Drawing.Size(25, 694);
            this.shadowPanel4.TabIndex = 27;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.BackgroundImage = global::CAN_data_logger.Properties.Resources.textFileImage;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(1033, 467);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(115, 117);
            this.pictureBox2.TabIndex = 43;
            this.pictureBox2.TabStop = false;
            // 
            // roundedButton4
            // 
            this.roundedButton4.BackColor = System.Drawing.Color.White;
            this.roundedButton4.Location = new System.Drawing.Point(1017, 467);
            this.roundedButton4.Name = "roundedButton4";
            this.roundedButton4.Size = new System.Drawing.Size(201, 123);
            this.roundedButton4.TabIndex = 37;
            this.roundedButton4.UseVisualStyleBackColor = false;
            this.roundedButton4.Click += new System.EventHandler(this.roundedButton4_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.SteelBlue;
            this.btnDownload.Font = new System.Drawing.Font("Agency FB", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownload.ForeColor = System.Drawing.Color.White;
            this.btnDownload.Location = new System.Drawing.Point(1154, 533);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(50, 50);
            this.btnDownload.TabIndex = 41;
            this.btnDownload.Text = "⤓";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // CANdataPlotter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1246, 661);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnUpload);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.howToUse);
            this.Controls.Add(this.logo);
            this.Controls.Add(this.shadowPanel6);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.componentListbox);
            this.Controls.Add(this.modeButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.shadowPanel3);
            this.Controls.Add(this.versionText);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.portCombo);
            this.Controls.Add(this.componentCombo);
            this.Controls.Add(this.parameterCombo);
            this.Controls.Add(this.plotter);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.shadowPanel1);
            this.Controls.Add(this.shadowPanel2);
            this.Controls.Add(this.shadowPanel7);
            this.Controls.Add(this.roundedButton2);
            this.Controls.Add(this.shadowPanel5);
            this.Controls.Add(this.shadowPanel4);
            this.Controls.Add(this.roundedButton4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CANdataPlotter";
            this.Text = "Nyobolt CAN datalogger";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plotter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.shadowPanel6.ResumeLayout(false);
            this.shadowPanel1.ResumeLayout(false);
            this.shadowPanel7.ResumeLayout(false);
            this.legendPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Main Visuals

        //Dropdown menu
        private System.Windows.Forms.ComboBox parameterCombo;
        private System.Windows.Forms.ComboBox componentCombo;
        private System.Windows.Forms.ComboBox portCombo;

        //Graphs
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DataVisualization.Charting.Chart plotter;

        //Shadow Panels (for aesthetics)
        private shadowPanel shadowPanel1;
        private shadowPanel shadowPanel2;

        //Pictures and Labels
        private System.Windows.Forms.PictureBox pictureBox1;
        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label infoLabel;
        private shadowPanel shadowPanel4;
        private System.Windows.Forms.Label versionText;
        private shadowPanel shadowPanel3;
        private RoundedButton startButton;
        private System.Windows.Forms.Label cycleCount;
        private System.Windows.Forms.Label cycleCountLabel;
        private RoundedButton roundedButton1;
        private System.Windows.Forms.Button modeButton;
        private System.Windows.Forms.ListBox componentListbox;
        private RoundedButton refreshButton;
        private RoundedButton roundedButton2;
        private shadowPanel shadowPanel5;
        private System.Windows.Forms.ListBox hexData;
        private RoundedButton roundedButton3;
        private shadowPanel shadowPanel6;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Label howToUse;
        private shadowPanel shadowPanel7;
        private System.Windows.Forms.Panel legendPanel;
        private System.Windows.Forms.RichTextBox legendTextBox;
        private RoundedButton btnUpload;
        private System.Windows.Forms.PictureBox pictureBox2;
        private RoundedButton roundedButton4;
        private RoundedButton btnDownload;
    }
}


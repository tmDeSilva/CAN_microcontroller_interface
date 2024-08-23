#region Libraries
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Imaging;
using static System.Windows.Forms.LinkLabel;
using System.Windows.Forms.DataVisualization.Charting;
using LiveCharts.Wpf.Charts.Base;
using System.Data.OleDb;
using System.Windows.Input;
using System.IO.Ports;
using CAN_data_logger.Properties;
using System.Windows.Controls;
#endregion

namespace CAN_data_logger
{

    public partial class CANdataPlotter : Form
    {

        #region Field Variables
        //Imports
        private CanDecoder decoder; //raw CAN data / DBC file DECODER
        private SerialPort serialPort; //Initialize the Serial Port
        private IconDrawing iconDrawing; //Separate class for drawing icons (To overwrite the faulty default "transparent" picture settings
        private DeviceScanner deviceScanner; //Initialize the scanner for serial / bluetooth comms
        public PrivateFontCollection privateFonts = new PrivateFontCollection(); //Import DSEG font

        //Other Variables
        private Dictionary<string, string> parameterToUnit = new Dictionary<string, string>();
        private string axisIdentifier = "AX";
        private bool timerStarted = false; //Start the timer
        private System.Windows.Forms.Timer timer;
        private double startTime = 0.0;

        private int indexFile = 0; //Index for line in the SampleData.txt file
        public List<string> sampleDataLines { get; private set; } //Store all the lines of SampleData.txt in this list

        public static int mode = 0; //Modes: {0 : disconnected, 1 : Serial (USB), 2 : Bluetooth (Classic)}
        private int modeButtonHeight = 40;
        private List<System.Drawing.Image> images = new List<System.Drawing.Image>(); //Mode button images

        private List<string> chartLegendList = new List<string>(); //The elements in the Chart (plotter) legend
        private List<Color> colours = new List<Color>(); //Colour each graph in the Chart (plotter)
        private List<string> canDataToWrite = new List<string>();

        public static Dictionary<string,List<string>> uploadedFiles = new Dictionary<string,List<string>>();

        private string nullData = "000 0 00 00 00 00 00 00 00 00";
        #endregion

        public CANdataPlotter()
        {
            #region CANdataPlotter Init
            #region General
            InitializeComponent();

            //Setup for debugging so Console.WriteLine() prints to the VS Output
            Console.SetOut(new DebugTextWriter());
            btnDownload.Enabled = false;
            #endregion

            #region Text File assignment
            sampleDataLines = new List<string>(Properties.Resources.sampleData.Split(new string[] { "\r\n" },StringSplitOptions.None));
            decoder = new CanDecoder(Properties.Resources.CANdbcFile, 
                Properties.Resources.sampleData,
                Properties.Resources.EssentialParameters);
            #endregion region

            #region ComboBox and ListBox setup
            //Trigger the Chart Legend update and allow the Start Button selection when the Parameter, Component and Port have been chosen
            portCombo.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            parameterCombo.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            componentCombo.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);

            //A custom function that allows the Legend components to be coloured based on the Graph colour
            componentListbox.DrawItem += new DrawItemEventHandler(componentListbox_DrawItem);

            portCombo.Enabled = true;
            parameterCombo.Enabled = true;
            componentCombo.Enabled = true;

            //Fonts
            portCombo.Font = new Font(SharedResources.KardustFont, 11);
            componentCombo.Font = new Font(SharedResources.KardustFont, 11);
            parameterCombo.Font = new Font(SharedResources.KardustFont, 11);
            componentListbox.Font = new Font(SharedResources.KardustFont, 11);

            colours.Add(Color.Red);
            colours.Add(Color.MediumOrchid);
            colours.Add(Color.DodgerBlue);
            colours.Add(Color.DarkGreen);
 
            //Iterate through all the available CAN channel IDs and add this to the ID Combobox
            foreach (var parameter in decoder.parameterToComponent.Keys) 
            {
                parameterCombo.Items.Add(parameter);
            }

            //Call a function when the item chosen in the ID Combobox is changed
            parameterCombo.SelectedIndexChanged += IDCombo_SelectedIndexChanged;
            #endregion
            #region timer setup
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;
            timer.Tick += Timer_Tick;
            #endregion
            #region label setup
            LoadCustomFont();
            cycleCountLabel.Font = new Font(SharedResources.KardustFont, 12);
            label1.Font = new Font(SharedResources.KardustFont, 16);
            label2.Font = new Font(SharedResources.KardustFont, 16);
            label3.Font = new Font(SharedResources.KardustFont, 16);
            infoLabel.Font = new Font(SharedResources.KardustFont, 12);

            infoLabel.Text = "Forseti 28-000001\nNominal Voltage: 35.2V\nMaximum Charge Voltage: 46.3V\nDC Nominal Capacity:8.1 Ah";
            cycleCount.Font = new Font(privateFonts.Families[0], 20);
            #endregion

            #region Legend Text Box Setup
            legendTextBox.BorderStyle = BorderStyle.None;
            legendTextBox.Font = new Font(SharedResources.KardustFont, 10);
            #endregion

            #region Start Button setup
            //Start Button visuals
            startButton.BackColor = Color.FromArgb(80, 180, 0);
            startButton.Font = new Font(SharedResources.KardustFont, 18);
            startButton.ForeColor = Color.White;
            startButton.Text = "START";
            startButton.Enabled = false;

            //Hover functionality
            //Go Red when the data is being plotted and Green whilst waiting to start plotting
            startButton.MouseEnter += (object sender, EventArgs e) =>
            {
                if (!timerStarted)
                {
                    startButton.BackColor = Color.FromArgb(130,240,0);
                }
                else
                {
                    startButton.BackColor = Color.Red;
                }
                
    
            };

            startButton.MouseLeave += (object sender, EventArgs e) =>
            {
                if (!timerStarted)
                {
                    startButton.BackColor = Color.FromArgb(80, 180, 0);
                }
                else
                {
                    startButton.BackColor = Color.FromArgb(200,0,0);
                }
            };
            #endregion
            #region Refresh Button setup
            //Button hover functionality
            refreshButton.MouseEnter += (object sender, EventArgs e) =>
            {
                refreshButton.BackColor = Color.SkyBlue;
            };

            refreshButton.MouseLeave += (object sender, EventArgs e) =>
            {

                refreshButton.BackColor = Color.SteelBlue; 
            };

            refreshButton.Enabled = false;
            #endregion
            #region Mode button setup
            images.Add(Properties.Resources.disconnected);
            images.Add(Properties.Resources.USB);
            images.Add(Properties.Resources.bluetooth);
            modeButton.Size = new Size(modeButtonHeight, modeButtonHeight);
            modeButton.Location = new Point(765, 505);
            modeButton.Enabled = true;
            #endregion
            #region Download and Upload File Button setup
            //Button hover functionality
            btnDownload.MouseEnter += (object sender, EventArgs e) =>
            {
                btnDownload.BackColor = Color.SkyBlue;
            };

            btnDownload.MouseLeave += (object sender, EventArgs e) =>
            {

                btnDownload.BackColor = Color.SteelBlue;
            };

            btnDownload.Enabled = false;

            btnUpload.MouseEnter += (object sender, EventArgs e) =>
            {
                btnUpload.BackColor = Color.SkyBlue;
            };

            btnUpload.MouseLeave += (object sender, EventArgs e) =>
            {

                btnUpload.BackColor = Color.SteelBlue;
            };

            #endregion

            #region Window min/max handler
            this.Resize += new EventHandler(windowResize);
            #endregion

            #region DrawingArea Class setup
            iconDrawing = new IconDrawing(this)
            {
                Dock = DockStyle.Fill
            };

            this.Controls.Add(iconDrawing);

            #endregion
            #region DeviceScanner Class Setup
            deviceScanner = new DeviceScanner(portCombo);
            #endregion

            #region Visuals arrangement
            iconDrawing.BringToFront();
            portCombo.BringToFront();
            parameterCombo.BringToFront();
            componentCombo.BringToFront();
            startButton.BringToFront();
            modeButton.BringToFront();
            refreshButton.BringToFront();
            logo.BringToFront();
            btnUpload.BringToFront();
            btnDownload.BringToFront();
            #endregion

            #region Get Unit form Parameter dictionary
            parameterToUnit.Add("voltage", " (V)");
            parameterToUnit.Add("current", " (A)");
            parameterToUnit.Add("temperature", " (°C)");
            parameterToUnit.Add("state", " (%)");
            parameterToUnit.Add("cycle count", " (%)");
            #endregion
            #endregion
        }

        #region Button methods
        private void refreshButton_Click(object sender, EventArgs e)
        {
            chartLegendList = new List<string>();
            componentListbox.Items.Clear();
            componentCombo.SelectedItem = null;
            refreshButton.Enabled = false;
            legendTextBox.Text = "";
        }
        private void modeButton_Click(object sender, EventArgs e)
        {
            mode = (mode + 1) % 3; //Toggle the image
            if (mode == 1)
            {
                label3.Text = "Ports";
                modeButton.Size = new Size((int)(0.8*modeButtonHeight), modeButtonHeight);
                modeButton.Location = new Point(765 + (int)(0.1*modeButtonHeight), 505);
            }
            else
            {
                modeButton.Size = new Size(modeButtonHeight, modeButtonHeight);
                modeButton.Location = new Point(765, 505);
            }
            if (mode == 0)
            {
                label3.Text = "Files";
            }
            modeButton.BackgroundImage = images[mode];
        }
        private void startButton_Click(object sender, EventArgs e)
        {
            timerStarted = !timerStarted; //toggle the button / timer status 
            if (timerStarted)
            {
                #region Serial Port setup
                if (portCombo.SelectedItem.ToString().Substring(0, 3) == "COM")
                {
                    serialPort = new SerialPort
                    {
                        PortName = portCombo.SelectedItem.ToString(), // Replace with your port name
                        BaudRate = 115200,   // Replace with your baud rate
                        DataBits = 8,
                        Parity = Parity.None,
                        StopBits = StopBits.One,
                        Handshake = Handshake.None,
                        ReadTimeout = 500, // Adjust as necessary
                        WriteTimeout = 500
                    };
                    try
                    {
                        serialPort.Open();
                        serialPort.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error opening serial port: {ex.Message}");                        
                        timerStarted = false;

                    }
                }
                #endregion
            }
            if (timerStarted)
            {
                

                #region Reset variables
                startTime = 0.0;
                hexData.Items.Clear();
                plotter.Series.Clear();
                plotter.ChartAreas.Clear();
                canDataToWrite = new List<string>();
                #endregion

                int index = 0; //Position in the chartLegendList

                int plotAreaWidth = 100;
                int plotAreaHeight = 95;

                int innerPlotOffset = 12;
                int axisOffset = 7;
                int plotXOffset = 0;

                ElementPosition plotArea = new ElementPosition(plotXOffset, 0, plotAreaWidth, plotAreaHeight);
                foreach (string iAreaName in chartLegendList)
                {
                    #region Chart area setup
                    // Create a new chart area for each component in the Chart Legend
                    var iChartArea = new ChartArea(iAreaName);
                    iChartArea.AxisY.IsStartedFromZero = false;
                    iChartArea.BackColor = Color.Transparent;
                    iChartArea.AxisX.LineColor = Color.Transparent;
                    iChartArea.AxisX.LabelStyle.ForeColor = Color.Transparent;

                    iChartArea.AxisX.MajorGrid.Enabled = false;
                    iChartArea.AxisY.MajorGrid.Enabled = false;
                    iChartArea.AxisX.MajorTickMark.Enabled = false;
                    iChartArea.AxisY.MajorTickMark.Enabled = false;


                    iChartArea.AxisY.LineColor = Color.Transparent;
                    iChartArea.AxisY.LabelStyle.ForeColor = Color.White;
                    iChartArea.AxisX.LabelStyle.Format = "{0:0}";

                    iChartArea.Position = plotArea;
                    iChartArea.InnerPlotPosition = new ElementPosition(plotXOffset+innerPlotOffset + axisOffset, innerPlotOffset, plotAreaWidth - 2*(innerPlotOffset + axisOffset), plotAreaHeight - 2 * innerPlotOffset);

                    plotter.ChartAreas.Add(iChartArea);
                    #endregion
                    #region Graph points setup
                    // Create a new series for each component
                    var iSeries = new Series(iAreaName)
                    {
                        BorderWidth = 2,
                        ChartType = SeriesChartType.StepLine,
                        Color = colours[index],
                        ChartArea = iAreaName,
                    };

                    iSeries.SetCustomProperty("LineTension", "0.55");
                    plotter.Series.Add(iSeries);
                    #endregion
                    #region Aligning Graphs
                    if (index != 0) //Only display the first X-axis (time)
                    {
                        iChartArea.AxisX.LabelStyle.Enabled = false;
                        
                    }
                    iChartArea.AxisY.LabelStyle.Enabled = false;
                    iChartArea.AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                    #endregion
                    #region Axes setup
                    string iAxisAreaName = iAreaName + axisIdentifier;


                    Color axisColor = Color.Black;
                    var iComponentAxis = new ChartArea(iAxisAreaName);
                    iComponentAxis.AxisY.IsStartedFromZero = false;
                    iComponentAxis.BackColor = Color.Transparent;
                    iComponentAxis.AxisY.LineColor = axisColor;
                    iComponentAxis.AxisY.LabelStyle.ForeColor = colours[index];
                    iComponentAxis.AxisX.LineColor = Color.Transparent;
                    iComponentAxis.AxisX.LabelStyle.Enabled = false;
                    iComponentAxis.AxisX.MajorGrid.Enabled = false;
                    iComponentAxis.AxisY.MajorGrid.Enabled = false;
                    iComponentAxis.AxisX.MajorTickMark.Enabled = false;
                    iComponentAxis.AxisY.LabelStyle.Font = new Font(SharedResources.KardustFont, 14);
                    

                    if (index==0)
                    {

                        iComponentAxis.Position = plotArea;
                    }

                    else if (index == 1)
                    {
                        iComponentAxis.Position = plotArea;
                        iComponentAxis.AxisY2.Enabled = AxisEnabled.True;
                        iComponentAxis.AxisY2.IsStartedFromZero = false;
                        iComponentAxis.AxisY2.LabelStyle.Enabled = true;
                        iComponentAxis.AxisY2.LabelStyle.ForeColor = Color.White;
                        iComponentAxis.AxisY2.LineColor = axisColor;
                        iComponentAxis.AxisY2.LabelStyle.ForeColor = colours[index];
                        iComponentAxis.AxisY2.LabelStyle.Font = new Font(SharedResources.KardustFont, 14);
                        iComponentAxis.AxisX2.MajorGrid.Enabled = false;
                        iComponentAxis.AxisY2.MajorGrid.Enabled = false;
                        iComponentAxis.AxisY.LabelStyle.Enabled = false;
                    }
                    else if (index == 2)
                    {
                        iComponentAxis.Position = new ElementPosition(plotXOffset - (plotAreaWidth - 2 * innerPlotOffset), 0, plotAreaWidth, plotAreaHeight);
                        iComponentAxis.AxisY2.Enabled = AxisEnabled.True;
                        iComponentAxis.AxisY2.IsStartedFromZero = false;
                        iComponentAxis.AxisY2.LabelStyle.Enabled = true;
                        iComponentAxis.AxisY2.LabelStyle.ForeColor = Color.White;
                        iComponentAxis.AxisY2.LabelStyle.ForeColor = colours[index];
                        iComponentAxis.AxisY2.LabelStyle.Font = new Font(SharedResources.KardustFont, 14);
                        iComponentAxis.AxisY2.LineColor = axisColor;
                        iComponentAxis.AxisX2.MajorGrid.Enabled = false;
                        iComponentAxis.AxisY2.MajorGrid.Enabled = false;
                        iComponentAxis.AxisY.LabelStyle.Enabled = false;
                    }

                    if (index == 3)
                    {
                        iComponentAxis.Position = new ElementPosition(plotXOffset+plotAreaWidth - 2 * innerPlotOffset, 0, plotAreaWidth, plotAreaHeight);
                        
                        
                    }

                    iComponentAxis.InnerPlotPosition = new ElementPosition(plotXOffset+innerPlotOffset, innerPlotOffset, plotAreaWidth - 2 * innerPlotOffset, plotAreaHeight - 2 * innerPlotOffset);

                    plotter.ChartAreas.Add(iComponentAxis);

                    // Create a new series for each key
                    var iSeriesComponentAxis = new Series(iAxisAreaName)
                    {
                        ChartType = SeriesChartType.StepLine,
                        Color = Color.Transparent, // Change this to the color you want for this series
                        ChartArea = iAxisAreaName,
                    };

                    plotter.Series.Add(iSeriesComponentAxis);
                    #endregion
                    index++;
                }
                #region X axis setup
                var xAxisArea = new ChartArea("X");
                xAxisArea.BackColor = Color.Transparent;
                xAxisArea.AxisX.LineColor = Color.Black;
                xAxisArea.AxisX.LabelStyle.ForeColor = Color.Black;
                xAxisArea.AxisY.LineColor = Color.Transparent;
                xAxisArea.AxisY.LabelStyle.ForeColor = Color.Transparent;

                xAxisArea.AxisX.MajorGrid.Enabled = false;
                xAxisArea.AxisY.MajorGrid.Enabled = false;
                xAxisArea.AxisX.LabelStyle.Format = "{0:0}";
                xAxisArea.AxisX.Title = "Time (s)";
                xAxisArea.Position = plotArea;
                xAxisArea.AxisY.MajorTickMark.Enabled = false;
                xAxisArea.AxisX.LabelStyle.Font = new Font(SharedResources.KardustFont, 8);
                xAxisArea.AxisX.TitleFont = new Font(SharedResources.KardustFont, 12);
                double offsetScaleFactor = 1.7;
                xAxisArea.InnerPlotPosition = new ElementPosition(plotXOffset + innerPlotOffset + axisOffset, innerPlotOffset, plotAreaWidth - 2 * (innerPlotOffset + axisOffset), plotAreaHeight - (int)(offsetScaleFactor * innerPlotOffset));
                plotter.ChartAreas.Add(xAxisArea);

                var xSeries = new Series("X")
                {
                    BorderWidth = 1,
                    Color = Color.Transparent,
                    ChartArea = "X",

                };
                plotter.Series.Add(xSeries);
                #endregion

                #region Start the timer, START => STOP, disable Buttons and Comboboxes
                indexFile = 0;
                startButton.Text = "STOP";
                startButton.BackColor = Color.FromArgb(200, 0, 0);
                timer.Start();
                portCombo.Enabled = false;
                parameterCombo.Enabled = false;
                componentCombo.Enabled = false;
                modeButton.Enabled = false;
                refreshButton.Enabled = false;
                #endregion
            }
            else
            {
                btnDownload.Enabled = true;
                #region Stop the timer,  STOP => START, enable Buttons and Comboboxes, close the seral port
                refreshButton.Enabled = true;
                modeButton.Enabled = true;
                portCombo.Enabled = true;
                parameterCombo.Enabled = true;
                componentCombo.Enabled = true;
                startButton.BackColor = Color.FromArgb(80, 180, 0);
                startButton.Text = "START";
                timer.Stop();

                if (portCombo.SelectedItem.ToString().Substring(0, 3) == "COM")
                {
                    serialPort.Close();
                }
                #endregion
            }
        }
        private void btnDownload_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Text files (*.txt)|*.txt";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        foreach (var item in canDataToWrite)
                        {
                            sw.WriteLine(item.ToString());
                        }
                    }

                    MessageBox.Show("File has been created successfully.");
                }
            }
            btnDownload.Enabled = false;
        }
        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text files (*.txt)|*.txt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string[] fileLines = File.ReadAllLines(ofd.FileName);
                    string uploadFileNameKey = Path.GetFileName(ofd.FileName);
                    if (!uploadedFiles.Keys.Contains(uploadFileNameKey))
                    {
                        uploadedFiles.Add(uploadFileNameKey, fileLines.ToList());
                    }

                    MessageBox.Show("File has been uploaded successfully.");
                }
            }
        }
        #endregion

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timerStarted)
            {
                readAndPlotData(null, null);
            }
        }
        private void componentListbox_DrawItem(object send, DrawItemEventArgs e) //Implement the custom list box format
        {
            customListBox item = componentListbox.Items[e.Index] as customListBox;
            if (item != null)
            {
                e.Graphics.DrawString(
                    item.Message,
                    componentListbox.Font,
                    new SolidBrush(item.ItemColor),
                    0,
                    (int)(e.Index * componentListbox.ItemHeight*1.4)+10
                    );
            }
            else
            {
                //
            }
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Add components to the Chart Legend
            if (componentCombo.SelectedIndex != -1){
                string iKey = (parameterCombo.SelectedItem + "-" + componentCombo.SelectedItem).ToLower();
                if (!chartLegendList.Contains(iKey) && chartLegendList.Count < 4 && decoder.componentToID.TryGetValue((iKey).ToLower(),out var P))
                {
                    string iLegendTitle = componentCombo.SelectedItem.ToString() + parameterToUnit[parameterCombo.SelectedItem.ToString().ToLower()];
                    componentListbox.Items.Add(new customListBox(colours[chartLegendList.Count], iLegendTitle));
                    legendTextBox.SelectionColor = colours[chartLegendList.Count];
                    legendTextBox.AppendText(iLegendTitle + "\t");
                    chartLegendList.Add(iKey);
                    refreshButton.Enabled = true;
                }
            }
            #endregion
            #region Button enable control
            //Only enable the start button once the Paramter, Component and Port have been chosen
            if (portCombo.SelectedIndex != -1 && parameterCombo.SelectedIndex != -1 && componentCombo.SelectedIndex != -1)
            {
                startButton.Enabled = true;
            }
            else
            {
                startButton.Enabled = false;
            }
            #endregion
        }
        private void IDCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the componentCombo items
            componentCombo.Items.Clear();


            // Get the selected ID
            string selectedID = parameterCombo.SelectedItem.ToString();

            // Check if the selected ID has components
            if (decoder.parameterToComponent.TryGetValue(selectedID, out var components))
            {
                // If it does, add the components to componentCombo
                foreach (var component in components)
                {
                    componentCombo.Items.Add(component);
                }
            }
        }
        private void readAndPlotData(object sender, EventArgs e)
        {
            if (componentCombo.SelectedItem != null)
            {
                var line = ""; //initialize the line (of raw data) to process

                #region Read from a text file or from the serial port?
                if (portCombo.SelectedItem.ToString() == "Sample Data")
                {
                    line = sampleDataLines[indexFile];
                }
                else if (uploadedFiles.Keys.Contains(portCombo.SelectedItem.ToString()))
                {
                    line = uploadedFiles[portCombo.SelectedItem.ToString()][indexFile];
                    
                }
                else
                {
                    #region Reading from the Serial Port
                    if (portCombo.SelectedItem.ToString().Substring(0,3) == "COM")
                    {
                        try
                        {
                            // Attempt to read a line from the serial port
                            line = serialPort.ReadLine();
                        }
                        catch
                        {
                            line = nullData;
                        }
                        
                    }
                    #endregion
                }
                #endregion
                if (line.Length == 0)
                {
                    line = nullData;
                }
                if (line[0] == '7')
                {
                    canDataToWrite.Add(line);

                    #region Process each line
                    var splitLine = line.Split(':'); //Split the line up to { HEX DATA, Time(micros) }
                    var decodedData = decoder.ProcessData(splitLine[0]); //Process the data using the decoder object
                    #endregion
                    #region Display floating raw Hex Data
                    if (hexData.Items.Count == 8)
                    {
                        hexData.Items.RemoveAt(hexData.Items.Count - 1);
                    }

                    hexData.Items.Insert(0, "  " + splitLine[0]);
                    #endregion
                    #region Reset X-axis Time
                    if (startTime == 0.0)
                    {
                        startTime = Convert.ToDouble(splitLine[1]) / (double)(1000);
                    }
                    #endregion
                    #region Isolate important / chosen data and plot the data
                    //Iterate through the components and the corresponding components
                    foreach (var (component, result, unit) in decodedData)
                    {
                        foreach (string key in chartLegendList)
                        {
                            if (decoder.componentToID.TryGetValue(key, out var point))
                            {
                                string iID = Convert.ToString(point.Item1);
                                int icomponentIndex = point.Item2;
                                if (decoder.channelID_Components.TryGetValue(iID, out var component_list))
                                {
                                    if (component == component_list[icomponentIndex])
                                    {
                                        //Add data to the graphs and adjust the axes
                                        plotter.Series[key].Points.AddXY((Convert.ToDouble(splitLine[1]) / (double)(1000)) - startTime, result);
                                        plotter.Series[key + axisIdentifier].Points.AddXY((Convert.ToDouble(splitLine[1]) / (double)(1000)) - startTime, result);
                                    }
                                    if (key == chartLegendList[0])
                                    {
                                        plotter.Series["X"].Points.AddXY((Convert.ToDouble(splitLine[1]) / (double)(1000)) - startTime, result);
                                    }
                                    //Update the icons
                                    #region Result Assignment
                                    if (component == "SG_ BmsVoltage ")
                                    {
                                        iconDrawing.voltage = result;
                                    }
                                    if (component == "SG_ BmsCurrent ")
                                    {
                                        iconDrawing.current = result;
                                    }
                                    if (component == "SG_ BmsPackTempMax ")
                                    {
                                        iconDrawing.maxTemperature = result;
                                    }
                                    if (component == "SG_ BmsPackTempMin ")
                                    {
                                        iconDrawing.minTemperature = result;
                                    }
                                    if (component == "SG_ BmsPackTempMin ")
                                    {
                                        iconDrawing.minTemperature = result;
                                    }
                                    if (component == "SG_ BmsFgSoC ")
                                    {
                                        iconDrawing.SoC = result / 100;
                                    }
                                    if (component == "SG_ BmsFgSoH ")
                                    {
                                        iconDrawing.SoH = result / 100;
                                    }
                                    if (component == "SG_ BmsFgCycCount ")
                                    {
                                        cycleCount.Text = Convert.ToString(result);
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    #endregion region
                    #region Draw Icons if any of the important battery parameters have been changed
                    int decimalID = int.Parse(splitLine[0].Substring(0, 3), System.Globalization.NumberStyles.HexNumber);
                    List<int> importantIDs = new List<int> { 1937, 1938, 1939 }; //The CAN channels that contain the components displayed
                    if (importantIDs.Contains(decimalID))
                    {
                        iconDrawing.OnDraw();
                    }
                    #endregion
                    
                }
                #region Increment the index to iterate through a text file
                //Console.WriteLine(uploadedFiles[portCombo.SelectedItem.ToString()].Count);
                if (portCombo.SelectedItem.ToString() == "Sample Data")
                {
                    if (indexFile < sampleDataLines.Count)
                    {
                        indexFile++;
                    }
                }
                if (uploadedFiles.Keys.Contains(portCombo.SelectedItem.ToString()))
                {
                    if (indexFile < uploadedFiles[portCombo.SelectedItem.ToString()].Count)
                    {
                        indexFile++;
                    }
                }

                #endregion
            }
        }

        #region General methods
        private void windowResize(object sender, EventArgs e)
        {
            iconDrawing.ccount = 0;
            iconDrawing.drawAnyway = true;
            #region Visuals arrangement
            iconDrawing.BringToFront();
            portCombo.BringToFront();
            parameterCombo.BringToFront();
            componentCombo.BringToFront();
            startButton.BringToFront();
            modeButton.BringToFront();
            refreshButton.BringToFront();
            logo.BringToFront();
            btnDownload.BringToFront();
            btnUpload.BringToFront();
            #endregion
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            deviceScanner.StopScanning();
        }
        private void LoadCustomFont()
        {
            string fontPath = Path.Combine(Application.StartupPath, "Fonts", "DSEG7Classic-Regular.ttf");
            Console.WriteLine(fontPath);
            if (File.Exists(fontPath))
            {
                byte[] fontData = File.ReadAllBytes(fontPath);
                IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
                Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
                privateFonts.AddMemoryFont(fontPtr, fontData.Length);
                Marshal.FreeCoTaskMem(fontPtr);
            }
            else
            {
                MessageBox.Show("Font file not found: " + fontPath);
            }

        }
        #endregion 
        #region Pop up message control
        private void logo_MouseEnter(object sender, EventArgs e)
        {
            #region Display the pop up message
            howToUse.Text = "1.Plug in your CAN reader to your computer or connect via Bluetooth." +
                "\n2.Choose the correct Serial Port/Bluetooth Device." +
                "\n3.Chose a battery parameter and the component which you want to monitor." +
                "\n4.Chose as many parameters and components as you want" +
                "\n(they will pop up in the components control which acts as a legend for your graph)." +
                "\n5.You should now be able to press the \"START\" button." +
                "\n6.The essential data will be displayed on the side of the graph through the icons." +
                "\n7.Watch the data come in, be processed and displayed." +
                "\nThank you for using this software! - TheHanDS.";
            howToUse.Font = new Font(SharedResources.KardustFont, 11);
            howToUse.ForeColor = Color.Black;
            howToUse.BackColor = Color.Gray;
            howToUse.BringToFront();
            #endregion
        }
        private void logo_MouseLeave(object sender, EventArgs e)
        {
            #region Hide the pop up message
            howToUse.Text = "";
            howToUse.BackColor = Color.Transparent;
            howToUse.SendToBack();
            #endregion
        }
        #endregion
        #region Misc
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }


        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void portCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timeUntilEmptyFull_Click(object sender, EventArgs e)
        {

        }

        private void plotter_Click(object sender, EventArgs e)
        {

        }

        private void componentListbox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void logo_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }
        private void roundedButton4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }


}

public class DebugTextWriter : TextWriter
{
    public override void Write(char[] buffer, int index, int count)
    {
        System.Diagnostics.Debug.Write(new String(buffer, index, count));
    }

    public override void Write(string value)
    {
        System.Diagnostics.Debug.Write(value);
    }

    public override Encoding Encoding
    {
        get { return Encoding.UTF8; }
    }
    
}
public class customListBox
{
    public customListBox(Color c, string m)
    {
        ItemColor = c;
        Message = m;

    }
    public Color ItemColor { get; set; }
    public string Message { get; set; }
}
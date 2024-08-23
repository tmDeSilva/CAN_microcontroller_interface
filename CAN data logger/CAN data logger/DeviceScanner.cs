
using CAN_data_logger;
using System;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;



public class DeviceScanner
{
    private ComboBox portCombo;
    private System.Windows.Forms.Timer scanTimer;
    private int scanInterval = 1000;
    


    public DeviceScanner(ComboBox portCombo)
    {
        this.portCombo = portCombo;
        InitializeScanner();
        
    }

    private void InitializeScanner()
    {
        scanTimer = new System.Windows.Forms.Timer();
        scanTimer.Interval = scanInterval;
        scanTimer.Tick += OnScanTick;
        scanTimer.Start();
    }

    private void OnScanTick(object sender, EventArgs e)
    {
        string[] ports = SerialPort.GetPortNames();
        portCombo.Invoke((MethodInvoker)delegate
        {
            UpdatePortCombo(ports);
        });
    }

    private void UpdatePortCombo(string[] ports)
    {
        string selectedItem = portCombo.SelectedItem as string;
        if (CANdataPlotter.mode == 1)
        {
            portCombo.Items.Clear();
            foreach (var port in ports)
            {
                portCombo.Items.Add(port);
            }
            
        } 
        else
        {
            portCombo.Items.Clear();
            foreach (var key in CANdataPlotter.uploadedFiles.Keys) 
            {
                portCombo.Items.Add(key);
            }
            portCombo.Items.Add("Sample Data");

        }

        if (portCombo.Items.Count > 0)
        {
            if (selectedItem != null && portCombo.Items.Contains(selectedItem))
            {
                portCombo.SelectedItem = selectedItem;
            }
            
        }
    }


    public void StopScanning()
    {
        scanTimer?.Stop();
        scanTimer?.Dispose();
    }
}

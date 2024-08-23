# README #

### Description ###

This branch will give a brief overview of each of the main C# classes used to program the GUI's functionality.

- [`CANDataLoggerPloter.cs`]
	* The main class
		- Reads data from the serial port
		- Plots the data on a graph
		- Controls all the button and drop-down menu functionality
		
- [`CanDecoder.cs`](https://bitbucket.org/nyobolt/can_microcontroller_interface/src/Can-Decoder/)
	- Reads `CANdbcFile.txt` and `EssentialParameters.txt` and creates 5 dictionaries
	- `ProcessData(string pData)` processes the raw hexadecimal CAN data and returns a list of all the channel components and their decimal values
	
- [`DrawingArea.cs`]
	- Responsible for drawing the SoC and SoH icons (along side the green and red progress bars)
	- Draws two circles
		- The First displays BMS Voltage and Current
		- The second displays the maximum BMS temperature and the BMS power
	- Draws the Forseti Pack image
	
- [`DeviceScanner.cs`]
	- If the mode button is on serial port mode the device scanner scans device
	- Updates the `portCombo` drop-down menu
	

[Go back to the Main repository](https://bitbucket.org/nyobolt/can_microcontroller_interface/src/main/)

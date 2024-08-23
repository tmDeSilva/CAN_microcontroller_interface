# README #

### What is this repository for? ###

* This repository contains the software that:
	* Programs the microcontroller so that it can read CAN messages from the MCP2515 CAN Module
	* Reads data sent from the microcontroller, processes it and diplays it onto a GUI

### How do I get set up? ###

* To find the hardware components and microcontroller program click [here](https://bitbucket.org/nyobolt/can_microcontroller_interface/src/CAN-Reader-Arduino-Code/)
	* (Includes an in-depth explanation of the connections and program)
	
* By installing the `CAN data logger` folder run `CAN data logger\CAN data logger\bin\CAN data logger.exe` or run the solution instead
	* To find a break down of the program, [here](https://bitbucket.org/nyobolt/can_microcontroller_interface/src/GUI/) are the main C# classes
		- `CANDataPlotterLogger.cs`
		- `DrawingArea.cs`
		- `CanDecoder.cs`
		- `DeviceScanner.cs`


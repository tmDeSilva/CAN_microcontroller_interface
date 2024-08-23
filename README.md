# README #

## Description
```cpp
```
* This branch contains the microcontroller code and will:
	* Describe the hardware used and the required connections
	* Provide an overview of all the code's functions and libraries
	* Explain the code can be modified to choose the CAN data channels the MCU outputs (e.g. 7B3)

### *Hardware Setup*

* Components
	* Arduino Nano (MCU)
	* MCP2515 CAN module
	* SSD1306 128x32 OLED
	* HC-05 Bluetooth Module
	* FC684216 Female CAN connector
	
* Connections
	* MCU <==> MCP2515
		* D10 => CS; D11 => SI, D12 <= SO, D13 => SCK, +5V => VCC, **GND**
	* MCU <==> SSD1306 OLED
		* A4 <=> SDA; A5 => SCL, +5V => VCC, **GND**
	* MCU <==> HC-05
		* D2 (Software RX) <= TX
		* D3 (Software TX) => RX
			* Voltage divider is required because HC-05 operates at a logic level of 3.3V
			* Use 1K and 2K resistors: D3 => 1K => RX; RX => 2K => GND
		* +5V => VCC, **GND**
	* BMS CAN Bus ==> FC684216 ==> MCP2515
		* CAN P => Pins 1 or 2 => CAN high
		* CAN N => Pins 3 or 4 => CAN low
		* *Note that these pins are not fixed and can be chosen when soldering wires to the FC684216 CAN connector*

### *Embedded code*

* Libraries Used
	
	- [`<mcp2515.h>`](https://github.com/autowp/arduino-mcp2515)
		* This library allows the microcontroller to:
			* Establish SPI communication with the MCP2515 CAN module
			* Read CAN messages
	- [`<SSD1306Ascii.h>`](https://github.com/greiman/SSD1306Ascii)
		* This is a simpler version of [`<Adafruit_SSD1306.h>`](https://github.com/adafruit/Adafruit_SSD1306) specifically for displaying text
		* By using this library the MCU's flash memory usage is significantly
	- `<SPI.h>` 
		* For communication between the MCU and the MCP2515 CAN module
	- `<stdlib.h>`
		* For converting raw hexadecimal CAN data to decimal numbers for displaying specific battery parameters
	- `<Wire.h>`
		* Mainly for I2C communication with the SSD1306 OLED
	- `<SoftwareSerial.h>`
		* For UART communication with the HC-05 module to transmit CAN data via bluetooth
		
* Code Specific functions
	- `void setup()`
		* Serial (and software serial) initialization (set baudrate : `Serial.begin(115200)`)
		* MCP2515 SPI initialization (set bitrate with `mcp2515.setBitrate(CAN_500KBPS, MCP_8MHZ)`)
		* SSD1306 I2C initialiation 
		
	- `void loop()`
		* Checks if a CAN Message has been received
			* Read the CAN data ID (e.g. 7B3) and check if it is part of the `Important IDs` list
			* If the data in the CAN message (for each ID) has been changed:
				* Replace the data already stored in a 2D array (called "arr") with the new data
				* Write the new data to the serial port in the format `XXX X XX XX XX XX XX XX XX XX : time`
					* Each `X` represents a nibble (a hex character)
					* `XXX` is the CAN data ID, the next `X` is the number of bytes of data (usually 8) and the next 8 pairs of nibbles (bytes) are the actual data
					* `time` is measured in milliseconds
			* Every "period" number of CAN messsages read process and display the most recent CAN data from OLED parameter IDs
				* `1937 (Pack Voltage, Pack Current), 1938 (Temperature), 1939 (SoC, SoH)`
				
	- `String processData(String pMsg)`
		* This is a C++ version of the "decoder.cs" code that is part of the GUI
		* Converts the raw hexadecimal CAN data to a binary string to be processed by the {parameter}ValCalc functions
		
	- `void displayData(double pVoltage, double pCurrent, double pTemperature , int pSoC)`
		* Displays the reading for every selected parameter
		* Uses `display.home()` rather than `display.clear()` to eliminate the "flickering" affect caused by fully clearing the OLED
		
	- `double {parameter}ValCalc(double p{parameter}, String pBinaryString)`
		* Selects the specific section of the binary string to convert to decimal
		* Calculates the actual result using the formula `(scale_factor * decimal_value + offset)`
		* These values are found in the DBC file and are hard coded at the moment (and commented above each function)
		
	- `int findIndex(...), String reverseString(...)`
		* Additional functions created because C++ does not have standard equivalents
		
### *Modifications*
* The user can change important IDs so that the data written to the serial port is that which is useful to them
* The user can change OLED parameters (however this process may require renaming and adding functions which should be automated in the future)

[Go back to the Main repository](https://bitbucket.org/nyobolt/can_microcontroller_interface/src/main/)

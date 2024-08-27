# README #

## Description ##
This branch will provide an in-depth explanation to the `CanDecoder.cs` class.
For the original python version of this program - click [here](https://github.com/tmDeSilva/CAN_microcontroller_interface/tree/Can-Decoder-.py).

### *DBC*
* All the `BO_` Messages are highlighted form the DBC file and written to the `CANdbcFile.txt` file separated by a `*` character
* The text is then read from `CANdbcFile.txt` to create three dictionaries required to decode the raw CAN data

* `IDs` set contains all the decimal IDs
* `ChannelID_Components` dictionary provides the list of components that fall under each channel ID (e.g. `(Key: "1937", Value: [....,Bms_Voltage, Bms_Current,....])`)
* `ChannelID_channelName` dictionary allows conversion of decimal ID to the name of the CAN message (e.g. `(Key : "1937", Value: "Bms_Msg02")`)
* `ChannelID_decodeData` dictionary contains all the data needed to convert the raw hexadecimal CAN data to the actual decimal value
	* The key is a channel ID (e.g. `"1937"`) and the value is a tuple: `(int startingBit, int bitLength, float scaleFactor, float offset, string unit)`
	* Each component line has a general format:
		* `SG_ {component} : {startingBit}|{bitLength}@1+ ({scaleFactor},{offset}) [{minValue}|{maxValue}] "{unit}"  _{ignore}`
		
#### Functions
* `DisplayIDs` and `DisplayIDComponents` can be used to iterate throught the `IDs` set and the `channelID_Components` dictionary (respectively) for debugging purposes
* `ProcessData(string pData)`
	* CAN data is in the format `XXX X XX XX XX XX XX XX XX XX` where each X represents a nibble of data (a hexadecimal character)
		* Take the first three characters and convert it to a decimal number to find the decimal channel (e.g. `791 => 1937`) 
		* Take the last 8 bytes (`XX`) and create the `pBinary` string
			* The most significant bit (MSB) of the byte is the last bit so each byte needs to be converted to binary and reversed (e.g. AC => 1010 110**0** => **0**011 0101)
			* Each byte is then added to the `pBinary` string
			* Then by using the `ChannelID_decodeData` dictionary, the tuple required to process the data can be obtained:
				* Select the part of the `pBinary` string by slicing based on the `startingBit` and `bitLength`
				* As the MSB rule still applies, the sliced string has to be reversed once more before it can be converted into a decimal value
				* The final result is then calculated by `scaleFactor * decimalValue + offset`
				* A final tuple `(component, result, unit)` is created and appended to the `res` list which is the final output of this function

### *Esential Parameters*
Reads from `EssentialParameters.txt` to create the parameter and component drop-down menus in the GUI

* `parameterToComponent` dictionary allows the user to choose a battery parameter (such as voltage) that they want to monitor and the status of each selected component (e.g. Pack or Cell 15)
* `componentToID` dictionary finds the DBC file channel ID and component index that hold the information needed to find the status of the selected component based on the selected battery parameter
	* (e.g. `(Key : "VoltageBMS", Value : (1937,37))`
	
#### Processing the `EssentialParameters.txt` file
* This text file can be altered so that the user can view their desired parameters and components
* This text file has a defined format that needs to be followed in order for the `CanDecoder.cs` class to process it properly
* Here is a simplified version to demonstrate the format:

#### Example
```txt

Voltage{
	-BMS:(1937,37)
	-Cell{n}r[1,16]:(1972 + (n-1)//4, 3 - (n-1)%4)
}
Temperature{
	-Cell Pair{n}l[11,13,15,5,3]:(1976,i)
}

```
* When writing a line in the file:
	* The first line is the parameter name and open curly brackets
	* The next lines correspond the component names to the `(ID, componentIndex)` tuple
		* These lines must be indented with a dash character
		* Make sure the component title and tuple are separated by a colon (without spaces)
		* By using the `r` or `l` decorators, creating multiple components becomes a lot easier
#### `r`(range) decorator
* The format is `{n}r[{start}:{end}]`
	* `n` ranges from `start` to `end` and the curly brackets mean that it will act as text 
		* (e.g. in the above example Cell1 to Cell16 will be the component names)
	* `n` can be then used in a formula that makes it easier for the user to designate `(ID, componentIndex)` tuples to the components
	* `-Cell{n}r[1,16]:(1972 + (n-1)//4, 3 - (n-1)%4)` effectively acts as 16 lines in 1:

| Component | ID | component index |
|-|-|-|
| **Cell{n}** | **1972 + (n-1)//4 **|  ** 3 - (n-1)%4 ** |
| *Cell1* | *1972* | *3* |
| Cell2 | 1972 | 2 |
| Cell3 | 1972 | 1 |
| Cell4 | 1972 | 0 |
| *Cell5* | *1973* | *3* |
|...|...|...|


#### `l` (list) decorator
* The format is `{n}l[element1,....element?]`
	* A list `l = [element1,....element?]` is created where `n` represents the actual item and the variable `i` represents the index of that item so in the formula you can use both of these variables
* This becomes useful when dealing with unordered components that are part of the same channel ID
	
| Component | ID | component index |
|-|-|-|
| **Cell Pair{n}** | **1976 **|  **i **|	
|11|1976|0|
|13|1976|1|
|15|1976|2|
|5|1976|3|
|3|1976|4|
	

[Go back to the Main repository](https://github.com/tmDeSilva/CAN_microcontroller_interface/tree/main)

[Go back to the GUI repository](https://github.com/tmDeSilva/CAN_microcontroller_interface/tree/GUI)

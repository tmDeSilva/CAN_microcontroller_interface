//General Libraries
#include <SPI.h>
#include <stdlib.h>
#include <Wire.h>

//CAN data library
#include <mcp2515.h>

//Software serial library for the bluetooth module
//#include <SoftwareSerial.h>

//OLED libraries
#include <SSD1306Ascii.h>
#include <SSD1306AsciiAvrI2c.h>

//OLED constants
#define I2C_ADDRESS 0x3C
#define RST_PIN -1
SSD1306AsciiAvrI2c display;

struct can_frame canMsg;
MCP2515 mcp2515(SS); //Initialize the chip select to the Nano Default SS

//SoftwareSerial bluetoothSerial(2,3); //Initialize the virtual serial port

//Only bother writing data with these IDs
const int importantIDs[] = {1937,1938,1939,1945,1972,1973,1974,1975,1976,1977,1979}; 
const int numberOfIDs = sizeof(importantIDs) / sizeof(importantIDs[0]);
//int numberOfIDs = 11;
//CAN message variables init
char arr[12][35]; // Reduced the size of the array
char nullData[] = "000 0 00 00 00 00 00 00 00 00";

void setup() {
  for (int i = 0; i < numberOfIDs; i++){
    strcpy(arr[i], nullData);
  }
  //SPI and Serial init
  Serial.begin(115200);
  //bluetoothSerial.begin(115200);
  SPI.begin();

  //MCP2515 init
  mcp2515.reset();
  if (MCP2515::ERROR_OK != mcp2515.setBitrate(CAN_500KBPS, MCP_8MHZ)){
    Serial.println("MCP2515 initialization failed");
  }
  mcp2515.setNormalMode();

  //OLED init
#if RST_PIN >= 0
  display.begin(&Adafruit128x32, I2C_ADDRESS, RST_PIN);
#else // RST_PIN >= 0
  display.begin(&Adafruit128x32, I2C_ADDRESS);
#endif // RST_PIN >= 0

  display.setFont(Adafruit5x7);
  display.clear();
}


char msg[32];
long ID = 0;
unsigned long t1 = millis();

//OLED parameters init
double voltage = 0.0;
double current = 0.0;
double temperature = 0.0;
int SoC = 0;
char currentUnit[3] = "A"; // Changed from String to char array
String prefix = "";
String binaryString = "";
int count = 0;
int period = 50;

void loop() {

  if (mcp2515.readMessage(&canMsg) == MCP2515::ERROR_OK) {
    snprintf(msg, sizeof(msg), "%03X %01X ", canMsg.can_id, canMsg.can_dlc);
    ID = strtol(msg, NULL, 16);
    int IDPos = findIndex(importantIDs, numberOfIDs, ID);
    if (IDPos != -1){
      for (int i = 0; i < canMsg.can_dlc; i++) {
        if (i < 8){
          snprintf(msg + strlen(msg), sizeof(msg) - strlen(msg), "%02X ", canMsg.data[i]);
        }
      }

      //Selection ensures that data is not printed if a change hasn't been made
      if (strcmp(arr[IDPos], msg) != 0) {
        strcpy(arr[IDPos], msg);
        //Print XXX X XX XX XX XX XX XX XX XX : time 
        //  - where each 'X' represents a nibble of data and time is measured in µs
        if (msg[0] == '7'){
          Serial.print(msg);
          Serial.print(":");
          Serial.println(millis() - t1);

          /*
          //Sending data through the bluetooth module
          bluetoothSerial.write(msg);
          bluetoothSerial.write(":");
          bluetoothSerial.println(micros() - t1);
          */
        }
      }

      if (count % period == 0){
        //Diplay import battery parameters to the OLED

        

        prefix = String(arr[0]).substring(0, 3);
        if (prefix != "000"){
          binaryString = processData(arr[0]);
          voltage = voltageValCalc(voltage, binaryString);
          current = currentValCalc(current, binaryString);
          //Decide whether to display current in A or mA for readability
          if (current < 1){
            current = current * 1000;
            strcpy(currentUnit, "mA"); // Changed from String to char array

          }
          else{
            strcpy(currentUnit, "A "); // Changed from String to char array
          }

        }

        prefix = String(arr[1]).substring(0, 3);
        if (prefix != "000"){
          binaryString = processData(arr[1]);
          temperature = tempValCalc(temperature, binaryString);
        }

        prefix = String(arr[2]).substring(0, 3);
        if (prefix != "000"){
          binaryString = processData(arr[2]);
          SoC = SoCValCalc(SoC, binaryString);
        }
        
        displayData(voltage, current, temperature, SoC);
        count = 1;
      }
      else{
        count ++;
      }

    }

  } else {
    //If a message is not received from CAN, do nothing
    delay(10);
  }
  delay(5);
}


String processData(String pMsg){
  String pData = pMsg.substring(6);  // Get substring starting from the 6th character

  String parts[10]; 
  int partIndex = 0;
  int startIndex = 0;
  int endIndex = pData.indexOf(' ');

  while (endIndex != -1) {
      parts[partIndex++] = pData.substring(startIndex, endIndex);
      startIndex = endIndex + 1;
      endIndex = pData.indexOf(' ', startIndex);
  }
  parts[partIndex] = pData.substring(startIndex); 

  String pBinaryString = "";
  for (int i = 0; i <= partIndex; i++) {
      int iByte = strtol(parts[i].c_str(), NULL, 16);
      String temp = String(iByte, BIN); 
      temp = String(temp.length() < 8 ? "00000000" + temp : temp);
      temp = temp.substring(temp.length() - 8); 
      temp = reverseString(temp);
      pBinaryString += temp;
  }

  return pBinaryString;
}

String reverseString(String str) {
    String reversed = "";
    for (int i = str.length() - 1; i >= 0; i--) {
        reversed += str[i];
    }
    return reversed;
}

void displayData(double pVoltage, double pCurrent, double pTemperature , int pSoC){
  display.home();
  char bufferVoltage[20]; 
  dtostrf(pVoltage, 5, 2, bufferVoltage);
  display.print("Voltage: ");
  display.print(bufferVoltage);
  display.println("V");

  char bufferCurrent[20]; 
  dtostrf(pCurrent, 5, 2, bufferCurrent);
  display.print("Current: ");
  display.print(bufferCurrent);
  display.println(currentUnit);

  char bufferTemp[20]; 
  dtostrf(pTemperature, 5, 2, bufferTemp);
  display.print("Temp: ");
  display.print(bufferTemp);
  display.println("C");
  
  char bufferSoC[20]; 
  dtostrf(pSoC, 3, 0, bufferSoC);
  display.print("SoC: ");
  display.print(bufferSoC);
  display.println("%");
}

//Each calculation function has the import DBC file data commented above it


//  BmsVoltage 0|16 (0.001,0) V

double voltageValCalc(double pVoltage, String pBinaryString){
  pVoltage = strtol(reverseString(pBinaryString.substring(0,16)).c_str() , NULL , 2) * 0.001;
  return pVoltage;
}
//1937 = 0x791
//  BmsCurrent 16|20 (0.001,-500) A
double currentValCalc(double pCurrent, String pBinaryString){
  pCurrent = strtol(reverseString(pBinaryString.substring(16,36)).c_str() , NULL , 2) * 0.001 - 500.0;
  return pCurrent;
}
//1938 = 0x792
//  BmsPackTempMin 32|12 (0.1,-40) °C
//  BmsPackTempMax 44|12 (0.1,-40) °C
double tempValCalc(double pTemperature, String pBinaryString){
  pTemperature = strtol(reverseString(pBinaryString.substring(44,56)).c_str() , NULL , 2) * 0.1 - 40.0;
  return pTemperature;
}
//1939 = 0x793
//  BmsFgSoC 0|8 (1,0) %
//  BmsFgSoH 8|8 (1,0) %
int SoCValCalc(double pSoC, String pBinaryString){
  pSoC = strtol(reverseString(pBinaryString.substring(0,8)).c_str() , NULL , 2);
  return pSoC;
}

int findIndex(int pArr[], int size, int target) {
  for (int i = 0; i < size; i++) {
    if (pArr[i] == target) {
      return i;  // return the index if the target is found
    }
  }
  return -1;  // return -1 if the target is not found
}

#################
#Library imports#
#################
import matplotlib.pyplot as plt
import serial
import dbcFileDecoder as dbc
###########
#Constants#
###########
#Serial port requiremnts
PORT = "COM4"
BAUD = 115200
MODE = ""
# Set up the serial connection (adjust 'COM3' and baudrate as needed)
try:
    ser = serial.Serial(PORT, BAUD , timeout=1)
except:
    MODE = "text_file_read"

#Dictionaries are named in the format key_value
channelID_Components = dbc.channelID_Components
channelID_channelName = dbc.channelID_channelName 
channelID_decodeData = dbc.channelID_decodeData

def filler(length,string,char = "0"): #Used to fill the gaps so the byte of data is fully represented (E.g. 10010 should be 00010010 so that it is a full byte)
    return char * (length - len(string)) + string

def process_data(pData): #Process the data (using the dictionaries previously created)

    pID = str(int(pData[0:3],16)) #Convert the line of data's first 3 hex characters into a decimal ID that is the key of all the dictionaries
    pBinary = "".join([filler(8,bin(int(i,16))[2:])[::-1] for i in pData[6:].split()]) #Convert the hexidecimal into an appropriate binary string that reads from right to left rather than left to right


    res = []

    for iComponent in channelID_Components[pID]:
        (iBrS, iBrL, iSf, iOf, iUnit) = channelID_decodeData[iComponent]
        iRes = int(pBinary[iBrS: iBrS + iBrL][::-1],2) * iSf + iOf
        res.append((iComponent,iRes,iUnit)) #Creates the appropriate tuple of th (component name, real decimal result, unit of the result)
    
    return res

IDs = dbc.IDs
dbc.display_IDs()

time = []
CANData = []
specificChannelID = input("Enter ID: ")
yAxisLabel = ""

dbc.display_ID_components(specificChannelID)
components = channelID_Components[specificChannelID]
component = int(input("Enter component: "))

def is_ID(pLine, pID): #Checks if the line contains the required ID
    return int(pLine[0:3],16) == pID

if MODE == "text_file_read":
    for line in open("sampleData.txt","r").readlines():
        t = int(line.strip().split(":")[1])
        line = line.split(":")[0].strip()

        if str(int(line[0:3],16)) == specificChannelID:
            res = process_data(line)
            for i in range(len(components)):
                if i == component:
                    CANData.append(res[i][1])
                    time.append(t)
                    yAxisLabel = res[i][2]

    plt.step(time,CANData)
    plt.ylabel(yAxisLabel)
    plt.show()

else:
    plt.ion()
    cycles = 10000
    for _ in range(cycles): #main loop should be controlled by a start and stop button which decides when to start and exit the loop
        channelData_byID_line = str(ser.readline().strip())[2:-1]

        try:
            t = int(channelData_byID_line.strip().split(":")[1])
            channelData_byID_line = channelData_byID_line.split(":")[0].strip()

            if str(int(channelData_byID_line[0:3],16)) == specificChannelID:
                processedData = process_data(channelData_byID_line)
                for i in range(len(components)):
                    if i == component:
                        CANData.append(processedData[i][1])
                        time.append(t)
                        yAxisLabel = processedData[i][2]

                    ########################
                    #Live plotting commands#
                    ########################
                    plt.clf()  # Clear the plot
                    plt.step(time,CANData)  # Plot x against y
                    plt.draw()  # Draw the plot
                    plt.pause(0.1)
        except:
            pass


    plt.ioff() 
    plt.show()

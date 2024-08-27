##########################################################
#Read from the dbc file and create a list to be processed#
##########################################################
MODE = "r"
dbcChannelData = open("CANdbcFile.txt", MODE).read().split("*\n") #Open the file and split it into sections (each representing channel data) by the "*" character

##########################################################################################################
#Process the dbc file to create the dictionaries which hold all the necessary data concering the dbc file#
##########################################################################################################

#Dictionaries are named in the format key_value
channelID_Components = {}
channelID_channelName = {}
channelID_decodeData = {}

#Iterate through the partially processed file to create the dictionaries
#i(Name) is a format used for temporary looping variables and will be commonly used throughout this file
for channelData_byID in dbcChannelData:
    channelData_byID_line = channelData_byID.split("\n")

    specificChannelID = channelData_byID_line[0][4:8] #Extracts the ID
    channelID_channelName[specificChannelID] = channelData_byID_line[0][9:] #Extracts the channel name and saves it as a value in the dictionary

    channelData_byID_line_withoutHeaderFooter = channelData_byID_line[1:-1]
    channelID_Components[specificChannelID] = [iComponentData.split(":")[0][1:-1] for iComponentData in channelData_byID_line_withoutHeaderFooter]#Extracts the name of each component from the remaining data which is not the "BO_ xxx" header
    
    for iComponentData in channelData_byID_line_withoutHeaderFooter:
        componentName = iComponentData.split(":")[0][1:-1]
        rawDecodeData_fromDbcFile = iComponentData.split(":")[1].split()
        bit_range = [int(iNumber) for iNumber in rawDecodeData_fromDbcFile[0][:-3].split("|")] #[Starting bit, Length of data]
   
        formulaTuple = [float(iNumber) for iNumber in rawDecodeData_fromDbcFile[1][1:-1].split(",")]
        sf = formulaTuple[0] #Scale Factor
        of = formulaTuple[1] #Offset
        #Formula for raw data to real result is bin(val) * sf + of where val is the bits within the bit range
        
        unit = rawDecodeData_fromDbcFile[3][1:-1].strip("Â")

        channelID_decodeData[componentName] = (bit_range[0], bit_range[1],sf , of, unit)

IDs = set()
for channelData_byID_line in open("sampleData.txt",MODE).readlines():
    IDs |= {int(channelData_byID_line[0:3],16)}

def display_IDs():
    for iID in IDs:

        try:
            print(iID,channelID_channelName[str(iID)]) #This is the output telling the user all the channel options and hould be converted into a drop down menu feature in the GUI

        except:#If the ID is not a real ID ignore that ID
            pass


def display_ID_components(pSpecificChannelID):
    components = channelID_Components[pSpecificChannelID]
    for i in range(len(components)): #Iterate through the components and provide the position of each component in the list it is found in
        print(i,components[i]) #(position, component name)


import sys
import numpy as np
import matplotlib.pyplot as plt
import matplotlib
matplotlib.use('Qt5Agg')
import serial
import serial.tools.list_ports
import dbcFileDecoder as dbc
from PyQt5.QtWidgets import QApplication, QMainWindow, QPushButton, QVBoxLayout, QWidget, QLabel, QComboBox, QListWidget, QListWidgetItem, QDialog, QHBoxLayout, QLabel, QToolButton, QAction
from PyQt5.QtGui import QPixmap, QIcon, QTransform
from PyQt5.QtCore import Qt, QThread, pyqtSignal, QTimer, QPropertyAnimation, QVariantAnimation
from PyQt5.QtGui import QFont
from matplotlib.backends.backend_qt5agg import FigureCanvasQTAgg as FigureCanvas
import time

BAUD = 115200

def filler(length, string, char="0"):
    return char * (length - len(string)) + string

def process_data(pData):
    pID = str(int(pData[0:3], 16))
    pBinary = "".join([filler(8, bin(int(i, 16))[2:])[::-1] for i in pData[6:].split()])
    res = []
    for iComponent in dbc.channelID_Components[pID]:
        (iBrS, iBrL, iSf, iOf, iUnit) = dbc.channelID_decodeData[iComponent]
        iRes = int(pBinary[iBrS: iBrS + iBrL][::-1], 2) * iSf + iOf
        res.append((iComponent, iRes, iUnit))
    return res

class SerialThread(QThread):
    data_received = pyqtSignal(str)

    def __init__(self, port, baud):
        super().__init__()
        self.port = port
        self.baud = baud
        self.is_running = True

    def run(self):
        self.serial_port = serial.Serial(self.port, self.baud, timeout=1)
        while self.is_running:
            line =str(self.serial_port.readline().strip())[2:-1]
            if line:
                self.data_received.emit(line)

    def stop(self):
        self.is_running = False
        time.sleep(0.1)  # Add a small delay
        if self.serial_port.is_open:  # Check if the serial port is open before closing it
            self.serial_port.close()

class CANDataPlotter(QMainWindow):
    def __init__(self):
        super().__init__()
        self.initUI()
        self.serial_thread = None
        self.is_running = False
        self.twin_axes = []
        self.port = "COM4"  # Default port

    def initUI(self):
        self.setWindowTitle('CAN Data Plotter')
        self.setGeometry(100, 100, 800, 600)  # Set window size
        self.setStyleSheet("""
            QWidget {
                background-color: #161616;
                font-family: Arial;
                
            }
            QLabel, QComboBox, QListWidget {
                color: #AAA;
                font-size: 20px;
            }
            QComboBox QAbstractItemView {
                color: #FFF;
                background-color: #161616; 
            }

            QPushButton {
                background-color: #555;
                color: #AAA;
                padding: 5px;
                border-radius: 3px;
            }
            QPushButton:hover {
                background-color: #777;
            }
        """)

        # Load the logo from the file
        logo_pixmap = QPixmap('nyobolt_logo.png')

        # Create a QLabel to hold the logo
        logo_label = QLabel(self)
        logo_label.setPixmap(logo_pixmap)

        self.start_button = QPushButton('Start', self)
        self.start_button.setStyleSheet("""
            QPushButton {
                background-color: green;
                border-style: outset;
                border-width: 2px;
                border-radius: 10px;
                border-color: beige;
                font: bold 20px;
                
                min-width: 10em;
                padding: 6px;
            }
            QPushButton:pressed {
                background-color: rgb(0, 100, 0);
                border-style: inset;
            }
        """)
 
        self.start_button.clicked.connect(self.start_plotting)

        self.stop_button = QPushButton('Stop', self)
        self.stop_button.setStyleSheet("""
            QPushButton {
                background-color: red;
                border-style: outset;
                border-width: 2px;
                border-radius: 10px;
                border-color: beige;
                font: bold 20px;
                min-width: 10em;
                padding: 6px;
            }
            QPushButton:pressed {
                background-color: rgb(100, 0, 0);
                border-style: inset;
            }
        """)

        self.stop_button.clicked.connect(self.stop_plotting)

        self.id_label = QLabel('Select ID:', self)
        self.id_input = QComboBox(self)
        self.id_input.addItems([f"{dbc.channelID_channelName[key]} ({key})" for key in sorted(dbc.channelID_Components.keys())])

        self.component_label = QLabel('Select Components:', self)
        self.component_input = QListWidget(self)
        self.component_input.setSelectionMode(QListWidget.MultiSelection)

        

        # Create a refresh button with an icon
        menu_layout = QVBoxLayout()
        self.refresh_icon = QLabel(self)
        self.refresh_pixmap = QPixmap('refresh_icon.png').scaled(30, 30, Qt.KeepAspectRatio) # Replace with the path to your refresh icon
        self.refresh_icon.setPixmap(self.refresh_pixmap)

        # Create a QLabel and QComboBox for the COM ports
        self.port_label = QLabel('Select COM Port:', self)
        self.port_input = QComboBox(self)
        self.update_ports()

        # Add the QLabel to the menu layout
        menu_layout.addWidget(self.refresh_icon)

        # Connect the QLabel's clicked signal to the refresh_ports method and the animation
        self.refresh_icon.mousePressEvent = self.refresh_ports

        # Add the QLabel, QToolButton, and QComboBox to the menu layout
        menu_layout = QVBoxLayout()
        menu_layout.addWidget(self.port_label)
        menu_layout.addWidget(self.port_input)

        self.figure, self.ax = plt.subplots()
        self.ax.set_facecolor('#161616')
        self.figure.patch.set_facecolor('#161616')  # Make the plot canvas the same color as the background
        self.canvas = FigureCanvas(self.figure)
        self.canvas.setStyleSheet("background-color:#161616;")  # Make the plot canvas the same color as the background

        self.timer = QTimer()
        self.timer.timeout.connect(self.plot_data)

        # Create a layout for the logo and buttons
        logo_button_layout = QVBoxLayout()
        logo_button_layout.addWidget(logo_label)
        logo_button_layout.addWidget(self.start_button)
        logo_button_layout.addWidget(self.stop_button)
   
        # Create a layout for the dropdown menus
        menu_layout = QVBoxLayout()
        menu_layout.addWidget(self.id_label)
        menu_layout.addWidget(self.id_input)
        menu_layout.addWidget(self.component_label)
        menu_layout.addWidget(self.component_input)
        menu_layout.addWidget(self.refresh_icon)
        menu_layout.addWidget(self.port_label)
        menu_layout.addWidget(self.port_input)

        # Create a main layout and add the logo/button layout and menu layout
        main_layout = QHBoxLayout()
        main_layout.addLayout(menu_layout, 3)
        main_layout.addLayout(logo_button_layout, 1)

        # Create a vertical layout for the main layout and canvas
        v_layout = QVBoxLayout()
        v_layout.addLayout(main_layout)
        v_layout.addWidget(self.canvas)

        container = QWidget()
        container.setLayout(v_layout)
        self.setCentralWidget(container)

        self.id_input.currentIndexChanged.connect(self.update_components)
        self.port_input.currentIndexChanged.connect(self.update_port)

    def update_components(self):
        self.component_input.clear()
        selected_id = self.id_input.currentText().split('(')[-1].strip(')')
        for component in dbc.channelID_Components[selected_id]:
            item = QListWidgetItem(component)
            self.component_input.addItem(item)

    def update_ports(self):
        # Get a list of available COM ports
        ports = serial.tools.list_ports.comports()
        port_names = [port.device for port in ports]

        # Clear the QComboBox and add the available COM ports
        self.port_input.clear()
        if port_names:
            self.port_input.addItems(port_names)
        else:
            self.port_input.addItem("No serial port available")

    def refresh_ports(self, event):
        # Start the rotation
        transform = QTransform()
        transform = transform.rotate(180)  # Set the rotation angle
        rotated_pixmap = self.refresh_pixmap.transformed(transform, Qt.SmoothTransformation)
        self.refresh_icon.setPixmap(rotated_pixmap)
 
        # Update the list of COM ports
        self.update_ports()

        # Reset the icon after a delay
        QTimer.singleShot(500, self.reset_icon)  # Set the duration of the rotation (in milliseconds)

    def reset_icon(self):
        # Reset the icon to its original state
        self.refresh_icon.setPixmap(self.refresh_pixmap)

    def update_port(self):
        # Update the port when a new one is selected
        self.port = self.port_input.currentText()

    def start_plotting(self):
        self.is_running = True
        self.specificChannelID = self.id_input.currentText().split('(')[-1].strip(')')
        self.components = [item.text() for item in self.component_input.selectedItems()]

        self.time = []
        self.CANData = {component: [] for component in self.components}
        self.yAxisLabels = {component: "" for component in self.components}

        dbc.display_ID_components(self.specificChannelID)
        self.component_names = dbc.channelID_Components[self.specificChannelID]

        try:
            serial.Serial(self.port, BAUD, timeout=1)  # Use the selected port
            self.serial_thread = SerialThread(self.port, BAUD)  # Use the selected port
            self.serial_thread.data_received.connect(self.plot_from_serial)
            self.serial_thread.start()
        except:
            self.plot_from_file()

    def stop_plotting(self):
        self.is_running = False
        if self.serial_thread:
            self.serial_thread.stop()
            self.serial_thread.wait()

    def plot_from_file(self):
        for line in open("sampleData.txt", "r").readlines():
            if not self.is_running:
                break
            t = int(line.strip().split(":")[1])
            line = line.split(":")[0].strip()

            if str(int(line[0:3], 16)) == self.specificChannelID:
                res = process_data(line)
                for component in self.components:
                    idx = self.component_names.index(component)
                    self.CANData[component].append(res[idx][1])
                    self.yAxisLabels[component] = f"{component} ({res[idx][2]})"
                self.time.append(t)

                self.plot_data()

    def plot_from_serial(self,channelData_byID_line):
        try:
            t = int(channelData_byID_line.strip().split(":")[1])
            channelData_byID_line = channelData_byID_line.split(":")[0].strip()
            if str(int(channelData_byID_line[0:3], 16)) == self.specificChannelID:
                processedData = process_data(channelData_byID_line)
                for component in self.components:
                    idx = self.component_names.index(component)
                    self.CANData[component].append(processedData[idx][1])
                    self.yAxisLabels[component] = f"{component} ({processedData[idx][2]})"

                self.time.append(t)
                self.plot_data()
        except:
            pass

    def plot_data(self):
        self.ax.clear()
          # Make the graph background dark
        colors = ['#FF6347', '#7FFF00', '#1E90FF']  # Use bright RGB colors for the lines

        # Clear previous twin axes
        for twin_ax in self.twin_axes:
            twin_ax.remove()
        self.twin_axes = []

        for i, (component, data) in enumerate(self.CANData.items()):

            if i > 0:
                ax = self.ax.twinx()
                ax.spines['right'].set_position(('outward', 70 * (i - 1)))
                self.twin_axes.append(ax)
            else:
                ax = self.ax

            ax.step(self.time, data, color=colors[i], label=component)
            ax.set_ylabel(self.yAxisLabels[component], color=colors[i])
            ax.tick_params(axis='y', labelcolor=colors[i])
            ax.tick_params(axis='x', labelcolor=colors[i])

        self.canvas.draw()
        QApplication.processEvents()


if __name__ == '__main__':
    matplotlib.use('Qt5Agg')
    app = QApplication(sys.argv)
    mainWin = CANDataPlotter()
    mainWin.show()
    sys.exit(app.exec_())
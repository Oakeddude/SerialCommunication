using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {   
        private SerialPort serialPortArduino;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                serialPortArduino = new SerialPort();
                serialPortArduino.ReadTimeout = 1000;
                serialPortArduino.WriteTimeout = 1000;

                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
                labelStatus.Text = "Gereed";
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Fout bij initialisatie: " + ex.Message;
            }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private Parity GetSelectedParity()
        {
            if (radioButtonParityNone.Checked) return Parity.None;
            if (radioButtonParityEven.Checked) return Parity.Even;
            if (radioButtonParityOdd.Checked) return Parity.Odd;
            if (radioButtonParityMark.Checked) return Parity.Mark;
            if (radioButtonParitySpace.Checked) return Parity.Space;
            return Parity.None;
        }

        private StopBits GetSelectedStopBits()
        {
            if (radioButtonStopbitsNone.Checked) return StopBits.None;
            if (radioButtonStopbitsOne.Checked) return StopBits.One;
            if (radioButtonStopbitsOnePointFive.Checked) return StopBits.OnePointFive;
            if (radioButtonStopbitsTwo.Checked) return StopBits.Two;
            return StopBits.One;
        }

        private Handshake GetSelectedHandshake()
        {
            if (radioButtonHandshakeNone.Checked) return Handshake.None;
            if (radioButtonHandshakeRTS.Checked) return Handshake.RequestToSend;
            if (radioButtonHandshakeRTSXonXoff.Checked) return Handshake.RequestToSendXOnXOff;
            if (radioButtonHandshakeXonXoff.Checked) return Handshake.XOnXOff;
            return Handshake.None;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!serialPortArduino.IsOpen)
                {
                    serialPortArduino.PortName = (string)comboBoxPoort.SelectedItem;
                    serialPortArduino.BaudRate = int.Parse((string)comboBoxBaudrate.SelectedItem);
                    serialPortArduino.DataBits = (int)numericUpDownDatabits.Value;
                    serialPortArduino.Parity = GetSelectedParity();
                    serialPortArduino.StopBits = GetSelectedStopBits();
                    serialPortArduino.Handshake = GetSelectedHandshake();
                    serialPortArduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPortArduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPortArduino.Open();

                    radioButtonVerbonden.Checked = true;
                    buttonConnect.Text = "Disconnect";
                    labelStatus.Text = "Verbonden met " + serialPortArduino.PortName;
                }
                else
                {
                    serialPortArduino.Close();

                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                    labelStatus.Text = "Verbinding verbroken";
                }
            }
            catch (Exception ex)
            {
                labelStatus.Text = "Fout: " + ex.Message;
            }
        }
    }   


}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace Kurs_Project_var25
{
    public partial class SettingsForm : Form
    {
        public bool flag = false;
        public SettingsForm()
        {
            InitializeComponent();
            BaudRateComboBox.SelectedIndex = Properties.Settings.Default.BaudRateIndex;
            foreach (string s in SerialPort.GetPortNames())
                ComPortComboBox.Items.Add(s);
            ComPortComboBox.SelectedIndex = Properties.Settings.Default.ComNameIndex;
            InBufferNumericUpDown.Value = Properties.Settings.Default.InBuffer;
            OutBufferNumericUpDown.Value = Properties.Settings.Default.OutBuffer;
            ReadNumericUpDown.Value = Properties.Settings.Default.ReadTimeout;
            WriteNumericUpDown.Value = Properties.Settings.Default.WriteTimeout;
            FrequencyNUD.Value = Properties.Settings.Default.Frequency;
            if (Properties.Settings.Default.SequentialMode == false)
            {
                ParallelRB.Checked = true;
            }
            else
            {
                SequentialRB.Checked = true;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            //Проверка сохранения параметров программы (для сохранения настроек)
            Properties.Settings.Default.BaudRate = int.Parse(BaudRateComboBox.Text);
            Properties.Settings.Default.BaudRateIndex = BaudRateComboBox.SelectedIndex;
            Properties.Settings.Default.ComNameIndex = ComPortComboBox.SelectedIndex;
            Properties.Settings.Default.ComName = (string)ComPortComboBox.SelectedItem;
            Properties.Settings.Default.InBuffer = (int)InBufferNumericUpDown.Value;
            Properties.Settings.Default.OutBuffer = (int)OutBufferNumericUpDown.Value;
            Properties.Settings.Default.ReadTimeout = (int)ReadNumericUpDown.Value;
            Properties.Settings.Default.WriteTimeout = (int)WriteNumericUpDown.Value;
            Properties.Settings.Default.Frequency = (uint)FrequencyNUD.Value;
            if (ParallelRB.Checked == true)
                Properties.Settings.Default.SequentialMode = false;
            else
                Properties.Settings.Default.SequentialMode = true;
            Properties.Settings.Default.Save();
            flag = true;
            this.Close();
        }
    }
}

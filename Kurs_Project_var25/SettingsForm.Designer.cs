namespace Kurs_Project_var25
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.BaudRateComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.InBufferNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.OutBufferNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.ComPortComboBox = new System.Windows.Forms.ComboBox();
            this.WriteNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ReadNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.InBufferNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutBufferNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WriteNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CancelButton.Location = new System.Drawing.Point(0, 264);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(354, 23);
            this.CancelButton.TabIndex = 0;
            this.CancelButton.Text = "Отмена";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.OKButton.Location = new System.Drawing.Point(0, 241);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(354, 23);
            this.OKButton.TabIndex = 1;
            this.OKButton.Text = "Сохранить";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Скорость передачи (в бодах):";
            // 
            // BaudRateComboBox
            // 
            this.BaudRateComboBox.FormattingEnabled = true;
            this.BaudRateComboBox.Items.AddRange(new object[] {
            "110",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "56000",
            "57600",
            "115200"});
            this.BaudRateComboBox.Location = new System.Drawing.Point(209, 58);
            this.BaudRateComboBox.Name = "BaudRateComboBox";
            this.BaudRateComboBox.Size = new System.Drawing.Size(120, 21);
            this.BaudRateComboBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(138, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Размер входного буфера:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Размер выходного буфера:";
            // 
            // InBufferNumericUpDown
            // 
            this.InBufferNumericUpDown.Location = new System.Drawing.Point(209, 95);
            this.InBufferNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.InBufferNumericUpDown.Minimum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.InBufferNumericUpDown.Name = "InBufferNumericUpDown";
            this.InBufferNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.InBufferNumericUpDown.TabIndex = 6;
            this.InBufferNumericUpDown.Value = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            // 
            // OutBufferNumericUpDown
            // 
            this.OutBufferNumericUpDown.Location = new System.Drawing.Point(209, 131);
            this.OutBufferNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.OutBufferNumericUpDown.Minimum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.OutBufferNumericUpDown.Name = "OutBufferNumericUpDown";
            this.OutBufferNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.OutBufferNumericUpDown.TabIndex = 7;
            this.OutBufferNumericUpDown.Value = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(108, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Имя COM-порта:";
            // 
            // ComPortComboBox
            // 
            this.ComPortComboBox.FormattingEnabled = true;
            this.ComPortComboBox.Location = new System.Drawing.Point(209, 21);
            this.ComPortComboBox.Name = "ComPortComboBox";
            this.ComPortComboBox.Size = new System.Drawing.Size(120, 21);
            this.ComPortComboBox.TabIndex = 9;
            // 
            // WriteNumericUpDown
            // 
            this.WriteNumericUpDown.Location = new System.Drawing.Point(209, 203);
            this.WriteNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.WriteNumericUpDown.Name = "WriteNumericUpDown";
            this.WriteNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.WriteNumericUpDown.TabIndex = 13;
            this.WriteNumericUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // ReadNumericUpDown
            // 
            this.ReadNumericUpDown.Location = new System.Drawing.Point(209, 167);
            this.ReadNumericUpDown.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.ReadNumericUpDown.Name = "ReadNumericUpDown";
            this.ReadNumericUpDown.Size = new System.Drawing.Size(120, 20);
            this.ReadNumericUpDown.TabIndex = 12;
            this.ReadNumericUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(104, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Тайм-аут записи:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(106, 168);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Тайм-аут чтения:";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 287);
            this.Controls.Add(this.WriteNumericUpDown);
            this.Controls.Add(this.ReadNumericUpDown);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ComPortComboBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.OutBufferNumericUpDown);
            this.Controls.Add(this.InBufferNumericUpDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BaudRateComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            ((System.ComponentModel.ISupportInitialize)(this.InBufferNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutBufferNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WriteNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox BaudRateComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown InBufferNumericUpDown;
        private System.Windows.Forms.NumericUpDown OutBufferNumericUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ComPortComboBox;
        private System.Windows.Forms.NumericUpDown WriteNumericUpDown;
        private System.Windows.Forms.NumericUpDown ReadNumericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}
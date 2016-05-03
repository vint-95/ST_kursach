namespace Kurs_Project_var25
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SendGroupBox = new System.Windows.Forms.GroupBox();
            this.SendNameLabel = new System.Windows.Forms.Label();
            this.SendProgressBar = new System.Windows.Forms.ProgressBar();
            this.SendPathTextBox = new System.Windows.Forms.TextBox();
            this.PathSendLabel = new System.Windows.Forms.Label();
            this.SendFileButton = new System.Windows.Forms.Button();
            this.ChooseFileButton = new System.Windows.Forms.Button();
            this.GetGroupBox = new System.Windows.Forms.GroupBox();
            this.GetLabel = new System.Windows.Forms.Label();
            this.DeclineButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.GetMessageLabel = new System.Windows.Forms.Label();
            this.GetProgressBar = new System.Windows.Forms.ProgressBar();
            this.InfoRTB = new System.Windows.Forms.RichTextBox();
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.ProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ChooseFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AutoAcceptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RestoreLostPacketsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConnSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AuthorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SendOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.AcceptedSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.COMPort = new System.IO.Ports.SerialPort(this.components);
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.ConnectionStatusTSSL = new System.Windows.Forms.ToolStripStatusLabel();
            this.ConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SendGroupBox.SuspendLayout();
            this.GetGroupBox.SuspendLayout();
            this.MainMenuStrip.SuspendLayout();
            this.MainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // SendGroupBox
            // 
            this.SendGroupBox.Controls.Add(this.SendNameLabel);
            this.SendGroupBox.Controls.Add(this.SendProgressBar);
            this.SendGroupBox.Controls.Add(this.SendPathTextBox);
            this.SendGroupBox.Controls.Add(this.PathSendLabel);
            this.SendGroupBox.Controls.Add(this.SendFileButton);
            this.SendGroupBox.Controls.Add(this.ChooseFileButton);
            this.SendGroupBox.Location = new System.Drawing.Point(12, 56);
            this.SendGroupBox.Name = "SendGroupBox";
            this.SendGroupBox.Size = new System.Drawing.Size(773, 97);
            this.SendGroupBox.TabIndex = 0;
            this.SendGroupBox.TabStop = false;
            this.SendGroupBox.Text = "Отправка";
            // 
            // SendNameLabel
            // 
            this.SendNameLabel.AutoSize = true;
            this.SendNameLabel.Location = new System.Drawing.Point(6, 52);
            this.SendNameLabel.Name = "SendNameLabel";
            this.SendNameLabel.Size = new System.Drawing.Size(116, 13);
            this.SendNameLabel.TabIndex = 5;
            this.SendNameLabel.Text = "Отправляемый файл:";
            // 
            // SendProgressBar
            // 
            this.SendProgressBar.Location = new System.Drawing.Point(9, 68);
            this.SendProgressBar.Name = "SendProgressBar";
            this.SendProgressBar.Size = new System.Drawing.Size(758, 23);
            this.SendProgressBar.TabIndex = 4;
            // 
            // SendPathTextBox
            // 
            this.SendPathTextBox.Location = new System.Drawing.Point(179, 21);
            this.SendPathTextBox.Name = "SendPathTextBox";
            this.SendPathTextBox.Size = new System.Drawing.Size(426, 20);
            this.SendPathTextBox.TabIndex = 3;
            // 
            // PathSendLabel
            // 
            this.PathSendLabel.AutoSize = true;
            this.PathSendLabel.Location = new System.Drawing.Point(6, 24);
            this.PathSendLabel.Name = "PathSendLabel";
            this.PathSendLabel.Size = new System.Drawing.Size(167, 13);
            this.PathSendLabel.TabIndex = 2;
            this.PathSendLabel.Text = "Выберите отправляемый файл:";
            // 
            // SendFileButton
            // 
            this.SendFileButton.Location = new System.Drawing.Point(692, 19);
            this.SendFileButton.Name = "SendFileButton";
            this.SendFileButton.Size = new System.Drawing.Size(75, 23);
            this.SendFileButton.TabIndex = 1;
            this.SendFileButton.Text = "Отправить";
            this.SendFileButton.UseVisualStyleBackColor = true;
            this.SendFileButton.Click += new System.EventHandler(this.SendFileButton_Click);
            // 
            // ChooseFileButton
            // 
            this.ChooseFileButton.Location = new System.Drawing.Point(611, 19);
            this.ChooseFileButton.Name = "ChooseFileButton";
            this.ChooseFileButton.Size = new System.Drawing.Size(75, 23);
            this.ChooseFileButton.TabIndex = 0;
            this.ChooseFileButton.Text = "Выбрать...";
            this.ChooseFileButton.UseVisualStyleBackColor = true;
            this.ChooseFileButton.Click += new System.EventHandler(this.ChooseFileButton_Click);
            // 
            // GetGroupBox
            // 
            this.GetGroupBox.Controls.Add(this.GetLabel);
            this.GetGroupBox.Controls.Add(this.DeclineButton);
            this.GetGroupBox.Controls.Add(this.ApplyButton);
            this.GetGroupBox.Controls.Add(this.GetMessageLabel);
            this.GetGroupBox.Controls.Add(this.GetProgressBar);
            this.GetGroupBox.Location = new System.Drawing.Point(12, 159);
            this.GetGroupBox.Name = "GetGroupBox";
            this.GetGroupBox.Size = new System.Drawing.Size(773, 142);
            this.GetGroupBox.TabIndex = 1;
            this.GetGroupBox.TabStop = false;
            this.GetGroupBox.Text = "Получение";
            // 
            // GetLabel
            // 
            this.GetLabel.AutoSize = true;
            this.GetLabel.Location = new System.Drawing.Point(6, 97);
            this.GetLabel.Name = "GetLabel";
            this.GetLabel.Size = new System.Drawing.Size(103, 13);
            this.GetLabel.TabIndex = 5;
            this.GetLabel.Text = "Получаемый файл:";
            // 
            // DeclineButton
            // 
            this.DeclineButton.Location = new System.Drawing.Point(424, 70);
            this.DeclineButton.Name = "DeclineButton";
            this.DeclineButton.Size = new System.Drawing.Size(82, 23);
            this.DeclineButton.TabIndex = 4;
            this.DeclineButton.Text = "Отклонить";
            this.DeclineButton.UseVisualStyleBackColor = true;
            this.DeclineButton.Click += new System.EventHandler(this.DeclineButton_Click);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(266, 70);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(82, 23);
            this.ApplyButton.TabIndex = 3;
            this.ApplyButton.Text = "Подтвердить";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // GetMessageLabel
            // 
            this.GetMessageLabel.AutoSize = true;
            this.GetMessageLabel.Location = new System.Drawing.Point(231, 16);
            this.GetMessageLabel.Name = "GetMessageLabel";
            this.GetMessageLabel.Size = new System.Drawing.Size(310, 39);
            this.GetMessageLabel.TabIndex = 1;
            this.GetMessageLabel.Text = "Получен запрос на принятие нового файла. Принять файл?\r\n\r\nИмя файла:";
            // 
            // GetProgressBar
            // 
            this.GetProgressBar.Location = new System.Drawing.Point(9, 113);
            this.GetProgressBar.Name = "GetProgressBar";
            this.GetProgressBar.Size = new System.Drawing.Size(758, 23);
            this.GetProgressBar.TabIndex = 0;
            // 
            // InfoRTB
            // 
            this.InfoRTB.Location = new System.Drawing.Point(12, 307);
            this.InfoRTB.Name = "InfoRTB";
            this.InfoRTB.ReadOnly = true;
            this.InfoRTB.Size = new System.Drawing.Size(773, 216);
            this.InfoRTB.TabIndex = 2;
            this.InfoRTB.Text = "Здесь будет отображаться информация о передаче файлов";
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgramToolStripMenuItem,
            this.AboutToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainMenuStrip.Size = new System.Drawing.Size(797, 24);
            this.MainMenuStrip.TabIndex = 3;
            // 
            // ProgramToolStripMenuItem
            // 
            this.ProgramToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChooseFileToolStripMenuItem,
            this.ConsoleToolStripMenuItem,
            this.AutoAcceptToolStripMenuItem,
            this.RestoreLostPacketsToolStripMenuItem,
            this.ConnSettingsToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.ProgramToolStripMenuItem.Name = "ProgramToolStripMenuItem";
            this.ProgramToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.ProgramToolStripMenuItem.Text = "Программа";
            // 
            // ChooseFileToolStripMenuItem
            // 
            this.ChooseFileToolStripMenuItem.Name = "ChooseFileToolStripMenuItem";
            this.ChooseFileToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.ChooseFileToolStripMenuItem.Text = "Выбрать файл...";
            this.ChooseFileToolStripMenuItem.Click += new System.EventHandler(this.ChooseFileToolStripMenuItem_Click);
            // 
            // AutoAcceptToolStripMenuItem
            // 
            this.AutoAcceptToolStripMenuItem.CheckOnClick = true;
            this.AutoAcceptToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AutoAcceptToolStripMenuItem.Name = "AutoAcceptToolStripMenuItem";
            this.AutoAcceptToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.AutoAcceptToolStripMenuItem.Text = "Автоматически подтверждать принятие";
            this.AutoAcceptToolStripMenuItem.Click += new System.EventHandler(this.AutoAcceptToolStripMenuItem_Click);
            // 
            // RestoreLostPacketsToolStripMenuItem
            // 
            this.RestoreLostPacketsToolStripMenuItem.CheckOnClick = true;
            this.RestoreLostPacketsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.RestoreLostPacketsToolStripMenuItem.Name = "RestoreLostPacketsToolStripMenuItem";
            this.RestoreLostPacketsToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.RestoreLostPacketsToolStripMenuItem.Text = "Прерывать передачу при ошибке?";
            this.RestoreLostPacketsToolStripMenuItem.Click += new System.EventHandler(this.RestoreLostPacketsToolStripMenuItem_Click);
            // 
            // ConnSettingsToolStripMenuItem
            // 
            this.ConnSettingsToolStripMenuItem.Name = "ConnSettingsToolStripMenuItem";
            this.ConnSettingsToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.ConnSettingsToolStripMenuItem.Text = "Свойства соединения";
            this.ConnSettingsToolStripMenuItem.Click += new System.EventHandler(this.ConnSettingsToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.ExitToolStripMenuItem.Text = "Выход";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AuthorsToolStripMenuItem});
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.AboutToolStripMenuItem.Text = "О программе";
            // 
            // AuthorsToolStripMenuItem
            // 
            this.AuthorsToolStripMenuItem.Name = "AuthorsToolStripMenuItem";
            this.AuthorsToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.AuthorsToolStripMenuItem.Text = "Об авторах";
            this.AuthorsToolStripMenuItem.Click += new System.EventHandler(this.AuthorsToolStripMenuItem_Click);
            // 
            // COMPort
            // 
            this.COMPort.PortName = "COM4";
            this.COMPort.WriteBufferSize = 4096;
            // 
            // MainStatusStrip
            // 
            this.MainStatusStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainStatusStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectionStatusTSSL});
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 24);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.Size = new System.Drawing.Size(797, 22);
            this.MainStatusStrip.SizingGrip = false;
            this.MainStatusStrip.TabIndex = 4;
            // 
            // ConnectionStatusTSSL
            // 
            this.ConnectionStatusTSSL.Name = "ConnectionStatusTSSL";
            this.ConnectionStatusTSSL.Size = new System.Drawing.Size(77, 17);
            this.ConnectionStatusTSSL.Text = "Соединение:";
            // 
            // ConsoleToolStripMenuItem
            // 
            this.ConsoleToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ConsoleToolStripMenuItem.Name = "ConsoleToolStripMenuItem";
            this.ConsoleToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.ConsoleToolStripMenuItem.Text = "Консоль?";
            this.ConsoleToolStripMenuItem.Click += new System.EventHandler(this.ConsoleToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 530);
            this.Controls.Add(this.MainStatusStrip);
            this.Controls.Add(this.InfoRTB);
            this.Controls.Add(this.GetGroupBox);
            this.Controls.Add(this.SendGroupBox);
            this.Controls.Add(this.MainMenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Передача данных через COM-порт";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SendGroupBox.ResumeLayout(false);
            this.SendGroupBox.PerformLayout();
            this.GetGroupBox.ResumeLayout(false);
            this.GetGroupBox.PerformLayout();
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.MainStatusStrip.ResumeLayout(false);
            this.MainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox SendGroupBox;
        private System.Windows.Forms.GroupBox GetGroupBox;
        private System.Windows.Forms.RichTextBox InfoRTB;
        private System.Windows.Forms.TextBox SendPathTextBox;
        private System.Windows.Forms.Label PathSendLabel;
        private System.Windows.Forms.Button SendFileButton;
        private System.Windows.Forms.Button ChooseFileButton;
        private System.Windows.Forms.Button DeclineButton;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Label GetMessageLabel;
        private System.Windows.Forms.ProgressBar GetProgressBar;
        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ProgramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ChooseFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AutoAcceptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AuthorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog SendOpenFileDialog;
        private System.Windows.Forms.SaveFileDialog AcceptedSaveFileDialog;
        private System.Windows.Forms.Label SendNameLabel;
        private System.Windows.Forms.ProgressBar SendProgressBar;
        private System.IO.Ports.SerialPort COMPort;
        private System.Windows.Forms.Label GetLabel;
        private System.Windows.Forms.StatusStrip MainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ConnectionStatusTSSL;
        private System.Windows.Forms.ToolStripMenuItem ConnSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RestoreLostPacketsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ConsoleToolStripMenuItem;
    }
}


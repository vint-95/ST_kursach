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
            this.SendGroupBox.Location = new System.Drawing.Point(18, 86);
            this.SendGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SendGroupBox.Name = "SendGroupBox";
            this.SendGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SendGroupBox.Size = new System.Drawing.Size(1160, 149);
            this.SendGroupBox.TabIndex = 0;
            this.SendGroupBox.TabStop = false;
            this.SendGroupBox.Text = "Отправка";
            // 
            // SendNameLabel
            // 
            this.SendNameLabel.AutoSize = true;
            this.SendNameLabel.Location = new System.Drawing.Point(9, 80);
            this.SendNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SendNameLabel.Name = "SendNameLabel";
            this.SendNameLabel.Size = new System.Drawing.Size(176, 20);
            this.SendNameLabel.TabIndex = 5;
            this.SendNameLabel.Text = "Отправляемый файл:";
            // 
            // SendProgressBar
            // 
            this.SendProgressBar.Location = new System.Drawing.Point(14, 105);
            this.SendProgressBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SendProgressBar.Name = "SendProgressBar";
            this.SendProgressBar.Size = new System.Drawing.Size(1137, 35);
            this.SendProgressBar.TabIndex = 4;
            // 
            // SendPathTextBox
            // 
            this.SendPathTextBox.Location = new System.Drawing.Point(268, 32);
            this.SendPathTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SendPathTextBox.Name = "SendPathTextBox";
            this.SendPathTextBox.Size = new System.Drawing.Size(637, 26);
            this.SendPathTextBox.TabIndex = 3;
            // 
            // PathSendLabel
            // 
            this.PathSendLabel.AutoSize = true;
            this.PathSendLabel.Location = new System.Drawing.Point(9, 37);
            this.PathSendLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PathSendLabel.Name = "PathSendLabel";
            this.PathSendLabel.Size = new System.Drawing.Size(253, 20);
            this.PathSendLabel.TabIndex = 2;
            this.PathSendLabel.Text = "Выберите отправляемый файл:";
            // 
            // SendFileButton
            // 
            this.SendFileButton.Location = new System.Drawing.Point(1038, 29);
            this.SendFileButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SendFileButton.Name = "SendFileButton";
            this.SendFileButton.Size = new System.Drawing.Size(112, 35);
            this.SendFileButton.TabIndex = 1;
            this.SendFileButton.Text = "Отправить";
            this.SendFileButton.UseVisualStyleBackColor = true;
            this.SendFileButton.Click += new System.EventHandler(this.SendFileButton_Click);
            // 
            // ChooseFileButton
            // 
            this.ChooseFileButton.Location = new System.Drawing.Point(916, 29);
            this.ChooseFileButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ChooseFileButton.Name = "ChooseFileButton";
            this.ChooseFileButton.Size = new System.Drawing.Size(112, 35);
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
            this.GetGroupBox.Location = new System.Drawing.Point(18, 245);
            this.GetGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GetGroupBox.Name = "GetGroupBox";
            this.GetGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GetGroupBox.Size = new System.Drawing.Size(1160, 218);
            this.GetGroupBox.TabIndex = 1;
            this.GetGroupBox.TabStop = false;
            this.GetGroupBox.Text = "Получение";
            // 
            // GetLabel
            // 
            this.GetLabel.AutoSize = true;
            this.GetLabel.Location = new System.Drawing.Point(9, 149);
            this.GetLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GetLabel.Name = "GetLabel";
            this.GetLabel.Size = new System.Drawing.Size(156, 20);
            this.GetLabel.TabIndex = 5;
            this.GetLabel.Text = "Получаемый файл:";
            // 
            // DeclineButton
            // 
            this.DeclineButton.Location = new System.Drawing.Point(636, 108);
            this.DeclineButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DeclineButton.Name = "DeclineButton";
            this.DeclineButton.Size = new System.Drawing.Size(123, 35);
            this.DeclineButton.TabIndex = 4;
            this.DeclineButton.Text = "Отклонить";
            this.DeclineButton.UseVisualStyleBackColor = true;
            this.DeclineButton.Click += new System.EventHandler(this.DeclineButton_Click);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(399, 108);
            this.ApplyButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(123, 35);
            this.ApplyButton.TabIndex = 3;
            this.ApplyButton.Text = "Подтвердить";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
            // 
            // GetMessageLabel
            // 
            this.GetMessageLabel.AutoSize = true;
            this.GetMessageLabel.Location = new System.Drawing.Point(346, 25);
            this.GetMessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GetMessageLabel.Name = "GetMessageLabel";
            this.GetMessageLabel.Size = new System.Drawing.Size(470, 60);
            this.GetMessageLabel.TabIndex = 1;
            this.GetMessageLabel.Text = "Получен запрос на принятие нового файла. Принять файл?\r\n\r\nИмя файла:";
            // 
            // GetProgressBar
            // 
            this.GetProgressBar.Location = new System.Drawing.Point(14, 174);
            this.GetProgressBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GetProgressBar.Name = "GetProgressBar";
            this.GetProgressBar.Size = new System.Drawing.Size(1137, 35);
            this.GetProgressBar.TabIndex = 0;
            // 
            // InfoRTB
            // 
            this.InfoRTB.Location = new System.Drawing.Point(18, 472);
            this.InfoRTB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.InfoRTB.Name = "InfoRTB";
            this.InfoRTB.ReadOnly = true;
            this.InfoRTB.Size = new System.Drawing.Size(1158, 330);
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
            this.MainMenuStrip.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.MainMenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainMenuStrip.Size = new System.Drawing.Size(1196, 35);
            this.MainMenuStrip.TabIndex = 3;
            // 
            // ProgramToolStripMenuItem
            // 
            this.ProgramToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ChooseFileToolStripMenuItem,
            this.AutoAcceptToolStripMenuItem,
            this.RestoreLostPacketsToolStripMenuItem,
            this.ConnSettingsToolStripMenuItem,
            this.ExitToolStripMenuItem});
            this.ProgramToolStripMenuItem.Name = "ProgramToolStripMenuItem";
            this.ProgramToolStripMenuItem.Size = new System.Drawing.Size(121, 29);
            this.ProgramToolStripMenuItem.Text = "Программа";
            // 
            // ChooseFileToolStripMenuItem
            // 
            this.ChooseFileToolStripMenuItem.Name = "ChooseFileToolStripMenuItem";
            this.ChooseFileToolStripMenuItem.Size = new System.Drawing.Size(424, 30);
            this.ChooseFileToolStripMenuItem.Text = "Выбрать файл...";
            this.ChooseFileToolStripMenuItem.Click += new System.EventHandler(this.ChooseFileToolStripMenuItem_Click);
            // 
            // AutoAcceptToolStripMenuItem
            // 
            this.AutoAcceptToolStripMenuItem.CheckOnClick = true;
            this.AutoAcceptToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.AutoAcceptToolStripMenuItem.Name = "AutoAcceptToolStripMenuItem";
            this.AutoAcceptToolStripMenuItem.Size = new System.Drawing.Size(424, 30);
            this.AutoAcceptToolStripMenuItem.Text = "Автоматически подтверждать принятие";
            this.AutoAcceptToolStripMenuItem.Click += new System.EventHandler(this.AutoAcceptToolStripMenuItem_Click);
            // 
            // RestoreLostPacketsToolStripMenuItem
            // 
            this.RestoreLostPacketsToolStripMenuItem.CheckOnClick = true;
            this.RestoreLostPacketsToolStripMenuItem.Name = "RestoreLostPacketsToolStripMenuItem";
            this.RestoreLostPacketsToolStripMenuItem.Size = new System.Drawing.Size(424, 30);
            this.RestoreLostPacketsToolStripMenuItem.Text = "Прерывать передачу при ошибке?";
            this.RestoreLostPacketsToolStripMenuItem.Click += new System.EventHandler(this.RestoreLostPacketsToolStripMenuItem_Click);
            // 
            // ConnSettingsToolStripMenuItem
            // 
            this.ConnSettingsToolStripMenuItem.Name = "ConnSettingsToolStripMenuItem";
            this.ConnSettingsToolStripMenuItem.Size = new System.Drawing.Size(424, 30);
            this.ConnSettingsToolStripMenuItem.Text = "Свойства соединения";
            this.ConnSettingsToolStripMenuItem.Click += new System.EventHandler(this.ConnSettingsToolStripMenuItem_Click);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(424, 30);
            this.ExitToolStripMenuItem.Text = "Выход";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AuthorsToolStripMenuItem});
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(137, 29);
            this.AboutToolStripMenuItem.Text = "О программе";
            // 
            // AuthorsToolStripMenuItem
            // 
            this.AuthorsToolStripMenuItem.Name = "AuthorsToolStripMenuItem";
            this.AuthorsToolStripMenuItem.Size = new System.Drawing.Size(211, 30);
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
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 35);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.Padding = new System.Windows.Forms.Padding(2, 0, 21, 0);
            this.MainStatusStrip.Size = new System.Drawing.Size(1196, 30);
            this.MainStatusStrip.SizingGrip = false;
            this.MainStatusStrip.TabIndex = 4;
            // 
            // ConnectionStatusTSSL
            // 
            this.ConnectionStatusTSSL.Name = "ConnectionStatusTSSL";
            this.ConnectionStatusTSSL.Size = new System.Drawing.Size(115, 25);
            this.ConnectionStatusTSSL.Text = "Соединение:";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1196, 815);
            this.Controls.Add(this.MainStatusStrip);
            this.Controls.Add(this.InfoRTB);
            this.Controls.Add(this.GetGroupBox);
            this.Controls.Add(this.SendGroupBox);
            this.Controls.Add(this.MainMenuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
    }
}


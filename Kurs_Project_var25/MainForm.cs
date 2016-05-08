using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Diagnostics;

namespace Kurs_Project_var25
{
    public partial class MainForm : Form
    {
        private bool AutoApplying = false;  //Переменная для автоматического принятия файлов
        string SendedFileName;              //Имя отправляемого файла
        string AppliedFileName;             //Имя получаемого файла
        Thread SynchronizationThread;       //Потоки для ежесекундной обработки информации, 
        Thread ConnectionThread;            //касающейся входящих данных и соединения
        FileStream SaveFileStream;          //Потоки для сохранения
        FileStream SendFileStream;          //и отправки файлов
        SynchronizationContext UIContext;   //Вещь для синхронизации контролов в форме. Очень нужна для управления контролами из не-родных потоков
        bool ConnStatus = false;            //Текущий статус порта
        bool RHeader = false;               //Получен заголовок
        bool SHeader = false;               //Отправлен заголовок
        bool RData = false;                 //Получен файл
        bool SData = false;                 //Отправлен файл
        byte[][] WriteData;                 //Отправляемые данные
        byte[] ReadData;                    //Получаемые данные
        long Pointer;                       //Указатель на текущий передаваемый пакет
        bool Accepting = false;             //Подтверждение отправки
        int ErrorCounter = 0;               //Счётчик ошибок
        bool Restore = false;               //Восстановление передачи (а надо ли)
        bool Console = false;               //Вывод в RichTextBox
        byte IndexOfFile;                   //Идентификатор для файла
        uint IndexOfInfopacket = 0;         //Идентификатор для пакета
        uint MaxIndexOfInfopacket = 0;      //Количество пакетов
        byte FinalizationStatus = 1;        //Текущий статус окончания передачи
        static bool ErrorInfo = false;      //Информация об ошибке
        uint Frequency = 100;               //Частота чтения входящего потока
        bool BigFile = false;               //Проверка на величину файла
        byte[] byFileData = new byte[] { }; //Файл, заносимый в одномерный массив
        Encoding ANSI = Encoding.Default;   //С помощью этого задаем кодировку ANSI
        int CountPackets;                   //Максимальный индекс текущего получаемого файла 

        public MainForm()
        {
            InitializeComponent();
            GetMessageLabel.Visible = false;
            ApplyButton.Visible = false;
            DeclineButton.Visible = false;
            if (Properties.Settings.Default.FirstLaunch == true)
            {
                MessageBox.Show("Здравствуйте! Похоже, Вы запускаете программу в первый раз.\n Для начала необходимо выбрать COM-порт, с которым программа будет работать.");
                while(Properties.Settings.Default.FirstLaunch == true)
                {
                    var f = new SettingsForm();
                    f.ShowDialog();
                    if (f.flag == true)
                    {
                        Frequency = Properties.Settings.Default.Frequency;
                        COMPort.BaudRate = Properties.Settings.Default.BaudRate;
                        COMPort.PortName = Properties.Settings.Default.ComName;
                        COMPort.ReadBufferSize = Properties.Settings.Default.InBuffer;
                        COMPort.WriteBufferSize = Properties.Settings.Default.OutBuffer;
                        COMPort.ReadTimeout = Properties.Settings.Default.ReadTimeout;
                        COMPort.WriteTimeout = Properties.Settings.Default.WriteTimeout;
                        Properties.Settings.Default.FirstLaunch = false;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, настройте программу для первого запуска.");
                    }
                }
            }
            COMPort.BaudRate = Properties.Settings.Default.BaudRate;
            COMPort.PortName = Properties.Settings.Default.ComName;
            COMPort.ReadBufferSize = Properties.Settings.Default.InBuffer;
            COMPort.WriteBufferSize = Properties.Settings.Default.OutBuffer;
            COMPort.ReadTimeout = Properties.Settings.Default.ReadTimeout;
            COMPort.WriteTimeout = Properties.Settings.Default.WriteTimeout;
            Frequency = Properties.Settings.Default.Frequency;
            COMPort.DtrEnable = true;
            COMPort.RtsEnable = false;
            COMPort.Handshake = Handshake.None;
            MaxIndexOfInfopacket = (uint)Properties.Settings.Default.OutBuffer;
            //Restore = Properties.Settings.Default.Restore;
            AutoApplying = Properties.Settings.Default.AutomatedGet;
            UIContext = SynchronizationContext.Current;
            ConnectionThread = new Thread(Connect);
            SynchronizationThread = new Thread(ReadingThread);
            while(true)
            {
            try
            {
                COMPort.Open();
                break;
            }
            catch
            {
                MessageBox.Show("К сожалению, не удалось загрузить нужный порт. Необходимо перенастроить программу.");
                var f = new SettingsForm();
                f.ShowDialog();
                if (f.flag == true)
                {
                    Frequency = Properties.Settings.Default.Frequency;
                    COMPort.BaudRate = Properties.Settings.Default.BaudRate;
                    COMPort.PortName = Properties.Settings.Default.ComName;
                    COMPort.ReadBufferSize = Properties.Settings.Default.InBuffer;
                    COMPort.WriteBufferSize = Properties.Settings.Default.OutBuffer;
                    COMPort.ReadTimeout = Properties.Settings.Default.ReadTimeout;
                    COMPort.WriteTimeout = Properties.Settings.Default.WriteTimeout;
                    Properties.Settings.Default.FirstLaunch = false;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, перенастройте программу.");
                }
            }
            }
            ConnectionThread.Start();
            SynchronizationThread.Start();
            InfoRTB.Visible = false;
            this.Size = new Size(803, 335);
        }

        public byte[] ReadLocalFile(string sLocalFile)
        {
            //Занесение в массив через цикл - реализовать
            using (FileStream oFS = new FileStream(sLocalFile, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader oBR = new BinaryReader(oFS))
                {
                    if (oFS.Length < 2147483648)
                        return oBR.ReadBytes((int)oFS.Length);
                    else
                    {
                        MessageBox.Show("Выберите, пожалуйста, файл, размер которого не превышает 2 Гб.","Предупреждение",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                        BigFile = true;
                        return new byte[] { 0 };
                    }
                        
                }
            }
        }

        private void ConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Console == true)
            {
                Console = false;
                InfoRTB.Visible = false;
                this.Size = new Size(803, 335);
                ConsoleToolStripMenuItem.Checked = false;
            }
            else
            {
                Console = true;
                InfoRTB.Visible = true;
                this.Size = new Size(803, 558);
                ConsoleToolStripMenuItem.Checked = true;
            }
        }

        private void ChooseFileButton_Click(object sender, EventArgs e)
        {
            ChoosePath(false);
        }

        private void AuthorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данный курсовой проект по дисциплине\n\"Сетевые технологии в АСОИУ\" сделали:\nЕгоров Алексей ИУ5-64\nТимур Мусин ИУ5-64\nВострокнутов Илья ИУ5-64", "Авторы", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Exit() == true)
            {
                ConnectionThread.Abort();
                SynchronizationThread.Abort();
                this.Dispose();
                Application.Exit();
            }
        }

        private void AutoAcceptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoApplying = AutoAcceptToolStripMenuItem.Checked;
        }

        private void RestoreLostPacketsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Restore = RestoreLostPacketsToolStripMenuItem.Checked;
        }

        private void ChooseFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoosePath(false);
        }

        /// <summary>
        /// Выбор файла для пересылки/Выбор имени файла и диретории для сохранения
        /// </summary>
        /// <param name="flag">true - сохранение, false - пересылка</param>
        /// <returns>Выполнилась функция или нет</returns>
        private bool ChoosePath(bool flag)
        {
            if (flag == false)
            {
                if (SendOpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)   //Костыль для того, чтобы при нажатии "Отмена" путь к папке не "исчезал"
                {
                    if (SendOpenFileDialog.FileName != "")
                    {
                        Array.Resize(ref byFileData, byFileData.Length);
                        byFileData = ReadLocalFile(SendOpenFileDialog.FileName);
                    }
                    if (BigFile == true)
                        return false;
                    SendPathTextBox.Text = SendOpenFileDialog.FileName;
                    SendedFileName = SendOpenFileDialog.SafeFileName;
                    return true;
                }
                return false;
            }
            if (AutoApplying == false && flag == true)
            {
                AcceptedSaveFileDialog.FileName = AppliedFileName;
                if (AcceptedSaveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    AppliedFileName = AcceptedSaveFileDialog.FileName;
                    return true;
                }
                return false;
            }
            return false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Exit() == true)
            {
                ConnectionThread.Abort();
                SynchronizationThread.Abort();
                this.Dispose();
                Application.Exit();
            }
            else e.Cancel = true;
        }

        private bool Exit()
        {
            DialogResult Exit = MessageBox.Show("Вы действительно хотите выйти?", "Подтверждение выхода", MessageBoxButtons.YesNo);
            if (Exit == DialogResult.Yes) return true;
            else return false;
        }

        private void ConnSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new SettingsForm();
            f.ShowDialog();
            if (f.flag == true)
            {
                DialogResult Reload = MessageBox.Show("Перезагрузить соединение для применения новых параметров?", "Подтверждение перезагрузки", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                if (Reload == DialogResult.Yes)
                {
                    SynchronizationThread.Abort();
                    ConnectionThread.Abort();
                    COMPort.Close();
                    COMPort.BaudRate = Properties.Settings.Default.BaudRate;
                    COMPort.PortName = Properties.Settings.Default.ComName;
                    COMPort.ReadBufferSize = Properties.Settings.Default.InBuffer;
                    COMPort.WriteBufferSize = Properties.Settings.Default.OutBuffer;
                    COMPort.ReadTimeout = Properties.Settings.Default.ReadTimeout;
                    COMPort.WriteTimeout = Properties.Settings.Default.WriteTimeout;
                    COMPort.Open();
                    SynchronizationThread = new Thread(ReadingThread);
                    ConnectionThread = new Thread(Connect);
                    SynchronizationThread.Start();
                    ConnectionThread.Start();
                }
            }
        }

        //итог: первый байт - количество отбрасываемых бит при переводе в двоичную систему
        //      второй - количество отбрасываемых бит в декодированном битовом массиве
        static void Code(ref byte[] msg)
        {
            System.Collections.BitArray bitMsg = new System.Collections.BitArray(msg);
            int numParts = bitMsg.Length / 11;
            if (bitMsg.Length % 11 != 0) numParts++;
            byte secondCheck = Convert.ToByte(11 - bitMsg.Length % 11);
            System.Collections.BitArray codeBitMsg = new System.Collections.BitArray(numParts * 15);
            for (int i = 0; i < numParts; ++i)
            {
                System.Collections.BitArray _15bitCodeArr = new System.Collections.BitArray(15);
                for (int j = 10; j >= 0; --j)
                {
                    bool temp = (bitMsg.Length > i * 11 + j) ? bitMsg.Get(i * 11 + j) : false;
                    if (j >= 4)
                    {
                        _15bitCodeArr.Set(j + 4, temp);
                    }
                    else if (j >= 1)
                    {
                        _15bitCodeArr.Set(j + 3, temp);
                    }
                    else
                    {
                        _15bitCodeArr.Set(2, temp);
                    }
                }
                _15bitCodeArr[7] = _15bitCodeArr.Get(14) ^ _15bitCodeArr.Get(13) ^ _15bitCodeArr.Get(12) ^ _15bitCodeArr.Get(11) ^
                                   _15bitCodeArr.Get(10) ^ _15bitCodeArr.Get(9) ^ _15bitCodeArr.Get(8);
                _15bitCodeArr[3] = _15bitCodeArr.Get(14) ^ _15bitCodeArr.Get(13) ^ _15bitCodeArr.Get(12) ^ _15bitCodeArr.Get(11) ^
                                   _15bitCodeArr.Get(6) ^ _15bitCodeArr.Get(5) ^ _15bitCodeArr.Get(4);
                _15bitCodeArr[1] = _15bitCodeArr.Get(14) ^ _15bitCodeArr.Get(13) ^ _15bitCodeArr.Get(10) ^ _15bitCodeArr.Get(9) ^
                                   _15bitCodeArr.Get(6) ^ _15bitCodeArr.Get(5) ^ _15bitCodeArr.Get(2);
                _15bitCodeArr[0] = _15bitCodeArr.Get(14) ^ _15bitCodeArr.Get(12) ^ _15bitCodeArr.Get(10) ^ _15bitCodeArr.Get(8) ^
                                   _15bitCodeArr.Get(6) ^ _15bitCodeArr.Get(4) ^ _15bitCodeArr.Get(2);
                for (int j = 0; j < 15; ++j)
                {
                    codeBitMsg.Set(i * 15 + j, _15bitCodeArr.Get(j));
                }
            }
            byte firstCheck = Convert.ToByte(8 - codeBitMsg.Length % 8);
            int newSize = codeBitMsg.Length / 8 + ((codeBitMsg.Length % 8 == 0) ? 2 : 3);
            msg = new byte[newSize];
            msg[0] = firstCheck;
            msg[1] = secondCheck;
            codeBitMsg.CopyTo(msg, 2);
            return;
        }

        static void Decode(ref byte[] msg)
        {
            byte firstCheck = msg[0];
            byte secondCheck = msg[1];
            int start = 16;
            System.Collections.BitArray bitMsg = new System.Collections.BitArray(msg);
            int length = bitMsg.Length - 16 - firstCheck;
            int numParts = length / 15;
            System.Collections.BitArray decodeBitMsg = new System.Collections.BitArray(numParts * 11 - secondCheck);
            for (int i = 0; i < numParts; ++i)
            {
                int current = start + i * 15;
                bool c1;
                bool c2;
                bool c3;
                bool c4;
                c4 = bitMsg.Get(current + 14) ^ bitMsg.Get(current + 13) ^ bitMsg.Get(current + 12) ^ bitMsg.Get(current + 11) ^
                     bitMsg.Get(current + 10) ^ bitMsg.Get(current + 9) ^ bitMsg.Get(current + 8) ^ bitMsg.Get(current + 7);
                c3 = bitMsg.Get(current + 14) ^ bitMsg.Get(current + 13) ^ bitMsg.Get(current + 12) ^ bitMsg.Get(current + 11) ^
                     bitMsg.Get(current + 6) ^ bitMsg.Get(current + 5) ^ bitMsg.Get(current + 4) ^ bitMsg.Get(current + 3);
                c2 = bitMsg.Get(current + 14) ^ bitMsg.Get(current + 13) ^ bitMsg.Get(current + 10) ^ bitMsg.Get(current + 9) ^
                     bitMsg.Get(current + 6) ^ bitMsg.Get(current + 5) ^ bitMsg.Get(current + 2) ^ bitMsg.Get(current + 1);
                c1 = bitMsg.Get(current + 14) ^ bitMsg.Get(current + 12) ^ bitMsg.Get(current + 10) ^ bitMsg.Get(current + 8) ^
                     bitMsg.Get(current + 6) ^ bitMsg.Get(current + 4) ^ bitMsg.Get(current + 2) ^ bitMsg.Get(current);

                if (c1 || c2 || c3 || c4)
                {
                    ErrorInfo = true;
                    return;
                }
                System.Collections.BitArray _11bitDecodeArr = new System.Collections.BitArray(11);
                _11bitDecodeArr[0] = bitMsg.Get(current + 2);
                _11bitDecodeArr[1] = bitMsg.Get(current + 4);
                _11bitDecodeArr[2] = bitMsg.Get(current + 5);
                _11bitDecodeArr[3] = bitMsg.Get(current + 6);
                _11bitDecodeArr[4] = bitMsg.Get(current + 8);
                _11bitDecodeArr[5] = bitMsg.Get(current + 9);
                _11bitDecodeArr[6] = bitMsg.Get(current + 10);
                _11bitDecodeArr[7] = bitMsg.Get(current + 11);
                _11bitDecodeArr[8] = bitMsg.Get(current + 12);
                _11bitDecodeArr[9] = bitMsg.Get(current + 13);
                _11bitDecodeArr[10] = bitMsg.Get(current + 14);

                for (int j = 0; j < 11; ++j)
                {
                    if (i * 11 + j < decodeBitMsg.Length)
                        decodeBitMsg.Set(i * 11 + j, _11bitDecodeArr.Get(j));
                }
            }
            int newSize = decodeBitMsg.Length / 8;
            msg = new byte[newSize];
            decodeBitMsg.CopyTo(msg, 0);
            return;
        }

        private void SendFileButton_Click(object sender, EventArgs e)
        {
            if (SendedFileName != null)
            {
                //Заносим в буфер недопереданных файлов (не недокачанных!)
                if (Properties.Settings.Default.NotCompletedFiles != null)
                {
                    IndexOfFile = (byte)Properties.Settings.Default.NotCompletedFiles.Count;
                    Properties.Settings.Default.NotCompletedFiles.Add(SendPathTextBox.Text);
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.NotCompletedFiles = new System.Collections.Specialized.StringCollection();
                    Properties.Settings.Default.NotCompletedFiles.Add(SendPathTextBox.Text);
                    Properties.Settings.Default.Save();
                }

                PartPacking(new byte[] { }, 'H', (uint)SendedFileName.Length);
                SHeader = true;
                InfoRTB.AppendText("\nЖдём ответа принимающей стороны");
            }
            else
                MessageBox.Show("Пожалуйста, выберите файл.");
        }

        private void DeclineButton_Click(object sender, EventArgs e)
        {
            GetMessage(false);
            if (Properties.Settings.Default.SequentialMode==true)
            SequentialHide(false);
            //Отправить на другой комп сигнал о том, что принятие файла отклонено
            PartPacking(new byte[] { }, 'N', 0);
        }

        /// <summary>
        /// Скрытие контролов, отвечающих за принятие решения по поводу получаемых файлов
        /// </summary>
        /// <param name="flag">false - скрываем, true - нутыпонял</param>
        private void GetMessage(bool flag)
        {
            if (flag == false)
            {
                UIContext.Send(io => GetMessageLabel.Visible = false, null);
                UIContext.Send(io => ApplyButton.Visible = false, null);
                UIContext.Send(io => DeclineButton.Visible = false, null);
            }
            else
            {
                UIContext.Send(io => GetMessageLabel.Visible = true, null);
                UIContext.Send(io => ApplyButton.Visible = true, null);
                UIContext.Send(io => DeclineButton.Visible = true, null);
                UIContext.Send(io => GetMessageLabel.Text = "Получен запрос на принятие нового файла. Принять файл?\n\nИмя файла: " + AppliedFileName, null); 
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ChoosePath(true);
            GetMessage(false);
            GetLabel.Text = "Получаемый файл: " + AppliedFileName;

            //Отправить сигнал о том, что разрешено отсылать файл
            PartPacking(new byte[] { }, 'Y', 0);
        }

        private void SequentialHide(bool flag)
        {
            if (flag == true)
                UIContext.Send(g => SendGroupBox.Visible = false, null);
            else
                UIContext.Send(g => SendGroupBox.Visible = true, null);
        }

        ///// <summary>
        ///// Кодирование по алгоритму Хэмминга
        ///// </summary>
        ///// <param name="information">Необработанный байт-массив с данными</param>
        //static byte[] HammingCoding(ref byte[] information, bool flag)
        //{
        //    //byte[] msg = new byte[] {}; //Вспомогательный массив для того, чтобы не затирать старый. Нафиг его
        //    if(flag == false)
        //        Code(ref information);
        //    else
        //        Decode(ref information);
        //    return information;
        //}

        /// <summary>
        /// Функция, принимаемая потоком. 
        /// Служит для соединения двух компьютеров и обозначения текущего статуса соединения
        /// </summary>
        private void Connect()
        {
            while (true)
            {
                Thread.Sleep(2000);
                try
                {
                    if (COMPort.IsOpen == true)
                    {
                        if (COMPort.DsrHolding == true)
                        {
                            ConnStatus = true;
                            UIContext.Send(d => ConnectionStatusTSSL.Text = "Соединение: активно", null);
                            //UIContext.Send(d => GetProgressBar.Value = (int)((IndexOfInfopacket / CountPackets) * 100), 0);
                        }
                        if (COMPort.DsrHolding == false)
                        {
                            ConnStatus = false;
                            UIContext.Send(d => ConnectionStatusTSSL.Text = "Соединение: отсутствует", null);
                        }
                        if (((RHeader == true) || (SHeader == true)) & (ConnStatus == false))
                        {
                            //Добавить сохранение индекса для восстановления передачи (но не сюда)

                            
                            UIContext.Send(d => InfoRTB.AppendText("\nВо время передачи произошла ошибка.\nПередача прервана."), null);
                            
                            RHeader = false;
                            //RData = false;
                            SHeader = false;
                            //SData = false;
                            //SaveFileStream.Close();
                            //SaveFileStream.Dispose();
                            //SendFileStream.Close();
                            //SendFileStream.Dispose();
                        }
                    }
                }
                catch (TimeoutException)
                {
                    UIContext.Send(d => InfoRTB.AppendText("\nВремя ожидания передачи/приёма сообщения истекло"), null);
                }
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.AutomatedGet = AutoApplying;
            Properties.Settings.Default.Restore = Restore;
            ConnStatus = false;
            SynchronizationThread.Abort();
            ConnectionThread.Abort();
            COMPort.Close();
        }

        private void IntToByte(uint Number, ref int index, byte [] VByte)
        {
            byte[] helparr = BitConverter.GetBytes(Number);                             //Запись во вспомогательный массив длины массива (побайтово)
            for(int i = 0; i <4;i++)                                                    //Непосредственная запись в основной массив
            {
                VByte[index] = helparr[i];
                index++;
            }
        }

        private uint ByteToInt(byte[] VByte, int index)
        {
            byte[] qq = new byte[4];                        //Вспомогательный массив для выписывания длины инфочасти
            for (int i = 0; i < 4; i++)
                qq[i] = VByte[index + i];
            uint low = 0;                                   //Переменная, в которой будет храниться значение длины инфочасти
            low = BitConverter.ToUInt32(qq, 0);
            return low;
        }

        static int CheckSize(int size)
        {
            return (int)Math.Floor(Math.Floor(((double)size - 18) * 8.0 / 15.0) * 11.0 / 8.0);
        }

        private void FileDividing()
        {
            int op = Properties.Settings.Default.OutBuffer;
            int i = CheckSize(Properties.Settings.Default.OutBuffer);
            CountPackets = byFileData.Length / i;
            bool lastpack = false;
            if (byFileData.Length % i != 0)
            {
                CountPackets++;
                lastpack = true;
            }
            Array.Resize(ref WriteData, CountPackets);
            int bytesIndex = 0;
            if (i == 0)
            {
                Code(ref byFileData);
                WriteData[0] = byFileData;
                return;
            }
            for (int IndexOfMas = 0; IndexOfMas < CountPackets; IndexOfMas++)
            {
                if (lastpack == true && IndexOfMas == CountPackets - 1)
                {
                    int lastLength = byFileData.Length - bytesIndex;
                    Array.Resize(ref WriteData[IndexOfMas], lastLength + 2);
                }
                else
                Array.Resize(ref WriteData[IndexOfMas], i);
                for (int j = 0; j < i && (bytesIndex < byFileData.Length); j++, bytesIndex++)
                    WriteData[IndexOfMas][j] = byFileData[bytesIndex];
                Code(ref WriteData[IndexOfMas]);
            }
        }

        private void NullVariablesHost()
        {
            SendedFileName = null;
            SHeader = false;
            SData = false;
            WriteData = new byte[][] { };
            IndexOfInfopacket = 0;
            MaxIndexOfInfopacket = 0;
            FinalizationStatus = 1;
            byFileData = new byte[] { };
            CountPackets = 0;
        }

        private void NullVariablesClient()
        {
            AppliedFileName = null;            
            RHeader = false;              
            RData = false;                 
            ReadData = new byte[]{};                   
            ErrorCounter = 0;               
        }

        /// <summary>
        /// Функция для упаковки информации любого вида в сообщение
        /// </summary>
        /// <param name="InfByte">Массив данных</param>
        /// <param name="Type">Тип передаваемых данных</param>
        /// <param name="Length">Длина передаваемых данных</param>
        /// <returns>Готовый к отправке пакет с данными</returns>
        private byte[] PartPacking(byte[] InfByte, char Type, uint Length)
        {
            byte[] VByte = new byte [] {};
            int index = 0;  
            switch(Type)
            {
                #region I
                case 'I':
                VByte = new byte[Length + 16];
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);   //Старт-байт
                index++;
                VByte[index] = Convert.ToByte(Type);                                                    //Тип пакета
                index++;
                IntToByte(Length,ref index,VByte);
                VByte[index] = IndexOfFile;
                index++;
                IntToByte(IndexOfInfopacket, ref index, VByte);
                IntToByte((uint)CountPackets, ref index, VByte);
                for (int j = 0; j < Length; index++, j++) //Запись в массив инфочасти
                    VByte[index] = InfByte[j];
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
                COMPort.Write(VByte, 0, VByte.Length);        //Запись на порт
                    break;
                #endregion
                #region A
                case 'A':
                VByte = new byte[8];
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);   //Старт-байт
                index++;
                VByte[index] = Convert.ToByte(Type);                                                    //Тип пакета
                index++;
                VByte[index] = IndexOfFile;
                index++;
                IntToByte(IndexOfInfopacket, ref index, VByte);
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
                COMPort.Write(VByte,0,VByte.Length);        //Запись на порт
                    break;
                #endregion
                #region H
                case 'H':
                VByte = new byte[Length + 4];
                //string lol = "djfgoprdeg sefsefg 32453htgfrdr4 -5t4-eyh-rkt43"; //Проверка занесения имени файла
                char[] FName = SendedFileName.ToCharArray();
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);   //Старт-байт
                index++;
                VByte[index] = Convert.ToByte(Type);                                                    //Тип пакета
                index++;
                VByte[index] = IndexOfFile;
                index++;
                
                byte[] lol = ANSI.GetBytes(FName);
                foreach (byte ch in lol)
                {
                    VByte[index] = ch;
                    index++;
                }
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
                COMPort.Write(VByte,0,VByte.Length);        //Запись на порт
                    break;
                #endregion
                #region F
                case 'F':
                VByte = new byte[5];
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);   //Старт-байт
                index++;
                VByte[index] = Convert.ToByte(Type);                                                    //Тип пакета
                index++;
                VByte[index] = IndexOfFile;
                index++;
                if (FinalizationStatus == 1 || FinalizationStatus == 2)
                {
                    VByte[index] = FinalizationStatus;
                    index++;
                }
                else
                {
                    VByte[index] = FinalizationStatus;
                    index++;
                    VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
                    COMPort.Write(VByte, 0, VByte.Length);
                    NullVariablesHost();
                    COMPort.RtsEnable = true;
                    FinalizationStatus = 1;
                    MessageBox.Show("Передача завершена");
                    break;
                }
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
                COMPort.Write(VByte,0,VByte.Length);        //Запись на порт
                    break;
                #endregion
                #region Y
                case 'Y':
                VByte = new byte[4];
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);   //Старт-байт
                index++;
                VByte[index] = Convert.ToByte(Type);                                                    //Тип пакета
                index++;
                VByte[index] = IndexOfFile;
                index++;
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
                COMPort.Write(VByte,0,VByte.Length);        //Запись на порт
                    break;
                #endregion
                #region N
                case 'N':
                VByte = new byte[4];
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);   //Старт-байт
                index++;
                VByte[index] = Convert.ToByte(Type);                                                    //Тип пакета
                index++;
                VByte[index] = IndexOfFile;
                index++;
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
                COMPort.Write(VByte,0,VByte.Length);        //Запись на порт
                    break;
                #endregion
                default:
                    break;
            }
            return VByte;
        }

        /// <summary>
        /// Функция, отвечающая за чтение и интерпретацию входных данных
        /// </summary>
        public void ReadingThread()
        {
            while (true)
            {
                Thread.Sleep((int)Frequency);
                //Будет работать, если порт открыт и готов к приёму
                //while (ConnStatus == true && COMPort.CtsHolding == true)
                //{
                #region Чтение данных из потока
                byte[] InfoBuffer = new byte[]{};
                Array.Resize(ref InfoBuffer, COMPort.BytesToRead);
                COMPort.Read(InfoBuffer, 0, COMPort.BytesToRead);

                #endregion
                if (InfoBuffer.Length!=0)
                {
                    #region Декодирование
                    byte[] HelpBuffer = new byte[] { };
                    if (InfoBuffer.Count() != 0) //InfoBuffer[0] == Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier) &
                    //Выполнить декодирование
                    {
                        Array.Resize(ref HelpBuffer, InfoBuffer.Count() - 2);
                        for (int i = 1; i < InfoBuffer.Count() - 1; i++)
                            HelpBuffer[i - 1] = InfoBuffer[i];
                        //Decode(ref HelpBuffer);
                    }
                    #endregion
                    #region Основная часть
                    char TypeOfPacket = Convert.ToChar(HelpBuffer[0]);
                    switch (TypeOfPacket)
                    {
                        #region Информационные пакеты
                        case 'I':
                            {
                                uint ili = ByteToInt(HelpBuffer, 1);
                                byte[] ret = new byte[HelpBuffer.Length - 14];
                                for (int hf = 14; hf < HelpBuffer.Length; hf++)
                                    ret[hf - 14] = HelpBuffer[hf];
                                Decode(ref ret);
                                if (ErrorInfo == false)
                                {
                                    uint MAX = ByteToInt(HelpBuffer,10);
                                    IndexOfInfopacket++;
                                    UIContext.Send(d => GetProgressBar.Value = (int)((IndexOfInfopacket / (double)MAX)*100),0);
                                    char[] dof = ANSI.GetChars(ret);
                                    string df = new string(dof);
                                    File.AppendAllText(AppliedFileName, df, ANSI);
                                }
                                else
                                {
                                    ErrorInfo = false;
                                }
                                //Encoding ANSI = Encoding.Default;
                                //File.WriteAllText(AppliedFileName, df, temp);
                                //SaveFileStream.Write(ret, 0, ret.Length);
                                //SaveFileStream.Close();
                                PartPacking(new byte[] { }, 'A', 0);
                            }
                            break;
                        #endregion
                        #region ACK-пакеты: отвечают за подтверждение принятия инфопакет или за запрос на повторную передачу
                        case 'A':
                            {
                                IndexOfInfopacket = ByteToInt(HelpBuffer, 2);
                                
                                UIContext.Send(d => SendProgressBar.Value = (int)((IndexOfInfopacket / (double)CountPackets)*100), 0);
                                if(IndexOfInfopacket != CountPackets)
                                    PartPacking(WriteData[IndexOfInfopacket], 'I', (uint)WriteData[IndexOfInfopacket].Length);
                                else
                                    PartPacking(new byte[] { }, 'F', 0);
                            }
                            break;
                        #endregion
                        #region HEAD-пакеты: передают инфо о названии файла
                        case 'H':
                            {
                                RHeader = true;
                                char[] met = new char[HelpBuffer.Length - 2];
                                for (int hf = 2; hf < HelpBuffer.Length; hf++)
                                    met[hf - 2] = Convert.ToChar(HelpBuffer[hf]);
                                AppliedFileName = new string(ANSI.GetChars(HelpBuffer, 2, HelpBuffer.Length - 2));
                                GetMessage(true);
                                if (Properties.Settings.Default.SequentialMode == true)
                                    SequentialHide(true);
                            }
                            break;
                        #endregion
                        #region FIN-пакеты: окончание передачи, закрытие соединения
                        case 'F':
                            {
                                if (HelpBuffer[2] == 1)
                                {
                                    FinalizationStatus = 2;
                                    PartPacking(new byte[] { },'F',0);
                                }
                                else if (HelpBuffer[2] == 2)
                                {
                                    FinalizationStatus = 3;
                                    PartPacking(new byte[] { }, 'F', 0);
                                }
                                else
                                {
                                    NullVariablesClient();
                                    COMPort.RtsEnable = true;
                                    MessageBox.Show("Передача завершена");
                                }
                            }
                            break;
                        #endregion
                        #region YES-пакеты: положительный ответ на запрос о передаче файла
                        case 'Y':
                            {
                                #region Параметры для сохранения недокачанных файлов
                                //byte FileForSending = HelpBuffer[1];
                                //if (Properties.Settings.Default.NotCompletedFiles != null)
                                //    Properties.Settings.Default.NotCompletedFiles.Add("elelel");
                                //else
                                //{
                                //    Properties.Settings.Default.NotCompletedFiles = new System.Collections.Specialized.StringCollection();
                                //    Properties.Settings.Default.NotCompletedFiles.Add("elelel");
                                //    Properties.Settings.Default.Save();
                                //}
                                //if (Properties.Settings.Default.NotCompletedFilesIDs != null)
                                //    Properties.Settings.Default.NotCompletedFilesIDs.Add(FileForSending);
                                //else
                                //{
                                //    Properties.Settings.Default.NotCompletedFilesIDs = new System.Collections.ArrayList();
                                //    Properties.Settings.Default.NotCompletedFilesIDs.Add(FileForSending);
                                //    Properties.Settings.Default.Save();
                                //}
                                #endregion
                                FileDividing();
                                PartPacking(WriteData[0], 'I', (uint)WriteData[0].Length);
                            }
                            break;
                        #endregion
                        #region NO-пакеты: отрицательный ответ на запрос о передаче файла
                        case 'N':
                            {
                                SHeader = false;
                                //Удалить всю инфу о файле из буфера и конфига
                            }
                            break;
                        #endregion
                        default:
                            break;
                    }
                    #endregion
                }
            }
        }
    }
}


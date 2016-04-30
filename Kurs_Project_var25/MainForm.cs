#define DEBUG
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
        private bool AutoApplying = false;   //Переменная для автоматического принятия файлов
        string SendedFileName;              //Имя отправляемого файла
        string AppliedFileName;             //Имя получаемого файла
        //Thread SynchronizationThread;       //Потоки для ежесекундной обработки информации, 
        Thread ConnectionThread;            //касающейся входящих данных и соединения
        FileStream SaveFileStream;          //Потоки для сохранения
        //FileStream SendFileStream;          //и отправки файлов
        SynchronizationContext UIContext;   //Вещь для синхронизации контролов в форме. Очень нужна для управления контролами из не-родных потоков
        bool ConnStatus = false;//Текущий статус порта
        bool RHeader = false;   //Получен заголовок
        bool SHeader = false;   //Отправлен заголовок
        bool RData = false;     //Получен файл
        bool SData = false;     //Отправлен файл
        byte?[] WriteData;      //Отправляемые данные
        byte?[] ReadData;       //Получаемые данные
        long Pointer;           //Указатель на текущий передаваемый пакет
        bool Accepting = false; //Подтверждение отправки
        int ErrorCounter = 0;   //Счётчик ошибок
        bool Restore = false;

        public MainForm()
        {
            InitializeComponent();
            GetMessageLabel.Visible = false;
            ApplyButton.Visible = false;
            DeclineButton.Visible = false;
            COMPort.BaudRate = Properties.Settings.Default.BaudRate;
#region Фича для переключения локальной и реальной версий программы
#if DEBUG
            if(Properties.Settings.Default.TestValue==false)
            {
                COMPort.PortName = "COM6";
                Properties.Settings.Default.TestValue = true;
                InfoRTB.AppendText("\nCOM6\n");
            }
            else
            {
                COMPort.PortName = "COM7";
                Properties.Settings.Default.TestValue = false;
                InfoRTB.AppendText("\nCOM7\n");
            }
            Properties.Settings.Default.Save();
#else
            COMPort.PortName = Properties.Settings.Default.ComName;
#endif
#endregion
            COMPort.ReadBufferSize = Properties.Settings.Default.InBuffer;
            COMPort.WriteBufferSize = Properties.Settings.Default.OutBuffer;
            COMPort.ReadTimeout = Properties.Settings.Default.ReadTimeout;
            COMPort.WriteTimeout = Properties.Settings.Default.WriteTimeout;
            COMPort.DtrEnable = true;
            //SaveFileStream.
        }

        private void ChooseFileButton_Click(object sender, EventArgs e)
        {
            ChoosePath(false);
        }

        private void SendFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                COMPort.WriteLine("Soobchenie s porta " + COMPort.PortName); //Отсылаемое сообщение
                COMPort.WriteLine("Put': " + SendPathTextBox.Text);
                //InfoRTB.AppendText("\nЖдём ответа принимающей стороны");

                ////Ждём-с

                //InfoRTB.AppendText("...Подтверждено");
                //InfoRTB.AppendText("\nНачата пересылка файла " + SendedFileName);

                ////Код для отсылки файла либо здесь, либо в потоке READ

                //InfoRTB.AppendText("...Завершено");
            }
            catch
            {
                InfoRTB.AppendText("...Ошибка");
            }
        }

        private void AuthorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Данный курсовой проект по дисциплине\n\"Сетевые технологии в АСОИУ\" сделали:\nЕгоров Алексей ИУ5-64\nТимур Мусин ИУ5-64\nВострокнутов Илья ИУ5-64", "Авторы", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Exit() == true)
            {
                this.Dispose();
                Application.Exit();
            }
        }

        private void AutoAcceptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoApplying = AutoAcceptToolStripMenuItem.Checked;
        }

        private void DeclineButton_Click(object sender, EventArgs e)
        {
            GetMessage(false);

            //Отправить на другой комп сигнал о том, что принятие файла отклонено
            SendMessage('N');
        }

        private void ChooseFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChoosePath(false);
        }

        /// <summary>
        /// Выбор файла для пересылки/Выбор имени файла и диретории для сохранения
        /// </summary>
        /// <param name="flag">true - сохранение, false - пересылка</param>
        private bool ChoosePath(bool flag)
        {
            if (flag == false)
            {
                if (SendOpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)   //Костыль для того, чтобы при нажатии "Отмена" путь к папке не "исчезал"
                {
                    SendPathTextBox.Text = SendOpenFileDialog.FileName;
                    SendedFileName = SendOpenFileDialog.SafeFileName;
                    return true;
                }
                return false;
            }
            if (AutoApplying == false && flag == true)
            {
                if (AcceptedSaveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SendPathTextBox.Text = AcceptedSaveFileDialog.FileName;
                    AppliedFileName = AcceptedSaveFileDialog.FileName;
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Скрытие контролов, отвечающих за принятие решения по поводу получаемых файлов
        /// </summary>
        /// <param name="flag">false - скрываем, true - нутыпонял</param>
        private void GetMessage(bool flag)
        {
            if (flag == false)
            {
                GetMessageLabel.Visible = false;
                ApplyButton.Visible = false;
                DeclineButton.Visible = false;
            }
            else
            {
                GetMessageLabel.Visible = true;
                ApplyButton.Visible = true;
                DeclineButton.Visible = true;
                GetMessageLabel.Text = "Получен запрос на принятие нового файла. Принять файл?\n\nИмя файла: " + AppliedFileName;
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ChoosePath(true);
            GetMessage(false);
            GetLabel.Text = "Получаемый файл: " + AppliedFileName;

            //Отправить сигнал о том, что разрешено отсылать файл
            SendMessage('Y');
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Потом напишу. По документации
        }

        /// <summary>
        /// Кодирование по алгоритму Хэмминга
        /// </summary>
        /// <param name="information">Необработанный байт-массив с данными</param>
        /// <returns>Закодированные данные</returns>
        private byte[] HammingCoding(byte[] information)
        {
            byte[] RefactoredInfo = new byte[information.Length];
            unsafe 
            {

            }
            return RefactoredInfo;
        }

        /// <summary>
        /// Декодирование по алгоритму Хэмминга
        /// </summary>
        /// <param name="information">Закодированные Хэммингом данные</param>
        /// <returns>Декодированные данные</returns>
        private byte[] HammingDecoding(byte[] information)
        {
            byte[] RefactoredInfo = new byte[information.Length];
            unsafe {

            }
            return RefactoredInfo;
        }

        public void Connect()
        {
            int i = 0;
            while (true)
            {
                Thread.Sleep(2000);
                Random rand = new Random();
                int IntRand = rand.Next(10000, 99999);
                try
                {
                    i++;
                    string g = "0-" + IntRand + "-" + i;
                    UIContext.Send(d => InfoRTB.AppendText("Сообщение отправлено: " + g), null);
                    COMPort.WriteLine(g);
                    string s = COMPort.ReadExisting();
                    UIContext.Send(d => InfoRTB.AppendText("\nСообщение принято: " + s), null);
                }
                catch (TimeoutException)
                {
                    UIContext.Send(d => InfoRTB.AppendText("\nВремя ожидания передачи/приёма сообщения истекло"), null);
                }
            }
        }

        private void NewConnect()
        {
            while (true)
            {
                try
                {
                    if ((COMPort.DsrHolding == true) & (RHeader == false) & (SHeader == false))
                    {
                        ConnStatus = true;
                        UIContext.Send(d => ConnectionStatusTSSL.Text = "Соединение: активно", null);
                    }
                    if ((COMPort.DsrHolding == false) & (RHeader == false) & (SHeader == false))
                    {
                        ConnStatus = false;
                        UIContext.Send(d => ConnectionStatusTSSL.Text = "Соединение: отсутствует", null);
                    }
                    if (((RHeader == true) || (SHeader == true)) & (ConnStatus == false))
                    {
                        SendMessage('E');
                        UIContext.Send(d => InfoRTB.AppendText("\nВо время передачи произошла ошибка.\nПередача прервана."), null);
                        RHeader = false;
                        RData = false;
                        RHeader = false;
                        SData = false;
                        //SaveFileStream.Close();
                        //SaveFileStream.Dispose();
                        //SendFileStream.Close();
                        //SendFileStream.Dispose();
                    }
                }
                catch
                {
                    //Что-то для обозначения того, что порт недоступен
                }
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AutoApplying = Properties.Settings.Default.AutomatedGet;
            Restore = Properties.Settings.Default.Restore;
            COMPort.BaudRate = Properties.Settings.Default.BaudRate;
            COMPort.Handshake = Handshake.None;
            COMPort.DtrEnable = true;
            COMPort.RtsEnable = false;
            COMPort.ReadTimeout = 500;
            COMPort.WriteTimeout = 500;
            UIContext = SynchronizationContext.Current;
            ConnectionThread = new Thread(Connect);
            //SynchronizationThread = new Thread(ReadingThread);
            COMPort.Open();
            ConnectionThread.Start();
            //SynchronizationThread.Start();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.AutomatedGet = AutoApplying;
            Properties.Settings.Default.Restore = Restore;
            ConnStatus = false;
            //SynchronizationThread.Abort();
            ConnectionThread.Abort();
            COMPort.Close();
            //foreach (Process currentProcess in Process.GetProcessesByName("Kurs_Project_var25"))
            //    currentProcess.Kill();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Exit() == true)
            {
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
                    COMPort.Close();
                    COMPort.BaudRate = Properties.Settings.Default.BaudRate;
                    COMPort.PortName = Properties.Settings.Default.ComName;
                    COMPort.ReadBufferSize = Properties.Settings.Default.InBuffer;
                    COMPort.WriteBufferSize = Properties.Settings.Default.OutBuffer;
                    COMPort.ReadTimeout = Properties.Settings.Default.ReadTimeout;
                    COMPort.WriteTimeout = Properties.Settings.Default.WriteTimeout;
                    COMPort.Open();
                }
            }
        }

        private void SendMessage(char TypeOfMessage)
        {
            COMPort.RtsEnable = false;
            if (TypeOfMessage == 'I')
                COMPort.Write(PartPacking(WriteData, TypeOfMessage, WriteData.Length), 0, PartPacking(WriteData, TypeOfMessage, WriteData.Length).Length);
            else if (TypeOfMessage == 'A') { }
            //COMPort.Write(PartPacking(ToByteMas(SendedFileName), TypeOfMessage, SendedFileName.Length * 8), 0, ??);
            else
                COMPort.Write(PartPacking(null, TypeOfMessage, 0), 0, PartPacking(null, TypeOfMessage, 0).Length);
            COMPort.RtsEnable = true;
        }

        /// <summary>
        /// Функция для упаковки информации любого вида в сообщение
        /// </summary>
        /// <param name="InfByte">Массив данных</param>
        /// <param name="Type">Тип передаваемых данных</param>
        /// <param name="Length">Длина передаваемых данных</param>
        /// <returns>Готовый к отправке пакет с данными</returns>
        private byte[] PartPacking(byte?[] InfByte, char Type, long Length)
        {
            byte[] VByte;
            if (InfByte != null)
            {
                VByte = new byte[Length + 4];
                VByte[0] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);           //Старт-байт
                VByte[1] = Convert.ToByte(Type);
                VByte[2] = Convert.ToByte(Length);
                for (int i = 3, j = 0; i < Length + 3; i++, j++)
                    VByte[i] = (byte)InfByte[j];
                VByte[Length + 3] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
            }
            else
            {
                VByte = new byte[3];
                VByte[0] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Старт-байт
                VByte[1] = Convert.ToByte(Type);
                VByte[2] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
            }
            return VByte;
        }

        /// <summary>
        /// Перевод строки string в байт-массив. Доказано, что не работает
        /// </summary>
        /// <param name="InputString">Строка для перевода</param>
        /// <returns>Готовый байт-массив</returns>
        private byte[] ToByteMas(string InputString)
        {
            byte[] RetByteMas = new byte[InputString.Count()];
            //for (int i = 0; i < InputString.Count(); i++)
            //    RetByteMas[i] = Convert.ToByte(InputString.Substring(i * 8, 8), 2); //Не думаю, что будет работать
            return RetByteMas;
        }


        /// <summary>
        /// Функция, отвечающая за чтение и интерпретацию входных данных
        /// Пока не работает, надо разобрать
        /// </summary>
        public void ReadingThread()
        {
            while (true)
            {
                //Будет работать, если порт открыт и готов к приёму
                //while (ConnStatus == true && COMPort.CtsHolding == true)
                //{
                #region Чтение данных из потока
                //string HeaderInfo = "";
                string InputMessage = "";
                string TempMessage = "";
                for (int i = 0; COMPort.BytesToRead > 0; i++)
                {
                    TempMessage = Convert.ToString(COMPort.ReadByte(), 2);  //Перевод в двоичную СС
                    //Если в Temp меньше байта, то оставшееся до восьми заполняем нулями
                    if (TempMessage.Count() < 8)         
                    {
                        for (int j = 0; TempMessage.Count() < 8; j++)
                            TempMessage = "0" + TempMessage;
                    }
                    InputMessage += TempMessage; //Конечное принятое сообщение, с которым работаем далее
                    Thread.Sleep(30);
                }
                #endregion
                            if (InputMessage.Count() > 0)    //Если что-то обнаружено, то выполняем
                            {
                                #region Ошибка
                                if (!true) //Вместо !true указать функцию проверки ошибок - иначе проверки не будет
                                //Здесь обрабатывается событие, при котором наступает большое количество ошибок
                                {
                                    ErrorCounter++;
                                    if (ErrorCounter >= 5)
                                    {
                                        InfoRTB.AppendText("\nВо время передачи произошла ошибка.\nПередача прервана.");
                                        RHeader = false;
                                        RData = false;
                                        RHeader = false;
                                        SData = false;
                                        //SendFileStream.Close();
                                        //SendFileStream.Dispose();
                                    }
                                    SendMessage('E');
                                }
                                #endregion
                                else //Тут происходит основное действие
                                {
                                    byte[] DekMes = ToByteMas(InputMessage);
                                    #region Обработка ACK
                                    if (DekMes[1] == Convert.ToByte('A'))
                                    {
                                        ErrorCounter = 0;
                                        COMPort.RtsEnable = false;
                                        #region Всё передано
                                        if ((SHeader == true) & (SData == true) & (Accepting == false))
                                        {
                                            #region Сброс флагов
                                            RHeader = false;
                                            RData = false;
                                            SHeader = false;
                                            SData = false;
                                            #endregion
                                            #region Оповещение и закрытие файлового потока
                                            InfoRTB.AppendText("\nФайл передан!");
                                            SaveFileStream.Close();
                                            SaveFileStream.Dispose();
                                            #endregion
                                        }
                                        #endregion
                                        #region Заголовок передан, подтверждение есть, данные не переданы
                                        if ((SHeader == true) & (SData == false) & (Accepting == true))
                                        {
                                            //UIContext.Send(d => progressBar1.Value++, null); //Прогрессбар - заполнение
                                            Pointer = SaveFileStream.Position;
                                            long k = SaveFileStream.Length - Pointer;
                                            if (k > 0)
                                            {
                                                byte[] InformationMas;
                                                //if (k > 50)
                                                //{
                                                //    InformationMas = new byte[50];
                                                //    for (int i = 0; i < 50; i++)
                                                //        InformationMas[i] = Convert.ToByte(SaveFileStream.ReadByte());
                                                //}
                                                //else
                                                //{
                                                InformationMas = new byte[k];
                                                for (int i = 0; i < k; i++)
                                                    InformationMas[i] = Convert.ToByte(SaveFileStream.ReadByte());
                                                //}
                                                byte[] telegram = null; //Кодирование и помещение в пакеты - обязательно
                                                //= Kodir(Upakovat(InformationMas, 'I', InformationMas.Count()), 4, "1011");
                                                #region Запрос на передачу выключен, флаг установки приёма в true
                                                COMPort.RtsEnable = false;
                                                Accepting = true;
                                                #endregion
                                                COMPort.Write(telegram, 0, telegram.Count());
                                                Thread.Sleep(100);
                                                COMPort.RtsEnable = true;
                                            }
                                            #region Отправка сообщения о конце передачи
                                            else
                                            {
                                                SData = true;
                                                Accepting = false;
                                                SendMessage('E');
                                            }
                                            #endregion
                                        }
                                        #endregion
                                        #region Заголовок передан, подтверждения нет
                                        if ((SHeader == false) & (Accepting == true))
                                            SHeader = true;
                                        #endregion
                                    #endregion
                                        #region Обработка отрицательной квитанции (из другого проекта)
                                        //if (DekMes[1] == Convert.ToByte('R'))
                                        //{
                                        //    ErrorCounter++;
                                        //    #region Если очень много ошибок
                                        //    if (ErrorCounter >= 5)
                                        //    {
                                        //        InfoRTB.AppendText("\nВо время передачи возникла ошибка!\nПередача прервана!");
                                        //        RHeader = false;
                                        //        RData = false;
                                        //        SHeader = false;
                                        //        SData = false;
                                        //        ErrorCounter = 0;
                                        //        SaveFileStream.Close();
                                        //        SaveFileStream.Dispose();
                                        //    }
                                        //    #endregion
                                        //    #region Хз, потом разберусь
                                        //    else
                                        //    {
                                        //        if (SHeader == true)
                                        //        {
                                        //            byte[] inf;
                                        //            long k = SaveFileStream.Length - Pointer;
                                        //            SaveFileStream.Position = Pointer;
                                        //            //if (k > 50)
                                        //            //{
                                        //            //    inf = new byte[50];
                                        //            //    for (int i = 0; i < 50; i++)
                                        //            //        inf[i] = Convert.ToByte(SaveFileStream.ReadByte());
                                        //            //}
                                        //            //else
                                        //            //{
                                        //            try
                                        //            {
                                        //                inf = new byte[k];
                                        //                for (int i = 0; i < k; i++)
                                        //                    inf[i] = Convert.ToByte(SaveFileStream.ReadByte());
                                        //                //}
                                        //                byte[] telegram; //Кодирование информации Хэммингом
                                        //                //= Kodir(Upakovat(inf, 'I', inf.Count()), 4, "1011");
                                        //                COMPort.RtsEnable = false;
                                        //                Accepting = true;
                                        //                COMPort.Write(telegram, 0, telegram.Count());
                                        //                Thread.Sleep(100);
                                        //                COMPort.RtsEnable = true;
                                        //            }
                                        //            catch
                                        //            {
                                        //                InfoRTB.AppendText("\nERROR");
                                        //            }
                                        //        }
                                        //        #region Отправка имени файла
                                        //        else
                                        //        {
                                        //            string FileName = SendedFileName;
                                        //            byte[] Zagolovok = new byte[FileName.Count()];
                                        //            for (int i = 0; i < FileName.Count(); i++)
                                        //            {
                                        //                Zagolovok[i] = Convert.ToByte(FileName[i]);
                                        //            }
                                        //            byte[] telegram; //Кодирование и помещение в пакеты - обязательно
                                        //            //= Kodir(Upakovat(Zagolovok, 'I', Zagolovok.Count()), 4, "1011");
                                        //            COMPort.RtsEnable = false;
                                        //            Accepting = true;
                                        //            COMPort.Write(telegram, 0, telegram.Count());
                                        //            Thread.Sleep(100);
                                        //            COMPort.RtsEnable = true;
                                        //        }
                                        //        #endregion
                                        //    }
                                        //    #endregion
                                        //}
                                        #endregion
                                        #region Обработка ответа ДА
                                        if (DekMes[1] == Convert.ToByte('Y'))
                                        {
                                            //UIContext.Send(d => button1.IsEnabled = false, null);
                                            //UIContext.Send(d => MenuItem_Action.IsEnabled = false, null);

                                            UIContext.Send(d => InfoRTB.AppendText("YES"), null);

                                            //byte[] inf;
                                            //Pointer = SaveFileStream.Position;
                                            //long k = SaveFileStream.Length - Pointer;
                                            //if (k > 50)
                                            //{
                                            //    inf = new byte[50];
                                            //    for (int i = 0; i < 50; i++)
                                            //    {
                                            //        inf[i] = Convert.ToByte(SaveFileStream.ReadByte());
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    inf = new byte[k];
                                            //    for (int i = 0; i < k; i++)
                                            //    {
                                            //        inf[i] = Convert.ToByte(SaveFileStream.ReadByte());
                                            //    }
                                            //}
                                            //Accepting = false;
                                            //byte[] telegram = null; //Кодирование и помещение в пакеты - обязательно
                                            //// = Kodir(Upakovat(inf, 'I', inf.Count()), 4, "1011");
                                            //COMPort.RtsEnable = false;
                                            //Accepting = true;
                                            //COMPort.Write(telegram, 0, telegram.Count());
                                            //Thread.Sleep(100);
                                            //COMPort.RtsEnable = true;
                                        }
                                        #endregion
                                        #region Обработка ответа НЕТ
                                        if (DekMes[1] == Convert.ToByte('N'))
                                        {
                                            InfoRTB.AppendText("\nПринимающая сторона отказывается принимать файл!");
                                            SaveFileStream.Close();
                                            SaveFileStream.Dispose();
                                            RHeader = false;
                                            RData = false;
                                            SHeader = false;
                                            SData = false;
                                        }
                                        #endregion
                                        #region Обработка конца передачи
                                        if (DekMes[1] == Convert.ToByte('E'))
                                        {
                                            RData = true;
                                            //UIContext.Send(d => label4.Visibility = Visibility.Hidden, null);
                                            //UIContext.Send(d => MenuItem_Action.IsEnabled = true, null);
                                            //UIContext.Send(d => button1.IsEnabled = true, null);
                                            InfoRTB.AppendText("\nФайл принят!");
                                            //ACK();
                                            RHeader = false;
                                            RData = false;
                                            SHeader = false;
                                            SData = false;
                                            //SendFileStream.Close();
                                            //SendFileStream.Dispose();
                                        }
                                        #endregion
                                        #region Обработка информационной части
                                        if (DekMes[1] == Convert.ToByte('I'))
                                        {
                                            if (DekMes.Count() == DekMes[2] + 4)
                                            {
                                                #region В случае, если заголовка не было
                                                if (RHeader == false)
                                                {
                                                    for (int i = 0; i < Convert.ToInt32(DekMes[2]); i++)
                                                        //HeaderInfo = HeaderInfo + Convert.ToChar(DekMes[3 + i]);
                                                    RHeader = true;
                                                    //ACK();
                                                    //if (MessageBox.Show("Принять файл " + HeaderInfo + "?", "Согласие на передачу", MessageBoxButtons.YesNo) == MessageBoxButton.Yes)
                                                    //{
                                                    //    if (Sohranenie(HeaderInfo) == true)
                                                    //    {
                                                    //        RHeader = true;
                                                    //        //UIContext.Send(d => button1.IsEnabled = false, null);
                                                    //        //UIContext.Send(d => MenuItem_Action.IsEnabled = false, null);
                                                    //        //UIContext.Send(d => label4.Visibility = Visibility.Visible, null);
                                                    //        //YES();
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        //NO();
                                                    //        RHeader = false;
                                                    //        RData = false;
                                                    //        SHeader = false;
                                                    //        SData = false;
                                                    //    }
                                                    //}
                                                    //else
                                                    //{
                                                    //    //NO();
                                                    //    RHeader = false;
                                                    //    RData = false;
                                                    //    SHeader = false;
                                                    //    SData = false;
                                                    //}
                                                }
                                                #endregion
                                                #region В случае, если заголовок был
                                                else
                                                {
                                                    //ACK();
                                                    //SendFileStream.Write(DekMes, 3, Convert.ToInt32(DekMes[2]));
                                                }
                                                #endregion
                                            }
                                            #region Ошибка
                                            else
                                            {
                                                ErrorCounter++;
                                                if (ErrorCounter >= 5)
                                                {
                                                    InfoRTB.AppendText("\nВо время передачи возникла ошибка!\nПередача прервана!");
                                                    RHeader = false;
                                                    RData = false;
                                                    SHeader = false;
                                                    SData = false;
                                                    ErrorCounter = 0;
                                                    //SendFileStream.Close();
                                                    //SendFileStream.Dispose();
                                                }
                                                //NAK();
                                            }
                                            #endregion
                                        }
                                        #endregion
                                    }
                                }
                            }
                      }
            }

        private void RestoreLostPacketsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Restore = RestoreLostPacketsToolStripMenuItem.Checked;
        }
    }
}


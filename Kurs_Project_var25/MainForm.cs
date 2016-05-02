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
        Thread SynchronizationThread;       //Потоки для ежесекундной обработки информации, 
        Thread ConnectionThread;            //касающейся входящих данных и соединения
        FileStream SaveFileStream;          //Потоки для сохранения
        FileStream SendFileStream;          //и отправки файлов
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
                COMPort.PortName = "COM16";
                Properties.Settings.Default.TestValue = true;
                InfoRTB.AppendText("\n"+ COMPort.PortName +"\n");
            }
            else
            {
                COMPort.PortName = "COM17";
                Properties.Settings.Default.TestValue = false;
                InfoRTB.AppendText("\n" + COMPort.PortName + "\n");
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
            COMPort.RtsEnable = false;
            COMPort.Handshake = Handshake.None;
            //Restore = Properties.Settings.Default.Restore;
            //COMPort.BaudRate = Properties.Settings.Default.BaudRate;
            //COMPort.ReadTimeout = 500;
            //COMPort.WriteTimeout = 500;
            AutoApplying = Properties.Settings.Default.AutomatedGet;
            UIContext = SynchronizationContext.Current;
            ConnectionThread = new Thread(Connect);
            SynchronizationThread = new Thread(ReadingThread);
            COMPort.Open();
            ConnectionThread.Start();
            SynchronizationThread.Start();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }


        #region Всё, что вряд ли будет меняться
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
        #endregion

        private void SendFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                //COMPort.WriteLine("Soobchenie s porta " + COMPort.PortName); //Отсылаемое сообщение
                //COMPort.WriteLine("Put': " + SendPathTextBox.Text);
                InfoRTB.AppendText("\nЖдём ответа принимающей стороны");

                //Ждём-с
                //while(true)
                //{
                    //Thread.Sleep(200);
                    //if (COMPort.CtsHolding == true)
                    //{
                        InfoRTB.AppendText("...Подтверждено");

                        //Отослать первый пакет
                        byte [] newbyte = new byte [] {};
                        InfoRTB.AppendText("\nНачата пересылка файла " + SendedFileName);
                        //COMPort.Write(newbyte,0,1);
                        //break;
                    //}
                //}
                //InfoRTB.AppendText("...Завершено");
            }
            catch
            {
                InfoRTB.AppendText("...Ошибка");
            }
        }

        private void DeclineButton_Click(object sender, EventArgs e)
        {
            GetMessage(false);

            //Отправить на другой комп сигнал о том, что принятие файла отклонено
            SendMessage('N');
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

        /// <summary>
        /// Кодирование по алгоритму Хэмминга
        /// </summary>
        /// <param name="information">Необработанный байт-массив с данными</param>
        static byte[] HammingCoding(byte[] information, bool flag)
        {
            //byte[] msg = new byte[] {}; //Вспомогательный массив для того, чтобы не затирать старый
            if(flag == false)
                Code(ref information);
            else
                Decode(ref information);
            return information;
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
                    Console.WriteLine("ERROR! Error byte:" + i);
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


        //public void oldConnect()
        //{
        //    int i = 0;
        //    while (true)
        //    {
        //        Thread.Sleep(2000);
        //        Random rand = new Random();
        //        int IntRand = rand.Next(10000, 99999);
        //        try
        //        {
        //            i++;
        //            string g = "0-" + IntRand + "-" + i;
        //            UIContext.Send(d => InfoRTB.AppendText("Сообщение отправлено: " + g), null);
        //            COMPort.WriteLine(g);
        //            string s = COMPort.ReadExisting();
        //            UIContext.Send(d => InfoRTB.AppendText("\nСообщение принято: " + s), null);
        //        }
        //        catch (TimeoutException)
        //        {
        //            UIContext.Send(d => InfoRTB.AppendText("\nВремя ожидания передачи/приёма сообщения истекло"), null);
        //        }
        //    }
        //}

        /// <summary>
        /// Функция, принимаемая потоком. Служит для соединения двух компьютеров и обозначения текущего статуса соединения
        /// </summary>
        private void Connect()
        {
            while (true)
            {
                Thread.Sleep(2000);
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

                        //Добавить сохранение индекса для восстановления передачи

                        //SendMessage('E');
                        UIContext.Send(d => InfoRTB.AppendText("\nВо время передачи произошла ошибка.\nПередача прервана."), null);
                        //RHeader = false;
                        //RData = false;
                        //RHeader = false;
                        //SData = false;
                        //SaveFileStream.Close();
                        //SaveFileStream.Dispose();
                        //SendFileStream.Close();
                        //SendFileStream.Dispose();
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

        private void SendMessage(char TypeOfMessage)
        {

            #region
            //COMPort.RtsEnable = false;
            //if (TypeOfMessage == 'I')
            //    COMPort.Write(PartPacking(WriteData, TypeOfMessage, WriteData.Length), 0, PartPacking(WriteData, TypeOfMessage, WriteData.Length).Length);
            //else if (TypeOfMessage == 'A') { }
            //COMPort.Write(PartPacking(ToByteMas(SendedFileName), TypeOfMessage, SendedFileName.Length * 8), 0, ??);
            //else
            //    COMPort.Write(PartPacking(null, TypeOfMessage, 0), 0, PartPacking(null, TypeOfMessage, 0).Length);
            //COMPort.RtsEnable = true;
            #endregion
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
            byte ID_File = 154;       //Идентификатор для файла
            uint ID_Package = 546679653;   //Идентификатор для пакета
            switch(Type)
            {
                case 'I':
                VByte = new byte[Length + 12];
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);   //Старт-байт
                index++;
                VByte[index] = Convert.ToByte(Type);                                                    //Тип пакета
                index++;
                IntToByte(Length,ref index,VByte);
                //byte [] helparr = BitConverter.GetBytes(Length);                                    //Запись во вспомогательный массив длины массива (побайтово)
                //int o=2;
                //foreach (byte h in helparr)                                                         //Непосредственная запись в основной массив
                //{
                //    VByte[o] = h;
                //    o++;
                //}
                //#region Преоразование обратно в int
                //byte[] qq = new byte[4];                                                            //Вспомогательный массив для выписывания длины инфочасти
                //for (int i = 0; i < 4; i++)
                //    qq[i] = VByte[2 + i];
                //uint low = 0;                       //Переменная, в которой будет храниться значение длины инфочасти
                //low=BitConverter.ToUInt32(qq,0);
                //#endregion
                VByte[index] = ID_File;
                index++;
                IntToByte(ID_Package, ref index, VByte);
                uint tr = ByteToInt(VByte,7);
                for (int j = 0; index < Length + 3; index++, j++) //Запись в массив инфочасти
                    VByte[index] = InfByte[j];
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
                COMPort.Write(VByte,0,VByte.Length);        //Запись на порт
                    break;

                case 'A':
                VByte = new byte[8];
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);   //Старт-байт
                index++;
                VByte[index] = Convert.ToByte(Type);                                                    //Тип пакета
                index++;
                VByte[index] = ID_File;
                index++;
                IntToByte(ID_Package, ref index, VByte);
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
                COMPort.Write(VByte,0,VByte.Length);        //Запись на порт
                    break;

                case 'H':

                    break;

                case 'F':

                    break;

                case 'Y':
                VByte = new byte[4];
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);   //Старт-байт
                index++;
                VByte[index] = Convert.ToByte(Type);                                                    //Тип пакета
                index++;
                VByte[index] = ID_File;
                index++;
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
                COMPort.Write(VByte,0,VByte.Length);        //Запись на порт
                    break;

                case 'N':
                                    VByte = new byte[4];
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);   //Старт-байт
                index++;
                VByte[index] = Convert.ToByte(Type);                                                    //Тип пакета
                index++;
                VByte[index] = ID_File;
                index++;
                VByte[index] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
                COMPort.Write(VByte,0,VByte.Length);        //Запись на порт
                    break;

                default:
                    break;
            }
            #region
            //if (InfByte != null)
            //{
            //    VByte = new byte[Length + 4];
            //    VByte[0] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);           //Старт-байт
            //    VByte[1] = Convert.ToByte(Type);
            //    VByte[2] = Convert.ToByte(Length);
            //    for (int i = 3, j = 0; i < Length + 3; i++, j++)
            //        VByte[i] = (byte)InfByte[j];
            //    VByte[Length + 3] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
            //}
            //else
            //{
            //    VByte = new byte[3];
            //    VByte[0] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Старт-байт
            //    VByte[1] = Convert.ToByte(Type);
            //    VByte[2] = Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier);  //Стоп-байт
            //}
            #endregion
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
                //string InputMessage = "";
                //string TempMessage = "";
                byte[] InfoBuffer = new byte[25999];
                PartPacking(InfoBuffer, 'Y', Convert.ToUInt32(InfoBuffer.Length));
                //COMPort.Read(InfoBuffer, 0, COMPort.BytesToRead);

                //for (int i = 0; COMPort.BytesToRead > 0; i++)
                //{
                //    //TempMessage = Convert.ToString(COMPort.ReadByte(), 2);  //Перевод в двоичную СС
                //    ////Если в Temp меньше байта, то оставшееся до восьми заполняем нулями
                //    //if (TempMessage.Count() < 8)         
                //    //{
                //    //    for (int j = 0; TempMessage.Count() < 8; j++)
                //    //        TempMessage = "0" + TempMessage;
                //    //}
                //    //InputMessage += TempMessage; //Конечное принятое сообщение, с которым работаем далее
                //    //Thread.Sleep(30);
                //}
                #endregion
                #region Декодирование
                byte[] HelpBuffer = new byte[] { };
                if (InfoBuffer[0] == Byte.Parse("FF", System.Globalization.NumberStyles.AllowHexSpecifier) & InfoBuffer.Count() != 0)
                //Выполнить декодирование
                {
                    Array.Resize(ref HelpBuffer, InfoBuffer.Count() - 2);
                    for (int i = 1; i < InfoBuffer.Count()-1; i++)
                        HelpBuffer[i - 1] = InfoBuffer[i];
                    //Decode(ref HelpBuffer);
                }
                #endregion
                #region Основная часть
                char TypeOfPacket = Convert.ToChar(HelpBuffer[0]);
                switch(TypeOfPacket)
                {
                    #region Информационные пакеты
                    case 'I':
                        {

                        }
                        break;
                    #endregion
                    #region ACK-пакеты: отвечают за подтверждение принятия инфопакет или за запрос на повторную передачу
                    case 'A':
                        {

                        }
                        break;
                    #endregion
                    #region HEAD-пакеты: передают инфо о названии файла
                    case 'H':
                        {

                        }
                        break;
                    #endregion
                    #region FIN-пакеты: окончание передачи, закрытие соединения
                    case 'F':
                        {

                        }
                        break;
                    #endregion
                    #region YES-пакеты: положительный ответ на запрос о передаче файла
                    case 'Y':
                        {

                        }
                        break;
                    #endregion
                    #region NO-пакеты: отрицательный ответ на запрос о передаче файла
                    case 'N':
                        {

                        }
                        break;
                    #endregion
                    default:
                        break;
                }
                #endregion
                #region Прошлая версия
                //if (InputMessage.Count() > 0)    //Если что-то обнаружено, то выполняем
                            //{
                            //    #region Ошибка
                            //    if (!true) //Вместо !true указать функцию проверки ошибок - иначе проверки не будет
                            //    //Здесь обрабатывается событие, при котором наступает большое количество ошибок
                            //    {
                            //        ErrorCounter++;
                            //        if (ErrorCounter >= 5)
                            //        {
                            //            InfoRTB.AppendText("\nВо время передачи произошла ошибка.\nПередача прервана.");
                            //            RHeader = false;
                            //            RData = false;
                            //            RHeader = false;
                            //            SData = false;
                            //            //SendFileStream.Close();
                            //            //SendFileStream.Dispose();
                            //        }
                            //        SendMessage('E');
                            //    }
                            //    #endregion
                            //    else //Тут происходит основное действие
                            //    {
                            //        byte[] DekMes = ToByteMas(InputMessage);
                            //        #region Обработка ACK
                            //        if (DekMes[1] == Convert.ToByte('A'))
                            //        {
                            //            ErrorCounter = 0;
                            //            COMPort.RtsEnable = false;
                            //            #region Всё передано
                            //            if ((SHeader == true) & (SData == true) & (Accepting == false))
                            //            {
                            //                #region Сброс флагов
                            //                RHeader = false;
                            //                RData = false;
                            //                SHeader = false;
                            //                SData = false;
                            //                #endregion
                            //                #region Оповещение и закрытие файлового потока
                            //                InfoRTB.AppendText("\nФайл передан!");
                            //                SaveFileStream.Close();
                            //                SaveFileStream.Dispose();
                            //                #endregion
                            //            }
                            //            #endregion
                            //            #region Заголовок передан, подтверждение есть, данные не переданы
                            //            if ((SHeader == true) & (SData == false) & (Accepting == true))
                            //            {
                            //                //UIContext.Send(d => progressBar1.Value++, null); //Прогрессбар - заполнение
                            //                Pointer = SaveFileStream.Position;
                            //                long k = SaveFileStream.Length - Pointer;
                            //                if (k > 0)
                            //                {
                            //                    byte[] InformationMas;
                            //                    //if (k > 50)
                            //                    //{
                            //                    //    InformationMas = new byte[50];
                            //                    //    for (int i = 0; i < 50; i++)
                            //                    //        InformationMas[i] = Convert.ToByte(SaveFileStream.ReadByte());
                            //                    //}
                            //                    //else
                            //                    //{
                            //                    InformationMas = new byte[k];
                            //                    for (int i = 0; i < k; i++)
                            //                        InformationMas[i] = Convert.ToByte(SaveFileStream.ReadByte());
                            //                    //}
                            //                    byte[] telegram = null; //Кодирование и помещение в пакеты - обязательно
                            //                    //= Kodir(Upakovat(InformationMas, 'I', InformationMas.Count()), 4, "1011");
                            //                    #region Запрос на передачу выключен, флаг установки приёма в true
                            //                    COMPort.RtsEnable = false;
                            //                    Accepting = true;
                            //                    #endregion
                            //                    COMPort.Write(telegram, 0, telegram.Count());
                            //                    Thread.Sleep(100);
                            //                    COMPort.RtsEnable = true;
                            //                }
                            //                #region Отправка сообщения о конце передачи
                            //                else
                            //                {
                            //                    SData = true;
                            //                    Accepting = false;
                            //                    SendMessage('E');
                            //                }
                            //                #endregion
                            //            }
                            //            #endregion
                            //            #region Заголовок передан, подтверждения нет
                            //            if ((SHeader == false) & (Accepting == true))
                            //                SHeader = true;
                            //            #endregion
                            //        #endregion
                            //            #region Обработка RESET
                            //            if (DekMes[1] == Convert.ToByte('R'))
                            //            {
                            //                ErrorCounter++;
                            //                #region Если очень много ошибок
                            //                if (ErrorCounter >= 5)
                            //                {
                            //                    InfoRTB.AppendText("\nВо время передачи возникла ошибка!\nПередача прервана!");
                            //                    RHeader = false;
                            //                    RData = false;
                            //                    SHeader = false;
                            //                    SData = false;
                            //                    ErrorCounter = 0;
                            //                    SaveFileStream.Close();
                            //                    SaveFileStream.Dispose();
                            //                }
                            //                #endregion
                            //                #region Хз, потом разберусь
                            //                else
                            //                {
                            //                    if (SHeader == true)
                            //                    {
                            //                        byte[] inf;
                            //                        long k = SaveFileStream.Length - Pointer;
                            //                        SaveFileStream.Position = Pointer;
                            //                        //if (k > 50)
                            //                        //{
                            //                        //    inf = new byte[50];
                            //                        //    for (int i = 0; i < 50; i++)
                            //                        //        inf[i] = Convert.ToByte(SaveFileStream.ReadByte());
                            //                        //}
                            //                        //else
                            //                        //{
                            //                        try
                            //                        {
                            //                            inf = new byte[k];
                            //                            for (int i = 0; i < k; i++)
                            //                                inf[i] = Convert.ToByte(SaveFileStream.ReadByte());
                            //                            //}
                            //                            byte[] telegram; //Кодирование информации Хэммингом
                            //                            //= Kodir(Upakovat(inf, 'I', inf.Count()), 4, "1011");
                            //                            COMPort.RtsEnable = false;
                            //                            Accepting = true;
                            //                            COMPort.Write(InfoBuffer, 0, InfoBuffer.Count());
                            //                            Thread.Sleep(100);
                            //                            COMPort.RtsEnable = true;
                            //                        }
                            //                        catch
                            //                        {
                            //                            InfoRTB.AppendText("\nERROR");
                            //                        }
                            //                    }
                            //                    #region Отправка имени файла
                            //                    else
                            //                    {
                            //                        string FileName = SendedFileName;
                            //                        byte[] Zagolovok = new byte[FileName.Count()];
                            //                        for (int i = 0; i < FileName.Count(); i++)
                            //                        {
                            //                            Zagolovok[i] = Convert.ToByte(FileName[i]);
                            //                        }
                            //                        byte[] telegram; //Кодирование и помещение в пакеты - обязательно
                            //                        //= Kodir(Upakovat(Zagolovok, 'I', Zagolovok.Count()), 4, "1011");
                            //                        COMPort.RtsEnable = false;
                            //                        Accepting = true;
                            //                        COMPort.Write(InfoBuffer, 0, InfoBuffer.Count());
                            //                        Thread.Sleep(100);
                            //                        COMPort.RtsEnable = true;
                            //                    }
                            //                    #endregion
                            //                }
                            //                #endregion
                            //            }
                            //            #endregion
                            //            #region Обработка ответа ДА
                            //            if (DekMes[1] == Convert.ToByte('Y'))
                            //            {
                            //                //UIContext.Send(d => button1.IsEnabled = false, null);
                            //                //UIContext.Send(d => MenuItem_Action.IsEnabled = false, null);
                            //                UIContext.Send(d => InfoRTB.AppendText("YES"), null);
                            //                //byte[] inf;
                            //                //Pointer = SaveFileStream.Position;
                            //                //long k = SaveFileStream.Length - Pointer;
                            //                //if (k > 50)
                            //                //{
                            //                //    inf = new byte[50];
                            //                //    for (int i = 0; i < 50; i++)
                            //                //    {
                            //                //        inf[i] = Convert.ToByte(SaveFileStream.ReadByte());
                            //                //    }
                            //                //}
                            //                //else
                            //                //{
                            //                //    inf = new byte[k];
                            //                //    for (int i = 0; i < k; i++)
                            //                //    {
                            //                //        inf[i] = Convert.ToByte(SaveFileStream.ReadByte());
                            //                //    }
                            //                //}
                            //                //Accepting = false;
                            //                //byte[] telegram = null; //Кодирование и помещение в пакеты - обязательно
                            //                //// = Kodir(Upakovat(inf, 'I', inf.Count()), 4, "1011");
                            //                //COMPort.RtsEnable = false;
                            //                //Accepting = true;
                            //                //COMPort.Write(telegram, 0, telegram.Count());
                            //                //Thread.Sleep(100);
                            //                //COMPort.RtsEnable = true;
                            //            }
                            //            #endregion
                            //            #region Обработка ответа НЕТ
                            //            if (DekMes[1] == Convert.ToByte('N'))
                            //            {
                            //                InfoRTB.AppendText("\nПринимающая сторона отказывается принимать файл!");
                            //                SaveFileStream.Close();
                            //                SaveFileStream.Dispose();
                            //                RHeader = false;
                            //                RData = false;
                            //                SHeader = false;
                            //                SData = false;
                            //            }
                            //            #endregion
                            //            #region Обработка конца передачи
                            //            if (DekMes[1] == Convert.ToByte('E'))
                            //            {
                            //                RData = true;
                            //                //UIContext.Send(d => label4.Visibility = Visibility.Hidden, null);
                            //                //UIContext.Send(d => MenuItem_Action.IsEnabled = true, null);
                            //                //UIContext.Send(d => button1.IsEnabled = true, null);
                            //                InfoRTB.AppendText("\nФайл принят!");
                            //                //ACK();
                            //                RHeader = false;
                            //                RData = false;
                            //                SHeader = false;
                            //                SData = false;
                            //                //SendFileStream.Close();
                            //                //SendFileStream.Dispose();
                            //            }
                            //            #endregion
                            //            #region Обработка информационной части
                            //            if (DekMes[1] == Convert.ToByte('I'))
                            //            {
                            //                if (DekMes.Count() == DekMes[2] + 4)
                            //                {
                            //                    #region В случае, если заголовка не было
                            //                    if (RHeader == false)
                            //                    {
                            //                        for (int i = 0; i < Convert.ToInt32(DekMes[2]); i++)
                            //                            //HeaderInfo = HeaderInfo + Convert.ToChar(DekMes[3 + i]);
                            //                        RHeader = true;
                            //                        //ACK();
                            //                        //if (MessageBox.Show("Принять файл " + HeaderInfo + "?", "Согласие на передачу", MessageBoxButtons.YesNo) == MessageBoxButton.Yes)
                            //                        //{
                            //                        //    if (Sohranenie(HeaderInfo) == true)
                            //                        //    {
                            //                        //        RHeader = true;
                            //                        //        //UIContext.Send(d => button1.IsEnabled = false, null);
                            //                        //        //UIContext.Send(d => MenuItem_Action.IsEnabled = false, null);
                            //                        //        //UIContext.Send(d => label4.Visibility = Visibility.Visible, null);
                            //                        //        //YES();
                            //                        //    }
                            //                        //    else
                            //                        //    {
                            //                        //        //NO();
                            //                        //        RHeader = false;
                            //                        //        RData = false;
                            //                        //        SHeader = false;
                            //                        //        SData = false;
                            //                        //    }
                            //                        //}
                            //                        //else
                            //                        //{
                            //                        //    //NO();
                            //                        //    RHeader = false;
                            //                        //    RData = false;
                            //                        //    SHeader = false;
                            //                        //    SData = false;
                            //                        //}
                            //                    }
                            //                    #endregion
                            //                    #region В случае, если заголовок был
                            //                    else
                            //                    {
                            //                        //ACK();
                            //                        //SendFileStream.Write(DekMes, 3, Convert.ToInt32(DekMes[2]));
                            //                    }
                            //                    #endregion
                            //                }
                            //                #region Ошибка
                            //                else
                            //                {
                            //                    ErrorCounter++;
                            //                    if (ErrorCounter >= 5)
                            //                    {
                            //                        InfoRTB.AppendText("\nВо время передачи возникла ошибка!\nПередача прервана!");
                            //                        RHeader = false;
                            //                        RData = false;
                            //                        SHeader = false;
                            //                        SData = false;
                            //                        ErrorCounter = 0;
                            //                        //SendFileStream.Close();
                            //                        //SendFileStream.Dispose();
                            //                    }
                            //                    //NAK();
                            //                }
                            //                #endregion
                            //            }
                            //            #endregion
                            //        }
                            //    }
                //}
                #endregion
            }
        }
    }
}


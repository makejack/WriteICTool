using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WriteICTool.Properties;

namespace WriteICTool
{
    public partial class Form1 : Form
    {


        // Token: 0x04000001 RID: 1
        private Computer m_computer;

        // Token: 0x04000002 RID: 2
        private SerialPort m_serialPort;

        // Token: 0x04000003 RID: 3
        private List<byte> m_byteData;

        // Token: 0x04000004 RID: 4
        private System.Timers.Timer m_overTimer;

        // Token: 0x04000005 RID: 5
        private System.Timers.Timer m_autoWriteTimer;

        // Token: 0x04000006 RID: 6
        private System.Timers.Timer m_testTimer;

        // Token: 0x04000007 RID: 7
        private bool m_testOperation = false;

        // Token: 0x04000008 RID: 8
        private string m_dataIcNumber = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        // Token: 0x06000002 RID: 2 RVA: 0x0000207C File Offset: 0x0000027C
        private void Main_Load(object sender, EventArgs e)
        {
            this.m_computer = new Computer();
            this.m_byteData = new List<byte>();
            this.m_serialPort = new SerialPort
            {
                BaudRate = 19200,
                StopBits = StopBits.One,
                DataBits = 8,
                Parity = Parity.None
            };
            this.m_serialPort.DataReceived += this.SerialPort_DataReceived;
            this.m_overTimer = new System.Timers.Timer(3000.0)
            {
                AutoReset = false
            };
            this.m_overTimer.Elapsed += this.OverTime_Elapsed;
            this.m_autoWriteTimer = new System.Timers.Timer
            {
                AutoReset = false
            };
            this.m_autoWriteTimer.Elapsed += this.DelayTime_Elapsed;
            this.m_testTimer = new System.Timers.Timer
            {
                AutoReset = false
            };
            this.m_testTimer.Elapsed += this.TestTimer_Elapsed;
            this.cbDelay.SelectedIndex = 1;
            this.cbContrastDelay.SelectedIndex = 1;
        }

        // Token: 0x06000003 RID: 3 RVA: 0x0000218B File Offset: 0x0000038B
        private void TestTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.BtnContrast_Click(null, null);
            this.m_testTimer.Enabled = false;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x000021A4 File Offset: 0x000003A4
        private void StartTestTimer()
        {
            this.m_testTimer.Start();
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000021B3 File Offset: 0x000003B3
        private void StopTestTimer()
        {
            this.m_testTimer.Stop();
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000021C2 File Offset: 0x000003C2
        private void DelayTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.BtnRead_Click(null, null);
            this.m_autoWriteTimer.Enabled = false;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x000021DB File Offset: 0x000003DB
        private void StartWriteTimer()
        {
            this.m_autoWriteTimer.Start();
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000021EA File Offset: 0x000003EA
        private void StopWriteTimer()
        {
            this.m_autoWriteTimer.Stop();
        }

        // Token: 0x06000009 RID: 9 RVA: 0x000021F9 File Offset: 0x000003F9
        private void StartOverTimer()
        {
            this.m_overTimer.Start();
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002208 File Offset: 0x00000408
        private void StopOverTimer()
        {
            this.m_overTimer.Stop();
        }

        // Token: 0x0600000B RID: 11 RVA: 0x00002218 File Offset: 0x00000418
        private void OverTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.AddRecord("操作超时");
            bool @checked = this.cbAutoWriteChecked.Checked;
            if (@checked)
            {
                this.StartWriteTimer();
            }
            else
            {
                bool checked2 = this.cbAutoContrastChecked.Checked;
                if (checked2)
                {
                    this.StartTestTimer();
                }
                else
                {
                    this.EnabledControl(true);
                }
            }
            this.m_overTimer.Enabled = false;
        }

        // Token: 0x0600000C RID: 12 RVA: 0x0000227D File Offset: 0x0000047D
        private void Main_Shown(object sender, EventArgs e)
        {
            Task.Factory.StartNew(delegate
            {
                int count = 0;
                for (; ; )
                {
                    int serialportNameCount = this.m_computer.Ports.SerialPortNames.Count;
                    bool flag = count != serialportNameCount;
                    if (flag)
                    {
                        count = serialportNameCount;
                        this.AddRecord("检测到端口变化");
                        List<string> serialNames = new List<string>(this.m_computer.Ports.SerialPortNames);
                        bool invokeRequired = this.cbPortName.InvokeRequired;
                        if (invokeRequired)
                        {
                            this.cbPortName.Invoke(new Action(delegate
                            {
                                this.cbPortName.Items.Clear();
                                bool flag3 = serialNames.Count > 0;
                                if (flag3)
                                {
                                    ComboBox.ObjectCollection items = this.cbPortName.Items;
                                    object[] items2 = serialNames.ToArray();
                                    items.AddRange(items2);
                                }
                                bool isOpen = this.m_serialPort.IsOpen;
                                if (isOpen)
                                {
                                    string portName = this.m_serialPort.PortName;
                                    int index = this.cbPortName.Items.IndexOf(portName);
                                    bool flag4 = index == -1;
                                    if (flag4)
                                    {
                                        this.AddRecord("端口：" + portName + " 已断开");
                                        bool flag5 = this.cbPortName.Items.Count > 0;
                                        if (flag5)
                                        {
                                            this.cbPortName.SelectedIndex = 0;
                                        }
                                    }
                                    else
                                    {
                                        this.cbPortName.SelectedIndex = index;
                                    }
                                }
                                else
                                {
                                    bool flag6 = this.cbPortName.Items.Count > 0;
                                    if (flag6)
                                    {
                                        this.cbPortName.SelectedIndex = 0;
                                    }
                                }
                                this.ChangeSerialControl(this.m_serialPort.IsOpen);
                                this.EnabledControl(this.m_serialPort.IsOpen);
                            }));
                        }
                    }
                    Thread.Sleep(100);
                    try
                    {
                        bool flag2 = !base.IsHandleCreated;
                        if (flag2)
                        {
                            this.AddRecord("结束搜索端口");
                            break;
                        }
                    }
                    catch
                    {
                        bool isHandleCreated = this.rtbRecord.IsHandleCreated;
                        if (isHandleCreated)
                        {
                            this.AddRecord("搜索端口停止");
                        }
                        break;
                    }
                }
            });
        }

        // Token: 0x0600000D RID: 13 RVA: 0x00002298 File Offset: 0x00000498
        private void Main_KeyUp(object sender, KeyEventArgs e)
        {
            bool flag = e.KeyCode == Keys.Return;
            if (flag)
            {
                bool flag2 = this.tbNumber.Text.Length == 0;
                if (flag2)
                {
                    bool enabled = this.btnRead.Enabled;
                    if (enabled)
                    {
                        this.BtnRead_Click(null, null);
                        return;
                    }
                }
                bool flag3 = this.tbICNumber.Text.Length == 0;
                if (flag3)
                {
                    bool enabled2 = this.btnReadIc.Enabled;
                    if (enabled2)
                    {
                        this.BtnReadIc_Click(null, null);
                    }
                }
                else
                {
                    bool flag4 = this.tbClientNumber.Text.Length == this.tbClientNumber.MaxLength;
                    if (flag4)
                    {
                        bool enabled3 = this.btnWrite.Enabled;
                        if (enabled3)
                        {
                            this.BtnWrite_Click(null, null);
                        }
                    }
                }
            }
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002368 File Offset: 0x00000568
        private void EnabledControl(bool enabled)
        {
            bool invokeRequired = this.btnWrite.InvokeRequired;
            if (invokeRequired)
            {
                this.btnWrite.Invoke(new Action(delegate
                {
                    this.EnabledControl(enabled);
                }));
            }
            else
            {
                this.tbICNumber.Enabled = enabled;
                this.btnWrite.Enabled = enabled;
                this.btnRead.Enabled = enabled;
                this.btnReadIc.Enabled = enabled;
                this.tbClientNumber.Enabled = enabled;
                this.btnSetting.Enabled = enabled;
                bool @checked = this.cbAutoContrastChecked.Checked;
                if (@checked)
                {
                    this.btnContrast.Enabled = false;
                }
                else
                {
                    this.btnContrast.Enabled = enabled;
                }
                bool flag = !this.cbAutoContrastChecked.Checked && !this.cbAutoWriteChecked.Checked;
                if (flag)
                {
                    this.EnabledAutoWriteControl(enabled);
                    this.EnabledAutoContrastControl(enabled);
                }
            }
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00002498 File Offset: 0x00000698
        private void ClearTextContent()
        {
            bool invokeRequired = this.tbNumber.InvokeRequired;
            if (invokeRequired)
            {
                this.tbNumber.Invoke(new Action(delegate
                {
                    this.ClearTextContent();
                }));
            }
            else
            {
                this.tbNumber.Text = string.Empty;
                this.tbICNumber.Text = string.Empty;
            }
        }

        // Token: 0x06000010 RID: 16 RVA: 0x000024F2 File Offset: 0x000006F2
        private void ChangeSerialControl(bool isOpen)
        {
            this.btnOpen.Text = (isOpen ? "关闭" : "打开");
            this.cbPortName.Enabled = !isOpen;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x00002520 File Offset: 0x00000720
        private void UpdateNumberControl(string strNumber)
        {
            bool invokeRequired = this.tbNumber.InvokeRequired;
            if (invokeRequired)
            {
                this.tbNumber.Invoke(new Action(delegate
                {
                    this.UpdateNumberControl(strNumber);
                }));
            }
            else
            {
                this.tbNumber.Text = strNumber;
            }
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002580 File Offset: 0x00000780
        private void UpdateIcNumberControl(string strIcNumber)
        {
            bool invokeRequired = this.tbICNumber.InvokeRequired;
            if (invokeRequired)
            {
                this.tbICNumber.Invoke(new Action(delegate
                {
                    this.UpdateIcNumberControl(strIcNumber);
                }));
            }
            else
            {
                this.tbICNumber.Text = strIcNumber;
            }
        }

        // Token: 0x06000013 RID: 19 RVA: 0x000025E0 File Offset: 0x000007E0
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(10);
            try
            {
                int len = this.m_serialPort.BytesToRead;
                bool flag = len > 0;
                if (flag)
                {
                    byte[] by = new byte[len];
                    this.m_serialPort.Read(by, 0, len);
                    this.m_byteData.AddRange(by);
                    bool flag2 = this.m_byteData[0] == 2 && this.m_byteData[this.m_byteData.Count - 1] == 3;
                    if (flag2)
                    {
                        this.StopOverTimer();
                        bool flag3 = this.m_byteData[1] == 65;
                        if (flag3)
                        {
                            bool flag4 = this.m_byteData[6] == 48 && this.m_byteData[7] == 65;
                            if (flag4)
                            {
                                bool flag5 = this.m_byteData[8] == 48 && this.m_byteData[9] == 48;
                                if (flag5)
                                {
                                    string strNumber = Encoding.ASCII.GetString(this.m_byteData.ToArray(), 10, 6);
                                    int typestate = (int)this.m_byteData[16];
                                    int typestate2 = (int)this.m_byteData[17];
                                    bool flag6 = typestate == 69 || typestate2 == 56;
                                    if (flag6)
                                    {
                                        this.AddRecord("错误：" + strNumber + " 密码错误");
                                    }
                                    else
                                    {
                                        StringBuilder sb = new StringBuilder("读取卡片：" + strNumber);
                                        bool testOperation = this.m_testOperation;
                                        if (testOperation)
                                        {
                                            this.m_dataIcNumber = Encoding.ASCII.GetString(this.m_byteData.ToArray(), 24, 8);
                                            sb.Append(" -> IC卡号：" + this.m_dataIcNumber);
                                        }
                                        else
                                        {
                                            this.UpdateNumberControl(strNumber);
                                        }
                                        this.AddRecord(sb.ToString());
                                    }
                                    this.StartOverTimer();
                                }
                                else
                                {
                                    bool @checked = this.cbAutoWriteChecked.Checked;
                                    if (@checked)
                                    {
                                        bool flag7 = this.tbNumber.Text.Length == 0;
                                        if (flag7)
                                        {
                                            this.StartWriteTimer();
                                        }
                                        else
                                        {
                                            this.BtnReadIc_Click(null, null);
                                        }
                                    }
                                    else
                                    {
                                        bool testOperation2 = this.m_testOperation;
                                        if (testOperation2)
                                        {
                                            bool flag8 = this.m_dataIcNumber.Length == 0;
                                            if (flag8)
                                            {
                                                this.AddRecord("读取结束，请调整卡片位置");
                                                bool checked2 = this.cbAutoContrastChecked.Checked;
                                                if (checked2)
                                                {
                                                    this.StartTestTimer();
                                                }
                                                else
                                                {
                                                    this.EnabledControl(true);
                                                }
                                            }
                                            else
                                            {
                                                this.BtnReadIc_Click(null, null);
                                            }
                                        }
                                        else
                                        {
                                            this.AddRecord("读取结束");
                                            this.EnabledControl(true);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                bool flag9 = this.m_byteData[6] == 49 && this.m_byteData[7] == 66;
                                if (flag9)
                                {
                                    bool flag10 = this.m_byteData[8] == 48 && this.m_byteData[9] == 48;
                                    if (flag10)
                                    {
                                        string strNumber2 = Encoding.ASCII.GetString(this.m_byteData.ToArray(), 10, 6);
                                        this.AddRecord("卡片：" + strNumber2 + " 写入成功");
                                        this.ClearTextContent();
                                    }
                                    else
                                    {
                                        this.AddRecord("写入失败，请重新操作");
                                    }
                                    bool checked3 = this.cbAutoWriteChecked.Checked;
                                    if (checked3)
                                    {
                                        this.StartWriteTimer();
                                    }
                                    else
                                    {
                                        this.EnabledControl(true);
                                    }
                                }
                                else
                                {
                                    bool flag11 = this.m_byteData[6] == 65 && this.m_byteData[7] == 48;
                                    if (flag11)
                                    {
                                        bool flag12 = this.m_byteData[8] == 48 && this.m_byteData[9] == 48;
                                        if (flag12)
                                        {
                                            this.AddRecord("设备加密成功");
                                        }
                                        else
                                        {
                                            this.AddRecord("设备加密失败");
                                        }
                                        this.EnabledControl(true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            bool flag13 = this.m_byteData[1] == 66;
                            if (flag13)
                            {
                                bool flag14 = this.m_byteData[4] == 48 && this.m_byteData[5] == 57;
                                if (flag14)
                                {
                                    try
                                    {
                                        string strIcNumber = Encoding.ASCII.GetString(this.m_byteData.ToArray(), 6, 8);
                                        this.AddRecord("读取IC编号：" + strIcNumber);
                                        this.StopReadIcNumber();
                                        bool checked4 = this.cbAutoWriteChecked.Checked;
                                        if (checked4)
                                        {
                                            this.UpdateIcNumberControl(strIcNumber);
                                            Thread.Sleep(20);
                                            this.BtnWrite_Click(null, null);
                                        }
                                        else
                                        {
                                            bool testOperation3 = this.m_testOperation;
                                            if (testOperation3)
                                            {
                                                bool flag15 = strIcNumber == this.m_dataIcNumber;
                                                if (flag15)
                                                {
                                                    this.SetImage(Resources.check);
                                                    this.AddRecord("IC卡号对比结果相同");
                                                }
                                                else
                                                {
                                                    this.SetImage(Resources.close);
                                                    this.AddRecord("IC卡号对比结果不同");
                                                }
                                                bool checked5 = this.cbAutoContrastChecked.Checked;
                                                if (checked5)
                                                {
                                                    this.StartTestTimer();
                                                }
                                                else
                                                {
                                                    this.EnabledControl(true);
                                                }
                                                this.m_testOperation = false;
                                            }
                                            else
                                            {
                                                this.UpdateIcNumberControl(strIcNumber);
                                                this.EnabledControl(true);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        this.AddRecord("错误：" + ex.Message);
                                        this.EnabledControl(true);
                                    }
                                }
                            }
                        }
                        this.m_byteData.Clear();
                    }
                }
            }
            catch (Exception ex2)
            {
                this.AddRecord("错误：" + ex2.Message + ex2.StackTrace);
            }
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002BBC File Offset: 0x00000DBC
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            bool isOpen = this.m_serialPort.IsOpen;
            if (isOpen)
            {
                try
                {
                    this.m_serialPort.Close();
                    this.AddRecord("端口：" + this.m_serialPort.PortName + " 关闭");
                }
                catch (Exception ex)
                {
                    this.AddRecord("错误：" + ex.Message);
                }
            }
            else
            {
                try
                {
                    bool flag = this.cbPortName.Items.Count > 0;
                    if (flag)
                    {
                        string portName = this.cbPortName.SelectedItem.ToString();
                        this.m_serialPort.PortName = portName;
                        this.m_serialPort.Open();
                        this.AddRecord("端口：" + this.m_serialPort.PortName + " 打开");
                        this.rtbRecord.Focus();
                    }
                }
                catch (Exception ex2)
                {
                    this.AddRecord("错误：" + ex2.Message);
                }
            }
            this.ChangeSerialControl(this.m_serialPort.IsOpen);
            this.EnabledControl(this.m_serialPort.IsOpen);
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002D04 File Offset: 0x00000F04
        private void AddRecord(string message)
        {
            bool invokeRequired = this.rtbRecord.InvokeRequired;
            if (invokeRequired)
            {
                this.rtbRecord.Invoke(new Action(delegate
                {
                    this.AddRecord(message);
                }));
            }
            else
            {
                this.rtbRecord.AppendText(message + "\n");
                this.rtbRecord.ScrollToCaret();
            }
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00002D78 File Offset: 0x00000F78
        private void BtnRead_Click(object sender, EventArgs e)
        {
            try
            {
                bool isOpen = this.m_serialPort.IsOpen;
                if (isOpen)
                {
                    string data = "0A8000000000010004";
                    byte[] byData = this.DealPackage(65, data);
                    this.m_serialPort.Write(byData, 0, byData.Length);
                    this.StartOverTimer();
                    this.EnabledControl(false);
                    this.rtbRecord.Clear();
                }
            }
            catch (Exception ex)
            {
                this.AddRecord("错误：" + ex.Message);
            }
        }

        // Token: 0x06000017 RID: 23 RVA: 0x00002E04 File Offset: 0x00001004
        private byte[] StrToBytes(string str)
        {
            int count = str.Length / 2;
            byte[] by = new byte[count];
            int index = 0;
            for (int i = 0; i < str.Length; i += 2)
            {
                by[index] = Convert.ToByte(str.Substring(i, 2), 16);
                index++;
            }
            return by;
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00002E5C File Offset: 0x0000105C
        private void BtnWrite_Click(object sender, EventArgs e)
        {
            bool invokeRequired = this.btnWrite.InvokeRequired;
            if (invokeRequired)
            {
                this.btnWrite.Invoke(new Action(delegate
                {
                    this.BtnWrite_Click(sender, e);
                }));
            }
            else
            {
                string strNumber = this.tbNumber.Text;
                string icNumber = this.tbICNumber.Text;
                bool flag = strNumber.Length == 0;
                if (flag)
                {
                    MessageBox.Show("定距卡号不能为空，请重新输入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.tbNumber.Focus();
                }
                else
                {
                    bool flag2 = icNumber.Length == 0;
                    if (flag2)
                    {
                        MessageBox.Show("IC编号不能为空，请重新输入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        this.tbICNumber.Focus();
                    }
                    else
                    {
                        bool flag3 = icNumber.Length < this.tbICNumber.MaxLength;
                        if (flag3)
                        {
                            MessageBox.Show("IC编号长度不正确，8位十六进制数", "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            this.tbICNumber.Focus();
                        }
                        else
                        {
                            try
                            {
                                bool isOpen = this.m_serialPort.IsOpen;
                                if (isOpen)
                                {
                                    string data = string.Format("1B00{0}000100{1:X2}{2}", strNumber, icNumber.Length / 2, icNumber);
                                    byte[] byData = this.DealPackage(65, data);
                                    this.m_serialPort.Write(byData, 0, byData.Length);
                                    this.StartOverTimer();
                                    this.EnabledControl(false);
                                }
                            }
                            catch (Exception ex)
                            {
                                this.AddRecord("错误：" + ex.Message);
                            }
                        }
                    }
                }
            }
        }

        // Token: 0x06000019 RID: 25 RVA: 0x00002FFC File Offset: 0x000011FC
        private byte[] DealPackage(byte funAddress, string data)
        {
            return this.DealPackage(funAddress, 48, 48, data);
        }

        // Token: 0x0600001A RID: 26 RVA: 0x0000301C File Offset: 0x0000121C
        private byte[] DealPackage(byte funAddress, byte command, byte command2, string data)
        {
            List<byte> byData = new List<byte>();
            byData.Add(2);
            byData.Add(funAddress);
            byData.AddRange(new byte[]
            {
                48,
                48,
                command,
                command2
            });
            byData.AddRange(Encoding.ASCII.GetBytes(data));
            int xor = this.Xor(byData);
            byte[] xorData = Encoding.ASCII.GetBytes(string.Format("{0:X2}", xor));
            byData.AddRange(xorData);
            byData.Add(3);
            return byData.ToArray();
        }

        // Token: 0x0600001B RID: 27 RVA: 0x000030B0 File Offset: 0x000012B0
        private int Xor(List<byte> data)
        {
            int result = 0;
            foreach (byte item in data)
            {
                result ^= (int)item;
            }
            return result;
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00003108 File Offset: 0x00001308
        private void ICNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool flag = e.KeyChar == '\b';
            if (!flag)
            {
                bool flag2 = (e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F');
                if (flag2)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        // Token: 0x0600001D RID: 29 RVA: 0x00003188 File Offset: 0x00001388
        private void BtnReadIc_Click(object sender, EventArgs e)
        {
            bool invokeRequired = this.btnReadIc.InvokeRequired;
            if (invokeRequired)
            {
                this.btnReadIc.Invoke(new Action(delegate
                {
                    this.BtnReadIc_Click(sender, e);
                }));
            }
            else
            {
                try
                {
                    bool isOpen = this.m_serialPort.IsOpen;
                    if (isOpen)
                    {
                        byte[] byData = this.DealPackage(66, 48, 57, string.Empty);
                        this.m_serialPort.Write(byData, 0, byData.Length);
                        this.StartOverTimer();
                        this.EnabledControl(false);
                    }
                }
                catch (Exception ex)
                {
                    this.AddRecord("错误：" + ex.Message);
                }
            }
        }

        // Token: 0x0600001E RID: 30 RVA: 0x00003254 File Offset: 0x00001454
        private void StopReadIcNumber()
        {
            byte[] byData = this.DealPackage(67, 68, 50, string.Empty);
            this.m_serialPort.Write(byData, 0, byData.Length);
        }

        // Token: 0x0600001F RID: 31 RVA: 0x00003288 File Offset: 0x00001488
        private void ClientNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool flag = e.KeyChar == '\b';
            if (!flag)
            {
                bool flag2 = e.KeyChar < '0' && e.KeyChar > '9';
                if (flag2)
                {
                    e.Handled = true;
                }
            }
        }

        // Token: 0x06000020 RID: 32 RVA: 0x000032CC File Offset: 0x000014CC
        private void BtnSetting_Click(object sender, EventArgs e)
        {
            string strClientNumber = this.tbClientNumber.Text;
            bool flag = strClientNumber.Length == 0;
            if (flag)
            {
                MessageBox.Show("客户编号不能为空，请重新输入。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.tbClientNumber.Focus();
            }
            else
            {
                bool flag2 = strClientNumber.Length < this.tbClientNumber.MaxLength;
                if (flag2)
                {
                    MessageBox.Show("客户编号长度不正确", "提示", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.tbClientNumber.Focus();
                }
                else
                {
                    try
                    {
                        bool isOpen = this.m_serialPort.IsOpen;
                        if (isOpen)
                        {
                            string data = string.Format("A00001000000{0}{1}", strClientNumber, 766554);
                            byte[] byData = this.DealPackage(65, data);
                            this.m_serialPort.Write(byData, 0, byData.Length);
                            this.EnabledControl(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.AddRecord("错误：" + ex.Message);
                    }
                }
            }
        }

        // Token: 0x06000021 RID: 33 RVA: 0x000033D4 File Offset: 0x000015D4
        private void Delay_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.m_autoWriteTimer.Interval = (double)((this.cbDelay.SelectedIndex + 1) * 1000);
        }

        // Token: 0x06000022 RID: 34 RVA: 0x000033F8 File Offset: 0x000015F8
        private void AutoWriteChecked_CheckedChanged(object sender, EventArgs e)
        {
            this.cbDelay.Enabled = !this.cbAutoWriteChecked.Checked;
            bool @checked = this.cbAutoWriteChecked.Checked;
            if (@checked)
            {
                this.StartWriteTimer();
            }
            else
            {
                this.StopWriteTimer();
            }
            this.EnabledControl(!this.cbAutoWriteChecked.Checked);
            this.EnabledAutoContrastControl(!this.cbAutoWriteChecked.Checked);
        }

        // Token: 0x06000023 RID: 35 RVA: 0x00003470 File Offset: 0x00001670
        private void EnabledAutoWriteControl(bool enabled)
        {
            this.StopWriteTimer();
            bool flag = !this.m_serialPort.IsOpen;
            if (flag)
            {
                bool @checked = this.cbAutoWriteChecked.Checked;
                if (@checked)
                {
                    this.cbAutoWriteChecked.Checked = false;
                }
            }
            this.cbAutoWriteChecked.Enabled = enabled;
            this.cbDelay.Enabled = enabled;
        }

        // Token: 0x06000024 RID: 36 RVA: 0x000034D4 File Offset: 0x000016D4
        private void EnabledAutoContrastControl(bool enalbed)
        {
            bool flag = !this.m_serialPort.IsOpen;
            if (flag)
            {
                bool @checked = this.cbAutoContrastChecked.Checked;
                if (@checked)
                {
                    this.cbAutoContrastChecked.Checked = false;
                }
            }
            this.cbAutoContrastChecked.Enabled = enalbed;
            this.cbContrastDelay.Enabled = enalbed;
        }

        // Token: 0x06000025 RID: 37 RVA: 0x0000352E File Offset: 0x0000172E
        private void ContrastDelay_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.m_testTimer.Interval = (double)((this.cbContrastDelay.SelectedIndex + 1) * 1000);
        }

        // Token: 0x06000026 RID: 38 RVA: 0x00003554 File Offset: 0x00001754
        private void BtnContrast_Click(object sender, EventArgs e)
        {
            bool invokeRequired = this.btnContrast.InvokeRequired;
            if (invokeRequired)
            {
                this.btnContrast.Invoke(new Action(delegate
                {
                    this.BtnContrast_Click(sender, e);
                }));
            }
            else
            {
                try
                {
                    bool isOpen = this.m_serialPort.IsOpen;
                    if (isOpen)
                    {
                        this.pbImage.Image = null;
                        this.m_dataIcNumber = string.Empty;
                        this.BtnRead_Click(null, null);
                        this.m_testOperation = true;
                        this.AddRecord("测试开始");
                    }
                }
                catch (Exception ex)
                {
                    this.AddRecord("错误：" + ex.Message);
                }
            }
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00003620 File Offset: 0x00001820
        private void AutoContrastChecked_CheckedChanged(object sender, EventArgs e)
        {
            bool @checked = this.cbAutoContrastChecked.Checked;
            if (@checked)
            {
                this.cbContrastDelay.Enabled = false;
            }
            else
            {
                this.cbContrastDelay.Enabled = true;
            }
            this.EnabledAutoWriteControl(!this.cbAutoContrastChecked.Checked);
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00003674 File Offset: 0x00001874
        private void SetImage(Bitmap img)
        {
            bool invokeRequired = this.pbImage.InvokeRequired;
            if (invokeRequired)
            {
                this.pbImage.Invoke(new Action(delegate
                {
                    this.SetImage(img);
                }));
            }
            else
            {
                this.pbImage.Image = img;
            }
        }

    }
}

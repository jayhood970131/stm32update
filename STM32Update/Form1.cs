using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Windows;
namespace STM32Update
{
    public partial class MainForm : Form
    {
        
        
        private SerialPort sp = null;
        private FileHandler myFileHandler=null;
        private CRC16 cs16 = null;      //用于计算CRC校验值

        private byte[] configData = null;  //用于发送的数据
        byte[] ReceivedByteData = null;

        private List<TextBox> tBxHeadList = new List<TextBox>();    //存放一次性发送页面的textbox
        private List<RegRecord> regRecList = new List<RegRecord>(); //存放寄存器记录，即将寄存器值、寄存器地址和对应的文本框结合在一起的类
        private byte max_addr = 0x00;                               //寄存器记录里面最大的地址

        private Int32 bautRate;
        /*
         * 计时器
         */
        private int uartReSendTimeOnCount = 0;
        private int uartSendTimeOnCount = 0;

        private int regCfgTimeCount = 0; //计数
        private int regCfgTimeOnVal = 0; //计数最大值
        /*
         * 各类标志位
         */
        private bool isSpOpen = false;
        private bool isSpPropertySet = false;

        private bool flag_FrameRecv=false;  //stm32确认收到标志
        private bool flag_reTx = false;   //重发标志
        private bool flag_uartTxTimeOn = false;   //自动发送10s后仍未收到恢复则此标记设为真
        private bool flag_stopAutoIAPTx = false; //停止自动发送
        private bool flag_stopAutoRegTx = false;

        private bool flag_RegCfgRecv = false;   //寄存器配置确认收到标志
        private bool flag_RegTxTimeOn = false; //寄存器发送任务失败标志

        private bool isFileSelected = false;
        private bool isFrameDataSizeChanged = false; //数据包大小的设置是否改变过
        private bool isCRCCorrect = false;
        private bool comAvailable = false;  //是否有可用串口

        private bool isIgnoreSpData = true; //是否忽略串口数据

        private byte sendMode;      //一次性发送完还是分帧发送

        private const byte frameSendMode = 1;       //分帧发送
        private const byte fullSendMode = 2;        //一次性全部发送
        private const byte readRegMode = 3;         //读取寄存器模式
        private const string cfgFileName = "ProtocolConfig.bin";
        private const string str_hasCom = "COM_AVAILABLE";
        private const string str_noCom = "COM_NOT_AVAILABLE";
        public MainForm()
        {
            InitializeComponent();
            //改变窗体风格，使之不能用鼠标拖拽改变大小
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            //禁止使用最大化按钮
            this.MaximizeBox = false;
            
            this.myFileHandler=null;
           
            this.tbx_SrcAddr.Text = "00";
            this.tbx_TargetAddr.Text = "01";
            this.tbx_FrameSize.Text = "0128";

            this.updateBtn.Enabled = false;         //设置刚开始时某些按钮不可用，避免某些操作出错
            this.sendNextFrameBtn.Enabled = false;
            this.frameRSTBtn.Enabled = false;
            this.stopUpdateBtn.Enabled = false;
            this.btnSaveReg.Enabled = false;
            this.btnSendReg.Enabled = false;
            this.tbx_currentFrame.Text = "当前未发送任何数据包";
            this.sendMode = MainForm.frameSendMode; 
            label_spSetUp.Text = "校验位：无\r\n数据位：8\r\n停止位：1";
            label_spSetUp.ForeColor = Color.Gray;
            labelVerNum.Text = "Version:V1.16";
            labelVerNum.Visible = false;

            this.picBoxOff.Visible = true;
            this.picBoxOn.Visible = false;

            this.cs16 = new CRC16();

            this.sendMode = MainForm.frameSendMode;

            this.tBxHeadList.Add(this.FrameHeadTbx);
            this.tBxHeadList.Add(this.PortocolNumTbx);
            this.tBxHeadList.Add(this.VersionNumTbx);
            this.tBxHeadList.Add(this.PortocolNumTbx);
            this.tBxHeadList.Add(this.TargetAddrTbx);
            this.tBxHeadList.Add(this.SourceAddrTbx);
            this.tBxHeadList.Add(this.PortNumTbx);
            this.tBxHeadList.Add(this.ControlCodeTbx);
            this.tBxHeadList.Add(this.PortocolNumTbx);
            this.tBxHeadList.Add(this.StartRegAddr_L_Tbx);
            this.tBxHeadList.Add(this.StartRegAddr_H_Tbx);

            this.flwLyPan_RegList.AutoScroll = true;
            this.flwLyPan_RegList.FlowDirection = FlowDirection.TopDown;
            this.flwLyPan_RegList.WrapContents = false;//不截断内容

            sp = new SerialPort();
            this.bautRate = Convert.ToInt32(115200);
            this.rbtn_Baud115200.Checked = true; //默认选择115200

            //this.groupBox4.Visible = false;

            this.Size = new Size(this.Width / 2, this.Height);
        }

        private void checkSpBtn_Click(object sender, EventArgs e)
        {
            string[] str = SerialPort.GetPortNames();
            if (str == null)
            {
                this.cBx_sp.Items.Clear();
                this.cBx_sp.Text = "未检测到有效串口";
                this.cBx_sp.Enabled = false;
                this.comAvailable = false;
                MessageBox.Show("无可用串口", "Error");
                return;
            }

            this.cBx_sp.Items.Clear();  //清除combobox中的数据
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                this.cBx_sp.Items.Add(s);
            }
            this.cBx_sp.SelectedIndex = 0;
            this.cBx_sp.Enabled = true;
            this.comAvailable = true;
        }

        private void spInit()
        {
            string[] str = SerialPort.GetPortNames();
            if (str.Length == 0)
            {
                this.cBx_sp.Text = "未检测到可用串口";
                this.cBx_sp.Enabled = false;
                comAvailable = false;
            }
            else
            {
                this.cBx_sp.Items.Clear();  //清除comboBox中的内容
                foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
                {
                    this.cBx_sp.Items.Add(s);
                }
                this.cBx_sp.SelectedIndex = 0;
                comAvailable = true;

                if (isSpSelected())//检测串口设置
                {
                    if (!isSpPropertySet)//串口未设置则设置串口
                    {
                        setSpProperties();
                        isSpPropertySet = true;
                    }
                    try//打开串口
                    {
                        sp.Open();
                        isSpOpen = true;
                        spOpenBtn.Text = "关闭串口";
                        spOpenBtn.BackColor = Color.LimeGreen;
                        //串口打开后则相关的串口设置按钮便不可再用
                        this.cBx_sp.Enabled = false;

                        this.rbtn_Baud9600.Enabled = false;
                        this.rbtn_Baud115200.Enabled = false;
                    }
                    catch (Exception)
                    {
                        //打开串口失败后，相应标志位取消
                        isSpPropertySet = false;
                        isSpOpen = false;

                        this.rbtn_Baud9600.Enabled = true;
                        this.rbtn_Baud115200.Enabled = true;
                    }
                }
                else
                {
                    this.cBx_sp.Items.Clear();
                    this.cBx_sp.Text = "未检测到可用串口";
                    comAvailable = false;
                    this.rbtn_Baud9600.Enabled = true;
                    this.rbtn_Baud115200.Enabled = true;
                }
            }

            picBoxOff.Visible = true;
            picBoxOn.Visible = false;
        }

        private void spOpenBtn_Click(object sender, EventArgs e)
        {
            if (!isSpOpen)
            {
                if (!isSpSelected())//检测串口设置
                {
                    MessageBox.Show("未选择串口！", "错误提示");
                    return;
                }

                if (!isSpPropertySet)//串口未设置则设置串口
                {
                    setSpProperties();
                    isSpPropertySet = true;
                }
                try//打开串口
                {
                    sp.Open();
                    isSpOpen = true;
                    spOpenBtn.Text = "关闭串口";
                    spOpenBtn.BackColor = Color.LimeGreen;
                    //串口打开后则相关的串口设置按钮便不可再用
                    this.cBx_sp.Enabled = false;
                    if (this.isFileSelected)
                    {
                        this.updateBtn.Enabled = true;
                        this.sendNextFrameBtn.Enabled = true;
                    }
                    else
                    {
                        this.updateBtn.Enabled = false;
                        this.sendNextFrameBtn.Enabled = false;
                    }

                    this.btnSendReg.Enabled = true;
                    this.rbtn_Baud9600.Enabled = false;
                    this.rbtn_Baud115200.Enabled = false;
                }
                catch (Exception)
                {
                    //打开串口失败后，相应标志位取消
                    isSpPropertySet = false;
                    isSpOpen = false;
                    MessageBox.Show("串口无效或已被占用！", "错误提示");
                    this.rbtn_Baud9600.Enabled = true;
                    this.rbtn_Baud115200.Enabled = true;
                }
            }
            else
            {
                this.sp.Close();
                isSpOpen = false;
                isSpPropertySet = false;
                spOpenBtn.Text = "打开串口";
                spOpenBtn.BackColor = Color.Transparent;
                //关闭串口后，串口设置选项便可以继续使用
                this.cBx_sp.Enabled = true;
                this.updateBtn.Enabled = false;
                this.btnSendReg.Enabled = false;
                this.sendNextFrameBtn.Enabled = false;

                this.flag_stopAutoIAPTx = false;
                this.stopUpdateBtn.Enabled = false;

                this.timer_UART.Enabled = false;
                this.timer_RegCfg.Enabled = false;

                this.flag_stopAutoIAPTx = true;
                this.flag_stopAutoRegTx = true;

                this.rbtn_Baud9600.Enabled = true;
                this.rbtn_Baud115200.Enabled = true;

            }
        }

        private bool isSpSelected()//检查串口是否设置
        {
            if (this.cBx_sp.Text.Trim() == "") return false;
            return true;
        }

        private void setSpProperties()//设置串口的属性
        {          
            sp.PortName = this.cBx_sp.Text.Trim();  //设置串口名
            sp.BaudRate = this.bautRate;//设置串口的波特率
            //sp.BaudRate = Convert.ToInt32(115200);
            sp.StopBits = StopBits.One;
            sp.DataBits = Convert.ToInt16(8);//设置数据位
            sp.Parity = Parity.None;  //无校验位

            sp.ReadTimeout = -1;//设置超时读取时间
            sp.RtsEnable = true;
            sp.ReceivedBytesThreshold = 1;
            //定义 DataReceived 事件，当串口收到数据后触发事件
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            

        }

        /*串口接收消息处理*/
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //if (isIgnoreSpData)
            //{
            //    sp.DiscardInBuffer();
            //    return;
            //}
            
            System.Threading.Thread.Sleep(50);//延时 50ms 等待接收完数据
            
            this.BeginInvoke((EventHandler)(delegate
            {
                if (!sp.IsOpen)
                    return;
                if (sp.BytesToRead == 0)
                    return;
                byte[] ReceivedByteData = new byte[sp.BytesToRead]; //创建接收字节数组
                sp.Read(ReceivedByteData, 0, ReceivedByteData.Length); //读取所接收到的数据

                string tempStr = "";
                //System.Diagnostics.Debug.WriteLine("rx" + "\r\n");
                //System.Diagnostics.Debug.WriteLine("Len:" + ReceivedByteData.Length.ToString()+"\r\n");
                for (int k = 0; k < ReceivedByteData.Length; k++)
                {
                    tempStr += string.Format("{0:X2}", ReceivedByteData[k]) + " ";
                }
                //System.Diagnostics.Debug.WriteLine(tempStr + "\r\n");

                this.tbx_ResultView.Text += "\r\nrx:\r\n";
                this.tbx_ResultView.Text += tempStr;
                //if (ReceivedByteData.Length == 0)
                //{
                //    return;
                //}
                //else if(ReceivedByteData[0] != 0x7e)        //不是0x7e开头的数据不收
                //{
                //    sp.DiscardInBuffer();
                //    return;
                //}
                this.tbx_ResultView.Text += "\r\nRx：" + ReceivedByteData.Length.ToString() + " Bytes--" + DateTime.Now.ToLongTimeString();
                
                //CRC校验
                this.cs16.CRC16_modbus(ReceivedByteData, ReceivedByteData.Length - 2);
                if (this.cs16.getCRC16_L() == ReceivedByteData[ReceivedByteData.Length - 2] &&
                    this.cs16.getCRC16_H() == ReceivedByteData[ReceivedByteData.Length - 1])
                    this.isCRCCorrect = true;
                else
                    this.isCRCCorrect = false;
                switch (this.sendMode)
                {
                    case MainForm.frameSendMode:
                        {
                            
                            this.timer_UART.Enabled = false;
                            
                            
                            if (this.isCRCCorrect)
                            {
                                flag_FrameRecv = true;

                                this.tbx_ResultView.Text += "\r\nFrame #" +
                                    this.myFileHandler.fileData.getCurrentFrameNum().ToString() + " confirmed. \r\n\r\n";
                            }
                            else
                            {
                                flag_FrameRecv = false;             //传输任务失败
                                this.flag_stopAutoIAPTx = true;
                                this.stopUpdateBtn.Enabled = false;

                                this.tbx_ResultView.Text += "\r\nCRC Check failed:";
                                Application.DoEvents();
                                string[] s = new string[ReceivedByteData.Length];
                                string strAscii = "";
                                byte[] temp = new byte[1];
                                
                                for (int i = 0; i < s.Length; i++)
                                {

                                    if ((ReceivedByteData[i] >= 0 && ReceivedByteData[i] <= 31) || ReceivedByteData[i] >= 127)
                                        temp[0] = 0x21;
                                    else
                                        temp[0] = ReceivedByteData[i];
                                
                                    s[i] = Encoding.ASCII.GetString(temp).ToUpper();        //将CRC校验错误的帧当作ASCII码输出
                                    strAscii += s[i];
                                }
                                this.tbx_ResultView.Text += strAscii;
                                this.tbx_ResultView.Text += "\r\n";    

                                
                                this.tbx_ResultView.Text += "\r\nCancel Tx mission --" + DateTime.Now.ToLongTimeString() + "\r\n\r\n";
                                return;
                            }

                            if (this.myFileHandler.fileData.getCurrentFrameNum() == this.myFileHandler.fileData.getTotalFrameNum())  //发送完最后一帧，亮起指示图片
                            {
                                this.tbx_FrameSize.Enabled = true;
                                this.picBoxOn.Visible = true;
                                this.picBoxOff.Visible = false;
                            }

                        }
                        break;
                    case MainForm.fullSendMode:
                        {
                            this.timer_RegCfg.Enabled = false;

                            if (isCRCCorrect)
                            {
                                this.flag_RegCfgRecv = true;
                                this.tbx_ResultView.Text += "\r\nCRC Check Passed\r\n";
                            }
                            else
                            {
                                this.flag_RegCfgRecv = false;
                                this.flag_RegTxTimeOn = false;
                                
                                this.flag_RegTxTimeOn = false;
                                this.regCfgTimeCount = 0;
                                
                                this.tbx_ResultView.Text += "\r\nCRC Check Failed\r\n";
                            }

                            
                            /*输出接收结果和crc值*/
                            //byte crcL = this.cs16.getCRC16_L();
                            //byte crcH = this.cs16.getCRC16_H();
                            //byte dataCRCL = ReceivedByteData[ReceivedByteData.Length - 2];
                            //byte dataCRCH = ReceivedByteData[ReceivedByteData.Length - 1];
                            //string temp = "";
                            //System.Diagnostics.Debug.WriteLine("rx" + "\r\n");
                            //for (int k = 0; k < ReceivedByteData.Length; k++)
                            //{
                            //    temp += string.Format("{0:X2}", ReceivedByteData[k]) + " ";
                            //}

                            //System.Diagnostics.Debug.WriteLine(temp + "\r\n" + this.isCRCCorrect.ToString() + "\r\n");
                            //System.Diagnostics.Debug.WriteLine(string.Format("{0:X2}", crcL) + " " + string.Format("{0:X2}", crcH) + "\r\n");
                            //System.Diagnostics.Debug.WriteLine(string.Format("{0:X2}", dataCRCL) + " " + string.Format("{0:X2}", dataCRCH) + "\r\n");
                                
                        }
                        break;
                    case MainForm.readRegMode:
                        {

                        }
                        break;
                    default:
                        break;
                }
              
            }));
                              
        }


        /*选择BIN文件*/
        private void SelectFileBtn_Click(object sender, EventArgs e)
        {
            this.myFileHandler = new FileHandler(FileHandler.FILE_BIN);

            byte resultCode = myFileHandler.read_configFile();  //尝试读取一次文件
            
            if(resultCode!= FileHandler.FILE_SELECTED)
            {
                switch(resultCode)
                {
                    case FileHandler.FILE_NOT_SELECTED:     //没有选择文件
                        {
                            this.tbx_SelectedFile.Text = "未选择文件，请重新选择。";
                            this.updateBtn.Enabled = false;
                            this.sendNextFrameBtn.Enabled = false;
                            this.frameRSTBtn.Enabled = false;
                        }
                        break;
                    case FileHandler.FILE_FORMAT_ILLEGAL:
                        {

                        }
                        break;
                    default:
                        break;
                }
                return;
            }
           
            this.tbx_SelectedFile.Text = myFileHandler.get_cfg_filename() + "\r\n";
            configData = new byte[myFileHandler.get_fileByteCounts()];
            myFileHandler.get_configData(configData);    //将配置文件中的数据写入新的数组configData。
            this.tbx_SelectedFile.Text += "Total bytes:" + configData.Length.ToString()+"\r\n";

            this.tbx_FrameSize.Enabled = true;
            this.myFileHandler.fileData.setCurrentFrameNum(0);
            this.myFileHandler.fileData.calculateTotalFrame();
            this.tbx_currentFrame.Text = "当前未发送任何数据包";
            /*
            int byteCount = 0;      
            this.Invoke((EventHandler)(delegate
            {
                for (int i = 0; i < configData.Length; i++)
                {
                    this.DataViewTbx.Text += configData[i].ToString("X2") + " ";
                    Application.DoEvents();
                    byteCount++;
                    if (byteCount == 20)
                    {
                        this.SelectFileTbx.Text += "\r\n";
                        byteCount = 0;
                    }
                        
                }
            }));
          */
            this.isFileSelected = this.myFileHandler.isFileSelected();
            if (this.isFileSelected)
                this.frameRSTBtn.Enabled = true;

            if (this.isFileSelected && this.isSpOpen)
            {
                this.updateBtn.Enabled = true;
                this.sendNextFrameBtn.Enabled = true;
            }
            else
            {
                this.updateBtn.Enabled = false;
                this.sendNextFrameBtn.Enabled = false;
            }
            
        }

        /*自动发送全部数据包*/
        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            this.sendMode = MainForm.frameSendMode;
            this.sendFullDataBtn.Enabled = false;
            this.frameRSTBtn.Enabled = false;
            if (!this.checkParaConfig())
            {
                MessageBox.Show("配置错误。请确认是否填入配置，并确保配置合法。", "ERROR");
                return;
            }
            if(!sp.IsOpen)
            {
                MessageBox.Show("串口未打开。", "ERROR");
                return;
            }

            this.myFileHandler.fileData.frameRST();  //重新载入
            this.myFileHandler.read_configFile();
            configData = new byte[myFileHandler.get_fileByteCounts()];
            myFileHandler.get_configData(configData);

            MultiTypeNumber m1 = new MultiTypeNumber(); //将文本框的字符串数据转为byte类型
            m1.setByteNumber(this.tbx_TargetAddr.Text, MultiTypeNumber.STR_HEX);
            byte targetAddr = m1.getByteNumber_Byte();
            m1.setByteNumber(this.tbx_SrcAddr.Text,MultiTypeNumber.STR_HEX);
            byte srcAddr = m1.getByteNumber_Byte();
            this.myFileHandler.fileData.setDataFrameSize(int.Parse(this.tbx_FrameSize.Text));  //提供一帧大小，计算出一共多少帧

            flag_reTx = false;
            flag_FrameRecv = false;

            this.tbx_FrameSize.Enabled = false;  //不能再中途改变一帧的大小
            this.updateBtn.Enabled = false;
            this.sendNextFrameBtn.Enabled = false;
            this.picBoxOn.Visible = false;
            this.picBoxOff.Visible = true;
            this.tbx_currentFrame.Text = this.myFileHandler.fileData.getCurrentFrameNum().ToString();

            this.stopUpdateBtn.Enabled = true;
            int totalFrameNum = this.myFileHandler.fileData.getTotalFrameNum();

            this.isIgnoreSpData = false;

            // 设置进度条初始值
            prgsBar.Value = 1;
            prgsBar.Maximum = this.regRecList.Count + 1;

            this.flag_stopAutoIAPTx = false;
            /*发送一帧并等待回复*/
            for(int i=0;i<totalFrameNum;i++)
            {

                this.uartReSendTimeOnCount = 0;
                this.uartSendTimeOnCount = 0;
                this.flag_uartTxTimeOn = false;
                

                this.myFileHandler.fileData.loadNextFrame(targetAddr,srcAddr);
                this.myFileHandler.fileData.sendFrame(sp);
                System.Threading.Thread.Sleep(100); //加上这句快一点
                this.timer_UART.Enabled = true;

                this.tbx_currentFrame.Text = this.myFileHandler.fileData.getCurrentFrameNum().ToString();
                this.tbx_ResultView.Text += "\r\nTx Frame #" 
                    + this.myFileHandler.fileData.getCurrentFrameNum().ToString()+"...--"+DateTime.Now.ToLongTimeString();
                //this.ResultViewTbx.Text += "\r\nCRC_L:" + string.Format("{0:X2}", this.dataCRCCheck[0]);          //显示正确的CRC检验码
                //this.ResultViewTbx.Text += "\r\nCRC_H:" + string.Format("{0:X2}", this.dataCRCCheck[1]);

                while (!(flag_uartTxTimeOn ||flag_FrameRecv))           //等待回复
                {
                    Application.DoEvents();
                    if (flag_reTx)  //没有收到回复，重发
                    {
                        this.myFileHandler.fileData.sendFrame(sp);
                        this.tbx_ResultView.Text += "\r\nNo Resbond,try again..."+DateTime.Now.ToLongTimeString();
                        this.timer_UART.Enabled = true;
                        this.flag_reTx = false;
                    }
                    if (this.flag_stopAutoIAPTx)
                    {
                        this.timer_UART.Enabled = false;
                        this.isIgnoreSpData = true;

                        flag_reTx = false;
                        flag_FrameRecv = false;

                        this.flag_stopAutoIAPTx = false;                     
                        this.sendNextFrameBtn.Enabled = true;
                        this.tbx_FrameSize.Enabled = true;
                        this.updateBtn.Enabled = true;

                        this.tbx_ResultView.Text += "\r\nTx stop -- " + DateTime.Now.ToLongTimeString();
                        
                        return;
                    }
                       
                }
                
                this.timer_UART.Enabled = false;                       
                flag_reTx = false;
                flag_FrameRecv = false;
                if (flag_uartTxTimeOn)
                    break;

                prgsBar.PerformStep();
            }                                           //for end

            if (flag_uartTxTimeOn)
            {
                this.tbx_ResultView.Text += "\r\nTx failed!--" + DateTime.Now.ToLongTimeString()+"\r\n";
                this.updateBtn.Enabled = true;
                this.sendNextFrameBtn.Enabled = true;
                this.sendFullDataBtn.Enabled = true;    //一次性发送按钮可用
                this.frameRSTBtn.Enabled = true;
                this.tbx_FrameSize.Enabled = true;

                flag_reTx = false;
                flag_FrameRecv = false;

                this.isIgnoreSpData = true;
                return;
            }

            this.stopUpdateBtn.Enabled = false;
            this.flag_stopAutoIAPTx = false;
            this.tbx_ResultView.Text += "\r\nTx completed."+DateTime.Now.ToLongTimeString();
            this.tbx_ResultView.Text += "\r\n-----------------------------------\r\n";

            this.picBoxOn.Visible = true;
            this.picBoxOff.Visible = false;
            this.sendNextFrameBtn.Enabled = true;
            this.tbx_FrameSize.Enabled = true;
            this.updateBtn.Enabled = true;
            this.frameRSTBtn.Enabled = true;
            this.isIgnoreSpData = true;  //忽略串口数据
        }

        /*检查 源地址，目标地址，数据包大小是否已经填好*/
        private bool checkParaConfig()
        {
            if (this.tbx_SrcAddr.Text == "" || this.tbx_TargetAddr.Text == "" || this.tbx_FrameSize.Text == "")  //没有填入配置
                return false;
            if (this.tbx_SrcAddr.Text.Length == 1)  //最高位补0
                this.tbx_SrcAddr.Text = "0" + this.tbx_SrcAddr.Text;
            if (this.tbx_TargetAddr.Text.Length == 1)
                this.tbx_TargetAddr.Text = "0" + this.tbx_TargetAddr.Text;
            if(this.tbx_FrameSize.Text.Length<4)
            {
                switch(this.tbx_FrameSize.Text.Length)
                {
                    case 1:
                        this.tbx_FrameSize.Text = "000" + this.tbx_FrameSize.Text;
                        break;
                    case 2:
                        this.tbx_FrameSize.Text = "00" + this.tbx_FrameSize.Text;
                        break;
                    case 3:
                        this.tbx_FrameSize.Text = "0" + this.tbx_FrameSize.Text;
                        break;
                    default:
                        break;
                }   
            }
            if (this.tbx_FrameSize.Text == "0000")
                return false;
            return true;
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            //byte[] data = new byte[2];
            //data[0] = 0x5a;
            //data[1] = 0x45;
            //BCPHeader h = new BCPHeader();
            //h.UseDefaultHeader();
            //BCP_FrameHandler bcp_test = new BCP_FrameHandler(h,data);
            //byte[] testFrame = null;
            //bcp_test.getBCP_Frame(ref testFrame);
            //sp.Write(testFrame, 0, testFrame.Length); //数据写入缓冲


            //for(int i=0;i<10;i++)
            //{
            //    TextBox TextBox1 = new TextBox();
            //    TextBox1.Text = "";
            //    TextBox1.Size = new Size(50, 50);
            //    TextBox1.Margin = new System.Windows.Forms.Padding(0);

            //    Label Label_1 = new Label();
            //    Label_1.Text = i.ToString();

            //    FlowLayoutPanel flp = new FlowLayoutPanel();
            //    flp.Width = 200;//宽度
            //    flp.Height = 60;//高度           
            //    flp.AutoScroll = false; //增加自动滚动条
            //    flp.FlowDirection = FlowDirection.LeftToRight;//自上而下的流布局
            //    flp.WrapContents = true;//不截断内容



            //    flp.Controls.Add(TextBox1);//添加标签进容器
            //    flp.GetFlowBreak(TextBox1);//获取流中断

            //    flp.Controls.Add(Label_1);
            //    flp.GetFlowBreak(Label_1);


            //    this.flwLyPan_RegList.Controls.Add(flp);
            //    this.flwLyPan_RegList.GetFlowBreak(flp);
                
            //}

            byte[] val;
            byte addr = 0;
            string str_name = "";   //寄存器名
            string str_val = "";    //寄存器值
            string str_addr = "";   //寄存器地址
            string temp = "";

            for (int i = 0; i < this.regRecList.Count; i++)
            {
                val = new byte[this.regRecList[i].regValArr.Length];
                temp = "";
                for (int j = 0; j < val.Length; j++)
                {
                    this.regRecList[i].regValArr[j] = byte.Parse(this.regRecList[i].tbxHexVal.Text.Substring(2 * j, 2),  //更新寄存器值
                        System.Globalization.NumberStyles.HexNumber);

                    val[j] = this.regRecList[i].regValArr[j];       //获取寄存器值
                    temp += String.Format("{0:X2}", val[j]);
                }
                addr = this.regRecList[i].regAddr;     //获取寄存器地址
                str_name = this.regRecList[i].regName;     //获取寄存器名


                str_val = "0x" + temp;
                str_addr = "0x" + String.Format("{0:X2}", addr);

                System.Diagnostics.Debug.WriteLine("Name: " + str_name + "\r\n");
                System.Diagnostics.Debug.WriteLine("Val: " + str_val + "\r\n");
                System.Diagnostics.Debug.WriteLine("addr: " + str_addr +"\r\n\r\n");
            }
        }
        /*计时器*/
        private void timer_UART_Tick(object sender, EventArgs e)
        {
            this.uartReSendTimeOnCount++;
            this.uartSendTimeOnCount++;
            if (this.uartReSendTimeOnCount == 5 && (!this.flag_FrameRecv ) && uartSendTimeOnCount != 10 )  //5s后重发
            {
                this.uartReSendTimeOnCount = 0;
                this.flag_reTx = true;
                this.timer_UART.Enabled = false;
                
            }
            if(this.uartSendTimeOnCount == 10)  //10s后确认写入失败
            {
                this.flag_uartTxTimeOn = true;
                this.uartReSendTimeOnCount = 0;
                this.uartSendTimeOnCount = 0;
                this.timer_UART.Enabled = false;
            }
                
        }

        private void clearResultBtn_Click(object sender, EventArgs e)
        {
            this.tbx_ResultView.Text = "";
        }

        /*手动发送下一个数据包*/
        private void sendNextFrameBtn_Click(object sender, EventArgs e)
        {
            this.sendMode = MainForm.frameSendMode;
            if (!this.checkParaConfig())
            {
                MessageBox.Show("配置错误。请确认是否填入配置，并确保配置合法。", "ERROR");
                return;
            }
            

            MultiTypeNumber m1 = new MultiTypeNumber(); //将文本框的字符串数据转为byte类型
            m1.setByteNumber(this.tbx_TargetAddr.Text, MultiTypeNumber.STR_HEX);
            byte targetAddr = m1.getByteNumber_Byte();
            m1.setByteNumber(this.tbx_SrcAddr.Text, MultiTypeNumber.STR_HEX);
            byte srcAddr = m1.getByteNumber_Byte();
          
            if(this.isFrameDataSizeChanged)
            {
                this.myFileHandler.fileData.setDataFrameSize(int.Parse(this.tbx_FrameSize.Text));
                this.isFrameDataSizeChanged = false;
            }

            if (this.myFileHandler.fileData.getTotalFrameNum() == this.myFileHandler.fileData.getCurrentFrameNum())    //发送完毕
            {
                
                this.myFileHandler.fileData.frameRST();  //重新载入
                this.myFileHandler.read_configFile();
                configData = new byte[myFileHandler.get_fileByteCounts()];
                myFileHandler.get_configData(configData);
                this.myFileHandler.fileData.setCurrentFrameNum(0);
                this.myFileHandler.fileData.setDataFrameSize(int.Parse(this.tbx_FrameSize.Text));  //提供一帧大小，计算出一共多少帧

                flag_reTx = false;
                flag_FrameRecv = false;

                this.tbx_currentFrame.Text = "未发送任何数据包";
                //this.updateBtn.Enabled = true;  //自动发送按钮可用
            }
            else                                      //发送下一帧
            {

                
                this.picBoxOn.Visible = false;
                this.picBoxOff.Visible = true;

                this.myFileHandler.fileData.loadNextFrame(targetAddr,srcAddr);  //发送下一帧
                this.myFileHandler.fileData.sendFrame(sp);
                //this.currentFrameNum++;

                this.tbx_currentFrame.Text = this.myFileHandler.fileData.getCurrentFrameNum().ToString();
                this.tbx_ResultView.Text += "\r\nTransmiting Frame #" 
                    + this.myFileHandler.fileData.getCurrentFrameNum().ToString() + "...";
                
            }

        }

        private void FrameSizeTbx_TextChanged(object sender, EventArgs e)
        {
            this.isFrameDataSizeChanged = true; //数据包大小有改变
        }

        /*针对只能输入16进制数字的文本框--检测是否输入了非法字符,并自动加入空格*/
        private void HexRichTbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键 
            {
                bool check_result_1 = (e.KeyChar >= '0') && (e.KeyChar <= '9');
                bool check_result_2 = (e.KeyChar >= 'a') && (e.KeyChar <= 'f');
                bool check_result_3 = (e.KeyChar >= 'A') && (e.KeyChar <= 'F');
                if (check_result_1 || check_result_2 || check_result_3)//这是允许输入0-9数字 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                    MessageBox.Show("请勿输入非法字符。\r\n" +
                    "合法字符为 0~9,abcdef,ABCDEF,'\\b'(退格键)", "ERROR");
                }
            }
            else
            {
                if (this.InputDataTbx.Text == "")
                {
                    e.Handled = true;
                }
                else if (this.InputDataTbx.Text.Substring(this.InputDataTbx.Text.Length - 1, 1) == " ")   //最后一个字符是空格时，把空格去掉，否则会在keypress事件里又补上一个空格
                {
                    this.InputDataTbx.TextChanged -= new System.EventHandler(this.RichTbx_TextChanged);
                    this.InputDataTbx.Text = this.InputDataTbx.Text.Substring(0, this.InputDataTbx.Text.Length - 1);
                    this.InputDataTbx.Select(this.InputDataTbx.Text.Length, 0);//选择文本末尾位置
                    e.Handled = false;
                    this.InputDataTbx.TextChanged += new System.EventHandler(this.RichTbx_TextChanged);
                }
                else
                {
                    e.Handled = true;
                }
            }
 
            
        }
        /*针对只能输入十进制数字的文本框--检测是否输入了非法字符*/
        private void OctTbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            char[] legalKeyChar ={
                                     '0','1','2','3','4','5',
                                     '6','7','8','9',
                                     (char)8                       //退格键
                                };
            bool legalFlag = false;
            for (byte i = 0; i < legalKeyChar.Length; i++)
            {
                if (e.KeyChar == legalKeyChar[i])
                {
                    legalFlag = true;
                    break;
                }
            }
            if (!legalFlag)
            {
                e.Handled = true; //无效化刚才的事件
                MessageBox.Show("请勿输入非法字符。\r\n" +
                    "合法字符为 0~9,'\\b'(退格键)", "ERROR");
            }
        }

        private void ResultViewTbx_TextChanged(object sender, EventArgs e)
        {
            this.tbx_ResultView.SelectionStart = this.tbx_ResultView.Text.Length;
            this.tbx_ResultView.SelectionLength = 0;
            this.tbx_ResultView.ScrollToCaret();
        }

        private void stopUpdateBtn_Click(object sender, EventArgs e)
        {
            this.flag_stopAutoIAPTx = true;
            this.stopUpdateBtn.Enabled = false;
            this.tbx_ResultView.Text += "\r\n传输任务已停止--" + DateTime.Now.ToLongTimeString()+"\r\n\r\n";
        }

        private void frameRSTBtn_Click(object sender, EventArgs e)
        {
            this.myFileHandler.fileData.frameRST();  //重新载入
            this.myFileHandler.read_configFile();
            configData = new byte[myFileHandler.get_fileByteCounts()];
            myFileHandler.get_configData(configData);
            this.myFileHandler.fileData.setDataFrameSize(int.Parse(this.tbx_FrameSize.Text));  //提供一帧大小，计算出一共多少帧
            this.tbx_currentFrame.Text = "未发送任何数据包";
            this.tbx_ResultView.Text += "\r\n已重新载入数据。传输任务将从第一个数据包开始重新发送--" + DateTime.Now.ToLongTimeString();
        }
        /*画tab控件的样式*/
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font fntTab;
            Brush bshBack;
            Brush bshFore;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;

            if (e.Index != this.tabControl1.SelectedIndex)    //当前Tab页的样式
            {
                fntTab = new Font(e.Font, FontStyle.Bold);
                bshBack = new System.Drawing.Drawing2D.LinearGradientBrush(e.Bounds, SystemColors.Control, SystemColors.Control, System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal);
                bshFore = Brushes.Black;
            }
            else   //其余Tab页样式
            {
                fntTab = e.Font;
                bshBack = new SolidBrush(Color.LightSkyBlue);
                bshFore = new SolidBrush(Color.Black);
            }
            //画样式
            Rectangle recTab = e.Bounds;
            string tabName = this.tabControl1.TabPages[e.Index].Text;
            recTab = new Rectangle(recTab.X, recTab.Y, recTab.Width, recTab.Height);
            e.Graphics.FillRectangle(bshBack, e.Bounds);
            e.Graphics.DrawString(tabName, new Font("微软雅黑", 10), bshFore, recTab, sf);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            tabControl1.ItemSize = new Size(tabControl1.Size.Width / (tabControl1.TabCount + 1), 20);  //设置tabcontrol1中的tabpage占的宽度一样，并占满tabcontrol的宽度

            //判断文件的存在
            if (System.IO.File.Exists(cfgFileName))
            {
                //存在文件
                BinaryReader br;
                br = new BinaryReader(new FileStream(cfgFileName,
                FileMode.Open));

                for (int i = 0; i < this.tBxHeadList.Count; i++)
                {
                    this.tBxHeadList[i].Text = br.ReadString();
                }

                string str = br.ReadString();
                if (str.Equals(str_hasCom)) //存在串口
                {
                    string s = br.ReadString();
                    this.cBx_sp.Text = s;
                    this.cBx_sp.Items.Add(s);
                    this.cBx_sp.SelectedIndex = 0;
                    this.comAvailable = true;
                    spOpenBtn.Enabled = true;

                    if (isSpSelected())//检测串口设置
                    {
                        if (!isSpPropertySet)//串口未设置则设置串口
                        {
                            setSpProperties();
                            isSpPropertySet = true;
                        }
                        try//打开串口
                        {
                            sp.Open();
                            isSpOpen = true;
                            spOpenBtn.Text = "关闭串口";
                            spOpenBtn.BackColor = Color.LimeGreen;
                            //串口打开后则相关的串口设置按钮便不可再用
                            this.cBx_sp.Enabled = false;

                            this.rbtn_Baud9600.Enabled = false;
                            this.rbtn_Baud115200.Enabled = false;
                        }
                        catch (Exception)
                        {
                            //打开串口失败后，相应标志位取消
                            isSpPropertySet = false;
                            isSpOpen = false;

                            this.rbtn_Baud9600.Enabled = false;
                            this.rbtn_Baud115200.Enabled = false;
                        }
                    }
                    else
                    {
                        this.cBx_sp.Items.Clear();
                        this.cBx_sp.Text = "未检测到可用串口";
                        comAvailable = false;

                        this.rbtn_Baud9600.Enabled = true;
                        this.rbtn_Baud115200.Enabled = true;
                    }
                }
                br.Close();
            }
            else
            {
                //不存在文件
                this.FrameHeadTbx.Text = "7E";
                this.PortocolNumTbx.Text = "00";
                this.VersionNumTbx.Text = "00";
                this.TargetAddrTbx.Text = "FF";
                this.SourceAddrTbx.Text = "00";
                this.PortNumTbx.Text = "00";
                this.ControlCodeTbx.Text = "01";
                this.StartRegAddr_L_Tbx.Text = "00";
                this.StartRegAddr_H_Tbx.Text = "00";
                spInit();
            }

            // 显示进度条控件.
            prgsBar.Visible = true;
            // 设置进度条最小值.
            prgsBar.Minimum = 1;
            // 设置进度条最大值.
            prgsBar.Maximum = 15;
            // 设置进度条初始值
            prgsBar.Value = 1;
            // 设置每次增加的步长
            prgsBar.Step = 1;

        }

        /*一次性发送按钮*/
        private void sendFullDataBtn_Click(object sender, EventArgs e)
        {
            this.sendMode = MainForm.fullSendMode;
            if (isTab2TbxEmpty())
            {
                MessageBox.Show("存在未输入的文本框。", "ERROR");
                return;
            }
            normalizeTab2Tbx();
            if(!this.isSpOpen)
            {
                MessageBox.Show("串口未打开","ERROR");
                    return;
            }
            string[] data_str = null;                //获取文本框中的数据
            string temp = this.InputDataTbx.Text.Substring(0, this.InputDataTbx.Text.Length - 1); //去掉末尾的空格
            data_str = temp.Split(' ');           
            byte[] data_byte = new byte[data_str.Length];
            MultiTypeNumber m1 = new MultiTypeNumber();
            for ( uint i =0;i<data_str.Length;i++)
            {
               m1.setByteNumber(data_str[i],MultiTypeNumber.STR_HEX);
               data_byte[i] = m1.getByteNumber_Byte();
            }
            data_str = null;

            BCPHeader h = new BCPHeader();          //将数据按照协议组装成一帧
            m1.setByteNumber(this.FrameHeadTbx.Text, MultiTypeNumber.STR_HEX);
            h.Frame_Head = m1.getByteNumber_Byte();

            m1.setByteNumber(this.PortocolNumTbx.Text, MultiTypeNumber.STR_HEX);
            h.Protocal_Num = m1.getByteNumber_Byte();

            m1.setByteNumber(this.VersionNumTbx.Text, MultiTypeNumber.STR_HEX);
            h.Version_Num = m1.getByteNumber_Byte();

            m1.setByteNumber(this.TargetAddrTbx.Text, MultiTypeNumber.STR_HEX);
            h.Target_Addr = m1.getByteNumber_Byte();

            m1.setByteNumber(this.SourceAddrTbx.Text, MultiTypeNumber.STR_HEX);
            h.Source_Addr = m1.getByteNumber_Byte();

            m1.setByteNumber(this.PortNumTbx.Text, MultiTypeNumber.STR_HEX);
            h.Port_Num = m1.getByteNumber_Byte();

            m1.setByteNumber(this.ControlCodeTbx.Text, MultiTypeNumber.STR_HEX);
            h.Control_Code = m1.getByteNumber_Byte();

            m1.setByteNumber(this.StartRegAddr_L_Tbx.Text, MultiTypeNumber.STR_HEX);
            h.StartReg_Addr_L = m1.getByteNumber_Byte();

            m1.setByteNumber(this.StartRegAddr_H_Tbx.Text, MultiTypeNumber.STR_HEX);
            h.StartReg_Addr_H = m1.getByteNumber_Byte();
            
            FrameIAP frame = new FrameIAP();
            frame.enterData(h, data_byte);
            frame.sp_FrameSend(this.sp);    //发送数据

            m1.setByteNumber(frame.IAP_frame[10],MultiTypeNumber.STR_HEX_HAS_HEAD);          //将数据长度和CRC结果显示到文本框上
            this.DataLength_L_Tbx.Text = m1.getByteNumber_Hex();

            m1.setByteNumber(frame.IAP_frame[11], MultiTypeNumber.STR_HEX_HAS_HEAD);
            this.DataLength_H_Tbx.Text = m1.getByteNumber_Hex();

            m1.setByteNumber(frame.IAP_frame[frame.IAP_frame.Length - 2], MultiTypeNumber.STR_HEX_HAS_HEAD);
            this.CRC_L_Tbx.Text = m1.getByteNumber_Hex();

            m1.setByteNumber(frame.IAP_frame[frame.IAP_frame.Length - 1], MultiTypeNumber.STR_HEX_HAS_HEAD);
            this.CRC_H_Tbx.Text = m1.getByteNumber_Hex();

            this.tbx_ResultView.Text += "\r\nTX:" + (data_byte.Length+14).ToString() + 
                " Bytes" + "--" + DateTime.Now.ToLongTimeString();  //显示发送时间
            this.picBoxOff.Visible = true;
            this.picBoxOn.Visible = false;
        }

        /*整理好协议配置文本框中的数据*/
        public void normalizeTab2Tbx()
        {
            TextBox[] tbxTab2 = {
                                    this.FrameHeadTbx,this.PortocolNumTbx,this.VersionNumTbx,this.TargetAddrTbx,
                                    this.SourceAddrTbx,this.PortNumTbx,this.ControlCodeTbx,this.StartRegAddr_L_Tbx,
                                    this.StartRegAddr_H_Tbx
                                };
            for (uint i = 0; i < tbxTab2.Length; i++)
            {
                if (tbxTab2[i].Text.Length == 1)
                {
                    tbxTab2[i].Text = "0" + tbxTab2[i].Text;
  
                }
                
            }
        }
        /*
         * 检查文本框是否为空
         */
        public bool isTab2TbxEmpty()
        {
            bool flag = false;
            TextBox[] tbxTab2 = {
                                    this.FrameHeadTbx,this.PortocolNumTbx,this.VersionNumTbx,this.TargetAddrTbx,
                                    this.SourceAddrTbx,this.PortNumTbx,this.ControlCodeTbx,this.StartRegAddr_L_Tbx,
                                    this.StartRegAddr_H_Tbx,this.InputDataTbx
                                };
            for(uint i = 0;i<tbxTab2.Length;i++)
            {
                if(tbxTab2[i].Text == "")
                {
                    tbxTab2[i].BackColor = Color.Red;
                    flag = true;
                }
                else
                {
                    tbxTab2[i].BackColor = Color.White;
                }
            }
            return flag;
        }

        private void clearDataBtn_Click(object sender, EventArgs e)
        {
            this.InputDataTbx.Text = "";
        }

        private void RichTbx_TextChanged(object sender, EventArgs e)
        {
            if((this.InputDataTbx.Text.Length+1)%3 == 0)
            {
                this.InputDataTbx.Text += ' ';//追加空格
                this.InputDataTbx.Select(this.InputDataTbx.Text.Length, 0);//选择文本末尾位置
                this.InputDataTbx.ScrollToCaret();//将光标滚动到末尾位置，不然追加一个空格光标会跳转到开头位置
            }
        }

        /*主窗口关闭，保存协议的配置*/
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool flagNotComplete = false;       //配置是否不完整
            
            DialogResult WR = DialogResult.No;
            for(int i =0;i<this.tBxHeadList.Count;i++)
            {
                if(tBxHeadList[i].Text.Length == 0 || tBxHeadList[i].Text.Length == 1)
                {
                    WR = MessageBox.Show("协议配置填写不完整，继续退出将不保存配置，是否继续退出？", "警告",MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    flagNotComplete = true;
                    break;
                }
            };
            
            if(flagNotComplete)
            {
                if (WR == DialogResult.Yes)
                    e.Cancel = false;
                else
                    e.Cancel = true;

            }
            else
            {
                BinaryWriter bw;
                bw = new BinaryWriter(new FileStream(cfgFileName,
                    FileMode.Create));
                //写入文件
                for (int i = 0; i < this.tBxHeadList.Count;i++ )
                {
                    bw.Write(this.tBxHeadList[i].Text);
                }

                if(comAvailable)        //串口存在的话保存当前串口号至文件
                {
                    string str = str_hasCom;    //添加串口存在标记
                    bw.Write(str);
                    str = this.cBx_sp.Text;    //添加串口号
                    bw.Write(str);
                }
                else
                {
                    string str = str_noCom;
                    bw.Write(str);
                }
                bw.Close();
            }
        }
        /*一个字节的十六进制输入框按下事件*/
        private void HexByteTbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键 
            {
                bool check_result_1 = (e.KeyChar >= '0') && (e.KeyChar <= '9');
                bool check_result_2 = (e.KeyChar >= 'a') && (e.KeyChar <= 'f');
                bool check_result_3 = (e.KeyChar >= 'A') && (e.KeyChar <= 'F');
                if (check_result_1 || check_result_2 || check_result_3)//这是允许输入0-9数字 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                    MessageBox.Show("请勿输入非法字符。\r\n" +
                    "合法字符为 0~9,abcdef,ABCDEF,'\\b'(退格键)", "ERROR");
                }
            }
        }
        /*十进制输入框按下事件*/
        private void DecTbx_KeyPress(object sender,KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b')//这是允许输入退格键 
            {
                bool check_result_1 = (e.KeyChar >= '0') && (e.KeyChar <= '9');
                if (check_result_1)//这是允许输入0-9数字 
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                    MessageBox.Show("请勿输入非法字符。\r\n" +
                    "合法字符为 0~9,'\\b'(退格键)", "ERROR");
                }
            }
        }

        /*寻找寄存器记录里面最大值*/
        private byte findMaxAddr()
        {
            byte max = 0xff;  //0记录则返回ff
            if(this.regRecList.Count == 0)
            {
                max = 0xff;
            }
            else
            {
                max = regRecList[0].regAddr;
                for(int i=1;i<regRecList.Count;i++)
                {
                    if (regRecList[i].regAddr >= max)
                        max = regRecList[i].regAddr;
                }
            }

            return max;
        }
        private void btnSelRegConfig_Click(object sender, EventArgs e)
        {
            this.myFileHandler = new FileHandler(FileHandler.FILE_HEX);

            byte resultCode = myFileHandler.read_configFile();  //尝试读取文件一次
            if (resultCode != FileHandler.FILE_SELECTED)    //检查错误类型
            {
                switch(resultCode)
                {
                    case FileHandler.FILE_NOT_SELECTED:
                        this.tbx_SelectedFile.Text = "未选择文件，请重新选择。";  //没有选到文件
                        break;
                    case FileHandler.FILE_FORMAT_ILLEGAL:
                        this.tbx_SelectedFile.Text = "配置文件格式错误，请重新选择。";  //配置文件各式错误
                        break;
                    default:
                        break;
                }
                return;
            }

            this.btnSaveReg.Enabled = true;
            this.btnSendReg.Enabled = true;

            this.flwLyPan_RegList.Controls.Clear();         //清除原来的控件
            this.regRecList.Clear();                        //清除原来的寄存器记录
            this.tbx_SelectedFile.Text = myFileHandler.get_cfg_filename() + "\r\n";

            List<string> hexStringList = this.myFileHandler.getHexString();  //保存配置数据的链表
            int totalLine = hexStringList.Count / 4;  //行数

            Label labelName = new Label();
            labelName.Text = "寄存器名";

            Label labelHexVal = new Label();
            labelHexVal.Text = "寄存器值(HEX）";

            Label labelDecVal = new Label();
            labelDecVal.Text = "寄存器值(DEC）";

            Label labelComment = new Label();
            labelComment.Text = "注释";

            FlowLayoutPanel flp = new FlowLayoutPanel();
            flp.Width = this.flwLyPan_RegList.Width-10; //宽度
            flp.Height = labelName.Height;  //高度           
            flp.AutoScroll = false;     //不增加自动滚动条
            flp.FlowDirection = FlowDirection.LeftToRight;  //自左而右的流布局
            flp.WrapContents = false;   //不截断内容

            labelName.Margin = new Padding(5, 0, 72, 0);
            labelHexVal.Margin = new Padding(30, 0, 1, 0);
            labelDecVal.Margin = new Padding(0, 0, 5, 0);
            labelComment.Margin = new Padding(5, 0, 5, 0);

            flp.Controls.Add(labelName);    //添加标签进容器
            flp.GetFlowBreak(labelName);    //获取流中断
            flp.Controls.Add(labelHexVal);
            flp.GetFlowBreak(labelHexVal);
            flp.Controls.Add(labelDecVal);
            flp.GetFlowBreak(labelDecVal);
            flp.Controls.Add(labelComment);
            flp.GetFlowBreak(labelComment);

            this.flwLyPan_RegList.Controls.Add(flp);
            this.flwLyPan_RegList.GetFlowBreak(flp);

            uint regVal = 0;
            byte[] regValArr;
            byte regAddr;
            string str_RegComm = "";
            try
            {
                for (int i = 0; i < totalLine; i++)
                {
                    regVal = 0;
                    TextBox tbxHexVal = new TextBox();       //寄存器值
                    tbxHexVal.Size = new Size(80, 25);
                    tbxHexVal.Margin = new System.Windows.Forms.Padding(0);
                    tbxHexVal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexByteTbx_KeyPress);  //添加按下事件的处理
                    tbxHexVal.Multiline = true;
                    tbxHexVal.Font = new Font("幼圆", 12, tbxHexVal.Font.Style);

                    int strLen = hexStringList[4 * i + 2].Length - 2;                
                    tbxHexVal.Text = hexStringList[4 * i + 2].Substring(2, strLen);                                   
                    tbxHexVal.MaxLength = strLen;  //最大输入长度根据寄存器值的长度而定
                    tbxHexVal.TextChanged += new System.EventHandler(this.tbxHexVal_TextChanged);
                    int byteCounts = strLen / 2;    //字节数
                    
                    regValArr = new byte[byteCounts];
                    for(int j=0;j<byteCounts;j++)       //获取寄存器值--数组形式
                    {
                        regValArr[j] = byte.Parse(tbxHexVal.Text.Substring(2*j,2), 
                            System.Globalization.NumberStyles.HexNumber); //字符串转16进制                       
                    }
                    for(int j=0;j<byteCounts;j++)       //获取寄存器值
                    {
                        regVal <<= 8;
                        regVal = regVal | regValArr[j];
                    }

                    TextBox tbxDecVal = new TextBox();      //放10进制的值
                    tbxDecVal.Size = new Size(90, 25);
                    tbxDecVal.Margin = new System.Windows.Forms.Padding(0);
                    tbxDecVal.Text = regVal.ToString();
                    tbxDecVal.TextChanged += new System.EventHandler(this.tbxDecVal_TextChanged);
                    tbxDecVal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DecTbx_KeyPress);
                    tbxDecVal.Multiline = true;
                    tbxDecVal.Font = new Font("幼圆", 12, tbxDecVal.Font.Style);

                    Label labelRegName = new Label();
                    labelRegName.Text = hexStringList[4 * i + 1];          //寄存器名
                    labelRegName.Font = new Font("幼圆", 12, labelRegName.Font.Style);
                    labelRegName.AutoSize = false;       //设为false就能改变width，否则会根据text的大小改变width
                    labelRegName.Dock = DockStyle.Fill;
                    labelRegName.TextAlign = ContentAlignment.MiddleLeft;

                    //labelRegName.BackColor = Color.LimeGreen;
                    labelRegName.Width = 200;
                    

                    regAddr = byte.Parse(hexStringList[4 * i].Substring(2, 2),
                        System.Globalization.NumberStyles.HexNumber);  //寄存器地址


                    Label labelRegComment = new Label();
                    labelRegComment.Text = hexStringList[4 * i + 3];          //寄存器注释
                    str_RegComm = labelRegComment.Text;
                    //labelRegComment.BackColor = Color.LimeGreen;
                    labelRegComment.Width = this.flwLyPan_RegList.Width - tbxDecVal.Width - tbxHexVal.Width - labelRegName.Width-50;
                    labelRegComment.AutoSize = false;
                    labelRegComment.Dock = DockStyle.Fill;
                    labelRegComment.TextAlign = ContentAlignment.MiddleLeft;
                    
                    flp = new FlowLayoutPanel();
                    flp.Width = this.flwLyPan_RegList.Width - 10;//宽度
                              
                    flp.AutoScroll = false; //增加自动滚动条
                    flp.FlowDirection = FlowDirection.LeftToRight;//自上而下的流布局
                    flp.WrapContents = false;//不截断内容
                    flp.BackColor = Color.LightBlue;

                    labelRegName.Margin = new Padding(1, 0, 5, 0);
                    tbxHexVal.Margin = new Padding(5, 0,5, 0);
                    tbxDecVal.Margin = new Padding(5, 0, 5, 0);
                    labelRegComment.Margin = new Padding(5, 0, 2, 0);

                    flp.Controls.Add(labelRegName);
                    flp.GetFlowBreak(labelRegName);
                    flp.Controls.Add(tbxHexVal);//添加标签进容器
                    flp.GetFlowBreak(tbxHexVal);//获取流中断
                    flp.Controls.Add(tbxDecVal);
                    flp.GetFlowBreak(tbxDecVal);
                    flp.Controls.Add(labelRegComment);
                    flp.GetFlowBreak(labelRegComment);

                    flp.Height = labelRegComment.Height;//高度 
                   
                    this.flwLyPan_RegList.Controls.Add(flp);
                    this.flwLyPan_RegList.GetFlowBreak(flp);

                    RegRecord rec = new RegRecord();        //新增一条记录
                    rec.regName = labelRegName.Text;
                    rec.regValArr = regValArr;
                    rec.regVal = regVal;
                    rec.regAddr = regAddr;
                    rec.tbxHexVal = tbxHexVal;
                    rec.tbxDecVal = tbxDecVal;
                    rec.regComment = str_RegComm;
                    this.regRecList.Add(rec);

                }

                this.max_addr = findMaxAddr(); 
                System.Diagnostics.Debug.WriteLine(string.Format("{0:X2}", max_addr));
            }
            catch
            {
                MessageBox.Show("读取配置文件出错，请检查配置文件格式。", "ERROR");
            }

        }

        /*检查寄存器配置文本框是否填写正确*/
        private bool CheckRegConfig()
        {
            for (int i = 0; i < this.regRecList.Count; i++)
            {
                if (regRecList[i].tbxHexVal.Text == "")
                {
                    MessageBox.Show("存在未输入取值的寄存器,取消发送。\r\n请确保各寄存器都已输入取值", "ERROR");
                    return false;
                }
                int leftZeros = regRecList[i].tbxHexVal.MaxLength - regRecList[i].tbxHexVal.Text.Length;
                if (leftZeros > 0)
                {
                    for (int j = 0; j < leftZeros;j++ )     //添加leftzeros个0上去
                        regRecList[i].tbxHexVal.Text = "0" + regRecList[i].tbxHexVal.Text; 
                }
            }
            return true;
        }
        private void btnSendReg_Click(object sender, EventArgs e)
        {
            this.sendMode = MainForm.fullSendMode;
            if (!CheckRegConfig())      //检查配置完整性
                return;
            if(!isSpOpen)
            {
                MessageBox.Show("串口未打开，取消发送。\r\n请打开串口", "ERROR");
                return;
            }

            flag_stopAutoRegTx = false;
            this.btnSendReg.Enabled = false;
            this.sendMode = MainForm.fullSendMode;
            this.isIgnoreSpData = false;
            byte[] val;   //寄存器值
            byte addr = 0;  //寄存器地址

            // 设置进度条初始值
            prgsBar.Value = 1;
            prgsBar.Maximum = this.regRecList.Count+1;

            MultiTypeNumber m1 = new MultiTypeNumber();
            BCPHeader h = new BCPHeader();          //将数据按照协议组装成一帧
            FrameIAP frame = new FrameIAP();
            for (int i = 0; i < this.regRecList.Count; i++)     //读取配置文件里的数据
            {
                val = new byte[this.regRecList[i].regValArr.Length];
                for (int j = 0; j < val.Length;j++ )
                {
                    this.regRecList[i].regValArr[j] = byte.Parse(this.regRecList[i].tbxHexVal.Text.Substring(2*j,2),  //更新寄存器值
                        System.Globalization.NumberStyles.HexNumber);
                    val[j] = this.regRecList[i].regValArr[j];
                }

                this.tbx_ResultView.Text += "\r\nTx Reg:" + this.regRecList[i].regName +" :" + DateTime.Now.ToLongTimeString();


                int mid = (val.Length%2 == 0)?(val.Length / 2)-1 :(val.Length/2);   //寄存器值要低位先发，所以要头尾颠倒一下
                byte tmp = 0;
                for (int j = 0; j <= mid;j++ )
                {
                    tmp = val[val.Length - j - 1];
                    val[val.Length - j - 1] = val[j];
                    val[j] = tmp;
                }


                addr = this.regRecList[i].regAddr;
               
                m1.setByteNumber(this.FrameHeadTbx.Text, MultiTypeNumber.STR_HEX);
                h.Frame_Head = m1.getByteNumber_Byte();
                m1.setByteNumber(this.PortocolNumTbx.Text, MultiTypeNumber.STR_HEX);
                h.Protocal_Num = m1.getByteNumber_Byte();
                m1.setByteNumber(this.VersionNumTbx.Text, MultiTypeNumber.STR_HEX);
                h.Version_Num = m1.getByteNumber_Byte();
                m1.setByteNumber(this.TargetAddrTbx.Text, MultiTypeNumber.STR_HEX);
                h.Target_Addr = m1.getByteNumber_Byte();
                m1.setByteNumber(this.SourceAddrTbx.Text, MultiTypeNumber.STR_HEX);
                h.Source_Addr = m1.getByteNumber_Byte();
                m1.setByteNumber(this.PortNumTbx.Text, MultiTypeNumber.STR_HEX);
                h.Port_Num = m1.getByteNumber_Byte();

                h.Control_Code = Command.CTRLCODE_WRITE_REGISTER;      //write reg
                h.StartReg_Addr_L = addr;   //addr就是寄存器起始地址
                h.StartReg_Addr_H = 0x00;
              
                frame.enterData(h, val);
                frame.sp_FrameSend(this.sp);    //发送数据


                string temp = "";
                System.Diagnostics.Debug.WriteLine("Tx" + i.ToString() + "\r\n");
                for (int k = 0; k < frame.IAP_frame.Length; k++)
                {
                    temp += string.Format("{0:X2}", frame.IAP_frame[k]) + " ";
                }
                System.Diagnostics.Debug.WriteLine(temp + "\r\n");

                this.picBoxOff.Visible = true;
                this.picBoxOn.Visible = false;
                this.timer_RegCfg.Interval = 1000;
                this.regCfgTimeOnVal = 10;     //500ms
                this.regCfgTimeCount = 0;
                this.flag_RegTxTimeOn = false; //延时
                this.flag_RegCfgRecv = false;
                this.isCRCCorrect = true;
                this.timer_RegCfg.Enabled = true; //开定时器
                


                while (!(flag_RegCfgRecv || flag_RegTxTimeOn))      //等待回复
                {
                    Application.DoEvents();
                    if (!isCRCCorrect)
                        break;
                    if(flag_stopAutoRegTx)
                    {
                        this.tbx_ResultView.Text += "\r\nTx Reg stop -- " + DateTime.Now.ToLongTimeString();
                        return;
                    }
                }
                    
                this.flag_RegCfgRecv = false;
                this.regCfgTimeCount = 0;       //计数清零
                this.timer_RegCfg.Enabled = false;  //关计时器

                if(flag_RegTxTimeOn)
                {
                    this.tbx_ResultView.Text += "\r\nNo respond,Tx failed -- " +DateTime.Now.ToLongTimeString();
                    this.isIgnoreSpData = true;
                    break;
                }

                prgsBar.PerformStep();
            }
            if (!flag_RegTxTimeOn)
            {
                this.tbx_ResultView.Text += "\r\nConfig:" + (this.regRecList.Count).ToString() +
                            " Registers" + "--" + DateTime.Now.ToLongTimeString();  //显示发送时间
                this.picBoxOff.Visible = false;
                this.picBoxOn.Visible = true;
            }
         
            this.btnSendReg.Enabled = true;
            this.isIgnoreSpData = true;

        }

        private void timer_RegCfg_Tick(object sender, EventArgs e)
        {
            if(this.regCfgTimeCount==this.regCfgTimeOnVal)
            {
                this.timer_RegCfg.Enabled = false;
                this.regCfgTimeCount = 0;
                this.flag_RegTxTimeOn = true;
            }
            else
            {
                this.regCfgTimeCount++;
            }
        }
        /*保存配置文件*/
        private void btnSaveReg_Click(object sender, EventArgs e)
        {
            if (!CheckRegConfig())      //检查配置完整性
                return;

            byte[] val;
            byte addr = 0;
            string str_name = "";   //寄存器名
            string str_val = "";    //寄存器值
            string str_addr = "";   //寄存器地址
            string temp = "";

            SaveFileDialog file2 = new SaveFileDialog();
            file2.Filter = "文本文件|*.txt";
            if (file2.ShowDialog() == DialogResult.OK)
            {
                StreamWriter mySw = File.CreateText(file2.FileName);
                for (int i = 0; i < this.regRecList.Count; i++)
                {
                    val = new byte[this.regRecList[i].regValArr.Length];
                    temp = "";
                    for (int j = 0; j < val.Length; j++)
                    {
                        this.regRecList[i].regValArr[j] = byte.Parse(this.regRecList[i].tbxHexVal.Text.Substring(2 * j, 2),  //更新寄存器值
                            System.Globalization.NumberStyles.HexNumber);

                        val[j] = this.regRecList[i].regValArr[j];       //获取寄存器值
                        temp += String.Format("{0:X2}", val[j]);    
                    }
                    addr = this.regRecList[i].regAddr;     //获取寄存器地址
                    str_name = this.regRecList[i].regName;     //获取寄存器名

                    
                    str_val = "0x" + temp;
                    str_addr = "0x" + String.Format("{0:X2}", addr);

                    string line = str_addr + " " + str_name + " " + str_val + " "+this.regRecList[i].regComment+"\r\n";   //生成配置文件的一行--寄存器地址+寄存器命+寄存器值+寄存器注释
                    mySw.Write(line);
                    mySw.Flush();
                    
                }
                this.tbx_ResultView.Text += "\r\n已保存寄存器配置文件--" + DateTime.Now.ToLongTimeString() + "\r\n";
                mySw.Close();
            }
            else
            {
                return;
            }
            
        }

        /*hex的值改变，dec的值也改变*/
        private void tbxHexVal_TextChanged(object sender, EventArgs e)
        {
            //获取当前改变文本的Textbox
            TextBox thisTbx = sender as TextBox;
            int index = 0;
            for (int i = 0; i < this.regRecList.Count; i++)  //寻找当前tbx在record中的下标
            {
                if (this.regRecList[i].tbxHexVal == thisTbx)
                {
                    index = i;
                    break;
                }
            }

            this.regRecList[index].tbxDecVal.TextChanged -= new System.EventHandler(this.tbxDecVal_TextChanged); //避免又触发这个事件
            if (thisTbx.Text != "")                 //文本框非空
            {                            
                string temp = "";
                uint regVal = 0;
                byte[] regValArr;
                int leftZeros = thisTbx.MaxLength - thisTbx.Text.Length;
                int byteCounts = thisTbx.MaxLength / 2;

                temp = thisTbx.Text;
                if (leftZeros > 0)
                {                  
                    for (int j = 0; j < leftZeros; j++)     //添加leftzeros个0上去
                        temp = "0" + temp;
                }

                regValArr = new byte[byteCounts];
                for (int j = 0; j < byteCounts; j++)       //获取寄存器值--数组形式
                {
                    regValArr[j] = byte.Parse(temp.Substring(2 * j, 2),
                        System.Globalization.NumberStyles.HexNumber); //字符串转16进制                       
                }
                for (int j = 0; j < byteCounts; j++)       //获取寄存器值
                {
                    regVal <<= 8;
                    regVal = regVal | regValArr[j];
                }
                this.regRecList[index].tbxDecVal.Text = regVal.ToString();

            }
            else
            {
                this.regRecList[index].tbxDecVal.Text = "";
            }
            this.regRecList[index].tbxDecVal.TextChanged += new System.EventHandler(this.tbxDecVal_TextChanged);
        }

        /*dec的值改变，hex的值也改变*/
        private void tbxDecVal_TextChanged(object sender, EventArgs e)
        {
            //获取当前改变文本的Textbox
            TextBox thisTbx = sender as TextBox;
            int index = 0;
            for (int i = 0; i < this.regRecList.Count; i++)  //寻找当前tbx在record中的下标
            {
                if (this.regRecList[i].tbxDecVal == thisTbx)
                {
                    index = i;
                    break;
                }
            }

            this.regRecList[index].tbxHexVal.TextChanged -= new System.EventHandler(this.tbxHexVal_TextChanged);  //避免又触发这个事件
            long maxDecVal = 16;         //十进制文本框能最大表示的十进制数,注意数据类型不要溢出
            long decVal = 0;
            int byteCounts = 0;
            byte byteVal = 0;
            string strHexVal = "";
            if(thisTbx.Text != "")
            {
                for (int i = 1; i < this.regRecList[index].tbxHexVal.MaxLength; i++)
                    maxDecVal *= 16;
                maxDecVal -= 1;

                decVal = long.Parse(thisTbx.Text);
                
                if(decVal > maxDecVal)              //是否溢出
                {
                    this.regRecList[index].tbxHexVal.Text = "";
                }
                else
                {
                    long tempDecVal;
                    MultiTypeNumber m1 = new MultiTypeNumber();
                    byteCounts = this.regRecList[index].tbxHexVal.MaxLength / 2;
                    for(int i=0;i<byteCounts;i++)
                    {
                        tempDecVal = decVal >> ((byteCounts - i - 1) * 8);

                        byteVal = (byte)(0xff & tempDecVal);        //取最低8位的值
                        m1.setByteNumber(byteVal,MultiTypeNumber.STR_HEX_NO_HEAD);
                        strHexVal += m1.getByteNumber_Hex();
                    }
                    this.regRecList[index].tbxHexVal.Text = strHexVal;
                }

            }
            else
            {

            }
            this.regRecList[index].tbxHexVal.TextChanged += new System.EventHandler(this.tbxHexVal_TextChanged);
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                this.Size = new Size(this.Width * 2, this.Height);
            }
            else
            {
                this.Size = new Size(this.Width / 2, this.Height);
            }
        }

        private void rbtn_Baud115200_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbtn_Baud115200.Checked)
                this.bautRate = Convert.ToInt32(115200);
        }

        private void rbtn_Baud9600_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbtn_Baud9600.Checked)
                this.bautRate = Convert.ToInt32(9600);
        }

        private void btnReadReg_Click(object sender, EventArgs e)
        {
            this.sendMode = MainForm.readRegMode;

            if (!isSpOpen)
            {
                MessageBox.Show("串口未打开，取消发送。\r\n请打开串口", "ERROR");
                return;
            }

            MultiTypeNumber m1 = new MultiTypeNumber();
            BCPHeader h = new BCPHeader();          //将数据按照协议组装成一帧
            FrameIAP frame = new FrameIAP();

            m1.setByteNumber(this.FrameHeadTbx.Text, MultiTypeNumber.STR_HEX);
            h.Frame_Head = m1.getByteNumber_Byte();
            m1.setByteNumber(this.PortocolNumTbx.Text, MultiTypeNumber.STR_HEX);
            h.Protocal_Num = m1.getByteNumber_Byte();
            m1.setByteNumber(this.VersionNumTbx.Text, MultiTypeNumber.STR_HEX);
            h.Version_Num = m1.getByteNumber_Byte();
            m1.setByteNumber(this.TargetAddrTbx.Text, MultiTypeNumber.STR_HEX);
            h.Target_Addr = m1.getByteNumber_Byte();
            m1.setByteNumber(this.SourceAddrTbx.Text, MultiTypeNumber.STR_HEX);
            h.Source_Addr = m1.getByteNumber_Byte();
            m1.setByteNumber(this.PortNumTbx.Text, MultiTypeNumber.STR_HEX);
            h.Port_Num = m1.getByteNumber_Byte();


            h.Control_Code = Command.CTRLCODE_READ_REGISTER;  //read reg
            h.StartReg_Addr_L = 0x00;   //addr就是寄存器起始地址
            h.StartReg_Addr_H = 0x00;

            frame.enterData(h, 0x00, this.max_addr);        //读0~max_addr号寄存器，故长度设置位max_addr
            frame.sp_FrameSend(this.sp);    //发送数据


        }

        /// <summary>
        /// 打开文件对话框，选取二进制文件并且返回二进制文件的绝对地址
        /// </summary>
        /// <returns></returns>
        private string getFilename()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "请选择固件";
            dialog.Filter = "固件(*.bin*)|*.bin";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.FileName;
            }
            return "";
        }

        /// <summary>
        /// 给定文件绝对地址，读取文件的数据返回byte类型的数组
        /// </summary>
        /// <param name="filename">文件的绝对地址</param>
        /// <returns></returns>
        private byte[] getFileData(string filename)
        {
            FileStream fstream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            byte[] buff = new byte[(int)fstream.Length];
            int readBytes = fstream.Read(buff, 0, buff.Length);
            if (readBytes == 0)
            {
                return null;
            }
            fstream.Close();
            return buff;
        }

        /// <summary>
        /// 校验文件是否符合crc校验
        /// </summary>
        /// <param name="buff"></param>
        /// <returns></returns>
        private bool crcCorrect(byte[] buff)
        {
            CRC16 crc16 = new CRC16();
            crc16.CRC16_modbus(buff, buff.Length);
            if (crc16.getCRC16_L() == 0 && crc16.getCRC16_H() == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 同一路径下创建副本文件，在原文件名后添加后缀名，非格式后缀;比如 c:\\filename.bin -> c:\\filename_crc.bin ，suffix为_crc
        /// </summary>
        /// <param name="oldFilename">原文件绝对路径名</param>
        /// <param name="suffix">文件名的后缀名</param>
        /// <returns></returns>
        private string createDuplicateFileName(string oldFilename, string suffix)
        {
            int filenameStartIndex = oldFilename.LastIndexOf("\\");
            int filenameEndIndex = oldFilename.LastIndexOf(".");
            StringBuilder newFilenamePath = new StringBuilder(oldFilename.Substring(0, filenameStartIndex + 1));
            string suffixFormat = oldFilename.Substring(filenameEndIndex);
            string newFilename = oldFilename.Substring(filenameStartIndex + 1, filenameEndIndex - filenameStartIndex - 1);
            newFilenamePath.Append(newFilename + suffix + suffixFormat);
            return newFilenamePath.ToString();
        }

        /// <summary>
        /// 为新文件赋值原来的文件的值并且在文件末尾添加值
        /// </summary>
        /// <param name="filename">新文件绝对路径名</param>
        /// <param name="oldFileData">原文件数据</param>
        /// <param name="newValue">需要在末尾添加的新值</param>
        private void duplicateNewFileAndAppendValue(string filename, byte[] oldFileData, byte[] newValue)
        {
            FileStream fStream = new FileStream(filename, FileMode.Create);
            fStream.Write(oldFileData, 0, oldFileData.Length);
            // write 是在末尾添加数据
            fStream.Write(newValue, 0, newValue.Length);
            fStream.Close();
        }

        /// <summary>
        /// 选取一个目标文件，如果目标文件未添加crc16校验码，则添加；若添加了，则校验是否成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVerify_Click(object sender, EventArgs e)
        {
            // 获得文件的绝对地址
            string filename = this.getFilename();
            // 获得文件数据
            byte[] buff = this.getFileData(filename);
            if (buff == null)
            {
                MessageBox.Show("文件读取失败，请选择有效文件");
                return;
            }
            if (!this.crcCorrect(buff)) // 检验文件末尾是否有正确的crc校验值
            {
                if (MessageBox.Show("文件末尾无crc校验，是否添加crc校验", "校验",MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
                // 为新文件末尾添加crc校验
                string newFilename = this.createDuplicateFileName(filename, "_crc");
                CRC16 crc16 = new CRC16();
                crc16.CRC16_modbus(buff, buff.Length);
                this.duplicateNewFileAndAppendValue(newFilename, buff, crc16.getCRC16());
                MessageBox.Show("成功添加校验");
                return;
            }
            MessageBox.Show("文件末尾已有正确的crc校验");
        }
    }
}

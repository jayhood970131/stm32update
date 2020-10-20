namespace STM32Update
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.updateBtn = new System.Windows.Forms.Button();
            this.SelectFileBtn = new System.Windows.Forms.Button();
            this.tbx_ResultView = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.spOpenBtn = new System.Windows.Forms.Button();
            this.checkSpBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtn_Baud9600 = new System.Windows.Forms.RadioButton();
            this.rbtn_Baud115200 = new System.Windows.Forms.RadioButton();
            this.cBx_sp = new System.Windows.Forms.ComboBox();
            this.label_spSetUp = new System.Windows.Forms.Label();
            this.picBoxOff = new System.Windows.Forms.PictureBox();
            this.picBoxOn = new System.Windows.Forms.PictureBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.sendNextFrameBtn = new System.Windows.Forms.Button();
            this.tbx_SelectedFile = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timer_UART = new System.Windows.Forms.Timer(this.components);
            this.clearResultBtn = new System.Windows.Forms.Button();
            this.tbx_currentFrame = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbx_SrcAddr = new System.Windows.Forms.TextBox();
            this.tbx_TargetAddr = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbx_FrameSize = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.stopUpdateBtn = new System.Windows.Forms.Button();
            this.frameRSTBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.clearDataBtn = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.InputDataTbx = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.StartRegAddr_H_Tbx = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.StartRegAddr_L_Tbx = new System.Windows.Forms.TextBox();
            this.VersionNumTbx = new System.Windows.Forms.TextBox();
            this.PortocolNumTbx = new System.Windows.Forms.TextBox();
            this.FrameHeadTbx = new System.Windows.Forms.TextBox();
            this.CRC_H_Tbx = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.DataLength_H_Tbx = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.CRC_L_Tbx = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.DataLength_L_Tbx = new System.Windows.Forms.TextBox();
            this.ControlCodeTbx = new System.Windows.Forms.TextBox();
            this.PortNumTbx = new System.Windows.Forms.TextBox();
            this.SourceAddrTbx = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TargetAddrTbx = new System.Windows.Forms.TextBox();
            this.sendFullDataBtn = new System.Windows.Forms.Button();
            this.labelVerNum = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.flwLyPan_RegList = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSelRegConfig = new System.Windows.Forms.Button();
            this.btnSaveReg = new System.Windows.Forms.Button();
            this.btnSendReg = new System.Windows.Forms.Button();
            this.timer_RegCfg = new System.Windows.Forms.Timer(this.components);
            this.prgsBar = new System.Windows.Forms.ProgressBar();
            this.btnReadReg = new System.Windows.Forms.Button();
            this.btnVerify = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxOn)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // updateBtn
            // 
            this.updateBtn.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.updateBtn.Location = new System.Drawing.Point(267, 46);
            this.updateBtn.Name = "updateBtn";
            this.updateBtn.Size = new System.Drawing.Size(174, 58);
            this.updateBtn.TabIndex = 0;
            this.updateBtn.Text = "自动发送全部数据包";
            this.updateBtn.UseVisualStyleBackColor = true;
            this.updateBtn.Click += new System.EventHandler(this.UpdateBtn_Click);
            // 
            // SelectFileBtn
            // 
            this.SelectFileBtn.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SelectFileBtn.Location = new System.Drawing.Point(267, 242);
            this.SelectFileBtn.Name = "SelectFileBtn";
            this.SelectFileBtn.Size = new System.Drawing.Size(174, 58);
            this.SelectFileBtn.TabIndex = 1;
            this.SelectFileBtn.Text = "选择文件";
            this.SelectFileBtn.UseVisualStyleBackColor = true;
            this.SelectFileBtn.Click += new System.EventHandler(this.SelectFileBtn_Click);
            // 
            // tbx_ResultView
            // 
            this.tbx_ResultView.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbx_ResultView.Location = new System.Drawing.Point(8, 437);
            this.tbx_ResultView.Multiline = true;
            this.tbx_ResultView.Name = "tbx_ResultView";
            this.tbx_ResultView.ReadOnly = true;
            this.tbx_ResultView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbx_ResultView.Size = new System.Drawing.Size(633, 206);
            this.tbx_ResultView.TabIndex = 2;
            this.tbx_ResultView.TextChanged += new System.EventHandler(this.ResultViewTbx_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 422);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(605, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "发送结果:自动发送数据包模式下，5秒未收到回复则重发一次，距离第一次发送10秒后仍没有回复视为发送失败。";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "选择串口";
            // 
            // spOpenBtn
            // 
            this.spOpenBtn.Font = new System.Drawing.Font("幼圆", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.spOpenBtn.Location = new System.Drawing.Point(7, 117);
            this.spOpenBtn.Name = "spOpenBtn";
            this.spOpenBtn.Size = new System.Drawing.Size(102, 34);
            this.spOpenBtn.TabIndex = 8;
            this.spOpenBtn.Text = "打开串口";
            this.spOpenBtn.UseVisualStyleBackColor = true;
            this.spOpenBtn.Click += new System.EventHandler(this.spOpenBtn_Click);
            // 
            // checkSpBtn
            // 
            this.checkSpBtn.Font = new System.Drawing.Font("幼圆", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkSpBtn.Location = new System.Drawing.Point(6, 77);
            this.checkSpBtn.Name = "checkSpBtn";
            this.checkSpBtn.Size = new System.Drawing.Size(102, 34);
            this.checkSpBtn.TabIndex = 9;
            this.checkSpBtn.Text = "检测串口";
            this.checkSpBtn.UseVisualStyleBackColor = true;
            this.checkSpBtn.Click += new System.EventHandler(this.checkSpBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtn_Baud9600);
            this.groupBox1.Controls.Add(this.rbtn_Baud115200);
            this.groupBox1.Controls.Add(this.cBx_sp);
            this.groupBox1.Controls.Add(this.label_spSetUp);
            this.groupBox1.Controls.Add(this.checkSpBtn);
            this.groupBox1.Controls.Add(this.spOpenBtn);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.picBoxOff);
            this.groupBox1.Controls.Add(this.picBoxOn);
            this.groupBox1.Location = new System.Drawing.Point(492, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(149, 317);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "串口控制模块";
            // 
            // rbtn_Baud9600
            // 
            this.rbtn_Baud9600.AutoSize = true;
            this.rbtn_Baud9600.Location = new System.Drawing.Point(8, 180);
            this.rbtn_Baud9600.Name = "rbtn_Baud9600";
            this.rbtn_Baud9600.Size = new System.Drawing.Size(47, 16);
            this.rbtn_Baud9600.TabIndex = 18;
            this.rbtn_Baud9600.TabStop = true;
            this.rbtn_Baud9600.Text = "9600";
            this.rbtn_Baud9600.UseVisualStyleBackColor = true;
            this.rbtn_Baud9600.CheckedChanged += new System.EventHandler(this.rbtn_Baud9600_CheckedChanged);
            // 
            // rbtn_Baud115200
            // 
            this.rbtn_Baud115200.AutoSize = true;
            this.rbtn_Baud115200.Location = new System.Drawing.Point(8, 158);
            this.rbtn_Baud115200.Name = "rbtn_Baud115200";
            this.rbtn_Baud115200.Size = new System.Drawing.Size(59, 16);
            this.rbtn_Baud115200.TabIndex = 17;
            this.rbtn_Baud115200.TabStop = true;
            this.rbtn_Baud115200.Text = "115200";
            this.rbtn_Baud115200.UseVisualStyleBackColor = true;
            this.rbtn_Baud115200.CheckedChanged += new System.EventHandler(this.rbtn_Baud115200_CheckedChanged);
            // 
            // cBx_sp
            // 
            this.cBx_sp.FormattingEnabled = true;
            this.cBx_sp.Location = new System.Drawing.Point(6, 51);
            this.cBx_sp.Name = "cBx_sp";
            this.cBx_sp.Size = new System.Drawing.Size(102, 20);
            this.cBx_sp.TabIndex = 16;
            // 
            // label_spSetUp
            // 
            this.label_spSetUp.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_spSetUp.Location = new System.Drawing.Point(5, 209);
            this.label_spSetUp.Name = "label_spSetUp";
            this.label_spSetUp.Size = new System.Drawing.Size(127, 69);
            this.label_spSetUp.TabIndex = 15;
            this.label_spSetUp.Text = "波特率:115200";
            // 
            // picBoxOff
            // 
            this.picBoxOff.BackgroundImage = global::STM32Update.Properties.Resources.trafficlight_red_128px_1187358_easyicon_net;
            this.picBoxOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picBoxOff.Location = new System.Drawing.Point(45, 281);
            this.picBoxOff.Name = "picBoxOff";
            this.picBoxOff.Size = new System.Drawing.Size(31, 30);
            this.picBoxOff.TabIndex = 5;
            this.picBoxOff.TabStop = false;
            // 
            // picBoxOn
            // 
            this.picBoxOn.BackgroundImage = global::STM32Update.Properties.Resources.trafficlight_green_128px_1187357_easyicon_net;
            this.picBoxOn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picBoxOn.Location = new System.Drawing.Point(8, 281);
            this.picBoxOn.Name = "picBoxOn";
            this.picBoxOn.Size = new System.Drawing.Size(31, 30);
            this.picBoxOn.TabIndex = 4;
            this.picBoxOn.TabStop = false;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(288, 340);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(51, 29);
            this.btnTest.TabIndex = 14;
            this.btnTest.Text = "use for test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.button1_Click);
            // 
            // sendNextFrameBtn
            // 
            this.sendNextFrameBtn.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.sendNextFrameBtn.Location = new System.Drawing.Point(390, 340);
            this.sendNextFrameBtn.Name = "sendNextFrameBtn";
            this.sendNextFrameBtn.Size = new System.Drawing.Size(51, 24);
            this.sendNextFrameBtn.TabIndex = 16;
            this.sendNextFrameBtn.Text = "手动发送下一数据包";
            this.sendNextFrameBtn.UseVisualStyleBackColor = true;
            this.sendNextFrameBtn.Visible = false;
            this.sendNextFrameBtn.Click += new System.EventHandler(this.sendNextFrameBtn_Click);
            // 
            // tbx_SelectedFile
            // 
            this.tbx_SelectedFile.Location = new System.Drawing.Point(16, 295);
            this.tbx_SelectedFile.Multiline = true;
            this.tbx_SelectedFile.Name = "tbx_SelectedFile";
            this.tbx_SelectedFile.ReadOnly = true;
            this.tbx_SelectedFile.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbx_SelectedFile.Size = new System.Drawing.Size(245, 63);
            this.tbx_SelectedFile.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 281);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "文件名：";
            // 
            // timer_UART
            // 
            this.timer_UART.Interval = 1000;
            this.timer_UART.Tick += new System.EventHandler(this.timer_UART_Tick);
            // 
            // clearResultBtn
            // 
            this.clearResultBtn.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.clearResultBtn.Location = new System.Drawing.Point(488, 357);
            this.clearResultBtn.Name = "clearResultBtn";
            this.clearResultBtn.Size = new System.Drawing.Size(153, 58);
            this.clearResultBtn.TabIndex = 15;
            this.clearResultBtn.Text = "清除发送结果";
            this.clearResultBtn.UseVisualStyleBackColor = true;
            this.clearResultBtn.Click += new System.EventHandler(this.clearResultBtn_Click);
            // 
            // tbx_currentFrame
            // 
            this.tbx_currentFrame.Font = new System.Drawing.Font("幼圆", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbx_currentFrame.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tbx_currentFrame.Location = new System.Drawing.Point(16, 224);
            this.tbx_currentFrame.Multiline = true;
            this.tbx_currentFrame.Name = "tbx_currentFrame";
            this.tbx_currentFrame.ReadOnly = true;
            this.tbx_currentFrame.Size = new System.Drawing.Size(245, 54);
            this.tbx_currentFrame.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 28);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "源地址：0x";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "目标地址：0x";
            // 
            // tbx_SrcAddr
            // 
            this.tbx_SrcAddr.Location = new System.Drawing.Point(133, 25);
            this.tbx_SrcAddr.MaxLength = 2;
            this.tbx_SrcAddr.Name = "tbx_SrcAddr";
            this.tbx_SrcAddr.Size = new System.Drawing.Size(77, 21);
            this.tbx_SrcAddr.TabIndex = 20;
            this.tbx_SrcAddr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexRichTbx_KeyPress);
            // 
            // tbx_TargetAddr
            // 
            this.tbx_TargetAddr.Location = new System.Drawing.Point(133, 68);
            this.tbx_TargetAddr.MaxLength = 2;
            this.tbx_TargetAddr.Name = "tbx_TargetAddr";
            this.tbx_TargetAddr.Size = new System.Drawing.Size(77, 21);
            this.tbx_TargetAddr.TabIndex = 21;
            this.tbx_TargetAddr.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexRichTbx_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 12);
            this.label7.TabIndex = 22;
            this.label7.Text = "数据包大小：(Bytes)";
            // 
            // tbx_FrameSize
            // 
            this.tbx_FrameSize.Location = new System.Drawing.Point(133, 109);
            this.tbx_FrameSize.MaxLength = 4;
            this.tbx_FrameSize.Name = "tbx_FrameSize";
            this.tbx_FrameSize.Size = new System.Drawing.Size(77, 21);
            this.tbx_FrameSize.TabIndex = 23;
            this.tbx_FrameSize.TextChanged += new System.EventHandler(this.FrameSizeTbx_TextChanged);
            this.tbx_FrameSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OctTbx_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 209);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(137, 12);
            this.label8.TabIndex = 24;
            this.label8.Text = "当前已发送数据包包号：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbx_FrameSize);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.tbx_TargetAddr);
            this.groupBox2.Controls.Add(this.tbx_SrcAddr);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(18, 43);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(243, 154);
            this.groupBox2.TabIndex = 25;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "基本协议配置";
            // 
            // stopUpdateBtn
            // 
            this.stopUpdateBtn.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.stopUpdateBtn.Location = new System.Drawing.Point(267, 114);
            this.stopUpdateBtn.Name = "stopUpdateBtn";
            this.stopUpdateBtn.Size = new System.Drawing.Size(174, 58);
            this.stopUpdateBtn.TabIndex = 26;
            this.stopUpdateBtn.Text = "停止自动发送";
            this.stopUpdateBtn.UseVisualStyleBackColor = true;
            this.stopUpdateBtn.Click += new System.EventHandler(this.stopUpdateBtn_Click);
            // 
            // frameRSTBtn
            // 
            this.frameRSTBtn.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.frameRSTBtn.Location = new System.Drawing.Point(267, 180);
            this.frameRSTBtn.Name = "frameRSTBtn";
            this.frameRSTBtn.Size = new System.Drawing.Size(174, 56);
            this.frameRSTBtn.TabIndex = 27;
            this.frameRSTBtn.Text = "重新载入数据";
            this.frameRSTBtn.UseVisualStyleBackColor = true;
            this.frameRSTBtn.Click += new System.EventHandler(this.frameRSTBtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControl1.Location = new System.Drawing.Point(8, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(478, 407);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 28;
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnVerify);
            this.tabPage1.Controls.Add(this.stopUpdateBtn);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.btnTest);
            this.tabPage1.Controls.Add(this.frameRSTBtn);
            this.tabPage1.Controls.Add(this.SelectFileBtn);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.tbx_SelectedFile);
            this.tabPage1.Controls.Add(this.sendNextFrameBtn);
            this.tabPage1.Controls.Add(this.updateBtn);
            this.tabPage1.Controls.Add(this.tbx_currentFrame);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(470, 381);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "固件升级";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.clearDataBtn);
            this.tabPage2.Controls.Add(this.label19);
            this.tabPage2.Controls.Add(this.InputDataTbx);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.sendFullDataBtn);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(470, 381);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "寄存器配置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // clearDataBtn
            // 
            this.clearDataBtn.Location = new System.Drawing.Point(371, 333);
            this.clearDataBtn.Name = "clearDataBtn";
            this.clearDataBtn.Size = new System.Drawing.Size(93, 45);
            this.clearDataBtn.TabIndex = 29;
            this.clearDataBtn.Text = "清除数据";
            this.clearDataBtn.UseVisualStyleBackColor = true;
            this.clearDataBtn.Click += new System.EventHandler(this.clearDataBtn_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(3, 246);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(347, 12);
            this.label19.TabIndex = 28;
            this.label19.Text = "请输入十六进制数据：(文本框会自动添加空格，例：7F 43 2C )";
            // 
            // InputDataTbx
            // 
            this.InputDataTbx.Location = new System.Drawing.Point(6, 261);
            this.InputDataTbx.Multiline = true;
            this.InputDataTbx.Name = "InputDataTbx";
            this.InputDataTbx.Size = new System.Drawing.Size(359, 114);
            this.InputDataTbx.TabIndex = 27;
            this.InputDataTbx.TextChanged += new System.EventHandler(this.RichTbx_TextChanged);
            this.InputDataTbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexRichTbx_KeyPress);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.StartRegAddr_H_Tbx);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.StartRegAddr_L_Tbx);
            this.groupBox3.Controls.Add(this.VersionNumTbx);
            this.groupBox3.Controls.Add(this.PortocolNumTbx);
            this.groupBox3.Controls.Add(this.FrameHeadTbx);
            this.groupBox3.Controls.Add(this.CRC_H_Tbx);
            this.groupBox3.Controls.Add(this.label25);
            this.groupBox3.Controls.Add(this.DataLength_H_Tbx);
            this.groupBox3.Controls.Add(this.label24);
            this.groupBox3.Controls.Add(this.label22);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.CRC_L_Tbx);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.DataLength_L_Tbx);
            this.groupBox3.Controls.Add(this.ControlCodeTbx);
            this.groupBox3.Controls.Add(this.PortNumTbx);
            this.groupBox3.Controls.Add(this.SourceAddrTbx);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.TargetAddrTbx);
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(398, 237);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "协议设置";
            // 
            // StartRegAddr_H_Tbx
            // 
            this.StartRegAddr_H_Tbx.Location = new System.Drawing.Point(331, 74);
            this.StartRegAddr_H_Tbx.MaxLength = 2;
            this.StartRegAddr_H_Tbx.Name = "StartRegAddr_H_Tbx";
            this.StartRegAddr_H_Tbx.Size = new System.Drawing.Size(36, 21);
            this.StartRegAddr_H_Tbx.TabIndex = 34;
            this.StartRegAddr_H_Tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexByteTbx_KeyPress);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(198, 77);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(131, 12);
            this.label21.TabIndex = 33;
            this.label21.Text = "起始寄存器地址(高8位)";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(119, 184);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(29, 12);
            this.label17.TabIndex = 32;
            this.label17.Text = "0x00";
            // 
            // StartRegAddr_L_Tbx
            // 
            this.StartRegAddr_L_Tbx.Location = new System.Drawing.Point(331, 47);
            this.StartRegAddr_L_Tbx.MaxLength = 2;
            this.StartRegAddr_L_Tbx.Name = "StartRegAddr_L_Tbx";
            this.StartRegAddr_L_Tbx.Size = new System.Drawing.Size(36, 21);
            this.StartRegAddr_L_Tbx.TabIndex = 31;
            this.StartRegAddr_L_Tbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexByteTbx_KeyPress);
            // 
            // VersionNumTbx
            // 
            this.VersionNumTbx.Location = new System.Drawing.Point(121, 74);
            this.VersionNumTbx.MaxLength = 2;
            this.VersionNumTbx.Name = "VersionNumTbx";
            this.VersionNumTbx.Size = new System.Drawing.Size(36, 21);
            this.VersionNumTbx.TabIndex = 30;
            this.VersionNumTbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexByteTbx_KeyPress);
            // 
            // PortocolNumTbx
            // 
            this.PortocolNumTbx.Location = new System.Drawing.Point(121, 47);
            this.PortocolNumTbx.MaxLength = 2;
            this.PortocolNumTbx.Name = "PortocolNumTbx";
            this.PortocolNumTbx.Size = new System.Drawing.Size(36, 21);
            this.PortocolNumTbx.TabIndex = 29;
            this.PortocolNumTbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexByteTbx_KeyPress);
            // 
            // FrameHeadTbx
            // 
            this.FrameHeadTbx.Location = new System.Drawing.Point(121, 20);
            this.FrameHeadTbx.MaxLength = 2;
            this.FrameHeadTbx.Name = "FrameHeadTbx";
            this.FrameHeadTbx.Size = new System.Drawing.Size(36, 21);
            this.FrameHeadTbx.TabIndex = 28;
            this.FrameHeadTbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexByteTbx_KeyPress);
            // 
            // CRC_H_Tbx
            // 
            this.CRC_H_Tbx.Location = new System.Drawing.Point(331, 181);
            this.CRC_H_Tbx.Name = "CRC_H_Tbx";
            this.CRC_H_Tbx.ReadOnly = true;
            this.CRC_H_Tbx.Size = new System.Drawing.Size(36, 21);
            this.CRC_H_Tbx.TabIndex = 25;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(198, 184);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(101, 12);
            this.label25.TabIndex = 24;
            this.label25.Text = "CRC校验（高8位）";
            // 
            // DataLength_H_Tbx
            // 
            this.DataLength_H_Tbx.Location = new System.Drawing.Point(331, 128);
            this.DataLength_H_Tbx.Name = "DataLength_H_Tbx";
            this.DataLength_H_Tbx.ReadOnly = true;
            this.DataLength_H_Tbx.Size = new System.Drawing.Size(36, 21);
            this.DataLength_H_Tbx.TabIndex = 23;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(198, 131);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(107, 12);
            this.label24.TabIndex = 22;
            this.label24.Text = "数据长度（高8位）";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 181);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(29, 12);
            this.label22.TabIndex = 20;
            this.label22.Text = "保留";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(6, 77);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(41, 12);
            this.label20.TabIndex = 18;
            this.label20.Text = "版本号";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(6, 50);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(41, 12);
            this.label18.TabIndex = 16;
            this.label18.Text = "协议号";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 27);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(29, 12);
            this.label16.TabIndex = 14;
            this.label16.Text = "帧头";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(198, 157);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(101, 12);
            this.label15.TabIndex = 13;
            this.label15.Text = "CRC校验（低8位）";
            // 
            // CRC_L_Tbx
            // 
            this.CRC_L_Tbx.Location = new System.Drawing.Point(331, 154);
            this.CRC_L_Tbx.Name = "CRC_L_Tbx";
            this.CRC_L_Tbx.ReadOnly = true;
            this.CRC_L_Tbx.Size = new System.Drawing.Size(36, 21);
            this.CRC_L_Tbx.TabIndex = 13;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(198, 104);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(107, 12);
            this.label14.TabIndex = 12;
            this.label14.Text = "数据长度（低8位）";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(198, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(131, 12);
            this.label13.TabIndex = 11;
            this.label13.Text = "起始寄存器地址(低8位)";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(198, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 10;
            this.label12.Text = "控制码";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 157);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 9;
            this.label11.Text = "端口";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 131);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 8;
            this.label10.Text = "源地址";
            // 
            // DataLength_L_Tbx
            // 
            this.DataLength_L_Tbx.Location = new System.Drawing.Point(331, 101);
            this.DataLength_L_Tbx.Name = "DataLength_L_Tbx";
            this.DataLength_L_Tbx.ReadOnly = true;
            this.DataLength_L_Tbx.Size = new System.Drawing.Size(36, 21);
            this.DataLength_L_Tbx.TabIndex = 7;
            // 
            // ControlCodeTbx
            // 
            this.ControlCodeTbx.Location = new System.Drawing.Point(331, 20);
            this.ControlCodeTbx.MaxLength = 2;
            this.ControlCodeTbx.Name = "ControlCodeTbx";
            this.ControlCodeTbx.Size = new System.Drawing.Size(36, 21);
            this.ControlCodeTbx.TabIndex = 5;
            this.ControlCodeTbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexByteTbx_KeyPress);
            // 
            // PortNumTbx
            // 
            this.PortNumTbx.Location = new System.Drawing.Point(121, 155);
            this.PortNumTbx.MaxLength = 2;
            this.PortNumTbx.Name = "PortNumTbx";
            this.PortNumTbx.Size = new System.Drawing.Size(36, 21);
            this.PortNumTbx.TabIndex = 4;
            this.PortNumTbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexByteTbx_KeyPress);
            // 
            // SourceAddrTbx
            // 
            this.SourceAddrTbx.Location = new System.Drawing.Point(121, 128);
            this.SourceAddrTbx.MaxLength = 2;
            this.SourceAddrTbx.Name = "SourceAddrTbx";
            this.SourceAddrTbx.Size = new System.Drawing.Size(36, 21);
            this.SourceAddrTbx.TabIndex = 3;
            this.SourceAddrTbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexByteTbx_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "目标地址";
            // 
            // TargetAddrTbx
            // 
            this.TargetAddrTbx.Location = new System.Drawing.Point(121, 101);
            this.TargetAddrTbx.MaxLength = 2;
            this.TargetAddrTbx.Name = "TargetAddrTbx";
            this.TargetAddrTbx.Size = new System.Drawing.Size(36, 21);
            this.TargetAddrTbx.TabIndex = 0;
            this.TargetAddrTbx.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HexByteTbx_KeyPress);
            // 
            // sendFullDataBtn
            // 
            this.sendFullDataBtn.Location = new System.Drawing.Point(371, 282);
            this.sendFullDataBtn.Name = "sendFullDataBtn";
            this.sendFullDataBtn.Size = new System.Drawing.Size(93, 45);
            this.sendFullDataBtn.TabIndex = 26;
            this.sendFullDataBtn.Text = "发送";
            this.sendFullDataBtn.UseVisualStyleBackColor = true;
            this.sendFullDataBtn.Click += new System.EventHandler(this.sendFullDataBtn_Click);
            // 
            // labelVerNum
            // 
            this.labelVerNum.AutoSize = true;
            this.labelVerNum.Location = new System.Drawing.Point(1128, 634);
            this.labelVerNum.Name = "labelVerNum";
            this.labelVerNum.Size = new System.Drawing.Size(47, 12);
            this.labelVerNum.TabIndex = 29;
            this.labelVerNum.Text = "label23";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.flwLyPan_RegList);
            this.groupBox4.Location = new System.Drawing.Point(647, 35);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(642, 538);
            this.groupBox4.TabIndex = 30;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "寄存器列表";
            // 
            // flwLyPan_RegList
            // 
            this.flwLyPan_RegList.Location = new System.Drawing.Point(7, 21);
            this.flwLyPan_RegList.Name = "flwLyPan_RegList";
            this.flwLyPan_RegList.Size = new System.Drawing.Size(629, 511);
            this.flwLyPan_RegList.TabIndex = 0;
            // 
            // btnSelRegConfig
            // 
            this.btnSelRegConfig.Font = new System.Drawing.Font("幼圆", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelRegConfig.Location = new System.Drawing.Point(657, 579);
            this.btnSelRegConfig.Name = "btnSelRegConfig";
            this.btnSelRegConfig.Size = new System.Drawing.Size(107, 45);
            this.btnSelRegConfig.TabIndex = 1;
            this.btnSelRegConfig.Text = "选择配置文件";
            this.btnSelRegConfig.UseVisualStyleBackColor = true;
            this.btnSelRegConfig.Click += new System.EventHandler(this.btnSelRegConfig_Click);
            // 
            // btnSaveReg
            // 
            this.btnSaveReg.Font = new System.Drawing.Font("幼圆", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSaveReg.Location = new System.Drawing.Point(770, 579);
            this.btnSaveReg.Name = "btnSaveReg";
            this.btnSaveReg.Size = new System.Drawing.Size(102, 45);
            this.btnSaveReg.TabIndex = 31;
            this.btnSaveReg.Text = "保存配置文件";
            this.btnSaveReg.UseVisualStyleBackColor = true;
            this.btnSaveReg.Click += new System.EventHandler(this.btnSaveReg_Click);
            // 
            // btnSendReg
            // 
            this.btnSendReg.Font = new System.Drawing.Font("幼圆", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSendReg.Location = new System.Drawing.Point(878, 578);
            this.btnSendReg.Name = "btnSendReg";
            this.btnSendReg.Size = new System.Drawing.Size(80, 45);
            this.btnSendReg.TabIndex = 32;
            this.btnSendReg.Text = "发送";
            this.btnSendReg.UseVisualStyleBackColor = true;
            this.btnSendReg.Click += new System.EventHandler(this.btnSendReg_Click);
            // 
            // timer_RegCfg
            // 
            this.timer_RegCfg.Tick += new System.EventHandler(this.timer_RegCfg_Tick);
            // 
            // prgsBar
            // 
            this.prgsBar.Location = new System.Drawing.Point(1075, 600);
            this.prgsBar.Name = "prgsBar";
            this.prgsBar.Size = new System.Drawing.Size(214, 23);
            this.prgsBar.TabIndex = 33;
            // 
            // btnReadReg
            // 
            this.btnReadReg.Font = new System.Drawing.Font("幼圆", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadReg.Location = new System.Drawing.Point(964, 579);
            this.btnReadReg.Name = "btnReadReg";
            this.btnReadReg.Size = new System.Drawing.Size(80, 45);
            this.btnReadReg.TabIndex = 34;
            this.btnReadReg.Text = "读取";
            this.btnReadReg.UseVisualStyleBackColor = true;
            this.btnReadReg.Click += new System.EventHandler(this.btnReadReg_Click);
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(267, 311);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(174, 23);
            this.btnVerify.TabIndex = 28;
            this.btnVerify.Text = "校验文件";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 655);
            this.Controls.Add(this.btnReadReg);
            this.Controls.Add(this.prgsBar);
            this.Controls.Add(this.btnSendReg);
            this.Controls.Add(this.btnSaveReg);
            this.Controls.Add(this.btnSelRegConfig);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.labelVerNum);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.clearResultBtn);
            this.Controls.Add(this.tbx_ResultView);
            this.Name = "MainForm";
            this.Text = "寄存器配置-V1.16";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxOn)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button updateBtn;
        private System.Windows.Forms.Button SelectFileBtn;
        private System.Windows.Forms.TextBox tbx_ResultView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox picBoxOn;
        private System.Windows.Forms.PictureBox picBoxOff;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button spOpenBtn;
        private System.Windows.Forms.Button checkSpBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbx_SelectedFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Timer timer_UART;
        private System.Windows.Forms.Button clearResultBtn;
        private System.Windows.Forms.Button sendNextFrameBtn;
        private System.Windows.Forms.TextBox tbx_currentFrame;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbx_SrcAddr;
        private System.Windows.Forms.TextBox tbx_TargetAddr;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbx_FrameSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button stopUpdateBtn;
        private System.Windows.Forms.Button frameRSTBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox TargetAddrTbx;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox DataLength_L_Tbx;
        private System.Windows.Forms.TextBox PortNumTbx;
        private System.Windows.Forms.TextBox SourceAddrTbx;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox CRC_L_Tbx;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox CRC_H_Tbx;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox DataLength_H_Tbx;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button sendFullDataBtn;
        private System.Windows.Forms.TextBox ControlCodeTbx;
        private System.Windows.Forms.TextBox VersionNumTbx;
        private System.Windows.Forms.TextBox PortocolNumTbx;
        private System.Windows.Forms.TextBox FrameHeadTbx;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox StartRegAddr_L_Tbx;
        private System.Windows.Forms.TextBox InputDataTbx;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox StartRegAddr_H_Tbx;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button clearDataBtn;
        private System.Windows.Forms.Label label_spSetUp;
        private System.Windows.Forms.ComboBox cBx_sp;
        private System.Windows.Forms.Label labelVerNum;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.FlowLayoutPanel flwLyPan_RegList;
        private System.Windows.Forms.Button btnSelRegConfig;
        private System.Windows.Forms.Button btnSaveReg;
        private System.Windows.Forms.Button btnSendReg;
        private System.Windows.Forms.Timer timer_RegCfg;
        private System.Windows.Forms.RadioButton rbtn_Baud9600;
        private System.Windows.Forms.RadioButton rbtn_Baud115200;
        private System.Windows.Forms.ProgressBar prgsBar;
        private System.Windows.Forms.Button btnReadReg;
        private System.Windows.Forms.Button btnVerify;
    }
}


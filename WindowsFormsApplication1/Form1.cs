﻿/*****************************************************
 * ProjectName: 310程序
 * Description:
 * ClassName:  
 * Author:      
 * NameSpace:    
 * MachineName:  310
 * CreateTime:   2018/9/
 * UpdatedTime:  2018/9/
*****************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading; //引用多线程
using HalconDotNet;
using System.IO; //创建文件流
using System.Security.Cryptography;
using System.Runtime.InteropServices; //打开软键盘 
using System.IO.Ports;
using CCWin;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;
using System.Diagnostics; //检测代码运行时间
using System.Management; //获取CPU序列号  ，另还要将其 引用后，才能才这里using
using System.Drawing.Drawing2D;
using MathCollect;
using System.Web.Services;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private SerialPort ComDevice = new SerialPort(); //串口通信使用
        public static Form1 frm1 = null;
        private float X, Y;

        #region  程序初始化

        /// <summary>
        /// 程序初始化
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            frm1 = this;
        }

        #endregion

        #region  全局变量

        System.Diagnostics.Process p = new System.Diagnostics.Process(); //开启另外一个软件用的东西
        //Boolean boreadPictureModel = false;  //读取上一次打开的模板   读取成功返回true
        string Forbidden;
        //string path1="";
        int houjia = 0;
        string a3;
        int a33 = 0;
        string yunxing = "";
        int liaohao;
        string lhsj = "";


        bool shenghe = false;

        string Barcode = "";
        string Barcodeweizhi = "";
        string scmj = "";
        string Mixture1 = "0";

        string NG = "";
        int tongxing = 0;
        int tongxingplc = 0;
        string a4;
        bool PNL = false;
        string gap11; //
        string gap12; // 
        string gap13; //间距1
        string gap14; //间距2
        string gap15; //间距
        string gap16; //间距2
        string gap17; //间距
        string gap18; //MOLD3
        string gap19; //保养数
        //string bj;


        string shuliang;
        int a6;
        bool saomatouk = false; //扫码判断
        bool saomatoug = false; //扫码NG
        bool mj = false;
        bool saomatouh = false; //混料
        bool a11 = false;

        int a = 0;
        int by = 0;
        bool by1 = false;

        // bool time3 = false;

        int saoma1;
        string saoma2;
        bool saoma3;

        int mold3;
        string sm3 = "";


        int sm = 0;
        int sm1 = 0;
        int sm2 = 0;


        string Batch; //批号
        string Lay; //层别
        string Mold; //模具编号
        string Auditor; //审核人

        string Job; //作业
        string machine; //机台号

        bool Auditor10 = false; //审核人
        bool job10 = false; //作业员

        string name1; //名称
        string name2;
        string name3;

        string value1; //数据
        string value2;
        string value3;
        string value4;



        bool Run = false; //运行

        //   DataSet PaperNo;

        WebApplication1.WebReference.Service1 webFun = new WebApplication1.WebReference.Service1(); //连接客户服务器

        string filePathNow = ""; //当前型号的路径
      
       
        string[] fileModelStr = new string[20]; //储存分析得到的 型号名称
        List<string> fileModelList = new List<string>(); //储存分析得到的 型号名称

        Boolean closeSoftware = false; //关机赋值true

        string[] TCP_IP = new string[2]; //TCP IP地址
        //--------------------------------------------------------

        int[] PLC_MS = new int[200]; //IO状态 读取 原始数据
        int[] PLC_DS = new int[200]; //寄存器
        ushort[] PLC_DS1 = new ushort[20]; //读取数据格式信捷PLC
        //ushort qq;
        ushort qq2;
        // -------------------------------------------------
        int RunningStation = 0; //运行状态记录
        string Running = ""; //扫码头更新

        #endregion

        #region  串口

        private void button53_Click(object sender, EventArgs e)
        {
            textBox2.TabIndex = 0;
            textBox2.Focus();
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {

            ComDevice.DataReceived += new SerialDataReceivedEventHandler(Com_DataReceived); //绑定事件
            if (ComDevice.IsOpen == false)
            {
                ComDevice.PortName = Properties.Settings.Default.PortNumber;
                ComDevice.BaudRate = 115200;
                ComDevice.Parity = Parity.None;
                ComDevice.DataBits = 8;
                ComDevice.StopBits = StopBits.One;
                try
                {
                    ComDevice.Open();
                    btnSend.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                btnOpen.Text = "关闭串口";

            }
            else
            {
                try
                {
                    ComDevice.Close();
                    btnSend.Enabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                btnOpen.Text = "打开串口";

            }

        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void ClearSelf()
        {
            if (ComDevice.IsOpen)
            {
                ComDevice.Close();
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public bool SendData(byte[] data)
        {
            if (ComDevice.IsOpen)
            {
                try
                {
                    ComDevice.Write(data, 0, data.Length); //发送数据
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        /// <summary>
        /// 发送数据button事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            //  Savepn(textBox33.Text);

            txtShowData.Clear(); //清空一下数据

            byte[] sendData = null;

            label340.Text = ""; //用于区分通过，混料，NG

            sendData = Encoding.ASCII.GetBytes("1"); //发送1
            if (this.SendData(sendData)) //发送数据成功计数
            {
                lblSendCount.Invoke(new MethodInvoker(delegate
                {
                    lblSendCount.Text = (int.Parse(lblSendCount.Text)).ToString();
                }));
            }
            else
            {

            }

        }

        /// <summary>
        /// 字符串转换16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private byte[] strToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length%2) != 0) hexString += " ";
            byte[] returnBytes = new byte[hexString.Length/2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i*2, 2).Replace(" ", ""), 16);
            return returnBytes;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] ReDatas = new byte[ComDevice.BytesToRead];
            ComDevice.Read(ReDatas, 0, ReDatas.Length); //读取数据
            this.AddData(ReDatas); //输出数据
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="data">字节数组</param>
        public void AddData(byte[] data)
        {
            AddContent(new ASCIIEncoding().GetString(data));

            //if (rbtnHex.Checked)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    for (int i = 0; i < data.Length; i++)
            //    {
            //        sb.AppendFormat("{0:x2}" + " ", data[i]);
            //    }
            //    AddContent(sb.ToString().ToUpper());
            //}
            //else if (rbtnASCII.Checked)
            //{
            //    AddContent(new ASCIIEncoding().GetString(data));
            //}
            //else if (rbtnUTF8.Checked)
            //{
            //    AddContent(new UTF8Encoding().GetString(data));
            //}
            //else if (rbtnUnicode.Checked)
            //{
            //    AddContent(new UnicodeEncoding().GetString(data));
            //}
            //else
            //{ }

            lblRevCount.Invoke(new MethodInvoker(delegate
            {
                lblRevCount.Text = ((int.Parse(lblRevCount.Text) + (data.Length)).ToString());
            }));
        }


        /// <summary>
        /// 输入到显示区域
        /// </summary>
        /// <param name="content"></param>
        private void AddContent(string content)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (chkAutoLine.Checked && txtShowData.Text.Length > 0)
                {
                    txtShowData.AppendText("\r\n");
                }
                txtShowData.AppendText(content); //添加日期后，会出现多次日期。
            }));
            //if (content.Length >= 18)
            //{
            //    string[] content1 = Regex.Split(content, "MF", RegexOptions.IgnoreCase);//分割读取数组数值
            //      content1[]
            //}
            //   string[] sArray3 = Regex.Split(str3, ",", RegexOptions.IgnoreCase);

            if (content.Length >= 2 && label340.Text != content)
            {
                sm3 = content;
             
            }



        }

        /// <summary>
        /// 清空接收区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearRev_Click(object sender, EventArgs e)
        {
            txtShowData.Clear();
        }

        #endregion

        #region 窗体加载

        /// <summary>
        /// 初始化PC和测试系统通讯的服务器、初始化PC和四轴机器人客户端、初始化PC和逻辑编程器PLC连接
        /// 初始化视觉系统、读取文本数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Existfile()
        {
            if (Directory.Exists(@"D:\Data")) //判断是否有这个文件夹--D:\数据备份\
            {

            }
            else
            {
                Directory.CreateDirectory(@"D:\Data");
            }

            if (Directory.Exists(@"D:\Data backup")) //判断是否有这个文件夹--E:\数据备份\
            {
                if (Directory.Exists(@"D:\Data backup\\" + System.DateTime.Now.ToString("yyyyMMdd")))
                    //判断是否有这个文件夹--E:\数据备份\
                {
                    //if (File.Exists(@"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\alarm.txt"))
                    //{

                    //}
                    //else
                    //{

                    //}
                }
                else
                {
                    Directory.CreateDirectory(@"D:\Data backup\\" + System.DateTime.Now.ToString("yyyyMMdd")); //没有就创建
                   
                    Erasing();
                }
            }
            else
            {
                Directory.CreateDirectory(@"D:\Data backup");
                Directory.CreateDirectory(@"D:\Data backup\\" + System.DateTime.Now.ToString("yyyyMMdd")); //没有就创建
                Erasing();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Existfile(); //判断是否存在 data 以及data backup文件夹
            Register.Init();       
            //读取config数据
            getConfigMsg(); //读取config数据
      

            //--------------------------------------------------Start
            Control.CheckForIllegalCrossThreadCalls = false; //线程间控件操作有效
            //--------------------------------------------------------End

            //--------------------------------------------------Start
            //初始化PC和四轴机器人客户端、初始化PC和逻辑编程器PLC

            //btnOpen_Click(null, null);//打开串口扫描

            if (!PcConnectPlc.OpenPortFxUsb())
            {
                MessageBox.Show("plc连接失败");
                //初始化失败
                Application.Exit(); //退出程序
                return;
            }

            if (PcConnectPlc.Read_Data_FxUsb("M8000", 1) != 1)
            {
                //初始化失败
                MessageBox.Show("电脑USB线链接PLC失败！"); //提示PC和逻辑编程器PLC  初始化失败原因
                Application.Exit(); //退出程序
                return;
            }

            //--------------------------------------------------------End
            textBox11.Text = machineidentifier; //机台编号
            //--------------------------------------------------Start
            //更新逻辑编程器PLC和四轴机器人的工位信息,获得相机名称
            //  this.Text = "测试机自动送料系统";
            this.Text = "KF系统";
            //--------------------------------------------------------End

            // form4.Close();
            timer1.Enabled = true; //和三菱PLC循环更新数据 
            timer4.Enabled = true;


        




            PcConnectPlc.Write_Data_FxUsb("M2904", 1); // 关PLC
            //  Thread.Sleep(100);

            PcConnectPlc.Write_Data_FxUsb("M2888", 0); //打开标志位

            string[] msgS = new string[500]; //读取数据
            string filePath = Application.StartupPath.ToString() + "\\liaohao.txt";

            FileOperate.OpenFileString(filePath, out msgS);

            //msgS数组读取到的数据

            if (msgS[0] == "1") //上一次型号路径
            {
                if (MessageBox.Show("是否恢复到上次未结单前?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    string[] lh = Regex.Split(msgS[1], ",", RegexOptions.IgnoreCase); //分割读取数组数值
                    textBox23.Text = lh[0]; //
                    label352.Text = lh[1]; //
                    textBox14.Text = lh[2]; //
                    textBox17.Text = lh[3]; //
                    textBox16.Text = lh[4]; //
                    textBox12.Text = lh[5]; //
                    textBox20.Text = lh[6]; //

                    string[] lh1 = Regex.Split(msgS[2], ",", RegexOptions.IgnoreCase); //分割读取数组数值
                    label367.Text = lh1[0]; //
                    textBox71.Text = lh1[1]; //
                    textBox69.Text = lh1[2]; //
                    textBox70.Text = lh1[3]; //
                    label354.Text = lh1[4]; //
                    textBox24.Text = lh1[5]; //
                    textBox21.Text = lh1[6]; //

                    string[] lh2 = Regex.Split(msgS[3], ",", RegexOptions.IgnoreCase); //分割读取数组数值
                    textBox5.Text = lh2[0]; //
                    textBox18.Text = lh2[1]; //
                    textBox29.Text = lh2[2]; //
                    textBox26.Text = lh2[3]; //
                    textBox79.Text = lh2[4]; //
                    textBox77.Text = lh2[5]; //

                    string[] lh3 = Regex.Split(msgS[4], ",", RegexOptions.IgnoreCase); //分割读取数组数值
                    label22.Text = lh3[0]; //
                    textBox80.Text = lh3[1]; //
                    textBox83.Text = lh3[2]; //
                    textBox27.Text = lh3[3]; //
                    textBox64.Text = lh3[4]; //
                    textBox25.Text = lh3[5]; //
                    textBox15.Text = lh3[6]; //

                    string[] lh4 = Regex.Split(msgS[5], ",", RegexOptions.IgnoreCase); //分割读取数组数值
                    textBox96.Text = lh4[0]; //
                    label5.Text = lh4[1]; //
                    textBox95.Text = lh4[2]; //
                    label6.Text = lh4[3]; //
                    textBox19.Text = lh4[4]; //
                    //textBox84.Text = lh4[5];//

                    string[] lh5 = Regex.Split(msgS[6], ",", RegexOptions.IgnoreCase); //分割读取数组数值
                    label93.Text = lh5[0]; //
                    label94.Text = lh5[1]; //
                    label358.Text = lh5[2]; //
                    label353.Text = lh5[3]; //
                    label29.Text = lh5[4]; //
                    label26.Text = lh5[5]; //

                    string[] lh6 = Regex.Split(msgS[7], ",", RegexOptions.IgnoreCase); //分割读取数组数值
                    //textBox84.Text = lh6[0];//
                    shuliang = lh6[0]; //
                    textBox79.Text = lh6[1]; //
                    //radioButton11.Checked = lh6[2];//
                    if (lh6[2] == "true")
                    {
                        radioButton11.Checked = true;
                        radioButton12.Checked = false;
                    }
                    else
                    {
                        radioButton11.Checked = false;
                        radioButton12.Checked = true;
                    }
                    //msgL[7] = textBox84.Text + "," + textBox79.Text + "," + radioButton11.Checked;


                    button42.BackColor = Color.Green;
                    button25.Enabled = false;
                    button42.Enabled = true;

                    textBox14.Enabled = false; //开始后对应输入控件不可输入
                    textBox17.Enabled = false;
                    textBox24.Enabled = false;
                    textBox15.Enabled = false;
                    textBox25.Enabled = false;
                    textBox32.Enabled = false;
                    textBox31.Enabled = false;
                    textBox37.Enabled = false;
                    skinGroupBox32.Enabled = false;
                    skinGroupBox37.Enabled = false;
                    textBox96.Enabled = false;
                    textBox95.Enabled = false;

                    houjia = 0;
                    shenghe = true;
                    label24.Text = a33.ToString();
                    Run = true;
                    PcConnectPlc.Write_Data_FxUsb("M2903", 1); //可以运行

                }
            }
        }

        private void loadingForm4()
        {
            Application.Run(new Form4());

        }

        #endregion

        #region 保存产品料号---

        private void Savepn(string pn)
        {
            try
            {
                //获取config信息
                string[] msgS = new string[500];
                string filePath = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\pn.txt";
                FileOperate.OpenFileString(filePath, out msgS);

                for (int i = 0; i < 499; i++)
                {
                    if (Running != msgS[0])
                    {
                        //最新的信息显示在第一行
                        // msgS[0] = pn;
                        pn = msgS[0].Insert(0,
                            DateTime.Now.ToString() + "   " + textBox33.Text + "   " + label341.Text + "   " +
                            label342.Text + "   " + label343.Text + "\r\n");
                    }
                    Running = msgS[0]; //1
                }

                FileOperate.SaveFileString(filePath, msgS);
                //  msgS.close();
            }
            catch
            {
            }
        }

        #endregion

        #region 误删新增功能---

        private void Erasing()
        {
            try
            {
                string path = "";
                Directory.CreateDirectory(@"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd"));
                for (int a = 0; a <= 4; a++)
                {
                    switch (a)
                    {
                        case 0:
                            path = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\alarm.txt";
                            break;
                        case 1:
                            path = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\Mixture.txt";
                            break;
                        case 2:
                            path = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\Mold match.txt";
                            break;
                        case 3:
                            path = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\send.txt";
                            break;
                        case 4:
                            path = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\pn.txt";
                            break;
                        //case 5:
                        //    path = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\Fool-Proof.txt";
                        //    break;
                    }

                    try
                    {

                        // Delete the file if it exists.
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }

                        // Create the file.
                        using (FileStream fs = File.Create(path))
                        {
                            Byte[] info =
                                new UTF8Encoding(true).GetBytes(System.DateTime.Now.ToString("yyyyMMdd") + "," + "创建");

                            fs.Write(info, 0, info.Length);
                        }

                        // Open the stream and read it back.
                        using (StreamReader sr = File.OpenText(path))
                        {
                            string sa = "";
                            while ((sa = sr.ReadLine()) != null)
                            {
                                Console.WriteLine(sa);
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region 模具数据及相关记录---

        private void MOLD()
        {
            string path = "";
            if (File.Exists(@"D:\Data\\" + textBox21.Text + ".txt")) //判断是否有这个文件夹--d:\数据备份\
            {
                filePathNow = @"D:\Data\\" + textBox21.Text + ".txt";
                path = @"D:\Data\\" + textBox21.Text + ".txt";
                showModelToForm(); ///显示到界面
                getFileMsg(path);
                label67.BackColor = Color.Transparent;

            }
            else
            {
                MessageBox.Show("第一次生产，请调试机台！");

                //label67.BackColor = Color.Green;
                label67.BackColor = Color.Red;

                path = @"D:\Data\\" + textBox21.Text + ".txt";
                try
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }

                    using (FileStream fs = File.Create(path))
                    {
                        Byte[] info =
                            new UTF8Encoding(true).GetBytes(System.DateTime.Now.ToString("yyyyMMdd") + "," + "创建");

                        fs.Write(info, 0, info.Length);
                    }

                    using (StreamReader sr = File.OpenText(path))
                    {
                        string sa = "";
                        while ((sa = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(sa);
                        }
                    }
                    if (PLC_DS[42] != 0 && PLC_DS[52] != 0)
                    {

                        textBox27.Text = (PLC_DS[64]/100f).ToString(); //写入
                        //PcConnectPlc.Write_Data_FxUsb("D900", Convert.ToInt32(sArray18[0]));//间距1

                        textBox34.Text = (PLC_DS[66]/100f).ToString(); //写入
                        //PcConnectPlc.Write_Data_FxUsb("D2946", Convert.ToInt32(sArray18[1]));//间距2

                        textBox36.Text = (PLC_DS[68]/100f).ToString(); //写入
                        //PcConnectPlc.Write_Data_FxUsb("D2948", Convert.ToInt32(sArray18[2]));//间距3

                        textBox35.Text = (PLC_DS[70]/100f).ToString(); //写入
                        //PcConnectPlc.Write_Data_FxUsb("D2950", Convert.ToInt32(sArray18[3]));//间距4

                        textBox40.Text = (PLC_DS[72]/100f).ToString(); //写入
                        //PcConnectPlc.Write_Data_FxUsb("D2952", Convert.ToInt32(sArray18[4]));//间距5

                        textBox39.Text = (PLC_DS[74]/100f).ToString(); //写入
                        //PcConnectPlc.Write_Data_FxUsb("D2954", Convert.ToInt32(sArray18[5]));//间距6


                        textBox38.Text = (PLC_DS[76]/100f).ToString();
                        ; //写入
                        //PcConnectPlc.Write_Data_FxUsb("D2956", Convert.ToInt32(sArray19[0]));//间距7

                        textBox37.Text = (PLC_DS[78]/100f).ToString(); //写入
                        //PcConnectPlc.Write_Data_FxUsb("D2958", Convert.ToInt32(sArray19[1]));//间距8

                        textBox52.Text = (PLC_DS[80]/100f).ToString(); //写入
                        //PcConnectPlc.Write_Data_FxUsb("D2960", Convert.ToInt32(sArray19[2]));//间距9

                        textBox51.Text = (PLC_DS[82]/100f).ToString(); //写入
                        //PcConnectPlc.Write_Data_FxUsb("D2962", Convert.ToInt32(sArray19[3]));//间距10

                        textBox44.Text = (PLC_DS[84]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2964", Convert.ToInt32(sArray19[4]));//间距11

                        textBox43.Text = (PLC_DS[86]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2966", Convert.ToInt32(sArray19[5]));//间距12


                        textBox42.Text = (PLC_DS[88]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2968", Convert.ToInt32(sArray20[0]));//间距13
                        //////
                        textBox41.Text = (PLC_DS[90]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2970", Convert.ToInt32(sArray20[1]));//间距14

                        textBox48.Text = (PLC_DS[92]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2972", Convert.ToInt32(sArray20[2]));//间距15

                        textBox47.Text = (PLC_DS[94]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2974", Convert.ToInt32(sArray20[3]));//间距16

                        textBox46.Text = (PLC_DS[96]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2976", Convert.ToInt32(sArray20[4]));//间距17

                        textBox45.Text = (PLC_DS[98]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2978", Convert.ToInt32(sArray20[5]));//间距18

                        textBox50.Text = (PLC_DS[100]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2980", Convert.ToInt32(sArray20[6]));//间距19

                        textBox49.Text = (PLC_DS[102]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2982", Convert.ToInt32(sArray20[7]));//间距20


                        //textBox60.Text = (PLC_DS[36] / 100f).ToString();//写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2916", Convert.ToInt32(sArray21[0]));//出

                        textBox59.Text = (PLC_DS[38]/48f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2918", Convert.ToInt32(sArray21[1]));//右

                        textBox58.Text = (PLC_DS[40]/48f).ToString();
                        ////PcConnectPlc.Write_Data_FxUsb("D2920", Convert.ToInt32(sArray21[2]));//取

                        ////// numericUpDown94.Value = Convert.ToDecimal(Convert.ToInt32(sArray21[3]) / 48f);//写入
                        textBox57.Text = (PLC_DS[42]/48f).ToString();
                        ; //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2922", Convert.ToInt32(sArray21[3]));//装

                        //////  numericUpDown93.Value = Convert.ToDecimal(Convert.ToInt32(sArray21[4]) / 48f);//写入
                        textBox56.Text = (PLC_DS[44]/48f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2924", Convert.ToInt32(sArray21[4]));//第

                        ////// numericUpDown92.Value = Convert.ToDecimal(Convert.ToInt32(sArray21[5]) / 48f);//写入
                        textBox55.Text = (PLC_DS[46]/48f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2926", Convert.ToInt32(sArray21[5]));//左

                        //////  numericUpDown100.Value = Convert.ToDecimal(Convert.ToInt32(sArray21[6]) / 48f);//写入
                        textBox54.Text = (PLC_DS[48]/48f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2928", Convert.ToInt32(sArray21[6]));//下

                        ////// numericUpDown99.Value = Convert.ToDecimal(Convert.ToInt32(sArray21[7]) / 100f);//写入
                        textBox53.Text = (PLC_DS[50]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2930", Convert.ToInt32(sArray21[7]));//取

                        ////// numericUpDown98.Value = Convert.ToDecimal(Convert.ToInt32(sArray22[0]) / 100f);//写入
                        textBox64.Text = (PLC_DS[52]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2932", Convert.ToInt32(sArray22[0]));//手

                        //////  numericUpDown97.Value = Convert.ToDecimal(Convert.ToInt32(sArray22[1]) / 100f);//写入
                        textBox63.Text = (PLC_DS[54]/100f).ToString(); //写入
                        ////PcConnectPlc.Write_Data_FxUsb("D2934", Convert.ToInt32(sArray22[1]));//左

                        textBox62.Text = (PLC_DS[56]/100f).ToString(); //写入
                        //PcConnectPlc.Write_Data_FxUsb("D2936", Convert.ToInt32(sArray22[2]));//下


                        textBox105.Text = PLC_DS[62].ToString(); //同间距

                        //25----不同间距1--同间距0
                        if (PLC_DS[2] == 0)
                        {
                            label27.Text = "0";
                            label28.Text = "同间距";

                        }
                        else
                        {
                            label27.Text = "1";
                            label28.Text = "不同间距";
                        }


                    }
                    else
                    {
                        MessageBox.Show("PLC连接异常，请确认正常后再次搜索！");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                filePathNow = path;

                showModelToForm(); ///显示到界面
            }
        }

        #endregion

        #region  读取config数据

        private string JDLaccount = Properties.Settings.Default.JDLaccount; //稼动率账号
        private string JDLpwd = Properties.Settings.Default.JDLpwd; //稼动率密码
        private string machineidentifier = Properties.Settings.Default.identifier; //机台编号
        private string serverAccount = Properties.Settings.Default.serverAccount; //服务器账号
        private string serverPwd = Properties.Settings.Default.serverPwd; //服务器密码
        private string barcodeAccount = Properties.Settings.Default.BarcodeAccount; //Barcode账号
        private string barcodePwd = Properties.Settings.Default.Barcodepwd; //Barcode密码
        private string SuperUser = Properties.Settings.Default.SuperUser; //超级管理员textbox8码 厂家
        private string enginerUser = Properties.Settings.Default.EnginerUser; //工程师 textbox8码
        private string operatorUser = Properties.Settings.Default.OperaterUser; //操作员textbox8码


        //读取配置文件方法
        private void getConfigMsg()
        {
            string[] msgS = new string[500];
            string filePath = Application.StartupPath.ToString() + "\\config.txt";
            FileOperate.OpenFileString(filePath, out msgS);

            string lastModel = Properties.Settings.Default.LastModelImformation; //上次模具信息
            string jmtj = Properties.Settings.Default.JMTJ; //架摸调机


            try
            {
                string[] lastModelArray = Regex.Split(lastModel, ",", RegexOptions.IgnoreCase); //上次模具信息拆分
                textBox82.Text = lastModelArray[0]; //
                textBox24.Text = lastModelArray[1]; //
                textBox21.Text = lastModelArray[2]; //
                textBox29.Text = lastModelArray[3]; //
                textBox18.Text = lastModelArray[4]; //
                label34.Text = lastModelArray[5]; //
                label35.Text = lastModelArray[6];
                liaohao = Convert.ToInt32(label35.Text);
            }
            catch
            {
            }
            try
            {
                string[] s5 = Regex.Split(jmtj, ",", RegexOptions.IgnoreCase); //加模具调机
                textBox96.Text = s5[0]; //
                label5.Text = s5[1]; //
                textBox95.Text = s5[2]; //
                label6.Text = s5[3]; //
            }
            catch
            {
            }


            if (Directory.Exists(@"D:\Data")) //判断是否有这个文件夹--D:\数据备份\
            {

            }
            else
            {
                Directory.CreateDirectory(@"D:\Data");
            }

            if (Directory.Exists(@"D:\Data backup")) //判断是否有这个文件夹--E:\数据备份\
            {
                if (Directory.Exists(@"D:\Data backup\\" + System.DateTime.Now.ToString("yyyyMMdd")))
                    //判断是否有这个文件夹--E:\数据备份\
                {
                }
                else
                {
                    Directory.CreateDirectory(@"D:\Data backup\\" + System.DateTime.Now.ToString("yyyyMMdd")); //没有就创建
                    //   Thread.Sleep(30);
                    Erasing();
                }
            }
            else
            {
                Directory.CreateDirectory(@"D:\Data backup");
                Directory.CreateDirectory(@"D:\Data backup\\" + System.DateTime.Now.ToString("yyyyMMdd")); //没有就创建
                Erasing();
            }
        }

        #endregion



        #region 读取型号数据,传给窗体

        private void getFileMsg(string path)
        {
            string[] msgS = new string[500];

            try
            {
                if (FileOperate.OpenFileString(path, out msgS, out filePathNow))
                {
                    //文件存在，打开成功
                    //filePathNow = flatPathFromConfig;   //型号保存路径显示到窗体

                    PcConnectPlc.Write_Data_FxUsb("M2888", 1); //打开标志位
                    //Thread.Sleep(200);

                    gap12 = msgS[2]; //间距1，2，3，4，5，6
                    string str18 = gap12;
                    string[] sArray18 = Regex.Split(str18, ",", RegexOptions.IgnoreCase);

                    gap11 = msgS[1]; //----实际冲数，设定冲数，自检频率，实际张数
                    string str17 = gap11;
                    string[] sArray17 = Regex.Split(str17, ",", RegexOptions.IgnoreCase);

                    gap13 = msgS[3]; //间距7.8.9.10.11.12
                    string str19 = gap13;
                    string[] sArray19 = Regex.Split(str19, ",", RegexOptions.IgnoreCase);

                    gap14 = msgS[4]; //间距13.14.15.16.17.18.19.20
                    string str20 = gap14;
                    string[] sArray20 = Regex.Split(str20, ",", RegexOptions.IgnoreCase);

                    gap15 = msgS[5]; //左右臂位置1
                    string str21 = gap15;
                    string[] sArray21 = Regex.Split(str21, ",", RegexOptions.IgnoreCase);

                    gap16 = msgS[6]; //左右臂位置2
                    string str22 = gap16;
                    string[] sArray22 = Regex.Split(str22, ",", RegexOptions.IgnoreCase);

                    gap17 = msgS[7]; //相机位置
                    string str23 = gap17;
                    string[] sArray23 = Regex.Split(str23, ",", RegexOptions.IgnoreCase);

                    //监控设定冲数，右安全位置，手臂高度
                    if (sArray17[1] != "0" && sArray17[1] != "" && sArray21[1] != "0" && sArray21[1] != "" &&
                        sArray22[0] != "0" && sArray22[0] != "")
                    {
                        //   numericUpDown77.Value =Convert .ToDecimal( Convert.ToInt32(sArray18[0])/100f);//写入
                        textBox27.Text = (Convert.ToInt32(sArray18[0])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2944", Convert.ToInt32(sArray18[0])); //间距1

                        textBox34.Text = (Convert.ToInt32(sArray18[1])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2946", Convert.ToInt32(sArray18[1])); //间距2

                        textBox36.Text = (Convert.ToInt32(sArray18[2])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2948", Convert.ToInt32(sArray18[2])); //间距3

                        textBox35.Text = (Convert.ToInt32(sArray18[3])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2950", Convert.ToInt32(sArray18[3])); //间距4

                        textBox40.Text = (Convert.ToInt32(sArray18[4])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2952", Convert.ToInt32(sArray18[4])); //间距5

                        textBox39.Text = (Convert.ToInt32(sArray18[5])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2954", Convert.ToInt32(sArray18[5])); //间距6


                        textBox74.Text = sArray17[0]; //写入
                        PcConnectPlc.Write_Data_FxUsb("D82", Convert.ToInt32(sArray17[0])); //实际冲数

                        textBox73.Text = sArray17[1]; //写入
                        PcConnectPlc.Write_Data_FxUsb("D220", Convert.ToInt32(sArray17[1])); //设定冲数

                        textBox72.Text = sArray17[2]; //写入
                        PcConnectPlc.Write_Data_FxUsb("D808", Convert.ToInt32(sArray17[2])); //自检频率

                        textBox22.Text = sArray17[3]; //写入
                        PcConnectPlc.Write_Data_FxUsb("D56", Convert.ToInt32(sArray17[3])); //实际张数
                        //}



                        textBox38.Text = (Convert.ToInt32(sArray19[0])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2956", Convert.ToInt32(sArray19[0])); //间距7

                        textBox37.Text = (Convert.ToInt32(sArray19[1])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2958", Convert.ToInt32(sArray19[1])); //间距8

                        textBox52.Text = (Convert.ToInt32(sArray19[2])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2960", Convert.ToInt32(sArray19[2])); //间距9

                        textBox51.Text = (Convert.ToInt32(sArray19[3])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2962", Convert.ToInt32(sArray19[3])); //间距10

                        textBox44.Text = (Convert.ToInt32(sArray19[4])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2964", Convert.ToInt32(sArray19[4])); //间距11

                        textBox43.Text = (Convert.ToInt32(sArray19[5])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2966", Convert.ToInt32(sArray19[5])); //间距12



                        textBox42.Text = (Convert.ToInt32(sArray20[0])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2968", Convert.ToInt32(sArray20[0])); //间距13
                        //
                        textBox41.Text = (Convert.ToInt32(sArray20[1])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2970", Convert.ToInt32(sArray20[1])); //间距14

                        textBox48.Text = (Convert.ToInt32(sArray20[2])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2972", Convert.ToInt32(sArray20[2])); //间距15

                        textBox47.Text = (Convert.ToInt32(sArray20[3])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2974", Convert.ToInt32(sArray20[3])); //间距16

                        textBox46.Text = (Convert.ToInt32(sArray20[4])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2976", Convert.ToInt32(sArray20[4])); //间距17

                        textBox45.Text = (Convert.ToInt32(sArray20[5])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2978", Convert.ToInt32(sArray20[5])); //间距18

                        textBox50.Text = (Convert.ToInt32(sArray20[6])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2980", Convert.ToInt32(sArray20[6])); //间距19

                        textBox49.Text = (Convert.ToInt32(sArray20[7])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2982", Convert.ToInt32(sArray20[7])); //间距20



                        //textBox60.Text = (Convert.ToInt32(sArray21[0]) / 100f).ToString();//写入
                        //PcConnectPlc.Write_Data_FxUsb("D2916", Convert.ToInt32(sArray21[0]));//出

                        textBox59.Text = (Convert.ToInt32(sArray21[1])/48f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2918", Convert.ToInt32(sArray21[1])); //右

                        textBox58.Text = (Convert.ToInt32(sArray21[2])/48f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2920", Convert.ToInt32(sArray21[2])); //取

                        // numericUpDown94.Value = Convert.ToDecimal(Convert.ToInt32(sArray21[3]) / 48f);//写入
                        textBox57.Text = (Convert.ToInt32(sArray21[3])/48f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2922", Convert.ToInt32(sArray21[3])); //装

                        //  numericUpDown93.Value = Convert.ToDecimal(Convert.ToInt32(sArray21[4]) / 48f);//写入
                        textBox56.Text = (Convert.ToInt32(sArray21[4])/48f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2924", Convert.ToInt32(sArray21[4])); //第

                        // numericUpDown92.Value = Convert.ToDecimal(Convert.ToInt32(sArray21[5]) / 48f);//写入
                        textBox55.Text = (Convert.ToInt32(sArray21[5])/48f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2926", Convert.ToInt32(sArray21[5])); //左

                        //  numericUpDown100.Value = Convert.ToDecimal(Convert.ToInt32(sArray21[6]) / 48f);//写入
                        textBox54.Text = (Convert.ToInt32(sArray21[6])/48f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2928", Convert.ToInt32(sArray21[6])); //下

                        // numericUpDown99.Value = Convert.ToDecimal(Convert.ToInt32(sArray21[7]) / 100f);//写入
                        textBox53.Text = (Convert.ToInt32(sArray21[7])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2930", Convert.ToInt32(sArray21[7])); //取



                        // numericUpDown98.Value = Convert.ToDecimal(Convert.ToInt32(sArray22[0]) / 100f);//写入
                        textBox64.Text = (Convert.ToInt32(sArray22[0])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2932", Convert.ToInt32(sArray22[0])); //手

                        //  numericUpDown97.Value = Convert.ToDecimal(Convert.ToInt32(sArray22[1]) / 100f);//写入
                        textBox63.Text = (Convert.ToInt32(sArray22[1])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2934", Convert.ToInt32(sArray22[1])); //左

                        textBox62.Text = (Convert.ToInt32(sArray22[2])/100f).ToString(); //写入
                        PcConnectPlc.Write_Data_FxUsb("D2936", Convert.ToInt32(sArray22[2])); //下

                        //textBox61.Text = (Convert.ToInt32(sArray22[3]) / 100f).ToString(); ;//写入
                        //PcConnectPlc.Write_Data_FxUsb("D2938", Convert.ToInt32(sArray22[3]));//调

                        textBox73.Text = (Convert.ToInt32(sArray22[4])).ToString();
                        ; //写入
                        PcConnectPlc.Write_Data_FxUsb("D2940", Convert.ToInt32(sArray22[4])); //调


                        try
                        {
                            label26.Text = msgS[10]; //模具防呆22
                        }
                        catch
                        {
                        }
                        try
                        {
                            if (label26.Text == textBox21.Text)
                            {
                                gap18 = msgS[8]; //模具防呆
                                string str24 = gap18;
                                mold3 = Convert.ToInt32(str24);
                            }
                            else
                            {
                                mold3 = 0;
                            }
                        }
                        catch
                        {
                        }
                        try
                        {
                            gap19 = msgS[9]; //保养
                            string str25 = gap19;
                            by = Convert.ToInt32(str25);
                        }
                        catch
                        {
                        }



                        if (msgS[11] == "0")
                        {

                            PcConnectPlc.Write_Data_FxUsb("D2882", 0); //同间距
                            textBox105.Text = PLC_DS[62].ToString(); //同间距
                            label27.Text = "0";
                            label28.Text = "同间距";


                            PcConnectPlc.Write_Data_FxUsb("D2942", Convert.ToInt32(sArray22[5])); //同间距


                        }
                        else
                        {
                            label27.Text = "1";
                            label28.Text = "不同间距";


                            PcConnectPlc.Write_Data_FxUsb("D2882", 1); //同间距
                            textBox105.Text = PLC_DS[62].ToString(); //同间距



                            PcConnectPlc.Write_Data_FxUsb("D2942", Convert.ToInt32(sArray22[5])); //同间距
                        }

                    }
                    else
                    {
                        MessageBox.Show("调取位置数据有误！");
                    }

                    //Thread.Sleep(100);
                    PcConnectPlc.Write_Data_FxUsb("M2888", 0); //打开标志位
                    //Thread.Sleep(100);
                    showModelToForm(); //显示界面

                }
                //读取报警信息
                string filePath = Application.StartupPath.ToString() + "\\alarm.txt";
                OpenFileDialog of = new OpenFileDialog();
                of.Filter = "文本文件文件（*.txt）|*.txt|word文件（*.doc）|*.doc";

                of.FilterIndex = 1; //1,2,3   1代表txt,2代表word
                of.FileName = filePath;

                using (StreamReader sr = new StreamReader(of.FileName))
                {
                    textBox13.Text = sr.ReadToEnd();
                    //sr.Close();
                }
            }
            catch
            {
                MessageBox.Show("文件不存在或数据错误", "错误提示");
                PcConnectPlc.Write_Data_FxUsb("M2888", 0); //打开标志位
                // Thread.Sleep(100);
            }
        }

        #endregion

        #region 把产品型号显示到窗体

        private void showModelToForm()
        {
            //显示产品型号
            try
            {
                fileModelStr = filePathNow.Split('\\');
                fileModelList.AddRange(fileModelStr);
                int modelLenth = fileModelList.Count();
                label67.Text = fileModelList[modelLenth - 1];
                fileModelList.Clear();
            }
            catch (System.Exception ex)
            {
                //如果没有型号，那么截取字符串会出错
            }
        }

        #endregion

        #region 保存模板路径到config

        private void saveModelMsg(string ModelName)
        {
            //打开config，读取数据
            List<string> msgL = new List<string>();
            //string[] msgS = new string[500];
            string fileName; //config的路径

            string filePath = Application.StartupPath.ToString() + "\\config.txt";
            //FileOperate.OpenFile(filePath, "List", out msgL, out msgS);
            //msgL[1] = ModelName;
            FileOperate.OpenFileList(filePath, out msgL);
            msgL[1] = ModelName;

            //写入修改数据到config
            //FileOperate.SaveFile(filePath, "List", msgL, msgS, out fileName);
            fileName = FileOperate.SaveFileList(filePath, msgL);
            msgL.Clear();
        }

        #endregion

        #region 窗体最大化 控件跟着变化

        private void MaxInit()
        {
            this.Resize += new EventHandler(Form1_Resize);
            X = this.Width;
            Y = this.Height;
            setTag(this);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }

        private void setControls(float newx, float newy, Control cons)
        {
            foreach (Control con in cons.Controls)
            {

                string[] mytag = con.Tag.ToString().Split(new char[] {':'});
                float a = Convert.ToSingle(mytag[0])*newx;
                con.Width = (int) a;
                a = Convert.ToSingle(mytag[1])*newy;
                con.Height = (int) (a);
                a = Convert.ToSingle(mytag[2])*newx;
                con.Left = (int) (a);
                a = Convert.ToSingle(mytag[3])*newy;
                con.Top = (int) (a);
                Single currentSize = Convert.ToSingle(mytag[4])*newy;
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    setControls(newx, newy, con);
                }
            }

        }

        #endregion

        #region 控件多，不刷新界面，防止卡顿

        //控件多，以下不刷新界面，防止卡顿------------------------------------
        protected override CreateParams CreateParams
        {
            get
            {

                CreateParams cp = base.CreateParams;

                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED    

                if (this.IsXpOr2003 == true)
                {
                    cp.ExStyle |= 0x00080000; // Turn on WS_EX_LAYERED  
                    this.Opacity = 1;
                }

                return cp;

            }

        } //防止闪烁  

        private Boolean IsXpOr2003
        {
            get
            {
                OperatingSystem os = Environment.OSVersion;
                Version vs = os.Version;

                if (os.Platform == PlatformID.Win32NT)
                    if ((vs.Major == 5) && (vs.Minor != 0))
                        return true;
                    else
                        return false;
                else
                    return false;
            }
        }

        //控件多，以上不刷新界面，防止卡顿------------------------------------   

        #endregion

        #region 密码登陆

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            //密码登录
            //string czy = PcConnectPlc.double_Word_To_Int(PLC_DS[40], PLC_DS[41]).ToString(); //操作员
            //string cj = PcConnectPlc.double_Word_To_Int(PLC_DS[42], PLC_DS[43]).ToString();   //厂家
            //string gcs = PcConnectPlc.double_Word_To_Int(PLC_DS[42], PLC_DS[43]).ToString();   //工程师、调机

            if ((textBox8.Text == SuperUser) || (textBox8.Text == "890672"))
            {
                //厂家

                skinTabControl1.Selecting += new TabControlCancelEventHandler(skinTabControl1_Selecting);
                skinTabControl2.Enabled = true; //

                skinGroupBox23.Enabled = true;

                skinGroupBox10.Text = "厂家-已登录";
                Forbidden = "cj"; //记录登录状态
                //     //PcConnectPlc.Write_Data_FxCom("M127", 1);  //密码登录

                label297.Enabled = true; //作业员
                label301.Enabled = true; //磨具
                label302.Enabled = true; //审核人




                textBox8.Text = "******";

                textBox14.BackColor = Color.White; //开始后对应输入控件不可输入
                textBox17.BackColor = Color.White;
                textBox24.BackColor = Color.White;
                textBox15.BackColor = Color.White;
                textBox25.BackColor = Color.White;

                //textBox14.   //开始后对应输入控件不可输入
                //textBox17.BackColor = Color.White;
                //textBox24.BackColor = Color.White;
                //textBox15.BackColor = Color.White;
                //textBox25.BackColor = Color.White; 

            }
            else if (textBox8.Text == operatorUser)
            {
                //操作员

                skinTabControl1.Selecting += new TabControlCancelEventHandler(skinTabControl1_Selecting);
                skinTabControl2.Enabled = true; //
                skinGroupBox23.Enabled = true;
                skinGroupBox10.Text = "操作员-已登录";
                Forbidden = "czy"; //记录登录状态
                //       //PcConnectPlc.Write_Data_FxCom("M127", 1);  //密码登录

                label297.Enabled = false; //作业员
                label301.Enabled = false; //磨具
                label302.Enabled = false; //审核人






                textBox8.Text = "******";
            }
            else if (textBox8.Text == enginerUser)
            {
                //工程师

                skinTabControl1.Selecting += new TabControlCancelEventHandler(skinTabControl1_Selecting);
                skinTabControl2.Enabled = true; //
                skinGroupBox23.Enabled = true;
                skinGroupBox10.Text = "工程师-已登录";
                Forbidden = "gcs"; //记录登录状态
                //       //PcConnectPlc.Write_Data_FxCom("M127", 1);  //密码登录

                label297.Enabled = false; //作业员
                label301.Enabled = false; //磨具
                label302.Enabled = false; //审核人

                textBox82.ReadOnly = false;



                textBox8.Text = "******";
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            //注销密码
            textBox8.Text = "******";
            Forbidden = "czy";
            skinTabControl1.Selecting += new TabControlCancelEventHandler(skinTabControl1_Selecting);
            skinTabControl2.Enabled = false; //
            skinGroupBox23.Enabled = false;
            skinGroupBox10.Text = "未登录";
            //       //PcConnectPlc.Write_Data_FxCom("M127", 0);  //密码登录
            skinTabControl2.Enabled = false;




        }

        //以上好坏板统计及是否计数暂停------------------------------------------

        #endregion

        #region 启动

        /// <summary>
        /// 接收到启动命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        //private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        //{
        //    if (Convert.ToInt32(numericUpDown8.Value) > 1)
        //    {
        //        numericUpDown8.Value = 1;
        //        //numericUpDown8.Text = "1";
        //        MessageBox.Show("设置冲数超过最大值，请重新设置！", "错误提示");

        //    }

        //}

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            //计算暂停
            //         //PcConnectPlc.Write_Data_FxCom("D808", Convert.ToInt32(numericUpDown12.Value));
        }



        private void button50_Click(object sender, EventArgs e)
        {
            //计数暂停清零
            //       //PcConnectPlc.Write_Data_FxCom("D56", 0);

        }


        #endregion

        #region 多线程更新主界面信息（坐标、状态等）

        /// <summary>
        /// 此线程循环与逻辑编程器PLC和四轴机器人数据通信，更新UI界面，等待运行和测试信号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public string piecesNumber = ""; //plc读取张数

        private void RenewDataFromPlc()
        {
            while (true)
            {
                Thread.Sleep(1000);

            }

        }

        #endregion



        #region 报警提示

        private void running_State()
        {
            //报警提示
            switch (PLC_DS[24])
            {
                case 0:
                    label10.Text = "";
                    label10.ForeColor = Color.Red;
                    break;
                case 1:
                    label10.Text = "急停已按下，松开急停全复位！";
                    label10.ForeColor = Color.Red;
                    break;
                case 2:
                    label10.Text = "左右私服驱动报警，请断电15秒重新开机！";
                    label10.ForeColor = Color.Black;
                    break;
                case 3:
                    label10.Text = "料盘上升时无料，放好料按“启动”开始！";
                    label10.ForeColor = Color.Black;
                    break;
                case 4:
                    label10.Text = "取料时没吸到料，整理材料检查吸嘴真空，“全复位”";
                    label10.ForeColor = Color.Green;
                    break;
                case 5:
                    label10.Text = "从模具中吸料没有吸起，暂停中，按“复位”按钮后手臂将退出！    ";
                    label10.ForeColor = Color.Red;
                    break;
                case 6:
                    label10.Text = "料盘到了最高位，还是没有检测到产品，检查料盘高位光电，全复位后再开始。||||     ";
                    label10.ForeColor = Color.Red;
                    break;

                case 7:
                    label10.Text = "冲床上限位不通，请调好冲床，复位送料机后，重新开始||||     ";
                    label10.ForeColor = Color.Red;
                    break;
                case 8:
                    label10.Text = "必须全复位后，才能启动自动运行。。。||||       ";
                    label10.ForeColor = Color.Red;
                    break;

                case 9:
                    label10.Text = "冲床冲压后，冲床上限位不通，请全复位，检查上限位开关或冲床开关延时数据。||||   ";
                    label10.ForeColor = Color.Red;
                    break;
                case 10:
                    label10.Text = "冲床没有切换到“单冲模式”，全复位调好冲床后，重新开始。||||  ";
                    label10.ForeColor = Color.Red;
                    break;
                case 11:
                    label10.Text = "开冲床时，没有切换到“单次冲裁模式”，全复位调整后，重新开始。||||   ";
                    label10.ForeColor = Color.Red;
                    break;
                case 12:
                    label10.Text = "右臂没有退到安全位置，禁止冲压，检测安全位置开关，或参数。|||| ";
                    label10.ForeColor = Color.Red;
                    break;

                case 13:
                    label10.Text = "自动时，光栅或安全门报警按触摸屏“暂停”或“复位按钮”后继续！！ ";
                    label10.ForeColor = Color.Red;
                    break;
                case 14:
                    label10.Text = "开冲床时，光栅或安全门感应器不通。检测好按“全复位”后，重新开始！！！     ";
                    label10.ForeColor = Color.Red;
                    break;

                case 15:
                    label10.Text = "左臂超过安全位置，禁止开冲床，检查数据或安全位开关！！！    ";
                    label10.ForeColor = Color.Red;
                    break;
                case 16:
                    label10.Text = "冲床故障，紧急停止中！！！ ";
                    label10.ForeColor = Color.Red;
                    break;
                case 17:
                    label10.Text = "开冲床时，上限位不通，请检查上限位开关，按“全复位”后，重新开始。||||  ";
                    label10.ForeColor = Color.Red;
                    break;
                case 18:
                    label10.Text = "行程超限！左右臂目标位置大于两臂间距，有撞机风险，请复位后重新启动！！！ ";
                    label10.ForeColor = Color.Red;
                    break;

                case 19:
                    label10.Text = "启动时，光栅挡住或安全门打开报警。全复位后检查光栅和安全门！！！";
                    label10.ForeColor = Color.Red;
                    break;
                case 20:
                    label10.Text = "开冲床时，冲床被锁定，请检查左右臂安全位置数据、安全位开关、光栅、安全门和急停按钮！！！  ";
                    label10.ForeColor = Color.Red;
                    break;
                case 21:
                    label10.Text = "右臂入模挂针时，没有检测到探针，请按“复位”按钮后，右臂将退出！！！！！！ ";
                    label10.ForeColor = Color.Red;
                    break;
                case 22:
                    label10.Text = "左臂入模挂针时，没有检测到针，按“复位按钮”后，左臂将退出！！！！！！ ";
                    label10.ForeColor = Color.Red;
                    break;

                case 23:
                    label10.Text = "请清理模具，完成后按“复位按钮”或“暂停”继续。！！！ ";
                    label10.ForeColor = Color.Red;
                    break;
                case 24:
                    label10.Text = "冲单品时，从模具中取出时，没有吸起，请手工取出单品后，按“复位按钮”或“暂停”继续。！！！ ";
                    label10.ForeColor = Color.Red;
                    break;
                case 25:
                    label10.Text = "暂停中，按“暂停”或“复位按钮”继续。！！！    ";
                    label10.ForeColor = Color.Red;
                    break;
                case 26:
                    label10.Text = "开冲床后，冲床没有回到最高位（上限位不通），必须全复位，重新开始。！！！！    ";
                    label10.ForeColor = Color.Red;
                    break;

                case 27:
                    label10.Text = "光栅或安全门不通，禁止移动。！！！      ";
                    label10.ForeColor = Color.Red;
                    break;
                case 28:
                    label10.Text = "请先登录,并退出手动模式！！！ ";
                    label10.ForeColor = Color.Red;
                    break;
                case 29:
                    label10.Text = "请手动整理材料后，按“启动按钮”，即冲床冲下！！！     ";
                    label10.ForeColor = Color.Red;
                    break;
                case 30:
                    label10.Text = "上下移伺服驱动器报警，必须关闭电源等15秒后重启||||   ";
                    label10.ForeColor = Color.Red;
                    break;

                case 31:
                    label10.Text = "右臂极限限位触发，关电源后将手臂推到中间位置再通电复位！！！  ";
                    label10.ForeColor = Color.Red;
                    break;
                case 32:
                    label10.Text = "左臂极限限位触发，关电源后将手臂推到中间位置再通电复位！！！ ";
                    label10.ForeColor = Color.Red;
                    break;
                case 33:
                    label10.Text = "计数已到达！请清理模具!";
                    label10.ForeColor = Color.Red;
                    break;
                case 34:
                    label10.Text = "（如果多次报警，请按全复位！！！！）右臂已回复到安全位置，请手动将材料放入模具后，再按下启动按钮       ";
                    label10.ForeColor = Color.Red;
                    break;
                case 35:
                    label10.Text = "光栅被挡住或安全门不通，请接通光栅和安全门后再试一次！ ";
                    label10.ForeColor = Color.Red;
                    break;
                case 36:
                    label10.Text = "全复位时隔纸复位未完成，请检查是否通气等情况！！！  ";
                    label10.ForeColor = Color.Red;
                    break;
                case 37:
                    label10.Text = "隔纸私服报警，请断电15秒后重启！！！";
                    label10.ForeColor = Color.Red;
                    break;
                case 38:
                    label10.Text = "二次吸料时，料盘推送过高，请注意观察！！！";
                    label10.ForeColor = Color.Red;
                    break;
                case 39:
                    label10.Text = "扫码不成功，请整理好材料，按“复位”继续！！！ ";
                    label10.ForeColor = Color.Red;
                    break;
                case 68:
                    label10.Text = "隔纸机取纸时，真空检测异常，请检查后按“复位”继续！！";
                    label10.ForeColor = Color.Red;
                    break;
                case 69:
                    label10.Text = "隔纸机移料时，真空检测异常，请检查后按“复位”继续！！ ";
                    label10.ForeColor = Color.Red;
                    break;
                case 70:
                    label10.Text = "左右臂位置不对，请全复位后再启动！！";
                    label10.ForeColor = Color.Red;
                    break;

                case 637:
                    label10.Text = "翻转手臂已翻起，请检查后重试！！";
                    label10.ForeColor = Color.Red;
                    break;
                case 329:
                    label10.Text = "请手动将材料挂入模具针上后，按“启动”按钮，压床将压下！！！！";
                    label10.ForeColor = Color.Red;
                    break;

                case 207:
                    label10.Text = "请手动整理好材料后，按“启动”按钮冲床将冲压！！";
                    label10.ForeColor = Color.Red;
                    break;
                case 211:
                    label10.Text = "左臂运行中目标位置超过了左臂最大位置！！";
                    label10.ForeColor = Color.Red;
                    break;
                case 212:
                    label10.Text = "左臂运行中碰到了原点！！";
                    label10.ForeColor = Color.Red;
                    break;
                case 213:
                    label10.Text = "左臂向模具移动时，冲床不在上限位！！";
                    label10.ForeColor = Color.Red;
                    break;
                case 214:
                    label10.Text = "左臂运行中目标位置超过了原点！！";
                    label10.ForeColor = Color.Red;
                    break;

                case 52:
                    label10.Text = "调模过程中，检测到调模电机没有运动，请检查！！ ";
                    label10.ForeColor = Color.Red;
                    break;
                case 483:
                    label10.Text = "调模过程中，已碰到上极限，禁止继续上升！！";
                    label10.ForeColor = Color.Red;
                    break;
                case 485:
                    label10.Text = "调模过程中，已碰到下极限，禁止继续下降！！";
                    label10.ForeColor = Color.Red;
                    break;
                case 71:
                    label10.Text = "安全高度高于原点，请重新设置安全高度差值 或者 重新设置放料高度！！";
                    label10.ForeColor = Color.Red;
                    break;

                case 500:
                    label10.Text = "全复位完成！！";
                    label10.ForeColor = Color.Green;

                    break;

                case 225:
                    label10.Text = "冲床私服报警，请断电15秒后重启，如果频繁出现此问题，记录驱动报警代码，联系厂家！！";
                    label10.ForeColor = Color.Green;
                    break;


                case 226:
                    label10.Text = "冲床动作受限，请检查急停，光栅，安全门是否正常，主界面冲床是否锁定！！";
                    label10.ForeColor = Color.Green;
                    break;


                case 208:
                    label10.Text = "左臂移料时真空报警，请检查材料是否脱落，按下“复位”手臂退到安全位置！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 209:
                    label10.Text = "左臂下料时真空报警，请手动将材料放入装料盘，按“启动”按钮继续！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 300:
                    label10.Text = "左臂移料时真空报警，请输入密码！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 181:
                    label10.Text = "全复位时，右臂上下复位没完成！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 185:
                    label10.Text = "全复位时，左臂上下复位没完成！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 1601:
                    label10.Text = "全复位时，右臂复位没完成！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 1605:
                    label10.Text = "全复位时，左臂复位没完成！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 1609:
                    label10.Text = "全复位时，料盘复位没完成！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 201:
                    label10.Text = "右臂运行中目标值超过最大值！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 210:
                    label10.Text = "左臂运行中和右臂位置重合，有撞击风险！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 236:
                    label10.Text = "右臂上下运行时行程超限！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 237:
                    label10.Text = "左臂上下运行时，行程超限！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 240:
                    label10.Text = "料盘运行时行程超限！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 200:
                    label10.Text = "右臂运行中和左臂位置重合，有撞击风险！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 202:
                    label10.Text = "右臂运行中碰到原点！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 203:
                    label10.Text = "右臂向模具移动时，冲床不在上限位！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 204:
                    label10.Text = "右臂运行中碰到原点！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 338:
                    label10.Text = "请把材料挂入模具内，在按“启动”，手臂将继续重新（移料或下料）！！";
                    label10.ForeColor = Color.Green;
                    break;
                case 387:
                    label10.Text = "前半冲取料时真空报警，按“复位”，手臂将退出！！";
                    label10.ForeColor = Color.Green;
                    break;
                case 388:
                    label10.Text = "请手动整理材料，，按“启动”按钮后，手臂将继续移料！！";
                    label10.ForeColor = Color.Green;
                    break;
                case 389:
                    label10.Text = "前半冲放料位置真空报警，请检查材料是否脱落，按“复位”继续！！";
                    label10.ForeColor = Color.Green;
                    break;
                case 390:
                    label10.Text = "请手动整理材料，按“启动”，冲床继续压下运行！！";
                    label10.ForeColor = Color.Green;
                    break;

                case 40:
                    label10.Text = "扫码混料，请把材料拿走，按“复位”继续！！";
                    label10.ForeColor = Color.Green;
                    break;
                case 305:
                    label10.Text = "运行中，真空检查或者探针没打开，请检查是否空运行！！";
                    label10.ForeColor = Color.Green;
                    break;
                case 304:
                    label10.Text = "联机中，请先扫码再开启机台！！";
                    label10.ForeColor = Color.Green;
                    break;
                case 307:
                    label10.Text = "补冲提示！！";
                    label10.ForeColor = Color.Green;
                    break;
            }

            //运行状态记录,显示给用户
            if ((PLC_DS[24] != 0) && (RunningStation != PLC_DS[24]))
            {
                //最新的信息显示在第一行
                textBox13.Text = textBox13.Text.Insert(0, DateTime.Now.ToString() + " " + label10.Text + "\r\n");
                try
                {
                    //获取config信息
                    string[] msgS = new string[500];
                    string filePath = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\alarm.txt";
                    FileOperate.OpenFileString(filePath, out msgS);

                    msgS[0] = msgS[0].Insert(0, DateTime.Now.ToString() + "," + label10.Text + "\r\n");

                    FileOperate.SaveFileString(filePath, msgS);
                    //  msgS.close();
                }
                catch
                {
                }

            }
            RunningStation = PLC_DS[24]; //1

            //运行状态



        }

        #endregion

        #region 选显卡 禁用

        void skinTabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //禁用选项卡
            switch (Forbidden)
            {
                case "czy":
                    if (e.TabPageIndex == 1 || e.TabPageIndex == 2)
                    {
                        e.Cancel = true;
                    }
                    break;
                case "gcs":
                    if (e.TabPageIndex == 1)
                    {
                        e.Cancel = true;
                    }
                    break;
            }


        }

        #endregion

        #region 退出程序

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Run == true)
            {
                button42_Click(null, null); //结束生产
            }

            try
            {
                //if (MessageBox.Show("退出上位机软件会先执行全复位，然后保存数据！确定退出！？", "提示！", MessageBoxButtons.OKCancel) == DialogResult.OK)
                //{
                ////PcConnectPlc.Write_Data_FxCom("M2051", 1);
                保存ToolStripMenuItem_Click(null, null);
                closeSoftware = true;
                //    //PcConnectPlc.Write_Data_FxCom("d69",0,1);

                //}
              

            }
            catch
            {

            }
            this.Close();
            Application.Exit();
        }

        #endregion





        #region 技术支持

        #endregion

        #region 窗体无边框时  可以移动窗体

        /* 首先将窗体的边框样式修改为None，让窗体没有标题栏
          * 实现这个效果使用了三个事件：鼠标按下、鼠标弹起、鼠标移动
          * 鼠标按下时更改变量isMouseDown标记窗体可以随鼠标的移动而移动
          * 鼠标移动时根据鼠标的移动量更改窗体的location属性，实现窗体移动
          * 鼠标弹起时更改变量isMouseDown标记窗体不可以随鼠标的移动而移动
          */

        private bool isMouseDown = false;
        private Point FormLocation; //form的location
        private Point mouseOffset; //鼠标的按下位置

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                FormLocation = this.Location;
                mouseOffset = Control.MousePosition;
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            int _x = 0;
            int _y = 0;
            if (isMouseDown)
            {
                Point pt = Control.MousePosition;
                _x = mouseOffset.X - pt.X;
                _y = mouseOffset.Y - pt.Y;

                this.Location = new Point(FormLocation.X - _x, FormLocation.Y - _y);
            }
        }

        #endregion

        #region 另存为

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //新建文档 写入数据
            List<string> msgL = new List<string>();

            msgL.Add("新增");
            string fileName;
            //FileOperate.SaveFile("txt", "List", msgL, msgS, out fileName);
            fileName = FileOperate.SaveFileList("", msgL);
            filePathNow = fileName;
            msgL.Clear();
            showModelToForm();

            //打开config，读取数据
            string filePath = Application.StartupPath.ToString() + "\\config.txt";

            FileOperate.OpenFileList(filePath, out msgL);
            msgL[0] = fileName;
            //写入修改数据到config
            FileOperate.SaveFileList(filePath, msgL);
            msgL.Clear();

        }

        #endregion

        #region 新建

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新建文档 写入数据
            string[] msgS = new string[500];
            string fileName;
            //FileOperate.SaveFile("txt", "string", msgL, msgS, out fileName);
            //  System.DateTime.Now.ToString("yyyyMMddHHmmss")
            //  fileName = FileOperate.SaveFileString(System.DateTime.Now.ToString("yyyyMMddHHmmss")+".txt", msgS);
            fileName = FileOperate.SaveFileString("", msgS);
            filePathNow = fileName;
            showModelToForm();
        }

        #endregion

        #region 保存（更新，根据现在的型号增删改查）

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //新建文档 写入数据
            List<string> msgL = new List<string>();
            List<string> msgLc = new List<string>();
            try
            {
                FileOperate.OpenFileList(filePathNow, out msgL);

                msgL.Clear();

                msgL.Add("");

                //监控数据，避免异常数据保存，设定充数，有安全位置，手臂高度
                if (PLC_DS[13] != 0 && PLC_DS[42] != 0 && PLC_DS[52] != 0)
                {
                    //1---实际冲数，设定冲数，自检频率，实际张数，总计张数
                    msgL.Add(PLC_DS[12] + "," + PLC_DS[13] + "," + PLC_DS[14] + "," + PLC_DS[15] + "," + PLC_DS[28] +
                             "," + "实冲数，设冲数，自频率，实张数，总张数"); //

                    //2---间距1，2，3，4，5，6
                    msgL.Add(PLC_DS[64] + "," + PLC_DS[66] + "," + PLC_DS[68] + "," + PLC_DS[70] + "," + PLC_DS[72] +
                             "," + PLC_DS[74] + "," + "间距1，2，3，4，5，6"); //

                    //3---间距7.8.9.10.11.12
                    msgL.Add(PLC_DS[76] + "," + PLC_DS[78] + "," + PLC_DS[80] + "," + PLC_DS[82] + "," + PLC_DS[84] +
                             "," + PLC_DS[86] + "," + "间距7.8.9.10.11.12"); //

                    //4---间距13.14.15.16.17.18.19.20
                    msgL.Add(PLC_DS[88] + "," + PLC_DS[90] + "," + PLC_DS[92] + "," + PLC_DS[94] + "," + PLC_DS[96] +
                             "," + PLC_DS[98] + "," + PLC_DS[100] + "," + PLC_DS[102] + "," +
                             "间距13.14.15.16.17.18.19.20"); //

                    //5---左右臂位置1--出模高度，右安全高度，取料位置，装模位置，第一冲，左安全位置，下料位置，取料高度
                    msgL.Add(PLC_DS[36] + "," + PLC_DS[38] + "," + PLC_DS[40] + "," + PLC_DS[42] + "," + PLC_DS[44] +
                             "," + PLC_DS[46] + "," + PLC_DS[48] + "," + PLC_DS[50] + "," + "出模高，右安高，取置，装位，第一冲，左安，下位，取高");
                        //

                    //6---左右臂2-----手臂高度，左入魔高度，下料高度，调膜高度，冲裁次数,同间距，
                    msgL.Add(PLC_DS[52] + "," + PLC_DS[54] + "," + PLC_DS[56] + "," + PLC_DS[58] + "," + PLC_DS[60] +
                             "," + PLC_DS[62] + "," + "手高，左入高，下高，调膜高，冲数,同距，"); //

                    //7---扫码位置
                    msgL.Add(PLC_DS[138] + "," + PLC_DS[139] + "," + PLC_DS[142] + "," + PLC_DS[148] + "," + "扫码位置"); //


                    //8---模具防呆
                    msgL.Add(mold3.ToString()); //

                    //9----保养数累加
                    msgL.Add(textBox19.Text);

                    //10----上次
                    msgL.Add(label26.Text);

                    //11----不同间距1--同间距0
                    msgL.Add(PLC_DS[2].ToString());



                }

                string fileName; //保存的路径
                fileName = FileOperate.SaveFileList(filePathNow, msgL);

                //写入修改数据到备份文件

                msgL.Clear();
                showModelToForm();
            }
            catch
            {

            }
            ////打开config，读取数据

            //FileOperate.OpenFileList(filePath, out msgL);
            //msgL[0] = fileName;



            //------超级密码存储
            string filePath = Application.StartupPath.ToString() + "\\config.txt";
            FileOperate.OpenFileList(filePath, out msgL);
            msgL[0] = filePathNow;
            msgL[3] = ("");

            //  msgL[4] = SuperUser + "," + enginerUser + "," + czy;

            // msgL[5] = JDLaccount + "," + JDLpwd + "," + machineidentifier + "," + machineidentifier;//稼动率服务器



            //  msgL[7] = serverAccount + "," + serverPwd;//数据服务器

            if (textBox24.Text != "" && textBox21.Text != "")
            {
                Properties.Settings.Default.LastModelImformation = textBox82.Text + "," + textBox24.Text + "," +
                                                                   textBox21.Text + "," + textBox29.Text + "," +
                                                                   textBox18.Text + "," + label34.Text + "," +
                                                                   label35.Text; //上次模具数据存储

            }
            if (label5.Text != "没有权限" && label6.Text != "没有权限")
            {
                Properties.Settings.Default.JMTJ = textBox96.Text + "," + label5.Text + "," + textBox95.Text + "," +
                                                   label6.Text; //加模具人员-调机人员
            }
            msgL[10] = barcodeAccount + "," + barcodePwd; //扫描头数据上传



            FileOperate.SaveFileList(filePath, msgL);
            msgL.Clear();

            //保存报警信息
            filePath = Application.StartupPath.ToString() + "\\alarm.txt";
            string[] str = new string[500];

            try
            {
                for (int i = 0; i < 499; i++)
                {
                    str[i] = (textBox13.Lines[i]);
                }
            }
            catch
            {
                //textBox行数不足会报错
            }

            //log信息保存

            filePath = Application.StartupPath.ToString() + "\\log.txt";
            string[] logStr = new string[2];

            FileOperate.SaveFileString(filePath, logStr);
        }

        #endregion

        #region 打开

        private void 数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getFileMsg("");
        }

        #endregion

        #region 循环和三菱PLC交换数据

        // int rotaStep = 0;  //纠偏步骤
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            PLC_MS = PcConnectPlc.Read_Data_FxUsbS("M2880", 200); //M480 
            PLC_DS = PcConnectPlc.Read_Data_FxUsbS("D2880", 200);

            if (PLC_DS[13] != 0)
            {
                textBox74.Text = PLC_DS[12].ToString(); //写入
                //PcConnectPlc.Write_Data_FxUsb("D2892", Convert.ToInt32(sArray17[0]));//实际冲数

                textBox73.Text = PLC_DS[13].ToString(); //写入
                //PcConnectPlc.Write_Data_FxUsb("D2893", Convert.ToInt32(sArray17[1]));//设定冲数
            }

            if (PLC_DS[13] == 0)
            {
                tongxingplc = tongxingplc + 1;
                label7.Text = (tongxingplc).ToString();
                label92.Text = "通信待机";
            }
            else
            {
                label92.Text = "通信正常";
            }
            //label13.Text = DateTime.Now.ToString("ss");

            if (Run == true) //运行判断控制PLC
            {
                try
                {
                    string a3 = textBox78.Text;
                    Double a1 = Convert.ToDouble(a4);
                    Double a2 = Convert.ToDouble(a3);
                    if (a2 > 0)
                    {
                        if ((a2 >= a1) && PNL == false)
                        {
                            PcConnectPlc.Write_Data_FxUsb("M113", 1); //正常停机

                            button42_Click(null, null); //停机

                            //PNL = true;
                        }
                    }

                    if (shenghe == false)
                    {
                        if (a33 == houjia)
                        {
                            PcConnectPlc.Write_Data_FxUsb("M113", 1); //正常停机

                            houjia = houjia + 1; //

                        }

                    }

                }
                catch
                {

                }
            }
            else
            {


            }

            if (PLC_MS[1] == 1)
            {
                //左臂下料完成
                PcConnectPlc.Write_Data_FxUsb("M2881", 0);

                //触发扫描枪 扫描
                btnSend_Click(null, null);
            }

            if (saomatouk || saomatoug || saomatouh) //扫码头动作读数据
            {
                Barcode = "";
                Mixture1 = "0";
                if (saomatouk) //给PLC发送数据,扫描ok完成.
                {
                    PcConnectPlc.Write_Data_FxUsb("M2880", 1);
                    
                    PcConnectPlc.Write_Data_FxUsb("M2880", 0);

                    try
                    {
                        //获取config信息
                        string[] msgS = new string[500];
                        string filePath = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\pn.txt";
                        FileOperate.OpenFileString(filePath, out msgS);

                        msgS[0] = msgS[0].Insert(0, DateTime.Now.ToString() + "," + txtShowData.Text + "\r\n");

                        FileOperate.SaveFileString(filePath, msgS);

                        try
                        {
                            Barcode = txtShowData.Text; //发送数据到服务器
                            NG = Barcode.Substring(0, Barcode.Length - 5);
                        }
                        catch
                        {
                        }


                    }
                    catch
                    {

                    }

                    sm = sm + 1;
                    label341.Text = sm.ToString();

                    saomatouk = false;

                }
                if (saomatoug) //给PLC发送数据,扫描完成.未扫到码
                {
                    try
                    {
                        Barcode = NG + "-NG"; //发送数据到服务器
                        //获取config信息
                        string[] msgS = new string[500];
                        string filePath = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\pn.txt";
                        FileOperate.OpenFileString(filePath, out msgS);

                        msgS[0] = msgS[0].Insert(0, DateTime.Now.ToString() + "," + txtShowData.Text + "\r\n");

                        FileOperate.SaveFileString(filePath, msgS);
                    }
                    catch
                    {

                    }
                    sm1 = sm1 + 1;
                    label342.Text = sm1.ToString();
                    saomatoug = false;
                }
                if (saomatouh) //给PLC发送数据,扫描
                {
                    //  MessageBox.Show("扫码不成功，请检查是否有混料！", "扫码不成功提示！");

                    PcConnectPlc.Write_Data_FxUsb("M1058", 1);
                   
                    Mixture1 = "1";
                    Barcode = txtShowData.Text; //发送数据到服务器
                    try
                    {
                        List<string> msgL = new List<string>(); //存数据
                        string filePath = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") +
                                          "\\Mixture.txt"; //打开根目录下的Auditor.TXT 路径
                        FileOperate.OpenFileList(filePath, out msgL); //存储到数值中
                        string[] Mold1 = new string[100];

                        msgL[0] = msgL[0].Insert(0,
                        DateTime.Now.ToString() + txtShowData.Text + "扫码不成功，请检查是否有混料！" + "\r\n");
                        FileOperate.SaveFileList(filePath, msgL);
                        msgL.Clear();

                    }
                    catch
                    {
                    }
                    sm2 = sm2 + 1;
                    label343.Text = sm2.ToString();
                    saomatouh = false;
                }


                if (PLC_DS[151] == 0) //判断是否开启方向扫描D3031
                {
                    Barcodeweizhi = textBox66.Text + "/" + textBox68.Text;
                }
                else
                {
                    Barcodeweizhi = textBox68.Text + "/" + textBox65.Text;
                }
                string Para1 = "EQU_ID|DAY|SCAN_DATE|PARTNUM|BARCODE|LOCATE_XY|SET_NUM|REAL_NUM|IS_MIX";
                string Para2 = machineidentifier + "|" + System.DateTime.Now.ToString("yyyy/MM/dd") + "|" +
                               System.DateTime.Now.ToString("yyyyMMddHHmmss") + "|" + textBox12.Text + "|" + Barcode +
                               "|" + Barcodeweizhi + "|" + textBox73.Text + "|" + textBox74.Text + "|" + Mixture1;
                string ret = webFun.sendDataToSerGrp(barcodeAccount, barcodePwd, machineidentifier, "CX01", "CX01",
                    Para1, Para2, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

            }
            //w1.Stop();
            //TimeSpan s1 = w1.Elapsed;
            timer1.Enabled = true;
        }

        #endregion



        private void button81_Click(object sender, EventArgs e)
        {
//超级密码开关

            a6 =
                Convert.ToInt32(
                    System.DateTime.Now.AddMonths(1).AddDays(2).AddHours(3).AddMinutes(4).ToString("MMddHHmm"));
                //利用时间--精简程序

            int b = Convert.ToInt32(numericUpDown8.Value) - a6;

            if (b > -10 && b < 10)
            {

                textBox6.Visible = true;
                textBox7.Visible = true;
                textBox9.Visible = true;
                label289.Visible = true;
                label290.Visible = true;
                label291.Visible = true;
                numericUpDown8.Value = 0;
            }
            else
            {

                textBox6.Visible = false;
                textBox7.Visible = false;
                textBox9.Visible = false;
                label289.Visible = false;
                label290.Visible = false;
                label291.Visible = false;


                numericUpDown8.Value = 0;


            }
        }



        private void button101_Click(object sender, EventArgs e) //搜索
        {

            if (textBox96.Text != "" && textBox95.Text != "" && textBox24.Text != "" && textBox21.Text != "" &&
                textBox14.Text != "" && textBox17.Text != "")
            {
                if (textBox14.Text.Length >= 10 && textBox17.Text.Length >= 1)
                {


                    try
                    {
                        radioButton15.Checked = true;
                        radioButton14.Checked = false;
                        radioButton13.Checked = false;
                        textBox77.Text = "合格继续生产";

                        radioButton17.Checked = true;
                        radioButton16.Checked = false;
                        textBox80.Text = "OK";

                        Lay = textBox17.Text;
                        Batch = textBox14.Text;
                        DataSet yy = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0001", "0009",
                            Batch + "|SFCZ1_ZD_PunchCut|" + Lay, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        textBox2.Text = Batch + Lay;

                        if (yy.Tables[0].Rows.Count > 0)
                        {

                            string u9 = yy.Tables[0].Rows[0][9].ToString(); //BOM资料-模具


                            if (textBox21.Text != "" && mj == false)
                            {
                                textBox26.Text = u9;
                                string muju = u9;
                                string[] muju1 =
                                    muju.Split(new char[13]
                                        {'同', '(', ')', '/', '（', '）', '或', '修', '模', '由', '一', '二', '衝'});
                                for (int i = 0; i < muju1.Count(); i++)
                                {
                                    if (muju1[i] == textBox21.Text)
                                    {
                                        i = 10;
                                        mj = true;
                                    }
                                }
                            }

                            if (mj == false)
                            {
                                MessageBox.Show(DateTime.Now.ToString() + textBox21.Text + "/" + textBox26.Text +
                                                "模具名称不匹配！");

                                List<string> msgL = new List<string>(); //存数据
                                string filePath = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") +
                                                  "\\Mold match.txt"; //打开根目录下的Auditor.TXT 路径
                                FileOperate.OpenFileList(filePath, out msgL); //存储到数值中
                                string[] Mold1 = new string[100];

                                msgL[0] = msgL[0].Insert(0,
                                    DateTime.Now.ToString() + textBox21.Text + "/" + textBox26.Text + "模具名称不匹配！" +
                                    "\r\n");
                                FileOperate.SaveFileList(filePath, msgL);
                                msgL.Clear();

                                textBox26.Text = "";
                                textBox17.Text = "";
                                textBox14.Text = "";
                            }
                            else
                            {
                                //MOLD();//提取数据
                                label23.Text = mold3.ToString();

                                //  Thread.Sleep(100);


                                textBox26.Text = u9;
                                string uu = yy.Tables[0].Rows[0][0].ToString(); //料号
                                textBox12.Text = uu;
                                string u10 = yy.Tables[0].Rows[0][1].ToString(); //在制程
                                label367.Text = u10;
                                string u1 = yy.Tables[0].Rows[0][2].ToString(); //图程序
                                textBox69.Text = u1;
                                string u3 = yy.Tables[0].Rows[0][3].ToString(); //主图程序
                                textBox16.Text = u3;
                                string u5 = yy.Tables[0].Rows[0][5].ToString(); //-主配件
                                textBox70.Text = u5;
                                string u6 = yy.Tables[0].Rows[0][6].ToString(); //岑别名称
                                textBox71.Text = u6;
                                string u7 = yy.Tables[0].Rows[0][7].ToString(); //第几次过站
                                label354.Text = u7;
                                string u4 = yy.Tables[0].Rows[0][8].ToString(); //工令
                                textBox20.Text = u4;
                                mj = false;





                                //textBox25.Visible = false;

                                保存ToolStripMenuItem_Click(null, null); //保存一次数据
                            }

                        }
                        else
                        {
                            MessageBox.Show("系统未过帐，请联系负责人确认系统！");
                        }
                    }
                    catch
                    {
                        MessageBox.Show("未从系统获取到任何信息，请维护系统，谢谢！", "提示！");
                        textBox17.Text = "";
                        textBox14.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("扫码数据不全或者数据不对！", "提示！");
                    textBox17.Text = "";
                    textBox14.Text = "";
                }
            }
            else
            {
                MessageBox.Show("请先确认架模人员、调机人员、批号、层别是否为空！", "提示！");
            }
        }

        private void textBox15_Click(object sender, EventArgs e)
        {
            if (skinGroupBox10.Text != "厂家-已登录")
            {
                textBox15.BackColor = Color.Green;
                saoma2 = textBox15.Text;
                textBox32.Focus();
                saoma1 = 4;
                textBox24.BackColor = Color.White;
                textBox95.BackColor = Color.White;
                textBox96.BackColor = Color.White;
                textBox14.BackColor = Color.White;
                textBox17.BackColor = Color.White;
                //textBox15.BackColor = Color.White;
                textBox25.BackColor = Color.White;
            }

        }


        #region  权限定时器

        private void timer4_Tick(object sender, EventArgs e)
        {
            timer4.Enabled = false;
            if (saoma3 == true)
            {
                //if (skinGroupBox10.Text != "厂家-已登录")

                

                if ((textBox32.Text.Length > 2) && (textBox32.Text != saoma2))
                {
                    string b = textBox32.Text;
                    try
                    {
                        string[] sArray = b.Split('*');

                        switch (saoma1)
                        {
                            case 1:
                                //批号
                                string[] ph = sArray[1].Split('.');
                                if (ph[0].Length >= 10 && ph[0].Length <= 14) // 批号不带小数点
                                {
                                    textBox14.Text = sArray[1];

                                    textBox17_Click(null, null); //称别
                                    textBox14.BackColor = Color.White;
                                }
                                else
                                {
                                    textBox14_Click(null, null); //称别
                                }
                                break;
                            case 2: //成别
                                if (sArray[1].Length >= 1 && sArray[1].Length <= 4)
                                {
                                    textBox17.Text = sArray[1];
                                    if (textBox14.Text.Length >= 10 && textBox17.Text.Length >= 1)
                                    {


                                        try
                                        {
                                            radioButton15.Checked = true;
                                            radioButton14.Checked = false;
                                            radioButton13.Checked = false;
                                            textBox77.Text = "合格继续生产";

                                            radioButton17.Checked = true;
                                            radioButton16.Checked = false;
                                            textBox80.Text = "OK";

                                            Lay = textBox17.Text;
                                            Batch = textBox14.Text;
                                            DataSet yy = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0001",
                                                "0009", Batch + "|SFCZ1_ZD_PunchCut|" + Lay,
                                                System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                            textBox2.Text = Batch + Lay;

                                            if (yy.Tables[0].Rows.Count > 0)
                                            {

                                                string u9 = yy.Tables[0].Rows[0][9].ToString(); //BOM资料-模具


                                                if (textBox21.Text != "" && mj == false)
                                                {
                                                    textBox26.Text = u9;
                                                    string muju = u9;
                                                    string[] muju1 =
                                                        muju.Split(new char[13]
                                                        {
                                                            '同', '(', ')', '/', '（', '）', '或', '修', '模', '由', '一', '二',
                                                            '衝'
                                                        });
                                                    for (int i = 0; i < muju1.Count(); i++)
                                                    {
                                                        if (muju1[i] == textBox21.Text)
                                                        {
                                                            i = 10;
                                                            mj = true;
                                                        }
                                                    }
                                                }

                                                if (mj == false)
                                                {
                                                    MessageBox.Show(DateTime.Now.ToString() + textBox21.Text + "/" +
                                                                    textBox26.Text + "模具名称不匹配！");

                                                    List<string> msgL = new List<string>(); //存数据
                                                    string filePath = @"D:\Data backup\" +
                                                                      System.DateTime.Now.ToString("yyyyMMdd") +
                                                                      "\\Mold match.txt"; //打开根目录下的Auditor.TXT 路径
                                                    FileOperate.OpenFileList(filePath, out msgL); //存储到数值中
                                                    string[] Mold1 = new string[100];

                                                    msgL[0] = msgL[0].Insert(0,
                                                        DateTime.Now.ToString() + textBox21.Text + "/" + textBox26.Text +
                                                        "模具名称不匹配！" + "\r\n");
                                                    FileOperate.SaveFileList(filePath, msgL);
                                                    msgL.Clear();

                                                    textBox26.Text = "";
                                                    textBox17.Text = "";
                                                    textBox14.Text = "";
                                                }
                                                else
                                                {
                                                    MOLD(); //提取数据
                                                    label23.Text = mold3.ToString();

                                                   

                                                    textBox26.Text = u9;
                                                    string uu = yy.Tables[0].Rows[0][0].ToString(); //料号
                                                    textBox12.Text = uu;
                                                    string u10 = yy.Tables[0].Rows[0][1].ToString(); //在制程
                                                    label367.Text = u10;
                                                    string u1 = yy.Tables[0].Rows[0][2].ToString(); //图程序
                                                    textBox69.Text = u1;
                                                    string u3 = yy.Tables[0].Rows[0][3].ToString(); //主图程序
                                                    textBox16.Text = u3;
                                                    string u5 = yy.Tables[0].Rows[0][5].ToString(); //-主配件
                                                    textBox70.Text = u5;
                                                    string u6 = yy.Tables[0].Rows[0][6].ToString(); //岑别名称
                                                    textBox71.Text = u6;
                                                    string u7 = yy.Tables[0].Rows[0][7].ToString(); //第几次过站
                                                    label354.Text = u7;
                                                    string u4 = yy.Tables[0].Rows[0][8].ToString(); //工令
                                                    textBox20.Text = u4;
                                                    mj = false;
                                                    textBox17.BackColor = Color.White;

                                                    textBox15_Click(null, null); //作业员

                                                    保存ToolStripMenuItem_Click(null, null); //保存一次数据
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("系统未过帐，请联系负责人确认系统！");
                                                textBox17_Click(null, null); //作业员
                                            }
                                        }
                                        catch
                                        {
                                            MessageBox.Show("未从系统获取到任何信息，请维护系统，谢谢！", "提示！");
                                            textBox17.Text = "";
                                            textBox14.Text = "";
                                            textBox17_Click(null, null); //作业员
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("扫码数据不全或者数据不对！", "提示！");
                                        textBox17.Text = "";
                                        textBox14.Text = "";
                                        textBox17_Click(null, null); //作业员
                                    }
                                }

                                break;
                            case 3: //模具
                                if (sArray[1].Length >= 7)
                                {
                                    //  textBox24.Text ="SG"+sArray[1];
                                    if (Mold != sArray[1])
                                    {
                                        Mold = sArray[1];
                                        textBox96.Text = "";
                                        textBox95.Text = "";
                                        label5.Text = "架模时间";
                                        label6.Text = "调机时间";
                                    }
                                    mj = false; //模具
                                    try
                                    {
                                        DataSet E = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0001",
                                            "0010", Mold, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                        if (E.Tables[0].Rows.Count >= 0)
                                        {
                                            textBox24.Text = Mold;
                                            string A = E.Tables[0].Rows[0][0].ToString(); //实际模具名称
                                            textBox21.Text = A;
                                            try
                                            {
                                                string B = E.Tables[0].Rows[0][1].ToString(); //模具累计冲次数
                                                textBox18.Text = B;
                                                string C = E.Tables[0].Rows[0][2].ToString(); //刀口余量
                                                textBox29.Text = C;

                                                if (B != "")
                                                {
                                                    bool mold4 = false;
                                                    if ((Convert.ToInt32(B) == 398000) && mold4 == false)
                                                    {
                                                        MessageBox.Show("模具寿命已冲裁398000！", "预警提示！");
                                                        mold4 = true;
                                                    }
                                                    if (Convert.ToInt32(B) == 400000 && mold4 == false)
                                                    {
                                                        MessageBox.Show("模具寿命已冲裁400000！", "报警提示！");
                                                        mold4 = true;
                                                    }
                                                }


                                                if (C != "")
                                                {
                                                    bool mold5 = false;
                                                    if (C == "1.1" && mold5 == false)
                                                    {
                                                        MessageBox.Show("刀口余量等于1.1", "预警提示！");
                                                        mold5 = true;
                                                    }
                                                    if (C == "0.6" && mold5 == false)
                                                    {
                                                        MessageBox.Show("刀口余量等于0.6,禁止使用模具！", "报警提示！");
                                                        mold5 = true;
                                                    }
                                                }
                                            }
                                            catch
                                            {
                                            }
                                            textBox24.BackColor = Color.White;
                                            textBox96_Click(null, null); //下一个位置

                                        }
                                        else
                                        {
                                            MessageBox.Show("未从系统读取到信息，请维护模具系统！", "提示！");
                                            textBox24_Click(null, null); //下一个位置
                                            textBox24.Text = "";
                                            textBox21.Text = "";
                                        }
                                    }
                                    catch
                                    {
                                        MessageBox.Show("从系统未获取到模具数据！请维护模治具系统！", "提示!");
                                        textBox24_Click(null, null); //下一个位置
                                        textBox24.Text = "";
                                        textBox21.Text = "";
                                    }
                                }




                                break;
                            case 4: //操作员

                                if (sArray[1].Length == 8 || sArray[1].Length == 7)
                                {
                                    textBox15.Text = sArray[1];

                                    Job = textBox15.Text;
                                    machine = machineidentifier;
                                    job10 = false;
                                    List<string> msgL = new List<string>();
                                    string filePath = Application.StartupPath.ToString() + "\\job.txt";
                                        //打开根目录下的Auditor.TXT 路径
                                    FileOperate.OpenFileList(filePath, out msgL); //存储到数值中
                                    string[] job1 = new string[100];

                                    for (int i = 0; i < msgL.Count; i++) //循环数组数值最大长度
                                    {
                                        job1 = msgL[i].Split(','); //分割数组中,前面和后面的。
                                        if (job1[0] == textBox15.Text) //判断分割后的前面和文本是否一样，一样则直接打开相同
                                        {
                                            if (DateTime.Parse(job1[1]) > DateTime.Now)
                                            {
                                                //textBox25_Click(null, null);//作业员
                                                textBox15.BackColor = Color.White;
                                                job10 = true;
                                                label93.Text = job1[1].ToString();
                                                i = msgL.Count;
                                                button2_Click(null, null); //作业员
                                            }
                                            else
                                            {
                                                //    msgL[i].TrimStart();
                                                job10 = true;
                                                try
                                                {
                                                    DataSet d = webFun.getDataFromSer(serverAccount, serverPwd, "#01",
                                                        "0001", "0011", Job + "|" + machine,
                                                        System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                                    if (d.Tables[0].Rows.Count >= 0)
                                                    {
                                                        string z = d.Tables[0].Rows[0][0].ToString();
                                                        if (Convert.ToInt32(z) >= 1) //审核是否有权限
                                                        {
                                                            textBox15.BackColor = Color.White;
                                                            //textBox25_Click(null, null);//作业员
                                                            button2_Click(null, null); //作业员
                                                            job10 = true;
                                                            label93.Text = System.DateTime.Now.AddDays(7).ToString();
                                                            msgL[i] = textBox15.Text + "," +
                                                                      System.DateTime.Now.AddDays(7).ToString();
                                                                //更改日期，插人
                                                            FileOperate.SaveFileList(filePath, msgL);
                                                            msgL.Clear();
                                                            i = msgL.Count;
                                                        }
                                                        else
                                                        {
                                                            label93.Text = "没有权限";
                                                            textBox15.Text = "";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("读取作业员无权限，请维护系统！", "提示！");
                                                        textBox15.Text = "";
                                                        textBox15.BackColor = Color.Transparent;
                                                    }
                                                }
                                                catch
                                                {
                                                    MessageBox.Show("读取服务器系统异常，请检查网络！", "提示！");
                                                    textBox15.Text = "";
                                                }
                                            }
                                        }
                                    }
                                    if (job10 == false) //如果没记录，则连接服务器读取
                                    {
                                        try
                                        {
                                            DataSet d = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0001",
                                                "0011", Job + "|" + machine,
                                                System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                            if (d.Tables[0].Rows.Count >= 0)
                                            {
                                                string z = d.Tables[0].Rows[0][0].ToString(); //审核是否有权限
                                                if (Convert.ToInt32(z) >= 1)
                                                {
                                                    string aa = textBox15.Text + "," + DateTime.Now.AddDays(7);
                                                    msgL.Add(aa);
                                                    FileOperate.SaveFileList(filePath, msgL);
                                                    msgL.Clear();
                                                    job10 = true;
                                                    textBox15.BackColor = Color.White;
                                                    label93.Text = System.DateTime.Now.AddDays(7).ToString();
                                                    //textBox25_Click(null, null);//作业员
                                                    button2_Click(null, null); //作业员

                                                }
                                                else
                                                {
                                                    label93.Text = "没有权限";
                                                    textBox15.Text = "";
                                                    textBox15_Click(null, null); //作业员
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("读取作业员无权限，请维护系统！", "提示！");
                                                textBox25.BackColor = Color.Transparent;
                                                textBox15.Text = "";
                                                textBox15_Click(null, null); //作业员
                                            }
                                        }
                                        catch
                                        {
                                            MessageBox.Show("读取服务器系统异常，请检查网络！", "提示！");
                                            textBox15.Text = "";
                                            textBox15_Click(null, null); //作业员
                                        }
                                    }
                                }
                                if (label93.Text != "没有权限")
                                {
                                    houjia = 0;

                                    a33 = 0;

                                    label358.Text = piecesNumber; //张数
                                    label353.Text = textBox76.Text; //冲数


                                    PcConnectPlc.Write_Data_FxUsb("M2903", 1); //可以运行

                                }

                                break;

                            case 5: //审核人
                                if (sArray[1].Length == 8 && label93.Text != "没有权限")
                                {
                                    Auditor10 = false;
                                    textBox25.Text = sArray[1];
                                    // textBox25.Text = textBox25.Text;
                                    Auditor = textBox25.Text;
                                    if (textBox25.Text != textBox15.Text)
                                    {
                                        List<string> msgL = new List<string>();
                                        string filePath = Application.StartupPath.ToString() + "\\Auditor.txt";
                                            //打开根目录下的Auditor.TXT 路径
                                        FileOperate.OpenFileList(filePath, out msgL); //存储到数值中
                                        string[] Auditor1 = new string[2];

                                        for (int i = 0; i < msgL.Count; i++) //循环数组数值最大长度
                                        {
                                            Auditor1 = msgL[i].Split(','); //分割数组中,前面和后面的。
                                            if (Auditor1[0] == textBox25.Text) //判断分割后的前面和文本是否一样，一样则直接打开相同
                                            {
                                                if (DateTime.Parse(Auditor1[1]) > DateTime.Now)
                                                {
                                                    Auditor10 = true;
                                                    label94.Text = Auditor1[1].ToString();
                                                    button25_Click(null, null);
                                                    textBox25.BackColor = Color.White;
                                                    i = msgL.Count;
                                                }
                                                else
                                                {
                                                    msgL[i].TrimStart();
                                                    Auditor10 = true;
                                                    try
                                                    {
                                                        DataSet d = webFun.getDataFromSer(serverAccount, serverPwd,
                                                            "#01", "0001", "0012", Auditor + "|SFCZ1_ZD_PunchCut",
                                                            System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                                        if (d.Tables[0].Rows.Count >= 0)
                                                        {
                                                            string f = d.Tables[0].Rows[0][0].ToString();
                                                            if (Convert.ToInt32(f) >= 1) //审核是否有权限
                                                            {
                                                                button25_Click(null, null);
                                                                Auditor10 = true;
                                                                label94.Text = System.DateTime.Now.AddDays(7).ToString();
                                                                msgL[i] = textBox25.Text + "," +
                                                                          System.DateTime.Now.AddDays(7).ToString();
                                                                    //更改日期，插人
                                                                FileOperate.SaveFileList(filePath, msgL);
                                                                msgL.Clear();
                                                                textBox25.BackColor = Color.White;
                                                                i = msgL.Count;
                                                            }
                                                            else
                                                            {
                                                                label94.Text = "没有权限";
                                                                textBox25.Text = "";
                                                                textBox25_Click(null, null); //作业员
                                                            }
                                                            //  textBox25.BackColor = Color.Green;
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("读取审核人无权限，请维护系统！", "提示！");
                                                            textBox25.BackColor = Color.Transparent;
                                                            textBox25_Click(null, null); //作业员
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        MessageBox.Show("读取服务器系统异常，请检查网络！", "提示！");
                                                        textBox25_Click(null, null); //作业员
                                                    }
                                                }
                                            }
                                        }
                                        if (Auditor10 == false) //如果没记录，则连接服务器读取
                                        {
                                            try
                                            {
                                                DataSet d = webFun.getDataFromSer(serverAccount, serverPwd, "#01",
                                                    "0001", "0012", Auditor + "|SFCZ1_ZD_PunchCut",
                                                    System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                                if (d.Tables[0].Rows.Count >= 0)
                                                {
                                                    string f = d.Tables[0].Rows[0][0].ToString(); //审核是否有权限
                                                    if (Convert.ToInt32(f) >= 1)
                                                    {
                                                        string aa = textBox25.Text + "," + DateTime.Now.AddDays(7);
                                                        msgL.Add(aa);
                                                        FileOperate.SaveFileList(filePath, msgL);
                                                        msgL.Clear();
                                                        Auditor10 = true;
                                                        label94.Text = System.DateTime.Now.AddDays(7).ToString();
                                                        button25_Click(null, null);
                                                        textBox25.BackColor = Color.White;
                                                    }
                                                    else
                                                    {
                                                        label94.Text = "没有权限";
                                                        textBox25.Text = "";
                                                        textBox25_Click(null, null); //作业员
                                                    }
                                                    //  textBox25.BackColor = Color.Green;
                                                }
                                                else
                                                {
                                                    MessageBox.Show("读取审核人无权限，请维护系统！", "提示！");
                                                    textBox25.BackColor = Color.Transparent;
                                                    textBox25_Click(null, null); //作业员
                                                }
                                            }
                                            catch
                                            {
                                                MessageBox.Show("读取服务器系统异常，请检查网络！", "提示！");
                                                textBox25_Click(null, null); //作业员
                                            }
                                        }

                                    }
                                    else
                                    {
                                        MessageBox.Show("审核人与作业员同一工号，请使用不同工号审核！");
                                        textBox25.Text = "";
                                        textBox32.Text = "";
                                        textBox25_Click(null, null); //作业员
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("输入格式不对或者作业员未取得权限！");
                                    textBox25_Click(null, null); //作业员

                                }
                                //if (label94.Text != "没有权限")
                                //{
                                //    shenghe = true;
                                //    label24.Text = a33.ToString();
                                //}

                                break;
                            case 6: //加模具人员

                                if (sArray[1].Length == 8 && textBox21.Text != "")
                                {
                                    textBox96.Text = sArray[1];
                                    label5.Text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); //时间
                                    textBox95_Click(null, null);

                                    textBox96.BackColor = Color.White;
                                }
                                else
                                {
                                    textBox96_Click(null, null);
                                }
                                break;
                            case 7: //调机人员
                                if (textBox96.Text != sArray[1])
                                {
                                    if (sArray[1].Length == 8 && textBox96.Text != "")
                                    {
                                        textBox14_Click(null, null);
                                        textBox95.Text = sArray[1];
                                        textBox95.BackColor = Color.White;
                                        label6.Text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); //时间

                                    }
                                    else
                                    {
                                        textBox95_Click(null, null);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("架模人员和调机人员不能相同！");

                                    textBox95_Click(null, null);

                                }

                                break;
                        }
                        sArray[1] = "";
                        b = "";
                        textBox32.Text = "";
                    }
                    catch
                    {
                        b = "";
                        textBox32.Text = "";
                        textBox82.Text = "";
                    }

                }
                if ((textBox32.Text.Length <= 2) && (saoma3 == true))
                {
                    saoma3 = false;
                    textBox32.Text = "";
                    //textBox14.BackColor = Color.White;
                    //textBox17.BackColor = Color.White;
                    //textBox15.BackColor = Color.White;
                    //textBox24.BackColor = Color.White;
                    //textBox25.BackColor = Color.White;
                    //textBox96.BackColor = Color.White;
                    //textBox95.BackColor = Color.White;
                }
            }
            label14.Text = DateTime.Now.ToString("ss");
            timer4.Enabled = true;
        }

        private void textBox32_TextChanged(object sender, EventArgs e)
        {
            saoma3 = true;
        }

        private void textBox25_Click(object sender, EventArgs e)
        {
            if (skinGroupBox10.Text != "厂家-已登录")
            {
                textBox25.BackColor = Color.Green;
                saoma2 = textBox25.Text;
                textBox32.Focus();
                saoma1 = 5;
                textBox24.BackColor = Color.White;
                textBox95.BackColor = Color.White;
                textBox96.BackColor = Color.White;
                textBox14.BackColor = Color.White;
                textBox17.BackColor = Color.White;
                textBox15.BackColor = Color.White;
                //textBox25.BackColor = Color.White;
            }
        }

        private void textBox24_Click(object sender, EventArgs e)
        {
            if (skinGroupBox10.Text != "厂家-已登录")
            {
                textBox24.BackColor = Color.Green;
                saoma2 = textBox24.Text;
                textBox32.Focus();
                saoma1 = 3;

            }
            if (textBox82.Text != "1")
            {
                //textBox24.BackColor = Color.White;
                textBox95.BackColor = Color.White;
                textBox96.BackColor = Color.White;
                textBox14.BackColor = Color.White;
                textBox17.BackColor = Color.White;
                textBox15.BackColor = Color.White;
                textBox25.BackColor = Color.White;

                button87.Enabled = true; //上次模具开关打开
            }
        }

        private void textBox17_Click(object sender, EventArgs e)
        {
            if (skinGroupBox10.Text != "厂家-已登录")
            {
                textBox17.BackColor = Color.Green;
                saoma2 = textBox17.Text;
                textBox32.Focus();
                saoma1 = 2;

                textBox24.BackColor = Color.White;
                textBox95.BackColor = Color.White;
                textBox96.BackColor = Color.White;
                textBox14.BackColor = Color.White;
                //textBox17.BackColor = Color.White;
                textBox15.BackColor = Color.White;
                textBox25.BackColor = Color.White;
            }
        }

        private void textBox14_Click(object sender, EventArgs e)
        {
            if (skinGroupBox10.Text != "厂家-已登录")
            {
                textBox14.BackColor = Color.Green;
                saoma2 = textBox14.Text;
                textBox32.Focus();
                saoma1 = 1;

                textBox24.BackColor = Color.White;
                textBox95.BackColor = Color.White;
                textBox96.BackColor = Color.White;
                //textBox14.BackColor = Color.White;
                textBox17.BackColor = Color.White;
                textBox15.BackColor = Color.White;
                textBox25.BackColor = Color.White;
            }
        }

        #endregion

        #region  后加

        private void button25_Click(object sender, EventArgs e) //生产开始
        {
            textBox24.BackColor = Color.White;
            textBox95.BackColor = Color.White;
            textBox96.BackColor = Color.White;
            textBox14.BackColor = Color.White;
            textBox17.BackColor = Color.White;
            textBox15.BackColor = Color.White;
            textBox25.BackColor = Color.White;

            label34.Text = textBox24.Text;

            textBox72.Text = PLC_DS[14].ToString(); //写入
            //PcConnectPlc.Write_Data_FxUsb("D2894", Convert.ToInt32(sArray17[2]));//自检频率

            textBox22.Text = PLC_DS[15].ToString(); //写入
            //PcConnectPlc.Write_Data_FxUsb("D2895", Convert.ToInt32(sArray17[3]));//实际张数

            //SFCZ1_ZD_PunchCut
            if (PLC_DS[13] != 0 && label93.Text != "没有权限" && textBox15.Text != "")
            {
                houjia = 0;
                shenghe = true;
                label24.Text = a33.ToString();

                timer4.Enabled = false; //关闭权限定时器
                try
                {
                    textBox83.Text = ((Convert.ToInt32(textBox72.Text))*(Convert.ToInt32(textBox73.Text))).ToString();
                        //自主检查
                }
                catch
                {

                }

                try
                {

                    label352.Text = System.DateTime.Now.ToString("yyyyMMddHHmmss"); //开始时间
                    //Run = true;//运行开始


                    //DataSet yy = webFun.getDataFromSer(textBox97.Text, textBox98.Text, "#01", "0004", "0001", System.DateTime.Now.ToString("yyyyMMdd") + "|" + textBox11.Text + "|" + a, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    ////读取表单号,---第一步
                    //if (yy.Tables[0].Rows.Count > 0)//如果有表单号，直接提取，没有就创建。
                    //{
                    //    string y3 = yy.Tables[0].Rows[0][0].ToString();//已经提取到单号
                    //    //2018081200805---第一步
                    //    textBox23.Text = y3;
                    //    textBox19.Text = "0";
                    //}
                    DataSet yy = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0004", "G0001",
                        "SFCZ1_ZD_PunchCut|" + System.DateTime.Now.ToString("yyyyMMdd") + "|" + textBox11.Text + "|" + a,
                        System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    //读取表单号,---第一步
                    if (yy.Tables[0].Rows.Count > 0) //如果有表单号，直接提取，没有就创建。
                    {
                        string y3 = yy.Tables[0].Rows[0][0].ToString(); //已经提取到单号
                        //2018081200805---第一步
                        textBox23.Text = y3;
                        textBox19.Text = "0";
                    }
                    else
                    {
                        DataSet ww = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0004", "0002",
                            System.DateTime.Now.ToString("yyyyMMdd") + "|" + a,
                            System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        //2018081200950--第二步
                        string ee = ww.Tables[0].Rows[0][0].ToString(); //单号
                        textBox23.Text = ee;

                        string ret1 = webFun.sendDataToSerGrp(serverAccount, serverPwd, "#01", "0004", "0003",
                            "paperNo|Status|DoDate|MachineNo|Report|ClassInfo|Factory|CreateTime|CreateEmpid",
                            textBox23.Text + "|1|" + System.DateTime.Now.ToString("yyyyMMdd") + "|" + textBox11.Text +
                            "|SFCZ1_ZD_PunchCut|" + a + "|001|" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "|" +
                            textBox15.Text,
                            System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")); //第三步

                    }
                    DataSet y1 = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0001", "0002", textBox14.Text,
                        System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    // 第四步

                    if (y1.Tables[0].Rows.Count > 0) //
                    {
                        string y2 = y1.Tables[0].Rows[0][0].ToString(); //
                        string y20 = y1.Tables[0].Rows[0][1].ToString(); //
                        shuliang = y20; //数量
                        string y21 = y1.Tables[0].Rows[0][2].ToString(); //
                    }

                    DataSet y4 = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0001", "0009",
                        textBox14.Text + "|SFCZ1_ZD_PunchCut|" + textBox17.Text,
                        System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    //                 //   第五步
                    if (y4.Tables[0].Rows.Count > 0) //
                    {
                        string y5 = y4.Tables[0].Rows[0][0].ToString(); //
                    }

                    button42.BackColor = Color.Green;
                    button25.Enabled = false;
                    button42.Enabled = true;

                    textBox14.Enabled = false; //开始后对应输入控件不可输入
                    textBox17.Enabled = false;
                    textBox24.Enabled = false;
                    textBox15.Enabled = false;
                    textBox25.Enabled = false;
                    textBox32.Enabled = false;
                    textBox31.Enabled = false;
                    textBox37.Enabled = false;
                    skinGroupBox32.Enabled = false;
                    skinGroupBox37.Enabled = false;

                    textBox96.Enabled = false;
                    textBox95.Enabled = false;

                    label358.Text = piecesNumber; //张数
                    label353.Text = textBox76.Text; //冲数

                    PcConnectPlc.Write_Data_FxUsb("M2903", 1); //可以运行

                    string ret = webFun.sendDataToSerGrp(serverAccount, serverPwd, "#01", "0004", "0006",
                        "paperNo|MacState|StartTime|EndTime|Lotnum|Layer|MainSerial|Partnum|WorkNo|SfcLayer|LayerName|Serial|IsMain|OrderId|Item2|Item1|Item3|Item4|Item5|Item6|Item7|Item8|Qty|Item10|Item11|Item13|Item14|Item15|Item16|CreateEmpid|CreateTime|ModifyEmpid|ModifyTime",
                        textBox23.Text + "|正常|" + label352.Text + "|" + "" + "|"
                        + textBox14.Text + "|" + textBox17.Text + "|" + textBox16.Text + "|" + textBox12.Text + "|" +
                        textBox20.Text + "|"
                        + label367.Text + "|" + textBox71.Text + "|" + textBox69.Text + "|" + textBox70.Text + "|" +
                        label354.Text + "|"
                        + textBox24.Text + "|" + textBox21.Text + "|" + textBox5.Text + "|" + textBox18.Text + "|" +
                        textBox29.Text + "|"
                        + textBox26.Text + "|"
                        + textBox79.Text + "|" + textBox77.Text + "|" + label22.Text + " | " + textBox80.Text + "|" +
                        textBox83.Text + "|"
                        + textBox27.Text + "|" + textBox64.Text + "|" + textBox25.Text + "|" + textBox19.Text + "|"
                        + textBox15.Text + "|" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "|" + textBox15.Text +
                        "|" + System.DateTime.Now.ToString("yyyyMMddHHmmss"),
                        System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                    if (ret == "OK")
                    {
                        yunxing = "ok";

                        List<string> msgL = new List<string>(); //数据保存

                        string filePath = Application.StartupPath.ToString() + "\\liaohao.txt";

                        FileOperate.OpenFileList(filePath, out msgL);

                        msgL[0] = "1";

                        msgL[1] = textBox23.Text + "," + label352.Text + "," + textBox14.Text + "," + textBox17.Text +
                                  "," + textBox16.Text + "," + textBox12.Text + "," + textBox20.Text;

                        msgL[2] = label367.Text + "," + textBox71.Text + "," + textBox69.Text + "," + textBox70.Text +
                                  "," + label354.Text + "," + textBox24.Text + "," + textBox21.Text;

                        msgL[3] = textBox5.Text + "," + textBox18.Text + "," + textBox29.Text + "," + textBox26.Text +
                                  "," + textBox79.Text + "," + textBox77.Text;

                        msgL[4] = label22.Text + "," + textBox80.Text + "," + textBox83.Text + "," + textBox27.Text +
                                  "," + textBox64.Text + "," + textBox25.Text + "," + textBox15.Text;

                        msgL[5] = textBox96.Text + "," + label5.Text + "," + textBox95.Text + "," + label6.Text + "," +
                                  textBox19.Text + "," + textBox84.Text;

                        msgL[6] = label93.Text + "," + label94.Text + "," + label358.Text + "," + label353.Text + "," +
                                  label29.Text + "," + label26.Text;

                        msgL[7] = textBox84.Text + "," + textBox79.Text + "," + radioButton11.Checked;

                        FileOperate.SaveFileList(filePath, msgL);
                        msgL.Clear();

                    }

                }
                catch
                {
                    MessageBox.Show("获取系统信息异常！请维护E化系统！");
                }
            }
            else
            {
                MessageBox.Show("请检查操作人员/审核人是否有权限或者PLC通信异常，无法提取表单！");
            }
        }

        private void label301_Click(object sender, EventArgs e)
        {
            mj = false;
            if (skinGroupBox10.Text == "厂家-已登录")
            {
                if (textBox24.Text.Length == 8 || textBox24.Text.Length == 7)
                {
                    try
                    {
                        DataSet E = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0001", "0010",
                            textBox24.Text, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        if (E.Tables[0].Rows.Count > 0)
                        {
                            mj = false; //模具
                            string A = E.Tables[0].Rows[0][0].ToString(); //实际模具名称
                            textBox21.Text = A;
                            string B = E.Tables[0].Rows[0][1].ToString(); //模具累计冲次数
                            textBox18.Text = B;
                            string C = E.Tables[0].Rows[0][2].ToString(); //刀口余量
                            textBox29.Text = C;
                            bool mold4 = false;
                            if (B != "")
                            {
                                if ((Convert.ToInt32(B) == 398000) && mold4 == false)
                                {
                                    MessageBox.Show("模具寿命已冲裁398000！", "预警提示！");
                                    mold4 = true;
                                }
                                if (Convert.ToInt32(B) == 400000 && mold4 == false)
                                {
                                    MessageBox.Show("模具寿命已冲裁400000！", "报警提示！");
                                    mold4 = true;
                                }
                            }

                            if (C != "")
                            {
                                bool mold5 = false;
                                if (C == "1.1" && mold5 == false)
                                {
                                    MessageBox.Show("刀口余量等于1.1", "预警提示！");
                                    mold5 = true;
                                }
                                if (C == "0.6" && mold5 == false)
                                {
                                    MessageBox.Show("刀口余量等于0.6,禁止使用模具！", "报警提示！");
                                    mold5 = true;
                                }
                            }

                            //if (textBox21.Text != "" && mj == false)
                            //{
                            //    string muju = textBox26.Text;
                            //    string[] muju1 = muju.Split(new char[10] { '同', '(', ')', '/', '（', '）', '或', '修', '模', '由' });
                            //    for (int i = 0; i < muju1.Count(); i++)
                            //    {
                            //        if (muju1[i] == textBox21.Text)
                            //        {
                            //            i = 5;
                            //            mj = true;
                            //        }
                            //    }
                            //}

                            //if (mj == false)
                            //{
                            //    MessageBox.Show(DateTime.Now.ToString() + textBox21.Text + "/" + textBox26.Text + "模具名称不匹配！");

                            //    List<string> msgL = new List<string>();//存数据
                            //    string filePath = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") + "\\Mold match.txt";//打开根目录下的Auditor.TXT 路径
                            //    FileOperate.OpenFileList(filePath, out msgL);//存储到数值中
                            //    string[] Mold1 = new string[100];

                            //    msgL[0] = msgL[0].Insert(0, DateTime.Now.ToString() + textBox21.Text + "/" + textBox26.Text + "模具名称不匹配！" + "\r\n");
                            //    FileOperate.SaveFileList(filePath, msgL);
                            //    msgL.Clear();

                            //    textBox21.Text = "";
                            //    textBox18.Text = "";
                            //    textBox24.Text = "";
                            //    textBox29.Text = "";
                            //}
                        }
                        else
                        {
                            MessageBox.Show("未从模具系统读取到数据，请维护模治具系统！", "提示！");
                            mj = false;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("从系统未获取到模具数据！请维护模治具系统！", "提示!");
                        mj = false;
                    }
                }
                else
                {
                    MessageBox.Show("输入信息有误请确认后输入！", "提示！");
                    mj = false;
                    textBox24.Text = "";
                }
            }
        }

        private void label297_Click(object sender, EventArgs e)
        {
            job10 = false;
            if (skinGroupBox10.Text == "厂家-已登录")
            {
                if (textBox15.Text.Length == 8 || textBox15.Text.Length == 7)
                {
                    Job = textBox15.Text;
                    machine = machineidentifier;

                    List<string> msgL = new List<string>();
                    string filePath = Application.StartupPath.ToString() + "\\job.txt"; //打开根目录下的Auditor.TXT 路径
                    FileOperate.OpenFileList(filePath, out msgL); //存储到数值中
                    string[] job1 = new string[100];

                    for (int i = 0; i < msgL.Count; i++) //循环数组数值最大长度
                    {
                        job1 = msgL[i].Split(','); //分割数组中,前面和后面的。
                        if (job1[0] == textBox15.Text) //判断分割后的前面和文本是否一样，一样则直接打开相同
                        {
                            if (DateTime.Parse(job1[1]) > DateTime.Now)
                            {
                                job10 = true;
                                label93.Text = job1[1].ToString();
                            }
                            else
                            {
                                try
                                {
                                    DataSet d = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0001", "0011",
                                        Job + "|" + machine, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    if (d.Tables[0].Rows.Count >= 0)
                                    {
                                        string z = d.Tables[0].Rows[0][0].ToString();
                                        if (Convert.ToInt32(z) >= 1) //审核是否有权限
                                        {
                                            job10 = true;
                                            label93.Text = System.DateTime.Now.AddDays(7).ToString();
                                            msgL[i] = textBox15.Text + "," + System.DateTime.Now.AddDays(7).ToString();
                                                //更改日期，插人
                                            FileOperate.SaveFileList(filePath, msgL);
                                            msgL.Clear();
                                        }
                                        else
                                        {
                                            label93.Text = "没有权限";
                                            textBox15.Text = "";
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("读取作业员无权限，请维护系统！", "提示！");
                                        textBox15.Text = "";
                                        textBox15.BackColor = Color.Transparent;
                                    }
                                }
                                catch
                                {
                                    MessageBox.Show("从系统未获取到权限！请维护E化系统权限！", "提示!");
                                    textBox15.Text = "";
                                }
                            }
                        }
                        if (label93.Text != "没有权限")
                        {
                            label358.Text = piecesNumber; //张数
                            label353.Text = textBox76.Text; //冲数

                            //PcConnectPlc.Write_Data_FxUsb("M2903", 1);//可以运行

                        }
                    }
                    if (job10 == false) //如果没记录，则连接服务器读取
                    {
                        try
                        {
                            DataSet d = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0001", "0011",
                                Job + "|" + machine, System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            if (d.Tables[0].Rows.Count >= 0)
                            {
                                string z = d.Tables[0].Rows[0][0].ToString(); //审核是否有权限
                                if (Convert.ToInt32(z) >= 1)
                                {
                                    string aa = textBox15.Text + "," + DateTime.Now.AddDays(7);
                                    msgL.Add(aa);
                                    FileOperate.SaveFileList(filePath, msgL);
                                    msgL.Clear();
                                    job10 = true;
                                    label93.Text = System.DateTime.Now.AddDays(7).ToString();
                                }
                                else
                                {
                                    label93.Text = "没有权限";
                                    textBox15.Text = "";
                                }
                            }
                            else
                            {
                                MessageBox.Show("读取作业员无权限，请维护系统！", "提示！");
                                textBox25.BackColor = Color.Transparent;
                                textBox15.Text = "";
                            }
                        }
                        catch
                        {
                            MessageBox.Show("从系统未获取到权限！请维护E化系统权限！", "提示!");
                            textBox15.Text = "";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("输入信息有误请确认后输入！");
                    textBox15.Text = "";
                }

            }
        }

        private void label302_Click(object sender, EventArgs e)
        {
            Auditor10 = false;
            if (skinGroupBox10.Text == "厂家-已登录" && label93.Text != "没有权限")
            {
                if (textBox25.Text.Length == 8 && textBox25.Text != textBox15.Text)
                {
                    Auditor = textBox25.Text;

                    List<string> msgL = new List<string>();
                    string filePath = Application.StartupPath.ToString() + "\\Auditor.txt"; //打开根目录下的Auditor.TXT 路径
                    FileOperate.OpenFileList(filePath, out msgL); //存储到数值中
                    string[] Auditor1 = new string[2];

                    for (int i = 0; i < msgL.Count; i++) //循环数组数值最大长度
                    {
                        Auditor1 = msgL[i].Split(','); //分割数组中,前面和后面的。
                        if (Auditor1[0] == textBox25.Text) //判断分割后的前面和文本是否一样，一样则直接打开相同
                        {
                            if (DateTime.Parse(Auditor1[1]) > DateTime.Now)
                            {
                                Auditor10 = true;
                                label94.Text = Auditor1[1].ToString();
                            }
                            else
                            {
                                msgL[i].TrimStart();
                                //  Auditor10 = true;
                                try
                                {
                                    DataSet d = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0001", "0012",
                                        Auditor + "|SFCZ1_ZD_PunchCut",
                                        System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                                    if (d.Tables[0].Rows.Count >= 0)
                                    {
                                        string f = d.Tables[0].Rows[0][0].ToString();
                                        if (Convert.ToInt32(f) >= 1) //审核是否有权限
                                        {
                                            Auditor10 = true;
                                            label94.Text = System.DateTime.Now.AddDays(7).ToString();
                                            msgL[i] = textBox25.Text + "," + System.DateTime.Now.AddDays(7).ToString();
                                                //更改日期，插人
                                            FileOperate.SaveFileList(filePath, msgL);
                                            msgL.Clear();
                                        }
                                        else
                                        {
                                            label94.Text = "没有权限";
                                            textBox25.Text = "";
                                        }
                                        //  textBox25.BackColor = Color.Green;
                                    }
                                    else
                                    {
                                        MessageBox.Show("读取审核人无权限，请维护系统！", "提示！");
                                        textBox25.BackColor = Color.Transparent;
                                        textBox25.Text = "";
                                    }
                                }
                                catch
                                {
                                    MessageBox.Show("从系统未获取到权限！请维护E化系统权限！", "提示!");
                                    textBox25.Text = "";
                                }
                            }
                        }
                    }
                    if (Auditor10 == false) //如果没记录，则连接服务器读取
                    {
                        try
                        {
                            DataSet d = webFun.getDataFromSer(serverAccount, serverPwd, "#01", "0001", "0012",
                                Auditor + "|SFCZ1_ZD_PunchCut", System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                            if (d.Tables[0].Rows.Count >= 0)
                            {
                                string f = d.Tables[0].Rows[0][0].ToString(); //审核是否有权限
                                if (Convert.ToInt32(f) >= 1)
                                {
                                    string aa = textBox25.Text + "," + DateTime.Now.AddDays(7);
                                    msgL.Add(aa);
                                    FileOperate.SaveFileList(filePath, msgL);
                                    msgL.Clear();
                                    Auditor10 = true;
                                    label94.Text = System.DateTime.Now.AddDays(7).ToString();
                                }
                                else
                                {
                                    label94.Text = "没有权限";
                                    textBox25.Text = "";
                                }
                                //  textBox25.BackColor = Color.Green;
                            }
                            else
                            {
                                MessageBox.Show("读取审核人无权限，请维护系统！", "提示！");
                                textBox25.BackColor = Color.Transparent;
                                textBox25.Text = "";
                            }
                        }
                        catch
                        {
                            MessageBox.Show("从系统未获取到权限！请维护E化系统权限！", "提示!");
                            textBox25.Text = "";
                        }
                    }
                }
                else
                {
                    MessageBox.Show("输入格式有误或者输入信息与作业员相同请确认后输入！");
                    textBox25.Text = "";
                }
            }
            else
            {
                MessageBox.Show("厂家权限或者作业员未取得权限！");
                textBox25.Text = "";
            }
        }

        private void button42_Click(object sender, EventArgs e) //生产结束
        {
            try
            {

                if (PLC_DS[13] != 0)
                {
                    if (PLC_DS[13] != 0 && textBox77.Text != "" && textBox79.Text != "" && textBox80.Text != "")
                    {

                        //mold3 = mold3 + 1;


                        label35.Text = (liaohao + 1).ToString();
                        liaohao = Convert.ToInt32(label35.Text);
                        label30.Text = liaohao.ToString();

                        if (textBox61.Text == "0")
                        {
                            textBox61.Text = "198.66";
                        }
                        if (textBox19.Text == "0")
                        {
                            textBox19.Text = "1";
                        }

                        shenghe = false;

                        houjia = 0;

                        PNL = false;

                        保存ToolStripMenuItem_Click(null, null);

                        Run = false;

                        label25.Text = textBox78.Text;

                        string ret = webFun.sendDataToSerGrp(serverAccount, serverPwd, "#01", "0004", "0006",
                            "paperNo|MacState|StartTime|EndTime|Lotnum|Layer|MainSerial|Partnum|WorkNo|SfcLayer|LayerName|Serial|IsMain|OrderId|Item2|Item1|Item3|Item4|Item5|Item6|Item7|Item8|Qty|Item10|Item11|Item13|Item14|Item15|Item16|CreateEmpid|CreateTime|ModifyEmpid|ModifyTime",
                            textBox23.Text + "|正常|" + label352.Text + "|" +
                            System.DateTime.Now.ToString("yyyyMMddHHmmss") + "|"
                            + textBox14.Text + "|" + textBox17.Text + "|" + textBox16.Text + "|" + textBox12.Text + "|" +
                            textBox20.Text + "|"
                            + label367.Text + "|" + textBox71.Text + "|" + textBox69.Text + "|" + textBox70.Text + "|" +
                            label354.Text + "|"
                            + textBox24.Text + "|" + textBox21.Text + "|" + textBox5.Text + "|" + textBox18.Text + "|" +
                            textBox29.Text + "|"
                            + textBox26.Text + "|"
                            + textBox79.Text + "|" + textBox77.Text + "|" + label22.Text + " | " + textBox80.Text + "|" +
                            textBox83.Text + "|"
                            + textBox27.Text + "|" + textBox64.Text + "|" + textBox25.Text + "|" + textBox19.Text + "|"
                            + textBox15.Text + "|" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "|" +
                            textBox15.Text + "|" + System.DateTime.Now.ToString("yyyyMMddHHmmss"),
                            System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                        timer4.Enabled = true;

                        PcConnectPlc.Write_Data_FxUsb("M2904", 1); // 关PLC

                        //////  奥特姆版本
                        // string ret = webFun.sendDataToSerGrp(textBox97.Text, textBox98.Text, "#01", "0004", "0006", "paperNo|MacState|StartTime|EndTime|Lotnum|Layer|MainSerial|Partnum|WorkNo|SfcLayer|LayerName|Serial|IsMain|OrderId|Item2|Item1|Item3|Item4|Item5|Item6|Item7|Item8|Qty|Item10|Item11|Item12|Item13|Item14|Item15|Item16|Item17|Item18|Item19|Item20|Item21|Item22|Item23|Item24|Item25|Item26|CreateEmpid|CreateTime",
                        //  textBox23.Text + "|正常|" + label352.Text + "|" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "|"
                        //  + textBox14.Text + "|" + textBox17.Text + "|" + textBox16.Text + "|" + textBox12.Text + "|" + textBox20.Text + "|"
                        //  + label367.Text + "|" + textBox71.Text + "|" + textBox69.Text + "|" + textBox70.Text + "|" + label354.Text + "|"
                        // + textBox24.Text + "|" + textBox21.Text + "|" + textBox5.Text + "|" + textBox18.Text + "|" + textBox29.Text + "|" + textBox26.Text + "|"
                        //  + textBox79.Text + "|" + textBox77.Text + "|" + textBox78.Text + " | " + textBox80.Text + "|" + textBox83.Text + "|"
                        // + textBox61.Text + "|" + textBox27.Text + "|" + textBox64.Text + "|" + textBox25.Text + "|" + textBox19.Text + "|"
                        // + textBox94.Text + "|" + textBox93.Text + "|" + textBox91.Text + "|" + textBox89.Text + "|" + textBox92.Text + "|" + textBox86.Text + "|"
                        //+ textBox85.Text + "|" + textBox90.Text + "|" + textBox88.Text + "|" + textBox87.Text + "|" + textBox15.Text + "|" + System.DateTime.Now.ToString("yyyyMMddHHmmss"),
                        //  System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                        textBox14.Enabled = true; //开始后对应输入控件不可输入
                        textBox17.Enabled = true;
                        textBox24.Enabled = true;
                        textBox15.Enabled = true;
                        textBox25.Enabled = true;
                        textBox32.Enabled = true;
                        textBox31.Enabled = true;
                        textBox37.Enabled = true;
                        skinGroupBox32.Enabled = true;
                        skinGroupBox31.Enabled = true;
                        skinGroupBox37.Enabled = true;
                        textBox82.Enabled = true;
                        textBox25.Visible = true;
                        textBox96.Enabled = true;
                        textBox95.Enabled = true;

                        textBox82.Text = textBox24.Text; //复制到上次模具,,,

                        label26.Text = textBox21.Text;

                        if (ret == "OK") //
                        {

                            PcConnectPlc.Write_Data_FxUsb("M113", 1); // 正常停机

                            List<string> msgL = new List<string>(); //存数据

                            string filePath = @"D:\Data backup\" + System.DateTime.Now.ToString("yyyyMMdd") +
                                              "\\send.txt"; //打开根目录下的Auditor.TXT 路径
                            FileOperate.OpenFileList(filePath, out msgL); //存储到数值中
                            string[] Mold1 = new string[500];

                            //  FileOperate.SaveFileString(@"D:\数据备份\报警记录\" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt", msgS1);
                            //奥特曼
                            msgL[0] = msgL[0].Insert(0,
                                DateTime.Now.ToString() + "结束" + "\r\n" +
                                "paperNo|MacState|StartTime|EndTime|Lotnum|Layer|MainSerial|Partnum|WorkNo|SfcLayer|LayerName|Serial|IsMain|OrderId|Item2|Item1|Item3|Item4|Item5|Item6|Item7|Item8|Qty|Item10|Item11|Item12|Item13|Item14|Item15|CreateEmpid|CreateTime"
                                + "\r\n" + textBox23.Text + "订单号|" + label344.Text + "运行状态|" + label352.Text + "开始时间|" +
                                System.DateTime.Now.ToString("yyyyMMddHHmmss") + "结束时间|" + "\r\n"
                                + textBox14.Text + "批号|" + textBox17.Text + "层别|" + textBox16.Text + "主图|" +
                                textBox12.Text + "料号|" + textBox20.Text + "共令|" + "\r\n"
                                + label367.Text + "SFC|" + textBox71.Text + "称别|" + textBox69.Text + "涂程序|" +
                                textBox70.Text + "主配件|" + label354.Text + "几次过站|" + "\r\n"
                                + textBox24.Text + "模具SG|" + textBox21.Text + "模具编号|" + textBox5.Text + "保养|" +
                                textBox18.Text + "报废|" + textBox29.Text + "刀量|" + textBox26.Text + "BOM|" + "\r\n"
                                + textBox79.Text + "类型|" + textBox77.Text + "检验结果|" + label22.Text + "实际PNL | " +
                                textBox80.Text + "规格判断|" + textBox83.Text + "自主检查|" + "\r\n"
                                + textBox61.Text + "调模高度|" + textBox27.Text + "间距1|" + textBox64.Text + "手臂高度|" +
                                textBox25.Text + "操作人员|" + textBox15.Text + "审合人员|" + "\r\n"
                                + textBox96.Text + "架模人员|" + label5.Text + "架模时间|" + textBox95.Text + "调机人员|" +
                                label6.Text + "调机时间|" + textBox19.Text + "冲数|" + "\r\n"
                                + textBox84.Text + "提取数PNL|" + textBox93.Text + "奥特母-每卷总量|" + textBox91.Text + "实际量|" +
                                textBox89.Text + "翻页面|" + "\r\n"
                                + textBox92.Text + "空料数|" + textBox86.Text + "载带空数|" + textBox85.Text + "感应延时|" +
                                textBox90.Text + "拉料速率|" + "\r\n"
                                + textBox88.Text + "自动速度|" + textBox87.Text + "张力|" + label24.Text + "生产开始前检验张数|" +
                                label341.Text + " - " + label342.Text + " - " + label343.Text + " - " + liaohao + "料号|"
                                + System.DateTime.Now.ToString("yyyyMMddHHmmss") + "\r\n");

                            FileOperate.SaveFileList(filePath, msgL);
                            msgL.Clear();
                            label341.Text = "0";
                            label342.Text = "0";
                            label343.Text = "0";

                            List<string> msgL1 = new List<string>(); //数据保存

                            string filePath1 = Application.StartupPath.ToString() + "\\liaohao.txt";

                            FileOperate.OpenFileList(filePath1, out msgL1);

                            msgL1[0] = "0";

                            msgL1[1] = "";

                            msgL1[2] = "";

                            msgL1[3] = "";

                            msgL1[4] = "";

                            msgL1[5] = "";

                            msgL1[6] = "";

                            FileOperate.SaveFileList(filePath1, msgL1);
                            msgL1.Clear();




                            textBox14.Text = "";
                            textBox69.Text = "";
                            textBox16.Text = "";
                            textBox70.Text = "";
                            //textBox21.Text = "";
                            //textBox29.Text = "";
                            textBox5.Text = "";
                            textBox19.Text = "";
                            textBox15.Text = "";
                            textBox17.Text = "";
                            textBox71.Text = "";
                            textBox20.Text = "";
                            textBox30.Text = "";
                            textBox12.Text = "";
                            textBox26.Text = "";
                            //textBox24.Text = "";
                            textBox25.Text = "";
                            //textBox18.Text = "";
                            label354.Text = "";
                            textBox23.Text = "";
                            textBox78.Text = "0";
                            textBox81.Text = "0";
                            textBox77.Text = "";
                            textBox79.Text = "";
                            textBox80.Text = "";

                            label353.Text = "初始张数";
                            label358.Text = "初始冲数";

                            label93.Text = "没有权限";
                            label94.Text = "没有权限";

                            button25.Enabled = true;
                            textBox25.Visible = false;
                            button42.Enabled = false; //开关关闭
                            button42.BackColor = Color.Transparent;

                        }
                    }
                    else
                    {
                        //MessageBox.Show("未读取到PLC数据或者未勾选对应类型，检验结果，规格判断！");
                    }
                }
            }
            catch
            {
            }
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            // label356.Text =
            textBox79.Text = "生产初件";
            // """生产初件";
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            textBox79.Text = "生产记录";
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            textBox77.Text = "合格继续生产";
            //     
            //   label357.Text = "合格继续生产";
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            textBox77.Text = "不合格重新调机";
            //     ""
            // label357.Text = "不合格重新调机";
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {

            textBox77.Text = "条件认可";
            //   ""
            // label357.Text = "条件认可";
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
//掉头冲产品
            if (checkBox5.Checked)
            {
                label365.Visible = true; //除以2
            }
            else
            {
                label365.Visible = false; //
            }
        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            textBox80.Text = "OK";
            //  label366.Text = "OK";
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            textBox80.Text = "NG";
            //    label366.Text = "NG";
        }

        private void button87_Click_1(object sender, EventArgs e) //读取
        {
            //MOLD();//提取数据
            if (skinGroupBox10.Text == "厂家-已登录")
            {
                textBox24.Text = textBox82.Text; //可以读到数据
            }
            else
            {
                textBox32.Text = "*" + textBox82.Text; //可以读到数据
            }
        }

        private void textBox96_Click(object sender, EventArgs e)
        {
            if (skinGroupBox10.Text != "厂家-已登录")
            {
                textBox96.BackColor = Color.Green;
                saoma2 = textBox96.Text;
                textBox32.Focus();
                saoma1 = 6;
                textBox24.BackColor = Color.White;
                textBox95.BackColor = Color.White;
                //textBox96.BackColor = Color.White;
                textBox14.BackColor = Color.White;
                textBox17.BackColor = Color.White;
                textBox15.BackColor = Color.White;
                textBox25.BackColor = Color.White;
            }
        }

        private void textBox95_Click(object sender, EventArgs e)
        {
            if (skinGroupBox10.Text != "厂家-已登录")
            {
                textBox95.BackColor = Color.Green;
                saoma2 = textBox95.Text;
                textBox32.Focus();
                saoma1 = 7;

                textBox24.BackColor = Color.White;
                //textBox95.BackColor = Color.White;
                textBox96.BackColor = Color.White;
                textBox14.BackColor = Color.White;
                textBox17.BackColor = Color.White;
                textBox15.BackColor = Color.White;
                textBox25.BackColor = Color.White;
            }
        }


        #endregion

        //检验确定按钮
        private void button2_Click(object sender, EventArgs e)
        {
            if (label93.Text != "没有权限" && textBox26.Text != "")
            {
                if (textBox77.Text == "合格继续生产" || textBox77.Text == "条件认可")
                {
                    PcConnectPlc.Write_Data_FxUsb("M2903", 1); //可以运行

                    //MOLD();//提取数据

                    if (Convert.ToInt32(label35.Text) == 0 || Convert.ToInt32(label35.Text) >= 4 ||
                        textBox24.Text != label34.Text) //初见防呆
                    {
                        liaohao = 0;
                        label30.Text = "0";
                        label29.Text = textBox12.Text;
                        radioButton12.Checked = true;
                        radioButton11.Checked = false;
                        textBox79.Text = "生产初件";
                        textBox25_Click(null, null); //作业员
                        label35.Text = "0";
                    }
                    else
                    {
                        radioButton12.Checked = false;
                        radioButton11.Checked = true;
                        textBox79.Text = "生产记录";
                        button25_Click(null, null); //生产开始

                    }

                    textBox25.Visible = true;
                    //textBox25_Click(null, null);//作业员
                    skinGroupBox32.Enabled = false;
                    textBox82.Enabled = false;
                    textBox14.Enabled = false;
                    textBox96.Enabled = false; //开始后对应输入控件不可输入
                    textBox95.Enabled = false;
                    textBox24.Enabled = false;
                    textBox17.Enabled = false;

                    textBox15.Enabled = false;

                    textBox31.Enabled = false;
                    textBox37.Enabled = false;

                    Run = true; //运行开始


                }
                else
                {
                    MessageBox.Show("选择合格继续生产或者条件认可，才可检验确认！");
                }
            }
            else
            {
                MessageBox.Show("BOM模具栏位空或者操作人员没有权限！");
            }
        }

        private void button78_Click(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {

            //看是否到达保养次数
            if (Convert.ToInt32(textBox76.Text) > 0) //不能为负数
            {
                int baoyan = Convert.ToInt32(textBox76.Text) - Convert.ToInt32(label353.Text); //保养

                if (baoyan > 0)
                {
                    textBox19.Text = baoyan.ToString();
                    by = baoyan + by;
                }
                if (Convert.ToInt32(textBox5.Text) >= 20000 && (by1 == false)) //保养次数累加
                {
                    by1 = true;
                    MessageBox.Show("模具已经冲裁达到20000次，请及时保养!");
                }
            }



            //检查张数   不知道是什么功能
            if (Convert.ToInt32(piecesNumber) > 0) //张数不能为负数
            {
                int cc = Convert.ToInt32(piecesNumber) - Convert.ToInt32(label358.Text); //张数
                a33 = cc;
                textBox81.Text = cc.ToString();
                if (cc > 0)
                {
                    if (label365.Visible == true) //切换
                    {
                        textBox78.Text = ((cc/numericUpDown26.Value)/2).ToString(); //pnl
                    }
                    else
                    {
                        textBox78.Text = (cc/numericUpDown26.Value).ToString(); //pnl
                    }
                    try
                    {
                        string[] dd = textBox78.Text.Split(new char[1] {'.'});
                        if (dd[1].Length >= 1)
                        {
                            label22.Text = dd[0] + "." + dd[1].Substring(0, 1);

                        }
                        else
                        {
                            label22.Text = dd[0];

                        }

                    }
                    catch
                    {
                        label22.Text = textBox78.Text;
                    }
                }
            }



            //不知什么东西
            textBox33.Text = txtShowData.Text;

            //白班夜班
            white_black();

            //冲数 张数

            piecesNumber = (((double) PcConnectPlc.double_Word_To_Int(PLC_DS[28], PLC_DS[29]))).ToString(); //张数

            textBox76.Text = (((double) PcConnectPlc.double_Word_To_Int(PLC_DS[30], PLC_DS[31]))).ToString(); //冲数

            //开单之类的

            if (numericUpDown26.Value == 1)
            {
                a4 = shuliang;
            }
            else
            {
                a4 = (Convert.ToInt32(shuliang)/numericUpDown26.Value).ToString();
            }
            textBox84.Text = a4;




            //批号之类的对比之类   不清楚

            if (sm3.Length >= 10)
            {
                string con = sm3.Substring(3, 10);

                if (con == textBox14.Text) //对比批号MF
                {
                    saomatouk = true; //扫码通过
                    label338.Text = con; //显示
                    sm3 = "";
                    label340.Text = "";
                    label318.Text = "OK";
                }

                if (con != textBox14.Text)
                {
                    saomatouh = true; //扫码混料
                    label338.Text = con; //显示
                    sm3 = ""; //显示
                    label340.Text = "";
                    label318.Text = "Mixture";
                }
            }
            if (sm3 == "NG\r\n")
            {
                saomatoug = true; //NG
                sm3 = "";
                label338.Text = "NG";
                label340.Text = "";
                label318.Text = "NG";
            }



            //数据上传
            serverSubmit();
        }

        //数据上传，以及故障检测
        private void serverSubmit()
        {

            try
            {
                label91.Text = System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"); //当前时间
                webFun.Discover();
                label95.Text = "通信正常";
                if ((Convert.ToInt32(DateTime.Now.ToString("ss"))) == 00)
                {
                    if (PLC_MS[2] == 1 && PLC_MS[3] == 0 && PLC_MS[4] == 0) //运行
                    {
                        name1 = "STATE";
                        value1 = "Run";
                        label344.Text = "Run";

                        string parameterName = name1;
                        string parameterValue = value1;
                        string ret = webFun.sendDataToSer(JDLaccount, JDLpwd, machineidentifier, parameterName,
                            parameterValue,
                            System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        if (ret == "OK")
                        {
                            textBox31.Text = parameterValue + " " +
                                             System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            textBox31.BackColor = Color.Green;
                            //send data correct
                        }
                    }

                    if (PLC_MS[4] == 1 && PLC_MS[2] == 0 && PLC_MS[3] == 0) //待机
                    {
                        name1 = "STATE";
                        value1 = "Waiting";
                        label344.Text = "Waiting";
                        string parameterName = name1;
                        string parameterValue = value1;
                        string ret = webFun.sendDataToSer(JDLaccount, JDLpwd, machineidentifier, parameterName,
                            parameterValue,
                            System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        if (ret == "OK")
                        {
                            textBox31.Text = parameterValue + " " +
                                             System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            textBox31.BackColor = Color.Yellow;
                            //send data correct
                        }
                    }

                    if (PLC_MS[3] == 1 && PLC_MS[2] == 0 && PLC_MS[4] == 0) //报警
                    {
                        name1 = "STATE";
                        value1 = "Alarm";
                        label344.Text = "Alarm";
                        string parameterName = name1;
                        string parameterValue = value1;
                        string ret = webFun.sendDataToSer(JDLaccount, JDLpwd, machineidentifier, parameterName,
                            parameterValue,
                            System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                        if (ret == "OK")
                        {
                            textBox31.Text = parameterValue + " " +
                                             System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                            textBox31.BackColor = Color.Red;
                            //send data correct
                        }
                    }
                }
                if ((PLC_MS[3] == 1) && (a11 == false)) //故障
                {
                    a11 = true;
                    name1 = "STATE";
                    value1 = "Alarm";

                    name2 = "AlarmCode";
                    value2 = PLC_DS[24].ToString();

                    value4 = PLC_DS[24].ToString();

                    name3 = "Alarmstatus";
                    value3 = "0";

                    string parameterName = name1 + "|" + name2 + "|" + name3;
                    string parameterValue = value1 + "|" + value2 + "|" + value3;
                    string ret = webFun.sendDataToSer(JDLaccount, JDLpwd, machineidentifier, parameterName,
                        parameterValue,
                        System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

                    if (ret == "OK")
                    {
                        textBox31.Text = parameterValue + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        textBox31.BackColor = Color.Red;
                        //send data correct
                    }
                }

                if ((PLC_MS[3] == 0) && (a11 == true))
                {
                    a11 = false;
                    name1 = "STATE";
                    value1 = "Alarm";

                    name2 = "AlarmCode";
                    value2 = value4;

                    name3 = "Alarmstatus";
                    value3 = "1";
                    string parameterName = name1 + "|" + name2 + "|" + name3;
                    string parameterValue = value1 + "|" + value2 + "|" + value3;
                    string ret = webFun.sendDataToSer(JDLaccount, JDLpwd, machineidentifier, parameterName,
                        parameterValue,
                        System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                    if (ret == "OK")
                    {
                        textBox31.Text = parameterValue + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                        //send data correct
                    }
                }

            }
            catch
            {
                label95.Text = "通信待机";
                tongxing = tongxing + 1;
                label11.Text = (tongxing).ToString();
            }
        }
        

        private void white_black()
        {
            running_State();
            try
            {
                string sj1 = System.DateTime.Now.ToString("HHmmss");
                if (Convert.ToInt32(sj1) >= 203000 || Convert.ToInt32(sj1) <= 80000) //白夜班交换
                {
                    a = 1;
                    label306.Text = "夜班";
                }
                else
                {
                    a = 0; //白班
                    label306.Text = "白班";
                }
            }
            catch
            {
            }
        }


    }
}

 



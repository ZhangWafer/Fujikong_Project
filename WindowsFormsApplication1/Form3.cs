using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;  //创建文件流
using System.Management; //获取CPU序列号  ，另还要将其 引用后，才能才这里using
using MathCollect;
namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {  //窗体2 按钮

            Application.Exit();
            this.Close(); // 关闭本窗体2
        }

        private void button2_Click(object sender, EventArgs e)
        {  //注册
            passwordFromPlc_FX();
        }

        private void passwordFromDll()
        {
            string S1 = Form1.frm1.Password_Read[1]; //ID号
            string S2 = textBox1.Text;  //注册密码
            int J1, J2, J3, J4, J5, J6, J7, J8, J9;
            int D1, D2, D3, D4, D5, D6, D7, D8, D9, D10;
            int S3, S4, S5;

            if (S2.Length != 9) { MessageBox.Show("注册失败，密码长度应为9！"); textBox1.Text = ""; return; }
            for (J1 = 5; J1 < Form1.frm1.Password_lenth; J1++)
            {   //对比dll文本密码是否用过
                if (S2 == Form1.frm1.Password_Read[J1])
                {
                    MessageBox.Show("密码无效，已注册过！");
                    textBox1.Text = "";
                    return;      //调出注册按钮的方法
                }
            }
            if (PcConnectPlc.Read_Data("D4102", 2) != 20110426)
            {    //PLC ID不是20110426
                MessageBox.Show("注册失败，PLC型号不对！");
                textBox1.Text = "";
                return;
            }

            J1 = Int32.Parse(S2.Substring(0, 1));   //ID和第一位 1   
            J2 = Int32.Parse(S2.Substring(1, 1));   //随机数百位 2
            J3 = Int32.Parse(S2.Substring(2, 1));   //天数十位 3
            J4 = Int32.Parse(S2.Substring(3, 1));   //随机数十位  4
            J5 = Int32.Parse(S2.Substring(4, 1));   //ID和第2位  5
            J6 = Int32.Parse(S2.Substring(5, 1));   //校验码十位     6
            J7 = Int32.Parse(S2.Substring(6, 1));   // 天数个位     
            J8 = Int32.Parse(S2.Substring(7, 1));   //校验码个位    8
            J9 = Int32.Parse(S2.Substring(8, 1));  //随机数个位   9//注册用的单个密码提取出来

            D1 = Int32.Parse(S1.Substring(0, 1));
            D2 = Int32.Parse(S1.Substring(1, 1));
            D3 = Int32.Parse(S1.Substring(2, 1));
            D4 = Int32.Parse(S1.Substring(3, 1));
            D5 = Int32.Parse(S1.Substring(4, 1));
            D6 = Int32.Parse(S1.Substring(5, 1));
            D7 = Int32.Parse(S1.Substring(6, 1));
            D8 = Int32.Parse(S1.Substring(7, 1));
            D9 = Int32.Parse(S1.Substring(8, 1));
            D10 = Int32.Parse(S1.Substring(9, 1));
            S3 = D1 + D2 + D3 + D4 + D5 + D6 + D7 + D8 + D9 + D10;  //注册用的ID和

            if (S3 < 10) { S3 = S3 + 10; } else if (S3 > 99) { S3 = S3 - 50; }
            S4 = J1 + J2 + J3 + J4 + J5 + J7 + J9;     //密码 和，与在其中两位校验和比较
            if (S4 < 10) { S4 = S4 + 10; } else if (S4 > 99) { S4 = S4 - 50; }
            J5 = J1 * 10 + J5;     //密码里 隐藏的ID校验和

            if ((J5 == S3) && (S4 == J6 * 10 + J8))
            {   //密码里 隐藏的ID校验和与注册ID和一致；密码里计算出来的校验和密码里隐藏的校验和一致
                S5 = (J3 * 10 + J7) * 24;   //可用小时数
                if (S5 == 2376)
                {   //99小时数为2376
                    S5 = 9876; //PLC9876才为 无限制
                }
                S5 = J3 * 10 + J7;
                Form1.frm1.Password_Read[0] = S5.ToString();  //可用天数
                Form1.frm1.Password_Read[Form1.frm1.Password_lenth] = S2; //密码写入新的最后一行

                //////////////////密码正确，以下把密码入寄存器，记录//////////////////////
                S5 = PcConnectPlc.Read_Data("D16402", 1);  //PLC自然时间 年
                Form1.frm1.Password_Read[2] = S5.ToString();
                S5 = PcConnectPlc.Read_Data("D16403", 1);  //PLC自然时间 月
                Form1.frm1.Password_Read[3] = S5.ToString();
                S5 = PcConnectPlc.Read_Data("D16404", 1);  //PLC自然时间 日
                Form1.frm1.Password_Read[4] = S5.ToString();
                Form1.frm1.JieMi();
                using (StreamWriter sw = File.CreateText("ActPcUSB.dll"))//创建或打开一个文件用于写入 UTF-8 编码的文本。
                {
                    for (J1 = 0; J1 <= Form1.frm1.Password_lenth; J1++)
                    {
                        sw.WriteLine(Form1.frm1.Password_Read[J1]);
                    }
                    sw.Close();
                }
                Form1.frm1.JiaMi();
                MessageBox.Show("注册成功，请重新打开软件！");
                Application.Exit();

            }
            else
            {
                MessageBox.Show("注册失败，密码错误！");
                textBox1.Text = "";
                return;
            }
        }

        private void passwordFromPlc_XD()
        {
            string S1 = label2.Text;    //ID
            string S2 = textBox1.Text;  //注册密码
            int J1, J2, J3, J4, J5, J6, J7, J8, J9;
            int D1, D2, D3, D4, D5, D6, D7, D8, D9, D10;
            int S3, S4, S5;
            string MiMa;

            if (S2.Length != 9) { MessageBox.Show("注册失败，密码长度应为9！"); textBox1.Text = ""; return; }
            for (J1 = 0; J1 <= 47; J1++)
            {   //对比PLC 48个密码寄存器 
                J2 = 41094 + J1 * 2; //D506
                MiMa = "D" + J2.ToString();
                if (Int32.Parse(S2) == PcConnectPlc.Read_Data(MiMa, 2))
                {
                    MessageBox.Show("密码无效，已注册过！");
                    textBox1.Text = "";
                    return;      //调出注册按钮的方法
                }
            }
            if (PcConnectPlc.Read_Data("D41190", 2) != 20110426)
            {    //PLC ID不是20110426
                MessageBox.Show("注册失败，PLC型号不对！");
                textBox1.Text = "";
                return;
            }

            J1 = Int32.Parse(S2.Substring(0, 1));   //ID和第一位 1   
            J2 = Int32.Parse(S2.Substring(1, 1));   //随机数百位 2
            J3 = Int32.Parse(S2.Substring(2, 1));   //天数十位 3
            J4 = Int32.Parse(S2.Substring(3, 1));   //随机数十位  4
            J5 = Int32.Parse(S2.Substring(4, 1));   //ID和第2位  5
            J6 = Int32.Parse(S2.Substring(5, 1));   //校验码十位     6
            J7 = Int32.Parse(S2.Substring(6, 1));   // 天数个位     
            J8 = Int32.Parse(S2.Substring(7, 1));   //校验码个位    8
            J9 = Int32.Parse(S2.Substring(8, 1));  //随机数个位   9//注册用的单个密码提取出来

            D1 = Int32.Parse(S1.Substring(0, 1));
            D2 = Int32.Parse(S1.Substring(1, 1));
            D3 = Int32.Parse(S1.Substring(2, 1));
            D4 = Int32.Parse(S1.Substring(3, 1));
            D5 = Int32.Parse(S1.Substring(4, 1));
            D6 = Int32.Parse(S1.Substring(5, 1));
            D7 = Int32.Parse(S1.Substring(6, 1));
            D8 = Int32.Parse(S1.Substring(7, 1));
            D9 = Int32.Parse(S1.Substring(8, 1));
            D10 = Int32.Parse(S1.Substring(9, 1));
            S3 = D1 + D2 + D3 + D4 + D5 + D6 + D7 + D8 + D9 + D10;  //注册用的ID和

            if (S3 < 10) { S3 = S3 + 10; } else if (S3 > 99) { S3 = S3 - 50; }
            S4 = J1 + J2 + J3 + J4 + J5 + J7 + J9;     //密码 和，与在其中两位校验和比较
            if (S4 < 10) { S4 = S4 + 10; } else if (S4 > 99) { S4 = S4 - 50; }
            J5 = J1 * 10 + J5;     //密码里 隐藏的ID校验和

            if ((J5 == S3) && (S4 == J6 * 10 + J8))
            {   //密码里 隐藏的ID校验和与注册ID和一致；密码里计算出来的校验和密码里隐藏的校验和一致
                S5 = (J3 * 10 + J7) * 24;   //可用小时数
                if (S5 == 2376)
                {   //99小时数为2376
                    S5 = 9876; //PLC9876才为 无限制
                }
                PcConnectPlc.Write_Data("D41088", S5);   //写可用小时数
                S5 = J3 * 10 + J7;
                PcConnectPlc.Write_Data("D41192", S5);   //写可用天数
                //////////////////密码正确，以下把密码入寄存器，记录//////////////////////
                for (J1 = 0; J1 <= 47; J1++)
                {   //对比PLC 48个密码寄存器 
                    J2 = 41094 + J1 * 2;
                    MiMa = "D" + J2.ToString();
                    if (PcConnectPlc.Read_Data(MiMa, 2) == 0)
                    { //等于0则说明这个寄存器是新的
                        PcConnectPlc.Write_Data(MiMa, Int32.Parse(S2)); //写入密码

                        S5 = PcConnectPlc.Read_Data("D28690", 1);  //PLC自然时间 年   
                        PcConnectPlc.Write_Data("D41194", S5);     //HD106
                        S5 = PcConnectPlc.Read_Data("D28689", 1);  //PLC自然时间 月
                        PcConnectPlc.Write_Data("D41196", S5);     //HD108
                        S5 = PcConnectPlc.Read_Data("D28688", 1);  //PLC自然时间 日
                        PcConnectPlc.Write_Data("D41198", S5);      //HD110

                        MessageBox.Show("注册成功，请重新打开软件！");
                        Application.Exit();
                        break;  //调出for循环
                    }
                }
            }
            else
            {
                MessageBox.Show("注册失败，密码错误！");
                textBox1.Text = "";
                return;
            }

        }


        private void passwordFromPlc_FX()
        {
            string S1 = label2.Text;    //ID
            string S2 = textBox1.Text;  //注册密码
            int J1, J2, J3, J4, J5, J6, J7, J8, J9;
            int D1, D2, D3, D4, D5, D6, D7, D8, D9, D10;
            int S3, S4, S5;
            string MiMa;

            if (S2.Length != 9) { MessageBox.Show("注册失败，密码长度应为9！"); textBox1.Text = ""; return; }
            for (J1 = 0; J1 <= 47; J1++)
            {   //对比PLC 48个密码寄存器 
                J2 = 906 + J1 * 2; //D506
                MiMa = "D" + J2.ToString();
                if (Int32.Parse(S2) == PcConnectPlc.Read_Data_FxCom(MiMa, 2))
                {
                    MessageBox.Show("密码无效，已注册过！");
                    textBox1.Text = "";
                    return;      //调出注册按钮的方法
                }
            }
            if (PcConnectPlc.Read_Data_FxCom("D1002", 2) != 20110426)
            {    //PLC ID不是20110426
                MessageBox.Show("注册失败，PLC型号不对！");
                textBox1.Text = "";
                return;
            }

            J1 = Int32.Parse(S2.Substring(0, 1));   //ID和第一位 1   
            J2 = Int32.Parse(S2.Substring(1, 1));   //随机数百位 2
            J3 = Int32.Parse(S2.Substring(2, 1));   //天数十位 3
            J4 = Int32.Parse(S2.Substring(3, 1));   //随机数十位  4
            J5 = Int32.Parse(S2.Substring(4, 1));   //ID和第2位  5
            J6 = Int32.Parse(S2.Substring(5, 1));   //校验码十位     6
            J7 = Int32.Parse(S2.Substring(6, 1));   // 天数个位     
            J8 = Int32.Parse(S2.Substring(7, 1));   //校验码个位    8
            J9 = Int32.Parse(S2.Substring(8, 1));  //随机数个位   9//注册用的单个密码提取出来

            D1 = Int32.Parse(S1.Substring(0, 1));
            D2 = Int32.Parse(S1.Substring(1, 1));
            D3 = Int32.Parse(S1.Substring(2, 1));
            D4 = Int32.Parse(S1.Substring(3, 1));
            D5 = Int32.Parse(S1.Substring(4, 1));
            D6 = Int32.Parse(S1.Substring(5, 1));
            D7 = Int32.Parse(S1.Substring(6, 1));
            D8 = Int32.Parse(S1.Substring(7, 1));
            D9 = Int32.Parse(S1.Substring(8, 1));
            D10 = Int32.Parse(S1.Substring(9, 1));
            S3 = D1 + D2 + D3 + D4 + D5 + D6 + D7 + D8 + D9 + D10;  //注册用的ID和

            if (S3 < 10) { S3 = S3 + 10; } else if (S3 > 99) { S3 = S3 - 50; }
            S4 = J1 + J2 + J3 + J4 + J5 + J7 + J9;     //密码 和，与在其中两位校验和比较
            if (S4 < 10) { S4 = S4 + 10; } else if (S4 > 99) { S4 = S4 - 50; }
            J5 = J1 * 10 + J5;     //密码里 隐藏的ID校验和

            if ((J5 == S3) && (S4 == J6 * 10 + J8))
            {   //密码里 隐藏的ID校验和与注册ID和一致；密码里计算出来的校验和密码里隐藏的校验和一致
                S5 = (J3 * 10 + J7) * 24;   //可用小时数
                if (S5 == 2376)
                {   //99小时数为2376
                    S5 = 9876; //PLC9876才为 无限制
                }
                PcConnectPlc.Write_Data_FxCom("D900", S5);   //写可用小时数
                S5 = J3 * 10 + J7;
                PcConnectPlc.Write_Data_FxCom("D1004", S5);   //写可用天数
                //////////////////密码正确，以下把密码入寄存器，记录//////////////////////
                for (J1 = 0; J1 <= 47; J1++)
                {   //对比PLC 48个密码寄存器 
                    J2 = 906 + J1 * 2;
                    MiMa = "D" + J2.ToString();
                    if (PcConnectPlc.Read_Data_FxCom(MiMa, 2) == 0)
                    { //等于0则说明这个寄存器是新的
                        PcConnectPlc.Write_Data_FxCom(MiMa, Int32.Parse(S2)); //写入密码

                        S5 = PcConnectPlc.Read_Data_FxCom("D8018", 1);  //PLC自然时间 年   
                        PcConnectPlc.Write_Data_FxCom("D1006", S5);     //HD106
                        S5 = PcConnectPlc.Read_Data_FxCom("D8017", 1);  //PLC自然时间 月
                        PcConnectPlc.Write_Data_FxCom("D1008", S5);     //HD108
                        S5 = PcConnectPlc.Read_Data_FxCom("D8016", 1);  //PLC自然时间 日
                        PcConnectPlc.Write_Data_FxCom("D41010", S5);      //HD110

                        MessageBox.Show("注册成功，请重新打开软件！");
                        Application.Exit();
                        break;  //调出for循环
                    }
                }
            }
            else
            {
                MessageBox.Show("注册失败，密码错误！");
                textBox1.Text = "";
                return;
            }

        }


        private void Form3_Load(object sender, EventArgs e)
        {
            //label2.Text = Form1.frm1.Password_Read[1];  //ID号
            //--信捷XD系列
            //label2.Text = PcConnectPlc.Read_Data("D41090", 2).ToString();

            //--三菱系列
            label2.Text = PcConnectPlc.Read_Data_FxCom("D902", 2).ToString();
        }

    }
}

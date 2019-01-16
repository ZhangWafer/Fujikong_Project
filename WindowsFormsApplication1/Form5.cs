using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MathCollect;


namespace WindowsFormsApplication1
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        Form1 f1 = new Form1();
       

        private void Form5_Load(object sender, EventArgs e)
        {
            //textBox1.Text = PcConnectPlc.Read_Data_FxCom("D376", 2).ToString();
            //textBox2.Text = PcConnectPlc.Read_Data_FxCom("D378", 2).ToString();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((!NumMath.isAllNum(textBox1.Text)) || (!NumMath.isAllNum(textBox2.Text)))
            {
                MessageBox.Show("新密码格式错误，请输入纯数字密码！", "提示");
                return;
            }

            if ((textBox1.Text.Substring(0,1)=="0") || (textBox2.Text.Substring(0,1)=="0"))
            {
                MessageBox.Show("新密码格式错误，不能以0开头！", "提示");
                return;
            }

            if ((textBox1.Text.Length>6) || (textBox2.Text.Length>6))
            {
                MessageBox.Show("新密码格式错误，长度不能大于6！", "提示");
                return;
            }

            if (MessageBox.Show("是否保存新密码?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //PcConnectPlc.Write_Data_FxCom("D376", int.Parse(textBox1.Text));
                //PcConnectPlc.Write_Data_FxCom("D378", int.Parse(textBox2.Text));
                f1.cj = textBox2.Text;
                f1.gcs  = textBox3.Text;
                f1.czy = textBox1.Text;


                MessageBox.Show("保存退出软件新密码才可生效！", "提示");
                this.Close();
            }
            
        }

    }
}

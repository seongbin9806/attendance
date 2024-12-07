using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin.Controls;
using MaterialSkin;

namespace 출결관리프로그램
{
    public partial class MainForm : MaterialForm
    {
        private string studentNumber;

        // 생성자 추가: 학번을 매개변수로 받음
        public MainForm(string studentNumber)
        {
            InitializeComponent();

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.BlueGrey800,
                Primary.BlueGrey900,
                Primary.BlueGrey500,
                Accent.LightBlue200,
                TextShade.WHITE
            );

            timer1 = new Timer();
            timer1.Interval = 100;
            timer1.Tick += Timer1_Tick;

            this.studentNumber = studentNumber;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = DateTime.Now.ToString("F");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}

using MaterialSkin;
using MaterialSkin.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 출결관리프로그램
{
    public partial class loginForm : MaterialForm
    {
        private DatabaseHelper dbHelper;
        public loginForm()
        {
            InitializeComponent();

            this.Text = "동의대학교 출석관리 프로그램";

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

            dbHelper = new DatabaseHelper("localhost", "attendance", "attendance", "0000");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToString("F"); // label1에 현재날짜시간 표시, F:자세한 전체 날짜/시간
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            timer1.Interval = 100; //타이머 간격 100ms
            timer1.Start();  //타이머 시작
        }

        private void onLogin(object sender, EventArgs e)
        {
            string studentNumber = studentNumberInput.Text;  // 학번 입력값
            string password = passwordInput.Text;            // 비밀번호 입력값

            if (string.IsNullOrEmpty(studentNumber))
            {
                MessageBox.Show("학번을 입력해주세요.");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("비밀번호를 입력해주세요.");
                return;
            }

            // 학번과 비밀번호를 쿼리 문자열에 직접 삽입
            string query = "SELECT COUNT(*) FROM student WHERE studentNumber = '" + studentNumber + "' AND password = '" + password + "'";

            MySqlConnection connection = dbHelper.GetConnection();
            MySqlCommand command = new MySqlCommand(query, connection);

            try
            {
                dbHelper.OpenConnection(connection); // 연결 열기
                int count = Convert.ToInt32(command.ExecuteScalar());  // 학번과 비밀번호로 존재 여부 확인

                if (count > 0) // 로그인 성공
                {
                    // MainForm으로 학번을 전달하며 이동
                    MainForm mainForm = new MainForm(studentNumber);
                    mainForm.Show();  // MainForm 열기
                    this.Hide();
                }
                else // 로그인 실패
                {
                    MessageBox.Show("학번 또는 비밀번호가 잘못되었습니다.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류가 발생했습니다: " + ex.Message);
            }
            finally
            {
                dbHelper.CloseConnection(connection); // 연결 닫기
            }
        }

        private void onSignUp(object sender, EventArgs e)
        {
            string studentNumber = studentNumberInput.Text;  // 학번 입력값
            string password = passwordInput.Text;            // 비밀번호 입력값

            if (string.IsNullOrEmpty(studentNumber))
            {
                MessageBox.Show("학번을 입력해주세요.");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("비밀번호를 입력해주세요.");
                return;
            }

            MySqlConnection connection = dbHelper.GetConnection(); // dbHelper를 통해 MySqlConnection 가져오기

            try
            {
                // 학번 중복 확인 쿼리
                string checkQuery = "SELECT COUNT(*) FROM student WHERE studentNumber = '" + studentNumber + "'";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);

                dbHelper.OpenConnection(connection); // 연결 열기
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());  // ExecuteScalar로 결과 반환

                if (count > 0) // 학번이 이미 존재하면
                {
                    MessageBox.Show("이미 가입된 학번입니다.");
                }
                else
                {
                    // 학번이 없으면 새로 삽입
                    string insertQuery = "INSERT INTO student (studentNumber, password) VALUES ('" + studentNumber + "', '" + password + "')";
                    MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);

                    dbHelper.ExecuteNonQuery(insertQuery);  // 데이터베이스에 회원 가입 처리

                    MessageBox.Show("가입이 완료되었습니다.");
                    studentNumberInput.Text = "";
                    passwordInput.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류가 발생했습니다: " + ex.Message);
            }
            finally
            {
                dbHelper.CloseConnection(connection); // 연결 닫기
            }
        }
    }
}
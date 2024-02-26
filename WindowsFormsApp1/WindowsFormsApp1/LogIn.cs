using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;
using WindowsFormsApp1.FormDisplayManager;

namespace WindowsFormsApp1
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
        }
        private void textPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            string user = txtPhone.Text;
            string pass = txtPass.Text;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Không được để trống Số điện thoại và Mật khẩu", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng hàm nếu có trường trống
            }

            if (LoginManager(user, pass) == true || LoginStaff(user, pass) == true)
            {
                if (LoginManager(user, pass) == true)
                {
                    AppSettings.IsEmployee = false;
                    this.Hide();
                    DisplayManager manager = new DisplayManager();
                    manager.ShowDialog();

                }
                else
                {
                    AppSettings.IsEmployee = true;
                    LoginDAO.Instance.SaveId(user);
                    this.Hide();
                    DisplayStaff staff = new DisplayStaff();
                    staff.nameStaff = getName(user);
                    staff.ShowDialog();
                     // Hoặc false tùy theo trường hợp

                }
            }
            else
            {
                MessageBox.Show("Mật khẩu hoặc tài khoản sai!!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool LoginManager(string user, string pass)
        {
            return LoginDAO.Instance.LoginManager(user, pass);
        }

        bool LoginStaff(string user, string pass)
        {
            return LoginDAO.Instance.LoginStaff(user, pass);
        }

        string getName(string phone)
        {
            return LoginDAO.Instance.getName(phone);
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            AppSettings.IsEmployee = false;
        }
    }
}

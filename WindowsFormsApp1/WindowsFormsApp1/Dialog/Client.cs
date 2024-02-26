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

namespace WindowsFormsApp1.Dialog
{
    public partial class Client : Form
    {
        public string id, name, email, phone, money, level;
        public int idPhanLoai; //1. sửa; 2. thêm

        private void btnClient_Click(object sender, EventArgs e)
        {
            if (idPhanLoai == 2)
            {
                if (txtEmail.Text != "" && txtPhone.Text != "" && txtName.Text != "")
                {
                    if (ClientDAO.Instance.CheckClient(int.Parse(txtPhone.Text)))
                    {
                        MessageBox.Show("Khách hàng đã tồn tại!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        ClientDAO.Instance.addClient(txtName.Text, txtEmail.Text, int.Parse(txtPhone.Text));
                        this.Hide();
                    }
                    
                }
                else
                {
                    MessageBox.Show("Không được bỏ trống", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (txtEmail.Text != "" && txtPhone.Text != "" && txtName.Text != "")
                {
                    if (ClientDAO.Instance.CheckClient(int.Parse(txtPhone.Text)))
                    {
                        MessageBox.Show("SDT đã tồn tại!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else {
                        ClientDAO.Instance.updateClient(int.Parse(txtID.Text), txtName.Text, txtEmail.Text, int.Parse(txtPhone.Text));
                        this.Hide();
                    }                   
                }
                else
                {
                    MessageBox.Show("Không được bỏ trống", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        
        private void Client_Load(object sender, EventArgs e)
        {
            if(idPhanLoai == 2)
            {
                txtLevel.Text = "";
                txtMoney.Text = "";
                btnClient.Text = "Thêm";
            }
            else
            {
                txtID.Text = id;
                txtName.Text = name;
                txtMoney.Text = money;
                txtLevel.Text = level;
                txtEmail.Text = email;
                txtPhone.Text = phone;
                btnClient.Text = "Sửa";
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
            if (e.Handled)
            {
                MessageBox.Show("Bạn phải nhâp số", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        
        public Client()
        {
            InitializeComponent();
        }
    }
}

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
    public partial class Account : Form
    {
        public Account()
        {
            InitializeComponent();
        }
        public int idPhanLoai,id,sdt; //1=thêm, 2=sửa

        

        public string name, email, pass;

        private void txtSDT_TextChanged(object sender, EventArgs e)
        {

        }

        private void Account_Load(object sender, EventArgs e)
        {
            if(idPhanLoai == 1)
            {
                btnThem.Text = "Thêm";
            }
            else
            {
                btnThem.Text = "Sửa";
                txtName.Text = name;
                txtPass.Text = pass;
                txtEmail.Text = email;
                txtSDT.Text = sdt.ToString();
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (idPhanLoai == 1)
            {
                if (txtName.Text != "" && txtEmail.Text != "" && txtPass.Text != "" && txtSDT.Text != "")
                {
                    AccountDAO.Instance.add(txtName.Text,txtPass.Text,txtEmail.Text,int.Parse(txtSDT.Text));
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Không được bỏ trống", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (txtName.Text != "" && txtEmail.Text != "" && txtPass.Text != "" && txtSDT.Text != "")
                {
                    AccountDAO.Instance.update(txtName.Text, txtPass.Text, txtEmail.Text, int.Parse(txtSDT.Text),id);
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Không được bỏ trống", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

    }
}

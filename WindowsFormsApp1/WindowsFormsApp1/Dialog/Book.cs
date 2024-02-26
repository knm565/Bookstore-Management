using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WindowsFormsApp1.DAO;

namespace WindowsFormsApp1.Dialog
{
    public partial class Book : Form
    {
        public string tenSach,tenTacGia,NXB;
        public int SL,id,Gia;
        public int check; //nếu = 1 là sửa; = 2 là thêm

        private void txtMoney_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
            if (e.Handled)
            {
                MessageBox.Show("Bạn phải nhập số", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public Book()
        {
            InitializeComponent();
       
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if(check == 2)
            {
                if (txtTenSach.Text != "" && txtTacGia.Text != "" && txtNXB.Text != "" && txtSL.Text != "" && txtMoney.Text != "")
                {
                    if (BookDAO.Instance.checkSach(txtTenSach.Text, txtTacGia.Text, txtNXB.Text))
                    {
                        MessageBox.Show("Đã cập nhật số lượng "+ txtTenSach.Text+"!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        BookDAO.Instance.updateSL(txtTenSach.Text,txtTacGia.Text,txtNXB.Text,int.Parse(txtSL.Text));
                        this.Hide();
                    }
                    else
                    {
                        BookDAO.Instance.add(txtTenSach.Text, txtTacGia.Text, txtNXB.Text, int.Parse(txtSL.Text), int.Parse(txtMoney.Text));
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
                if (txtTenSach.Text != "" && txtTacGia.Text != "" && txtNXB.Text != "" && txtSL.Text != "")
                {
                    BookDAO.Instance.update(txtTenSach.Text, txtTacGia.Text, txtNXB.Text, int.Parse(txtSL.Text),id, int.Parse(txtMoney.Text));
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Không được bỏ trống", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            
        }

        private void txtSL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
                e.Handled = true;
            if (e.Handled)
            {
                MessageBox.Show("Bạn phải nhập số!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Book_Load(object sender, EventArgs e)
        {
            if(check == 1)
            {
                lableBook.Text = "Sửa Sách";
                btnThem.Text = "Sửa";
            }
            else
            {
                lableBook.Text = "Nhập Sách";
                btnThem.Text = "Thêm";
            }
            txtTacGia.Text = tenTacGia;
            txtTenSach.Text = tenSach;
            txtNXB.Text = NXB;
            txtMoney.Text = Gia.ToString();
            txtSL.Text = SL.ToString();
       
        }

       
    }
}

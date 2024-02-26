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
using WindowsFormsApp1.Dialog;

namespace WindowsFormsApp1.FormDisplayManager
{
    public partial class FormAccount : Form
    {
        public FormAccount()
        {
            InitializeComponent();
        }
        int id,sdt;
        string name, pass,email;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.idPhanLoai = 1;
            account.ShowDialog();
            dataGridViewBook.DataSource = AccountDAO.Instance.selectAccount();
            dataGridViewBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBook.Columns[0].Width = 100;
            dataGridViewBook.ClearSelection();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.idPhanLoai = 2;
            account.id = id;
            account.sdt = sdt;
            account.name = name;
            account.email = email;
            account.pass = pass;
            account.ShowDialog();
            dataGridViewBook.DataSource = AccountDAO.Instance.selectAccount();
            dataGridViewBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBook.Columns[0].Width = 100;
            dataGridViewBook.ClearSelection();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    (dataGridViewBook.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                    dataGridViewBook.ClearSelection();
                }
                else
                {
                    (dataGridViewBook.DataSource as DataTable).DefaultView.RowFilter = string.Format("TenTaiKhoan LIKE '{0}%'", textBox1.Text);
                    dataGridViewBook.ClearSelection();
                }
            
        }

        private void FormAccount_Load(object sender, EventArgs e)
        {
            dataGridViewBook.DataSource = AccountDAO.Instance.selectAccount();
            dataGridViewBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBook.Columns[0].Width = 100;
            dataGridViewBook.ClearSelection();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var mess = MessageBox.Show("Bạn có muốn đăng xuất", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (mess == DialogResult.Yes)
            {
                AccountDAO.Instance.delete(id);
                dataGridViewBook.DataSource = AccountDAO.Instance.selectAccount();
                dataGridViewBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewBook.Columns[0].Width = 100;
                dataGridViewBook.ClearSelection();
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
            }
        }

        private void dataGridViewBook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridViewBook.Rows[e.RowIndex];
                    id = int.Parse(row.Cells[0].Value.ToString());
                    name = row.Cells[1].Value.ToString();
                    pass = row.Cells[2].Value.ToString();
                    email = row.Cells[3].Value.ToString();
                    sdt = int.Parse(row.Cells[4].Value.ToString());
                    btnDelete.Enabled = true;
                    btnUpdate.Enabled = true;
                }
            }
            catch (Exception)
            {

            }
        }
    }
}

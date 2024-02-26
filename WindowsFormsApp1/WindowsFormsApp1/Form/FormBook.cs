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
    public partial class FormBook : Form
    {
        public FormBook()
        {
            InitializeComponent();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private int id;
        public string tenSach, tenTacGia, NXB;
        public int soLuong;
        public int gia;
        private void FormBook_Load(object sender, EventArgs e)
        {
            dataGridViewBook.DataSource = BookDAO.Instance.selectBook();
            dataGridViewBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBook.Columns[0].Width = 50;
            dataGridViewBook.ClearSelection();

        }

        private void dataGridViewBook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridViewBook.Rows[e.RowIndex];
                    id = int.Parse(row.Cells[0].Value.ToString());
                    tenSach = row.Cells[1].Value.ToString();
                    NXB = row.Cells[2].Value.ToString();
                    tenTacGia = row.Cells[3].Value.ToString();                    
                    soLuong = int.Parse(row.Cells[4].Value.ToString());
                    gia = int.Parse(row.Cells[5].Value.ToString());
                    btnDelete.Enabled = true;
                    btnUpdate.Enabled = true;
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnNhapSach_Click(object sender, EventArgs e)
        {
            Book nhap = new Book();
            nhap.check = 2;
            nhap.ShowDialog();
            dataGridViewBook.DataSource = BookDAO.Instance.selectBook();
            dataGridViewBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBook.Columns[0].Width = 50;
            dataGridViewBook.ClearSelection();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var mess = MessageBox.Show("Bạn có muốn xóa sản phẩm này", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if(mess == DialogResult.Yes)
            {
                BookDAO.Instance.delete(id);
                dataGridViewBook.DataSource = BookDAO.Instance.selectBook();
                dataGridViewBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewBook.Columns[0].Width = 50;
                dataGridViewBook.ClearSelection();
            }
            
        }

        private void dataGridViewBook_SelectionChanged(object sender, EventArgs e)
        {
                // Kiểm tra xem có ít nhất một hàng được chọn trong DataGridView
            if (dataGridViewBook.SelectedRows.Count > 0)
            {
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnUpdate.Enabled = false;
                btnDelete.Enabled = false;
            }
           
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Book sua = new Book();
            sua.check = 1;
            sua.tenSach = tenSach;
            sua.tenTacGia = tenTacGia;
            sua.NXB = NXB;
            sua.SL = soLuong;
            sua.id = id;
            sua.Gia = gia;
            sua.ShowDialog();
            dataGridViewBook.DataSource = BookDAO.Instance.selectBook();
            dataGridViewBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBook.Columns[0].Width = 50;
            dataGridViewBook.ClearSelection();
        }

        private void txtTimSach_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimSach.Text))
            {
                (dataGridViewBook.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                dataGridViewBook.ClearSelection();
            }
            else
            {
                (dataGridViewBook.DataSource as DataTable).DefaultView.RowFilter = string.Format("TenSach LIKE '{0}%'", txtTimSach.Text);
                dataGridViewBook.ClearSelection();
            }
        }
    }
}

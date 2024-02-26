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
    public partial class FormClient : Form
    {
        string id, name, email, phone, level, money;

        private void txtTimSach_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimSach.Text))
            {
                (dataGridViewClient.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                dataGridViewClient.ClearSelection();
            }
            else
            {
                (dataGridViewClient.DataSource as DataTable).DefaultView.RowFilter = string.Format("TenKhachHang LIKE '{0}%'", txtTimSach.Text);
                dataGridViewClient.ClearSelection();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(AppSettings.IsEmployee == true)
            {
                return;
            }
            
                var mess = MessageBox.Show("Bạn có muốn đăng xuất", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (mess == DialogResult.Yes)
                {
                    ClientDAO.Instance.delete(int.Parse(id));
                    dataGridViewClient.DataSource = ClientDAO.Instance.selectClient();
                    dataGridViewClient.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dataGridViewClient.Columns[0].Width = 50;
                    dataGridViewClient.ClearSelection();
                }
            
            
        }

        public FormClient()
        {
            InitializeComponent();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
        }

        private void FormClient_Load(object sender, EventArgs e)
        {
            dataGridViewClient.DataSource = ClientDAO.Instance.selectClient();
            dataGridViewClient.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewClient.Columns[0].Width = 50;
            dataGridViewClient.ClearSelection();
            
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            
            Client client = new Client();
            client.idPhanLoai = 2;
            client.ShowDialog();
            dataGridViewClient.DataSource = ClientDAO.Instance.selectClient();
            dataGridViewClient.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewClient.Columns[0].Width = 50;
            dataGridViewClient.ClearSelection();
        }

        private void dataGridViewClient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridViewClient.Rows[e.RowIndex];
                    id = row.Cells[0].Value.ToString();
                    name = row.Cells[1].Value.ToString();
                    email = row.Cells[2].Value.ToString();
                    phone = row.Cells[3].Value.ToString();
                    level = row.Cells[4].Value.ToString();
                    money = row.Cells[5].Value.ToString();
                    btnDelete.Enabled = true;
                    btnUpdate.Enabled = true;
                }
            }
            catch (Exception)
            {

            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(AppSettings.IsEmployee == true)
            {
                return;
            }
            
                Client client = new Client();
                client.idPhanLoai = 1;
                client.id = id;
                client.name = name;
                client.email = email;
                client.phone = phone;
                client.level = level;
                client.money = money;
                client.ShowDialog();
                dataGridViewClient.DataSource = ClientDAO.Instance.selectClient();
                dataGridViewClient.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewClient.Columns[0].Width = 50;
                dataGridViewClient.ClearSelection();
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
            
            
        }
    }
}

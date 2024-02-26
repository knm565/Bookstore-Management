using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.Dialog;
using WindowsFormsApp1.DAO;

namespace WindowsFormsApp1.FormDisplayManager
{
    public partial class FormBill : Form
    {
        public FormBill()
        {
            InitializeComponent();
        }

        private void FormBill_Load(object sender, EventArgs e)
        {
            LoadBillData();
        }

        private void LoadBillData()
        {
            dataGridView1.DataSource = BillDAO.Instance.SelectBill("", "", "");
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.ClearSelection();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string dateFilter = txtDate.Text;
            string employeeFilter = txtEmployee.Text;
            string clientNameFilter = txtNameClient.Text;

            // Gọi phương thức SelectBill với các điều kiện lọc
            dataGridView1.DataSource = BillDAO.Instance.SelectBill(dateFilter, employeeFilter, clientNameFilter);

            // Cập nhật DataGridView
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.ClearSelection();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem người dùng đã chọn hóa đơn nào trong DataGridView chưa
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Lấy ID của hóa đơn đã chọn
                int selectedBillID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["IDHoaDon"].Value);

                // Tạo và hiển thị cửa sổ chi tiết hóa đơn
                BillDetail billDetailForm = new BillDetail();
                billDetailForm.DisplayBillDetails(selectedBillID);
                billDetailForm.ShowDialog(); // Sử dụng ShowDialog để hiển thị cửa sổ chi tiết
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xem chi tiết.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

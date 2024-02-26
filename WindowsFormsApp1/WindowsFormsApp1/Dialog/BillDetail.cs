using System;
using System.Data;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;

namespace WindowsFormsApp1.Dialog
{
    public partial class BillDetail : Form
    {
        public BillDetail()
        {
            InitializeComponent();
        }

        public void DisplayBillDetails(int billID)
        {
            // Hiển thị thông tin hóa đơn và chi tiết hóa đơn dựa trên billID
            DataTable billInfo = BillDAO.Instance.SelectBill("", "", "").AsEnumerable()
                .Where(row => row.Field<int>("IDHoaDon") == billID)
                .CopyToDataTable();

            DataTable billDetails = BillDAO.Instance.GetBillDetails(billID);

            if (billInfo.Rows.Count > 0)
            {
                txtClient.Text = billInfo.Rows[0]["TenKhachHang"].ToString();
                txtEmployee.Text = billInfo.Rows[0]["NhanVien"].ToString();
                DateTime ngayInHoaDon = Convert.ToDateTime(billInfo.Rows[0]["NgayInHoaDon"]);
                txtDate.Text = ngayInHoaDon.ToString("dd/MM/yyyy");
                txtTotal.Text = billInfo.Rows[0]["TongTien"].ToString();
                txtGiamGia.Text = BillDAO.Instance.GetDiscountLevel(billID);
                txtPrice.Text = BillDAO.Instance.GetPrice(billID). ToString();
                // Hiển thị chi tiết hóa đơn trong DataGridView
                dataGridView1.DataSource = billDetails;
            }
        }

        private void BillDetail_Load(object sender, EventArgs e)
        {

        }

        private void txtClient_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

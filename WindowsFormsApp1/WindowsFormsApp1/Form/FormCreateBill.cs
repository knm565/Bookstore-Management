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

namespace WindowsFormsApp1
{
    public partial class FormCreateBill : Form
    {
        private int idBook;
        private string book;
        //private int createClickCount = 0;
        private int checkClickCount = 0;
        public FormCreateBill()
        {
            InitializeComponent();
            txtSoHoaDon.Text = "";
            txtSoTienTra.Text = "";
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnInBill.Enabled = false;
            btnHuy.Enabled = false;
        }

        private void FormCreateBill_Load(object sender, EventArgs e)
        {
            dataGridViewBook.DataSource = CreateBillDAO.Instance.selectCreateBill();
            dataGridViewBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBook.Columns[0].Width = 20;
            dataGridViewBook.ClearSelection();


            dataGridViewInfoBill.DataSource = CreateBillDAO.Instance.selectInfoBill(7);
            dataGridViewInfoBill.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfoBill.Columns[1].Width = 70;
            dataGridViewInfoBill.ClearSelection();

        }

        private void txtTenSach_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTenSach.Text))
            {
                (dataGridViewBook.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                dataGridViewBook.ClearSelection();
            }
            else
            {
                (dataGridViewBook.DataSource as DataTable).DefaultView.RowFilter = string.Format("TenSach LIKE '{0}%'", txtTenSach.Text);
                dataGridViewBook.ClearSelection();
            }
        }



        private void txtSoTienTra_TextChanged(object sender, EventArgs e)
        {
            if (txtSoHoaDon.Text != "")
            {
                btnInBill.Enabled = true;
            }
        }

        private void btnTaoHoaDon_Click(object sender, EventArgs e)
        {
            //createClickCount = 1;
            string time = DateTime.Now.ToString("HH:mm:ss");
            string date = DateTime.Now.ToString("yyyy/MM/dd");
            CreateBillDAO.Instance.createBill(date, time, LoginDAO.Instance.getId());
            txtSoHoaDon.Text = CreateBillDAO.Instance.getIdBill().ToString();
            btnTaoHoaDon.Enabled = false;

            dataGridViewInfoBill.DataSource = CreateBillDAO.Instance.selectInfoBill(int.Parse(txtSoHoaDon.Text));
            dataGridViewInfoBill.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfoBill.Columns[1].Width = 70;
            dataGridViewInfoBill.ClearSelection();
            btnHuy.Enabled = true;
        }

        private void dataGridViewBook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridViewBook.Rows[e.RowIndex];
                    idBook = int.Parse(row.Cells[0].Value.ToString());
                    if (txtSoHoaDon.Text != "")
                    {
                        btnThem.Enabled = true;
                    }
                    
                }
                else
                {
                    btnThem.Enabled = false;
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (CreateBillDAO.Instance.checkTonKho(idBook)) 
            {
                CreateBillDAO.Instance.addBook(int.Parse(txtSoHoaDon.Text), idBook);
                btnThem.Enabled = false;
                dataGridViewInfoBill.DataSource = CreateBillDAO.Instance.selectInfoBill(int.Parse(txtSoHoaDon.Text));
                dataGridViewInfoBill.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewInfoBill.Columns[1].Width = 70;
                dataGridViewInfoBill.ClearSelection();
                txtSoTien.Text = CreateBillDAO.Instance.getMoney(int.Parse(txtSoHoaDon.Text)).ToString();

                dataGridViewBook.DataSource = CreateBillDAO.Instance.selectCreateBill();
                dataGridViewBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewBook.Columns[0].Width = 20;
                dataGridViewBook.ClearSelection();
            }
            else
            { 
                 DialogResult result = MessageBox.Show("Sách này đã hết tồn kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            


        }

        private void dataGridViewInfoBill_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewInfoBill.Rows.Count > 0)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = this.dataGridViewInfoBill.Rows[e.RowIndex];
                    book = row.Cells[0].Value.ToString();
                    btnXoa.Enabled = true;
                }
                else
                {
                    btnXoa.Enabled = false;
                }
            }
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            CreateBillDAO.Instance.deleteBook(int.Parse(txtSoHoaDon.Text), book);
            btnXoa.Enabled = false;
            dataGridViewInfoBill.DataSource = CreateBillDAO.Instance.selectInfoBill(int.Parse(txtSoHoaDon.Text));
            dataGridViewInfoBill.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfoBill.Columns[1].Width = 70;
            dataGridViewInfoBill.ClearSelection();
            txtSoTien.Text = CreateBillDAO.Instance.getMoney(int.Parse(txtSoHoaDon.Text)).ToString();

            dataGridViewBook.DataSource = CreateBillDAO.Instance.selectCreateBill();
            dataGridViewBook.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewBook.Columns[0].Width = 20;
            dataGridViewBook.ClearSelection();

        }

        private void txtSoTien_TextChanged(object sender, EventArgs e)
        {
            if (txtGiamGia.Text == "")
            {
                txtSoTienTra.Text = txtSoTien.Text; ;
            }
            else if (txtGiamGia.Text == "5%")
            {
                txtSoTienTra.Text = (int.Parse(txtSoTien.Text) * 90 / 100).ToString();
            }
            else if (txtGiamGia.Text == "10%")
            {
                txtSoTienTra.Text = (int.Parse(txtSoTien.Text) * 85 / 100).ToString();
            }
            else if (txtGiamGia.Text == "15%")
            {
                txtSoTienTra.Text = (int.Parse(txtSoTien.Text) * 75 / 100).ToString();
            }

        }

        private void txtGiamGia_TextChanged(object sender, EventArgs e)
        {
            if (txtSoTienTra.Text != "") 
            {
                if (txtGiamGia.Text == "")
                {
                    txtSoTienTra.Text = txtSoTien.Text; ;
                }
                else if (txtGiamGia.Text == "5%")
                {
                    txtSoTienTra.Text = (int.Parse(txtSoTien.Text) * 95 / 100).ToString();
                }
                else if (txtGiamGia.Text == "10%")
                {
                    txtSoTienTra.Text = (int.Parse(txtSoTien.Text) * 90 / 100).ToString();
                }
                else if (txtGiamGia.Text == "15%")
                {
                    txtSoTienTra.Text = (int.Parse(txtSoTien.Text) * 85 / 100).ToString();
                }
            }
            
        }

        private void btnInBill_Click(object sender, EventArgs e)
        {
            checkClickCount = 1;
            CreateBillDAO.Instance.updateBill(txtName.Text, int.Parse(txtSoHoaDon.Text), int.Parse(txtSoTienTra.Text));
            //CreateBillDAO.Instance.updateKho(int.Parse(txtSoHoaDon.Text));
            txtName.Text = "";
            txtName.Text = "";
            btnInBill.Enabled = false;
            btnThem.Enabled = false;
            btnXoa.Enabled = false;
            btnTaoHoaDon.Enabled = true;
            txtSoHoaDon.Text = "";
            txtGiamGia.Text = "";
            txtSoTien.Text = "";
            txtSoTienTra.Text = "";
            dataGridViewInfoBill.DataSource = CreateBillDAO.Instance.selectInfoBill(7);
            dataGridViewInfoBill.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewInfoBill.Columns[1].Width = 70;
            dataGridViewInfoBill.ClearSelection();
            btnHuy.Enabled = false;
            int selectedBillID = CreateBillDAO.Instance.getIdBill();
            BillDetail billDetailForm = new BillDetail();
            billDetailForm.DisplayBillDetails(selectedBillID);
            billDetailForm.ShowDialog();
        }
        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {

            // Kiểm tra nếu ký tự không phải là số hoặc không phải là các phím điều hướng (Arrow keys), Delete, hoặc Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete && e.KeyChar != (char)Keys.Left && e.KeyChar != (char)Keys.Right)
            {
                // Từ chối ký tự nhập vào bằng cách loại bỏ nó khỏi bộ đệm
                e.Handled = true;
            }
        }


        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            string phoneNumber = txtPhone.Text;

            // Kiểm tra nếu phoneNumber không rỗng
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                string customerInfo = CreateBillDAO.Instance.FindCustomerInfoByPhoneNumber(phoneNumber);
                if (customerInfo != null)
                {
                    string[] infoParts = customerInfo.Split('|');
                    txtName.Text = infoParts[0];
                    txtCap.Text = infoParts[1];
                }
                else
                {
                    // Nếu không tìm thấy khách hàng, xóa dữ liệu cũ
                    txtName.Text = "";
                    txtCap.Text = "";
                }
                if (txtCap.Text == "Thành viên") txtGiamGia.Text = "5%";
                else if (txtCap.Text == "Thành viên Thân thiết") txtGiamGia.Text = "10%";
                else if (txtCap.Text == "VIP") txtGiamGia.Text = "15%";
            }
            else
            {
                // Xóa dữ liệu nếu phoneNumber rỗng
                txtName.Text = "";
                txtCap.Text = "";
                txtGiamGia.Text = "";
            }
        }

        private void FormCreateBill_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((txtSoHoaDon.Text != "") && (checkClickCount == 0))
            {
                DialogResult result = MessageBox.Show("Bạn chưa hoàn thành in hóa đơn! Bạn có chắc muốn đóng form này?", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    CreateBillDAO.Instance.deleteBill(CreateBillDAO.Instance.getIdBill());
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (txtSoHoaDon.Text != "" && checkClickCount == 0)
            {
                DialogResult result = MessageBox.Show("Bạn chưa hoàn thành in hóa đơn! Bạn có chắc muốn hủy bill này?", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result == DialogResult.OK)
                {
                    CreateBillDAO.Instance.deleteBill(CreateBillDAO.Instance.getIdBill());
                }
                txtName.Text = "";
                txtName.Text = "";
                btnInBill.Enabled = false;
                btnThem.Enabled = false;
                btnXoa.Enabled = false;
                btnTaoHoaDon.Enabled = true;
                txtSoHoaDon.Text = "";
                txtGiamGia.Text = "";
                txtSoTien.Text = "";
                txtSoTienTra.Text = "";
            }
            
        }
    }
}
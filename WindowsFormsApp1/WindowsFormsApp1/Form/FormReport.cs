using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp1.DAO;

namespace WindowsFormsApp1.FormDisplayManager
{
    public partial class FormReport : Form
    {
        public FormReport()
        {
            InitializeComponent();

        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            LoadYearComboBox();
            LoadMonthComboBox();

            // Set mặc định năm và tháng ban đầu
            cmbYear.SelectedIndex = 0; // Chọn năm đầu tiên
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1; // Chọn tháng hiện tại

            ShowMonthlyRevenue(); // Hiển thị biểu đồ theo tháng mặc định
        }

        private void LoadYearComboBox()
        {
            // Thêm năm từ 2023 đến năm hiện tại vào combobox
            int currentYear = DateTime.Now.Year;
            for (int year = 2023; year <= currentYear; year++)
            {
                cmbYear.Items.Add(year.ToString());
            }
        }

        private void LoadMonthComboBox()
        {
            // Thêm tên tháng vào combobox
            for (int month = 1; month <= 12; month++)
            {
                cmbMonth.Items.Add(System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(month));
            }
        }

        private void ShowMonthlyRevenue()
        {
            int selectedYear = int.Parse(cmbYear.SelectedItem.ToString());
            int selectedMonth = cmbMonth.SelectedIndex + 1;

            // Hiển thị biểu đồ theo tháng và năm đã chọn
            chart1.DataSource = ReportDAO.Instance.SeeMonthlyRevenue(selectedYear, selectedMonth);
            chart1.ChartAreas["ChartArea1"].AxisX.Title = $"Doanh thu của tháng {selectedMonth} năm {selectedYear}";

            chart1.Series["Tiền"].XValueMember = "DateBill";
            chart1.Series["Tiền"].YValueMembers = "TotalPrice";
        }

        private void btnMonthReport_Click(object sender, EventArgs e)
        {
            ShowMonthlyRevenue();
        }

        /*private void btnYearReport_Click(object sender, EventArgs e)
        {
            int selectedYear = int.Parse(cmbYear.SelectedItem.ToString());

            // Hiển thị biểu đồ theo năm đã chọn
            chart1.DataSource = ReportDAO.Instance.SeeYearlyRevenue(selectedYear);
            chart1.ChartAreas["ChartArea1"].AxisX.Title = $"Doanh thu của năm {selectedYear}";

            chart1.Series["Tiền"].XValueMember = "Month";
            chart1.Series["Tiền"].YValueMembers = "TotalPrice";
        }*/
        private void btnYearReport_Click(object sender, EventArgs e)
        {
            int selectedYear = int.Parse(cmbYear.SelectedItem.ToString());

            // Lấy dữ liệu doanh thu theo năm
            DataTable yearlyData = ReportDAO.Instance.SeeYearlyRevenue(selectedYear);

            // Tạo một bảng mới để lưu trữ dữ liệu đã sửa đổi
            DataTable modifiedData = new DataTable();
            modifiedData.Columns.Add("Month", typeof(string));
            modifiedData.Columns.Add("TotalPrice", typeof(double));

            // Đối chiếu dữ liệu ngày thành tháng và thêm vào bảng đã sửa đổi
            for (int i = 1; i <= 12; i++)
            {
                string monthName = System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
                double totalPrice = 0.0;

                // Tìm tổng doanh thu cho tháng này nếu có dữ liệu
                DataRow[] monthData = yearlyData.Select($"Month = {i}");
                if (monthData.Length > 0)
                {
                    totalPrice = Convert.ToDouble(monthData[0]["TotalPrice"]);
                }

                modifiedData.Rows.Add(monthName, totalPrice);
            }

            // Thiết lập dữ liệu cho biểu đồ
            chart1.DataSource = modifiedData;
            chart1.Series["Tiền"].XValueMember = "Month";
            chart1.Series["Tiền"].YValueMembers = "TotalPrice";

            // Thiết lập tiêu đề của biểu đồ
            chart1.ChartAreas["ChartArea1"].AxisX.Title = $"Doanh thu của năm {selectedYear}";
        }




        private void btnExcel_Click(object sender, EventArgs e)
        {
            int selectedYear = int.Parse(cmbYear.SelectedItem.ToString());
            int selectedMonth = cmbMonth.SelectedIndex + 1;

            DataTable dt = ReportDAO.Instance.ExportToExcel(selectedYear, selectedMonth);
            Export(dt);
        }

        public void Export(DataTable dt)
        {
            // Các dòng code xuất Excel giữ nguyên
            Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbooks oBooks;
            Microsoft.Office.Interop.Excel.Sheets oSheets;
            Microsoft.Office.Interop.Excel.Workbook oBook;
            Microsoft.Office.Interop.Excel.Worksheet oSheet;
            oExcel.Visible = true;
            oExcel.DisplayAlerts = false;
            oExcel.Application.SheetsInNewWorkbook = 1;
            oBooks = oExcel.Workbooks;
            oBook = (Microsoft.Office.Interop.Excel.Workbook)(oExcel.Workbooks.Add(Type.Missing));
            oSheets = oBook.Worksheets;
            oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oSheets.get_Item(1);
            oSheet.Name = "Danh Sách";
            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "C1");
            head.MergeCells = true;
            head.Value2 = "Thống Kê Doanh Số Tháng " + cmbMonth.SelectedItem.ToString() + " Năm " + cmbYear.SelectedItem.ToString();
            head.Font.Bold = true;
            head.Font.Name = "Tahoma";
            head.Font.Size = "18";
            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            Microsoft.Office.Interop.Excel.Range cl1 = oSheet.get_Range("A3", "A3");
            cl1.Value2 = "Ngày";
            cl1.ColumnWidth = 13.5;
            Microsoft.Office.Interop.Excel.Range cl2 = oSheet.get_Range("B3", "B3");
            cl2.Value2 = "Số Tiền";
            cl2.ColumnWidth = 25.0;
            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "C3");
            rowHead.Font.Bold = true;
            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
            rowHead.Interior.ColorIndex = 15;
            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            object[,] arr = new object[dt.Rows.Count, dt.Columns.Count];
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                DataRow dr = dt.Rows[r];
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    arr[r, c] = dr[c];
                }
            }
            int rowStart = 4;
            int columnStart = 1;
            int rowEnd = rowStart + dt.Rows.Count - 1;
            int columnEnd = dt.Columns.Count;
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowStart, columnStart];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[rowEnd, columnEnd];
            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            range.Value2 = arr;
            range.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;
        }

        // Các sự kiện SelectedIndexChanged cho combobox cmbYear và cmbMonth
        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowMonthlyRevenue();
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowMonthlyRevenue();
        }


    }
}

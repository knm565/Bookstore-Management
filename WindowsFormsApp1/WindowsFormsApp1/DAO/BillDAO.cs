using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp1.DAO
{
    class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return instance; }
            private set { instance = value; }
        }

        private BillDAO() { }

        public DataTable SelectBill(string dateFilter, string employeeFilter, string clientNameFilter)
        {
            string query = "SELECT HoaDon.IDHoaDon, HoaDon.NgayInHoaDon, HoaDon.TongTien, TaiKhoan.TenTaiKhoan AS NhanVien, KhachHang.TenKhachHang " +
                           "FROM HoaDon " +
                           "LEFT JOIN TaiKhoan ON HoaDon.IDTaiKhoan = TaiKhoan.IDTaiKhoan " +
                           "LEFT JOIN KhachHang ON HoaDon.IDKhachHang = KhachHang.IDKhachHang " +
                           "WHERE 1 = 1";

            if (!string.IsNullOrEmpty(dateFilter))
            {
                query += $" AND CONVERT(varchar, HoaDon.NgayInHoaDon, 103) = '{dateFilter}'";
            }

            if (!string.IsNullOrEmpty(employeeFilter))
            {
                query += $" AND TaiKhoan.TenTaiKhoan = '{employeeFilter}'";
            }

            if (!string.IsNullOrEmpty(clientNameFilter))
            {
                query += $" AND KhachHang.TenKhachHang = '{clientNameFilter}'";
            }

            return DataProvider.Instance.ExecuteQuery(query);
        }

        public DataTable GetBillDetails(int billID)
        {
            string query = "SELECT s.TenSach, SUM(c.SoLuong) AS SoLuong, MAX(s.Gia) AS DonGia " +
                           "FROM ChiTietHoaDon c " +
                           "INNER JOIN Sach s ON c.IDSach = s.IDSach " +
                           $"WHERE c.IDHoaDon = {billID} GROUP BY s.TenSach";

            return DataProvider.Instance.ExecuteQuery(query);
        }

        public string GetDiscountLevel(int billID)
        {
            string query = "SELECT IDKhachHang FROM HoaDon WHERE IDHoaDon = " + billID;
            var ID = DataProvider.Instance.ExecuteQuery(query);

            if (ID.Rows.Count > 0 && ID.Rows[0]["IDKhachHang"] != DBNull.Value)
            {
                int idclient = Convert.ToInt32(ID.Rows[0]["IDKhachHang"].ToString());

                string query1 = "select CapBac from KhachHang where IDKhachHang =" + idclient;
                var cap = DataProvider.Instance.ExecuteQuery(query1);

                string capKhachHang = cap.Rows[0]["CapBac"].ToString(); // Đây là hàm tạo để lấy cấp độ của khách hàng

                switch (capKhachHang)
                {
                    case "Thành viên":
                        return "5%";
                    case "Thành viên Thân Thiết":
                        return "10%";
                    case "VIP":
                        return "15%";
                    default:
                        return "";
                }
            }

            return "";
        }

        public int GetPrice(int idBill)
        {
            string query = "SELECT IDHoaDon, SUM(c.SoLuong * s.Gia) AS ThanhTien " +
                           "FROM ChiTietHoaDon c " +
                           "INNER JOIN Sach s ON c.IDSach = s.IDSach " +
                           $"WHERE c.IDHoaDon = {idBill} GROUP BY IDHoaDon";
            var price = DataProvider.Instance.ExecuteQuery(query);

            if (price.Rows.Count > 0)
            {
                return int.Parse(price.Rows[0]["ThanhTien"].ToString());
            }

            // Trả về một giá trị mặc định nếu không có dữ liệu hoặc có lỗi.
            return 0;
        }








    }
}

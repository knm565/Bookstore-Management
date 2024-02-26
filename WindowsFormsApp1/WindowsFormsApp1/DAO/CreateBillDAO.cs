using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Dialog;

namespace WindowsFormsApp1.DAO
{
    class CreateBillDAO
    {
        private static CreateBillDAO instance;

        public static CreateBillDAO Instance
        {
            get { if (instance == null) instance = new CreateBillDAO(); return instance; }
            private set { instance = value; }
        }

        private CreateBillDAO() { }

        public object selectCreateBill()
        {
            string query = "select Sach.IDSach as 'ID', Sach.TenSach, Sach.Gia as N'Giá', Sach.Soluong as 'Số lượng tồn' from Sach where visible=1";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public object getKhach()
        {
            string query = "select * from KhachHang where visible=1";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public void createBill(string date, string time, int idStaff)
        {
            string query = "Insert into HoaDon(NgayInHoaDon,GioInHoaDon,IDTaiKhoan) values('" + date + "','" + time + "'," + idStaff + ")";
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public int getIdBill()
        {
            string query = " select Top(1) IDHoaDon from HoaDon order by IDHoaDon DESC";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            int id = int.Parse(result.Rows[0].ItemArray[0].ToString());
            return id;
        }

        public object selectInfoBill(int idBill)
        {
            string query = " select Sach.TenSach as N'Tên Sách', SUM(ChiTietHoaDon.SoLuong) as N'Số Lượng' from ChiTietHoaDon, Sach, HoaDon where HoaDon.IDHoaDon = ChiTietHoaDon.IDHoaDon and ChiTietHoaDon.IDSach = Sach.IDSach and ChiTietHoaDon.IDHoaDon = " + idBill + "Group by Sach.TenSach";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public void addBook(int bill, int idBook)
        {
            string query = "insert into ChiTietHoaDon(IDSach,IDHoaDon,SoLuong) values(" + idBook + "," + bill + ",1)";
            DataProvider.Instance.ExecuteNonQuery(query);
            string query1 = "update Sach set SoLuong = SoLuong-1 where IDSach = "+idBook;
            DataProvider.Instance.ExecuteNonQuery(query1);
        }

        public void deleteBook(int idBill, string Book)
        {
            string selectId = "select IDSach from Sach where TenSach = N'" + Book + "'";
            DataTable result = DataProvider.Instance.ExecuteQuery(selectId);

            if (result.Rows.Count > 0)
            {
                int id = int.Parse(result.Rows[0].ItemArray[0].ToString());

                // Lấy số lượng sách từ bảng ChiTietHoaDon
                string selectSL = "select SUM(SoLuong), IDSach from ChiTietHoaDon where IDSach = " + id + " and IDHoaDon = " + idBill+ "Group by IDSach";
                DataTable result1 = DataProvider.Instance.ExecuteQuery(selectSL);

                if (result1.Rows.Count > 0)
                {
                    int sl = int.Parse(result1.Rows[0].ItemArray[0].ToString());

                    // Xóa cuốn sách khỏi hóa đơn
                    string query = "delete from ChiTietHoaDon where IDHoaDon = " + idBill + " and IDSach = " + id;
                    DataProvider.Instance.ExecuteNonQuery(query);

                    // Cập nhật lại số lượng sách trong bảng Sach
                    string query1 = "update Sach set SoLuong = SoLuong + " + sl + " where IDSach = " + id;
                    DataProvider.Instance.ExecuteNonQuery(query1);
                }
            }
        }


        public int getMoney(int idBill)
        {
            string query = " select SUM(Sach.Gia) from Sach, ChiTietHoaDon where ChiTietHoaDon.IDHoaDon = " + idBill + " and ChiTietHoaDon.IDSach = Sach.IDSach and Sach.Gia > 0";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            int money;
            try
            {
                money = int.Parse(result.Rows[0].ItemArray[0].ToString());
            }
            catch (Exception)
            {
                money = 0;
            }
            return money;
        }

        public void updateBill(string nameClient, int idBill, int money)
        {
            if (nameClient == "")
            {
                string query = "update HoaDon set TongTien=" + money + " where IDHoaDon=" + idBill;
                DataProvider.Instance.ExecuteNonQuery(query);
            }
            else
            {
                string selectId = "select IDKhachHang from KhachHang where TenKhachHang = N'" + nameClient + "'";
                DataTable result = DataProvider.Instance.ExecuteQuery(selectId);
                int id = int.Parse(result.Rows[0].ItemArray[0].ToString());

                string query = "update HoaDon set TongTien=" + money + ", IDKhachHang=" + id + " where IDHoaDon=" + idBill;
                DataProvider.Instance.ExecuteNonQuery(query);

                string upMeneyKhach = "update KhachHang set SoTienDaMua=SoTienDaMua + " + money + " where IDKhachHang=" + id;
                DataProvider.Instance.ExecuteNonQuery(upMeneyKhach);

                //string selectLevel = "select CapBac from KhachHang where TenKhachHang = N'" + nameClient + "'";
                //DataTable resultLevel = DataProvider.Instance.ExecuteQuery(selectId);
                //string level = result.Rows[0].ItemArray[0].ToString();


                //Check vào update cấp bậc
                string selectToTalMoner = " select Sum(TongTien) from HoaDon where IDKhachHang = " + id + " and TongTien > 0";
                DataTable resultToTalMoner = DataProvider.Instance.ExecuteQuery(selectToTalMoner);
                int ToTalMoner = int.Parse(resultToTalMoner.Rows[0].ItemArray[0].ToString());
                Console.WriteLine(ToTalMoner.ToString());
                if (ToTalMoner < 1000000)
                {
                    string update = "update KhachHang set CapBac='Thành Viên' where IDKhachHang=" + id;
                    DataProvider.Instance.ExecuteNonQuery(update);
                }
                else if (ToTalMoner >= 1000000 && ToTalMoner < 5000000)
                {
                    string update = "update KhachHang set CapBac='Thành Viên Thân Thiết' where IDKhachHang=" + id;
                    DataProvider.Instance.ExecuteNonQuery(update);
                }    
                else if(ToTalMoner >= 5000000)
                {
                    string update = "update KhachHang set CapBac='VIP' where IDKhachHang=" + id;
                    DataProvider.Instance.ExecuteNonQuery(update);
                }
            }
        }

        public void updateKho(int idBill)
        {
            string query1 = " select ChiTietHoaDon.IDSach, SUM(ChiTietHoaDon.SoLuong) from ChiTietHoaDon where ChiTietHoaDon.IDHoaDon = " + idBill + " group by ChiTietHoaDon.IDSach";
            DataTable result = DataProvider.Instance.ExecuteQuery(query1);
            int len = result.Rows.Count;
            for (int i = 0; i < len; i++)
            {
                string query2 = "update Sach set SoLuong = SoLuong+" + int.Parse(result.Rows[i].ItemArray[1].ToString()) + " where IDSach=" + int.Parse(result.Rows[i].ItemArray[0].ToString());
                DataProvider.Instance.ExecuteNonQuery(query2);
            }
        }
        public string FindCustomerInfoByPhoneNumber(string phoneNumber)
        {
            string query = "SELECT TenKhachHang, CapBac FROM KhachHang WHERE SDT = " + phoneNumber;
            DataTable result = DataProvider.Instance.ExecuteQuery(query);

            if (result != null && result.Rows.Count > 0)
            {
                // Lấy dữ liệu từ dòng đầu tiên của DataTable
                string tenKhachHang = result.Rows[0]["TenKhachHang"].ToString();
                string capBac = result.Rows[0]["CapBac"].ToString();

                return tenKhachHang + "|" + capBac;
            }
            else
            {
                // Trả về null hoặc một giá trị khác để chỉ ra rằng không tìm thấy khách hàng
                return null;
            }
        }
        public void deleteBill(int idBill)
        {
            //string query2 = " select ChiTietHoaDon.IDSach, SUM(ChiTietHoaDon.SoLuong) from ChiTietHoaDon where ChiTietHoaDon.IDHoaDon = " + idBill + " group by ChiTietHoaDon.IDSach";
            //DataTable result = DataProvider.Instance.ExecuteQuery(query2);
            updateKho(idBill);
            string query = "DELETE FROM Chitiethoadon WHERE IDHoaDon = " + idBill;
            string query1 = "Delete from HoaDon where IDHoaDon =" + idBill;
            DataProvider.Instance.ExecuteNonQuery(query);
            DataProvider.Instance.ExecuteNonQuery(query1);
        }
        public bool checkTonKho(int idSach) {
            string query = "Select Soluong from Sach where IDSach =" + idSach;
            var check =  DataProvider.Instance.ExecuteQuery(query);
            int sl = int.Parse(check.Rows[0]["SoLuong"].ToString());
            if (sl == 0) {
                return false;
            }
            return true;
        }
    }
}

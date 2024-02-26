using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DAO
{
    class BookDAO
    {

        private static BookDAO instance;

        public static BookDAO Instance
        {
            get { if (instance == null) instance = new BookDAO(); return instance; }
            private set { instance = value; }
        }

        private BookDAO() { }

        public object selectBook()
        {
            string query = "select Sach.IDSach as 'ID', Sach.TenSach , Sach.NXB, Sach.TenTacGia as N'Tên Tác Giả', Sach.SoLuong as N'Số Lượng', Sach.Gia from Sach where visible=1";
            return DataProvider.Instance.ExecuteQuery(query);
        }


        public void add(string tenSach, string tenTacGia, string NXB, int soLuong,int Gia)
        {
            string query = "insert into Sach values(N'" + tenSach + "',N'" + NXB + "',N'" + tenTacGia + "'," + Gia + "," + soLuong + ",1)";
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public void delete(int id)
        {
            string query = "update Sach set visible=0 where IDSach=" + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
        
        public void update(string tenSach, string tenTacGia, string NXB, int soLuong,int id,int Gia)
        {
            string query = "update Sach set TenSach=N'" + tenSach + "', NXB=N'" + NXB + "', TenTacGia=N'" + tenTacGia + "', SoLuong=" + soLuong + ", Gia=" + Gia + " where IDSach=" + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
        public bool checkSach(string name, string author, string NXB)
        {
            string query = "SELECT * FROM Sach WHERE TenSach = N'" + name + "' and TenTacGia = N'" + author + "' and NXB = N'" + NXB + "';";
            var result = DataProvider.Instance.ExecuteQuery(query);
            if (result.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public void updateSL(string name, string author, string NXB, int sl)
        {
            string query1 = "select SoLuong from Sach where TenSach = N'" + name + "' and TenTacGia = N'" + author + "' and NXB = N'" + NXB + "';";
            var rsl = DataProvider.Instance.ExecuteQuery(query1);
            int soLuong = Convert.ToInt32(rsl.Rows[0]["SoLuong"]);
            soLuong += sl;
            string query = "update Sach set SoLuong=" + soLuong + " where TenSach = N'" + name + "' and TenTacGia = N'" + author + "' and NXB = N'" + NXB + "'";
            DataProvider.Instance.ExecuteNonQuery(query);
        }
    }
}

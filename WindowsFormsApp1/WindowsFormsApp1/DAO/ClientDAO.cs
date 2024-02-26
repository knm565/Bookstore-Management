using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DAO
{
    class ClientDAO
    {
        private static ClientDAO instance;

        public static ClientDAO Instance
        {
            get { if (instance == null) instance = new ClientDAO(); return instance; }
            private set { instance = value; }
        }

        private ClientDAO() { }

        public object selectClient()
        {
            string query = "select KhachHang.IDKhachHang as 'ID', KhachHang.TenKhachHang, KhachHang.Email, KhachHang.SDT, KhachHang.CapBac as N'Cấp Bậc', KhachHang.SoTienDaMua as N'Số Tiền Đã Mua' from KhachHang where visible=1";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public void addClient(string name, string emal, int sdt)
        {
            string query = "insert into KhachHang(TenKhachHang,SDT,Email,SoTienDaMua,visible) values(N'" + name + "','" + sdt + "','" + emal + "',0,1)";
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public void updateClient(int id, string name, string emal, int sdt)
        {
            string query = "update KhachHang set TenKhachHang=N'" + name + "', Email=N'" + emal + "', SDT='" + sdt + "' where IDKhachHang=" + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
        public void delete(int id)
        {
            string query = "update KhachHang set visible=0 where IDKhachHang=" + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
        public bool CheckClient(int sdt)
        {
            string query = "SELECT * FROM KhachHang WHERE SDT = "+sdt+";";
            var result = DataProvider.Instance.ExecuteQuery(query);
            if(result.Rows.Count > 0) 
                return true;
            else
                return false;           
        }
    }
}

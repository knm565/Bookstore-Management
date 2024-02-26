using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.DAO
{
    class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }

        public object selectAccount()
        {
            string query = "Select IDTaiKhoan,TenTaiKhoan,MatKhau,Email,SDT from TaiKhoan where visible=1";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public void add(string name, string pass, string email, int sdt)
        {
            string query = "insert into TaiKhoan(TenTaiKhoan,MatKhau,Email,SDT,visible) values(N'" + name + "','" + pass + "','" + email + "'," + sdt + ",2,1)";
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public void delete(int id)
        {
            string query = "update TaiKhoan set visible=0 where IDTaiKhoan=" + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public void update(string name, string pass, string email, int sdt,int id)
        {
            string query = "update TaiKhoan set TenTaiKhoan=N'" + name + "', MatKhau=N'" + pass + "', Email='" + email + "', SDT='" + sdt + "' where IDTaiKhoan=" + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
    }
}

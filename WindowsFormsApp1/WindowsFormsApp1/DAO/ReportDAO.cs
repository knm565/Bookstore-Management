using System;
using System.Data;
using System.Globalization;

namespace WindowsFormsApp1.DAO
{
    class ReportDAO
    {
        private static ReportDAO instance;

        public static ReportDAO Instance
        {
            get { if (instance == null) instance = new ReportDAO(); return instance; }
            private set { instance = value; }
        }

        private ReportDAO() { }

        public DataTable SeeMonthlyRevenue(int year, int month)
        {
            string query = $"SELECT HoaDon.NgayInHoaDon AS 'DateBill', Sum(HoaDon.TongTien) AS 'TotalPrice' FROM HoaDon WHERE YEAR(HoaDon.NgayInHoaDon) = {year} AND MONTH(HoaDon.NgayInHoaDon) = {month} GROUP BY HoaDon.NgayInHoaDon";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            return result;
        }

        /*public DataTable SeeYearlyRevenue(int year)
        {
            string query = $"SELECT MONTH(HoaDon.NgayInHoaDon) AS 'Month', Sum(HoaDon.TongTien) AS 'TotalPrice' FROM HoaDon WHERE YEAR(HoaDon.NgayInHoaDon) = {year} GROUP BY MONTH(HoaDon.NgayInHoaDon)";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            return result;
        }*/
        public DataTable SeeYearlyRevenue(int year)
        {
            string query = $"SELECT MONTH(HoaDon.NgayInHoaDon) AS 'Month', Sum(HoaDon.TongTien) AS 'TotalPrice' FROM HoaDon WHERE YEAR(HoaDon.NgayInHoaDon) = {year} GROUP BY MONTH(HoaDon.NgayInHoaDon)";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            return result;
        }


        public DataTable ExportToExcel(int year, int month)
        {
            string formattedMonth = month.ToString("00"); // Định dạng tháng thành "MM"
            string query = $"SELECT CONVERT(varchar, HoaDon.NgayInHoaDon, 103) AS 'DateBill', Sum(HoaDon.TongTien) AS 'TotalPrice' FROM HoaDon WHERE YEAR(HoaDon.NgayInHoaDon) = {year} AND MONTH(HoaDon.NgayInHoaDon) = {month} GROUP BY CONVERT(varchar, HoaDon.NgayInHoaDon, 103)";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            return result;
        }
    }
}

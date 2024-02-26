using System;
using System.Data;
using System.Data.SqlClient;

namespace WindowsFormsApp1.DAO
{
    class DataProvider
    {
        private static DataProvider instance;

        public static DataProvider Instance
        {
            get { if (instance == null) instance = new DataProvider(); return DataProvider.instance; }
            private set { DataProvider.instance = value; }
        }

        private DataProvider() { }

        private string connStr = "Data Source=MSI\\SQLEXPRESS01;Initial Catalog=QuanLyNhaSach;Integrated Security=True";

        public DataTable ExecuteQuery(string query)
        {
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();

            SqlCommand command = new SqlCommand(query, conn);

            DataTable data = new DataTable();

            SqlDataAdapter adapter = new SqlDataAdapter(command);

            adapter.Fill(data);

            conn.Close();

            return data;
        }

        public void ExecuteNonQuery(string query)
        {
            SqlConnection conn = new SqlConnection(connStr);

            conn.Open();

            SqlCommand command = new SqlCommand(query, conn);

            command.ExecuteNonQuery();

            conn.Close();
        }
    }
}

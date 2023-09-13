using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RFC_shippingHistory.lib
{
    internal class DB
    {
        // 取得 Connection
        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mssql_shipppingHistory"].ConnectionString);
                return con;
            }
            catch (Exception ex)
            {
                throw new Exception("無法連接資料庫[" + ex.Message + "]");
            }
        }

        // 執行沒有參數的 insert、update、delete 子句
        public static string ExecuteNoParams(string sql) 
        {
            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                int rowAffected = cmd.ExecuteNonQuery();
                if (rowAffected == 0)
                {
                    return "執行失敗";
                }
                else
                {
                    return $"執行成功，{rowAffected}筆資料受影響!";
                }
            }
        }

        // 取得 DataTable
        public static DataTable GetDataTable(string selectSQL)
        {
            SqlConnection con = GetConnection();
            con.Open();

            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectSQL, con);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            con.Close();
            con.Dispose();

            return dataSet.Tables[0];
        }
    }
}

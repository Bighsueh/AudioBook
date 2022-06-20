using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;

namespace AudioBook.Models
{
    public class DBTablePages
    {
        private readonly string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["MSSQL_DBconnect"].ConnectionString;

        public List<table_pages> GetTable(int book_id)
        {
            List<table_pages> pages = new List<table_pages>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);

            SqlCommand sqlCommand = new SqlCommand(String.Format("Select * from pages where book_id = {0}", book_id.ToString()));
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    table_pages page = new table_pages
                    {
                        page_id = reader.GetInt32(reader.GetOrdinal("page_id")),
                        page_num = reader.GetInt32(reader.GetOrdinal("page_num")),
                        image_path = reader.GetString(reader.GetOrdinal("image_path")),
                        page_title = reader.GetString(reader.GetOrdinal("page_title")),
                        page_content = reader.GetString(reader.GetOrdinal("page_content")),
                    };
                    pages.Add(page);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空!");
            }
            sqlConnection.Close();
            return pages;
        }

        public string InsertTable(int book_id,string image_path="")
        {
            string page_num = "";
            string page_title = "";
            string page_content = "";

            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format(
                "INSERT INTO pages(page_num,book_id,image_path,page_title,page_content)VALUES('{0}','{1}','{2}','{3}','{4}')"
                , page_num, book_id, image_path, page_title, page_content);

            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            sqlConnection.Close();

            return "success";
        }
    }
}
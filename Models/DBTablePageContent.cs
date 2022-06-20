using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AudioBook.Models
{
    public class DBTablePageContent
    {
        private readonly string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["MSSQL_DBconnect"]
            .ConnectionString;

        public List<table_page_content> GetTable(int content_id = 0)
        {
            List<table_page_content> page_contents = new List<table_page_content>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);

            String sql_query = "Select * from page_content";
            if (content_id != 0)
            {
                sql_query = String.Format("Select * from page_content where content_id = {0}", content_id.ToString());
            }

            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    table_page_content file = new table_page_content
                    {
                        content_id = reader.GetInt32(reader.GetOrdinal("content_id")),
                        content_name = reader.GetString(reader.GetOrdinal("content_name")),
                        content_type = reader.GetString(reader.GetOrdinal("content_type")),
                        content_code = reader.GetGuid(reader.GetOrdinal("content_code")),
                    };
                    page_contents.Add(file);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空!");
            }

            sqlConnection.Close();
            return page_contents;
        }

        public int InsertTable(string content_name = "", string content_type = "")
        {
            List<table_page_content> page_contents = new List<table_page_content>();
            String str_guid = Guid.NewGuid().ToString("D");

            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = "";
            sql_query =
                String.Format(
                    "INSERT INTO page_content(content_name,content_type,content_code)VALUES('{0}','{1}','{2}')",
                    content_name, content_type, str_guid);


            // 將資料插入
            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;

            sqlConnection.Open();
            sqlCommand.ExecuteReader();
            sqlConnection.Close();

            // 取得最新一筆資料(這樣就會拿到剛剛插入的資料)

            sql_query = "SELECT TOP 1 * FROM page_content ORDER BY content_id DESC";
            sqlCommand.CommandText = sql_query;

            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    table_page_content file = new table_page_content
                    {
                        content_id = reader.GetInt32(reader.GetOrdinal("content_id")),
                        content_name = reader.GetString(reader.GetOrdinal("content_name")),
                        content_code = reader.GetGuid(reader.GetOrdinal("content_code")),
                    };
                    page_contents.Add(file);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空!");
            }

            sqlConnection.Close();
            return page_contents[0].content_id;
        }

        public string UpdateTable(int content_id, string content_name = "", string content_type = "")
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format(
                "UPDATE page_content SET content_name = '{0}', content_type = '{1}'WHERE content_id = {2}",
                content_name, content_type, content_id);
            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            sqlConnection.Close();

            return "Update_Success";
        }
        public string DeleteRow(int content_id)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format("DELETE FROM page_content WHERE content_id = {0}", content_id);

            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            sqlConnection.Close();

            return "Delete_Success";
        }
    }
}

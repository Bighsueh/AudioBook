using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AudioBook.Models
{
    public class DBTableFiles
    {
        private readonly string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["MSSQL_DBconnect"].ConnectionString;

        public List<table_files> GetTable(int file_id = 0)
        {
            List<table_files> files = new List<table_files>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);

            String sql_query = "Select * from files";
            if (file_id != 0)
            {
                sql_query = String.Format("Select * from files where file_id = {0}", file_id.ToString());
            }
            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    table_files file = new table_files
                    {
                        file_id = reader.GetInt32(reader.GetOrdinal("file_id")),
                        content_id = reader.GetInt32(reader.GetOrdinal("content_id")),
                        local_path = reader.GetString(reader.GetOrdinal("local_path")),
                        file_title = reader.GetString(reader.GetOrdinal("file_title")),
                        file_type = reader.GetString(reader.GetOrdinal("file_type")),
                        file_code = reader.GetGuid(reader.GetOrdinal("file_code")),
                    };
                    files.Add(file);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空!");
            }
            sqlConnection.Close();
            return files;
        }

        // Guid file_code = new Guid()
        public string InsertTable(int content_id = 0, string local_path = "", string file_title = "", string file_type = "")
        {
            String str_guid = Guid.NewGuid().ToString("D");

            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format("INSERT INTO files(content_id,local_path,file_title,file_type,file_code)VALUES('{0}','{1}','{2}','{3}','{4}')", content_id, local_path, file_title, file_type, str_guid);


            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            sqlConnection.Close();

            return sql_query;
        }

        public string UpdateTable(int content_id, string file_title = "", string file_type = "", string local_path = "")
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format(
                        "UPDATE files SET file_title = '{0}', file_type = '{1}',local_path = '{2}' WHERE content_id = '{3}'",
                        file_title, file_type, local_path, content_id);
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
            String sql_query = String.Format("DELETE FROM files WHERE content_id = {0}", content_id);

            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            sqlConnection.Close();

            return "Delete_Success";
        }
    }
}
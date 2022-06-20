using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace AudioBook.Models
{
    public class DBTableBooks
    {
        private readonly string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["MSSQL_DBconnect"].ConnectionString;

        public List<table_books> GetTable(int book_id = 0)
        {
            List<table_books> audio_books = new List<table_books>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);

            String sql_query = "Select * from books";
            if (book_id != 0)
            {
                sql_query = String.Format("Select * from books where book_id = {0}", book_id.ToString());
            }
            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    table_books audio_book = new table_books
                    {
                        book_id = reader.GetInt32(reader.GetOrdinal("book_id")),
                        book_title = reader.GetString(reader.GetOrdinal("book_title")),
                        book_content = reader.GetString(reader.GetOrdinal("book_content")),
                        front_cover = reader.GetString(reader.GetOrdinal("front_cover")),
                    };
                    audio_books.Add(audio_book);
                }
            }
            else
            {
                Console.WriteLine("資料庫為空!");
            }
            sqlConnection.Close();
            return audio_books;
        }

        public string InsertTable(string book_title = "", string book_content = "", string image_path = "")
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format("INSERT INTO books(book_title,book_content,front_cover)VALUES('{0}','{1}','{2}')", book_title, book_content, image_path);

            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            sqlConnection.Close();

            return sql_query;
        }

        public string UpdateTable(int book_id, string book_title = "", string book_content = "", string image_path = "")
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format(
                        "UPDATE books SET book_title = '{0}', book_content = '{1}', front_cover = '{2}' WHERE book_id = '{3}'",
                        book_title, book_content, image_path, book_id);
            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            sqlConnection.Close();

            return "Update_Success";
        }

        public string DeleteRow(int book_id)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format("DELETE FROM books WHERE book_id = {0}", book_id);
            
            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            sqlConnection.Close();

            return "Delete_Success";
        }
    }
}
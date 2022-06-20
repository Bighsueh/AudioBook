using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace AudioBook.Models
{
    public class DBTableUser
    {
        private readonly string ConnStr = System.Configuration.ConfigurationManager.ConnectionStrings["MSSQL_DBconnect"]
            .ConnectionString;

        public Boolean AdminCheck(string account, string password)
        {
            String sql_query = String.Format("Select * from system_users where user_account = '{0}' and user_password = '{1}'",account,password);
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            Boolean account_exist = false;
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(reader.GetOrdinal("system_user_id")) != 0)
                    {
                        account_exist = true;
                    };
                    
                }
            }
            else
            {
                Console.WriteLine("資料庫為空!");
            }

            sqlConnection.Close();
            return account_exist;
        }
    }
}

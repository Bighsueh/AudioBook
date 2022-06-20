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
            String sql_query =
                String.Format("Select * from system_users where user_account = '{0}' and user_password = '{1}'",
                    account, password);
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
                    }

                    ;
                }
            }
            else
            {
                Console.WriteLine("資料庫為空!");
            }

            sqlConnection.Close();
            return account_exist;
        }

        //列出群組列表
        public List<list_user_group> ListUserGroups()
        {
            String sql_query;
            List<list_user_group> list_user_groups = new List<list_user_group>();

            //先將廠商-管理員加入群組
            list_user_group admin_group = new list_user_group
            {
                group_id = 0,
                group_name = "admin",
                group_content = "管理員成員群組",
            };
            list_user_groups.Add(admin_group);
            
            sql_query = String.Format("Select * from schools");
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list_user_group list_user_group = new list_user_group
                    {
                        group_id = reader.GetInt32(reader.GetOrdinal("school_id")),
                        group_name = reader.GetString(reader.GetOrdinal("school_name")),
                        group_content = reader.GetString(reader.GetOrdinal("school_content")),
                    };
                    list_user_groups.Add(list_user_group);
                }
            }

            sqlConnection.Close();
            return list_user_groups;
        }

        //列出某群組中的使用者列表
        public List<list_users> ListUsers(int group_id, string group_type)
        {
            String sql_query;
            List<list_users> list_users = new List<list_users>();

            //若群組類型為admin
            if (group_type == "admin")
            {
                sql_query = String.Format("Select * from system_users");
                SqlConnection sqlConnection = new SqlConnection(ConnStr);
                SqlCommand sqlCommand = new SqlCommand(sql_query);
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list_users list_user = new list_users
                        {
                            user_id = reader.GetInt32(reader.GetOrdinal("system_user_id")),
                            user_name = reader.GetString(reader.GetOrdinal("user_name")),
                            user_account = reader.GetString(reader.GetOrdinal("user_account")),
                            user_password = reader.GetString(reader.GetOrdinal("user_password")),
                        };
                        list_users.Add(list_user);
                    }
                }

                sqlConnection.Close();
                sql_query = String.Format("Select * from system_users");
                //sqlCommand.CommandText = sql_query
                return list_users;
            }

            //若群組類行為學校單位
            if (group_type != "admin")
            {
                sql_query = String.Format("Select * from school_users where school_id = {0}", group_id);
                SqlConnection sqlConnection = new SqlConnection(ConnStr);
                SqlCommand sqlCommand = new SqlCommand(sql_query);
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();

                SqlDataReader reader = sqlCommand.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list_users list_user = new list_users
                        {
                            user_id = reader.GetInt32(reader.GetOrdinal("school_user_id")),
                            user_name = reader.GetString(reader.GetOrdinal("user_name")),
                            user_account = reader.GetString(reader.GetOrdinal("user_account")),
                            user_password = reader.GetString(reader.GetOrdinal("user_password")),
                        };
                        list_users.Add(list_user);
                    }
                }

                sqlConnection.Close();
                return list_users;
            }

            return list_users;
        }

        //新增學校角色
        public string CreateGroup(string group_name, string group_content)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format("INSERT INTO schools(school_name,school_content)VALUES('{0}','{1}')", group_name, group_content);

            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            sqlConnection.Close();

            return "success";
        }
        
        //取得欲修改資料Group
        public List<list_user_group> GetGroupInfo(int group_id)
        {
            List<list_user_group> list_user_groups = new List<list_user_group>();
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format("SELECT * FROM schools WHERE school_id={0}",group_id);

            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    list_user_group list_user_group = new list_user_group
                    {
                        group_id = reader.GetInt32(reader.GetOrdinal("school_id")),
                        group_name = reader.GetString(reader.GetOrdinal("school_name")),
                        group_content = reader.GetString(reader.GetOrdinal("school_content")),
                    };
                    list_user_groups.Add(list_user_group);
                }
            }

            sqlConnection.Close();

            return list_user_groups;
        }
        
        public string UpdateGroupInfo(int group_id, string group_name = "", string group_content = "")
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format(
                "UPDATE schools SET school_name = '{0}', school_content = '{1}'WHERE school_id = '{2}'",
                group_name, group_content, group_id);
            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            sqlConnection.Close();

            return "Update_Success";
        }
        
        public string DeleteRow(int group_id)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnStr);
            String sql_query = String.Format("DELETE FROM schools WHERE school_id = {0}", group_id);
            
            SqlCommand sqlCommand = new SqlCommand(sql_query);
            sqlCommand.Connection = sqlConnection;
            sqlConnection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            sqlConnection.Close();

            return "Delete_Success";
        }
    }
}

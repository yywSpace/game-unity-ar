using System;
using System.IO;
using UnityEngine;

namespace Script.MainPage.UserManagement
{
    using MySql.Data;
    using MySql.Data.MySqlClient;
    using System.Data;
    public static class UserLab
    {
        private const string ConnectionString =
            "datasource=47.96.152.133;port=3306;database=game_learn_ar;user=argame;pwd=1095204049;Charset=utf8;";
        public static User CurrentUser { get; set; }

        public static User GetUser(string account)
        {
            try
            {
                User user = null;
                MySqlConnection connection = Connect();
                string sql = "SELECT * FROM user WHERE user_account=@account";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("account", "1095204049");
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32("user_id"),
                        Account = reader.GetString("user_account"),
                        UserName = reader.GetString("user_name"),
                        Password = reader.GetString("user_password"),
                        Sex = reader.IsDBNull(5)?"":reader.GetString("user_sex"),
                        City = reader.IsDBNull(6)?"":reader.GetString("user_city"),
                        MaxHp = reader.GetInt32("user_max_hp"),
                        Hp = reader.GetInt32("user_hp"),
                        Level = reader.GetInt32("user_level"),
                        Exp = reader.GetInt32("user_exp"),
                        Credits = reader.GetInt32("user_credits")
                    };
                    if (!reader.IsDBNull(3))
                    {
                        long len = reader.GetBytes(3, 0, null, 0, 0);
                        byte[] buffer = new byte[len];
                        reader.GetBytes(3, 0, buffer, 0, (int) len);
                        user.AvatarBytes = buffer;
                    }
                }
                reader.Close();
                if (user == null)
                    return null;
                sql = "SELECT " +
                      "     count(IF(task_type = '接取',true,NULL)) AS receive_number," +
                      "     count(IF(task_type = '发布',true,NULL)) AS release_number," +
                      "     count(IF(task_status = '已完成',true,NULL)) AS accomplish_number " +
                      "FROM task WHERE user_id = @userId";
                command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("userId", user.Id);
                reader = command.ExecuteReader();
                if (reader.Read())
                {
                    user.ReceiveTaskNum = reader.GetInt32("receive_number");
                    user.ReleaseTaskNum = reader.GetInt32("release_number");
                    user.AccomplishTaskNum = reader.GetInt32("accomplish_number");
                }

                CloseConnection(connection);
                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                Debug.LogError("打开数据库错误！" + e);
                return null;
            }
        }
        public static void CloseConnection(MySqlConnection conn)
        {
            if (conn != null)
                conn.Close();
            else
                Console.WriteLine("MySqlConnection不能为空");
        }
        
        private static void SaveImage()
        {
            MySqlConnection connection = UserLab.Connect();
            string sql = "UPDATE user SET user_avatar = @image where user_id = 4";
            string textPath = Application.dataPath + "/Image/avatar.jpg";
            FileStream fs = new FileStream(textPath, FileMode.Open);
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.Add("@image", MySqlDbType.Blob);
            byte[] fileData = new byte[fs.Length + 1];	
            fs.Read(fileData, 0, (int)fs.Length);       
            command.Parameters["@image"].Value = fileData;
            command.ExecuteNonQuery();
        }

        private static void ReadImage()
        {
            MySqlConnection connection = UserLab.Connect();
            string sql = "select user_avatar from user where user_id = 4";
            MySqlCommand command = new MySqlCommand(sql, connection);
            MySqlDataReader reader  =   command.ExecuteReader();
         
            if (reader.Read())
            {
                long len = reader.GetBytes(0, 0, null, 0, 0);
                byte[] buffer = new byte[len];
                reader.GetBytes(1, 0, buffer, 0, (int)len);
                FileStream fileStream = new FileStream(
                    Application.dataPath + "/Image/avatar_read.jpg", FileMode.Create, FileAccess.Write);
                fileStream.Write(buffer, 0, buffer.Length);
                fileStream.Close();
            }
        }
    }
}
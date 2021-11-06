using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SignalRChat.Models.Data
{
    public class User
    {
        public User()
        {
            Photo = new byte[0];
            Name = "";
        }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Sex { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public DateTime LastAccess { get; set; }
    }
    public class SEX
    {
        public static int Nu = 0;
        public static int Nam = 1;
    }
    public class UserFunc
    {
        public static (User, string) DangNhap(string username, string password)
        {
            User user = GetByUserName(username);
            if (user == null)
            {
                return (null, "Tài khoản hoặc mật khẩu không đúng !");
            }

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return (user, "");
            }
            else
            {
                return (null, "Tài khoản hoặc mật khẩu không đúng !");
            }
        }
        public static User GetByUserName(string Username)
        {
            User user = null;
            string query = "EXECUTE GetUserByUsername @Username ";
            var tb = Conn.ExecuteQuery(query, new object[] { Username});

            if(tb.Rows.Count <= 0)
            {
                return null;
            }
            else
            {
                var row = tb.Rows[0];
                var sb = row["Photo"].ToString();
                return new User
                {
                    Id = int.Parse(row["Id"].ToString()),
                    Username = row["Username"].ToString(),
                    Password = row["Password"].ToString(),
                    Name = row["Name"].ToString(),
                    Sex = int.Parse(row["Sex"].ToString()),
                    Photo = String.IsNullOrEmpty(sb) ? null : (byte[])row["Photo"],
                    LastAccess = DateTime.Parse(row["LastAccess"].ToString()),
                };
            }

        }
        public static (bool, string) Add(User user)
        {
            string cmd = "exec ThemUser @Username , @Password , @Name , @Sex , @Photo ";
            int res = Conn.ExecuteScalar(cmd, new object[] { user.Username, user.Password, user.Name, user.Sex, user.Photo});
            string msg;
            if(res > 0)
            {
                return (true, "");
            }
            else
            {
                return (false, "Tên đăng nhập đã tồn tại!");
            }
        }
        public static (bool,string) DangKy(string username, string password)
        {
            string msg ="";
            bool isSuccess = false;
            (isSuccess, msg) = Add(new User
            {
                Username = username,
                Password = password,
            });
            return (isSuccess, msg);
        }
        public static List<User> GetList()
        {
            string cmd = "select * from dbo.tblUser";
            DataTable dt = Conn.ExecuteQuery(cmd);
            var result = new List<User>();
            for(int i = 0; i<dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Photo"].ToString();
                result.Add(new User
                {
                    Id = int.Parse(row["Id"].ToString()),
                    Username = row["Username"].ToString(),
                    Password = row["Password"].ToString(),
                    Name = row["Username"].ToString(),
                    Photo = String.IsNullOrEmpty(tmp) ? new byte[0] : (byte[])row["Photo"],
                    LastAccess = DateTime.Parse(row["LastAccess"].ToString()),
                });
            }

            return result;
        }
    }

    
}
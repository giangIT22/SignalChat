using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SignalRChat.Models.Data
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }

        public DateTime CreationTime { get; set; }

        public List<User> Members { get; set; }
    }
    public class ROLE
    {
        public static int Member = 0;
        public static int Admin = 1;
    }

    public class GroupFunc
    {
        public static (bool, string) Add(User user)
        {
            string cmd = "exec ThemUser @Username , @Password , @Name , @Sex , @Photo ";
            int res = Conn.ExecuteScalar(cmd, new object[] { user.Username, user.Password, user.Name, user.Sex, user.Photo });
            string msg;
            if (res > 0)
            {
                return (true, "");
            }
            else
            {
                return (false, "Tên đăng nhập đã tồn tại!");
            }
        }
        public static List<User> GetList()
        {
            string cmd = "select * from dbo.tblUser";
            DataTable dt = Conn.ExecuteQuery(cmd);
            var result = new List<User>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Photo"].ToString();
                result.Add(new User
                {
                    Username = row["Username"].ToString(),
                    Password = row["Username"].ToString(),
                    Name = row["Username"].ToString(),
                    Photo = String.IsNullOrEmpty(tmp) ? new byte[0] : (byte[])row["Photo"],
                    LastAccess = DateTime.Parse(row["LastAccess"].ToString()),
                });
            }

            return result;
        }
    }

}
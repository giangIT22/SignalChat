using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SignalRChat.Models.Data
{
    public class Group
    {
        public Group()
        {
            Photo = new byte[0];
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }

        public DateTime CreationTime { get; set; }

        public int CountMembers { get; set; }
    }
    public class State
    {
        public static int Admin = 1;
        public static int Member = 2;
        public static int Request = 3;
        public static int Banned = 4;
        public static int None = 0;
    }

    public class GroupFunc
    {
        public static (bool, string) AddGroup(Group group, int ownerId)
        {
            string cmd = "exec AddGroup @SenderId , @Name , @Description , @Photo ";
            int res = Conn.ExecuteScalar(cmd, new object[] { ownerId, group.Name, group.Description, group.Photo });
            string msg;
            if (res == 2)
            {
                return (true, "");
            }
            else
            {
                return (false, "Đã xảy ra lỗi!");
            }
        }

        public static List<Group> GetList()
        {
            string cmd = "select * from AshChat.dbo.tblGroup";
            DataTable dt = Conn.ExecuteQuery(cmd);
            var result = new List<Group>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Photo"].ToString();
                result.Add(new Group
                {
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Photo = String.IsNullOrEmpty(tmp) ? new byte[0] : (byte[])row["Photo"],
                    CreationTime = DateTime.Parse(row["CreationTime"].ToString()),
                });
            }

            return result;
        }
        public static Group GetGroup(int GroupId, int SenderId)
        {
            string cmd = "exec GetGroupById @GroupId , @SenderId";
            DataTable dt = Conn.ExecuteQuery(cmd, new object[] { GroupId, SenderId });
            if(dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                var tmp = row["Photo"].ToString();
                return new Group
                {
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Photo = String.IsNullOrEmpty(tmp) ? new byte[0] : (byte[])row["Photo"],
                    CreationTime = DateTime.Parse(row["CreationTime"].ToString()),
                    CountMembers = int.Parse(row["CountMembers"].ToString())
                };
            }
            else
            {
                return null;
            }

        }
        public static (bool,string) DeleteGroup(int GroupId, int SenderId)
        {
            string cmd = "exec DeleteGroup @GroupId , @SenderId ";
            int res = Conn.ExecuteScalar(cmd, new object[] { GroupId, SenderId });
            string msg;
            if (res > 0)
            {
                return (true, "");
            }
            else
            {
                return (false, (res == -1 ? "Bạn không có quyền để thực hiện tác vụ này !" : "Lỗi không xác định !"));
            }
        }
        
        
    }

}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SignalRChat.Models.Data
{
    public class User_Group
    {
        public User_Group()
        {
            Photo = new byte[0];
        }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public int State { get; set; }
        public DateTime CreationTime { get; set; }

    }
    public class UserToGroup
    {
        public UserToGroup()
        {
            Photo = new byte[0];
        }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public int State { get; set; }
    }

    public class User_GroupFunc
    {
        public static List<User_Group> GetGroupsOfUser(int UserId)
        {
            string cmd = "exec GetGroupsOfUser @UserId";
            DataTable dt = Conn.ExecuteQuery(cmd, new object[] { UserId });
            var result = new List<User_Group>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Photo"].ToString();
                var time = row["CreationTime"].ToString();
                result.Add(new User_Group
                {
                    GroupId = int.Parse(row["Id"].ToString()),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Photo = String.IsNullOrEmpty(tmp) ? new byte[0] : (byte[])row["Photo"],
                    State = int.Parse(row["State"].ToString()),
                    CreationTime = String.IsNullOrEmpty(time) ? DateTime.Now : DateTime.Parse(row["CreationTime"].ToString()),
                });
            }

            return result;
        }
        public static List<User_Group> GetList(int UserId)
        {
            string cmd = "exec GetGroupsToUser @UserId";
            DataTable dt = Conn.ExecuteQuery(cmd, new object[] { UserId });
            var result = new List<User_Group>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Photo"].ToString();
                var time = row["CreationTime"].ToString();
                result.Add(new User_Group
                {
                    GroupId = int.Parse(row["GroupId"].ToString()),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Photo = String.IsNullOrEmpty(tmp) ? new byte[0] : (byte[])row["Photo"],
                    State = int.Parse(row["State"].ToString()),
                    CreationTime = String.IsNullOrEmpty(time) ? DateTime.Now : DateTime.Parse(row["CreationTime"].ToString()),
                });
            }

            return result;
        }
        public static List<UserToGroup> GetMembers(int UserId, int GroupId)
        {
            string cmd = "exec GetUsersToGroup @UserId , @GroupId";
            DataTable dt = Conn.ExecuteQuery(cmd, new object[] { UserId , GroupId });
            var result = new List<UserToGroup>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Photo"].ToString();
                result.Add(new UserToGroup
                {
                    GroupId = int.Parse(row["GroupId"].ToString()),
                    UserId = int.Parse(row["UserId"].ToString()),
                    Name = row["Name"].ToString(),
                    Username = row["Username"].ToString(),
                    Photo = String.IsNullOrEmpty(tmp) ? new byte[0] : (byte[])row["Photo"],
                    State = int.Parse(row["State"].ToString()),
                });
            }

            return result;
        }
        public static (bool, string) AddGroupRequest(int GroupId, int SenderId)
        {
            string cmd = "exec AddGroupRequest @GroupId , @UserId ";
            int res = Conn.ExecuteScalar(cmd, new object[] { GroupId, SenderId });
            string msg;
            if (res == 1)
            {
                return (true, "");
            }
            else
            {
                return (false, "Có lỗi xảy ra, số dòng được thêm " + res + " !");
            }
        }
        public static (bool, string) DeleteGroupRequest(int GroupId, int SenderId, int TargetUserId)
        {
            string cmd = "exec DeleteGroupRequest @GroupId , @SenderId , @TargetUserId ";
            int res = Conn.ExecuteScalar(cmd, new object[] { GroupId, SenderId , TargetUserId });
            string msg;
            if (res == 1)
            {
                return (true, "");
            }
            else
            {
                return (false, "Có lỗi xảy ra, số dòng được thêm " + res + " !");
            }
        }
        public static (bool, string) AcceptGroupRequest(int GroupId, int SenderId, int TargetUserId)
        {
            string cmd = "exec AcceptGroupRequest @GroupId , @UserId , @TargetUserId";
            int res = Conn.ExecuteScalar(cmd, new object[] { GroupId, SenderId, TargetUserId });
            string msg;
            if (res == 1)
            {
                return (true, "");
            }
            else
            {
                return (false, "Có lỗi xảy ra, số dòng được thêm " + res + " !");
            }
        }
        public static (bool, string) RemoveGroupMember(int GroupId, int SenderId, int TargetUserId)
        {
            string cmd = "exec RemoveGroupMember @GroupId , @UserId , @TargetUserId";
            int res = Conn.ExecuteScalar(cmd, new object[] { GroupId, SenderId , TargetUserId });
            string msg;
            if (res == 1)
            {
                return (true, "");
            }
            else
            {
                return (false, "Có lỗi xảy ra, số dòng được thêm " + res + " !");
            }
        }
        public static (bool, string) BanGroupMember(int GroupId, int SenderId, int TargetUserId)
        {
            string cmd = "exec BanGroupMember @GroupId , @SenderId , @TargetUserId";
            int res = Conn.ExecuteScalar(cmd, new object[] { GroupId, SenderId , TargetUserId });
            string msg;
            if (res == 1)
            {
                return (true, "");
            }
            else
            {
                return (false, "Có lỗi xảy ra, số dòng được thêm " + res + " !");
            }
        }
        public static (bool, string) UnBanGroupMember(int GroupId, int SenderId, int TargetUserId)
        {
            string cmd = "exec UnBanGroupMember @GroupId , @UserId , @TargetUserId";
            int res = Conn.ExecuteScalar(cmd, new object[] { GroupId, SenderId , TargetUserId });
            string msg;
            if (res == 1)
            {
                return (true, "");
            }
            else
            {
                return (false, "Có lỗi xảy ra, số dòng được thêm " + res + " !");
            }
        }
    }
}
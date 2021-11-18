using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SignalRChat.Models.Data
{
    public class UserUser
    {
        public int Id { get; set; }
        public int UserAId { get; set; }
        public int UserBId { get; set; }
        public int State { get; set; }
        // 1 : UserA gửi nhận yêu cầu kết bạn cho B
        // 2 : UserA nhận yêu cầu kết bạn từ UserB
        // 3 : Bạn bè
        // 4 : UserA chặn UserB
        // 5 : UserA bị UserB chặn

        public DateTime CreationTime { get; set; }
    }
    public class UserViewDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public int State { get; set; }
        public byte[] Photo { get; set; }

        public DateTime CreationTime { get; set; }

        // ** UserA là UserRequest
        // 0 : Không có trong bảng User_User, có thể kết bạn
        // 1 : UserA gửi nhận yêu cầu kết bạn cho B
        // 2 : UserA nhận yêu cầu kết bạn từ UserB
        // 3 : Bạn bè
        // 4 : UserA chặn UserB
        // 5 : UserA bị UserB chặn
    }
    public class User_UserFunc
    {
        public static List<UserViewDto> GetList(int UserId)
        {
            string query = "exec GetUser_User @UserId";
            DataTable dt = Conn.ExecuteQuery(query, new object[] { UserId }); ;
            var result = new List<UserViewDto>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Photo"].ToString();
                result.Add(new UserViewDto
                {
                    UserId = int.Parse(row["UserId"].ToString()),
                    Username = row["Username"].ToString(),
                    Name = row["Username"].ToString(),
                    State = int.Parse(row["State"].ToString()),
                    Photo = String.IsNullOrEmpty(tmp) ? new byte[0] : (byte[])row["Photo"],
                    //CreationTime = DateTime.Parse(row["CreationTime"].ToString()),
                });
            }

            return result;
        }
        public static List<UserViewDto> GetList(int UserId, List<int> States)
        {
            if(States.Count == 0)
            {
                return GetList(UserId);
            }
            else
            {
                return GetList(UserId).Where(e => States.Contains(e.State)).ToList(); ;
            }
        }
        public static List<User> GetFriends(int UserId)
        {
            string query = "exec GetFriends @UserId";
            DataTable dt = Conn.ExecuteQuery(query, new object[] { UserId }); ;
            var result = new List<User>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Photo"].ToString();
                result.Add(new User
                {
                    Id = int.Parse(row["Id"].ToString()),
                    Username = row["Username"].ToString(),
                    Name = row["Username"].ToString(),
                    Sex = int.Parse(row["Sex"].ToString()),
                    Photo = String.IsNullOrEmpty(tmp) ? new byte[0] : (byte[])row["Photo"],
                });
            }

            return result;
        }
        public static (bool, string) AddFriendRequest(int SenderId, int ReceiverId)
        {
            string cmd = "exec AddFriendRequest @SenderId , @ReceiverId ";
            int res = Conn.ExecuteScalar(cmd, new object[] { SenderId, ReceiverId });
            string msg;
            if (res == 2)
            {
                return (true, "");
            }
            else
            {
                return (false, "Có lỗi xảy ra, số dòng được thêm " + res + " !");
            }
        }
        public static (bool, string) AcceptFriendRequest(int SenderId, int ReceiverId)
        {
            string cmd = "exec AcceptFriendRequest @SenderId , @ReceiverId ";
            int res = Conn.ExecuteScalar(cmd, new object[] { SenderId, ReceiverId });
            string msg;
            if (res == 2)
            {
                return (true, "");
            }
            else
            {
                return (false, "Có lỗi xảy ra, số dòng được thêm " + res + " !");
            }
        }
        public static (bool, string) DeleteFriendRequest(int SenderId, int ReceiverId)
        {
            string cmd = "exec DeleteFriendRequest @SenderId , @ReceiverId ";
            int res = Conn.ExecuteScalar(cmd, new object[] { SenderId, ReceiverId });
            string msg;
            if (res == 2)
            {
                return (true, "");
            }
            else
            {
                return (false, "Có lỗi xảy ra, số dòng được thêm " + res + " !");
            }
        }

        public static (bool, string) BlockUser_User(int SenderId, int ReceiverId)
        {
            string cmd = "exec BlockUser_User @SenderId , @ReceiverId ";
            int res = Conn.ExecuteScalar(cmd, new object[] { SenderId, ReceiverId });
            string msg;
            if (res == 2)
            {
                return (true, "");
            }
            else
            {
                return (false, "Có lỗi xảy ra, số dòng được thêm " + res + " !");
            }
        }

        public static (bool, string) UnBlockUser_User(int SenderId, int ReceiverId)
        {
            string cmd = "exec UnBlockUser_User @SenderId , @ReceiverId ";
            int res = Conn.ExecuteScalar(cmd, new object[] { SenderId, ReceiverId });
            string msg;
            if (res == 2)
            {
                return (true, "");
            }
            else
            {
                return (false, "Có lỗi xảy ra, số dòng được thêm " + res + " !");
            }
        }

        public static (bool, string) UnFriendUser_User(int SenderId, int ReceiverId)
        {
            string cmd = "exec UnFriendUser_User @SenderId , @ReceiverId ";
            int res = Conn.ExecuteScalar(cmd, new object[] { SenderId, ReceiverId });
            string msg;
            if (res == 2)
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
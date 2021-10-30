using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SignalRChat.Models.Data
{
    public class Message
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public int GroupId { get; set; }

        public string Attachment { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentExtension { get; set; }

        public string Content { get; set; }
        public DateTime LastEditTime { get; set; }
        public DateTime CreationTime { get; set; }
    }
    public class CONTENT_TYPE
    {
        public static int Nu = 0;
        public static int Nam = 1;
    }

    public class MessageFunc
    {

        public static (bool, string) Add(Message message)
        {
            string cmd = "exec ThemMessage @Sender , @Receiver , @GroupId , @Attachment , @AttachmentName , @AttachmentExtention , @Content ";

            int res = Conn.ExecuteScalar(cmd, 
                    new object[] { 
                        message.Sender, message.Receiver, message.GroupId, System.Convert.FromBase64String(message.Attachment), message.AttachmentName,message.AttachmentExtension, message.Content
                    }
                );
            if (res > 0)
            {
                return (true, "");
            }
            else
            {
                return (false, "Lỗi không xác định");
            }
        }
        public static List<Message> GetConversation(string userA, string userB)
        {
            var listA = GetList(userA,userB);
            var listB = GetList(userB, userA);
            return listA.Concat(listB).ToList();
        }

        public static List<Message> GetList(string sender, string receiver)
        {

            string cmd = "select * from dbo.tblMessage where Sender ='"+ sender + "' and Receiver='"+ receiver + "'";
            DataTable dt = Conn.ExecuteQuery(cmd);
            var result = new List<Message>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Attachment"].ToString();
                result.Add(new Message
                {
                    Id = int.Parse(row["Id"].ToString()),
                    Sender = row["Sender"].ToString(),
                    Receiver = row["Receiver"].ToString(),
                    GroupId = int.Parse(row["GroupId"].ToString()),
                    Content = row["Content"].ToString(),
                    Attachment = String.IsNullOrEmpty(tmp) ? "" : Convert.ToBase64String((byte[])row["Attachment"]),
                    AttachmentName = row["AttachmentName"].ToString(),
                    AttachmentExtension = row["AttachmentExtension"].ToString(),
                    LastEditTime = DateTime.Parse(row["LastEditTime"].ToString()),
                    CreationTime = DateTime.Parse(row["CreationTime"].ToString()),
                });
            }

            return result;
        }

    }

}
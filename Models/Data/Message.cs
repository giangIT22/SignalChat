using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace SignalRChat.Models.Data
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int ReceiverId { get; set; }
        public int GroupId { get; set; }

        public string SenderPhoto { get; set; }

        public string Attachment { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentExtension { get; set; }

        public string Content { get; set; }
        public DateTime LastEditTime { get; set; }
        public DateTime CreationTime { get; set; }
        public string StrCreationTime
        {
            get
            {
                return CreationTime.ToString("dd/MM/yyyy HH:mm");
            }
        }
    }
    public class CONTENT_TYPE
    {
        public static int Nu = 0;
        public static int Nam = 1;
    }

    public class MessageFunc
    {

        public static (int, string) Add(Message message)
        {

            //string cmd = "exec ThemMessage @SenderId , @ReceiverId , @GroupId , @Attachment , @AttachmentName , @AttachmentExtention , @Content ";

            //int InsertedId = Conn.ExecuteScalar(cmd, 
            //        new object[] { 
            //            message.SenderId, message.ReceiverId, message.GroupId, System.Convert.FromBase64String(message.Attachment), message.AttachmentName,message.AttachmentExtension, message.Content
            //        }
            //    );
            //if (InsertedId > 0)
            //{
            //    return (InsertedId, "");
            //}
            //else
            //{
            //    return (-1, "Lỗi không xác định");
            //}
            DbConnection conObject = Conn.GetDatabaseConnection();
            try
            {
                conObject.Open();
                if (conObject.State == ConnectionState.Open)
                {
                    //Response.Write("Database Connection is Open");


                    using (var cmd = conObject.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ThemMessage";

                        cmd.Parameters.Add(new SqlParameter("@SenderId", SqlDbType.Int));
                        cmd.Parameters["@SenderId"].Value = message.SenderId;

                        cmd.Parameters.Add(new SqlParameter("@GroupId", SqlDbType.Int));
                        cmd.Parameters["@GroupId"].Value = message.GroupId;

                        cmd.Parameters.Add(new SqlParameter("@ReceiverId", SqlDbType.Int));
                        cmd.Parameters["@ReceiverId"].Value = message.ReceiverId;

                        cmd.Parameters.Add(new SqlParameter("@Attachment", SqlDbType.VarBinary,-1));
                        cmd.Parameters["@Attachment"].Value = Convert.FromBase64String(message.Attachment);

                        cmd.Parameters.Add(new SqlParameter("@AttachmentName", SqlDbType.NVarChar, 100));
                        cmd.Parameters["@AttachmentName"].Value = message.AttachmentName;

                        cmd.Parameters.Add(new SqlParameter("@AttachmentExtention", SqlDbType.VarChar, 30));
                        cmd.Parameters["@AttachmentExtention"].Value = message.AttachmentExtension;

                        cmd.Parameters.Add(new SqlParameter("@Content", SqlDbType.NVarChar, 500));
                        cmd.Parameters["@Content"].Value = message.Content;

                       

                        cmd.UpdatedRowSource = UpdateRowSource.OutputParameters;

                        var res = int.Parse(cmd.ExecuteScalar().ToString());

                        string CreateMsg = "";
                        if (res <= 0)
                        {
                            CreateMsg =  "Lỗi không xác định";
                            return (0, CreateMsg);
                        }

                        return (res, CreateMsg);
                    }

                }

            }
            catch (SqlException sqlexception)
            {
                return (-1, "Lỗi sql !");
            }
            catch (Exception ex)
            {
                return (-1, "Lỗi không xác định !");
            }
            finally
            {
                conObject.Close();
            }
            return (-1, "Lỗi Không xác định !");

        }
        public static List<Message> GetConversation(int userAId, int userBId)
        {
            var listA = GetList(userAId,userBId);
            var listB = GetList(userBId,userAId);
            return listA.Concat(listB).ToList();
        }
        public static void DeleteMessage(int Id)
        {
            string cmd = "delete AshChat.dbo.tblMessage where Id = "+ Id;
            var result = Conn.ExecuteNonQuery(cmd);
        }
        public static List<Message> GetList(int senderId, int receiverId)
        {

            string cmd = "exec GetListMessages @SenderId , @ReceiverId";
            DataTable dt = Conn.ExecuteQuery(cmd, new object[] { senderId, receiverId});
            var result = new List<Message>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Attachment"].ToString();
                var photo = row["Photo"].ToString();
                result.Add(new Message
                {
                    Id = int.Parse(row["Id"].ToString()),
                    SenderId = int.Parse( row["SenderId"].ToString() ),
                    SenderName = row["SenderUsername"].ToString(),
                    ReceiverId = int.Parse( row["ReceiverId"].ToString() ),
                    GroupId = int.Parse(row["GroupId"].ToString()),
                    Content = row["Content"].ToString(),
                    Attachment = String.IsNullOrEmpty(tmp) ? "" : Convert.ToBase64String((byte[])row["Attachment"]),
                    SenderPhoto = String.IsNullOrEmpty(photo) ? "" : Encoding.UTF8.GetString((byte[])row["Photo"]),
                    AttachmentName = row["AttachmentName"].ToString(),
                    AttachmentExtension = row["AttachmentExtension"].ToString(),
                    LastEditTime = DateTime.Parse(row["LastEditTime"].ToString()),
                    CreationTime = DateTime.Parse(row["CreationTime"].ToString()),
                });
            }

            return result;
        }

        public static Message GetById(int Id)
        {
            string cmd = "select * from AshChat.dbo.tblMessage where Id = " + Id;
            DataTable dt = Conn.ExecuteQuery(cmd);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Attachment"].ToString();
                return new Message
                {
                    Id = int.Parse(row["Id"].ToString()),
                    SenderId = int.Parse(row["SenderId"].ToString()),
                    ReceiverId = int.Parse(row["ReceiverId"].ToString()),
                    GroupId = int.Parse(row["GroupId"].ToString()),
                    Content = row["Content"].ToString(),
                    Attachment = String.IsNullOrEmpty(tmp) ? "" : Encoding.UTF8.GetString((byte[])row["Attachment"]),
                    AttachmentName = row["AttachmentName"].ToString(),
                    AttachmentExtension = row["AttachmentExtension"].ToString(),
                    LastEditTime = DateTime.Parse(row["LastEditTime"].ToString()),
                    CreationTime = DateTime.Parse(row["CreationTime"].ToString()),
                };
            }
            return null;

        }

        public static List<Message> GetGroupMessage(int GroupId)
        {
            string cmd = "exec GetListGroupMessages @GroupId";
            DataTable dt = Conn.ExecuteQuery(cmd, new object[] { GroupId});
            var result = new List<Message>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                var tmp = row["Attachment"].ToString();
                var photo = row["Photo"].ToString();
                result.Add(new Message
                {
                    Id = int.Parse(row["Id"].ToString()),
                    SenderId = int.Parse(row["SenderId"].ToString()),
                    SenderName = row["SenderUsername"].ToString(),
                    ReceiverId = int.Parse(row["ReceiverId"].ToString()),
                    GroupId = int.Parse(row["GroupId"].ToString()),
                    Content = row["Content"].ToString(),
                    Attachment = String.IsNullOrEmpty(tmp) ? "" : Convert.ToBase64String((byte[])row["Attachment"]),
                    SenderPhoto = String.IsNullOrEmpty(photo) ? "" : Encoding.UTF8.GetString((byte[])row["Photo"]),
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
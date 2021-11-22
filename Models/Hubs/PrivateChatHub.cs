using Microsoft.AspNet.SignalR;
using SignalRChat.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SignalRChat.Models.Hubs
{
    public class PrivateChatHub : Hub
    {
        // Duy trì IdConnection - 
        private static Dictionary<int, string> dctConnectionId = new Dictionary<int, string>();
        private static Dictionary<string, int> dctUserId = new Dictionary<string, int>();
        private static Dictionary<int, string> dctAvatar = new Dictionary<int, string>();
        // Group

        public void Connect(int UserId, string FileNameAvatar)
        {
            dctConnectionId[UserId] = Context.ConnectionId;
            dctUserId[Context.ConnectionId] = UserId;
            dctAvatar[UserId] = FileNameAvatar;

            var groupsOfUser = User_GroupFunc.GetGroupsOfUser(UserId);
            foreach(var e in groupsOfUser)
            {
                Groups.Add(Context.ConnectionId, e.GroupId.ToString());
            }
            
        }
        
        // Lấy danh sách liên hệ của một người dùng
        public void GetContactsList(int UserId)
        {
            // Hiện tại là đang lấy tất cả người dùng, trừ người(client) gọi
            var listUser = User_UserFunc.GetFriends(UserId)
                .Where(e => e.Id != UserId)
                .Select(e => new ContactDto {
                    Id = e.Id,
                    Name = e.Name,
                    IsGroup = false
                }).ToList();
            var listGroup = User_GroupFunc.GetGroupsOfUser(UserId)
                .Select(
                    e => new ContactDto
                    {
                        Id = e.GroupId,
                        Name = e.Name,
                        IsGroup = true
                    }
                );
            var result = listUser.Concat(listGroup).ToList();
            Clients.Caller.showContactsList(result);
        }

        // Lấy danh sách tin nhắn của 2 người.
        public void LoadMessageOf(int userAId, int userBId, string isGroup)
        {
            if (bool.Parse(isGroup))
            {
                List<Message> lstmsg = MessageFunc.GetGroupMessage(userBId);
                Clients.Caller.showListMessage(lstmsg.OrderBy(e => e.CreationTime));
            }
            else
            {
                List<Message> lstmsg = MessageFunc.GetConversation(userAId, userBId);
                Clients.Caller.showListMessage(lstmsg.OrderBy(e => e.CreationTime));
            }
            
        }

        public void SendPrivateMessage(int senderId, string senderName, int receiverId, string isGroup, string content, string FileName, string FileType, string FileContent)
        {

            string senderFileNameAvatar = "";
            if (dctAvatar.ContainsKey(senderId))
            {
                senderFileNameAvatar = dctAvatar[senderId];
            }

            string Time = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            if (bool.Parse(isGroup))
            {
                int InsertedId;
                string msg;

                string path = ImageFunc.SaveMessageImage(FileName, FileType, FileContent);

                (InsertedId, msg) = MessageFunc.Add(new Message
                {
                    SenderId = senderId,
                    GroupId = receiverId,
                    Content = content,
                    Attachment = FileContent,
                    AttachmentName = path,
                    AttachmentExtension = FileType
                });

                if(InsertedId > 0)
                {
                    Clients.Group(receiverId.ToString()).showMessage(InsertedId, senderId, senderFileNameAvatar, senderName, receiverId, isGroup, content, path, FileType, FileContent, Time);
                }
            }
            else
            {
                int InsertedId;
                string msg;
                
                string path = ImageFunc.SaveMessageImage(FileName, FileType, FileContent);

                (InsertedId, msg) = MessageFunc.Add(new Message
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Content = content,
                    Attachment = FileContent,
                    AttachmentName = path,
                    AttachmentExtension = FileType
                }); ;

                if(InsertedId > 0)
                {
                    if (dctConnectionId.ContainsKey(receiverId))
                    {
                        // Người nhận đang online
                        string receiverConnectionId = dctConnectionId[receiverId];
                        Clients.Client(receiverConnectionId).showMessage(InsertedId, senderId, senderFileNameAvatar, senderName, receiverId, isGroup, content, path, FileType, FileContent, Time);
                    }
                    Clients.Caller.showMessage(InsertedId, senderId, senderFileNameAvatar, senderName, receiverId, isGroup, content, path, FileType, FileContent, Time);
                }
            }
        }

        public void DeleteMessage(int MessageId)
        {
            var mes = MessageFunc.GetById(MessageId);

            ImageFunc.RemoveMessageImage(mes.AttachmentName);

            MessageFunc.DeleteMessage(MessageId);
            if (mes == null) return;
            if(mes.GroupId <= 0)
            {
                if (dctConnectionId.ContainsKey(mes.SenderId))
                {
                    Clients.Client(dctConnectionId[mes.SenderId]).removeMessage(MessageId);
                }
                if (dctConnectionId.ContainsKey(mes.ReceiverId))
                {
                    Clients.Client(dctConnectionId[mes.ReceiverId]).removeMessage(MessageId);
                }
            }
            else
            {
                //
                Clients.Group(mes.GroupId.ToString()).removeMessage(MessageId);
            }
            
        }
        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            string currentId = Context.ConnectionId;
            if (dctUserId.ContainsKey(currentId))
            {
                int UserId = dctUserId[currentId];
                dctConnectionId.Remove(UserId);

                dctUserId.Remove(currentId);

                var groupsOfUser = User_GroupFunc.GetGroupsOfUser(UserId);
                foreach (var e in groupsOfUser)
                {
                    Groups.Remove(UserId.ToString(), e.GroupId.ToString());
                }
           }

            Clients.Caller.reloadPage();

            // begin: for testing
            int DisconnectingUserId;
            if (dctUserId.ContainsKey(currentId))
            {
                DisconnectingUserId = dctUserId[currentId];
            }
            // end: for testing

            return base.OnDisconnected(stopCalled);
        }
        
    }

    public class ContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsGroup { get; set; }
    }
    
}
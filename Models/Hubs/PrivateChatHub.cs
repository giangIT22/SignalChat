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

        public void Connect(int UserId)
        {
            dctConnectionId[UserId] = Context.ConnectionId;
            dctUserId[Context.ConnectionId] = UserId;
        }
        
        // Lấy danh sách liên hệ của một người dùng
        public void GetContactsList(int UserId)
        {
            // Hiện tại là đang lấy tất cả người dùng, trừ người(client) gọi
            var listUser = User_UserFunc.GetFriends(UserId)
                .Where(e=>e.Id != UserId)
                .Select(e => new UserDto {
                    Id = e.Id,
                    Username = e.Username, 
                    Name = e.Name 
                }).ToList();

            Clients.Caller.showContactsList(listUser);
        }

        // Lấy danh sách tin nhắn của 2 người.
        public void LoadMessageOf(int userAId, int userBId)
        {
            List<Message> lstmsg = MessageFunc.GetConversation(userAId, userBId);
            Clients.Caller.showListMessage(lstmsg.OrderBy(e=>e.CreationTime));
        }

        public void SendPrivateMessage(int senderId, int receiverId, string content, string FileName, string FileType, string FileContent)
        {
            var szContent = FileContent.Length;
            if (dctConnectionId.ContainsKey(receiverId))
            {
                // Người nhận đang online
                string receiverConnectionId = dctConnectionId[receiverId];
                Clients.Client(receiverConnectionId).showMessage(senderId, receiverId, content, FileName, FileType, FileContent);
            }

            Clients.Caller.showMessage(senderId, receiverId, content, FileName, FileType, FileContent);

            MessageFunc.Add(new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = content,
                Attachment = FileContent,
                AttachmentName = FileName,
                AttachmentExtension = FileType
            });


        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            string currentId = Context.ConnectionId;
            int UserId = dctUserId[currentId];
            dctConnectionId.Remove(UserId);

            dctUserId.Remove(currentId);

            return base.OnDisconnected(stopCalled);
        }

        
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
    }
    
}
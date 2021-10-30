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
        public static Dictionary<string, string> dctConnectionId = new Dictionary<string, string>();
        public static Dictionary<string, string> dctUsername = new Dictionary<string, string>();

        public void Connect(string username)
        {
            dctConnectionId[username] = Context.ConnectionId;
            dctUsername[Context.ConnectionId] = username;
        }

       
        public List<string> getMessageOf(string user1, string user2)
        {
            return new List<string>() { "shiet", "mtfk" };
        }
        public void GetContactsList(string username)
        {
            var listUser = UserFunc.GetList().Where(e=>e.Username != username).Select(e => new { username = e.Username, name = e.Name }).ToList();
            
            Clients.Caller.showContactsList(listUser);

        }

        public void LoadMessageOf(string userA, string userB)
        {
            List<Message> lstmsg = MessageFunc.GetConversation(userA, userB);

            Clients.Caller.showListMessage(lstmsg.OrderBy(e=>e.CreationTime));

        }


        public void SendPrivateMessage(string sender, string receiver, string content, string FileName, string FileType, string FileContent)
        {
            var szContent = FileContent.Length;
            if (dctConnectionId.ContainsKey(receiver))
            {
                // Người nhận đang online
                string receiverId = dctConnectionId[receiver];
                Clients.Client(receiverId).showMessage(sender, receiver, content, FileName, FileType, FileContent);
            }

            Clients.Caller.showMessage(sender, receiver, content, FileName, FileType, FileContent);

            MessageFunc.Add(new Message
            {
                Sender = sender,
                Receiver = receiver,
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
            dctUsername.Remove(currentId);

            if (dctUsername.ContainsKey(currentId)) {
                string username = dctUsername[currentId];
                dctConnectionId.Remove(username);
            }



            return base.OnDisconnected(stopCalled);
        }

        
    }
    
}
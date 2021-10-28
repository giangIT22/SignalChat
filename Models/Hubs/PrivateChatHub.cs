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

        public void SendMessage(string SenderName, string ReceiverName, string content)
        {
            if (dctConnectionId.ContainsKey(ReceiverName))
            {
                // nếu đang kết nối mới gửi tin nhắn
                string receiverId = dctConnectionId[ReceiverName];
                Clients.Client(receiverId).AddMessage(SenderName,content);
            }
            // Dù kết nối hay không vẫn lưu tin nhắn
            // lưu tin nhắn
            
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            string currentId = Context.ConnectionId;
            dctUsername.Remove(currentId);

            if (dctUsername.ContainsKey(currentId)){
                string username = dctUsername[currentId];
                dctConnectionId.Remove(username);
            }
            


            return base.OnDisconnected(stopCalled);
        }
    }
}
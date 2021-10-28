using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;


namespace SignalRChat
{
    public class ChatHub : Hub
    {
        static List<Users2> ConnectedUsers = new List<Users2>();
        static List<Messages2> CurrentMessage = new List<Messages2>();
        ConnClass ConnC = new ConnClass();

        public void Connect(string userName)
        {
            var id = Context.ConnectionId;

            CurrentMessage.Clear();
            ConnectedUsers.Clear();
            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                string UserImg = this.GetUserImage(userName);
                string logintime = DateTime.Now.ToString();

                //show list messages
                this.showListMessagse();
                //ConnectedUsers.Add(new Users { ConnectionId = id, UserName = userName, UserImage = UserImg, LoginTime = logintime });
                // send to caller
                Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);

                // send to all except caller client
                //Clients.AllExcept(id).onNewUserConnected(id, userName, UserImg, logintime);
            }
        }

        public string GetUserImage(string username)
        {
            string RetimgName = "images/dummy.png";
            try
            {
                string query = "select photo from users where userName='" + username + "'";
                DataTable dt = ConnC.ExecuteQuery(query);
                string ImageName = dt.Rows[0].Field<string>("photo");

                if (ImageName != "")
                    RetimgName = "images/DP/" + ImageName;
            }
            catch (Exception ex)
            { }
            return RetimgName;
        }

        public void SendMessageToAll(string userName, string message, string time)
        {
            string UserImg = this.GetUserImage(userName);
            string query = "EXECUTE dbo.addMessage @userName , @content , @userImage , @time";
            // store last 100 messages in cache
            //AddMessageinCache(userName, message, time, UserImg);
            ConnC.ExecuteNonQuery(query, new object[] { userName, message, UserImg, time });
            // Broad cast message
            Clients.All.messageReceived(userName, message, time, UserImg);

        }

        //private void AddMessageinCache(string userName, string message, string time, string UserImg)
        //{
        //    CurrentMessage.Add(new Messages { UserName = userName, Message = message, Time = time, UserImage = UserImg });

        //    if (CurrentMessage.Count > 100)
        //        CurrentMessage.RemoveAt(0);

        //}

        public void showListMessagse()
        {
            //DataTable tb = ConnC.ExecuteQuery("select * from message");
            //for (int i = 0; i < tb.Rows.Count; i++)
            //{
            //    Messages2 messages = new Messages();
            //    messages.UserName = tb.Rows[i]["userName"].ToString();
            //    messages.UserImage = tb.Rows[i]["userImage"].ToString();
            //    messages.Message = tb.Rows[i]["content"].ToString();
            //    messages.Time = tb.Rows[i]["created_at"].ToString();
            //    CurrentMessage.Add(messages);
            //}
        }

        // Clear Chat History
        public void clearTimeout()
        {
            ConnC.ExecuteNonQuery("Delete from message");
            CurrentMessage.Clear();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);

                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.UserName);

            }
            return base.OnDisconnected(stopCalled);
        }

        public void SendPrivateMessage(string toUserId, string message)
        {

            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                string CurrentDateTime = DateTime.Now.ToString();
                string UserImg = GetUserImage(fromUser.UserName);
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message, UserImg, CurrentDateTime);

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message, UserImg, CurrentDateTime);
            }

        }
    }
}
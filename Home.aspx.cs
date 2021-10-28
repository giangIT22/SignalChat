using SignalRChat.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalRChat
{
    public partial class Home : System.Web.UI.Page
    {

        public string Username = "shjety";
        public List<string> friends = new List<string> { "hai", "jang", "toan" };
        public List<string> messages = new List<string> { "alo", "shjet", "pussy" };
        protected void Page_Load(object sender, EventArgs e)
        {

            var user = (User)Session[AppConst.SessionCurrentUserKey];
            this.Username = user.Username;

            var test = IsPostBack;
            rpListFriend.DataSource = friends;
            rpListFriend.DataBind();


            
            rpChatBox.DataSource = messages;
            rpChatBox.DataBind();
        }



        protected void btnSend_Click(object sender, EventArgs e)
        {
            messages.Add("test" + messages.Count);
            rpChatBox.DataSource = messages;
            rpChatBox.DataBind();
        }

 

    }
    
}
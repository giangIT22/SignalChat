using Microsoft.AspNet.SignalR;
using SignalRChat.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalRChat
{
    public partial class Home : System.Web.UI.Page
    {

        public User currentUser;
        public string FileNameAvatar = ""; 
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.AppendHeader("Pragma", "no-cache");
            Response.Expires = -1;
            Response.Cache.SetNoStore();

            currentUser = new User();
            var user = (User)Session[AppConst.SessionCurrentUserKey];
            if(user == null)
            {
                Response.Redirect("Login.aspx");
                
            }
            currentUser = user;
            FileNameAvatar = Encoding.UTF8.GetString(user.Photo);
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {

        }
    }
    
}
using Microsoft.AspNet.SignalR;
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

        public string Username = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            var user = (User)Session[AppConst.SessionCurrentUserKey];
            if(user == null)
            {
                Response.Redirect("Login.aspx");
            }
            this.Username = user.Username;

            var test = IsPostBack;
        }



        protected void btnSend_Click(object sender, EventArgs e)
        {
        }

 

    }
    
}
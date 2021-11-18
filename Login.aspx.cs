using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using SignalRChat.Models.Data;

namespace SignalRChat
{
    public partial class Login : System.Web.UI.Page
    {
        //Class Object
        ConnClass cnn = new ConnClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            User currentUser = (User)Session[AppConst.SessionCurrentUserKey];
            if (currentUser != null)
            {
                Response.Redirect("Home.aspx");
            }
        }

        protected void btnLogin_ServerClick(object sender, EventArgs e)
        {   
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            User user = null;
            string message = "";
            (user, message) = UserFunc.DangNhap(username, password);

            if (user != null)
            {
                Session[AppConst.SessionCurrentUserKey] = user;
                
                Response.Redirect("Home.aspx");
            }
            else
            {
                errorMessage.InnerText = message;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace SignalRChat
{
    public partial class Login : System.Web.UI.Page
    {
        //Class Object
        ConnClass cnn = new ConnClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                Response.Redirect("Chat.aspx");
            }
        }

        protected void btnLogin_ServerClick(object sender, EventArgs e)
        {
            string name = Request.Form["username"].ToString();
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(Request.Form["password"].ToString(), "MD5");
            string query = "select * from users where userName='" + name + "'" + " and password='" + password + "'";

            if (cnn.ExecuteQuery(query).Rows.Count > 0)
            {
                Session["UserName"] = name;
                Response.Redirect("Chat.aspx");
            }
            else
            {
                errorMessage.InnerText = "Username or password is incorrect";
            }
        }
    }
}
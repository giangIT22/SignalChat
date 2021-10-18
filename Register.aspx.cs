using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace SignalRChat
{
    public partial class Register : System.Web.UI.Page
    {
        ConnClass cnn = new ConnClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void btnRegister_ServerClick(object sender, EventArgs e)
        {
            string name = Request.Form["username"].ToString();
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(Request.Form["password"].ToString(), "MD5");

            string query = " EXEC dbo.insertUser @name , @password";
            string exist = "select * from users where userName = " + "'" + name + "'";

            if (name == "")
            {
                errorMessage.InnerText = "Vui lòng nhập tên người dùng";
                return;
            } else if (Password.Value == "")
            {
                errorMessage.InnerText = "Vui lòng nhập mật khẩu";
                return;
            }

            if (Password.Value != Repassword.Value)
            {
                errorMessage.InnerText = "Mật khẩu nhập lại không chính xác";
                return;
            }

            if (cnn.ExecuteQuery(exist).Rows.Count <= 0)
            {
                if (cnn.ExecuteNonQuery(query, new object[] { name, password }) > 0)
                {
                    Response.Redirect("Login.aspx");
                }
            }
            else
            {
                errorMessage.InnerText = "Tài khoản này đã tồn tại";
            }
        } 
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using SignalRChat.Models.Data;

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
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string repassword = txtRepassword.Text;


            if (username == "")
            {
                errorMessage.InnerText = "Vui lòng nhập tên người dùng";
                return;
            } else if (password == "")
            {
                errorMessage.InnerText = "Vui lòng nhập mật khẩu";
                return;
            }

            if (password != repassword)
            {
                errorMessage.InnerText = "Mật khẩu nhập lại không chính xác";
                return;
            }


            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            bool isSuccess = false;
            string msg = "";
            (isSuccess,msg) = UserFunc.DangKy(username, hashedPassword);

            if (isSuccess)
            {
               Response.Redirect("ChatBox.aspx");
            } 
            else
            {
                errorMessage.InnerText = msg;
            }
        } 
    }
}
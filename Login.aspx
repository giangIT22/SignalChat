<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SignalRChat.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <title>SignalR Chat : Login</title>
        <link rel="stylesheet" href="Styles/Login.css"  />

</head>
<body >
    <div class="login">
            <div class="container login--width">
               <div class="item">
                    <div class="login__logo">
                        <p>AshChat</p>
                    </div>
                    <h5>Đăng Nhập</h5> 
                    <form id="form1" runat="server">
                        <div id="login-form">
                            <div class="form-group">
                                <asp:TextBox ID="txtUsername" runat="server" placeholder="Tên đăng nhập" class="form-control" required></asp:TextBox>
                                <span class="form-message"></span>
                            </div>
                            <div class="form-group">
                                <asp:TextBox ID="txtPassword" runat="server" placeholder="Mật khẩu" class="form-control" required TextMode="Password"></asp:TextBox>
                                <span class="form-message"></span>
                            </div>
                            <button class="form-submit" id="Button2" onserverclick="btnLogin_ServerClick" runat="server">Đăng nhập</button>
                            <span  class="message" id="errorMessage" runat="server"></span>
                        </div>
                    </form>
                    <a href="/Register.aspx">Tạo tài khoản</a>
               </div>
            </div>
        </div>
</body>
</html>







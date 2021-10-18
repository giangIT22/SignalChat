<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SignalRChat.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SignalR Chat : Login</title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/style.css" rel="stylesheet" />
    <link href="Content/font-awesome.css" rel="stylesheet" />
    <link href="Content/icheck-bootstrap.css" rel="stylesheet" />
</head>
<body >
-     <div class="login">
        <div class="container login--width">
           <div class="item">
                <div class="login__logo">
                    <p>beHive Chat</p>
                </div>
                <h5>Đăng Nhập</h5>
                <form id="form1" runat="server">
                    <div class="form-group">
                        <input type="text" name="name" id="username" placeholder="Tên đăng nhập" class="form-control" runat="server" required>
                        <span class="form-message"></span>
                    </div>
                    <div class="form-group">
                        <input type="password" name="password" id="password" placeholder="Mật khẩu" class="form-control" runat="server">
                        <span class="form-message"></span>
                    </div>
                    <button class="form-submit" id="btnLogin" onserverclick="btnLogin_ServerClick" runat="server">Đăng nhập</button>
                    <span class="message" id="errorMessage" runat="server"></span>
                </form>
                <a href="/Register.aspx">Tạo tài khoản</a>
           </div>
        </div>
    </div>
 
</body>
</html>

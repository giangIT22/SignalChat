<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SignalRChat.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>SignalR Chat : Register</title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/style.css" rel="stylesheet" />
    <link href="Content/icheck-bootstrap.css" rel="stylesheet" />
    <link href="Content/font-awesome.css" rel="stylesheet" />
</head>
<body>
    <div class="login">
        <div class="container login--width">
           <div class="item">
                <div class="login__logo">
                    <p>beHive</p>
                </div>
                <h5>Tạo tài khoản</h5>
                <form id="form1" runat="server">
                    <div class="form-group">
                        <input type="text" name="name" id="username" placeholder="Tên đăng nhập" class="form-control" runat="server">
                        <span class="form-message"></span>
                    </div>
                    <div class="form-group">
                        <input type="password" name="password" id="Password" placeholder="Mật khẩu" class="form-control" runat="server">
                        <span class="form-message"></span>
                    </div>
                    <div class="form-group">
                        <input type="password" name="rePassword" id="Repassword" placeholder="Nhập lại mật khẩu" class="form-control" runat="server">
                        <span class="form-message"></span>
                    </div>
                    <button class="form-submit" runat="server" id="btnRegister" onserverclick="btnRegister_ServerClick">Đăng ký</button>
                    <span class="message" id="errorMessage" runat="server"></span>
                </form>
                <a href="/Login.aspx" style="float: right;">Đăng nhập</a>
           </div>
        </div>
    </div>
</body>
</html>

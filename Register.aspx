<%@ Page Title="" Language="C#" MasterPageFile="~/TopBar.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SignalRChat.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>SignalR Chat : Register</title>
    <link rel="stylesheet" href="Styles/Login.css"  />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="login">
        <div class="container login--width">
           <div class="item">
                <div class="login__logo">
                    <p>beHive</p>
                </div>
                <h5>Tạo tài khoản</h5>
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
                <a href="/Login.aspx" style="float: right;">Đăng nhập</a>
           </div>
        </div>
    </div>
</asp:Content>
    

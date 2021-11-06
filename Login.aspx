<%@ Page Title="" Language="C#" MasterPageFile="~/TopBar.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SignalRChat.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>SignalR Chat : Login</title>
    <link rel="stylesheet" href="Styles/Login.css"  />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    
         <div class="login">
            <div class="container login--width">
               <div class="item">
                    <div class="login__logo">
                        <p>AshChat</p>
                    </div>
                    <h5>Đăng Nhập</h5> 
                    <div id="login-form">
                        <div class="form-group">
                            <asp:TextBox ID="txtUsername" runat="server" placeholder="Tên đăng nhập" class="form-control" required></asp:TextBox>
                            <span class="form-message"></span>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtPassword" runat="server" placeholder="Mật khẩu" class="form-control" required TextMode="Password"></asp:TextBox>
                            <span class="form-message"></span>
                        </div>
                        <button class="form-submit" id="btnLogin" onserverclick="btnLogin_ServerClick" runat="server">Đăng nhập</button>
                        <span class="message" id="errorMessage" runat="server"></span>
                    </div>
                    <a href="/Register.aspx">Tạo tài khoản</a>
               </div>
            </div>
        </div>
 
</asp:Content>






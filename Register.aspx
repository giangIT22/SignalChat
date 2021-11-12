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
                        <asp:TextBox ID="txtUsername" runat="server" placeholder="Tên đăng nhập" class="form-control" ></asp:TextBox>
                        <span class="form-message"></span>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="txtPassword" runat="server" placeholder="Mật khẩu" class="form-control" TextMode="Password"></asp:TextBox>
                        <span class="form-message"></span>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="txtRepassword" runat="server" placeholder="Nhập lại mật khẩu" class="form-control" TextMode="Password"></asp:TextBox>
                        <span class="form-message"></span>
                    </div>
                    <button class="form-submit" runat="server" id="btnRegister" onserverclick="btnRegister_ServerClick">Đăng ký</button>
                    <span class="message" id="errorMessage" runat="server"></span>
                <a href="/Login.aspx" style="float: right;">Đăng nhập</a>
           </div>
        </div>
    </div>
</asp:Content>
    

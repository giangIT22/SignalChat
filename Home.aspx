<%@ Page Title="" Language="C#" MasterPageFile="~/TopBar.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SignalRChat.Home1" EnableEventValidation="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Styles/Home.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="home-container">
        

        <ul class="nav nav-tabs">
          <li id="user-tab" class="home-tab active" ><a href="#">Mọi người</a></li>
          <li id="group-tab" class="home-tab"><a href="#">Nhóm</a></li>
        </ul>
        <div id="content-container">
            <div id="content-user">
                <div id="filter-container">
                    <input type="text" value="" />
                    <button type="button" class="btn btn-default">
                        <span class="glyphicon glyphicon-search"></span> Search
                    </button>

                    <label class="checkbox-inline">
                        <asp:CheckBox ID="ckbFriendsRequest" runat="server" OnCheckedChanged="ckbFriendsRequest_CheckedChanged" /> Lời mời kết bạn
                    </label>
                    <label class="checkbox-inline">
                        <asp:CheckBox ID="ckbFriends" runat="server" OnCheckedChanged="ckbFriends_CheckedChanged" /> Bạn bè
                    </label>
                    <label class="checkbox-inline">
                        <asp:CheckBox ID="ckbFriendsBlock" runat="server" OnCheckedChanged="ckbFriendsBlock_CheckedChanged" /> Đã chặn
                    </label>


                </div>
                <div id="list-user">
                    <asp:Repeater ID="rptUsers" runat="server" ViewStateMode="Enabled">
                        <ItemTemplate>


                                <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem) != 5 )%>">

                                    <div class="user-box">

                                        <div class="user-name">
                                            <%#Eval("Name")%>
                                        </div>

                                        <div class="user-btn">

                                            <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem) == 0 )%>">
                                                <asp:Button Type="button"  Text="Thêm bạn" ID="test" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="AddFriendRequest" OnClick="UserBtnHandler" />
                                                <asp:Button Type="button" Text="Chặn" ID="Button1" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="BlockFriend" OnClick="UserBtnHandler" />
                                            </asp:PlaceHolder> 

                                            <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem)  == 1 )%>">
                                                <asp:Button Type="button" Text="Hủy lời mời kết bạn" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="DeleteFriendRequest" OnClick="UserBtnHandler"/>
                                            </asp:PlaceHolder> 

                                            <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem) == 2 )%>">
                                                <asp:Button Type="button" Text="Chấp nhận" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="AcceptFriendRequest" OnClick="UserBtnHandler"/>
                                                <asp:Button Type="button" Text="Xóa" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="ReverseDeleteFriendRequest" OnClick="UserBtnHandler"/>
                                            </asp:PlaceHolder>  

                                            <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem) == 3 )%>">
                                                <asp:Button Type="button" Text="Hủy kết bạn" ID="Button3" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="UnFriend" OnClick="UserBtnHandler" />
                                                <asp:Button Type="button" Text="Chặn" ID="Button2" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="BlockFriend" OnClick="UserBtnHandler" />
                                            </asp:PlaceHolder>  

                                            <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem)  == 4 )%>">
                                                <asp:Button Type="button" Text="Bỏ chặn" ID="Button4" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="UnBlock" OnClick="UserBtnHandler" />
                                            </asp:PlaceHolder>  

                                            <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem)  == 5 )%>">
                                            </asp:PlaceHolder>  
                                        </div>
                                    </div>

                                </asp:PlaceHolder>



                        </ItemTemplate>
                    </asp:Repeater>

                </div>
                
            </div>
            <div id="content-group">
                <h1>Group content</h1>
            </div>
        </div>
    </div>
    <script>
        // mặc định ban đầu ẩn tab group's content
        $('#content-group').hide();

        $('#user-tab').click(function () {
            $('#content-group').hide();
            $('#content-user').show();
            $('.home-tab').toArray().forEach(
                e => {
                    if (e.id == 'user-tab') {
                        e.classList.add('active');
                    } else {
                        e.classList.remove('active');
                    }
                });
        });
        $('#group-tab').click(function () {
            $('#content-group').show();
            $('#content-user').hide();
            $('.home-tab').toArray().forEach(
                e => {
                    if (e.id == 'group-tab') {
                        e.classList.add('active');
                    } else {
                        e.classList.remove('active');
                    }
                });
        });
        $(function () {
            // Đổi in đậm phần navigation

            var nav_options = $('.nav-option').toArray();
            console.log("nav_opts: ", nav_options);
            nav_options.forEach(
                e => {
                    console.log("e.datapage: ", e.dataset.page);
                    if (e.dataset.page == 'Home') {
                        e.classList = "nav-option active";

                    } else {
                        e.classList = "nav-option";
                    }

                })
        });

    </script>
</asp:Content>

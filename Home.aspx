<%@ Page Title="" Language="C#" MasterPageFile="~/TopBar.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SignalRChat.Home1" EnableEventValidation="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="Styles/Home.css">
    <style>
        .search-color{
            background:#0984e3;
            border:1px solid #0984e3;
            color:#Fff;
            margin-right: 10px;
            font-weight: normal !important;
        }

        .friend{
            width:100%;
            display:block !important;
            margin-bottom:10px;
        }

        label{
            font-weight: normal !important;
        }

        .fix-span{
            margin-right:10px;
            font-size:15px;
        }

        .triggerGroupModal{
            background: #0984e3;
            border: none;
            padding: 3px 8px;
            color:#fff;
            border-radius: 2px;
        }
        .user-avatar img{
            max-height:80px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="home-container">

        <ul class="nav nav-tabs">
          <li id="user-tab" class="home-tab active" ><a href="#">Mọi người</a></li>
          <li id="group-tab" class="home-tab"><a href="#">Nhóm</a></li>
        </ul>
        <div id="content-container">
            
            <div id="content-user">
                <div id="user-filter-container">

                    <asp:TextBox ID="txtUserSearch" runat="server"></asp:TextBox>
                    <asp:Button Text="Search" runat="server" ID="btnSearchUser" OnClick="btnSearchUser_Click" CssClass="search-color" />

                    <label class="checkbox-inline">
                        <asp:CheckBox ID="ckbFriendsRequest" runat="server" OnCheckedChanged="ckbFriendsRequest_CheckedChanged" AutoPostBack="True" /> Lời mời kết bạn
                    </label>
                    <label class="checkbox-inline">
                        <asp:CheckBox ID="ckbFriends" runat="server" OnCheckedChanged="ckbFriends_CheckedChanged" AutoPostBack="True" /> Bạn bè
                    </label>
                    <label class="checkbox-inline">
                        <asp:CheckBox ID="ckbFriendsBlock" runat="server" OnCheckedChanged="ckbFriendsBlock_CheckedChanged" AutoPostBack="True" /> Đã chặn
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
                                <div class="user-avatar">
                                    <img src="<%#GetUserAvatar64Str(Container.DataItem)%>" />
                                </div>

                                <div class="user-btn">

                                    <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem) == 0 )%>">
                                        <asp:Button CssClass="friend" Type="button"  Text="Thêm bạn" ID="test" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="AddFriendRequest" OnClick="UserBtnHandler" />
                                        <asp:Button CssClass="friend" Type="button" Text="Chặn" ID="Button1" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="BlockFriend" OnClick="UserBtnHandler" />
                                    </asp:PlaceHolder> 

                                    <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem)  == 1 )%>">
                                        <asp:Button CssClass="friend" Type="button" Text="Hủy lời mời kết bạn" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="DeleteFriendRequest" OnClick="UserBtnHandler"/>
                                    </asp:PlaceHolder> 

                                    <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem) == 2 )%>">
                                        <asp:Button CssClass="friend" Type="button" Text="Chấp nhận" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="AcceptFriendRequest" OnClick="UserBtnHandler"/>
                                        <asp:Button CssClass="friend" Type="button" Text="Xóa" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="ReverseDeleteFriendRequest" OnClick="UserBtnHandler"/>
                                    </asp:PlaceHolder>  

                                    <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem) == 3 )%>">
                                        <asp:Button CssClass="friend" Type="button" Text="Hủy kết bạn" ID="Button3" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="UnFriend" OnClick="UserBtnHandler" />
                                        <asp:Button CssClass="friend" Type="button" Text="Chặn" ID="Button2" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="BlockFriend" OnClick="UserBtnHandler" />
                                    </asp:PlaceHolder>  

                                    <asp:PlaceHolder runat="server" Visible="<%#( GetState(Container.DataItem)  == 4 )%>">
                                        <asp:Button CssClass="friend" Type="button" Text="Bỏ chặn" ID="Button4" runat="server" CommandArgument='<%#GetUserId(Container.DataItem)%>' CommandName="UnBlock" OnClick="UserBtnHandler" Enabled="True" />
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
                
                <div id="group-filter-container">
                        <asp:TextBox ID="txtGroupSearch" runat="server"></asp:TextBox>
                        <asp:Button ID="btnGroupSearch" runat="server" Text="Tìm kiếm" OnClick="btnGroupSearch_Click" CssClass="search-color" />

                        <label class="checkbox-inline" CssClass="label">
                            <asp:CheckBox ID="ckbJoinedGroup"  runat="server" AutoPostBack="True" OnCheckedChanged="ckbJoinedGroup_CheckedChanged" Text="Đã tham gia" /> 
                        </label>
                        <label class="checkbox-inline" CssClass="label">
                            <asp:CheckBox ID="ckbGroupRequest"  runat="server" AutoPostBack="True" OnCheckedChanged="ckbGroupRequest_CheckedChanged" Text="Đã gửi yêu cầu" /> 
                        </label>
                    
                </div>

                <div id="group-info-title">
                     <asp:Label ID="lblExitDetailMode" runat="server" Text="Thêm mới" CssClass="fix-span"></asp:Label>                
                    <asp:Button ID="btnExitDetailMode" runat="server" Text="+" OnClick="btnExitDetailMode_Click" class="triggerGroupModal"/>
                </div>

                <div id="list-group" >
                    <asp:Repeater ID="rptGroups" runat="server" ViewStateMode="Enabled">
                        <ItemTemplate>

                                <asp:PlaceHolder runat="server" Visible="<%#( GetGroupState(Container.DataItem) != 4 )%>">

                                    <div class="group-box">

                                        <div class="group-name">
                                            <div>
                                                <%#Eval("Name")%>
                                            </div>
                                            <div>
                                                <asp:Button Type="button"  Text="Chi tiết" ID="Button6" runat="server" CssClass="search-color" 
                                                    CommandArgument='<%#GetGroupId(Container.DataItem)%>' CommandName="DetailsOfGroup" OnClick="GroupBtnHandler" />
                                            </div>
                                        </div>

                                        <div class="group-btn">

                                            <asp:PlaceHolder runat="server" Visible="<%#( GetGroupState(Container.DataItem) == 0 )%>">
                                                <asp:Button Type="button"  Text="Tham gia" ID="Button7" runat="server" 
                                                    CommandArgument='<%#GetGroupId(Container.DataItem)%>' CommandName="AddGroupRequest" OnClick="GroupBtnHandler" />
                                            </asp:PlaceHolder>
                                            
                                            <asp:PlaceHolder runat="server" Visible="<%#( GetGroupState(Container.DataItem) == 3 )%>">
                                                <asp:Button Type="button"  Text="Hủy yêu cầu" ID="Button8" runat="server" 
                                                    CommandArgument='<%#GetGroupId(Container.DataItem)%>' CommandName="DeleteGroupRequest" OnClick="GroupBtnHandler" />

                                            </asp:PlaceHolder>

                                            <asp:PlaceHolder runat="server" Visible="<%#( GetGroupState(Container.DataItem) == 1 )%>">
                                                <asp:Button Text="OWNER" runat="server" Enabled="False" />
                                                <asp:Button Type="button"  Text="Xóa Group" ID="Button11" runat="server" 
                                                    CommandArgument='<%#GetGroupId(Container.DataItem)%>' CommandName="DeleteGroup" OnClick="GroupBtnHandler" />
                                            </asp:PlaceHolder>

                                            <asp:PlaceHolder runat="server" Visible="<%#( GetGroupState(Container.DataItem) == 2 )%>">
                                                <asp:Button Type="button"  Text="Rời nhóm" ID="Button5" runat="server" 
                                                    CommandArgument='<%#GetGroupId(Container.DataItem)%>' CommandName="RemoveGroupMember" OnClick="GroupBtnHandler" />
                                            </asp:PlaceHolder>
 
                                        </div>
                                    </div>

                                </asp:PlaceHolder>


                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <div id="group-info">
                    <div id="info-content">
                        <a class="btn btn-primary" data-toggle="collapse" href="#clsGroupMember" role="button" aria-expanded="false" aria-controls="clsGroupMember">
                            Danh sách thành viên
                        </a>
                            
                        <div class="collapse" id="clsGroupMember" style="padding:10px;">

                            <asp:Repeater ID="rptGroupMembers" runat="server" ViewStateMode="Enabled">
                                <ItemTemplate>
                                    <div style="padding:0px 0px 10px 0;">
                                        <%#Eval("Username")%>
                                    </div>
                                    <div>
                                        <asp:PlaceHolder runat="server" Visible="<%#( UTG_GetUserState(Container.DataItem) == 2 && currentUserState == 1)%>">
                                             <asp:Button Type="button"  Text="Xóa" ID="Button9" runat="server" 
                                            CommandArgument="<%#UTG_GetUserToUserArgs(Container.DataItem)%>" 
                                            CommandName="RemoveGroupMember" OnClick="UserToGroupBtnHandler" />
                                            <asp:Button Type="button"  Text="Chặn" ID="Button10" runat="server" 
                                                CommandArgument="<%#UTG_GetUserToUserArgs(Container.DataItem)%>" 
                                                CommandName="BanGroupMember" OnClick="UserToGroupBtnHandler" />   
                                        </asp:PlaceHolder>

                                        <asp:PlaceHolder runat="server" Visible="<%#( UTG_GetUserState(Container.DataItem) == 1 )%>">
                                             <button disabled>Owner</button>
                                        </asp:PlaceHolder>


                                        
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                        </div>

                        <a class="btn btn-primary" data-toggle="collapse" href="#clsGroupPendingMember" role="button" aria-expanded="false" aria-controls="clsGroupPendingMember">
                            Danh sách chờ duyệt
                        </a>
                            
                        <div class="collapse" id="clsGroupPendingMember"  style="padding:10px;">

                            <asp:Repeater ID="rptGroupPendingMembers" runat="server" ViewStateMode="Enabled">
                                <ItemTemplate>
                                    
                                    <div style="padding:0px 0px 10px 0;"">
                                        <%#Eval("Username")%>
                                    </div>
                                    <div>
                                        <asp:Button Type="button"  Text="Chấp nhận" ID="Button6" runat="server" 
                                            CommandArgument="<%#UTG_GetUserToUserArgs(Container.DataItem)%>" 
                                            CommandName="AcceptGroupRequest" OnClick="UserToGroupBtnHandler" />
                                        <asp:Button Type="button"  Text="Xóa" ID="Button9" runat="server" 
                                            CommandArgument="<%#UTG_GetUserToUserArgs(Container.DataItem)%>" 
                                            CommandName="DeleteGroupRequest" OnClick="UserToGroupBtnHandler" />
                                        <asp:Button Type="button"  Text="Chặn" ID="Button10" runat="server" 
                                            CommandArgument="<%#UTG_GetUserToUserArgs(Container.DataItem)%>" 
                                            CommandName="BanGroupMember" OnClick="UserToGroupBtnHandler" />
                                    </div>

                                </ItemTemplate>
                            </asp:Repeater>

                        </div>

                        <a class="btn btn-primary" data-toggle="collapse" href="#clsGroupBannedMember" role="button" aria-expanded="false" aria-controls="clsGroupBannedMember">
                            Danh sách đã chặn
                        </a>
                            
                        <div class="collapse" id="clsGroupBannedMember"  style="padding:10px;">

                            <asp:Repeater ID="rptGroupBannedMembers" runat="server" ViewStateMode="Enabled">
                                <ItemTemplate>
                                    
                                    <div style="padding:0px 0px 10px 0;">
                                        <%#Eval("Username")%>
                                    </div>
                                    <div>
                                        
                                        <asp:Button Type="button"  Text="Bỏ Chặn" ID="Button10" runat="server" 
                                            CommandArgument="<%#UTG_GetUserToUserArgs(Container.DataItem)%>" 
                                            CommandName="UnBanGroupMember" OnClick="UserToGroupBtnHandler" />
                                    </div>

                                </ItemTemplate>
                            </asp:Repeater>

                        </div>
                    </div>
                </div>

                 <!-- modal popup group -->

                <div class="modal-create">
                    <div class="modal-create-container">
                        <header class="modal-header">
                            <div class="modal-close">
                                <button type="button" class="btnClose">Close</button>
                                
                            </div>
                            <Span>Group</Span>
                
                        </header>

                        <div class="modal-body">
                            <label for="group-name" class="modal-label">
                            </label>
                            <!--<input id="group-name" type="text" class="modal-input" placeholder="Nhập tên group">-->
                            
                            <asp:TextBox class="modal-input" ID="txtGroupName" runat="server" placeholder="Nhập tên group"></asp:TextBox>
                            <asp:Button ID="btnCreateGroup" class="btnCreateGroup" runat="server" Text="Tạo group" OnClick="btnCreateGroup_Click" />
                            
                        </div>
                        
                    </div>  
                    
                </div>

                <!-- modal popup group -->

            </div>
        </div>
    </div>

    <script>
        let group_detail_is_on = "<%=this.ShowingDetail%>";
        if (group_detail_is_on === "True") {
            $('#info-content').show();
        } else {
            $('#info-content').hide();
            // Modal popup
            $('.triggerGroupModal').click(function (e) {
                e.preventDefault();
                $('.modal-create').css("display", "flex");
            });

            $('.btnClose').click(function (e) {
                e.preventDefault();
                $('.modal-create').css("display", "none");

            });
            $('.modal-create').click(function (e) {
                //e.preventDefault();
                //$('.modal-create').css("display", "none");
            });

        }
    </script>
    <script src="Scripts/Home/Home.js"></script>
    <script>
        if ("<%=this.currentTab%>" == "user") {
            $('#user-tab').trigger("click");
        } else {
            $('#group-tab').trigger("click");
        }
    </script>
</asp:Content>

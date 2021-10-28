<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SignalRChat.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-3.2.1.min.js"></script>
    <script src="Scripts/jquery.signalR-2.2.2.min.js"></script>
    <script src="signalr/hubs"></script>
    <script type="text/javascript">
        $(function () {
            // Declare a proxy to reference the hub.
            $.connection.hub.logging = true;
            $.connection.privateChatHub.logging = true;
            var privateChatHub = $.connection.privateChatHub;


            //registerClientMethods(privateChatHub);

            privateChatHub.client.showMessage = function (message) {
                // Html encode display name and message.
                console.log("Client-showMessage :" + message);
            };


            $.connection.hub.start().done(function () {
                let username = "<%= this.Username %>";
                privateChatHub.server.connect(username);
            });

            // Create a function that the hub can call to broadcast messages.
            
            
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="chat-container">
            <div id="list_friend">
                <asp:Repeater ID="rpListFriend" runat="server">
                    <ItemTemplate>
                        <div class="friend-box">
                            <div class="friend-name">
                                <asp:Label ID="Label1" runat="Server" Text='<%#Container.DataItem %>' />
                            </div>
                    
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">

                <ContentTemplate>
                    <div id="chat-box">

                        <asp:Repeater ID="rpChatBox" runat="server">
                            <ItemTemplate>
                                <div class="self-message">
                                    <asp:Label ID="Label1" runat="Server" Text='<%#Container.DataItem %>' />
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Button ID="btnSend" runat="server" Text="Add message" OnClick="btnSend_Click" />

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            

        </div>

        
    
    </form>


    <div id="un-form-chat">

    </div>

    <button onclick="RequestLoadBoxChat('haiiii')">
            shjet</button>
</body>
<style>
    #list_friend{
        border:1px solid blue;
        overflow-y: auto !important;
        width:150px;
        height:500px;
        display: flex;
        flex-direction:column;
    }

    #list_friend .friend-box{
        border:1px solid cyan;
        width:100%;
        height:20%;
    }
    #chat-box{
        width:500px;
        height:500px;
        border:1px solid orange;

    }       
</style>
</html>

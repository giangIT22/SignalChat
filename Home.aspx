<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SignalRChat.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-3.2.1.min.js"></script>
    <script src="Scripts/jquery.signalR-2.2.2.min.js"></script>
    <script src="signalr/hubs"></script>
    <link rel="stylesheet" href="Styles/Home.css">
</head>
<body>
    <form id="form1" runat="server">
    </form>
        
    <input id="current-receiver" type="text" value=""/>
        

        <div id="chat-container">

            <div id="chat-box">
                <div id="message-box">

                </div>
                <div id="function-bar">
                    <textarea id="ChatTextArea" cols="20" rows="1" disabled></textarea>
                    <label for="inputFile" id="inputFileLbl">
                        Upload
                         <input id='inputFile' type="file" value="" />
                    </label>
                    

                    <button id="btnSend" disabled>Gửi</button>
                </div>
            </div>

            <div id="contact_list">
                
            </div>

        </div>
    
    <img id="ItemImage" src="" width="auto" 
     height="150">

    

    <button onclick="Test()">
            shjet</button>
    <button onclick="DownLoadFile('admin2','admin','Dcmm')">
            shjet2s</button>
</body>

<script src="Scripts/Home/Home.js"></script>
<script>
    let user_name = '<%=this.Username%>';
    let gbl_file_variable = {
        name: '',
        content: '',
        type: '',
    };
</script>

</html>

<%@ Page Title="" Language="C#" MasterPageFile="~/TopBar.Master" AutoEventWireup="true" CodeBehind="ChatBox.aspx.cs" Inherits="SignalRChat.Home" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>SignalR Chat : Login</title>

    <script src="signalr/hubs"></script>
    <link rel="stylesheet" href="Styles/ChatBox.css">


    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/style.css" rel="stylesheet" />
    <link href="Content/font-awesome.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

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
                    

                    <button id="btnSend" type="button" disabled>Gửi</button>
                </div>
            </div>

            <div id="contact_list">
                
            </div>

        </div>
    
    <!--group -->
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
                    <input id="group-name" type="text" class="modal-input" placeholder="Nhập tên group">

                    <button class="btnCreate" type="button">
                        Tạo group
                    
                    </button>
                </div>
            </div>  
        </div>
            
    <!--group -->
            <div class="col-md-4">

                <div class="box box-solid box-primary">

                    <div class="box-header with-border">
                        <h3 class="box-title">Groups <span id='GroupCount'></span></h3>
                        <button id="btnCreateGroup" type="button" style="color:black;">Tạo</button>
                    </div>

                    <div class="box-footer box-comments" id="divgroup">
                        <button id="btnGroup" type="button" >Group1</button>
                        <button class="btnEdit" type="button">...</button>
                    </div>

                </div>


            </div>

            <div class="modal-edit">
                <div class="modal-edit-container">
                    <div class="modal-edit__nav">
                        <div class="modal-info__nav-title">Tên nhóm</div>
                        <button type="button" class=" btnclose">Close</button>          
                    </div>
                    <div class="box" style="width: 100%; height: 10px ; background-color: rgba(184, 188, 192, 0.623);"></div>
                    <div class="modal-edit__info">
                    <div class="modal-edit__info-title">
                        <div class="modal-edit__info-member">3 Members</div>
                        <button type="button" class=" btnAdd">Add member</button>
                    </div>
                    <div class="box" style="width: 100%; height: 10px ; background-color: rgba(184, 188, 192, 0.623);"></div>
            
                    <div class="modal-edit__info-list">
                        <span>Member 1</span>
                        <button type="button" class="btnDelete">Delete</button>
                    </div>
                </div>
            </div>
            </div>

    

    <button onclick="Test()">
            shjet</button>
    <button onclick="DownLoadFile('admin2','admin','Dcmm')">
            shjet2s</button>


    <script>
        $(function () {
            // Đổi in đậm phần navigation

            var nav_options = $('.nav-option').toArray();
            console.log("nav_opts: ", nav_options);
            nav_options.forEach(
                e => {
                    console.log("e.datapage: ", e.dataset.page);
                    if (e.dataset.page == 'ChatBox') {
                        e.classList = "nav-option active";

                    } else {
                        e.classList = "nav-option";
                    }
                    
                })
        });

    </script>
    <!-- scripp -->
    <script src="Scripts/ChatBox/ChatBox.js"></script>
    <script>

        // Global JS variable
        let user_name = '<%=this.currentUser.Username%>';
        let user_id = '<%=this.currentUser.Id%>';

        let default_file_value = {
            name: '',
            content: '',
            type: '',
        };

        let gbl_file_variable = default_file_value;
    </script>

    <script>
        // group - Toan
        //modal create
        $('#btnCreateGroup').click(function (e) {
            e.preventDefault();
            $('.modal-create').css("display", "flex");
        });
        $('.btnClose').click(function (e) {
            e.preventDefault();
            $('.modal-create').css("display", "none");
        });
        $('.modal-create').click(function (e) {
            e.preventDefault();
            $('.modal-create').css("display", "none");
        });
        // modal edit
        $('.btnEdit').click(function (e) {
            e.preventDefault();
            $('.modal-edit').css("display", "flex");
        })
        $('.btnclose').click(function (e) {
            e.preventDefault();
            $('.modal-edit').css("display", "none");
        });
        $('.modal-edit').click(function (e) {
            e.preventDefault();
            $('.modal-eidt').css("display", "none");
        });
    </script>
 
</asp:Content>



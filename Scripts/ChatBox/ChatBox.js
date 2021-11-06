
function GetSelectedContact() {
	// Lấy liên hệ đang được chọn 
	var e = $('.contact-box-selected');
	console.log("selected contact:", e);
	return e[0].dataset.userid;
}

function GetMessageContent() {
	console.log("message content: ", $('#ChatTextArea').val());
	return e = $('#ChatTextArea').val();
}

function addMessageToBoxChat(senderId, receiverId, content, fileName, fileType, fileContent) {
		console.log("AddMessageToBoxChat!!!!! ", fileName);
		console.log("fileContent: ", fileContent.length)
		let classList = "message-content";
		

		let fileBlock = '';
		if(fileName!=''){
			console.log(fileType);
			console.log(fileType.indexOf('image'));
			if(fileType.indexOf('image') != -1 ){
				// hinh anh
			   fileBlock = "<img class='message-img' src=\"data:" + fileType + ";base64," + fileContent + "\">"
			   console.log("fileBlock: ",fileBlock);
			}else{
			   // other file
			}
			fileBlock = "<div class='msg-attachment'>" + fileBlock + "</div>"
		}
		

		

		let txt = "<div class='message-container" + (user_id == senderId ? " ms-self" : "") + "'>"
		+ fileBlock 
		+"<div class='" + classList + "'>" + content + "</div></div>"

		console.log("message block: ",txt);

		$('#message-box').append(txt);
		$('#message-box').scrollTop($('#message-box')[0].scrollHeight);
}

$(function () {

	// Declare a proxy to reference the hub.
	$.connection.hub.logging = true;
	$.connection.privateChatHub.logging = true;
	let privateChatHub = $.connection.privateChatHub;


	$('#current-receiver').hide();
	$('#current-receiver').change(function(){
		let currentReceiverId = $('#current-receiver').val();
		console.log("currentReceiver:", currentReceiverId);
		privateChatHub.server.loadMessageOf(user_id,currentReceiverId);
	})

	$('#inputFile').attr('title', '');

	//registerClientMethods(privateChatHub);



	// Khởi tạo trang khi 
	privateChatHub.client.showListMessage = function (lstMessages) {
		// Html encode display name and message.
		console.log("getlist msg: ", lstMessages);
		$('#message-box').empty();
		lstMessages.forEach(e => {
			addMessageToBoxChat(e.SenderId,e.ReceiverId,e.Content,e.AttachmentName,e.AttachmentExtension,e.Attachment);
		})
	};



	privateChatHub.client.LoadMessageOf = function (username){
		console.log("LoadMessageOf: ", username);
	}

	// load danh sách các liên hệ
	privateChatHub.client.showContactsList = function (contactsArr) {
		console.log(contactsArr);
		contactsArr.forEach(e => {
			let elm = "<div class='contact-box' data-userid='" 
			+ e.Id + "' onclick=\"SelectContact(\'" + e.Id + "\')\" > " + e.Name + "</div>";
			$('#contact_list').append(elm);
		});

		// $('.contact-box').toArray().forEach(e =>{
		//     e.click = privateChatHub.client.LoadMessageOf(e.dataset.username);
		// })
 
	};

	privateChatHub.client.showMessage = function (senderId, receiverId, content, fileName, fileType, fileContent) {
		addMessageToBoxChat(senderId, receiverId, content, fileName, fileType, fileContent);
	}
	$()
	$('#btnSend').click(function () {
		console.log("send message", gbl_file_variable);
		let senderId = user_id;
		let receiverId = GetSelectedContact();
		let content = GetMessageContent();
		$('#ChatTextArea').val('');
		console.log("senderId: ", senderId);
		console.log("receiverId: ", receiverId);
		console.log("content: ", content);
		privateChatHub.server.sendPrivateMessage(senderId, receiverId, content, gbl_file_variable.name, gbl_file_variable.type, gbl_file_variable.content);
		gbl_file_variable = default_file_value;

	})


	$.connection.hub.start().done(function () {
		privateChatHub.server.connect(user_id);
		privateChatHub.server.getContactsList(user_id);
	});

	// Create a function that the hub can call to broadcast messages.

});

// Thêm 1 liên hệ vào danh sách liên hệ
function AddContactToView(username, name) {
	let e = "<div class='contact-box' data-username='" + username + "' onclick=\"SelectContact(\'" + username + "\')\" > " + name + "</div>";
	$('#contact_list').append(e);
}

// Khi lực chọn 1 liên hệ để nhắn tin
function SelectContact(UserId) {
	// able nút gửi và textarea
	// ban đầu khi chưa chọn ai thì không được nhập vào ô tin nhắn, và nút gửi, khi chọn liên hệ rồi thỉ phải enable lại
	$('#ChatTextArea').prop('disabled', false); 
	$('#btnSend').prop('disabled', false);
	$('#current-receiver').val(UserId).trigger('change');
	console.log("$('#current-receiver').val(): ", $('#current-receiver').val());

	// select các contact-box với prefix contact-box
	let contactsBoxArr = $("*[class^='contact-box']").toArray(); 

	contactsBoxArr.forEach(e => {
		if (e.dataset.userid == UserId) {
			e.classList = ['contact-box-selected'];
		} else {
			e.classList = ['contact-box'];
		}
	})
	console.log("after select:", contactsBoxArr);
}



function Test() {
	console.log("gbl_file_variable: ", gbl_file_variable);
	console.log(gbl_file_variable.length);
}

function DownLoadFile(){
	let contentType = gbl_file_variable.type;
	let base64Data = gbl_file_variable.content;
	const linkSource = `data:${contentType};base64,${base64Data}`;
	const downloadLink = document.createElement("a");
	downloadLink.href = linkSource;
	downloadLink.download = gbl_file_variable.name;
	downloadLink.click();
}

$('#inputFile').change(function () {

	console.log("input file change");
	var reader = new FileReader();
	reader.onload = function () {

		var binaryString = '';
		let bytes = new Uint8Array(reader.result);
		var len = bytes.byteLength;

		for (var i = 0; i < len; i++) {
			binaryString += String.fromCharCode(bytes[i]);
		}

		let input_file = $('#inputFile').prop('files')[0];

		gbl_file_variable = {
			name: input_file.name,
			content: btoa(binaryString),
			type: input_file.type,
		}
		console.log("gbl_file_variable : ",gbl_file_variable)
		console.log("clientSize: ",gbl_file_variable.content.length);

		// for loading image 
		// $('#ItemImage').prop('src', "data:image/png;base64," + btoa(binaryString));
	}

	reader.readAsArrayBuffer(this.files[0]);

});
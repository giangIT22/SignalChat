
function GetSelectedContact() {
	// Lấy liên hệ đang được chọn 
	var e = $('.contact-box-selected');
	//console.log("selected contact:", e);
	if (e.length == 0) {
		return {
			UserId: -1,
			IsGroup: -1
        }
    }
	return {
		UserId : e[0].dataset.userid,
		IsGroup : e[0].dataset.isgroup
	};
}
 
function GetMessageContent() {
	//console.log("message content: ", $('#ChatTextArea').val());
	return e = $('#ChatTextArea').val();
}

function addMessageToBoxChat(MessageId, senderId, senderName, receiverId, isGroup, content, fileName, fileType, fileContent) {
		console.log("AddMessageToBoxChat!!!!! ", fileName);
		//console.log("fileContent: ", fileContent.length)
		let classList = "message-content";

		let fileBlock = '';
		if(fileName!=''){
			//console.log(fileType);
			//console.log(fileType.indexOf('image'));
			if(fileType.indexOf('image') != -1 ){
				// hinh anh
				let linkSrc = "data:" + fileType + ";base64," + fileContent;
				fileBlock = "<img class='message-img' src=\"" + linkSrc+"\" onclick=\"DownLoadAttachment(\'" +linkSrc+ "\')\">"
				//console.log("fileBlock: ",fileBlock);
			}else{
			   // other file
			}
			fileBlock = "<div class='msg-attachment'>" + fileBlock + "</div>"
		}

		let txt = "";
		//console.log("isgroup: ", isGroup);

		if (current_user_id == senderId) {
			if (isGroup?.toLowerCase() == "true") {

				txt = "<div class='message-container ms-self' data-messageid='" + MessageId + "' >"
					+ fileBlock
					+ "<div class='" + classList + " group-msg'>"
					+ "<div class=\'sender-name\' >"
					+ senderName
					+ "</div>"
					+ content
					+ "<a class='delete-message' onclick='DeleteMessage(\"" + MessageId + "\")' href=\"#\" > Xóa </a>"
					+ "</div> </div>";

			} else {

				txt = "<div class='message-container ms-self'  data-messageid='" + MessageId + "' >"
					+ fileBlock
					+ `<div class="${classList}" id="delete-message" >`
					+ content
					+ "<a class='delete-message' onclick='DeleteMessage(\"" + MessageId + "\")' href=\"#\" > Xóa </a>"
					+ "</div> </div>";

			}
		} else {
			if (isGroup?.toLowerCase() == "true") {

				txt = "<div class='message-container' data-messageid='" + MessageId + "'>"
					+ fileBlock
					+ "<div class='" + classList + " group-msg'>"
					+ "<div class=\'sender-name\' >"
					+ senderName
					+ "</div>"
					+ content
					+ "</div> </div>";

			} else {

				txt = "<div class='message-container' data-messageid='" + MessageId + "'>"
					+ fileBlock
					+ "<div class='" + classList + "'>"
					+ content
					+ "</div> </div>";

			}
		}
		
	

		//console.log("message block: ",txt);

		$('#message-box').append(txt);
		$('#message-box').scrollTop($('#message-box')[0].scrollHeight);
}
function DeleteMessage(MessageId) {
	$('#delete-message').val(MessageId).trigger('change');
}

$(function () {

	// Declare a proxy to reference the hub.
	$.connection.hub.logging = true;
	$.connection.privateChatHub.logging = true;
	let privateChatHub = $.connection.privateChatHub;


	$('#current-receiver').hide();

	$('#current-receiver').change(function(){
		
		let selectedContact = GetSelectedContact();
		
		//console.log("currentReceiver:", selectedContact.UserId, selectedContact.IsGroup);
		privateChatHub.server.loadMessageOf(current_user_id, selectedContact.UserId, selectedContact.IsGroup);
	})

	$('#delete-message').hide();

	$('#delete-message').change(function () {
		let MessageId = $('#delete-message').val();
		privateChatHub.server.deleteMessage(MessageId);
	})


	$('#inputFile').attr('title', '');

	// Khởi tạo trang khi 
	privateChatHub.client.showListMessage = function (lstMessages) {
		// Html encode display name and message.
		//console.log("getlist msg: ", lstMessages);
		$('#message-box').empty();
		lstMessages.forEach(e => {
			addMessageToBoxChat(e.Id,e.SenderId,e.SenderName,e.ReceiverId,(e.GroupId == "0" ? "false" : "true" ),e.Content,e.AttachmentName,e.AttachmentExtension,e.Attachment);
		})
	};



	privateChatHub.client.LoadMessageOf = function (username){
		//console.log("LoadMessageOf: ", username);
	}

	// load danh sách các liên hệ
	privateChatHub.client.showContactsList = function (contactsArr) {
		//console.log(contactsArr);
		contactsArr.forEach(e => {
			let elm = "<div class='contact-box' "
				+ " data-userid='" + e.Id + "' "
				+ " data-isgroup='" + e.IsGroup + "' "
				+ "onclick=\"SelectContact(\'" + e.Id + "\',\'" + e.IsGroup + "\')\" > " + e.Name + "</div>";
			$('#contact_list').append(elm);
		});

	};

	privateChatHub.client.showMessage = function (MessageId, senderId, senderName, receiverId, isGroup, content, fileName, fileType, fileContent) {

		// There 2 case :
		// sender = current user => select box to push by receiverId
		// sender = other user => select box to push by senderId

		// group case
		// sender = current user / other user => only find select box by receiverId (groupId)
		

		let selectedContact = GetSelectedContact();

		//console.log("showMessage - current_user_id:", current_user_id);
		//console.log("showMessage - senderId:", senderId);
		//console.log("showMessage - receiverId:", receiverId);
		//console.log("showMessage - isGroup:", isGroup);
		//console.log("showMessage - isGroup:", isGroup=='true');
		//console.log("showMessage - selectedContact:", selectedContact);

		//console.log("type of isGroup: ", typeof isGroup);
		//console.log("type of selectedContact.IsGroup: ", typeof selectedContact.IsGroup);

		if (isGroup == 'true') {
			if (selectedContact.UserId == receiverId && selectedContact.IsGroup == 'true') {
				addMessageToBoxChat(MessageId, senderId, senderName, receiverId, isGroup, content, fileName, fileType, fileContent);
			} else {
				// push notification
            }

		} else {
			if ((selectedContact.UserId == senderId || selectedContact.UserId == receiverId) && selectedContact.IsGroup == 'false') {
				addMessageToBoxChat(MessageId, senderId, senderName, receiverId, isGroup, content, fileName, fileType, fileContent);
			} else {
				// push notification
            }
        }

		//if (
		//	(selectedContact.UserId == senderId && selectedContact.IsGroup == isGroup)
		//	|| (selectedContact.UserId == receiverId && selectedContact.IsGroup == isGroup)
		//) {
		//	addMessageToBoxChat(senderId, senderName, receiverId, isGroup, content, fileName, fileType, fileContent);
		//} else {
			
		//	console.log("push notification to: " + receiverId + " is group = " + isGroup);
		//	//$('.contact-box').toArray().forEach(e => {
		//	//	console.log(e);
		//	//	if (e.dataset.userid == senderId && e.dataset.isgroup == isGroup) {
		//	//		let notiDiv = e.querySelector('.noti');
		//	//		if (notiDiv != null) {
		//	//			notiDiv.innerHTML = Number.parseInt(notiDiv.innerHTML) + 1;
		//	//		} else {
		//	//			e.innerHTML += "<div class='noti'> 1 </div> ";
  // //                 }
		//	//		//e.innerHTML = e.innerHTML + "<div>";
  // //             }
  // //         })

  //      }
		
	}


	privateChatHub.client.removeMessage = function (MessageId) {
		//console.log("remove mes: ", MessageId);
		$('.message-container').toArray().forEach(
			e => {
				if (e.dataset.messageid == MessageId) {
					$(e).remove();
                }
            }
		)
    }

	privateChatHub.client.OnDisconnected = function () {
		location.reload();
    }
	$('#btnSend').click(function () {
		//console.log("send message", gbl_file_variable);
		let senderId = current_user_id;
		let senderName = current_user_name;
		let selectedContact = GetSelectedContact();
		let content = GetMessageContent();
		if (content !== "" || gbl_file_variable.content.length > 0 ) {
			$('#ChatTextArea').val('');
			//console.log("btn Send message")
			//console.log("senderId: ", senderId);
			//console.log("receiverId: ", selectedContact.UserId);
			//console.log("isGroup: ", selectedContact.IsGroup);
			//console.log("content: ", content);
			//console.log("isGroup: ", content);
			privateChatHub.server.sendPrivateMessage(senderId, senderName, selectedContact.UserId, selectedContact.IsGroup, content, gbl_file_variable.name, gbl_file_variable.type, gbl_file_variable.content);
			gbl_file_variable = DEFAULT_FILE_VALUE;
        }

	})


	$.connection.hub.start().done(function () {
		privateChatHub.server.connect(current_user_id);
		privateChatHub.server.getContactsList(current_user_id);
	});

	// Create a function that the hub can call to broadcast messages.

});

// Thêm 1 liên hệ vào danh sách liên hệ
function AddContactToView(userid,isgroup, name) {
	let e = "<div class='contact-box' data-userid='" + userid + "' data-isgroup='" + isgroup + "'  onclick=\"SelectContact(\'" + userid + "\',\'" + isgroup + "\')\" > " + name + "</div>";
	$('#contact_list').append(e);
}

// Khi lực chọn 1 liên hệ để nhắn tin
function SelectContact(UserId,IsGroup) {
	// able nút gửi và textarea
	// ban đầu khi chưa chọn ai thì không được nhập vào ô tin nhắn, và nút gửi, khi chọn liên hệ rồi thỉ phải enable lại
	$('#ChatTextArea').prop('disabled', false); 
	$('#btnSend').prop('disabled', false);
	//console.log("$('#current-receiver').val(): ", $('#current-receiver').val());

	// select các contact-box với prefix contact-box
	let contactsBoxArr = $("*[class^='contact-box']").toArray(); 

	contactsBoxArr.forEach(e => {
		if (e.dataset.userid == UserId && e.dataset.isgroup == IsGroup) {
			e.classList = ['contact-box-selected'];
		} else {
			e.classList = ['contact-box'];
		}
	})
	//console.log("after select:", contactsBoxArr);

	$('#current-receiver').val(UserId).trigger('change');
}


function DownLoadAttachment(linkSrc) {
	//console.log("DownLoadAttachment:", linkSrc);
	const downloadLink = document.createElement("a");
	downloadLink.href = linkSrc;
	downloadLink.download = "downloadfile";
	downloadLink.click();
}


$('#inputFile').change(function () {

	//console.log("input file change");
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
		//console.log("gbl_file_variable : ",gbl_file_variable)
		//console.log("clientSize: ",gbl_file_variable.content.length);

		// for loading image 
		// $('#ItemImage').prop('src', "data:image/png;base64," + btoa(binaryString));
	}

	reader.readAsArrayBuffer(this.files[0]);

});
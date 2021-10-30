CREATE DATABASE AshChat
go

USE AshChat
go

CREATE TABLE tblUser(
 Id int IDENTITY(1,1) PRIMARY KEY,
 Username varchar(30) not null UNIQUE ,
 Sex tinyint not null,
 Password nvarchar(200),
 Name nvarchar(100),
 Photo Varbinary(max),
 LastAccess datetime not null,
)
go

CREATE TABLE tblMessage(
 Id int IDENTITY(1,1) PRIMARY KEY,
 Sender varchar(30) not null,
 GroupId int,
 Receiver varchar(30),
 Attachment Varbinary(max),  -- lưu được tối đa 2gb
 AttachmentName varchar(100),
 AttachmentExtension varchar(30),
 Content nvarchar(500),
 LastEditTime datetime,
 CreationTime datetime,
)
go

CREATE TABLE tblGroup(
 Id int IDENTITY(1,1) PRIMARY KEY,
 Description nvarchar(500),
 Photo Varbinary(max),
 CreationTime datetime
)
go

CREATE TABLE tblGroup_User(
 Id int IDENTITY(1,1) PRIMARY KEY,
 GroupId int not null,
 UserId int not null,
 Role int not null,
 State int not null,
 CreationTime datetime
)
go


----	======================== USER ========================

create PROCEDURE GetUserByUsername
	@Username varchar(30)
AS
	select * from AshChat.dbo.tblUser where Username = @Username
go


create PROCEDURE ThemUser
-- Trả về 
--	Id == '-1' : Đã tồn tại tên đăng nhập
--	Id != '0' : thêm thành công
--
	@Username varchar(30), @Password varchar(200), @Name nvarchar(100) null,  @Sex tinyint,
	@Photo varbinary(max) null
AS
if exists (select Username from AshChat.dbo.tblUser where Username = @Username)
	begin
		select -1;
	end
else
	begin
		
		SET NOCOUNT ON;
		INSERT INTO AshChat.dbo.tblUser (Username, Password, Name, Sex, Photo ,LastAccess)
		Output inserted.Id
		VALUES(@Username, @Password, @Name, @Sex, @Photo ,CURRENT_TIMESTAMP)
		
	end

go

--	======================== MESSAGE ========================

create PROCEDURE ThemMessage
-- Trả về 
--	Id > '0' : thêm thành công
--
	@Sender varchar(30), @Receiver varchar(30), @GroupId int, 
	@Attachment varbinary(max) null, @AttachmentName varchar(100), @AttachmentExtention varchar(30),
	@Content nvarchar(500) null
AS
	begin
		
		SET NOCOUNT ON;
		INSERT INTO AshChat.dbo.tblMessage(Sender, Receiver, GroupId, Attachment, AttachmentName , AttachmentExtension, Content, LastEditTime, CreationTime)
		Output inserted.Id
		VALUES(@Sender, @Receiver, @GroupId, @Attachment, @AttachmentName, @AttachmentExtention, @Content, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
		
	end
go


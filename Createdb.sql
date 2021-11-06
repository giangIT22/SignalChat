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
 SenderId int not null,
 GroupId int,
 ReceiverId int,
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

CREATE TABLE tblUser_User(
 Id int IDENTITY(1,1) PRIMARY KEY,
 UserIdA int not null,
 UserIdB int not null,
 State int not null,
 ExecutorId int,
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
	@SenderId int, @ReceiverId int, @GroupId int, 
	@Attachment varbinary(max) null, @AttachmentName varchar(100), @AttachmentExtention varchar(30),
	@Content nvarchar(500) null
AS
	begin
		
		SET NOCOUNT ON;
		INSERT INTO AshChat.dbo.tblMessage(SenderId, ReceiverId, GroupId, Attachment, AttachmentName , AttachmentExtension, Content, LastEditTime, CreationTime)
		Output inserted.Id
		VALUES(@SenderId, @ReceiverId, @GroupId, @Attachment, @AttachmentName, @AttachmentExtention, @Content, CURRENT_TIMESTAMP, CURRENT_TIMESTAMP)
		
	end
go


--	======================== USES_USER ========================


create PROCEDURE GetUser_User
	@UserId int
AS
	select Id as UserId, tblUser.UserName, tblUser.Name, Case when X.State is null then 0 else X.State end as State, X.CreationTime
	from tblUser
	left join (select UserIdB,State,CreationTime from tblUser_User where tblUser_User.UserIdA = @UserId) as x 
		on tblUser.Id = x.UserIdB
	where Id != @UserId;
go

exec GetUser_User 4


create PROCEDURE AddFriendRequest
	@SenderId int, @ReceiverId int
AS
	-- @SenderId/@ReceiverId không tồn tại => 0

	if not exists (select Id from AshChat.dbo.tblUser where Id = @SenderId) or not exists (select Id from AshChat.dbo.tblUser where Id = @ReceiverId)
		begin
			select 0;
		end
	else
		begin
			if exists ( select Id from AshChat.dbo.tblUser_User where UserIdA = @SenderId and UserIdB = @ReceiverId)
				begin
					select -1;
				end
			else
				begin
					SET NOCOUNT ON;
					insert into AshChat.dbo.tblUser_User (UserIdA, UserIdB, State, CreationTime)
					values
						(@SenderId,@ReceiverId,1,CURRENT_TIMESTAMP),
						(@ReceiverId,@SenderId,2,CURRENT_TIMESTAMP);
					select @@ROWCOUNT
				end
		end
go

alter PROCEDURE AcceptFriendRequest
	@SenderId int, @ReceiverId int
AS
	-- @SenderId/@ReceiverId không tồn tại => 0

	if not exists ( select Id from AshChat.dbo.tblUser_User where UserIdA = @SenderId and UserIdB = @ReceiverId and State = 2)
		begin
			select -1;
		end
	else
		begin
			SET NOCOUNT ON;
			update AshChat.dbo.tblUser_User
			set State = 3
			where (UserIdA = @SenderId and UserIdB = @ReceiverId) or (UserIdA = @ReceiverId and UserIdB = @SenderId)
			select @@ROWCOUNT
		end
		
go

create PROCEDURE DeleteFriendRequest
	@SenderId int, @ReceiverId int
AS
	-- @SenderId/@ReceiverId không tồn tại => 0
	-- Không tồn tại FriendRequest => -1

	if not exists (select Id from AshChat.dbo.tblUser where Id = @SenderId) or not exists (select Id from AshChat.dbo.tblUser where Id = @ReceiverId)
		begin
			select 0;
		end
	else
		begin
			if not exists ( 
					select Id from AshChat.dbo.tblUser_User 
						where (UserIdA = @SenderId and UserIdB = @ReceiverId and State = 1)
								or
								(UserIdA = @ReceiverId and UserIdB = @SenderId and State = 1)
					)
				begin
					select -1;
				end
			else
				begin
					SET NOCOUNT ON;
					delete  AshChat.dbo.tblUser_User
					
					where (UserIdA = @SenderId and UserIdB = @ReceiverId)
								or
							(UserIdA = @ReceiverId and UserIdB = @SenderId)
					select @@ROWCOUNT
				end
		end
go



create PROCEDURE BlockUser_User
	@SenderId int, @ReceiverId int
AS
	-- @SenderId/@ReceiverId không tồn tại => 0

	if not exists (select Id from AshChat.dbo.tblUser where Id = @SenderId) or not exists (select Id from AshChat.dbo.tblUser where Id = @ReceiverId)
		begin
			select 0;
		end
	else
		begin
			if exists ( select Id from AshChat.dbo.tblUser_User where UserIdA = @ReceiverId and UserIdB = @SenderId and State = 4)
				begin
					-- @ReceiverId đã chặn @SenderId rồi
					select -1;
				end
			else
				begin
					if exists ( select Id from AshChat.dbo.tblUser_User where UserIdA = @SenderId and UserIdB = @ReceiverId)
						begin

							DECLARE @CNT INT
							set @CNT = 0;

							update AshChat.dbo.tblUser_User
							set State = 4
							where UserIdA = @SenderId and UserIdB = @ReceiverId

							set @CNT = @CNT + @@ROWCOUNT

							update AshChat.dbo.tblUser_User
							set State = 5
							where UserIdA = @ReceiverId and UserIdB = @SenderId

							set @CNT = @CNT + @@ROWCOUNT

							select @CNT
						end
					else
						begin
							insert into AshChat.dbo.tblUser_User (UserIdA, UserIdB, State, CreationTime)
							values 
								(@SenderId,@ReceiverId,4,CURRENT_TIMESTAMP),
								(@ReceiverId,@SenderId,5,CURRENT_TIMESTAMP);
							select @@ROWCOUNT
						end
				end
		end
go


create PROCEDURE UnBlockUser_User
	@SenderId int, @ReceiverId int
AS
	-- @SenderId/@ReceiverId không tồn tại cặp user đang block => 0

	if not exists (select Id from AshChat.dbo.tblUser_User where UserIdA = @SenderId and UserIdB = @ReceiverId and State = 4)
		begin
			select 0;
		end
	else
		begin
			delete from AshChat.dbo.tblUser_User
			where ( UserIdA = @SenderId and UserIdB = @ReceiverId ) or ( UserIdB = @SenderId and UserIdA = @ReceiverId )
			select @@ROWCOUNT
		end
go


create PROCEDURE UnFriendUser_User
	@SenderId int, @ReceiverId int
AS
	-- @SenderId/@ReceiverId không tồn tại cặp user đang block => 0

	if not exists (select Id from AshChat.dbo.tblUser_User where UserIdA = @SenderId and UserIdB = @ReceiverId and State = 3)
		begin
			select 0;
		end
	else
		begin
			delete from AshChat.dbo.tblUser_User
			where ( UserIdA = @SenderId and UserIdB = @ReceiverId ) or ( UserIdB = @SenderId and UserIdA = @ReceiverId )
			select @@ROWCOUNT
		end
go


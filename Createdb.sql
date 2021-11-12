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
 Name nvarchar(30) null,
 Description nvarchar(500),
 Photo Varbinary(max),
 CreationTime datetime
)
go

CREATE TABLE tblGroup_User(
 GroupId int not null,
 UserId int not null,
 State int not null,
 CreationTime datetime,
 primary key (GroupId,UserId)
)
go

CREATE TABLE tblUser_User(
 UserIdA int not null,
 UserIdB int not null,
 State int not null,
 CreationTime datetime,
 primary key(UserIdA, UserIdB)
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
create PROCEDURE GetFriends
	@UserId int
AS
	select tblUser.ID, tblUser.Username, tblUser.Sex, tblUser.Name, tblUser.Photo from tblUser_User 
	join tblUser on tblUser_User.UserIdB = tblUser.Id
	where UserIdA = @UserId and State = 3
go

create PROCEDURE GetUser_User
	@UserId int
AS
	select Id as UserId, tblUser.UserName, tblUser.Name, Case when X.State is null then 0 else X.State end as State, X.CreationTime
	from tblUser
	left join (select UserIdB,State,CreationTime from tblUser_User where tblUser_User.UserIdA = @UserId) as x 
		on tblUser.Id = x.UserIdB
	where Id != @UserId;
go

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
			if exists ( select * from AshChat.dbo.tblUser_User where UserIdA = @SenderId and UserIdB = @ReceiverId)
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

create PROCEDURE AcceptFriendRequest
	@SenderId int, @ReceiverId int
AS
	-- @SenderId/@ReceiverId không tồn tại => 0

	if not exists ( select * from AshChat.dbo.tblUser_User where UserIdA = @SenderId and UserIdB = @ReceiverId and State = 2)
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
					select * from AshChat.dbo.tblUser_User 
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
			if exists ( select * from AshChat.dbo.tblUser_User where UserIdA = @ReceiverId and UserIdB = @SenderId and State = 4)
				begin
					-- @ReceiverId đã chặn @SenderId rồi
					select -1;
				end
			else
				begin
					if exists ( select * from AshChat.dbo.tblUser_User where UserIdA = @SenderId and UserIdB = @ReceiverId)
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

	if not exists (select * from AshChat.dbo.tblUser_User where UserIdA = @SenderId and UserIdB = @ReceiverId and State = 4)
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

	if not exists (select * from AshChat.dbo.tblUser_User where UserIdA = @SenderId and UserIdB = @ReceiverId and State = 3)
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

--	======================== GROUP ========================
-- State: 1 - owner, 2 - member, 3 - Request , 4 - Banned


-- Thêm Group
create procedure AddGroup
	@SenderId int, @Name nvarchar(30), @Description nvarchar(500), 
	@Photo varbinary(max)
as
	DECLARE @CNT INT
	set @CNT = 0;
	insert into tblGroup(Name, Description, Photo, CreationTime)
	values (@Name, @Description, @Photo, CURRENT_TIMESTAMP);

	declare @InsertedId int;
	set @InsertedId = SCOPE_IDENTITY();
	
	set @CNT = @CNT + @@ROWCOUNT
	
	if(@InsertedId > 0 )
		begin
			insert into tblGroup_User(GroupId, UserId, State, CreationTime)
			values (@InsertedId, @SenderId, 1, CURRENT_TIMESTAMP);
			set @CNT = @CNT + @@ROWCOUNT

			select @CNT
		end
	else
		begin
			select -1;
		end

go

-- Get Group
create procedure GetGroupById
	@GroupId int, @SenderId int
as
	if not exists (select top 1 * from tblGroup_User where GroupId = @GroupId and UserId = @SenderId and State = 4)
		begin
			select * from tblGroup
				join (select GroupId, Count(UserId) as CountMembers from tblGroup_User where State = 2 or State = 1 group by GroupId ) as X 
					on tblGroup.Id = X.GroupId
			where Id = @GroupId
		end
go

-- Check quyền (State) : 1 - owner, 2 - member, 3 - Request , $ 4 - Banned, 0 - none
create procedure GroupStateOf 
	@GroupId int, @UserId int
as
	declare @state int
	set @state = (select State from tblGroup_User where UserId = @UserId and GroupId = @GroupId)
	if( @state is null )
		begin
			return 0
		end
	else
		begin
			return @state
		end
	
go

-- [User] Gửi Request vào group
create procedure AddGroupRequest 
	@GroupId int, @UserId int
as
	if not exists(select * from AshChat.dbo.tblGroup_User where GroupId = @GroupId and UserId = @UserId)
		begin
			insert into AshChat.dbo.tblGroup_User (GroupId, UserId, State, CreationTime)
			values (@GroupId, @UserId, 3, CURRENT_TIMESTAMP)
			select @@ROWCOUNT
		end
	else
		begin
			select -1
		end

go

-- [Group owner/Requester] Xóa Request vào group
create procedure DeleteGroupRequest 
	@GroupId int, @SenderId int, @TargetUserId int
	-- Invalid Input : -1
	-- Success : 1

as
	
	declare @SenderState int, @TargetUserState int;
	
	exec @SenderState = GroupStateOf @GroupId, @SenderID;
	exec @TargetUserState = GroupStateOf @GroupId, @TargetUserId;

	if @SenderState != 1 and (@SenderId != @TargetUserId or @TargetUserState != 3)
		begin
			select -1
		end
	else
		begin
			delete tblGroup_User
			where GroupId = @GroupId and UserId = @TargetUserId and State = 3
			select @@ROWCOUNT
		end

go

-- [Group owner] Đồng ý Request vào group
create procedure AcceptGroupRequest 
	@GroupId int, @SenderId int, @TargetUserId int
as
	if not exists(select * from AshChat.dbo.tblGroup_User where GroupId = @GroupId and UserId = @SenderId and State = 1)
		begin
			select -2
		end
	else
		begin
			if not exists(select * from AshChat.dbo.tblGroup_User where GroupId = @GroupId and UserId = @TargetUserId and State = 3)
				begin
					select -1
				end
			else
				begin
					update tblGroup_User
					set State = 2
					where GroupId = @GroupId and UserId = @TargetUserId and State = 3
					select @@ROWCOUNT
				end
		end
go

-- [Group owner/Sender = TargetUser] Xóa member khỏi group
create procedure RemoveGroupMember
	@GroupId int, @SenderId int, @TargetUserId int
as

	declare @SenderState int, @TargetUserState int;
	
	exec @SenderState = GroupStateOf @GroupId, @SenderID;
	exec @TargetUserState = GroupStateOf @GroupId, @TargetUserId;

	if @SenderState != 1 and (@SenderId != @TargetUserId or @TargetUserState != 2)
		begin
			select -1
		end
	else
		begin
			delete tblGroup_User
			where GroupId = @GroupId and UserId = @TargetUserId and State = 2
			select @@ROWCOUNT
		end

go

-- [Group owner] Chặn member
create procedure BanGroupMember
	@GroupId int, @SenderId int, @TargetUserId int
as

	declare @SenderState int, @TargetUserState int;
	
	exec @SenderState = GroupStateOf @GroupId, @SenderID;
	exec @TargetUserState = GroupStateOf @GroupId, @TargetUserId;

	if (@SenderState != 1 or @TargetUserState = 1)
		begin
			select -2
		end
	else
		begin
			if not exists(select * from AshChat.dbo.tblGroup_User where GroupId = @GroupId and UserId = @TargetUserId)
				begin
					select -1
				end
			else
				begin
					update tblGroup_User
					set State = 4
					where GroupId = @GroupId and UserId = @TargetUserId
					select @@ROWCOUNT
				end
		end

go

-- [Group owner] Bỏ chặn member
create procedure UnBanGroupMember
	@GroupId int, @SenderId int, @TargetUserId int
as
	if not exists(select * from AshChat.dbo.tblGroup_User where GroupId = @GroupId and UserId = @SenderId and State = 1)
		begin
			select -2
		end
	else
		begin
			if not exists(select * from AshChat.dbo.tblGroup_User where GroupId = @GroupId and UserId = @TargetUserId and State = 4)
				begin
					select -1
				end
			else
				begin
					delete tblGroup_User
					where GroupId = @GroupId and UserId = @TargetUserId
					select @@ROWCOUNT
				end
		end


go

-- Lấy tất cả group kèm theo trạng thái với User truyền vào
create procedure GetGroupsToUser
	@UserId int
as
	select Id as GroupId, tblGroup.Name, tblGroup.Description, tblGroup.Photo, 
	case when X.State is null then 0 else X.State end as State, X.CreationTime
	from tblGroup
	left join (select GroupId, State, CreationTime from tblGroup_User where tblGroup_User.UserId = @UserId) as X 
		on tblGroup.Id = x.GroupId
	
go

-- Lấy danh sách user của 1 group.
-- Chỉ owner/member của group mới thực hiện được tác vụ này, không phải sẽ trả về null
create procedure GetUsersToGroup
	@UserId int, @GroupId int
as
	if exists ( select top 1 * from tblGroup_User  where UserId = @UserId and GroupID = @GroupId and (State = 1 or State = 2) )
		begin
			select GroupId, UserId, State, UserName, Name, Photo from AshChat.dbo.tblGroup_User
			join tblUser on tblGroup_User.UserId = tblUser.Id
			where GroupId = @GroupId
		end
		
go

create procedure DeleteGroup
	@GroupId int, @SenderId int
as
	declare @SenderState int;
	exec @SenderState = GroupStateOf @GroupId, @SenderID;
	if( @SenderState != 1 )
		begin	
			select -1
		end
	else
		begin
			declare @cnt int;
			set @cnt = 0;

			delete tblGroup_User
			where GroupId = @GroupId
			set @cnt = @cnt + @@ROWCOUNT

			delete tblGroup
			where Id = @GroupId
			set @cnt = @cnt + @@ROWCOUNT
			
			select @cnt
		end
go

create procedure GetGroupsOfUser
	@UserId int
as
	select 
		tblGroup.Id, tblGroup.Name, tblGroup.Description, tblGroup.Photo, tblGroup_User.State, tblGroup_User.CreationTime
	
	from tblGroup
	join tblGroup_User on tblGroup.Id = tblGroup_User.GroupId
	where UserId = @UserId and (State = 1 or State = 2)
go

create procedure GetListMessages
	@SenderId int, @ReceiverId int
as
	select 
	tblMessage.Id, tblMessage.SenderId, tblMessage.ReceiverId, tblMessage.Attachment, 
	tblMessage.AttachmentName, tblMessage.AttachmentExtension, tblMessage.Content,
	tblMessage.LastEditTime, tblMessage.CreationTime, tblMessage.GroupId,
	tblUser.Username as SenderUsername, tblUser.Name as SenderName
	from tblMessage
	join tblUser on tblMessage.SenderId = tblUser.Id
	where SenderId = @SenderId and ReceiverId = @ReceiverId
go

create procedure GetListGroupMessages
	@GroupId int
as
	select 
	tblMessage.Id, tblMessage.SenderId, tblMessage.ReceiverId, tblMessage.Attachment, 
	tblMessage.AttachmentName, tblMessage.AttachmentExtension, tblMessage.Content,
	tblMessage.LastEditTime, tblMessage.CreationTime,tblMessage.GroupId,
	tblUser.Username as SenderUsername, tblUser.Name as SenderName
	from tblMessage
	join tblUser on tblMessage.SenderId = tblUser.Id
	where GroupId = @GroupId
go
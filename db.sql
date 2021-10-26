CREATE DATABASE AppChat
GO
USE AppChat
GO
 CREATE TABLE users
 (
	id INT PRIMARY KEY IDENTITY(1,1),
	userName NVARCHAR(50),	
	password NCHAR(50)
 )

 DROP TABLE dbo.users
 go

CREATE PROC insertUser(@name NVARCHAR(50), @password NCHAR(50))
AS
BEGIN
	INSERT INTO dbo.users
        ( userName, password )
	VALUES  ( @name, -- userName - nvarchar(50)
				@password  -- password - nchar(50)
			)
END
GO
EXEC dbo.insertUser @name = N'', -- nvarchar(50)
    @password = N'' -- nchar(50)

 


CREATE TABLE message
(
	id INT PRIMARY KEY IDENTITY(1,1),
	content NTEXT,
	userName NVARCHAR(50),
	userImage VARCHAR(200),
	created_at VARCHAR(200)
)

GO
ALTER PROC addMessage (@userName NVARCHAR(50), @content NTEXT, @userImage VARCHAR(200) , @time VARCHAR(200))
AS
BEGIN
	INSERT INTO dbo.message
	VALUES  ( 
	          @content , -- content - nvarchar(200)
	          @userName, -- userName - nvarchar(50)
	          @userImage , -- userImage - varchar(200)
	          @time  -- created_at - datetime
	        )
END

GO;

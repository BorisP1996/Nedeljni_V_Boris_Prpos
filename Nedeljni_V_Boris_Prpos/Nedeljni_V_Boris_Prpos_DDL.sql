--Creating database only if database is not created yet
IF DB_ID('Nedeljni_V_Boris_Prpos_DDL') IS NULL
CREATE DATABASE Nedeljni_V_Boris_Prpos_DDL
GO
USE Nedeljni_V_Boris_Prpos_DDL;


if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblFeed')
drop table tblFeed;

if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblProfile')
drop table tblProfile;

if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblRequest')
drop table tblRequest;

if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblUser')
drop table tblUser;

if OBJECT_ID('vwUser_Profile','v') IS NOT NULL DROP VIEW vwUser_Profile;

if OBJECT_ID('vwUser_Feed','v') IS NOT NULL DROP VIEW vwUser_Feed;

if OBJECT_ID('vwUser_Request_Sending','v') IS NOT NULL DROP VIEW vwUser_Request_Sending;

if OBJECT_ID('vwUser_Request_Receiving','v') IS NOT NULL DROP VIEW vwUser_Request_Receiving;

create table tblUser (
UserID int identity(1,1) primary key,
Username nvarchar (50) unique not null,
Pasword nvarchar (50) not null 
)

create table tblRequest (
RequestID int identity(1,1) primary key,
UserID_Sending int not null,
UserID_Receiving int not null,
Approved bit
)

create table tblFeed(
FeedID int identity (1,1) primary key,
UserID int not null,
FeedContent nvarchar (100),
PublishDate date not null,
LikeNumbers int not null
)

create table tblProfile(
ProileID int identity (1,1) primary key,
UserID int not null,
About nvarchar(100) not null,
Interests nvarchar(100) not null,
Age int not null
)



Alter Table tblRequest
Add foreign key (UserID_Sending) references tblUser(UserID);

Alter Table tblRequest
Add foreign key (UserID_Receiving) references tblUser(UserID);

Alter Table tblFeed
Add foreign key (UserID) references tblUser(UserID);

Alter Table tblProfile
Add foreign key (UserID) references tblUser(UserID);


GO
CREATE VIEW vwUser_Profile AS
	SELECT	tblProfile.*,
			tblUser.Username,tblUser.Pasword
	From tblProfile,tblUser
	WHERE	tblProfile.UserID = tblUser.UserID

	GO
CREATE VIEW vwUser_Feed AS
	SELECT	tblFeed.*,
			tblUser.Username,tblUser.Pasword
	From tblFeed,tblUser
	WHERE	tblFeed.UserID = tblUser.UserID

	GO
CREATE VIEW vwUser_Request_Sending AS
	SELECT	tblRequest.*,
			tblUser.Username,tblUser.Pasword
	From tblRequest,tblUser
	WHERE	tblRequest.UserID_Sending = tblUser.UserID

		GO
CREATE VIEW vwUser_Request_Receiving AS
	SELECT	tblRequest.*,
			tblUser.Username,tblUser.Pasword
	From tblRequest,tblUser
	WHERE	tblRequest.UserID_Receiving = tblUser.UserID



	


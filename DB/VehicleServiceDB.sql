USE [VehicleService]
GO
/****** Object:  Table [dbo].[Appointment]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointment](
	[ID] [int] NOT NULL,
	[VehicleNumber] [nvarchar](20) NOT NULL,
	[CustomerName] [nvarchar](30) NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL,
	[DateTimeStart] [smalldatetime] NOT NULL,
	[DateTimeEnd] [smalldatetime] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[DateTimeText] [nvarchar](50) NULL,
	[ConfirmTime] [datetime] NULL,
	[CompleteTime] [datetime] NULL,
	[Comment] [nvarchar](200) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CustomerUser]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerUser](
	[ID] [int] NOT NULL,
	[AccountName] [nvarchar](50) NOT NULL,
	[CustomerName] [nvarchar](30) NULL,
	[VehicleNumber] [nvarchar](20) NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL,
	[Gender] [bit] NULL,
	[Birthday] [date] NULL,
	[Address] [nvarchar](256) NULL,
	[Password] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Status] [tinyint] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DrivingTest]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DrivingTest](
	[ID] [int] NOT NULL,
	[CustomerName] [nvarchar](30) NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL,
	[Date] [datetime] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[VehicleTypeId] [int] NOT NULL,
	[Comment] [nvarchar](200) NULL,
	[Status] [tinyint] NOT NULL,
	[ConfirmTime] [datetime] NULL,
	[CompleteTime] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RescueRequest]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RescueRequest](
	[ID] [int] NOT NULL,
	[Time] [datetime] NOT NULL,
	[Latitude] [float] NOT NULL,
	[Longitude] [float] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[CustomerId] [int] NOT NULL,
	[ConfirmTime] [datetime] NULL,
	[CompleteTime] [datetime] NULL,
	[Comment] [nvarchar](200) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RichMessage]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RichMessage](
	[ID] [int] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Content] [nvarchar](max) NULL,
	[PictureUrl] [nvarchar](200) NULL,
	[TimeStamp] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServiceEmployee]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceEmployee](
	[ID] [int] NOT NULL,
	[Type] [tinyint] NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[WorkNumber] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[PictureUrl] [nvarchar](200) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tenant]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tenant](
	[TenantId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DisplayName] [nvarchar](1024) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TenantMembership]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TenantMembership](
	[TenantId] [int] NOT NULL,
	[UserId] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TenantUser]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TenantUser](
	[UserId] [int] NOT NULL,
	[UserName] [nvarchar](56) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleSubType]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleSubType](
	[ID] [int] NOT NULL,
	[VehicleTypeID] [int] NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[Price] [int] NOT NULL,
	[Description] [nvarchar](200) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VehicleType]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VehicleType](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Description] [nvarchar](50) NULL,
	[PictureUrl] [nvarchar](200) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Membership](
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL,
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_OAuthMembership]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_OAuthMembership](
	[Provider] [nvarchar](30) NOT NULL,
	[ProviderUserId] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[DrivingTestView]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE VIEW [dbo].[DrivingTestView]
AS
Select DrivingTest.ID, DrivingTest.CustomerName, DrivingTest.PhoneNumber, DrivingTest.Date,
 DrivingTest.VehicleTypeId, VehicleType.Name AS VehicleTypeName, DrivingTest.CustomerId, DrivingTest.TimeStamp,
 DrivingTest.Comment, DrivingTest.Status, DrivingTest.ConfirmTime, DrivingTest.CompleteTime
 from DrivingTest, VehicleType
where DrivingTest.VehicleTypeId = VehicleType.ID

GO
/****** Object:  View [dbo].[RescueRequestView]    Script Date: 2013/10/24 20:46:59 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE VIEW [dbo].[RescueRequestView]
AS
Select RescueRequest.ID, CustomerUser.CustomerName, CustomerUser.PhoneNumber, RescueRequest.Time,
 RescueRequest.CustomerId, RescueRequest.Latitude, RescueRequest.Longitude,
 RescueRequest.Comment, RescueRequest.Status, RescueRequest.ConfirmTime, RescueRequest.CompleteTime
 from RescueRequest, CustomerUser
where  RescueRequest.CustomerId = CustomerUser.ID

GO

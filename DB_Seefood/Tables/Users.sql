USE [StoreProduct]

CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [char](20) NULL,
	[PasswordHash] [char](250) NULL,
	[Fullname] [nvarchar](100) NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[MiddleName] [nvarchar](50) NULL,
	[DisplayName] [nvarchar](100) NULL,
	[Mobile] [char](20) NULL,
	[EmailAddress] [nvarchar](max) NULL,
	[Department] [nvarchar](250) NULL,
	[Title] [nvarchar](max) NULL,
	[Company] [nvarchar](250) NULL,
	[Roles] [nvarchar](100) NULL,
	[Session] [nvarchar](max) NULL,
	[SessionId] [nvarchar](max) NULL,
	[IsAdminUser] [bit] NULL,
	[IsLocked] [bit] NULL,
	[IsDeleted] [bit] NULL,
	[DeletedAt] [datetime] NULL,
	[DeletedBy] [nvarchar](100) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdatedBy] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Users]    Script Date: 9/28/2022 3:40:18 PM ******/


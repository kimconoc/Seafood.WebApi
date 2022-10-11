USE [Seefood]
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProdPromotions](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NULL,
	[Content] [nvarchar](max) NULL,
	[PromotionMain] bit NULL,
	[Note] [nvarchar](max) NULL,
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


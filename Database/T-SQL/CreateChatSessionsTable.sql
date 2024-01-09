USE [mobileapp]
GO

/****** Object:  Table [dbo].[ChatSessions]    Script Date: 1/8/2024 9:40:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ChatSessions](
	[ChatId] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[ChatSessionName] [nvarchar](255) NULL,
	[IsGroupChat] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ChatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



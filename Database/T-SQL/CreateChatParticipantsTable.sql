USE [mobileapp]
GO

/****** Object:  Table [dbo].[ChatParticipants]    Script Date: 12/6/2023 10:55:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ChatParticipants](
	[ChatId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[JoinedAt] [datetime] NOT NULL,
	[Role] [nvarchar](50) NULL,
	[NotificationsEnabled] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ChatId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



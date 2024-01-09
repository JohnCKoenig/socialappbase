USE [mobileapp]
GO

/****** Object:  Table [dbo].[ChatParticipants]    Script Date: 1/8/2024 9:39:26 PM ******/
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
	[SessionStatus] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ChatId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ChatParticipants] ADD  DEFAULT ((0)) FOR [SessionStatus]
GO

ALTER TABLE [dbo].[ChatParticipants]  WITH CHECK ADD  CONSTRAINT [FK_ChatParticipants_ChatSessions] FOREIGN KEY([ChatId])
REFERENCES [dbo].[ChatSessions] ([ChatId])
GO

ALTER TABLE [dbo].[ChatParticipants] CHECK CONSTRAINT [FK_ChatParticipants_ChatSessions]
GO

ALTER TABLE [dbo].[ChatParticipants]  WITH CHECK ADD  CONSTRAINT [FK_ChatParticipants_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([userid])
GO

ALTER TABLE [dbo].[ChatParticipants] CHECK CONSTRAINT [FK_ChatParticipants_Users]
GO



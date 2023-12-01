USE [mobileapp]
GO

/****** Object:  Table [dbo].[RefreshTokens]    Script Date: 11/30/2023 9:55:03 PM ******/
/* SQL script to generate refresh tokens table*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RefreshTokens](
	[RefreshTokenId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Token] [nvarchar](500) NOT NULL,
	[ExpiryDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RefreshTokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RefreshTokens] ADD  DEFAULT (newid()) FOR [RefreshTokenId]
GO

ALTER TABLE [dbo].[RefreshTokens]  WITH CHECK ADD  CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([userid])
GO

ALTER TABLE [dbo].[RefreshTokens] CHECK CONSTRAINT [FK_RefreshTokens_Users]
GO



USE [mobileapp]
GO

/****** Object:  Table [dbo].[Posts]    Script Date: 11/30/2023 9:53:18 PM ******/
/* SQL script to generate posts table*/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Posts](
	[postid] [uniqueidentifier] NOT NULL,
	[userid] [uniqueidentifier] NOT NULL,
	[postlocation] [nvarchar](100) NULL,
	[postdatetime] [datetime] NULL,
	[posttext] [nvarchar](max) NULL,
	[postimage] [varbinary](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[postid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Posts] ADD  DEFAULT (newid()) FOR [postid]
GO

ALTER TABLE [dbo].[Posts]  WITH CHECK ADD FOREIGN KEY([userid])
REFERENCES [dbo].[Users] ([userid])
GO



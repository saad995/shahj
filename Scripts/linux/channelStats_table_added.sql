USE [ShahjTask]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ChannelStats](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ChannelIdentifier] [varchar](100) NOT NULL,
	[ChannelTitle] [varchar](400) NOT NULL,
	[ChannelSubscribers] [bigint] NOT NULL,
	[ChannelViewCount] [bigint] NOT NULL
 CONSTRAINT [PK_ChannelStats] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



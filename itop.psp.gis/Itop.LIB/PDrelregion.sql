USE [itoppsp]
GO
/****** 对象:  Table [dbo].[PDrelregion]    脚本日期: 12/12/2011 11:33:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PDrelregion](
	[ID] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[AreaName] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[PeopleSum] [int] NULL,
	[Year] [int] NULL,
	[Title] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[ProjectID] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[S1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[S2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[S3] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[S4] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
 CONSTRAINT [PK_PDrelregion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

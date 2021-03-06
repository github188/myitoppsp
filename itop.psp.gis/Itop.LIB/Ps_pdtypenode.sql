USE [itop_dwgh]
GO
/****** 对象:  Table [dbo].[Ps_pdtypenode]    脚本日期: 04/18/2012 14:10:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ps_pdtypenode](
	[ID] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ParentID] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[DeviceID] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[pdreltypeid] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[devicetype] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[title] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[Code] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[S1] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[S2] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NULL,
	[D1] [float] NULL,
	[D2] [float] NULL,
 CONSTRAINT [PK_Ps_pdtypenode] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

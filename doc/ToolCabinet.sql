USE [ToolCabinet]
GO
/****** Object:  Table [dbo].[Tool]    Script Date: 2019/1/17 18:22:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tool](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ToolName] [nvarchar](50) NOT NULL,
	[ToolTypeId] [int] NOT NULL,
	[ToolBarCode] [nvarchar](100) NOT NULL,
	[ToolRFIDCode] [nvarchar](100) NOT NULL,
	[RangeMin] [decimal](9, 2) NOT NULL,
	[RangeMax] [decimal](9, 2) NOT NULL,
	[Position] [int] NOT NULL,
	[Status] [bit] NOT NULL,
	[Factory] [nvarchar](50) NOT NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[Note] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Tool] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ToolRecord]    Script Date: 2019/1/17 18:22:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToolRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ToolId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[BorrowDate] [datetime] NOT NULL,
	[ReturnDate] [datetime] NULL,
	[IsReturn] [bit] NOT NULL,
	[CreateDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ToolRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ToolType]    Script Date: 2019/1/17 18:22:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ToolType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [nvarchar](20) NOT NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[Note] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_ToolType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 2019/1/17 18:22:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] NOT NULL,
	[LoginName] [varchar](20) NOT NULL,
	[RealName] [nvarchar](20) NOT NULL,
	[UserID] [varchar](36) NOT NULL,
	[CompanyID] [int] NOT NULL,
	[Password] [varchar](36) NOT NULL,
	[Status] [tinyint] NOT NULL,
	[Face] [nvarchar](max) NULL,
	[Token] [nvarchar](max) NULL,
	[Nfc] [nvarchar](max) NULL,
	[CreateDateTime] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[UserImg] [varchar](100) NULL,
 CONSTRAINT [PK_dbo.Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Tool] ON 

INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (1, N'', 14, N'1', N'1', CAST(1.00 AS Decimal(9, 2)), CAST(1.00 AS Decimal(9, 2)), 1, 1, N'Atlas', CAST(0x0000A9CA0101A5CC AS DateTime), N'Atlas1')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (2, N'', 14, N'2', N'2', CAST(2.00 AS Decimal(9, 2)), CAST(2.00 AS Decimal(9, 2)), 2, 1, N'Atlas', CAST(0x0000A9CA0106BD19 AS DateTime), N'Atlas2')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (3, N'', 14, N'3', N'3', CAST(3.00 AS Decimal(9, 2)), CAST(3.00 AS Decimal(9, 2)), 3, 1, N'Atlas', CAST(0x0000A9CB00BE84CC AS DateTime), N'Atlas3')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (4, N'', 14, N'4', N'4', CAST(4.00 AS Decimal(9, 2)), CAST(4.00 AS Decimal(9, 2)), 4, 1, N'Atlas', CAST(0x0000A9CB00BE9017 AS DateTime), N'Atlas4')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (5, N'', 14, N'5', N'5', CAST(5.00 AS Decimal(9, 2)), CAST(5.00 AS Decimal(9, 2)), 5, 1, N'Atlas', CAST(0x0000A9CB00BE9638 AS DateTime), N'Atlas5')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (6, N'', 14, N'6', N'6', CAST(6.00 AS Decimal(9, 2)), CAST(6.00 AS Decimal(9, 2)), 6, 1, N'Atlas', CAST(0x0000A9CB00BE9BD7 AS DateTime), N'Atlas6')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (7, N'', 14, N'7', N'7', CAST(7.00 AS Decimal(9, 2)), CAST(7.00 AS Decimal(9, 2)), 7, 1, N'Atlas', CAST(0x0000A9CB00BEA0EB AS DateTime), N'Atlas7')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (8, N'', 14, N'8', N'8', CAST(8.00 AS Decimal(9, 2)), CAST(8.00 AS Decimal(9, 2)), 8, 1, N'Atlas', CAST(0x0000A9CB00BEA5F7 AS DateTime), N'Atlas8')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (9, N'', 13, N'9', N'9', CAST(9.00 AS Decimal(9, 2)), CAST(9.00 AS Decimal(9, 2)), 9, 1, N'马头', CAST(0x0000A9CB00BEAABC AS DateTime), N'Desoutter1')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (10, N'', 13, N'10', N'10', CAST(10.00 AS Decimal(9, 2)), CAST(10.00 AS Decimal(9, 2)), 10, 1, N'马头', CAST(0x0000A9CB00BEB30C AS DateTime), N'Desoutter2')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (11, N'', 13, N'11', N'11', CAST(11.00 AS Decimal(9, 2)), CAST(11.00 AS Decimal(9, 2)), 11, 1, N'马头', CAST(0x0000A9CB00BEB995 AS DateTime), N'Desoutter3')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (12, N'', 13, N'12', N'12', CAST(12.00 AS Decimal(9, 2)), CAST(12.00 AS Decimal(9, 2)), 12, 1, N'马头', CAST(0x0000A9CB00BEC0AE AS DateTime), N'Desoutter4')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (13, N'', 13, N'13', N'13', CAST(13.00 AS Decimal(9, 2)), CAST(13.00 AS Decimal(9, 2)), 13, 1, N'马头', CAST(0x0000A9CB00BECA4C AS DateTime), N'Desoutter5')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (14, N'', 13, N'14', N'14', CAST(14.00 AS Decimal(9, 2)), CAST(14.00 AS Decimal(9, 2)), 14, 1, N'马头', CAST(0x0000A9CB00BED450 AS DateTime), N'Desoutter6')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (15, N'', 13, N'15', N'15', CAST(15.00 AS Decimal(9, 2)), CAST(15.00 AS Decimal(9, 2)), 15, 1, N'马头', CAST(0x0000A9CB00BEDABB AS DateTime), N'Desoutter7')
INSERT [dbo].[Tool] ([id], [ToolName], [ToolTypeId], [ToolBarCode], [ToolRFIDCode], [RangeMin], [RangeMax], [Position], [Status], [Factory], [CreateDateTime], [Note]) VALUES (16, N'', 13, N'16', N'16', CAST(16.00 AS Decimal(9, 2)), CAST(16.00 AS Decimal(9, 2)), 16, 1, N'马头', CAST(0x0000A9CB00BEE51F AS DateTime), N'Desoutter8')
SET IDENTITY_INSERT [dbo].[Tool] OFF
SET IDENTITY_INSERT [dbo].[ToolRecord] ON 

INSERT [dbo].[ToolRecord] ([Id], [ToolId], [UserId], [BorrowDate], [ReturnDate], [IsReturn], [CreateDateTime]) VALUES (1, 1, 1009, CAST(0x0000A9CB00ED67C8 AS DateTime), CAST(0x0000A9D20106DFC3 AS DateTime), 1, CAST(0x0000A9CB00ED67C8 AS DateTime))
INSERT [dbo].[ToolRecord] ([Id], [ToolId], [UserId], [BorrowDate], [ReturnDate], [IsReturn], [CreateDateTime]) VALUES (2, 1, 1009, CAST(0x0000A9D701512874 AS DateTime), CAST(0x0000A9D70155743B AS DateTime), 1, CAST(0x0000A9D701512874 AS DateTime))
INSERT [dbo].[ToolRecord] ([Id], [ToolId], [UserId], [BorrowDate], [ReturnDate], [IsReturn], [CreateDateTime]) VALUES (8, 1, 1009, CAST(0x0000A9D701584BDC AS DateTime), CAST(0x0000A9D70158A88F AS DateTime), 1, CAST(0x0000A9D701584BDC AS DateTime))
INSERT [dbo].[ToolRecord] ([Id], [ToolId], [UserId], [BorrowDate], [ReturnDate], [IsReturn], [CreateDateTime]) VALUES (9, 1, 1009, CAST(0x0000A9D800DD21F6 AS DateTime), CAST(0x0000A9D801046DA8 AS DateTime), 1, CAST(0x0000A9D800DD21F6 AS DateTime))
INSERT [dbo].[ToolRecord] ([Id], [ToolId], [UserId], [BorrowDate], [ReturnDate], [IsReturn], [CreateDateTime]) VALUES (10, 4, 2, CAST(0x0000A9D800DEECA7 AS DateTime), CAST(0x0000A9D800E07FF7 AS DateTime), 1, CAST(0x0000A9D800DEECA7 AS DateTime))
INSERT [dbo].[ToolRecord] ([Id], [ToolId], [UserId], [BorrowDate], [ReturnDate], [IsReturn], [CreateDateTime]) VALUES (1002, 1, 1009, CAST(0x0000A9D8010F4A6F AS DateTime), CAST(0x0000A9D8010F86DE AS DateTime), 1, CAST(0x0000A9D8010F4A6F AS DateTime))
INSERT [dbo].[ToolRecord] ([Id], [ToolId], [UserId], [BorrowDate], [ReturnDate], [IsReturn], [CreateDateTime]) VALUES (1003, 1, 2, CAST(0x0000A9D80110AD79 AS DateTime), CAST(0x0000A9D80110CF85 AS DateTime), 1, CAST(0x0000A9D80110AD79 AS DateTime))
SET IDENTITY_INSERT [dbo].[ToolRecord] OFF
SET IDENTITY_INSERT [dbo].[ToolType] ON 

INSERT [dbo].[ToolType] ([id], [TypeName], [CreateDateTime], [Note]) VALUES (13, N'Desoutter扳手', CAST(0x0000A9CA010092D7 AS DateTime), N'马头扳手')
INSERT [dbo].[ToolType] ([id], [TypeName], [CreateDateTime], [Note]) VALUES (14, N'Atlas扳手', CAST(0x0000A9CA0100A0DE AS DateTime), N'阿特拉斯扳手')
SET IDENTITY_INSERT [dbo].[ToolType] OFF
INSERT [dbo].[Users] ([Id], [LoginName], [RealName], [UserID], [CompanyID], [Password], [Status], [Face], [Token], [Nfc], [CreateDateTime], [IsDeleted], [UserImg]) VALUES (1, N'jucheap', N'超级管理员', N'123456789', 1, N'376c43878878ac04e05946ec1dd7a55f', 2, NULL, NULL, NULL, CAST(0x0000A48B01812ABC AS DateTime), 0, NULL)
INSERT [dbo].[Users] ([Id], [LoginName], [RealName], [UserID], [CompanyID], [Password], [Status], [Face], [Token], [Nfc], [CreateDateTime], [IsDeleted], [UserImg]) VALUES (2, N'admin', N'管理员', N'543243510', 1, N'376c43878878ac04e05946ec1dd7a55f', 2, NULL, NULL, NULL, CAST(0x0000A48B01812ABC AS DateTime), 0, NULL)
INSERT [dbo].[Users] ([Id], [LoginName], [RealName], [UserID], [CompanyID], [Password], [Status], [Face], [Token], [Nfc], [CreateDateTime], [IsDeleted], [UserImg]) VALUES (3, N'wangzk', N'wangzk', N'A66CFA49', 1, N'e10adc3949ba59abbe56e057f20f883e', 2, NULL, N'b3ec049e-aed2-4558-8e26-79518e23f0f2', NULL, CAST(0x0000A9C400C7C53F AS DateTime), 0, N'2b67e620-8d48-4281-98bb-b4db0f6d5943.jpg')
INSERT [dbo].[Users] ([Id], [LoginName], [RealName], [UserID], [CompanyID], [Password], [Status], [Face], [Token], [Nfc], [CreateDateTime], [IsDeleted], [UserImg]) VALUES (4, N'11111', N'121212', N'1231', 1, N'96e79218965eb72c92a549dd5a330112', 1, NULL, NULL, NULL, CAST(0x0000A97500A2ACA6 AS DateTime), 0, NULL)
INSERT [dbo].[Users] ([Id], [LoginName], [RealName], [UserID], [CompanyID], [Password], [Status], [Face], [Token], [Nfc], [CreateDateTime], [IsDeleted], [UserImg]) VALUES (5, N'33333', N'343333', N'2612867609', 1, N'e10adc3949ba59abbe56e057f20f883e', 2, NULL, NULL, NULL, CAST(0x0000A9C40094F57B AS DateTime), 0, N'dcc73d64-371e-4519-803c-b47c667009b8.jpg')
INSERT [dbo].[Users] ([Id], [LoginName], [RealName], [UserID], [CompanyID], [Password], [Status], [Face], [Token], [Nfc], [CreateDateTime], [IsDeleted], [UserImg]) VALUES (6, N'ttttttttttttt', N'333', N'222', 1, N'e10adc3949ba59abbe56e057f20f883e', 1, NULL, NULL, NULL, CAST(0x0000A91501148051 AS DateTime), 0, NULL)
INSERT [dbo].[Users] ([Id], [LoginName], [RealName], [UserID], [CompanyID], [Password], [Status], [Face], [Token], [Nfc], [CreateDateTime], [IsDeleted], [UserImg]) VALUES (7, N'gulingling', N'gll', N'123', 1, N'e10adc3949ba59abbe56e057f20f883e', 2, NULL, NULL, NULL, CAST(0x0000A9C100BE512E AS DateTime), 0, N'26fc2c6d-ed59-43be-8938-993e503b8d30.jpg')
INSERT [dbo].[Users] ([Id], [LoginName], [RealName], [UserID], [CompanyID], [Password], [Status], [Face], [Token], [Nfc], [CreateDateTime], [IsDeleted], [UserImg]) VALUES (1009, N'123123', N'123123', N'123123', 1, N'4297f44b13955235245b2497399d7a93', 2, NULL, NULL, NULL, CAST(0x0000A9CB00E2FAD5 AS DateTime), 0, N'5a4905b4-95ab-470f-b6fe-ce6b37a8a32c.jpg')
ALTER TABLE [dbo].[Tool] ADD  CONSTRAINT [DF_Tool_CreateDateTime]  DEFAULT (getdate()) FOR [CreateDateTime]
GO
ALTER TABLE [dbo].[ToolType] ADD  CONSTRAINT [DF_ToolType_CreateDateTime]  DEFAULT (getdate()) FOR [CreateDateTime]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工具名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'ToolName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工具类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'ToolTypeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'条码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'ToolBarCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'RFID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'ToolRFIDCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工位号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'Position'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0=在库，1=借出，2=损坏' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tool', @level2type=N'COLUMN',@level2name=N'CreateDateTime'
GO

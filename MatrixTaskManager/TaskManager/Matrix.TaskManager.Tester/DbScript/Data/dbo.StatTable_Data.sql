SET IDENTITY_INSERT [dbo].[StatTable] ON
INSERT INTO [dbo].[StatTable] ([f1], [f2], [f3], [f4]) VALUES (5, N'vv        ', NULL, NULL)
SET IDENTITY_INSERT [dbo].[StatTable] OFF
SET IDENTITY_INSERT [dbo].[StatTable] ON
INSERT INTO [dbo].[StatTable] ([f1], [f2], [f3], [f4]) VALUES (1, N'aaaa      ', NULL, NULL)
INSERT INTO [dbo].[StatTable] ([f1], [f2], [f3], [f4]) VALUES (2, N'aaaa      ', NULL, NULL)
INSERT INTO [dbo].[StatTable] ([f1], [f2], [f3], [f4]) VALUES (3, N'aaaa      ', NULL, NULL)
INSERT INTO [dbo].[StatTable] ([f1], [f2], [f3], [f4]) VALUES (4, N'aaaa      ', NULL, NULL)
SET IDENTITY_INSERT [dbo].[StatTable] OFF

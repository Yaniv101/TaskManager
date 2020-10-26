SET IDENTITY_INSERT [dbo].[StatTable] ON
INSERT INTO [dbo].[StatTable] ([f1], [f2], [f3], [f4], [ggg]) VALUES (1, N'aaaa      ', NULL, NULL, NULL)
INSERT INTO [dbo].[StatTable] ([f1], [f2], [f3], [f4], [ggg]) VALUES (2, N'aaaa      ', NULL, NULL, NULL)
INSERT INTO [dbo].[StatTable] ([f1], [f2], [f3], [f4], [ggg]) VALUES (3, N'aaaa      ', NULL, NULL, NULL)
INSERT INTO [dbo].[StatTable] ([f1], [f2], [f3], [f4], [ggg]) VALUES (4, N'aaaa      ', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[StatTable] OFF
SET IDENTITY_INSERT [dbo].[StatTable] ON
INSERT INTO [dbo].[StatTable] ([f1], [f2], [f3], [f4], [ggg]) VALUES (5, N'ttt       ', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[StatTable] OFF

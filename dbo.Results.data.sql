SET IDENTITY_INSERT [dbo].[Results] ON
INSERT INTO [dbo].[Results] ([ResultID], [Component], [Value], [UOM], [SampleID]) VALUES (1, N'test', 1, N'gram', 1)
INSERT INTO [dbo].[Results] ([ResultID], [Component], [Value], [UOM], [SampleID]) VALUES (2, N'test', 1, N'gram', 2)
INSERT INTO [dbo].[Results] ([ResultID], [Component], [Value], [UOM], [SampleID]) VALUES (3, N'test', 1, N'gram', 2)
SET IDENTITY_INSERT [dbo].[Results] OFF

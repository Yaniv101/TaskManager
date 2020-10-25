CREATE TABLE [dbo].[StatTable]
(
[f1] [int] NOT NULL IDENTITY(1, 1),
[f2] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[f3] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StatTable] ADD CONSTRAINT [PK_StatTable] PRIMARY KEY CLUSTERED  ([f1]) ON [PRIMARY]
GO

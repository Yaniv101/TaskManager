CREATE TABLE [dbo].[Table_1]
(
[f1] [int] NOT NULL,
[f2] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[f3] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Table_1] ADD CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED  ([f1]) ON [PRIMARY]
GO

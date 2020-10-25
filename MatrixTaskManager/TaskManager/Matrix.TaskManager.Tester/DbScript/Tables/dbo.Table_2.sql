CREATE TABLE [dbo].[Table_2]
(
[f11] [int] NULL,
[f22] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[f33] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[f44] [nchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create TRIGGER [dbo].[TrackTable2Changes]
   ON [dbo].[Table_2]    AFTER UPDATE
AS 
 
INSERT INTO [dbo].Table_2_his
           (
		    F11,f22,f33,f44,LastUpdate
			)

		   select 
		   
		   F11,f22,f33,f44
		   ,GETDATE()
		   from inserted
GO

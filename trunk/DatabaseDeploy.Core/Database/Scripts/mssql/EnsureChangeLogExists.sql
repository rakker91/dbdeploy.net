IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[$(ChangeLog)]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[$(ChangeLog)](
		[change_number] [int] NOT NULL,
		[complete_dt] [datetime] NULL,
		[applied_by] [varchar](100) NOT NULL,
		[description] [varchar](500) NOT NULL,
	 CONSTRAINT [Pk$(ChangeLog)] PRIMARY KEY CLUSTERED 
	(
		[change_number] ASC
	)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
	)
	END
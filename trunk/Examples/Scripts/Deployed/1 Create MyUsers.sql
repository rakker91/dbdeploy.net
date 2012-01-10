/****** Object:  Table [dbo].[MyUsers]    Script Date: 01/10/2012 13:42:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[MyUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](255) NOT NULL,
	[Description] [varchar](max) NOT NULL,
 CONSTRAINT [PK_MyUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF) 
)

GO

SET ANSI_PADDING OFF
GO


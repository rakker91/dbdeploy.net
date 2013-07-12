USE [master]
GO

If not Exists (select Name from sys.sql_logins where name = '$(User)')
BEGIN
	CREATE LOGIN [$(User)] WITH PASSWORD=N'$(Password)', DEFAULT_DATABASE=[$(DatabaseName)], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
END
GO
EXEC master..sp_addsrvrolemember @loginame = N'$(User)', @rolename = N'sysadmin'
GO
USE [$(DatabaseName)]
GO
CREATE USER [$(User)] FOR LOGIN [$(User)]
GO
ALTER USER [$(User)] WITH DEFAULT_SCHEMA=[dbo]
GO
EXEC sp_addrolemember N'db_owner', N'$(User)'
GO

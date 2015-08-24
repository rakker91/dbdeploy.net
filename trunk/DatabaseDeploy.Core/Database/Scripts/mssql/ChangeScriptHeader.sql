-- Change Script Generated at $(CurrentDateTime) by $(CurrentUser)

DECLARE @currentDatabaseVersion INTEGER, @errMsg VARCHAR(max)
SELECT @currentDatabaseVersion = MAX(change_number) FROM $(Schema).$(ChangeLog)

IF (@currentDatabaseVersion <> $(CurrentVersion))
BEGIN
    SET @errMsg = 'Error: current database version is not $(CurrentVersion), but ' + CONVERT(VARCHAR, @currentDatabaseVersion)
    RAISERROR (@errMsg, 18, 1)
END
GO
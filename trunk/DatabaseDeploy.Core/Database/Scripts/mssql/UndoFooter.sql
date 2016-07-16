GO

DELETE from $(ChangeLog) WHERE change_number = $(ScriptId)
COMMIT TRANSACTION
GO

--------------- Fragment ends: $(ScriptName) ---------------
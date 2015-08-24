GO

DELETE from changelog WHERE change_number = $(ScriptId)
COMMIT TRANSACTION
GO

--------------- Fragment ends: $(ScriptName) ---------------
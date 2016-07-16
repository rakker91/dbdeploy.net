
INSERT INTO $(ChangeLog) (change_number, complete_dt, applied_by, description)
VALUES ($(ScriptId), GetDate(), SYSTEM_USER, '$(ScriptDescription)')

COMMIT TRANSACTION
GO

--------------- Fragment ends: $(ScriptName) ---------------
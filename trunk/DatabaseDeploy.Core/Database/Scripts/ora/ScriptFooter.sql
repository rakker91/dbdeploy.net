/

INSERT INTO $(ChangeLog) (change_number, complete_dt, applied_by, description)
VALUES ($(ScriptId), CURRENT_TIMESTAMP, sys_context('USERENV', 'SESSION_USER'), '$(ScriptDescription)');

COMMIT;

--------------- Fragment ends: $(ScriptName) ---------------
;
INSERT INTO changelog (change_number, complete_dt, applied_by, description)
VALUES ($(ScriptId), CURRENT_TIMESTAMP, sys_context('','AUTHENTICATED_IDENTITY'), '$(ScriptDescription)');

COMMIT;

--------------- Fragment ends: $(ScriptName) ---------------
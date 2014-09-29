;
INSERT INTO changelog (change_number, complete_dt, applied_by, description, script_hash)
VALUES ($(ScriptId), CURRENT_TIMESTAMP, sys_context('','AUTHENTICATED_IDENTITY'), '$(ScriptDescription)', '$(ScriptHash)');

COMMIT;

--------------- Fragment ends: $(ScriptName) ---------------
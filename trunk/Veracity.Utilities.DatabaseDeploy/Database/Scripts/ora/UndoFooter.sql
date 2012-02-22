;

DELETE from changelog WHERE change_number = $(ScriptId);
COMMIT;

--------------- Fragment ends: $(ScriptName) ---------------
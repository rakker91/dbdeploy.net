/

DELETE from $(ChangeLog) WHERE change_number = $(ScriptId);
COMMIT;

--------------- Fragment ends: $(ScriptName) ---------------
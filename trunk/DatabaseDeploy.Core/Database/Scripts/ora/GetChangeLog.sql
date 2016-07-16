Select
	change_number,
	complete_dt,
	applied_by,
	description
FROM
	$(ChangeLog)
ORDER BY
	change_number

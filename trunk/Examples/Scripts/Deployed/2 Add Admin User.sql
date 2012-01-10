insert into MyUsers(Username, Description)
VALUES ('AdminUser', 'Admin User')
--//@UNDO
Delete from MyUsers where Username = 'AdminUser'
insert into dbo.AspNetUsers (Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, UserName) values
('00000000-0000-0000-0000-000000000001','adm@myapp.ccc',1,'APblXneshuQ7dzDBAoUCHrD99sGoNtSZEBOTpyRUlPF6WHpybGqX+FBAIEbCQAq05g==','224feee5-eb58-4f6c-9978-dd609871754c',0,0,1,0,'adm@myapp.ccc')
go


insert into dbo.AspNetUserRoles (UserId, RoleId) values
('00000000-0000-0000-0000-000000000001','00000000-0000-0000-0000-000000000001')
go


insert into dbo.Usuarios (UsuarioId, Email, nombres, Apellidos, creadopor, creadofecha, ultimamodificacionpor, ultimamodificacionfecha) values
('00000000-0000-0000-0000-000000000001','adm@myapp.ccc','Admin','Pruebas','00000000-0000-0000-0000-000000000001',getdate(),'00000000-0000-0000-0000-000000000001',getdate())
go 

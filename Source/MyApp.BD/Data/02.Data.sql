--====
insert into dbo.Parametros (nombre, valor) values
('ErroresMensajeDefault','Ha ocurrido un error. Por favor, intente nuevamente mas tarde.'),
('SitioUrl', 'https://myapp.azurewebsites.net'),
('CloudStorageAccountConnectionString', 'UseDevelopmentStorage=true'),
('ServidorCorreos','{"Host":"smtp.host.ccc","Puerto":487,"Credenciales":{"UserName":"user","Password":"pwd"},"HabilitarSsl":true,"Remitente":"info@myapp.ccc","CopiaOculta":"bcc1@myapp.ccc","Headers":{{"Header1","Value1"},{"Header2","Value2"}}}')
go
--====


--====
insert into dbo.CorreosParametros (Id, Asunto, Contenido) values
(1,'Activa tu cuenta','<p>Hola {{NombreYApellido}},</p><p>Para activar tu cuenta haz clic <a href="{{ActivarCuentaUrl}}">aquí</a> o accede al siguiente enlace en tu navegador:<br/>{{ActivarCuentaUrl}}</p><p>Este link será válido por 24 horas.</p>'),
(2,'Restablece tu password','<p>Hola {{NombreYApellido}},</p><p>Para restablecer tu contraseña haz clic <a href="{{RestablecerPasswordUrl}}">aquí</a> o accede al siguiente enlace en tu navegador:<br/>{{RestablecerPasswordUrl}}</p><p>Este link será válido por 24 horas.</p>')
go
--====


--====
insert into dbo.AspNetRoles (Id,Name) values
('00000000-0000-0000-0000-000000000001','Administrador')
go
--====

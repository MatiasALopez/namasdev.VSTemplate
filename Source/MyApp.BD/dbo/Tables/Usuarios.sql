create table [dbo].[Usuarios]
(
	[UsuarioId] nvarchar(128) NOT NULL,
	[Email] nvarchar(100) NOT NULL,
	[Nombres] nvarchar(100) NOT NULL,
	[Apellidos] nvarchar(100) NOT NULL,
	[NombresYApellidos] AS CAST(concat([Nombres],' ',[Apellidos]) AS nvarchar(200)), 
	[ApellidosYNombres] AS CAST(concat([Apellidos],', ',[Nombres]) AS nvarchar(200)), 
	[CreadoPor] nvarchar(128) NOT NULL,
	[CreadoFecha] datetime NOT NULL,
	[UltimaModificacionPor] nvarchar(128) NOT NULL,
	[UltimaModificacionFecha] datetime NOT NULL,
	[BorradoPor] nvarchar(128) NULL,
	[BorradoFecha] datetime NULL,
	[Borrado] AS (ISNULL(CONVERT(bit,CASE WHEN [BorradoFecha] IS NULL THEN 0 ELSE 1 END), 0)),

	constraint [PK_Usuarios] primary key clustered ([UsuarioId]),
	constraint [FK_Usuarios_UsuarioId] foreign key ([UsuarioId]) references [dbo].[AspNetUsers] ([Id]) on delete cascade
)
go

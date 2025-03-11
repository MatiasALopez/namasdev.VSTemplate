--====
CREATE TABLE [dbo].[AspNetUsers] 
(
    [Id]                   NVARCHAR (128) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_AspNetUsers_UserName] ON [dbo].[AspNetUsers]([UserName] ASC);
go
--====


--====
CREATE TABLE [dbo].[AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
);
go

CREATE UNIQUE NONCLUSTERED INDEX [IX_AspNetRoles_Name] ON [dbo].[AspNetRoles]([Name] ASC);
GO
--====


--====
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    [UserId]        NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]([UserId] ASC);
GO
--====


--====
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [UserId]     NVARCHAR (128) NOT NULL,
    [ClaimType]  NVARCHAR (MAX) NULL,
    [ClaimValue] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]([UserId] ASC);
GO
--====


--====
CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]([RoleId] ASC);
GO

CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_UserId] ON [dbo].[AspNetUserRoles]([UserId] ASC);
GO
--====


--====
CREATE TABLE [dbo].[Parametros]
(
	[Nombre] nvarchar(100) not null,
	[Valor] nvarchar(max) null,

	constraint [PK_Parametros] primary key clustered ([Nombre])
)
go
--====


--====
create table [dbo].[CorreosParametros]
(
	[Id] smallint not null,
	[Asunto] nvarchar(256) not null,
	[Contenido] nvarchar(max) not null,
	[CopiaOculta] nvarchar(max) null, 

    constraint [PK_CorreosParametros] primary key clustered ([Id])
)
go
--====

--=====
CREATE TABLE [dbo].[Errores]
(
	[Id] uniqueidentifier NOT NULL,
	[Mensaje] nvarchar(max) NOT NULL,
	[StackTrace] nvarchar(max) NOT NULL,
	[Source] nvarchar(200) NOT NULL,
	[Argumentos] nvarchar(max) NULL,
	[FechaHora] datetime NOT NULL,
	[UserId] nvarchar(128) NULL,
 
	CONSTRAINT [PK_Errores] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
--=====


--=====
create table [dbo].[Usuarios]
(
	[UsuarioId] nvarchar(128) NOT NULL,
	[Email] nvarchar(256) NOT NULL,
	[Nombres] nvarchar(100) NOT NULL,
	[Apellidos] nvarchar(100) NOT NULL,
	[NombresYApellidos] AS CAST(concat([Nombres],' ',[Apellidos]) AS nvarchar(200)), 
	[ApellidosYNombres] AS CAST(concat([Apellidos],' ',[Nombres]) AS nvarchar(200)), 
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
--=====

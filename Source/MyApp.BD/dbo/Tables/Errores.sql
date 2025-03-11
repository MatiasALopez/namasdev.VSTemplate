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

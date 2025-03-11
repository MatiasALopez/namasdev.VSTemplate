CREATE TABLE [dbo].[Parametros]
(
	[Nombre] nvarchar(100) not null,
	[Valor] nvarchar(max) null,

	constraint [PK_Parametros] primary key clustered ([Nombre])
)
go

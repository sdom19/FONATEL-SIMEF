CREATE TABLE [dbo].[TipoReglaValidacion] (
    [IdTipo] INT           NOT NULL,
    [Nombre] VARCHAR (300) NOT NULL,
    [Estado] BIT           NOT NULL,
    CONSTRAINT [PK_TipoReglaValidacion] PRIMARY KEY CLUSTERED ([IdTipo] ASC)
);


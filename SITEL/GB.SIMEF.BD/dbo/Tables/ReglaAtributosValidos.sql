CREATE TABLE [dbo].[ReglaAtributosValidos] (
    [idCompara]             INT NOT NULL,
    [idTipoReglaValidacion] INT NOT NULL,
    [idCategoria]           INT NOT NULL,
    [idCategoriaAtributo]   INT NOT NULL,
    [Estado]                BIT NOT NULL,
    CONSTRAINT [PK_ReglaAtributosValidos] PRIMARY KEY CLUSTERED ([idCompara] ASC),
    CONSTRAINT [FK_ReglaAtributosValidos_ReglaValidacionTipo] FOREIGN KEY ([idTipoReglaValidacion]) REFERENCES [dbo].[ReglaValidacionTipo] ([idReglasValidacionTipo])
);


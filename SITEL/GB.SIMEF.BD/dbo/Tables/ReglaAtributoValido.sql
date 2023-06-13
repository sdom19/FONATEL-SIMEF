CREATE TABLE [dbo].[ReglaAtributoValido] (
    [IdReglaAtributoValido]    INT IDENTITY (1, 1) NOT NULL,
    [IdDetalleReglaValidacion] INT NULL,
    [IdCategoriaDesagregacion] INT NULL,
    [IdDetalleCategoriaTexto]  INT NULL,
    CONSTRAINT [PK_ReglaAtributoValido] PRIMARY KEY CLUSTERED ([IdReglaAtributoValido] ASC),
    CONSTRAINT [FK_ReglaAtributoValido_ReglaValidacionTipo] FOREIGN KEY ([IdDetalleReglaValidacion]) REFERENCES [dbo].[DetalleReglaValidacion] ([IdDetalleReglaValidacion])
);


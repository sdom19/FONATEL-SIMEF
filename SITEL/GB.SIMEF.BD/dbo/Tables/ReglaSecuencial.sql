CREATE TABLE [dbo].[ReglaSecuencial] (
    [IdReglaSecuencial]        INT IDENTITY (1, 1) NOT NULL,
    [IdDetalleReglaValidacion] INT NOT NULL,
    [IdCategoriaDesagregacion] INT NOT NULL,
    CONSTRAINT [PK_Reglaecuencial] PRIMARY KEY CLUSTERED ([IdReglaSecuencial] ASC),
    CONSTRAINT [FK_Reglaecuencial_DetalleReglaValidacion] FOREIGN KEY ([IdDetalleReglaValidacion]) REFERENCES [dbo].[DetalleReglaValidacion] ([IdDetalleReglaValidacion]),
    CONSTRAINT [FK_ReglaSecuencial_CategoriaDesagregacion] FOREIGN KEY ([IdCategoriaDesagregacion]) REFERENCES [dbo].[CategoriaDesagregacion] ([IdCategoriaDesagregacion])
);


CREATE TABLE [dbo].[DetalleIndicadorCategoria] (
    [IdDetalleIndicadorCategoria] INT IDENTITY (1, 1) NOT NULL,
    [IdIndicador]                 INT NOT NULL,
    [IdCategoriaDesagregacion]    INT NOT NULL,
    [IdDetalleCategoriaTexto]     INT NULL,
    [Estado]                      BIT NOT NULL,
    CONSTRAINT [PK_IndicadorDetalleCategoria] PRIMARY KEY CLUSTERED ([IdDetalleIndicadorCategoria] ASC, [IdIndicador] ASC),
    CONSTRAINT [FK_DetalleIndicadorCategoria_DetalleCategoriaTexto] FOREIGN KEY ([IdDetalleCategoriaTexto], [IdCategoriaDesagregacion]) REFERENCES [dbo].[DetalleCategoriaTexto] ([IdDetalleCategoriaTexto], [IdCategoriaDesagregacion]),
    CONSTRAINT [FK_IndicadorDetalleCategoria_Indicador] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);


CREATE TABLE [dbo].[DetalleIndicadorCategoria] (
    [idDetalleIndicador]  INT NOT NULL,
    [idIndicador]         INT NOT NULL,
    [idCategoriaRelacion] INT NOT NULL,
    [idDetalleRelacion]   INT NULL,
    [Estado]              BIT NOT NULL,
    CONSTRAINT [PK_IndicadorDetalleCategoria] PRIMARY KEY CLUSTERED ([idDetalleIndicador] ASC, [idIndicador] ASC),
    CONSTRAINT [FK_DetalleIndicadorCategoria_DetalleRelacionCategoria] FOREIGN KEY ([idDetalleRelacion], [idCategoriaRelacion]) REFERENCES [dbo].[DetalleRelacionCategoria] ([idDetalleRelacionCategoria], [IdRelacionCategoria]),
    CONSTRAINT [FK_IndicadorDetalleCategoria_Indicador] FOREIGN KEY ([idIndicador]) REFERENCES [dbo].[Indicador] ([idIndicador])
);


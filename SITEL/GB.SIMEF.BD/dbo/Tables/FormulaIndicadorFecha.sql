CREATE TABLE [dbo].[FormulaIndicadorFecha] (
    [idFormula]          INT  NOT NULL,
    [idDetalleIndicador] INT  NOT NULL,
    [idCategoriaInicio]  INT  NULL,
    [FechaActualInicio]  BIT  NULL,
    [FechaInicio]        DATE NULL,
    [idCategoriaFin]     INT  NULL,
    [FechaActualFin]     BIT  NULL,
    [FechaFin]           DATE NULL,
    [Estado]             BIT  NOT NULL,
    CONSTRAINT [PK_FormulaIndicadorFecha] PRIMARY KEY CLUSTERED ([idFormula] ASC, [idDetalleIndicador] ASC),
    CONSTRAINT [FK_FormulaIndicadorFecha_FormulasCalculoDetalle] FOREIGN KEY ([idDetalleIndicador], [idFormula]) REFERENCES [dbo].[FormulasCalculoDetalle] ([idDetalleFormula], [idFormula])
);


CREATE TABLE [dbo].[FormulaIndicadorMC] (
    [idFormula]          INT           NOT NULL,
    [idDetalleIndicador] INT           NOT NULL,
    [Criterio]           VARCHAR (100) NOT NULL,
    [Detalle]            VARCHAR (100) NOT NULL,
    [Indicador]          VARCHAR (100) NOT NULL,
    [idFuenteIndicador]  INT           NULL,
    [idTipoIndicador]    INT           NULL,
    [ValorTotal]         BIT           NULL,
    CONSTRAINT [PK_FormulaIndicadorMC] PRIMARY KEY CLUSTERED ([idFormula] ASC, [idDetalleIndicador] ASC),
    CONSTRAINT [FK_FormulaIndicadorMC_FormulasCalculoDetalle] FOREIGN KEY ([idDetalleIndicador], [idFormula]) REFERENCES [dbo].[FormulasCalculoDetalle] ([idDetalleFormula], [idFormula]),
    CONSTRAINT [FK_FormulaIndicadorMC_FuenteIndicador] FOREIGN KEY ([idFuenteIndicador]) REFERENCES [dbo].[FuenteIndicador] ([idFuenteIndicador]),
    CONSTRAINT [FK_FormulaIndicadorMC_TipoIndicadores] FOREIGN KEY ([idTipoIndicador]) REFERENCES [dbo].[TipoIndicadores] ([IdTipoIdicador])
);


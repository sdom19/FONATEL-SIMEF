CREATE TABLE [dbo].[FormulasCalculoDetalle] (
    [idDetalleFormula] INT NOT NULL,
    [idFormula]        INT NOT NULL,
    [Constante]        INT NULL,
    [Operador]         BIT NOT NULL,
    [IndicadorMC]      BIT NOT NULL,
    [IndicadorDSF]     BIT NOT NULL,
    CONSTRAINT [PK_FormulasCalculoDetalle] PRIMARY KEY CLUSTERED ([idDetalleFormula] ASC, [idFormula] ASC),
    CONSTRAINT [FK_FormulasCalculoDetalle_FormulasCalculo] FOREIGN KEY ([idFormula]) REFERENCES [dbo].[FormulasCalculo] ([idFormula]),
    CONSTRAINT [FK_FormulasCalculoDetalle_FormulasOperador] FOREIGN KEY ([idFormula], [idDetalleFormula]) REFERENCES [dbo].[FormulasOperador] ([idFormula], [idDetalleFormula]),
    CONSTRAINT [FK_FormulasCalculoDetalle_FuenteIndicador] FOREIGN KEY ([idDetalleFormula]) REFERENCES [dbo].[FuenteIndicador] ([idFuenteIndicador])
);


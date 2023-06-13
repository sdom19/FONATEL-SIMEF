CREATE TABLE [dbo].[FormulaVariableDatoCriterio] (
    [IdFormulaVariableDatoCriterio] INT           IDENTITY (1, 1) NOT NULL,
    [IdFuenteIndicador]             INT           NOT NULL,
    [IdIndicador]                   VARCHAR (250) NOT NULL,
    [IdDetalleIndicadorVariable]    INT           NULL,
    [IdCriterio]                    VARCHAR (250) NULL,
    [IdCategoriaDesagregacion]      INT           NULL,
    [IdDetalleCategoriaTexto]       INT           NULL,
    [IdAcumulacionFormula]          INT           NULL,
    [EsValorTotal]                  BIT           NOT NULL,
    CONSTRAINT [PK_FormulaVariableDatoCriterio] PRIMARY KEY CLUSTERED ([IdFormulaVariableDatoCriterio] ASC),
    CONSTRAINT [FK_FormulaVariableDatoCriterio_AcumulacionFormula] FOREIGN KEY ([IdAcumulacionFormula]) REFERENCES [dbo].[AcumulacionFormula] ([IdAcumulacionFormula]),
    CONSTRAINT [FK_FormulaVariableDatoCriterio_FuenteIndicador] FOREIGN KEY ([IdFuenteIndicador]) REFERENCES [dbo].[FuenteIndicador] ([IdFuenteIndicador])
);


CREATE TABLE [dbo].[FormulaIndicadorDSF] (
    [idFormula]          INT NOT NULL,
    [idDetalleIndicador] INT NOT NULL,
    [idIndicador]        INT NOT NULL,
    [idcategoria]        INT NOT NULL,
    [idDetalle]          INT NOT NULL,
    [idFuenteIndicador]  INT NULL,
    [idTipoIndicador]    INT NULL,
    [ValorTotal]         BIT NULL,
    CONSTRAINT [PK_FormulaIndicadorDSF] PRIMARY KEY CLUSTERED ([idFormula] ASC, [idDetalleIndicador] ASC),
    CONSTRAINT [FK_FormulaIndicadorDSF_FormulasCalculoDetalle] FOREIGN KEY ([idDetalleIndicador], [idFormula]) REFERENCES [dbo].[FormulasCalculoDetalle] ([idDetalleFormula], [idFormula]),
    CONSTRAINT [FK_FormulaIndicadorDSF_FuenteIndicador] FOREIGN KEY ([idFuenteIndicador]) REFERENCES [dbo].[FuenteIndicador] ([idFuenteIndicador]),
    CONSTRAINT [FK_FormulaIndicadorDSF_TipoIndicadores] FOREIGN KEY ([idTipoIndicador]) REFERENCES [dbo].[TipoIndicadores] ([IdTipoIdicador])
);


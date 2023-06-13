CREATE TABLE [dbo].[FormulaDefinicionFecha] (
    [IdFormulaDefinicionFecha]       INT      IDENTITY (1, 1) NOT NULL,
    [FechaInicio]                    DATETIME NULL,
    [FechaFinal]                     DATETIME NULL,
    [IdUnidadMedida]                 INT      NOT NULL,
    [IdTipoFechaInicio]              INT      NOT NULL,
    [IdTipoFechaFinal]               INT      NOT NULL,
    [IdCategoriaDesagregacionInicio] INT      NULL,
    [IdCategoriaDesagregacionFinal]  INT      NULL,
    [IdIndicador]                    INT      NOT NULL,
    CONSTRAINT [PK_FormulaDefinicionFecha] PRIMARY KEY CLUSTERED ([IdFormulaDefinicionFecha] ASC),
    CONSTRAINT [FK_FormulaDefinicionFecha_CategoriaDesagregacion] FOREIGN KEY ([IdCategoriaDesagregacionInicio]) REFERENCES [dbo].[CategoriaDesagregacion] ([IdCategoriaDesagregacion]),
    CONSTRAINT [FK_FormulaDefinicionFecha_CategoriaDesagregacion1] FOREIGN KEY ([IdCategoriaDesagregacionFinal]) REFERENCES [dbo].[CategoriaDesagregacion] ([IdCategoriaDesagregacion]),
    CONSTRAINT [FK_FormulaDefinicionFecha_FormulaCalculoTipoFecha] FOREIGN KEY ([IdTipoFechaInicio]) REFERENCES [dbo].[FormulaCalculoTipoFecha] ([IdFormulaCalculoTipoFecha]),
    CONSTRAINT [FK_FormulaDefinicionFecha_FormulaCalculoTipoFecha1] FOREIGN KEY ([IdTipoFechaFinal]) REFERENCES [dbo].[FormulaCalculoTipoFecha] ([IdFormulaCalculoTipoFecha]),
    CONSTRAINT [FK_FormulaDefinicionFecha_FormulaCalculoUnidadMedida] FOREIGN KEY ([IdUnidadMedida]) REFERENCES [dbo].[FormulaCalculoUnidadMedida] ([IdFormulaCalculoUnidadMedida]),
    CONSTRAINT [FK_FormulaDefinicionFecha_Indicador] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);


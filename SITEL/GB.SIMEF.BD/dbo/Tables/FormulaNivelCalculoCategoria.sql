CREATE TABLE [dbo].[FormulaNivelCalculoCategoria] (
    [IdFormulaNivelCalculoCategoria] INT IDENTITY (1, 1) NOT NULL,
    [IdFormulaCalculo]               INT NOT NULL,
    [IdCategoriaDesagregacion]       INT NOT NULL,
    CONSTRAINT [PK_FormulaNivelCalculoCategoria] PRIMARY KEY CLUSTERED ([IdFormulaNivelCalculoCategoria] ASC),
    CONSTRAINT [FK_FormulaNivelCalculoCategoria_CategoriaDesagregacion] FOREIGN KEY ([IdCategoriaDesagregacion]) REFERENCES [dbo].[CategoriaDesagregacion] ([IdCategoriaDesagregacion]),
    CONSTRAINT [FK_FormulaNivelCalculoCategoria_FormulaCalculo] FOREIGN KEY ([IdFormulaCalculo]) REFERENCES [dbo].[FormulaCalculo] ([IdFormulaCalculo])
);


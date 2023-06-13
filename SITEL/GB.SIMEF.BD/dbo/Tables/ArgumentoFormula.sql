CREATE TABLE [dbo].[ArgumentoFormula] (
    [IdArgumentoFormula]            INT            IDENTITY (1, 1) NOT NULL,
    [IdFormulaTipoArgumento]        INT            NOT NULL,
    [IdFormulaDefinicionFecha]      INT            NULL,
    [IdFormulaVariableDatoCriterio] INT            NULL,
    [IdFormulaCalculo]              INT            NOT NULL,
    [PredicadoSQL]                  VARCHAR (8000) NOT NULL,
    [OrdenEnFormula]                INT            NOT NULL,
    [Etiqueta]                      VARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_ArgumentoFormula] PRIMARY KEY CLUSTERED ([IdArgumentoFormula] ASC),
    CONSTRAINT [FK_ArgumentoFormula_FormulaCalculo] FOREIGN KEY ([IdFormulaCalculo]) REFERENCES [dbo].[FormulaCalculo] ([IdFormulaCalculo]),
    CONSTRAINT [FK_ArgumentoFormula_FormulaDefinicionFecha] FOREIGN KEY ([IdFormulaDefinicionFecha]) REFERENCES [dbo].[FormulaDefinicionFecha] ([IdFormulaDefinicionFecha]),
    CONSTRAINT [FK_ArgumentoFormula_FormulaTipoArgumento] FOREIGN KEY ([IdFormulaTipoArgumento]) REFERENCES [dbo].[FormulaTipoArgumento] ([IdFormulaTipoArgumento]),
    CONSTRAINT [FK_ArgumentoFormula_FormulaVariableDatoCriterio] FOREIGN KEY ([IdFormulaVariableDatoCriterio]) REFERENCES [dbo].[FormulaVariableDatoCriterio] ([IdFormulaVariableDatoCriterio])
);


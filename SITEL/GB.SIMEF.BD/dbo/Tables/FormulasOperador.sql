CREATE TABLE [dbo].[FormulasOperador] (
    [IdFormula]        INT NOT NULL,
    [IdDetalleFormula] INT NOT NULL,
    [IdOperador]       INT NOT NULL,
    CONSTRAINT [PK_FormulasOperador] PRIMARY KEY CLUSTERED ([IdFormula] ASC, [IdDetalleFormula] ASC),
    CONSTRAINT [FK_FormulasOperador_Operadores] FOREIGN KEY ([IdOperador]) REFERENCES [dbo].[OperadorArismetico] ([IdOperador])
);






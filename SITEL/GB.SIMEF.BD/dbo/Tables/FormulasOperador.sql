CREATE TABLE [dbo].[FormulasOperador] (
    [idFormula]        INT NOT NULL,
    [idDetalleFormula] INT NOT NULL,
    [idOperador]       INT NOT NULL,
    CONSTRAINT [PK_FormulasOperador] PRIMARY KEY CLUSTERED ([idFormula] ASC, [idDetalleFormula] ASC),
    CONSTRAINT [FK_FormulasOperador_Operadores] FOREIGN KEY ([idOperador]) REFERENCES [dbo].[Operadores] ([IdOperador])
);


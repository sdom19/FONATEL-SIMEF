CREATE TABLE [dbo].[ReglaComparacionIndicador] (
    [idCompara]             INT NOT NULL,
    [idTipoReglaValidacion] INT NOT NULL,
    [IdIndicadorCompara]    INT NOT NULL,
    [idVariableCompara]     INT NULL,
    [idOperador]            INT NOT NULL,
    [idvariable]            INT NULL,
    [Estado]                BIT NOT NULL,
    CONSTRAINT [PK_ReglaComparacionIndicador] PRIMARY KEY CLUSTERED ([idCompara] ASC),
    CONSTRAINT [FK_ReglaComparacionIndicador_OperadorReglaValidacion] FOREIGN KEY ([idOperador]) REFERENCES [dbo].[Operadores] ([IdOperador]),
    CONSTRAINT [FK_ReglaComparacionIndicador_ReglaValidacionTipo] FOREIGN KEY ([idTipoReglaValidacion]) REFERENCES [dbo].[ReglaValidacionTipo] ([idReglasValidacionTipo])
);


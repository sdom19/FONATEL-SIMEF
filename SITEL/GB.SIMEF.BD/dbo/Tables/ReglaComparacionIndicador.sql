CREATE TABLE [dbo].[ReglaComparacionIndicador] (
    [IdCompara]             INT NOT NULL,
    [IdTipoReglaValidacion] INT NOT NULL,
    [IdIndicadorCompara]    INT NOT NULL,
    [idVariableCompara]     INT NULL,
    [IdOperador]            INT NOT NULL,
    [idvariable]            INT NULL,
    [Estado]                BIT NOT NULL,
    CONSTRAINT [PK_ReglaComparacionIndicador] PRIMARY KEY CLUSTERED ([IdCompara] ASC),
    CONSTRAINT [FK_ReglaComparacionIndicador_OperadorReglaValidacion] FOREIGN KEY ([IdOperador]) REFERENCES [dbo].[OperadorArismetico] ([IdOperador]),
    CONSTRAINT [FK_ReglaComparacionIndicador_ReglaValidacionTipo] FOREIGN KEY ([IdTipoReglaValidacion]) REFERENCES [dbo].[ReglaValidacionTipo] ([IdReglasValidacionTipo])
);




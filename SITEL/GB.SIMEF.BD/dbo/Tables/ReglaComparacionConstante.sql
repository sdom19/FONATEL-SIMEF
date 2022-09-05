CREATE TABLE [dbo].[ReglaComparacionConstante] (
    [IdCompara]             INT          NOT NULL,
    [IdTipoReglaValidacion] INT          NOT NULL,
    [Constante]             VARCHAR (60) NOT NULL,
    [IdOperador]            INT          NOT NULL,
    [idvariable]            INT          NULL,
    [Estado]                INT          NOT NULL,
    CONSTRAINT [PK_ReglaComparacionCostante] PRIMARY KEY CLUSTERED ([IdCompara] ASC),
    CONSTRAINT [FK_ReglaComparacionConstante_OperadorReglaValidacion] FOREIGN KEY ([IdOperador]) REFERENCES [dbo].[OperadorArismetico] ([IdOperador]),
    CONSTRAINT [FK_ReglaComparacionConstante_ReglaValidacionTipo] FOREIGN KEY ([IdTipoReglaValidacion]) REFERENCES [dbo].[ReglaValidacionTipo] ([IdReglasValidacionTipo])
);




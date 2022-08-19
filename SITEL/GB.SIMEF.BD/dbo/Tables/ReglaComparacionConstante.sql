CREATE TABLE [dbo].[ReglaComparacionConstante] (
    [idCompara]             INT          NOT NULL,
    [idTipoReglaValidacion] INT          NOT NULL,
    [Constante]             VARCHAR (60) NOT NULL,
    [idOperador]            INT          NOT NULL,
    [idvariable]            INT          NULL,
    [Estado]                INT          NOT NULL,
    CONSTRAINT [PK_ReglaComparacionCostante] PRIMARY KEY CLUSTERED ([idCompara] ASC),
    CONSTRAINT [FK_ReglaComparacionConstante_OperadorReglaValidacion] FOREIGN KEY ([idOperador]) REFERENCES [dbo].[Operadores] ([IdOperador]),
    CONSTRAINT [FK_ReglaComparacionConstante_ReglaValidacionTipo] FOREIGN KEY ([idTipoReglaValidacion]) REFERENCES [dbo].[ReglaValidacionTipo] ([idReglasValidacionTipo])
);


CREATE TABLE [dbo].[ReglaSecuencial] (
    [idCompara]             INT NOT NULL,
    [idTipoReglaValidacion] INT NOT NULL,
    [idCategoriaId]         INT NOT NULL,
    [idOperador]            INT NOT NULL,
    [idvariable]            INT NULL,
    [Estado]                BIT NULL,
    CONSTRAINT [PK_ReglaSecuencial] PRIMARY KEY CLUSTERED ([idCompara] ASC),
    CONSTRAINT [FK_ReglaSecuencial_OperadorReglaValidacion] FOREIGN KEY ([idOperador]) REFERENCES [dbo].[Operadores] ([IdOperador]),
    CONSTRAINT [FK_ReglaSecuencial_ReglaValidacionTipo] FOREIGN KEY ([idTipoReglaValidacion]) REFERENCES [dbo].[ReglaValidacionTipo] ([idReglasValidacionTipo])
);


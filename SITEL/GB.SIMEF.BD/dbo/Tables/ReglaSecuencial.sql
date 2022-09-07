CREATE TABLE [dbo].[ReglaSecuencial] (
    [IdCompara]             INT NOT NULL,
    [IdTipoReglaValidacion] INT NOT NULL,
    [idCategoriaId]         INT NOT NULL,
    [IdOperador]            INT NOT NULL,
    [idvariable]            INT NULL,
    [Estado]                BIT NULL,
    CONSTRAINT [PK_ReglaSecuencial] PRIMARY KEY CLUSTERED ([IdCompara] ASC),
    CONSTRAINT [FK_ReglaSecuencial_OperadorReglaValidacion] FOREIGN KEY ([IdOperador]) REFERENCES [dbo].[OperadorArismetico] ([IdOperador]),
    CONSTRAINT [FK_ReglaSecuencial_ReglaValidacionTipo] FOREIGN KEY ([IdTipoReglaValidacion]) REFERENCES [dbo].[ReglaValidacionTipo] ([IdReglasValidacionTipo])
);






CREATE TABLE [dbo].[ReglaValidacionTipo] (
    [idReglasValidacionTipo] INT NOT NULL,
    [idRegla]                INT NOT NULL,
    [idTipo]                 INT NOT NULL,
    [idindicardorVariable]   INT NOT NULL,
    CONSTRAINT [PK_ReglaValidacionTipo] PRIMARY KEY CLUSTERED ([idReglasValidacionTipo] ASC),
    CONSTRAINT [FK_ReglaValidacionTipo_ReglaValidacion] FOREIGN KEY ([idRegla]) REFERENCES [dbo].[ReglaValidacion] ([idRegla]),
    CONSTRAINT [FK_ReglaValidacionTipo_TipoReglaValidacion] FOREIGN KEY ([idTipo]) REFERENCES [dbo].[TipoReglaValidacion] ([IdTipo])
);


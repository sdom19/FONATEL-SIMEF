CREATE TABLE [dbo].[DetalleReglaValidacion] (
    [IdDetalleReglaValidacion]   INT IDENTITY (1, 1) NOT NULL,
    [IdReglaValidacion]          INT NOT NULL,
    [IdTipoReglaValidacion]      INT NOT NULL,
    [IdOperadorAritmetico]       INT NOT NULL,
    [IdDetalleIndicadorVariable] INT NULL,
    [IdIndicador]                INT NULL,
    [Estado]                     BIT NOT NULL,
    CONSTRAINT [PK_ReglaValidacionTipo] PRIMARY KEY CLUSTERED ([IdDetalleReglaValidacion] ASC),
    CONSTRAINT [FK_DetalleReglaValidacion_DetalleIndicadorVariable] FOREIGN KEY ([IdDetalleIndicadorVariable], [IdIndicador]) REFERENCES [dbo].[DetalleIndicadorVariable] ([IdDetalleIndicadorVariable], [IdIndicador]),
    CONSTRAINT [FK_ReglaValidacionTipo_OperadorArismetico] FOREIGN KEY ([IdOperadorAritmetico]) REFERENCES [dbo].[OperadorAritmetico] ([IdOperadorAritmetico]),
    CONSTRAINT [FK_ReglaValidacionTipo_ReglaValidacion] FOREIGN KEY ([IdReglaValidacion]) REFERENCES [dbo].[ReglaValidacion] ([IdReglaValidacion]),
    CONSTRAINT [FK_ReglaValidacionTipo_TipoReglaValidacion] FOREIGN KEY ([IdTipoReglaValidacion]) REFERENCES [dbo].[TipoReglaValidacion] ([IdTipoReglaValidacion])
);


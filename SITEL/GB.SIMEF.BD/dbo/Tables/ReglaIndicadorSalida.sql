CREATE TABLE [dbo].[ReglaIndicadorSalida] (
    [IdReglaIndicadorSalida]     INT IDENTITY (1, 1) NOT NULL,
    [IdDetalleReglaValidacion]   INT NULL,
    [IdDetalleIndicadorVariable] INT NOT NULL,
    [IdIndicador]                INT NULL,
    CONSTRAINT [PK_ReglaIndicadorSalida] PRIMARY KEY CLUSTERED ([IdReglaIndicadorSalida] ASC),
    CONSTRAINT [FK_ReglaIndicadorSalida_DetalleIndicadorVariable] FOREIGN KEY ([IdDetalleIndicadorVariable], [IdIndicador]) REFERENCES [dbo].[DetalleIndicadorVariable] ([IdDetalleIndicadorVariable], [IdIndicador]),
    CONSTRAINT [FK_ReglaIndicadorSalida_DetalleReglaValidacion] FOREIGN KEY ([IdDetalleReglaValidacion]) REFERENCES [dbo].[DetalleReglaValidacion] ([IdDetalleReglaValidacion])
);


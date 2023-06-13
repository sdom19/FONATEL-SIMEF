CREATE TABLE [dbo].[ReglaComparacionEntradaSalida] (
    [IdReglaComparacionEntradaSalida] INT IDENTITY (1, 1) NOT NULL,
    [IdDetalleReglaValidacion]        INT NULL,
    [IdDetalleIndicadorVariable]      INT NULL,
    [IdIndicador]                     INT NULL,
    CONSTRAINT [PK_ReglaComparacionEntradaSalida] PRIMARY KEY CLUSTERED ([IdReglaComparacionEntradaSalida] ASC),
    CONSTRAINT [FK_ReglaComparacionEntradaSalida_DetalleIndicadorVariable] FOREIGN KEY ([IdDetalleIndicadorVariable], [IdIndicador]) REFERENCES [dbo].[DetalleIndicadorVariable] ([IdDetalleIndicadorVariable], [IdIndicador]),
    CONSTRAINT [FK_ReglaComparacionEntradaSalida_DetalleReglaValidacion] FOREIGN KEY ([IdDetalleReglaValidacion]) REFERENCES [dbo].[DetalleReglaValidacion] ([IdDetalleReglaValidacion])
);


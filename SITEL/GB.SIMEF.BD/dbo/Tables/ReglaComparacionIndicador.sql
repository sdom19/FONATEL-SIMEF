CREATE TABLE [dbo].[ReglaComparacionIndicador] (
    [IdReglaComparacionIndicador] INT IDENTITY (1, 1) NOT NULL,
    [IdDetalleReglaValidacion]    INT NULL,
    [IdDetalleIndicadorVariable]  INT NULL,
    [IdIndicador]                 INT NULL,
    CONSTRAINT [PK_ReglaComparacionIndicador_1] PRIMARY KEY CLUSTERED ([IdReglaComparacionIndicador] ASC),
    CONSTRAINT [FK_ReglaComparacionIndicador_DetalleIndicadorVariable] FOREIGN KEY ([IdDetalleIndicadorVariable], [IdIndicador]) REFERENCES [dbo].[DetalleIndicadorVariable] ([IdDetalleIndicadorVariable], [IdIndicador]),
    CONSTRAINT [FK_ReglaComparacionIndicador_DetalleReglaValidacion] FOREIGN KEY ([IdDetalleReglaValidacion]) REFERENCES [dbo].[DetalleReglaValidacion] ([IdDetalleReglaValidacion])
);


CREATE TABLE [dbo].[ReglaComparacionConstante] (
    [IdReglaComparacionConstante] INT           IDENTITY (1, 1) NOT NULL,
    [IdDetalleReglaValidacion]    INT           NULL,
    [Constante]                   VARCHAR (100) NOT NULL,
    CONSTRAINT [PK_ReglaComparacionCostante] PRIMARY KEY CLUSTERED ([IdReglaComparacionConstante] ASC),
    CONSTRAINT [FK_ReglaComparacionConstante_DetalleReglaValidacion] FOREIGN KEY ([IdDetalleReglaValidacion]) REFERENCES [dbo].[DetalleReglaValidacion] ([IdDetalleReglaValidacion])
);


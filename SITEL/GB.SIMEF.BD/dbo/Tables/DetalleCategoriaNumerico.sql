CREATE TABLE [dbo].[DetalleCategoriaNumerico] (
    [IdDetalleCategoriaNumerico] INT        IDENTITY (1, 1) NOT NULL,
    [IdCategoriaDesagregacion]   INT        NOT NULL,
    [Minimo]                     FLOAT (53) NULL,
    [Maximo]                     FLOAT (53) NULL,
    [Estado]                     BIT        NOT NULL,
    CONSTRAINT [PK_DetalleCategoriaNumerico] PRIMARY KEY CLUSTERED ([IdDetalleCategoriaNumerico] ASC, [IdCategoriaDesagregacion] ASC),
    CONSTRAINT [FK_DetalleCategoriaNumerico_CategoriaDesagregacion] FOREIGN KEY ([IdCategoriaDesagregacion]) REFERENCES [dbo].[CategoriaDesagregacion] ([IdCategoriaDesagregacion])
);


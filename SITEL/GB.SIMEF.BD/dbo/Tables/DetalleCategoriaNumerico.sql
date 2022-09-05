CREATE TABLE [dbo].[DetalleCategoriaNumerico] (
    [idCategoriaDetalle] INT IDENTITY (1, 1) NOT NULL,
    [idCategoria]        INT NOT NULL,
    [Minimo]             INT NULL,
    [Maximo]             INT NULL,
    [Estado]             BIT NOT NULL,
    CONSTRAINT [FK_DatelleCategoriaNumerico_CategoriasDesagregacion] FOREIGN KEY ([idCategoria]) REFERENCES [dbo].[CategoriasDesagregacion] ([idCategoria])
);


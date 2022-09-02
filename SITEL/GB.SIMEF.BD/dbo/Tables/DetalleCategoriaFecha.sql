CREATE TABLE [dbo].[DetalleCategoriaFecha] (
    [idCategoriaDetalle] INT  IDENTITY (1, 1) NOT NULL,
    [idCategoria]        INT  NOT NULL,
    [FechaMinima]        DATE NOT NULL,
    [FechaMaxima]        DATE NOT NULL,
    [Estado]             BIT  NOT NULL,
    CONSTRAINT [PK_DetalleCategoriaFecha_1] PRIMARY KEY CLUSTERED ([idCategoriaDetalle] ASC),
    CONSTRAINT [FK_DetalleCategoriaFecha_CategoriasDesagregacion] FOREIGN KEY ([idCategoria]) REFERENCES [dbo].[CategoriasDesagregacion] ([idCategoria])
);


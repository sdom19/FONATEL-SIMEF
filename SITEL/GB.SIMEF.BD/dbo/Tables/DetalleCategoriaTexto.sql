CREATE TABLE [dbo].[DetalleCategoriaTexto] (
    [idCategoriaDetalle] INT           IDENTITY (1, 1) NOT NULL,
    [idCategoria]        INT           NOT NULL,
    [Codigo]             INT           NULL,
    [Etiqueta]           VARCHAR (300) NOT NULL,
    [Estado]             BIT           NOT NULL,
    CONSTRAINT [PK_DetalleCategoriaTexto] PRIMARY KEY CLUSTERED ([idCategoriaDetalle] ASC, [idCategoria] ASC),
    CONSTRAINT [FK_DetalleCategoriaTexto_CategoriasDesagregacion] FOREIGN KEY ([idCategoria]) REFERENCES [dbo].[CategoriasDesagregacion] ([idCategoria])
);


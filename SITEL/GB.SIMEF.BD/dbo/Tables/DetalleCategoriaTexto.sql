CREATE TABLE [dbo].[DetalleCategoriaTexto] (
    [IdDetalleCategoriaTexto]  INT            IDENTITY (1, 1) NOT NULL,
    [IdCategoriaDesagregacion] INT            NOT NULL,
    [Codigo]                   VARCHAR (30)   NULL,
    [Etiqueta]                 VARCHAR (2000) NOT NULL,
    [Estado]                   BIT            NOT NULL,
    CONSTRAINT [PK_DetalleCategoriaTexto_1] PRIMARY KEY CLUSTERED ([IdDetalleCategoriaTexto] ASC, [IdCategoriaDesagregacion] ASC),
    CONSTRAINT [FK_DetalleCategoriaTexto_CategoriaDesagregacion] FOREIGN KEY ([IdCategoriaDesagregacion]) REFERENCES [dbo].[CategoriaDesagregacion] ([IdCategoriaDesagregacion])
);


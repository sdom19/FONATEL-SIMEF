CREATE TABLE [dbo].[DetalleRelacionCategoria] (
    [idDetalleRelacionCategoria] INT           NOT NULL,
    [IdRelacionCategoria]        INT           NOT NULL,
    [idCategoriaAtributo]        INT           NOT NULL,
    [CategoriaAtributoValor]     VARCHAR (300) NULL,
    [Estado]                     BIT           NOT NULL,
    CONSTRAINT [PK_DetalleRelacionCategoria_1] PRIMARY KEY CLUSTERED ([idDetalleRelacionCategoria] ASC, [IdRelacionCategoria] ASC),
    CONSTRAINT [FK_DetalleRelacionCategoria_RelacionCategoria1] FOREIGN KEY ([IdRelacionCategoria]) REFERENCES [dbo].[RelacionCategoria] ([idRelacionCategoria])
);


CREATE TABLE [dbo].[DetalleRelacionCategoria] (
    [IdDetalleRelacionCategoria] INT           IDENTITY (1, 1) NOT NULL,
    [IdRelacionCategoria]        INT           NOT NULL,
    [IdCategoriaAtributo]        INT           NOT NULL,
    [CategoriaAtributoValor]     VARCHAR (300) NULL,
    [Estado]                     BIT           NOT NULL,
    CONSTRAINT [PK_DetalleRelacionCategoria_1] PRIMARY KEY CLUSTERED ([IdDetalleRelacionCategoria] ASC, [IdRelacionCategoria] ASC),
    CONSTRAINT [FK_DetalleRelacionCategoria_RelacionCategoria1] FOREIGN KEY ([IdRelacionCategoria]) REFERENCES [dbo].[RelacionCategoria] ([IdRelacionCategoria])
);




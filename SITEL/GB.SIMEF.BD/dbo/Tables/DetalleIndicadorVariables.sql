CREATE TABLE [dbo].[DetalleIndicadorVariables] (
    [idDetalleIndicador] INT            IDENTITY (1, 1) NOT NULL,
    [idIndicador]        INT            NOT NULL,
    [idCategoria]        INT            NOT NULL,
    [NombreVariable]     VARCHAR (300)  NOT NULL,
    [Descripcion]        VARCHAR (3000) NOT NULL,
    [Estado]             BIT            NULL,
    CONSTRAINT [PK_DetalleIndicador] PRIMARY KEY CLUSTERED ([idDetalleIndicador] ASC, [idIndicador] ASC),
    CONSTRAINT [FK_DetalleIndicadorValores_Indicador] FOREIGN KEY ([idIndicador]) REFERENCES [dbo].[Indicador] ([idIndicador]),
    CONSTRAINT [FK_DetalleIndicadorVariables_CategoriasDesagregacion] FOREIGN KEY ([idCategoria]) REFERENCES [dbo].[CategoriasDesagregacion] ([idCategoria])
);


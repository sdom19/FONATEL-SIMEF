CREATE TABLE [dbo].[TipoDetalleCategoria] (
    [IdTipoDetalleCategoria] INT          NOT NULL,
    [Nombre]                 VARCHAR (30) NOT NULL,
    [TipoSQL]                VARCHAR (30) NULL,
    [Estado]                 BIT          NOT NULL,
    CONSTRAINT [PK_TiposCategoriaDesagregacion] PRIMARY KEY CLUSTERED ([IdTipoDetalleCategoria] ASC)
);


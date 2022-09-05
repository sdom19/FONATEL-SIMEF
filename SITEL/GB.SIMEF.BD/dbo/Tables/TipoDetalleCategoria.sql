CREATE TABLE [dbo].[TipoDetalleCategoria] (
    [idTipoCategoria] INT          NOT NULL,
    [Nombre]          VARCHAR (30) NOT NULL,
    [Estado]          BIT          NOT NULL,
    CONSTRAINT [PK_TiposCategoriaDesagregacion] PRIMARY KEY CLUSTERED ([idTipoCategoria] ASC)
);


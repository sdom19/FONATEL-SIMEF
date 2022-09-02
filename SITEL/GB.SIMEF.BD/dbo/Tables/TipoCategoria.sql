CREATE TABLE [dbo].[TipoCategoria] (
    [idTipoCategoria] INT          NOT NULL,
    [Nombre]          VARCHAR (30) NOT NULL,
    [Estado]          BIT          NOT NULL,
    CONSTRAINT [PK_TipoCategoria] PRIMARY KEY CLUSTERED ([idTipoCategoria] ASC)
);


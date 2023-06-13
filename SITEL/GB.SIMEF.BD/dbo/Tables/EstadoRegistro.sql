CREATE TABLE [dbo].[EstadoRegistro] (
    [IdEstadoRegistro] INT           NOT NULL,
    [Nombre]           VARCHAR (50)  NOT NULL,
    [Comentario]       VARCHAR (200) NULL,
    [Estado]           BIT           NOT NULL,
    CONSTRAINT [PK_EstadoRegistro] PRIMARY KEY CLUSTERED ([IdEstadoRegistro] ASC)
);


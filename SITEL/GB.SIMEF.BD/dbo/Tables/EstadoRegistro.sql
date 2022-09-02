CREATE TABLE [dbo].[EstadoRegistro] (
    [idEstado]    INT           NOT NULL,
    [Nombre]      VARCHAR (50)  NOT NULL,
    [Comentarios] VARCHAR (200) NULL,
    [Estado]      BIT           NOT NULL,
    CONSTRAINT [PK_EstadoRegistro] PRIMARY KEY CLUSTERED ([idEstado] ASC)
);


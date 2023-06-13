CREATE TABLE [dbo].[FuenteRegistro] (
    [IdFuenteRegistro]     INT           IDENTITY (1, 1) NOT NULL,
    [Fuente]               VARCHAR (500) NOT NULL,
    [CantidadDestinatario] INT           NOT NULL,
    [FechaCreacion]        DATETIME      NOT NULL,
    [UsuarioCreacion]      VARCHAR (100) NOT NULL,
    [FechaModificacion]    DATETIME      NULL,
    [UsuarioModificacion]  VARCHAR (100) NULL,
    [IdEstadoRegistro]     INT           NOT NULL,
    CONSTRAINT [PK_FuenteRegistro] PRIMARY KEY CLUSTERED ([IdFuenteRegistro] ASC),
    CONSTRAINT [FK_FuenteRegistro_EstadoRegistro] FOREIGN KEY ([IdEstadoRegistro]) REFERENCES [dbo].[EstadoRegistro] ([IdEstadoRegistro])
);


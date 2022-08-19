CREATE TABLE [dbo].[FuentesRegistro] (
    [idFuente]             INT           IDENTITY (1, 1) NOT NULL,
    [Fuente]               VARCHAR (300) NOT NULL,
    [CantidadDestinatario] INT           NOT NULL,
    [FechaCreacion]        DATETIME      NOT NULL,
    [UsuarioCreacion]      VARCHAR (100) NOT NULL,
    [FechaModificacion]    DATETIME      NULL,
    [UsuarioModificacion]  VARCHAR (100) NULL,
    [idEstado]             INT           NOT NULL,
    CONSTRAINT [PK_FuentesRegistro] PRIMARY KEY CLUSTERED ([idFuente] ASC),
    CONSTRAINT [FK_FuentesRegistro_EstadoRegistro] FOREIGN KEY ([idEstado]) REFERENCES [dbo].[EstadoRegistro] ([idEstado])
);


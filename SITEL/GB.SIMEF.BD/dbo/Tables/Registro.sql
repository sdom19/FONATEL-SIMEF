CREATE TABLE [dbo].[Registro] (
    [idRegistro]          INT           NOT NULL,
    [IdFormulario]        INT           NOT NULL,
    [IdSolicitud]         INT           NOT NULL,
    [FechaIncio]          DATE          NOT NULL,
    [FechaFin]            DATE          NULL,
    [FechaCreacion]       DATETIME      NOT NULL,
    [UsuarioCreacion]     VARCHAR (100) NULL,
    [FechaModificacion]   DATETIME      NULL,
    [UsuarioModificacion] VARCHAR (100) NULL,
    [idEstado]            INT           NOT NULL,
    [Activo]              BIT           NULL,
    CONSTRAINT [PK_RegistroIndicardor] PRIMARY KEY CLUSTERED ([idRegistro] ASC),
    CONSTRAINT [FK_Registro_DetalleSolicitudFormulario] FOREIGN KEY ([IdSolicitud], [IdFormulario]) REFERENCES [dbo].[DetalleSolicitudFormulario] ([IdSolicitud], [IdFormulario]),
    CONSTRAINT [FK_Registro_EstadoRegistro] FOREIGN KEY ([idEstado]) REFERENCES [dbo].[EstadoRegistro] ([idEstado])
);


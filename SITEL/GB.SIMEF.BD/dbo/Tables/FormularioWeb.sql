CREATE TABLE [dbo].[FormularioWeb] (
    [IdFormularioWeb]     INT            IDENTITY (1, 1) NOT NULL,
    [Codigo]              VARCHAR (50)   NULL,
    [Nombre]              VARCHAR (500)  NULL,
    [Descripcion]         VARCHAR (3000) NOT NULL,
    [CantidadIndicador]   INT            NOT NULL,
    [IdFrecuenciaEnvio]   INT            NOT NULL,
    [FechaCreacion]       DATETIME       NOT NULL,
    [UsuarioCreacion]     VARCHAR (100)  NOT NULL,
    [FechaModificacion]   DATETIME       NULL,
    [UsuarioModificacion] VARCHAR (100)  NULL,
    [IdEstadoRegistro]    INT            NOT NULL,
    CONSTRAINT [PK_FormularioWeb] PRIMARY KEY CLUSTERED ([IdFormularioWeb] ASC),
    CONSTRAINT [FK_FormularioWeb_EstadoRegistro] FOREIGN KEY ([IdEstadoRegistro]) REFERENCES [dbo].[EstadoRegistro] ([IdEstadoRegistro]),
    CONSTRAINT [FK_FormularioWeb_FrecuenciaEnvio] FOREIGN KEY ([IdFrecuenciaEnvio]) REFERENCES [dbo].[FrecuenciaEnvio] ([IdFrecuenciaEnvio])
);


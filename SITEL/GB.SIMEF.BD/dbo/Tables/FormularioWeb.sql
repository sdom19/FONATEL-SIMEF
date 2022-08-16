CREATE TABLE [dbo].[FormularioWeb] (
    [idFormulario]        INT           NOT NULL,
    [Codigo]              NCHAR (10)    NULL,
    [Nombre]              NCHAR (10)    NOT NULL,
    [Descripcion]         VARCHAR (300) NOT NULL,
    [CantidadIndicadores] INT           NOT NULL,
    [idFrecuencia]        INT           NOT NULL,
    [FechaCreacion]       DATETIME      NOT NULL,
    [UsuarioCreacion]     VARCHAR (100) NOT NULL,
    [FechaModificacion]   DATETIME      NULL,
    [UsuarioModificacion] VARCHAR (100) NULL,
    [IdEstado]            INT           NOT NULL,
    CONSTRAINT [PK_FormularioWeb] PRIMARY KEY CLUSTERED ([idFormulario] ASC),
    CONSTRAINT [FK_FormularioWeb_EstadoRegistro] FOREIGN KEY ([IdEstado]) REFERENCES [dbo].[EstadoRegistro] ([idEstado]),
    CONSTRAINT [FK_FormularioWeb_FrecuenciaEnvio] FOREIGN KEY ([idFrecuencia]) REFERENCES [dbo].[FrecuenciaEnvio] ([idFrecuencia])
);


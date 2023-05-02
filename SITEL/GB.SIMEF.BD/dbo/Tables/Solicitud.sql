CREATE TABLE [dbo].[Solicitud] (
    [idSolicitud]         INT            IDENTITY (1, 1) NOT NULL,
    [Codigo]              VARCHAR (30)   NULL,
    [Nombre]              VARCHAR (300)  NOT NULL,
    [FechaInicio]         DATE           NOT NULL,
    [FechaFin]            DATE           NOT NULL,
    [IdMes]               INT            NOT NULL,
    [IdAnno]              INT            NOT NULL,
    [CantidadFormularios] INT            NULL,
    [idFuente]            INT            NULL,
    [IdFrecuenciaEnvio]   INT            NULL,
    [Mensaje]             VARCHAR (3000) NOT NULL,
    [FechaCreacion]       DATETIME       CONSTRAINT [DF_Solicitud_FechaCreacion] DEFAULT (getdate()) NOT NULL,
    [UsuarioCreacion]     VARCHAR (100)  NOT NULL,
    [FechaModificacion]   DATETIME       NULL,
    [UsuarioModificacion] VARCHAR (100)  NULL,
    [IdEstado]            INT            NOT NULL,
    CONSTRAINT [PK_Solicitud] PRIMARY KEY CLUSTERED ([idSolicitud] ASC),
    CONSTRAINT [FK_Solicitud_Anno] FOREIGN KEY ([IdAnno]) REFERENCES [dbo].[Anno] ([idAnno]),
    CONSTRAINT [FK_Solicitud_EstadoRegistro] FOREIGN KEY ([IdEstado]) REFERENCES [dbo].[EstadoRegistro] ([idEstado]),
    CONSTRAINT [FK_Solicitud_FuentesRegistro] FOREIGN KEY ([idFuente]) REFERENCES [dbo].[FuentesRegistro] ([idFuente]),
    CONSTRAINT [FK_Solicitud_Mes] FOREIGN KEY ([IdMes]) REFERENCES [dbo].[Mes] ([idMes])
);


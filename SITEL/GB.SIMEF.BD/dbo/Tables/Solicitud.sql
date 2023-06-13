CREATE TABLE [dbo].[Solicitud] (
    [IdSolicitud]         INT            IDENTITY (1, 1) NOT NULL,
    [Codigo]              VARCHAR (30)   NULL,
    [Nombre]              VARCHAR (500)  NOT NULL,
    [FechaInicio]         DATE           NULL,
    [FechaFin]            DATE           NULL,
    [IdMes]               INT            NOT NULL,
    [IdAnno]              INT            NOT NULL,
    [CantidadFormulario]  INT            NULL,
    [IdFuenteRegistro]    INT            NULL,
    [IdFrecuenciaEnvio]   INT            CONSTRAINT [DF_Solicitud_IdFrecuenciaEnvio] DEFAULT ((3)) NULL,
    [Mensaje]             VARCHAR (3000) NOT NULL,
    [FechaCreacion]       DATETIME       CONSTRAINT [DF_Solicitud_FechaCreacion] DEFAULT (getdate()) NOT NULL,
    [UsuarioCreacion]     VARCHAR (100)  NOT NULL,
    [FechaModificacion]   DATETIME       NULL,
    [UsuarioModificacion] VARCHAR (100)  NULL,
    [IdEstadoRegistro]    INT            NOT NULL,
    CONSTRAINT [PK_Solicitud] PRIMARY KEY CLUSTERED ([IdSolicitud] ASC),
    CONSTRAINT [FK_Solicitud_Anno] FOREIGN KEY ([IdAnno]) REFERENCES [dbo].[Anno] ([IdAnno]),
    CONSTRAINT [FK_Solicitud_EstadoRegistro] FOREIGN KEY ([IdEstadoRegistro]) REFERENCES [dbo].[EstadoRegistro] ([IdEstadoRegistro]),
    CONSTRAINT [FK_Solicitud_FuenteRegistro] FOREIGN KEY ([IdFuenteRegistro]) REFERENCES [dbo].[FuenteRegistro] ([IdFuenteRegistro]),
    CONSTRAINT [FK_Solicitud_Mes] FOREIGN KEY ([IdMes]) REFERENCES [dbo].[Mes] ([IdMes])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fk a la tabla de estado de registro, determina que estado tiene la solicitud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'IdEstadoRegistro';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'usuario que la modifica ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'UsuarioModificacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'última modificación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'FechaModificacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'usuario que la creo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'UsuarioCreacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fecha de creación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'FechaCreacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'mensaje descripto por el operador simef ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'Mensaje';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'frecuencia con la que se repetira el envío', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'IdFrecuenciaEnvio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fk fuentes de registro, determina los correos a los que llegará lo solicitud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'IdFuenteRegistro';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'cantidad de formularios permitidos en una solicitud ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'CantidadFormulario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fk catalogo de años ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'IdAnno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fk catalogo de mes ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'IdMes';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fecha de finalización de la solicitud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'FechaFin';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fecha de inicio de la solicitud ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'FechaInicio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Nombre de la solicitud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'código unico en el sistema para identificar la solicitud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'Codigo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'llave primaria ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Solicitud', @level2type = N'COLUMN', @level2name = N'IdSolicitud';


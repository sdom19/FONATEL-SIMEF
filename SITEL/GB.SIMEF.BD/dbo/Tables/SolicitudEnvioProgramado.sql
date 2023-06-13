CREATE TABLE [dbo].[SolicitudEnvioProgramado] (
    [IdSolicitudEnvioProgramado] INT  IDENTITY (1, 1) NOT NULL,
    [IdSolicitud]                INT  NOT NULL,
    [IdFrecuenciaEnvio]          INT  NOT NULL,
    [CantidadRepeticion]         INT  NOT NULL,
    [FechaCiclo]                 DATE NOT NULL,
    [Estado]                     BIT  NOT NULL,
    CONSTRAINT [PK_SolicitudEnvioProgramado] PRIMARY KEY CLUSTERED ([IdSolicitudEnvioProgramado] ASC),
    CONSTRAINT [FK_SolicitudEnvioProgramado_FrecuenciaEnvio] FOREIGN KEY ([IdFrecuenciaEnvio]) REFERENCES [dbo].[FrecuenciaEnvio] ([IdFrecuenciaEnvio]),
    CONSTRAINT [FK_SolicitudEnvioProgramado_Solicitud] FOREIGN KEY ([IdSolicitud]) REFERENCES [dbo].[Solicitud] ([IdSolicitud])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Estado de la solicitud enviada 1 pendiente 0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudEnvioProgramado', @level2type = N'COLUMN', @level2name = N'Estado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fecha de inicio del ciclo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudEnvioProgramado', @level2type = N'COLUMN', @level2name = N'FechaCiclo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'cantidad de repeticiones una una solicitud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudEnvioProgramado', @level2type = N'COLUMN', @level2name = N'CantidadRepeticion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fk con la tabla frecuencia de envío', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudEnvioProgramado', @level2type = N'COLUMN', @level2name = N'IdFrecuenciaEnvio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'fk con solicitud de información', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudEnvioProgramado', @level2type = N'COLUMN', @level2name = N'IdSolicitud';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'llave primaria ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudEnvioProgramado', @level2type = N'COLUMN', @level2name = N'IdSolicitudEnvioProgramado';


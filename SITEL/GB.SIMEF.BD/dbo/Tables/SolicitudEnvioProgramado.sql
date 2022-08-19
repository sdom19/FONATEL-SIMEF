CREATE TABLE [dbo].[SolicitudEnvioProgramado] (
    [IdEnvioProgramado]    NCHAR (10) NOT NULL,
    [idSolicitud]          INT        NOT NULL,
    [idFrecuencia]         INT        NOT NULL,
    [CantidadRepiticiones] INT        NOT NULL,
    [Dia]                  INT        NOT NULL,
    [idMes]                INT        NOT NULL,
    [FechaCiclo]           DATE       NOT NULL,
    [Estado]               BIT        NOT NULL,
    CONSTRAINT [PK_SolicitudEnvioProgramado] PRIMARY KEY CLUSTERED ([IdEnvioProgramado] ASC),
    CONSTRAINT [FK_SolicitudEnvioProgramado_FrecuenciaEnvio] FOREIGN KEY ([idFrecuencia]) REFERENCES [dbo].[FrecuenciaEnvio] ([idFrecuencia]),
    CONSTRAINT [FK_SolicitudEnvioProgramado_Solicitud] FOREIGN KEY ([idSolicitud]) REFERENCES [dbo].[Solicitud] ([idSolicitud])
);


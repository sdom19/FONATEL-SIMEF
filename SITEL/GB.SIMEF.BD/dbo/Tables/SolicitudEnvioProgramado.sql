CREATE TABLE [dbo].[SolicitudEnvioProgramado] (
    [IdEnvioProgramado]    INT  NOT NULL,
    [IdSolicitud]          INT  NOT NULL,
    [IdFrecuencia]         INT  NOT NULL,
    [CantidadRepiticiones] INT  NOT NULL,
    [Dia]                  INT  NOT NULL,
    [IdMes]                INT  NOT NULL,
    [FechaCiclo]           DATE NOT NULL,
    [Estado]               BIT  NOT NULL,
    CONSTRAINT [PK_SolicitudEnvioProgramado] PRIMARY KEY CLUSTERED ([IdEnvioProgramado] ASC),
    CONSTRAINT [FK_SolicitudEnvioProgramado_FrecuenciaEnvio] FOREIGN KEY ([IdFrecuencia]) REFERENCES [dbo].[FrecuenciaEnvio] ([IdFrecuencia]),
    CONSTRAINT [FK_SolicitudEnvioProgramado_Solicitud] FOREIGN KEY ([IdSolicitud]) REFERENCES [dbo].[Solicitud] ([IdSolicitud])
);




CREATE TABLE [dbo].[EnvioSolicitud] (
    [IdEnvioSolicitud] INT            IDENTITY (1, 1) NOT NULL,
    [Fecha]            DATE           NOT NULL,
    [IdSolicitud]      INT            NOT NULL,
    [Enviado]          BIT            NOT NULL,
    [EnvioProgramado]  BIT            NOT NULL,
    [MensajeError]     VARCHAR (2000) NOT NULL,
    CONSTRAINT [PK_EnvioSolicitud_1] PRIMARY KEY CLUSTERED ([IdEnvioSolicitud] ASC),
    CONSTRAINT [FK_EnvioSolicitud_Solicitud] FOREIGN KEY ([IdSolicitud]) REFERENCES [dbo].[Solicitud] ([IdSolicitud])
);


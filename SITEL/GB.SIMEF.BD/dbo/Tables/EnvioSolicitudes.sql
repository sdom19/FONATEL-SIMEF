CREATE TABLE [dbo].[EnvioSolicitudes] (
    [idEnvio]         INT            NOT NULL,
    [Fecha]           DATE           NOT NULL,
    [idSolicitud]     INT            NULL,
    [Enviado]         BIT            NOT NULL,
    [EnvioProgramado] BIT            NOT NULL,
    [MensajError]     VARCHAR (2000) NOT NULL,
    CONSTRAINT [PK_EnvioSolicitudes] PRIMARY KEY CLUSTERED ([idEnvio] ASC),
    CONSTRAINT [FK_EnvioSolicitudes_Solicitud] FOREIGN KEY ([idSolicitud]) REFERENCES [dbo].[Solicitud] ([idSolicitud])
);


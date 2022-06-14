CREATE TABLE [dbo].[SolicitudGeneral] (
    [IdSolicitud]         INT           IDENTITY (1, 1) NOT NULL,
    [IdUsuario]           INT           NOT NULL,
    [Descripcion]         VARCHAR (250) NOT NULL,
    [FechaInicio]         DATE          NOT NULL,
    [FechaFinal]          DATE          NOT NULL,
    [Activo]              BIT           NOT NULL,
    [Path]                VARCHAR (MAX) NOT NULL,
    [NotificacionEnviada] BIT           NOT NULL,
    [Borrado]             BIT           NOT NULL,
    CONSTRAINT [PK_SolicitudGeneral] PRIMARY KEY CLUSTERED ([IdSolicitud] ASC),
    CONSTRAINT [FK_Usuarios] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuario] ([IdUsuario])
);


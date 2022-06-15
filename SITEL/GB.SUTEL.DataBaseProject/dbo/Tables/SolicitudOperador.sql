CREATE TABLE [dbo].[SolicitudOperador] (
    [IdSolicitud] INT           NOT NULL,
    [IdUsuario]   INT           NOT NULL,
    [Path]        VARCHAR (MAX) NULL,
    [Respuesta]   BIT           NOT NULL,
    CONSTRAINT [PK_SolicitudOperador] PRIMARY KEY CLUSTERED ([IdSolicitud] ASC, [IdUsuario] ASC),
    CONSTRAINT [FK_Solicitud] FOREIGN KEY ([IdSolicitud]) REFERENCES [dbo].[SolicitudGeneral] ([IdSolicitud]),
    CONSTRAINT [FK_Usuario_Respuesta] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuario] ([IdUsuario])
);


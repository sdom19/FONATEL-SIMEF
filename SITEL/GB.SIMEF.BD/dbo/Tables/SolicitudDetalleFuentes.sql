CREATE TABLE [dbo].[SolicitudDetalleFuentes] (
    [IdSolicitud] INT NOT NULL,
    [idFuente]    INT NOT NULL,
    CONSTRAINT [PK_SolicitudDetalleFuentes] PRIMARY KEY CLUSTERED ([IdSolicitud] ASC, [idFuente] ASC),
    CONSTRAINT [FK_SolicitudDetalleFuentes_FuentesRegistro] FOREIGN KEY ([idFuente]) REFERENCES [dbo].[FuentesRegistro] ([idFuente])
);


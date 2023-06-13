CREATE TABLE [dbo].[DetalleSolicitudFormulario] (
    [IdSolicitud]     INT NOT NULL,
    [IdFormularioWeb] INT NOT NULL,
    [Estado]          BIT NOT NULL,
    CONSTRAINT [PK_SolicitudDetalleFormulario] PRIMARY KEY CLUSTERED ([IdSolicitud] ASC, [IdFormularioWeb] ASC),
    CONSTRAINT [FK_SolicitudDetalle_Solicitud] FOREIGN KEY ([IdSolicitud]) REFERENCES [dbo].[Solicitud] ([IdSolicitud]),
    CONSTRAINT [FK_SolicitudDetalleFormulario_FormularioWeb] FOREIGN KEY ([IdFormularioWeb]) REFERENCES [dbo].[FormularioWeb] ([IdFormularioWeb])
);


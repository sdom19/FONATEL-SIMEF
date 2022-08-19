CREATE TABLE [dbo].[DetalleSolicitudFormulario] (
    [IdSolicitud]  INT NOT NULL,
    [IdFormulario] INT NOT NULL,
    [Estado]       BIT NOT NULL,
    CONSTRAINT [PK_SolicitudDetalleFormulario] PRIMARY KEY CLUSTERED ([IdSolicitud] ASC, [IdFormulario] ASC),
    CONSTRAINT [FK_SolicitudDetalle_Solicitud] FOREIGN KEY ([IdSolicitud]) REFERENCES [dbo].[Solicitud] ([idSolicitud]),
    CONSTRAINT [FK_SolicitudDetalleFormulario_FormularioWeb] FOREIGN KEY ([IdFormulario]) REFERENCES [dbo].[FormularioWeb] ([idFormulario])
);


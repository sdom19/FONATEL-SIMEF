CREATE TABLE [dbo].[SolicitudIndicador] (
    [IdSolicitudIndicador]    UNIQUEIDENTIFIER CONSTRAINT [DF__SOLICITUD__IDSOL__6C190EBB] DEFAULT (newid()) NOT NULL,
    [IdFrecuencia]            INT              NOT NULL,
    [IdServicio]              INT              NOT NULL,
    [IdDireccion]             INT              NOT NULL,
    [DescFormulario]          VARCHAR (500)    NOT NULL,
    [FechaInicio]             DATETIME         NOT NULL,
    [FechaFin]                DATETIME         NOT NULL,
    [Activo]                  TINYINT          NOT NULL,
    [Borrado]                 TINYINT          NOT NULL,
    [FechaBaseParaCrearExcel] DATETIME         NULL,
    CONSTRAINT [PK_SOLICITUDINDICADOR] PRIMARY KEY CLUSTERED ([IdSolicitudIndicador] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SOLICITU_REFERENCE_DIRECCIO] FOREIGN KEY ([IdDireccion]) REFERENCES [dbo].[Direccion] ([IdDireccion]),
    CONSTRAINT [FK_SOLICITU_REFERENCE_FRECUENC] FOREIGN KEY ([IdFrecuencia]) REFERENCES [dbo].[Frecuencia] ([IdFrecuencia]),
    CONSTRAINT [FK_SOLICITU_REFERENCE_SERVICIO] FOREIGN KEY ([IdServicio]) REFERENCES [dbo].[Servicio] ([IdServicio])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudIndicador', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica si esta activa la solicitud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudIndicador', @level2type = N'COLUMN', @level2name = N'Activo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Fecha fin del rango se la solicitud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudIndicador', @level2type = N'COLUMN', @level2name = N'FechaFin';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Fecha de inicio del periodo de la solicitud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudIndicador', @level2type = N'COLUMN', @level2name = N'FechaInicio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción del formulario', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudIndicador', @level2type = N'COLUMN', @level2name = N'DescFormulario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de direccion', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudIndicador', @level2type = N'COLUMN', @level2name = N'IdDireccion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de servicio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudIndicador', @level2type = N'COLUMN', @level2name = N'IdServicio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la frecuencia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudIndicador', @level2type = N'COLUMN', @level2name = N'IdFrecuencia';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la solicitud de indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudIndicador', @level2type = N'COLUMN', @level2name = N'IdSolicitudIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Registro de las solicitudes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudIndicador';


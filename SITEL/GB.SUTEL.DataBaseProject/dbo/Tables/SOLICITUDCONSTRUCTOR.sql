CREATE TABLE [dbo].[SolicitudConstructor] (
    [IdSolicitudContructor] UNIQUEIDENTIFIER NOT NULL,
    [IdSolicitudIndicador]  UNIQUEIDENTIFIER NOT NULL,
    [IdConstructor]         UNIQUEIDENTIFIER NOT NULL,
    [IdEstado]              INT              NOT NULL,
    [IdOperador]            VARCHAR (20)     NOT NULL,
    [Borrado]               TINYINT          NOT NULL,
    [OrdenIndicadorEnExcel] INT              NULL,
    CONSTRAINT [PK_SOLICITUDCONSTRUCTOR] PRIMARY KEY CLUSTERED ([IdSolicitudContructor] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_SOLICITU_REFERENCE_ESTADOSO] FOREIGN KEY ([IdEstado]) REFERENCES [dbo].[EstadoSolicitud] ([IdEstado]),
    CONSTRAINT [FK_SOLICITU_REFERENCE_OPERADOR] FOREIGN KEY ([IdOperador]) REFERENCES [dbo].[Operador] ([IdOperador]),
    CONSTRAINT [FK_SOLICITU_REFERENCE_SOLICITU] FOREIGN KEY ([IdSolicitudIndicador]) REFERENCES [dbo].[SolicitudIndicador] ([IdSolicitudIndicador]),
    CONSTRAINT [FK_SolicitudConstructor_Constructor] FOREIGN KEY ([IdConstructor]) REFERENCES [dbo].[Constructor] ([IdConstructor])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudConstructor', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de operador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudConstructor', @level2type = N'COLUMN', @level2name = N'IdOperador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del estado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudConstructor', @level2type = N'COLUMN', @level2name = N'IdEstado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del constructor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudConstructor', @level2type = N'COLUMN', @level2name = N'IdConstructor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la solicitud de indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudConstructor', @level2type = N'COLUMN', @level2name = N'IdSolicitudIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de solicitud de constructor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudConstructor', @level2type = N'COLUMN', @level2name = N'IdSolicitudContructor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Relación de los contructores con la solicitud y el operador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SolicitudConstructor';


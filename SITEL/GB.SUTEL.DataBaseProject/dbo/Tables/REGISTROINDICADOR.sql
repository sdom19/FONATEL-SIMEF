CREATE TABLE [dbo].[RegistroIndicador] (
    [IdRegistroIndicador]    UNIQUEIDENTIFIER CONSTRAINT [DF__REGISTROI__IDREG__7F2BE32F] DEFAULT (newid()) NOT NULL,
    [IdSolicitudConstructor] UNIQUEIDENTIFIER NOT NULL,
    [IdUsuario]              INT              NOT NULL,
    [FechaRegistroIndicador] DATETIME         NOT NULL,
    [Comentario]             VARCHAR (550)    NOT NULL,
    [Justificado]            VARCHAR (550)    NOT NULL,
    [Bloqueado]              TINYINT          NOT NULL,
    [Borrado]                TINYINT          NOT NULL,
    [Observacion]            VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_REGISTROINDICADOR] PRIMARY KEY CLUSTERED ([IdRegistroIndicador] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_REGISTRO_REFERENCE_SOLICITU] FOREIGN KEY ([IdSolicitudConstructor]) REFERENCES [dbo].[SolicitudConstructor] ([IdSolicitudContructor]),
    CONSTRAINT [FK_REGISTRO_REFERENCE_USUARIO] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuario] ([IdUsuario])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico del registro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicador', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica si el registro esta bloqueado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicador', @level2type = N'COLUMN', @level2name = N'Bloqueado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Justificación del registro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicador', @level2type = N'COLUMN', @level2name = N'Justificado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Comentario del registro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicador', @level2type = N'COLUMN', @level2name = N'Comentario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Fecha en que se registro el indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicador', @level2type = N'COLUMN', @level2name = N'FechaRegistroIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Id del usuario que subio el registro de indicador(excel)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicador', @level2type = N'COLUMN', @level2name = N'IdUsuario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la solicitud del constructor relacionado con el registro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicador', @level2type = N'COLUMN', @level2name = N'IdSolicitudConstructor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del registro del indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicador', @level2type = N'COLUMN', @level2name = N'IdRegistroIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Encabezado del registro del indicador externo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicador';


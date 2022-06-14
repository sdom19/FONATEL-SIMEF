CREATE TABLE [dbo].[BitacoraIndicador] (
    [IdBitacoIndicador]          UNIQUEIDENTIFIER CONSTRAINT [DF__BITACORAI__IDBIT__76619304] DEFAULT (newid()) NOT NULL,
    [IdDetalleRegistroIndicador] UNIQUEIDENTIFIER NOT NULL,
    [IdUsuario]                  INT              NOT NULL,
    [ValorAnterior]              VARCHAR (250)    NOT NULL,
    [ValorNuevo]                 VARCHAR (250)    NOT NULL,
    [Justificacion]              NVARCHAR (1000)  NOT NULL,
    [FechaModificacion]          DATETIME         NOT NULL,
    CONSTRAINT [PK_BITACORAINDICADOR] PRIMARY KEY CLUSTERED ([IdBitacoIndicador] ASC),
    CONSTRAINT [FK_BITACORA_REFERENCE_DETALLER] FOREIGN KEY ([IdDetalleRegistroIndicador]) REFERENCES [dbo].[DetalleRegistroIndicador] ([IdDetalleRegistroindicador]),
    CONSTRAINT [FK_BITACORA_REFERENCE_USUARIO] FOREIGN KEY ([IdUsuario]) REFERENCES [dbo].[Usuario] ([IdUsuario])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'fecha en que se modifico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraIndicador', @level2type = N'COLUMN', @level2name = N'FechaModificacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Justificación del porque se da el cambio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraIndicador', @level2type = N'COLUMN', @level2name = N'Justificacion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'valor que tiene ahora el registro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraIndicador', @level2type = N'COLUMN', @level2name = N'ValorNuevo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'valor que tenía el registro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraIndicador', @level2type = N'COLUMN', @level2name = N'ValorAnterior';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del usuario que hizo la modificación', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraIndicador', @level2type = N'COLUMN', @level2name = N'IdUsuario';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'ID del detalle de registro de indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraIndicador', @level2type = N'COLUMN', @level2name = N'IdDetalleRegistroIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del registro', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraIndicador', @level2type = N'COLUMN', @level2name = N'IdBitacoIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Contiene los detalles de registro de indicador que se modificaron', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BitacoraIndicador';


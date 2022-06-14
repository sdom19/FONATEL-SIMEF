CREATE TABLE [dbo].[Constructor] (
    [IdConstructor]            UNIQUEIDENTIFIER CONSTRAINT [DF__CONSTRUCT__IDCON__21B6055D] DEFAULT (newid()) NOT NULL,
    [IdIndicador]              NVARCHAR (50)    NOT NULL,
    [IdFrecuencia]             INT              NOT NULL,
    [IdDesglose]               INT              NULL,
    [FechaCreacionConstructor] DATETIME         NOT NULL,
    [Borrado]                  TINYINT          NOT NULL,
    [IdDireccion]              INT              NULL,
    CONSTRAINT [PK_CONSTRUCTOR] PRIMARY KEY CLUSTERED ([IdConstructor] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CONSTRUC_REFERENCE_DIRECCIO] FOREIGN KEY ([IdDireccion]) REFERENCES [dbo].[Direccion] ([IdDireccion]),
    CONSTRAINT [FK_CONSTRUC_REFERENCE_FRECUENC] FOREIGN KEY ([IdFrecuencia]) REFERENCES [dbo].[Frecuencia] ([IdFrecuencia]),
    CONSTRAINT [FK_CONSTRUC_REFERENCE_FRECUENC2] FOREIGN KEY ([IdDesglose]) REFERENCES [dbo].[Frecuencia] ([IdFrecuencia]),
    CONSTRAINT [FK_CONSTRUC_REFERENCE_INDICADO] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id de la dirección al que pertenece el constructor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Constructor', @level2type = N'COLUMN', @level2name = N'IdDireccion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'indica si esta borrado logicamente', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Constructor', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Fecha de registro del constructor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Constructor', @level2type = N'COLUMN', @level2name = N'FechaCreacionConstructor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al desglose(frecuencia)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Constructor', @level2type = N'COLUMN', @level2name = N'IdDesglose';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia a la frecuencia', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Constructor', @level2type = N'COLUMN', @level2name = N'IdFrecuencia';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'referencia al Indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Constructor', @level2type = N'COLUMN', @level2name = N'IdIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del constructor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Constructor', @level2type = N'COLUMN', @level2name = N'IdConstructor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Configuración del indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Constructor';


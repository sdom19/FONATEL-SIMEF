CREATE TABLE [dbo].[RegistroIndicadorExterno] (
    [IdRegistroIndicadorExterno] UNIQUEIDENTIFIER CONSTRAINT [DF__REGISTROI__IDREG__59FA5E80] DEFAULT (newid()) NOT NULL,
    [IdPeridiocidad]             INT              NOT NULL,
    [IdRegionIndicadorExterno]   INT              NOT NULL,
    [IdZonaIndicadorExterno]     INT              NOT NULL,
    [IdIndicadorExterno]         VARCHAR (50)     NOT NULL,
    [IdGenero]                   INT              NOT NULL,
    [ValorIndicador]             FLOAT (53)       NOT NULL,
    [Borrado]                    BIT              NOT NULL,
    [Anno]                       INT              NOT NULL,
    [IdCanton]                   INT              NOT NULL,
    [IdTrimestre]                INT              NOT NULL,
    [Modificado]                 TINYINT          NOT NULL,
    [FechaModificacion]          DATETIME         NULL,
    CONSTRAINT [PK__REGISTRO__6D5528C735EC9A80] PRIMARY KEY CLUSTERED ([IdRegistroIndicadorExterno] ASC),
    CONSTRAINT [FK__RegistroI__IdInd__0B5CAFEA] FOREIGN KEY ([IdIndicadorExterno]) REFERENCES [dbo].[IndicadorExterno] ([IdIndicadorExterno]),
    CONSTRAINT [FK_REGISTRO_REFERENCE_CANTON] FOREIGN KEY ([IdCanton]) REFERENCES [dbo].[Canton] ([IdCanton]),
    CONSTRAINT [FK_REGISTRO_REFERENCE_GENERO] FOREIGN KEY ([IdGenero]) REFERENCES [dbo].[Genero] ([IdGenero]),
    CONSTRAINT [FK_REGISTRO_REFERENCE_PERIODIC] FOREIGN KEY ([IdPeridiocidad]) REFERENCES [dbo].[Periodicidad] ([IdPeridiocidad]),
    CONSTRAINT [FK_REGISTRO_REFERENCE_REGIONIN] FOREIGN KEY ([IdRegionIndicadorExterno]) REFERENCES [dbo].[RegionIndicadorExterno] ([IdRegionIndicadorExterno]),
    CONSTRAINT [FK_REGISTRO_REFERENCE_TRIMESTR] FOREIGN KEY ([IdTrimestre]) REFERENCES [dbo].[Trimestre] ([IdTrimestre]),
    CONSTRAINT [FK_REGISTRO_REFERENCE_ZONAINDI] FOREIGN KEY ([IdZonaIndicadorExterno]) REFERENCES [dbo].[ZonaIndicadorExterno] ([IdZonaIndicadorExterno])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del trimestre', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdTrimestre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de cantón', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdCanton';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Año de registro del indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno', @level2type = N'COLUMN', @level2name = N'Anno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado logico', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Valor del indicador externo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno', @level2type = N'COLUMN', @level2name = N'ValorIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de genero', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdGenero';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de indicador externo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdIndicadorExterno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de zona', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdZonaIndicadorExterno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de region ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdRegionIndicadorExterno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de la periodicidad', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdPeridiocidad';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo del registro de indicador externo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno', @level2type = N'COLUMN', @level2name = N'IdRegistroIndicadorExterno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Registro del indicador externo por parte de SUTEL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'RegistroIndicadorExterno';


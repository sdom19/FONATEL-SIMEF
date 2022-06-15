CREATE TABLE [dbo].[Criterio] (
    [IdCriterio]   VARCHAR (50)  NOT NULL,
    [IdDireccion]  INT           NULL,
    [DescCriterio] VARCHAR (350) NULL,
    [Borrado]      TINYINT       NULL,
    [IdIndicador]  NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CRITERIO] PRIMARY KEY CLUSTERED ([IdCriterio] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK__CRITERIO__IDINDI__0D7A0286] FOREIGN KEY ([IdIndicador]) REFERENCES [dbo].[Indicador] ([IdIndicador]),
    CONSTRAINT [FK_CRITERIO_REFERENCE_DIRECCIO] FOREIGN KEY ([IdDireccion]) REFERENCES [dbo].[Direccion] ([IdDireccion])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id al que pertenece el indicador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Criterio', @level2type = N'COLUMN', @level2name = N'IdIndicador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'indica si esta borrado logicamente', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Criterio', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción del criterio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Criterio', @level2type = N'COLUMN', @level2name = N'DescCriterio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia a la dirección a la que pertenece', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Criterio', @level2type = N'COLUMN', @level2name = N'IdDireccion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Id del criterio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Criterio', @level2type = N'COLUMN', @level2name = N'IdCriterio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Contiene los criterios', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Criterio';


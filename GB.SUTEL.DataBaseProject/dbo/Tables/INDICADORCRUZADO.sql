CREATE TABLE [dbo].[IndicadorCruzado] (
    [IdIndicadorCruzado] NVARCHAR (50)   NOT NULL,
    [Nombre]             NVARCHAR (500)  NOT NULL,
    [Descripcion]        NVARCHAR (3000) NOT NULL,
    CONSTRAINT [PK_INDICADORCRUZADO] PRIMARY KEY CLUSTERED ([IdIndicadorCruzado] ASC) WITH (FILLFACTOR = 90)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción del indicador cruzado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorCruzado', @level2type = N'COLUMN', @level2name = N'Descripcion';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'nombre del indicador cruzado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorCruzado', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del indicador cruzado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorCruzado', @level2type = N'COLUMN', @level2name = N'IdIndicadorCruzado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descripción del indicador cruzado', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IndicadorCruzado';


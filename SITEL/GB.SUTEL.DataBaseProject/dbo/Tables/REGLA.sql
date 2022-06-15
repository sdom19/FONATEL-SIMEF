CREATE TABLE [dbo].[Regla] (
    [IdConstructorCriterio] UNIQUEIDENTIFIER NOT NULL,
    [ValorLimiteInferior]   NVARCHAR (50)    NOT NULL,
    [ValorLimiteSuperior]   NVARCHAR (50)    NOT NULL,
    [FechaCreacionRegla]    DATETIME         NOT NULL,
    [Borrado]               TINYINT          NOT NULL,
    CONSTRAINT [PK_REGLA] PRIMARY KEY CLUSTERED ([IdConstructorCriterio] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_REGLA_REFERENCE_CONSTRUC] FOREIGN KEY ([IdConstructorCriterio]) REFERENCES [dbo].[ConstructorCriterioDetalleAgrupacion] ([IdConstructorCriterio])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Borrado de la regla', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Regla', @level2type = N'COLUMN', @level2name = N'Borrado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Creación de la regla', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Regla', @level2type = N'COLUMN', @level2name = N'FechaCreacionRegla';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Valor del limete superior', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Regla', @level2type = N'COLUMN', @level2name = N'ValorLimiteSuperior';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Limite inferior de la regla', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Regla', @level2type = N'COLUMN', @level2name = N'ValorLimiteInferior';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Codigo de constructor de criterio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Regla', @level2type = N'COLUMN', @level2name = N'IdConstructorCriterio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Reglas definidas para el ultimo nivel de los arboles en el constructor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Regla';


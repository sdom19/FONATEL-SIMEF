CREATE TABLE [dbo].[ConstructorCriterio] (
    [IdConstructor] UNIQUEIDENTIFIER NOT NULL,
    [IdCriterio]    VARCHAR (50)     NOT NULL,
    [Ayuda]         VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_CONSTRUCTORCRITERIO] PRIMARY KEY CLUSTERED ([IdConstructor] ASC, [IdCriterio] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CONSTRUC_REFERENCE_CONSTRUC] FOREIGN KEY ([IdConstructor]) REFERENCES [dbo].[Constructor] ([IdConstructor]),
    CONSTRAINT [FK_CONSTRUC_REFERENCE_CRITERIO] FOREIGN KEY ([IdCriterio]) REFERENCES [dbo].[Criterio] ([IdCriterio])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Ayuda al usuario sobre el criterio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterio', @level2type = N'COLUMN', @level2name = N'Ayuda';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al Criterio', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterio', @level2type = N'COLUMN', @level2name = N'IdCriterio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Referencia al constructor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterio', @level2type = N'COLUMN', @level2name = N'IdConstructor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica los criterios asociados al constructor', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ConstructorCriterio';


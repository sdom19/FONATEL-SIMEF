CREATE TABLE [dbo].[Trimestre] (
    [IdTrimestre] INT           IDENTITY (1, 1) NOT NULL,
    [Nombre]      NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_TRIMESTRE] PRIMARY KEY CLUSTERED ([IdTrimestre] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'nombre del trimestre', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Trimestre', @level2type = N'COLUMN', @level2name = N'Nombre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del trimestre', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Trimestre', @level2type = N'COLUMN', @level2name = N'IdTrimestre';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Trimestres en que puede estar registrada la información', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Trimestre';


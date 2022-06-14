CREATE TABLE [dbo].[Operador] (
    [IdOperador]     VARCHAR (20)  NOT NULL,
    [NombreOperador] VARCHAR (150) NOT NULL,
    [Estado]         BIT           CONSTRAINT [DF_OPERADOR_ESTADO] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_OPERADOR_1] PRIMARY KEY CLUSTERED ([IdOperador] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Estado del operador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operador', @level2type = N'COLUMN', @level2name = N'Estado';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Nombre del operador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operador', @level2type = N'COLUMN', @level2name = N'NombreOperador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'id del operador', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operador', @level2type = N'COLUMN', @level2name = N'IdOperador';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Lista de operadores que equivalen a los que esten en la base de datos de Regulados', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Operador';


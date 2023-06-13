CREATE TABLE [dbo].[FormulaCalculoTipoFecha] (
    [IdFormulaCalculoTipoFecha] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre]                    VARCHAR (50) NOT NULL,
    [Estado]                    BIT          NOT NULL,
    CONSTRAINT [PK_FormulaCalculoTipoFecha] PRIMARY KEY CLUSTERED ([IdFormulaCalculoTipoFecha] ASC)
);


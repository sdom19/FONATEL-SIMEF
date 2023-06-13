CREATE TABLE [dbo].[FormulaCalculoUnidadMedida] (
    [IdFormulaCalculoUnidadMedida] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre]                       VARCHAR (20) NOT NULL,
    [Estado]                       BIT          NOT NULL,
    CONSTRAINT [PK_FormulaCalculoUnidadMedida] PRIMARY KEY CLUSTERED ([IdFormulaCalculoUnidadMedida] ASC)
);


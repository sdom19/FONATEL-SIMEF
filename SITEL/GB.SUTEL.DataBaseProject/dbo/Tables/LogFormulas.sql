CREATE TABLE [dbo].[LogFormulas] (
    [Idlog]                INT            IDENTITY (1, 1) NOT NULL,
    [Indicador]            VARCHAR (50)   NULL,
    [FechaModificacion]    DATETIME       NULL,
    [Usuario]              VARCHAR (20)   NULL,
    [FormulaAntiguaCumpli] VARCHAR (1000) NULL,
    [FormulaNuevaCumpli]   VARCHAR (1000) NULL,
    [FormulaAntiguaPor]    VARCHAR (1000) NULL,
    [FormulaNuevaPor]      VARCHAR (1000) NULL,
    [TipoAccion]           VARCHAR (4)    NULL,
    PRIMARY KEY CLUSTERED ([Idlog] ASC)
);


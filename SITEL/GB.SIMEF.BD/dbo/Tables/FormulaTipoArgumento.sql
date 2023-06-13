CREATE TABLE [dbo].[FormulaTipoArgumento] (
    [IdFormulaTipoArgumento] INT          IDENTITY (1, 1) NOT NULL,
    [Nombre]                 VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_FormulaTipoArgumento] PRIMARY KEY CLUSTERED ([IdFormulaTipoArgumento] ASC)
);


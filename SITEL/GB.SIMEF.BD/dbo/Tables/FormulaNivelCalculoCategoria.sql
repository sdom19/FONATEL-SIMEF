CREATE TABLE [dbo].[FormulaNivelCalculoCategoria] (
    [idFormulaNivel] INT NOT NULL,
    [idFormula]      INT NOT NULL,
    [idCategoria]    INT NOT NULL,
    CONSTRAINT [PK_FormulaNivelCalculoCategoria] PRIMARY KEY CLUSTERED ([idFormulaNivel] ASC),
    CONSTRAINT [FK_FormulaNivelCalculoCategoria_FormulasCalculo] FOREIGN KEY ([idFormula]) REFERENCES [dbo].[FormulasCalculo] ([idFormula])
);

